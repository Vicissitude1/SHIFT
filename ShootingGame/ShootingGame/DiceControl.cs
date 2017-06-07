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
        int diceTimer;

        /// <summary>
        /// Keyboard State for guessing High
        /// </summary>
        public static KeyboardState UpPressed { get; set; }

        /// <summary>
        /// A Keyboard State for guessing Low
        /// </summary>
        private KeyboardState downPressed;

        /// <summary>
        /// Boolean for checking if High or Low is guessed once.
        /// </summary>
        public static bool HasPressed { get; set; } = false;

        /// <summary>
        /// List of dies. Can be reworked for tests.
        /// </summary>
        public static List<IDice> Dies { get; set; }

        /// <summary>
        /// Result of the three dice.
        /// </summary>
        public static int Result { get; set; }

        /// <summary>
        /// The current die's number
        /// </summary>
        public int CurrentDice { get; set; }

        /// <summary>
        /// The Reserve Ammo property
        /// </summary>
        public int Reserve { get; set; }

        /// <summary>
        /// The Current Result value
        /// </summary>
        public int Current { get; set; }

        public static Random R { get; set; } = new Random();

        public DiceControl(List<IDice> dies)
        {
            Dies = dies;
            canRollDice = true;
            diceTimer = 0;
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

        /// <summary>
        /// A method for updating the dice in the User Interface by User Input. Calls High or Low.
        /// </summary>
        public void UpdateDiceUI()
        {
            KeyboardState k = Keyboard.GetState();
            if (k.IsKeyDown(Keys.Up))
            {
                if (!UpPressed.IsKeyDown(Keys.Up))
                {
                    High();
                    canRollDice = false;
                    diceTimer = 130;
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
                    diceTimer = 130;
                }

            }
            else if (downPressed.IsKeyDown(Keys.Down))
            {

            }
            downPressed = k;
        }

        /// <summary>
        /// Rolls a new set of dice and if the result is higher than the previous result,
        /// the ammo is added to the player. Otherwise, it's added to Reserve Ammo.
        /// </summary>
        public void High()
        {
            HasPressed = true;
            Current = 0;
            Current = Result;
            Result = 0;

            //All the dice's results get added together and the UI is updated.
            foreach (IDice dice in Dies)
            {
                CurrentDice = dice.Roll();
                Result += CurrentDice;
                dice.UpdateDice(CurrentDice);
            }

            //Ammo is added and reserve is emptied
            if (Current <= Result)
            {
                Player.CurrentWeapon.TotalAmmo += Current + Reserve;
                if (Reserve > 0)
                {
                    Reserve = 0;
                }

            }

            //Ammo is added to Reserve
            if (Current > Result)
            {
                Reserve += Current;
            }
        }

        /// <summary>
        /// Rolls a new set of dice and if the result is lower than the previous result,
        /// the ammo is added to the player. Otherwise, it's added to Reserve Ammo.
        /// </summary>
        public void Low()
        {
            HasPressed = true;
            Current = 0;
            Current = Result;
            Result = 0;

            //All the dice's results get added together and the UI is updated.
            foreach (IDice dice in Dies)
            {
                CurrentDice = dice.Roll();
                Result += CurrentDice;
                dice.UpdateDice(CurrentDice);
            }

            //Ammo is added and reserve is emptied
            if (Current >= Result)
            {
                Player.CurrentWeapon.TotalAmmo += Current + Reserve;
                if (Reserve > 0)
                {
                    Reserve = 0;
                }
            }

            //Ammo is added to Reserve
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
            if (GameWorld.Instance.StopGame || !GameWorld.Instance.PlayGame)
            {
                diceTimer = 0;
                Reserve = 0;
            }
            if (diceTimer > 0) diceTimer--;
            else if (!canRollDice) canRollDice = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(GameWorld.Instance.CFont, " = " + Result, new Vector2(950, 605), Color.DarkGoldenrod);
            spriteBatch.DrawString(GameWorld.Instance.BFont, "RESERVE: " + Reserve, new Vector2(800, 650), Color.Black);
            spriteBatch.Draw(GameWorld.Instance.Pixel, new Rectangle(800, 640, diceTimer, 5), Color.Blue);
        }
    }
}
