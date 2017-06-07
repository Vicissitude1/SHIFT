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

        /// <summary>
        /// The Result of all three dies.
        /// </summary>
        private int result = DiceControl.Result;

        /// <summary>
        /// The number of the current dice
        /// </summary>
        private int currentDice;


        public Dice(GameObject gameObject) : base(gameObject)
        {
            //Keyboard state used for checking if the button is being pressed down.
            DiceControl.UpPressed = Keyboard.GetState();

            //The value of current die is a random number from 1 to 6 on each dice.
            currentDice = Roll();

            // The current dice number is added to the result
            DiceControl.Result += currentDice;
        }

        public void LoadContent(ContentManager content)
        {
            animator = (Animator)GameObject.GetComponent("Animator");
            CreateAnimation();
        }

        /// <summary>
        /// A method for rolling a six-sided die.
        /// </summary>
        /// <returns></returns>
        public int Roll()
        {
            int roll = DiceControl.R.Next(1, 7);

            return roll;
        }

        /// <summary>
        /// Creates different pictures for the dice.
        /// </summary>
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

        /// <summary>
        /// Updates the pictures of the dice to match their values.
        /// </summary>
        /// <param name="d"></param>
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

        /// <summary>
        /// Updates the dies' pictures until High or Low is guessed. As not to use CPU power on constantly
        /// updating their animations in 60 fps.
        /// </summary>
        public void Update()
        {
            if (DiceControl.HasPressed == false)
            {
                UpdateDice(currentDice);
            }
        }
    }
}
