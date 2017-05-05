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
    class Explosion : Component, ILoadable, IAnimateable
    {
        Animator animator;
        Vector2 mouseCurrentPosition;
        public static bool PlayAnimation { get; set; }
        public Thread T { get; private set; }

        public Explosion(GameObject gameObject) : base(gameObject)
        {
            PlayAnimation = false;
            T = new Thread(Move);
            T.IsBackground = true;
        }

        public void Move()
        {
            while (true)
            {
                Thread.Sleep(100);

                if(Mouse.GetState().Position.Y <=450)
                mouseCurrentPosition = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);
                else mouseCurrentPosition = new Vector2(Mouse.GetState().Position.X, 450);

                GameObject.Transform.Position = new Vector2(mouseCurrentPosition.X - 64, mouseCurrentPosition.Y - 38);
                if (PlayAnimation && mouseCurrentPosition.Y <= 450)
                {
                    animator.PlayAnimation("Explode");
                }
                else animator.PlayAnimation("Idle");
            }
        }

        public void LoadContent(ContentManager content)
        {
            this.animator = (Animator)GameObject.GetComponent("Animator");
            CreateAnimation();
        }

        public void CreateAnimation()
        {
            //animator.CreateAnimation("Explode", new Animation(7, 20, 0, 74, 40, 10, Vector2.Zero));
            animator.CreateAnimation("Explode", new Animation(3, 25, 0, 128, 65, 15, Vector2.Zero));
            animator.CreateAnimation("Idle", new Animation(1, 0, 0, 10, 110, 0, Vector2.Zero));
            animator.PlayAnimation("Idle");
        }

        public void OnAnimationDone(string animationName)
        {
            if (animationName.Contains("Explode"))
            {
                PlayAnimation = false;
            }
        }
    }
}
