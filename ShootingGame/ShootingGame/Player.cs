using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        bool playAnimation;
        public static int Health { get; set; }
        public Thread T { get; private set; }

        public Player(GameObject gameObject) : base(gameObject)
        {
            Health = 100;
            playAnimation = false;
            T = new Thread(Move);
            T.IsBackground = true;
        }

        public void Move()
        {
            while (true)
            {
                Thread.Sleep(100);
                GameObject.Transform.Position = new Vector2(Mouse.GetState().Position.X - 30, GameObject.Transform.Position.Y);
                if (Mouse.GetState().LeftButton == ButtonState.Pressed || playAnimation)
                {
                    animator.PlayAnimation("Shoot");
                    if (Explosion.PlayAnimation == false)
                        Explosion.PlayAnimation = true;
                }
                else animator.PlayAnimation("Idle");
            }
        }

        /// <summary>
        /// Loads the player's content
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            //Sets up a reference to the palyer's animator
            animator = (Animator)GameObject.GetComponent("Animator");

            //We can make our animations when we have a reference to the player's animator.
            CreateAnimation();
        }

        public void CreateAnimation()
        {
            /*
            animator.CreateAnimation("IdleBack", new Animation(1, 0, 8, 33, 60, 6, Vector2.Zero));
            animator.CreateAnimation("WalkLeft", new Animation(5, 218, 0, 45, 60, 10, Vector2.Zero));
            animator.CreateAnimation("WalkRight", new Animation(5, 65, 5, 53, 60, 10, Vector2.Zero));
            animator.CreateAnimation("DieFront", new Animation(3, 920, 0, 150, 150, 5, Vector2.Zero));
            animator.PlayAnimation("IdleBack");*/
            animator.CreateAnimation("Idle", new Animation(3, 0, 0, 60, 100, 0, Vector2.Zero));
            animator.CreateAnimation("Shoot", new Animation(6, 0, 0, 61, 100, 20, Vector2.Zero));
            animator.PlayAnimation("Idle");

        }

        public void OnAnimationDone(string animationName)
        {
            if (animationName.Contains("Shoot"))
            {
                playAnimation = false;
            }
        }

        public void OnCollisionStay(Collider other)
        {
            // (other.GameObject.GetComponent("SpriteRenderer") as SpriteRenderer).Color = Color.Red;
        }

        public void OnCollisionEnter(Collider other)
        {

        }

        public void OnCollisionExit(Collider other)
        {
            //(other.GameObject.GetComponent("SpriteRenderer") as SpriteRenderer).Color = Color.White;
        }
    }
}
