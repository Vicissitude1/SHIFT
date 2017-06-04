using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ShootingGame.Interfaces;

namespace ShootingGame
{
    public class DiceControl
    {
        /// <summary>
        /// The dice timer thread
        /// </summary>
        Thread diceTimerThread;

        /// <summary>
        /// Checks if player can roll the dice
        /// </summary>
        bool canRollDice;

        /// <summary>
        /// The dice roll timer
        /// </summary>
        int diceTimerCounter;

        public static KeyboardState UpPressed { get; set; }

        private KeyboardState downPressed;

        public static bool HasPressed { get; set; } = false;

        public static List<IDice> Dies { get; set; }

        public int DiceResult { get; set; }

        public static int Result { get; set; }

        public int CurrentDice { get; set; }

        public bool IsTesting { get; set; } = false;

        public int Reserve { get; set; }

        public int Current { get; set; }

        public static Random R { get; set; } = new Random();

        public DiceControl(List<IDice> dies)
        {
            Dies = dies;
            canRollDice = true;
            diceTimerCounter = 0;
            diceTimerThread = new Thread(Update);
            diceTimerThread.IsBackground = true;
            diceTimerThread.Start();
        }

        public void Update()
        {
            while (true)
            {
                // Checks if player rolls the dice
                if (!GameWorld.Instance.StopGame && canRollDice)
                    UpdateDiceUI();
                UpdateDiceTimer();
                Thread.Sleep(50);
            }
        }

        public void UpdateDiceUI()
        {
            KeyboardState k = Keyboard.GetState();
            if (k.IsKeyDown(Keys.Up))
            {
                if (!UpPressed.IsKeyDown(Keys.Up))
                {
                    High();
                    canRollDice = false;
                    diceTimerCounter = 130;
                }

            }
            else if (UpPressed.IsKeyDown(Keys.Up))
            {

            }
            UpPressed = k;

            if (k.IsKeyDown(Keys.Down))
            {
                if (!downPressed.IsKeyDown(Keys.Down))
                {
                    Low();
                    canRollDice = false;
                    diceTimerCounter = 130;
                }

            }
            else if (downPressed.IsKeyDown(Keys.Down))
            {

            }
            downPressed = k;
        }

        public void High()
        {
            HasPressed = true;
            Current = 0;
            Current = Result;
            Result = 0;
            foreach (IDice dice in Dies)
            {
                CurrentDice = dice.Roll();
                Result += CurrentDice;
                dice.UpdateDice(CurrentDice);
            }

            if (Current < Result)
            {
                Player.CurrentWeapon.TotalAmmo += Current + Reserve;
                if (Reserve > 0)
                {
                    Reserve = 0;
                }

            }
            if (Current > Result)
            {
                Reserve += Current;
            }
        }

        public void Low()
        {
            HasPressed = true;
            Current = 0;

            Current = Result;
            Result = 0;
            foreach (IDice dice in Dies)
            {
                CurrentDice = dice.Roll();
                Result += CurrentDice;
                dice.UpdateDice(CurrentDice);
            }

            if (Current > Result)
            {
                Player.CurrentWeapon.TotalAmmo += Current + Reserve;
                if (Reserve > 0)
                {
                    Reserve = 0;
                }
            }
            if (Current < Result)
            {
                Reserve += Current;
            }
        }

        /// <summary>
        /// Updates the dice timer, the timer starts every time, when player rolls the dice
        /// </summary>
        public void UpdateDiceTimer()
        {
            if (diceTimerCounter > 0) diceTimerCounter--;
            else if (!canRollDice) canRollDice = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(GameWorld.Instance.CFont, " = " + Result, new Vector2(950, 605), Color.DarkGoldenrod);
            spriteBatch.DrawString(GameWorld.Instance.BFont, "RESERV: " + Reserve, new Vector2(800, 650), Color.Black);
            spriteBatch.Draw(GameWorld.Instance.Pixel, new Rectangle(800, 640, diceTimerCounter, 5), Color.Blue);
        }
    }
}
