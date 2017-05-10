using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    class SaveMenu
    {
        Keys[] keysToCheck = new Keys[] { Keys.A, Keys.B, Keys.C, Keys.D, Keys.E, Keys.F, Keys.G, Keys.H, Keys.I, Keys.J, Keys.K, Keys.L, Keys.M, Keys.N, Keys.O,
                                          Keys.P, Keys.Q, Keys.R, Keys.S, Keys.T, Keys.U, Keys.V, Keys.W, Keys.X, Keys.Y, Keys.Z, Keys.Back, Keys.Space };
        KeyboardState currentKeyboardState;
        KeyboardState lastKeyboardState;
        bool hasToLoadFormDB;
        bool canInsertName;
        string text;
        int insertIndex;
        Texture2D buttonSprite;
        List<PlayerListRow> players;
        Rectangle buttonSaveRectangle;
        Rectangle buttonExitRectangle;
        Color buttonSaveColor;
        Color buttonExitColor;
        Vector2 mousePosition;

        public SaveMenu()
        {
            hasToLoadFormDB = true;
            canInsertName = false;
            insertIndex = 0;
            text = "";
            players = new List<PlayerListRow>();
            buttonSaveColor = buttonExitColor = Color.Gray;
        }

        public void LoadContent(ContentManager content)
        {
            buttonSprite = content.Load<Texture2D>("redbutton1");
            buttonSaveRectangle = new Rectangle(900, 200, buttonSprite.Width, buttonSprite.Height);
            buttonExitRectangle = new Rectangle(900, 400, buttonSprite.Width, buttonSprite.Height);
        }

        public void ShowScoreTable(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(GameWorld.Instance.CFont, "PLACE              NAME                              SCORE ", new Vector2(200, 100), Color.DarkBlue);
            spriteBatch.Draw(buttonSprite, buttonSaveRectangle, buttonSaveColor);
            spriteBatch.DrawString(GameWorld.Instance.CFont, "SAVE ", new Vector2(buttonSaveRectangle.X + 80, buttonSaveRectangle.Y + 15), buttonSaveColor);
            spriteBatch.Draw(buttonSprite, buttonExitRectangle, buttonExitColor);
            spriteBatch.DrawString(GameWorld.Instance.CFont, "EXIT ", new Vector2(buttonExitRectangle.X + 80, buttonExitRectangle.Y + 15), buttonExitColor);
            if (players.Count > 0)
            {
                int currentPlace = 1;
                int yPosition = 150;
                for (int i = 0; i < players.Count; i++)
                {
                    spriteBatch.DrawString(GameWorld.Instance.CFont, (i + 1).ToString(), new Vector2(220, yPosition), Color.Black);
                    if (i == insertIndex && canInsertName)
                    {
                        spriteBatch.Draw(GameWorld.Instance.Pixel, new Rectangle(330, yPosition, 150, 25), Color.DarkGray);
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

        public void UpdateScoreTable()
        {
            if (hasToLoadFormDB)
            {
                players = DataBaseClass.DataBaseInstance.GetPlayersList();

                if (players.Count > 0)
                {
                    insertIndex = 0;
                    for (int i = 0; i < players.Count; i++)
                    {
                        if (players[i].Score >= Player.Scores)
                        {
                            insertIndex = i;
                            canInsertName = true;
                        }
                        if (i > 8) break;
                    }
                    insertIndex++;
                    players.Insert(insertIndex, new PlayerListRow("", Player.Scores));
                }
                else players.Add(new PlayerListRow("", Player.Scores));
                hasToLoadFormDB = false;
            }
        }

        public void UpdateInputUserName()
        {
            if (canInsertName)
            {
                currentKeyboardState = Keyboard.GetState();
                foreach (Keys key in keysToCheck)
                {
                    if (CheckKey(key))
                    {
                        AddKeyToText(key);
                        if (canInsertName)
                            players[insertIndex].Name = text;
                        break;
                    }
                }
                lastKeyboardState = currentKeyboardState;
            }
            var mouseState = Mouse.GetState();
            mousePosition = new Vector2(mouseState.Position.X, mouseState.Position.Y);

            if (buttonSaveRectangle.Contains(mousePosition) && canInsertName)
                buttonSaveColor = Color.White;
            else buttonSaveColor = Color.Gray;

            if (buttonExitRectangle.Contains(mousePosition))
                buttonExitColor = Color.White;
            else buttonExitColor = Color.Gray;

            if (mouseState.LeftButton == ButtonState.Pressed && buttonSaveRectangle.Contains(mousePosition))
                ButtonSavePressed();
            if (mouseState.LeftButton == ButtonState.Pressed && buttonExitRectangle.Contains(mousePosition))
                ButtonExitPressed();
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

        private bool CheckKey(Keys theKey)
        {
            return lastKeyboardState.IsKeyDown(theKey) && currentKeyboardState.IsKeyUp(theKey);
        }

        public void ButtonSavePressed()
        {
            if (players[insertIndex].Name != "")
                canInsertName = false;
        }

        public void ButtonExitPressed()
        {
            canInsertName = false;
            GameWorld.Instance.CanSavePlayer = false;
        }
    }
}
