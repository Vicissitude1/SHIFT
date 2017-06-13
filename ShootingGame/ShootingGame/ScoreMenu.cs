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
    /// Represents the ScoreMenu
    /// </summary>
    class ScoreMenu
    {
        /// <summary>
        /// The types of keys
        /// </summary>
        Keys[] keysToCheck = new Keys[] { Keys.A, Keys.B, Keys.C, Keys.D, Keys.E, Keys.F, Keys.G, Keys.H, Keys.I, Keys.J, Keys.K, Keys.L, Keys.M, Keys.N, Keys.O,
                                          Keys.P, Keys.Q, Keys.R, Keys.S, Keys.T, Keys.U, Keys.V, Keys.W, Keys.X, Keys.Y, Keys.Z, Keys.Back, Keys.Space };

        /// <summary>
        /// The current Keybord status
        /// </summary>
        KeyboardState currentKeyboardState;

        /// <summary>
        /// The previous keybord status
        /// </summary>
        KeyboardState lastKeyboardState;

        /// <summary>
        /// Checks if necassery to load the score table from data base
        /// </summary>
        bool hasToLoadFormDB;

        /// <summary>
        /// Checks if player can insert his name
        /// </summary>
        bool canInsertName;

        /// <summary>
        /// Checks if necassery to play button click sound
        /// </summary>
        bool canPlaySound;

        /// <summary>
        /// The Player's name, which has to be added to the score list
        /// </summary>
        string text;

        /// <summary>
        /// The current Player's index in the score list
        /// </summary>
        int insertIndex;

        /// <summary>
        /// The button's sprite
        /// </summary>
        Texture2D buttonSprite;

        /// <summary>
        /// The players list
        /// </summary>
        List<PlayerListRow> players;

        /// <summary>
        /// The "Save" button's rectangle
        /// </summary>
        Rectangle buttonSaveRectangle;

        /// <summary>
        /// The "Main Menu" button's rectangle
        /// </summary>
        Rectangle buttonExitRectangle;

        /// <summary>
        /// The "Save" button's color
        /// </summary>
        Color buttonSaveColor;

        /// <summary>
        /// The "Main Menu" button's color
        /// </summary>
        Color buttonExitColor;

        /// <summary>
        /// The "Button click" sound effect
        /// </summary>
        SoundEffect effect;

        /// <summary>
        /// The current mouse position
        /// </summary>
        Vector2 mousePosition;

        /// <summary>
        /// ScoreMenu's construtor
        /// </summary>
        public ScoreMenu()
        {
            hasToLoadFormDB = true;
            canInsertName = false;
            insertIndex = 0;
            text = "";
            players = new List<PlayerListRow>();
            buttonSaveColor = buttonExitColor = Color.LightGray;
            canPlaySound = true;
        }

        /// <summary>
        /// Loads the Scoremenu's content
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            buttonSprite = content.Load<Texture2D>("butt");
            buttonSaveRectangle = new Rectangle(1000, 350, buttonSprite.Width, buttonSprite.Height);
            buttonExitRectangle = new Rectangle(1000, 500, buttonSprite.Width, buttonSprite.Height);
            effect = content.Load<SoundEffect>("buttonClick");
        }

        /// <summary>
        /// Draws the buttons and score list on the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void ShowScoreTable(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(GameWorld.Instance.CFont, "PLACE              NAME                              SCORE ", new Vector2(200, 100), Color.DarkBlue);
            spriteBatch.Draw(buttonSprite, buttonSaveRectangle, buttonSaveColor);
            spriteBatch.DrawString(GameWorld.Instance.CFont, "SAVE ", new Vector2(buttonSaveRectangle.X + 70, buttonSaveRectangle.Y + 15), buttonSaveColor);
            spriteBatch.Draw(buttonSprite, buttonExitRectangle, buttonExitColor);
            spriteBatch.DrawString(GameWorld.Instance.CFont, "MAIN MENU", new Vector2(buttonExitRectangle.X + 40, buttonExitRectangle.Y + 15), buttonExitColor);
            
            // Shows on the screen score list
            if (players.Count > 0)
            {
                int currentPlace = 1;
                int yPosition = 150;
                for (int i = 0; i < players.Count; i++)
                {
                    spriteBatch.DrawString(GameWorld.Instance.CFont, (i + 1).ToString(), new Vector2(220, yPosition), Color.Black);
                    if (i == insertIndex && canInsertName)
                    {
                        spriteBatch.Draw(GameWorld.Instance.Pixel, new Rectangle(330, yPosition, 200, 25), Color.DarkGray);
                        spriteBatch.DrawString(GameWorld.Instance.CFont, players[i].Name, new Vector2(350, yPosition), Color.White);
                    }
                    else spriteBatch.DrawString(GameWorld.Instance.CFont, players[i].Name, new Vector2(350, yPosition), Color.Black);
                    spriteBatch.DrawString(GameWorld.Instance.CFont, players[i].Score.ToString(), new Vector2(600, yPosition), Color.Black);
                    if (i == 9) break;
                    currentPlace++;
                    yPosition += 30;
                }
            }
        }
         /// <summary>
         /// Updates score list from database and pastes current player
         /// </summary>
        public void UpdateScoreTable()
        {
            if (hasToLoadFormDB)
            {
                // Loads score list from database
                players = DataBaseClass.Instance.GetPlayersList();
                // Checks if player has some scores
                if (Player.Scores > 0)
                {
                    // Places the player in the score list on the corresponding place
                    if (players.Count > 0)
                    {
                        insertIndex = 0;
                        for (int i = 0; i < players.Count; i++)
                        {
                            if (players[i].Score >= Player.Scores)
                            {
                                insertIndex = i + 1;
                            }
                            if (i == 8) break;
                        }
                        players.Insert(insertIndex, new PlayerListRow("", Player.Scores));
                    }
                    else players.Add(new PlayerListRow("", Player.Scores));
                    canInsertName = true;
                }
                hasToLoadFormDB = false;
            }
        }

        /// <summary>
        /// Updates UI
        /// </summary>
        public void UpdateUI()
        {
            UpdateScoreTable();

            if (canInsertName)
            {
                currentKeyboardState = Keyboard.GetState();
                foreach (Keys key in keysToCheck)
                {
                    if (CheckKey(key))
                    {
                        AddKeyToText(key);
                        players[insertIndex].Name = text;
                        break;
                    }
                }
                lastKeyboardState = currentKeyboardState;
            }
            MouseState mouseState = Mouse.GetState();
            mousePosition = new Vector2(mouseState.Position.X, mouseState.Position.Y);
            // Checks if one of the buttons conatins the mouse
            buttonSaveColor = buttonSaveRectangle.Contains(mousePosition) && canInsertName && text != "" ? Color.White : Color.LightGray;
            buttonExitColor = (buttonExitRectangle.Contains(mousePosition)) ? Color.White : Color.LightGray;
            // Checks if one of the buttons is pressed
            if (mouseState.LeftButton == ButtonState.Pressed && buttonSaveRectangle.Contains(mousePosition))
                ButtonSavePressed();
            else if (mouseState.LeftButton == ButtonState.Pressed && buttonExitRectangle.Contains(mousePosition))
                ButtonExitPressed();
            else if (mouseState.LeftButton == ButtonState.Released && !canPlaySound)
                canPlaySound = true;
        }

        /// <summary>
        /// Adds the pressed keys corresponding letter to the string text.
        /// </summary>
        /// <param name="key"></param>
        private void AddKeyToText(Keys key)
        {
            string newChar = "";

            if (text.Length >= 10 && key != Keys.Back)
                return;

            switch (key)
            {
                case Keys.A:
                    newChar += "a";
                    break;
                case Keys.B:
                    newChar += "b";
                    break;
                case Keys.C:
                    newChar += "c";
                    break;
                case Keys.D:
                    newChar += "d";
                    break;
                case Keys.E:
                    newChar += "e";
                    break;
                case Keys.F:
                    newChar += "f";
                    break;
                case Keys.G:
                    newChar += "g";
                    break;
                case Keys.H:
                    newChar += "h";
                    break;
                case Keys.I:
                    newChar += "i";
                    break;
                case Keys.J:
                    newChar += "j";
                    break;
                case Keys.K:
                    newChar += "k";
                    break;
                case Keys.L:
                    newChar += "l";
                    break;
                case Keys.M:
                    newChar += "m";
                    break;
                case Keys.N:
                    newChar += "n";
                    break;
                case Keys.O:
                    newChar += "o";
                    break;
                case Keys.P:
                    newChar += "p";
                    break;
                case Keys.Q:
                    newChar += "q";
                    break;
                case Keys.R:
                    newChar += "r";
                    break;
                case Keys.S:
                    newChar += "s";
                    break;
                case Keys.T:
                    newChar += "t";
                    break;
                case Keys.U:
                    newChar += "u";
                    break;
                case Keys.V:
                    newChar += "v";
                    break;
                case Keys.W:
                    newChar += "w";
                    break;
                case Keys.X:
                    newChar += "x";
                    break;
                case Keys.Y:
                    newChar += "y";
                    break;
                case Keys.Z:
                    newChar += "z";
                    break;
                case Keys.Space:
                    newChar += " ";
                    break;
                case Keys.Back:
                    if (text.Length != 0)
                        text = text.Remove(text.Length - 1);
                    return;
            }
            if (currentKeyboardState.IsKeyDown(Keys.RightShift) || currentKeyboardState.IsKeyDown(Keys.LeftShift))
            {
                newChar = newChar.ToUpper();
            }
            text += newChar;
        }

        /// <summary>
        /// Makes sure that double click doesn't happen
        /// </summary>
        /// <param name="theKey"></param>
        /// <returns></returns>
        private bool CheckKey(Keys theKey)
        {
            return lastKeyboardState.IsKeyDown(theKey) && currentKeyboardState.IsKeyUp(theKey);
        }
       
        /// <summary>
        /// Saves the current score list to database
        /// </summary>
        public void ButtonSavePressed()
        {
            // Makes sure that player has name and some scores
            if (canInsertName && players[insertIndex].Name != "")
            {
                while (players.Count > 10)
                    players.RemoveAt(10);
                // Saves score list to the data base
                DataBaseClass.Instance.SavePlayersList(players);
                Player.Scores = 0;
                text = "";
                canInsertName = false;
                if (canPlaySound)
                {
                    effect.Play();
                    canPlaySound = false;
                }
            }
        }

        /// <summary>
        /// Performs the exit to the Main Menu
        /// </summary>
        public void ButtonExitPressed()
        {
            canInsertName = false;
            hasToLoadFormDB = true;
            text = "";
            if(canPlaySound)
            {
                effect.Play();
                canPlaySound = false;
            }
            GameWorld.Instance.ShowScoreMenu = false;
        }
    }
}
