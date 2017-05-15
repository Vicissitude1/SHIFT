using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShootingGame
{
    class Player : Component, ILoadable, IAnimateable, ICollisionStay, ICollisionEnter, ICollisionExit
    {
        Animator animator;
        bool canShoot;
        bool canChangeWeapon;
        bool isChanged;
        int currentWeaponIndex;
        int selectedWeaponIndex;
        int speed;
        object thisLock = new object();
        Song gunCocking;
        Weapon[] weapons;
        public static bool PlayAnimation { get; set; }
        public static Weapon CurrentWeapon { get; private set; }
        public static int Health { get; set; }
        public Thread T { get; private set; }
        public static int Scores { get; set; }

        public Player(GameObject gameObject) : base(gameObject)
        {
            weapons = new Weapon[] { new Weapon("GUN", 7, 20, 1500, ShootType.Gun),
                                      new Weapon("RIFLE", 15, 50, 2000, ShootType.Rifle),
                                      new Weapon("MACHINEGUN", 30, 35, 2000, ShootType.MachineGun)};
            Health = 100;
            PlayAnimation = false;
            T = new Thread(Move);
            T.IsBackground = true;
            Scores = 0;
            canShoot = true;
            canChangeWeapon = true;
            currentWeaponIndex = 0;
            speed = 5;
            isChanged = false;
            CurrentWeapon = weapons[currentWeaponIndex];
        }


        /// <summary>
        /// Loads the player's content
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            foreach (Weapon w in weapons)
            {
                w.LoadContent(content);
            }
            gunCocking = content.Load<Song>("gun-cocking-02");
            //Sets up a reference to the palyer's animator
            animator = (Animator)GameObject.GetComponent("Animator");

            //We can make our animations when we have a reference to the player's animator.
            CreateAnimation();
        }

        public void Move()
        {
            while (true)
            {
                Thread.Sleep(20);
                if (canChangeWeapon)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.D1) && canChangeWeapon)
                    {
                        selectedWeaponIndex = 0;
                        if (currentWeaponIndex != selectedWeaponIndex)
                        {
                            canChangeWeapon = false;
                            MediaPlayer.Play(gunCocking);
                        }
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.D2) && canChangeWeapon)
                    {
                        selectedWeaponIndex = 1;
                        if (currentWeaponIndex != selectedWeaponIndex)
                        {
                            canChangeWeapon = false;
                            MediaPlayer.Play(gunCocking);
                        }
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.D3) && canChangeWeapon)
                    {
                        selectedWeaponIndex = 2;
                        if (currentWeaponIndex != selectedWeaponIndex)
                        {
                            canChangeWeapon = false;
                            MediaPlayer.Play(gunCocking);
                        }
                    }
                    /*
                    else if (Mouse.GetState().LeftButton == ButtonState.Pressed && canChangeWeapon)
                    {
                        if(canShoot && !PlayAnimation)
                        {
                            CurrentWeapon.Shoot();
                        }
                        if (Explosion.PlayAnimation == false)
                            Explosion.PlayAnimation = true;
                        canShoot = false;
                    }
                    else if (Mouse.GetState().LeftButton == ButtonState.Released)
                    {
                        canShoot = true;
                    }*/
                    CurrentWeapon.UpdateWeaponStatus();
                    if (PlayAnimation && currentWeaponIndex==0) animator.PlayAnimation("GunShoot");
                    else if (PlayAnimation && currentWeaponIndex == 1) animator.PlayAnimation("RifleShoot");
                    else if (PlayAnimation && currentWeaponIndex == 2) animator.PlayAnimation("MachineGunShoot");
                    else if(currentWeaponIndex==0) animator.PlayAnimation("GunIdle");
                    else if (currentWeaponIndex == 1) animator.PlayAnimation("RifleIdle");
                    else if (currentWeaponIndex == 2) animator.PlayAnimation("MachineGunIdle");
                }
                else ChangeWeapon();

                GameObject.Transform.Position = new Vector2(Mouse.GetState().Position.X - 30, GameObject.Transform.Position.Y);
            }
        }

        public void CreateAnimation()
        {
            animator.CreateAnimation("GunIdle", new Animation(1, 20, 0, 60, 90, 1, Vector2.Zero));
            animator.CreateAnimation("GunShoot", new Animation(6, 20, 1, 62, 90, 20, Vector2.Zero));
            animator.CreateAnimation("RifleIdle", new Animation(1, 135, 0, 80, 70, 1, Vector2.Zero));
            animator.CreateAnimation("RifleShoot", new Animation(3, 135, 0, 80, 70, 20, Vector2.Zero));
            animator.CreateAnimation("MachineGunIdle", new Animation(1, 225, 0, 90, 110, 1, Vector2.Zero));
            animator.CreateAnimation("MachineGunShoot", new Animation(3, 225, 1, 96, 110, 20, Vector2.Zero));
            animator.PlayAnimation("GunIdle");

        }

        public void OnAnimationDone(string animationName)
        {
            if (animationName.Contains("GunShoot") || animationName.Contains("RifleShoot"))
            {
                PlayAnimation = false;
            }
        }

        public void OnCollisionStay(Collider other)
        {
            // (other.GameObject.GetComponent("SpriteRenderer") as SpriteRenderer).Color = Color.Red;
        }

        public void OnCollisionEnter(Collider other)
        {
            if (other.GameObject.GetComponent("EnemyBullet") is EnemyBullet)
            {
                lock (thisLock)
                {
                    Health--;
                    if (Health < 0) Health = 0;
                }
                (other.GameObject.GetComponent("EnemyBullet") as EnemyBullet).IsRealesed = true;
            }
        }

        public void OnCollisionExit(Collider other)
        {
            //(other.GameObject.GetComponent("SpriteRenderer") as SpriteRenderer).Color = Color.White;
        }

        public void ChangeWeapon()
        {
            if(canShoot) canShoot = false;
            if (PlayAnimation) PlayAnimation = false;

            Vector2 translation = Vector2.Zero;

            if (isChanged)
            {
                if(GameObject.Transform.Position.Y <= 500)
                {
                    isChanged = false;
                    canChangeWeapon = true;
                    canShoot = true;
                }
                else
                {
                    translation += new Vector2(0, -1);
                    GameObject.Transform.Position += translation * speed;
                }
            }
            else
            {
                if(GameObject.Transform.Position.Y > 600)
                {
                    currentWeaponIndex = selectedWeaponIndex;
                    CurrentWeapon = weapons[currentWeaponIndex];
                    isChanged = true;
                    if (currentWeaponIndex == 0) animator.PlayAnimation("GunIdle");
                    else if (currentWeaponIndex == 1) animator.PlayAnimation("RifleIdle");
                    else if (currentWeaponIndex == 2) animator.PlayAnimation("MachineGunIdle");
                }
                else
                {
                    translation += new Vector2(0, 1);
                    GameObject.Transform.Position += translation * speed;
                }
            }
        }
    }
}
