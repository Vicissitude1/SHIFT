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
    /// <summary>
    /// Represents the Player
    /// </summary>
    class Player : Component, ILoadable, IAnimateable, ICollisionEnter
    {
        /// <summary>
        /// The Player's health
        /// </summary>
        static int health;

        /// <summary>
        /// The reference to the player's animator
        /// </summary>
        Animator animator;

        /// <summary>
        /// Checks if player can shoot
        /// </summary>
        bool canShoot;

        /// <summary>
        /// Checks if it is possible to change weapon
        /// </summary>
        bool canChangeWeapon;

        /// <summary>
        /// Checks if weapon is changed
        /// </summary>
        bool isChanged;

        /// <summary>
        /// The current weapon index
        /// </summary>
        int currentWeaponIndex;

        /// <summary>
        /// The selected by player new weapon index
        /// </summary>
        int selectedWeaponIndex;

        /// <summary>
        /// The Player's movement speed
        /// </summary>
        int speed;

        /// <summary>
        /// The object that is going to be locked
        /// </summary>
        static object thisLock = new object();

        /// <summary>
        /// The weapon array
        /// </summary>
        Weapon[] weapons;

        /// <summary>
        /// The SoundEffect's instance to play sound
        /// </summary>
        SoundEffect effect;

        /// <summary>
        /// The Plyer's scores
        /// </summary>
        public static int Scores { get; set; }

        /// <summary>
        /// Checks if Player can start shoot
        /// </summary>
        public static bool CanStartShoot { get; set; }

        /// <summary>
        /// Checks if necessary to play animation
        /// </summary>
        public static bool PlayAnimation { get; set; }

        /// <summary>
        /// The current Player's weapon
        /// </summary>
        public static Weapon CurrentWeapon { get; private set; }

        /// <summary>
        /// The Player's thread
        /// </summary>
        public Thread T { get; private set; }

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

        /// <summary>
        /// The Player's constructor
        /// </summary>
        /// <param name="gameObject"></param>
        public Player(GameObject gameObject) : base(gameObject)
        {
            // gets the array with weapons from data base
            weapons = DataBaseClass.Instance.GetWeapons();
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
            // Loads content for all weapons
            foreach (Weapon w in weapons)
            {
                w.LoadContent(content);
            }
            // Loads the content for sound effect
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
                    Move(); //Makes sure that the player's move function is called
            }
        }

        public void Move()
        {
            if (canChangeWeapon)
            {
                // Checks if gun is selected (button 1 is pressed)
                if (Keyboard.GetState().IsKeyDown(Keys.D1))
                {
                    selectedWeaponIndex = 0;
                    // Makes possible to change weapon only if it is not selected already
                    if (currentWeaponIndex != selectedWeaponIndex)
                    {
                        canChangeWeapon = false;
                        effect.Play();
                        PlayAnimation = false;
                    }
                }
                // Checks if rifle is selected (button 2 is preassed)
                else if (Keyboard.GetState().IsKeyDown(Keys.D2))
                {
                    selectedWeaponIndex = 1;
                    // Makes possible to change weapon only if it is not selected already
                    if (currentWeaponIndex != selectedWeaponIndex)
                    {
                        canChangeWeapon = false;
                        effect.Play();
                        PlayAnimation = false;
                    }
                }
                // Checks if machinegun is selected (button 3 is preassed) 
                else if (Keyboard.GetState().IsKeyDown(Keys.D3))
                {
                    selectedWeaponIndex = 2;
                    // Makes possible to change weapon only if it is not selected already
                    if (currentWeaponIndex != selectedWeaponIndex)
                    {
                        canChangeWeapon = false;
                        effect.Play();
                        PlayAnimation = false;
                    }
                }
                // Checks if player is shooting
                if (CanStartShoot)
                CurrentWeapon.UpdateWeaponStatus();
                // Plays an animation, corresponding to the status of the selected weapon 
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
            //Moves the player's gameobject
            GameObject.Transform.Position = new Vector2(Mouse.GetState().Position.X - 30, GameObject.Transform.Position.Y);
        }

        /// <summary>
        /// Resets all the fields when game is restarted
        /// </summary>
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

        /// <summary>
        /// Creates all the player's animations
        /// </summary>
        public void CreateAnimation()
        {
            animator.CreateAnimation("GunIdle", new Animation(1, 20, 0, 60, 90, 1, Vector2.Zero));
            animator.CreateAnimation("GunShoot", new Animation(6, 20, 1, 62, 90, 30, Vector2.Zero));
            animator.CreateAnimation("RifleIdle", new Animation(1, 132, 0, 67, 70, 1, Vector2.Zero));
            animator.CreateAnimation("RifleShoot", new Animation(3, 132, 0, 67, 70, 20, Vector2.Zero));
            animator.CreateAnimation("MachineGunIdle", new Animation(1, 225, 0, 90, 110, 1, Vector2.Zero));
            animator.CreateAnimation("MachineGunShoot", new Animation(3, 225, 1, 96, 110, 20, Vector2.Zero));
            //Plays an aniamtion to make sure that we have an animation to play
            animator.PlayAnimation("GunIdle");
        }

        public void OnAnimationDone(string animationName)
        {
            if (animationName.Contains("GunShoot") || animationName.Contains("RifleShoot"))
            {
                PlayAnimation = false;
            }
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
                }
                (other.GameObject.GetComponent("EnemyBullet") as EnemyBullet).IsRealesed = true;
            }
        }

        /// <summary>
        /// Performs weapon change
        /// </summary>
        public void ChangeWeapon()
        {
            // Makes sure that player cannot shoot
            if(canShoot) canShoot = false;
            // Makes sure that animation will not play
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
