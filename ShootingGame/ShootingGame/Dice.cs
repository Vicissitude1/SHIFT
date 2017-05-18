using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Threading;
using Microsoft.Xna.Framework.Input;

namespace ShootingGame
{
    class Dice : Component, ILoadable, IUpdateable
    {
        private Animator animator;
        

        public int Ammo { get; set; }
        private int result = GameWorld.Instance.Result;
        public Thread T { get; set; }

        public Dice(GameObject gameObject) : base(gameObject)
        {
            GameWorld.Instance.UpPressed = Keyboard.GetState();
            GameWorld.Instance.Result += GameWorld.Instance.RollDices();
            T = new Thread(Update);
            T.IsBackground = true;
        }

        public void LoadContent(ContentManager content)
        {
            animator = (Animator)GameObject.GetComponent("Animator");
            CreateAnimation();
        }

        public void CreateAnimation()
        {
            animator.CreateAnimation("ShowOne", new Animation(1, 0, 0, 64, 64, 1, Vector2.Zero));
            animator.CreateAnimation("ShowTwo", new Animation(1, 0, 1, 64, 64, 1, Vector2.Zero));
            animator.CreateAnimation("ShowThree", new Animation(1, 0, 2, 64, 64, 1, Vector2.Zero));
            animator.CreateAnimation("ShowFour", new Animation(1, 0, 3, 64, 64, 1, Vector2.Zero));
            animator.CreateAnimation("ShowFive", new Animation(1, 0, 4, 64, 64, 1, Vector2.Zero));
            animator.CreateAnimation("ShowSix", new Animation(1, 0, 5, 64, 64, 1, Vector2.Zero));
            animator.PlayAnimation("ShowSix");
        }

        public void UpdateDice(int d)
        {
            switch (d)
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


        public void Update()
        {
            
            
        }
    }
}
