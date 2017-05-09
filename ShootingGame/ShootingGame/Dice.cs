using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Threading;

namespace ShootingGame
{
    class Dice : Component, ILoadable, IUpdateable
    {
        private Animator animator;
        private int dice;
        public Thread T { get; set; }

        public Dice(GameObject gameObject) : base(gameObject)
        {
            Roll();
            T = new Thread(Update);
            T.IsBackground = true;
        }

        public int Roll()
        {
            dice = GameWorld.Instance.Rnd.Next(0, 7);
            return dice;
        }

        public void LoadContent(ContentManager content)
        {
            animator = (Animator)GameObject.GetComponent("Animator");
            CreateAnimation();
        }

        public void CreateAnimation()
        {
            animator.CreateAnimation("ShowOne", new Animation(1, 0, 0, 64, 64, 0, Vector2.Zero));
            animator.CreateAnimation("ShowTwo", new Animation(1, 0, 64, 64, 64, 0, Vector2.Zero));
            animator.CreateAnimation("ShowThree", new Animation(1, 0, 128, 64, 64, 0, Vector2.Zero));
            animator.CreateAnimation("ShowFour", new Animation(1, 0, 192, 64, 64, 0, Vector2.Zero));
            animator.CreateAnimation("ShowFive", new Animation(1, 0, 254, 64, 64, 0, Vector2.Zero));
            animator.CreateAnimation("ShowSix", new Animation(1, 0, 318, 64, 64, 0, Vector2.Zero));
            animator.PlayAnimation("ShowOne");
        }
        

        public void Update()
        {
            switch (dice)
            {
                case 1:
                    animator.PlayAnimation("ShowOne");
                    break;
                case 2:
                    animator.PlayAnimation("ShowTwo");
                    break;
                case 3:
                    animator.PlayAnimation("ShowThree");
                    break;
                case 4:
                    animator.PlayAnimation("ShowFour");
                    break;
                case 5:
                    animator.PlayAnimation("ShowFive");
                    break;
                case 6:
                    animator.PlayAnimation("ShowSix");
                    break;
            }
        }
    }
}
