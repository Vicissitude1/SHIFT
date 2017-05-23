using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        static int health;
        Animator animator;
        bool canShoot;
        bool canChangeWeapon;
        bool isChanged;
        int currentWeaponIndex;
        int selectedWeaponIndex;
        int speed;
        static object thisLock = new object();
        Weapon[] weapons;
        DataBaseClass database;
        SoundEffect effect;
        public static bool CanStartShoot { get; set; }
        public static bool PlayAnimation { get; set; }
        public static Weapon CurrentWeapon { get; private set; }
        public Thread T { get; private set; }
        public static int Scores { get; set; }

        public static int Health
        {
            get { return health;}
            set
            {
                lock (thisLock)
                {
                    health = value;
                    if (health < 0) health = 0;
                    else if (health > 100) health = 100;
                }
            }
        }

        public Player(GameObject gameObject) : base(gameObject)
        {
            database = new DataBaseClass();
            weapons = database.GetWeapons();
            /*
            weapons = new Weapon[] { new Weapon("GUN", 7, 20, 1000, WeaponType.BoltAction),
                                      new Weapon("RIFLE", 15, 50, 1500, WeaponType.SemiAuto),
                                      new Weapon("MACHINEGUN", 30, 35, 1500, WeaponType.FullAuto)};*/
            Health = 100;
            PlayAnimation = false;
            T = new Thread(Update);
            T.IsBackground = true;
            Scores = 0;
            canShoot = true;
            canChangeWeapon = true;
            currentWeaponIndex = 0;
            speed = 5;
            isChanged = false;
            CurrentWeapon = weapons[currentWeaponIndex];
            CanStartShoot = true;
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
            effect = content.Load<SoundEffect>("gun-cocking-02");
            //Sets up a reference to the palyer's animator
            animator = (Animator)GameObject.GetComponent("Animator");
            //We can make our animations when we have a reference to the player's animator.
            CreateAnimation();
        }

        public void Update()
        {
            while(true)
            {
                Thread.Sleep(20);
                if (GameWorld.Instance.PlayGame && !GameWorld.Instance.StopGame)
                    Move();
            }
        }

        public void Move()
        {
            if (canChangeWeapon)
            {

                if (Keyboard.GetState().IsKeyDown(Keys.D1))
                {
                    selectedWeaponIndex = 0;
                    if (currentWeaponIndex != selectedWeaponIndex)
                    {
                        canChangeWeapon = false;
                        effect.Play();
                        //GameWorld.Instance.Engine.Play2D("Content/gun-cocking-02.wav", false);
                        PlayAnimation = false;
                    }
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.D2))
                {
                    selectedWeaponIndex = 1;
                    if (currentWeaponIndex != selectedWeaponIndex)
                    {
                        canChangeWeapon = false;
                        effect.Play();
                        //GameWorld.Instance.Engine.Play2D("Content/gun-cocking-02.wav", false);
                        PlayAnimation = false;
                    }
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.D3))
                {
                    selectedWeaponIndex = 2;
                    if (currentWeaponIndex != selectedWeaponIndex)
                    {
                        canChangeWeapon = false;
                        effect.Play();
                        //GameWorld.Instance.Engine.Play2D("Content/gun-cocking-02.wav", false);
                        PlayAnimation = false;
                    }
                }
  
                if (CanStartShoot)
                CurrentWeapon.UpdateWeaponStatus();

                if (currentWeaponIndex == 0)
                {
                    if (PlayAnimation) animator.PlayAnimation("GunShoot");
                    else animator.PlayAnimation("GunIdle");
                }
                else if (currentWeaponIndex == 1)
                {
                    if (PlayAnimation) animator.PlayAnimation("RifleShoot");
                    else animator.PlayAnimation("RifleIdle");
                }
                else if (currentWeaponIndex == 2)
                {
                    if (PlayAnimation) animator.PlayAnimation("MachineGunShoot");
                    else animator.PlayAnimation("MachineGunIdle");
                }
                
            }
            else ChangeWeapon();

            GameObject.Transform.Position = new Vector2(Mouse.GetState().Position.X - 30, GameObject.Transform.Position.Y);
        }

        public void Replace()
        {
            Health = 100;
            PlayAnimation = false;
            Scores = 0;
            canShoot = true;
            canChangeWeapon = true;
            currentWeaponIndex = 0;
            speed = 5;
            isChanged = false;
            foreach (Weapon w in weapons)
                w.RestartWeapon();
            CurrentWeapon = weapons[currentWeaponIndex];
        }

        public void CreateAnimation()
        {
            animator.CreateAnimation("GunIdle", new Animation(1, 20, 0, 60, 90, 1, Vector2.Zero));
            animator.CreateAnimation("GunShoot", new Animation(6, 20, 1, 62, 90, 30, Vector2.Zero));
            animator.CreateAnimation("RifleIdle", new Animation(1, 132, 0, 67, 70, 1, Vector2.Zero));
            animator.CreateAnimation("RifleShoot", new Animation(3, 132, 0, 67, 70, 20, Vector2.Zero));
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
                    if (health == 0)
                    {
                        GameWorld.Instance.StopGame = true;
                        CanStartShoot = false;
                    }
                    /*
                    if (Health < 0)
                    {
                        Health = 0;
                        GameWorld.Instance.StopGame = true;
                        CanStartShoot = false;
                    } */
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
