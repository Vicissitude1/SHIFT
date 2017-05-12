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
        private int dice;
        private bool isPressed = false;
        private KeyboardState upPressed;
        private KeyboardState downPressed;

        public int Ammo { get; set; }
        private int reserve;
        private int result = GameWorld.Instance.Result;
        public Thread T { get; set; }

        public Dice(GameObject gameObject) : base(gameObject)
        {
            upPressed = Keyboard.GetState();
            result += RollDices();
            T = new Thread(Update);
            T.IsBackground = true;
        }


        public int RollDices()
        {

            dice = GameWorld.Instance.Rnd.Next(1, 7);
            result += dice;

            return dice;
        }

        public void High()
        {
            int current;

            current = dice;
            RollDices();
            if (current > dice)
            {
                Ammo += current + reserve;
                if (reserve > 0)
                {
                    reserve = 0;
                }
            }
            if (current < dice)
            {
                reserve += current;
            }
        }

        public void Low()
        {
            int current;

            current = dice;
            RollDices();
            if (current < dice)
            {
                Ammo += current + reserve;
                if (reserve > 0)
                {
                    reserve = 0;
                }
            }
            if (current > dice)
            {
                reserve += current;
            }
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


        public void Update()
        {
            KeyboardState k = Keyboard.GetState();
            if (k.IsKeyDown(Keys.Up))
            {
                if (!upPressed.IsKeyDown(Keys.Up))
                {
                    High();
                }

            }
            else if (upPressed.IsKeyDown(Keys.Up))
            {

            }
            upPressed = k;

            if (k.IsKeyDown(Keys.Down))
            {
                if (!downPressed.IsKeyDown(Keys.Down))
                {
                    Low();
                }

            }
            else if (downPressed.IsKeyDown(Keys.Down))
            {

            }
            downPressed = k;

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
