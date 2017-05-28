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
        /// Checks if game is started first time
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
            buttonSprite = content.Load<Texture2D>("redbutton1");
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
            spriteBatch.DrawString(GameWorld.Instance.CFont, "**MAIN MENU** ", new Vector2(500, 50), Color.DarkBlue);
            spriteBatch.Draw(buttonSprite, buttonHowRectangle, buttonHowColor);
            spriteBatch.DrawString(GameWorld.Instance.CFont, "HOW TO PLAY", new Vector2(buttonHowRectangle.X + 25, buttonHowRectangle.Y + 15), buttonHowColor);
            spriteBatch.Draw(buttonSprite, buttonScoreRectangle, buttonScoreColor);
            spriteBatch.DrawString(GameWorld.Instance.CFont, "SCORE LIST ", new Vector2(buttonScoreRectangle.X + 35, buttonScoreRectangle.Y + 15), buttonScoreColor);
            spriteBatch.Draw(buttonSprite, buttonStartRectangle, buttonStartColor);
            spriteBatch.DrawString(GameWorld.Instance.CFont, "START GAME ", new Vector2(buttonStartRectangle.X + 35, buttonStartRectangle.Y + 15), buttonStartColor);
            spriteBatch.Draw(buttonSprite, buttonExitRectangle, buttonExitColor);
            spriteBatch.DrawString(GameWorld.Instance.CFont, "EXIT GAME", new Vector2(buttonExitRectangle.X + 45, buttonExitRectangle.Y + 15), buttonExitColor);
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
            // Checks if one of the buttons is pressed
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
            if (firstStart) firstStart = false; 
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
