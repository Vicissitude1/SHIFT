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
    /// Represents the Menu
    /// </summary>
    class Menu
    {
        Texture2D buttonSprite;
        SoundEffect effect;
        Rectangle buttonHowRectangle;
        Rectangle buttonStartRectangle;
        Rectangle buttonExitRectangle;
        Rectangle buttonScoreRectangle;
        Color buttonHowColor;
        Color buttonStartColor;
        Color buttonExitColor;
        Color buttonScoreColor;
        Vector2 mousePosition;
        bool canPlaySound;
        bool firstStart;

        public Menu()
        {
            buttonStartColor = buttonExitColor = buttonScoreColor = Color.LightGray;
            canPlaySound = true;
            firstStart = true;
        }

        public void LoadContent(ContentManager content)
        {
            buttonSprite = content.Load<Texture2D>("button");
            buttonHowRectangle = new Rectangle(1000, 200, buttonSprite.Width, buttonSprite.Height);
            buttonScoreRectangle = new Rectangle(1000, 300, buttonSprite.Width, buttonSprite.Height);
            buttonStartRectangle = new Rectangle(1000, 400, buttonSprite.Width, buttonSprite.Height);
            buttonExitRectangle = new Rectangle(1000, 550, buttonSprite.Width, buttonSprite.Height);
            effect = content.Load<SoundEffect>("buttonClick");
        }

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

        public void UpdateUI()
        {
            MouseState mouseState = Mouse.GetState();
            mousePosition = new Vector2(mouseState.Position.X, mouseState.Position.Y);

            buttonHowColor = (buttonHowRectangle.Contains(mousePosition)) ? Color.White : Color.LightGray;
            buttonScoreColor = (buttonScoreRectangle.Contains(mousePosition)) ? Color.White : Color.LightGray;
            buttonStartColor = (buttonStartRectangle.Contains(mousePosition)) ? Color.White : Color.LightGray;
            buttonExitColor = (buttonExitRectangle.Contains(mousePosition)) ? Color.White : Color.LightGray;

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

        public void ButtonHowPressed()
        {
            if (canPlaySound)
            {
                effect.Play();
                canPlaySound = false;
            }
        }

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
