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

        public Player(GameObject gameObject) : base(gameObject)
        {
            
        }

        public void Update()
        {
            Move();
        }

        public void Move()
        {
            translation = Vector2.Zero;
            keyState = Keyboard.GetState();

        }

        public void LoadContent(ContentManager content)
        {
            this.animator = (Animator)GameObject.GetComponent("Animator");
            CreateAnimation();
        }

        public void CreateAnimation()
        {
            animator.CreateAnimation("Walk", new Animation(4, 400, 0, 125, 90, 3, Vector2.Zero));
            animator.CreateAnimation("FlyRight", new Animation(4, 285, 0, 124, 80, 9, Vector2.Zero));
            animator.CreateAnimation("FlyLeft", new Animation(4, 165, 0, 123, 75, 8, Vector2.Zero));
            animator.CreateAnimation("Fall", new Animation(4, 5, 1, 120, 115, 0, Vector2.Zero));
            animator.CreateAnimation("Run", new Animation(4, 400, 0, 125, 90, 8, Vector2.Zero));
            animator.PlayAnimation("Walk");
        }

        public void OnAnimationDone(string animationName)
        {
            if (animationName.Contains("AttackRight"))
            {
                //canAttack = false;
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
