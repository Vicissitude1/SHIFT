using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    /// <summary>
    /// Represents the Main Menu
    /// </summary>
    class Menu
    {
        /// <summary>
        /// The button's sprite
        /// </summary>
        Texture2D buttonSprite;

        /// <summary>
        /// The "SHIFT" sprite
        /// </summary>
        Texture2D shiftSprite;

        /// <summary>
        /// The crosshair's sprite
        /// </summary>
        Texture2D crosshair;

        /// <summary>
        /// The "Button click" sound effect
        /// </summary>
        SoundEffect effect;

        /// <summary>
        /// The "How to play" button's rectangle
        /// </summary>
        Rectangle buttonHowRectangle;

        /// <summary>
        /// The "Start game" button's rectangle
        /// </summary>
        Rectangle buttonStartRectangle;

        /// <summary>
        /// The "Exit" button's rectangle
        /// </summary>
        Rectangle buttonExitRectangle;

        /// <summary>
        /// The "Score" button's rectangle
        /// </summary>
        Rectangle buttonScoreRectangle;

        /// <summary>
        /// The "How to play" button's color
        /// </summary>
        Color buttonHowColor;

        /// <summary>
        /// The "Start" button's color
        /// </summary>
        Color buttonStartColor;

        /// <summary>
        /// The "Exit" button's color
        /// </summary>
        Color buttonExitColor;

        /// <summary>
        /// The "Score list" button's color
        /// </summary>
        Color buttonScoreColor;

        /// <summary>
        /// The mouse position
        /// </summary>
        Vector2 mousePosition;

        /// <summary>
        /// Checks if necassery to play "Button click" sound effect
        /// </summary>
        bool canPlaySound;

        /// <summary>
        /// Checks if the Main Menu is started first time
        /// </summary>
        bool firstStart;

        /// <summary>
        /// The Menu's construtor
        /// </summary>
        public Menu()
        {
            buttonStartColor = buttonExitColor = buttonScoreColor = Color.LightGray;
            canPlaySound = true;
            firstStart = true;
        }

        /// <summary>
        /// Loads content for the buttons (sprite, sprite rectangles and soun effect)
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            buttonSprite = content.Load<Texture2D>("buttonsprite");
            crosshair = content.Load<Texture2D>("SHIFT Crosshair Shoot");
            shiftSprite = content.Load<Texture2D>("shiftmenu");
            buttonHowRectangle = new Rectangle(1000, 200, buttonSprite.Width, buttonSprite.Height);
            buttonScoreRectangle = new Rectangle(1000, 300, buttonSprite.Width, buttonSprite.Height);
            buttonStartRectangle = new Rectangle(1000, 400, buttonSprite.Width, buttonSprite.Height);
            buttonExitRectangle = new Rectangle(1000, 550, buttonSprite.Width, buttonSprite.Height);
            effect = content.Load<SoundEffect>("buttonClick");
        }

        /// <summary>
        /// Draws the buttons with their names
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void ShowMainMenu(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(GameWorld.Instance.DFont, "** MAIN MENU ** ", new Vector2(550, 50), Color.DarkGreen);
            spriteBatch.Draw(buttonSprite, buttonHowRectangle, buttonHowColor);
            spriteBatch.DrawString(GameWorld.Instance.CFont, "HOW TO PLAY", new Vector2(buttonHowRectangle.X + 25, buttonHowRectangle.Y + 15), buttonHowColor);
            spriteBatch.Draw(buttonSprite, buttonScoreRectangle, buttonScoreColor);
            spriteBatch.DrawString(GameWorld.Instance.CFont, "SCORE LIST ", new Vector2(buttonScoreRectangle.X + 35, buttonScoreRectangle.Y + 15), buttonScoreColor);
            spriteBatch.Draw(buttonSprite, buttonStartRectangle, buttonStartColor);
            spriteBatch.DrawString(GameWorld.Instance.CFont, "START GAME ", new Vector2(buttonStartRectangle.X + 35, buttonStartRectangle.Y + 15), buttonStartColor);
            spriteBatch.Draw(buttonSprite, buttonExitRectangle, buttonExitColor);
            spriteBatch.DrawString(GameWorld.Instance.CFont, "EXIT GAME", new Vector2(buttonExitRectangle.X + 45, buttonExitRectangle.Y + 15), buttonExitColor);

            if (firstStart)
            {
                spriteBatch.Draw(GameWorld.Instance.Pixel, new Rectangle(100, 200, 800, 410), Color.DarkSlateGray);
                spriteBatch.Draw(shiftSprite, new Rectangle(170, 220, shiftSprite.Width, shiftSprite.Height), Color.White);

            }
            else
            {
                spriteBatch.DrawString(GameWorld.Instance.CFont, "   During Gameplay:", new Vector2(100, 180), Color.DarkRed);
                spriteBatch.DrawString(GameWorld.Instance.CFont, "1. Switch Weapons: [1]-Gun, [2]-Rifle, [3]-Machinegun", new Vector2(100, 210), Color.Green);
                spriteBatch.DrawString(GameWorld.Instance.CFont, "2. Shoot: Left Mouse Key", new Vector2(100, 240), Color.Green);
                spriteBatch.DrawString(GameWorld.Instance.CFont, "3. Menu: [M]", new Vector2(100, 270), Color.Green);
                spriteBatch.DrawString(GameWorld.Instance.CFont, "4. High 'N Low: Up & Down Arrow Keys", new Vector2(100, 300), Color.Green);
                spriteBatch.DrawString(GameWorld.Instance.CFont, "   High 'N Low:", new Vector2(100, 350), Color.DarkRed);
                spriteBatch.DrawString(GameWorld.Instance.CFont, "The games primary function to restock your ammo is a dice game of High 'N Low: Look", new Vector2(100, 380), Color.DarkBlue);
                spriteBatch.DrawString(GameWorld.Instance.CFont, "at  the total number of eyes  displayed  by  the set of dice in the UI, and use the mouse", new Vector2(100, 410), Color.DarkBlue);
                spriteBatch.DrawString(GameWorld.Instance.CFont, "keys to  guess if  the  next set of  dice will have a higher or lower total number  of eyes.", new Vector2(100, 440), Color.DarkBlue);
                spriteBatch.DrawString(GameWorld.Instance.CFont, "Incorrect guesses will stockpile the ammo.", new Vector2(100, 470), Color.DarkBlue);
                spriteBatch.DrawString(GameWorld.Instance.CFont, "   Power Ups:", new Vector2(100, 520), Color.DarkRed);
                spriteBatch.DrawString(GameWorld.Instance.CFont, "During the gameplay, Power Ups will appear. Shoot them for extra bonuses like Health,", new Vector2(100, 550), Color.DarkGoldenrod);
                spriteBatch.DrawString(GameWorld.Instance.CFont, "Ammo or Score!", new Vector2(100, 580), Color.DarkGoldenrod);

            }
        }

        /// <summary>
        /// Updates the UI
        /// </summary>
        public void UpdateUI()
        {
            MouseState mouseState = Mouse.GetState();
            mousePosition = new Vector2(mouseState.Position.X, mouseState.Position.Y);
            // Checks if one of the buttons conatins the mouse
            buttonHowColor = (buttonHowRectangle.Contains(mousePosition)) ? Color.White : Color.LightGray;
            buttonScoreColor = (buttonScoreRectangle.Contains(mousePosition)) ? Color.White : Color.LightGray;
            buttonStartColor = (buttonStartRectangle.Contains(mousePosition)) ? Color.White : Color.LightGray;
            buttonExitColor = (buttonExitRectangle.Contains(mousePosition)) ? Color.White : Color.LightGray;
            // Checks if one of the buttons is pressed and performs the corresponding functionality
            if (mouseState.LeftButton == ButtonState.Pressed && buttonHowRectangle.Contains(mousePosition))
                ButtonHowPressed();
            else if (mouseState.LeftButton == ButtonState.Pressed && buttonScoreRectangle.Contains(mousePosition))
                ButtonScorePressed();
            else if (mouseState.LeftButton == ButtonState.Pressed && buttonStartRectangle.Contains(mousePosition))
                ButtonStartPressed();
            else if (mouseState.LeftButton == ButtonState.Pressed && buttonExitRectangle.Contains(mousePosition))
                ButtonExitPressed();
            else if (mouseState.LeftButton == ButtonState.Released && !canPlaySound)
                canPlaySound = true;
        }

        /// <summary>
        /// Provides the drawing of the "How to play" image 
        /// </summary>
        public void ButtonHowPressed()
        {
            if (firstStart) firstStart = false;
            if (canPlaySound)
            {
                effect.Play();
                canPlaySound = false;
            }
        }

        /// <summary>
        /// Performs the exit from Main Menu to the Score Menu
        /// </summary>
        public void ButtonScorePressed()
        {
            if (!firstStart) firstStart = true;
            GameWorld.Instance.PlayGame = false;
            GameWorld.Instance.ShowScoreMenu = true;
            if (canPlaySound)
            {
                effect.Play();
                canPlaySound = false;
            }
        }

        /// <summary>
        /// Starts the game
        /// </summary>
        public void ButtonStartPressed()
        {
            if (!firstStart) firstStart = true; 
            else  GameWorld.Instance.ReplaceObjects = true;
            GameWorld.Instance.PlayGame = true;
            GameWorld.Instance.StopGame = false;
            GameWorld.Instance.ShowScoreMenu = false;
            if (canPlaySound)
            {
                effect.Play();
                canPlaySound = false;
            }
        }

        /// <summary>
        /// Perform the exit from the game
        /// </summary>
        public void ButtonExitPressed()
        {
            GameWorld.Instance.Exit();
            if (canPlaySound)
            {
                effect.Play();
                canPlaySound = false;
            }
        }
    }
}
