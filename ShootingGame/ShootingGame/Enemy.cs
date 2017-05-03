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
    class Enemy : Component, IUpdateable, ILoadable, IAnimateable
    {
        int speed;
        Vector2 translation;
        Animator animator;
        float timer;
        public int EnemyHealth { get; set; }

        public Enemy(GameObject gameObject) : base(gameObject)
        {
            speed = 100;
            EnemyHealth = 100;
        }

        public void Update()
        {
            Move();
        }

        public void Move()
        {
            //A reference to the current keyboard state
            KeyboardState keyState = Keyboard.GetState();

            //The current translation of the player
            //We are restting it to make sure that he stops moving if not keys are pressed
            translation = Vector2.Zero;

            //checks for input and adds it to the translation
            if (keyState.IsKeyDown(Keys.W))
            {
                translation += new Vector2(0, -1);
                animator.PlayAnimation("WalkBack");
            }
            else if (keyState.IsKeyDown(Keys.A))
            {
                translation += new Vector2(-1, 0);
                animator.PlayAnimation("WalkLeft");
            }
            else if (keyState.IsKeyDown(Keys.S))
            {
                translation += new Vector2(0, 1);
                animator.PlayAnimation("WalkFront");
            }
            else if (keyState.IsKeyDown(Keys.D))
            {
                translation += new Vector2(1, 0);
                animator.PlayAnimation("WalkRight");
            }
            else animator.PlayAnimation("Shoot");

            //Move the player's gameobject framerate independent
            GameObject.Transform.Translate(translation * speed * GameWorld.Instance.DeltaTime);
        }

        public void LoadContent(ContentManager content)
        {
            animator = (Animator)GameObject.GetComponent("Animator");
            CreateAnimation();
        }

        public void CreateAnimation()
        {
            animator.CreateAnimation("WalkBack", new Animation(1, 0, 8, 33, 60, 6, Vector2.Zero));
            animator.CreateAnimation("WalkLeft", new Animation(5, 140, 1, 50, 60, 10, Vector2.Zero));
            animator.CreateAnimation("WalkRight", new Animation(4, 140, 7, 53, 60, 10, Vector2.Zero));
            animator.CreateAnimation("WalkFront", new Animation(5, 0, 0, 40, 60, 10, Vector2.Zero));
            animator.CreateAnimation("Shoot", new Animation(3, 285, 10, 35, 60, 10, Vector2.Zero));
            animator.CreateAnimation("DieFront", new Animation(3, 920, 0, 150, 150, 5, Vector2.Zero));
            animator.PlayAnimation("Shoot");
        }

        public void OnAnimationDone(string animationName)
        {
            if (animationName.Contains("AttackRight"))
            {

            }
        }
    }
}
