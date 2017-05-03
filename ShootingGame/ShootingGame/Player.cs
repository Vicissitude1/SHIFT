using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    class Player : Component, IUpdateable, ILoadable, IAnimateable, ICollisionStay, ICollisionEnter, ICollisionExit, IDrawable
    {
        Vector2 translation;
        Animator animator;
        KeyboardState keyState;
        float speed;
        bool playAnimation;

        public Player(GameObject gameObject) : base(gameObject)
        {
            speed = 100;
            playAnimation = false;
        }

        public void Move()
        {
            translation = Vector2.Zero;
            keyState = Keyboard.GetState();
            /*
            if (keyState.IsKeyDown(Keys.A))
            {
                translation += new Vector2(-1, 0);
                animator.PlayAnimation("WalkLeft");
            }
            else if (keyState.IsKeyDown(Keys.D))
            {
                translation += new Vector2(1, 0);
                animator.PlayAnimation("WalkRight");
            }
            else animator.PlayAnimation("IdleBack");*/
         
            if (Mouse.GetState().LeftButton == ButtonState.Pressed || playAnimation)
            {
                animator.PlayAnimation("Shoot");
                if(Explosion.PlayAnimation == false)
                Explosion.PlayAnimation = true;
            }
            else animator.PlayAnimation("Idle");

            GameObject.Transform.Position = new Vector2(Mouse.GetState().Position.X, GameObject.Transform.Position.Y);

            GameObject.Transform.Translate(translation * speed * GameWorld.Instance.DeltaTime);
        }

        public void Update()
        {
            //Makes sure that the player's move function is called
            Move();
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
        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.DrawString(GameWorld.Instance.AFont, "Score: " + score, new Vector2(10, 100), Color.LightGreen);
            //spriteBatch.DrawString(GameWorld.Instance.AFont, "Lifes left: " + lifesAmount, new Vector2(10, 130), Color.AliceBlue);
        }
    }
}
