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
using ShootingGame.Interfaces;

namespace ShootingGame
{
    public class Dice : Component, ILoadable, IUpdateable, IDice
    {
        private Animator animator;
        private bool isTesting = false;
        public int Ammo { get; set; }
        private int result = DiceControl.Result;
        private int currentDice;

        public Dice(GameObject gameObject) : base(gameObject)
        {
            DiceControl.UpPressed = Keyboard.GetState();
            currentDice = Roll();
            DiceControl.Result += currentDice;
        }

        public void LoadContent(ContentManager content)
        {
            animator = (Animator)GameObject.GetComponent("Animator");
            CreateAnimation();
        }

        public int Roll()
        {
            int roll = GameWorld.Instance.Rnd.Next(1, 7);
            
            return roll;
        }

        public void CreateAnimation()
        {
            animator.CreateAnimation("ShowOne", new Animation(1, 0, 0, 46, 46, 1, Vector2.Zero));
            animator.CreateAnimation("ShowTwo", new Animation(1, 0, 1, 46, 46, 1, Vector2.Zero));
            animator.CreateAnimation("ShowThree", new Animation(1, 0, 2, 46, 46, 1, Vector2.Zero));
            animator.CreateAnimation("ShowFour", new Animation(1, 46, 0, 46, 46, 1, Vector2.Zero));
            animator.CreateAnimation("ShowFive", new Animation(1, 46, 1, 46, 46, 1, Vector2.Zero));
            animator.CreateAnimation("ShowSix", new Animation(1, 46, 2, 46, 46, 1, Vector2.Zero));
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
            if (DiceControl.HasPressed == false)
            {
                UpdateDice(currentDice);
            }
        }
    }
}
