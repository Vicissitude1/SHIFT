using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShootingGame
{
    /// <summary>
    /// Represents the PlayerBullet
    /// </summary>
    class PlayerBullet : Component, ILoadable, IAnimateable
    {
        /// <summary>
        /// The PlayerBullet's movement speed
        /// </summary>
        int speed;

        /// <summary>
        /// Checks if animation is done
        /// </summary>
        bool animationDone;

        /// <summary>
        /// The PlayerBullet's translation
        /// </summary>
        Vector2 translation;

        /// <summary>
        /// The reference to the PlayerBullet's animator
        /// </summary>
        Animator animator;

        /// <summary>
        /// The PlayerBullet's aim position
        /// </summary>
        public Vector2 AimPosition { get; private set; }

        /// <summary>
        /// The PlayerBullet's damage level
        /// </summary>
        public int DamageLevel { get; private set; }

        /// <summary>
        /// The PlayerBullet's thread
        /// </summary>
        public Thread T { get; set; }

        /// <summary>
        /// Checks if the PlayerBullet has to be deleted
        /// </summary>
        public bool IsRealesed { get; set; }


        /// <summary>
        /// The PlayerBullet's constructor
        /// </summary>
        /// <param name="gameObject"></param>
        public PlayerBullet(GameObject gameObject) : base(gameObject)
        {
            speed = 12;
            DamageLevel = Player.CurrentWeapon.DamageLevel;
            IsRealesed = false;
            animationDone = false;
            AimPosition = new Vector2(Mouse.GetState().Position.X - 8, Mouse.GetState().Position.Y + 10);
            T = new Thread(Update);
            T.IsBackground = true;
            T.Start();
        }

        /// <summary>
        /// Updates the PlayerBullet functionality
        /// </summary>
        public void Update()
        {
            while (true)
                Move();
        }

        /// <summary>
        /// The PlayerBullet's movement
        /// </summary>
        public void Move()
        {
            // The PlayerBullet has to be deleted from the game, when the explosion animation is done
            if (animationDone || GameWorld.Instance.StopGame || !GameWorld.Instance.PlayGame) T.Abort();

            Thread.Sleep(30);

            // Changes the sprite's size according to the position
            (GameObject.GetComponent("SpriteRenderer") as SpriteRenderer).Scale = 1.2f - 400 / GameObject.Transform.Position.Y / 3;

            // Moves the PlayerBullet up
            translation = Vector2.Zero;
            translation += new Vector2(0, -1);
            GameObject.Transform.Position += translation * speed;

            // Makes sure that the Playerbullet has to be deleted when the game is finished
            if (GameWorld.Instance.StopGame)
            {
                if (GameObject.Transform.Position.Y < 120 || GameObject.Transform.Position.Y <= AimPosition.Y)
                {
                    speed = 0;
                    animationDone = true;
                }
            }

            // Stops the movement and starts to play explosion animation when the bullet reached to the aim
            else if (IsRealesed)
            {
                speed = GameObject.Transform.Position.Y < 120 ? 0 : 1;
                //speed = 0;
                animator.PlayAnimation("Expl");
            }
            
            // Checks if the PlayerBullet reached to the aim
            else if (GameObject.Transform.Position.Y < 120 || GameObject.Transform.Position.Y < AimPosition.Y)
            {
                IsRealesed = true;
            }
        }

        /// <summary>
        /// Loads the PlayerBullet's content
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            animator = (Animator)GameObject.GetComponent("Animator");
            CreateAnimation();
        }

        /// <summary>
        /// Creats the PlayerBullet's animation
        /// </summary>
        public void CreateAnimation()
        {
            animator.CreateAnimation("Idle", new Animation(1, 0, 0, 10, 10, 1, Vector2.Zero));
            animator.CreateAnimation("Expl", new Animation(3, 19, 0, 18, 18, 20, Vector2.Zero));
            animator.PlayAnimation("Idle");
        }

        /// <summary>
        /// Makes sure the PlayerBullet's explosion animation is done
        /// </summary>
        /// <param name="animationName"></param>
        public void OnAnimationDone(string animationName)
        {
            if (animationName.Contains("Expl"))
                animationDone = true;
        }
    }
}
