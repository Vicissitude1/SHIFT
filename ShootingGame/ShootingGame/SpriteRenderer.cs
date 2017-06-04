using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    /// <summary>
    /// Represents the SpriteRenderer
    /// </summary>
    class SpriteRenderer : Component, IDrawable, ILoadable
    {
        public Rectangle Rectangle { get; set; }

        public Texture2D Sprite { get; private set; }

        public Vector2 Offset { get; set; }

        private string spriteName;

        private float layerDepth;

        public float Scale { get; set; }

        Texture2D pixel;

        /// <summary>
        /// The SpriteRenderer\s constructor
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="spriteName"></param>
        /// <param name="layerDepth"></param>
        public SpriteRenderer(GameObject gameObject, string spriteName, float layerDepth) : base(gameObject)
        {
            this.spriteName = spriteName;
            this.layerDepth = layerDepth;
            Scale = 1;
            pixel = GameWorld.Instance.Pixel;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, GameObject.Transform.Position + Offset, Rectangle, Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, layerDepth);

            if (GameObject.GetComponent("Enemy") is Enemy)
            {
                spriteBatch.DrawString(GameWorld.Instance.AFont, ((int)(GameObject.GetComponent("Enemy") as Enemy).EnemyHealth).ToString(), new Vector2(GameObject.Transform.Position.X + 10, GameObject.Transform.Position.Y - 16), Color.Black);
                DrawBorder(spriteBatch, new Rectangle((int)(GameObject.GetComponent("Transform") as Transform).Position.X - 5, (int)(GameObject.GetComponent("Transform") as Transform).Position.Y - 5, 50, 5), 1, Color.Gray);
                if ((GameObject.GetComponent("Enemy") as Enemy).EnemyHealth >= 30)
                    spriteBatch.Draw(pixel, new Rectangle((int)(GameObject.GetComponent("Transform") as Transform).Position.X - 5, (int)(GameObject.GetComponent("Transform") as Transform).Position.Y - 4, (int)(GameObject.GetComponent("Enemy") as Enemy).EnemyHealth / 2, 3), Color.Green);
                else spriteBatch.Draw(pixel, new Rectangle((int)(GameObject.GetComponent("Transform") as Transform).Position.X - 5, (int)(GameObject.GetComponent("Transform") as Transform).Position.Y - 4, (int)(GameObject.GetComponent("Enemy") as Enemy).EnemyHealth / 2, 3), Color.Red);
            }
            else if (GameObject.GetComponent("Player") is Player)
            {
                spriteBatch.Draw(pixel, new Rectangle(0, 570, 1300, 150), Color.LightGray);
                spriteBatch.Draw(Player.CurrentWeapon.Sprite, new Rectangle(300, 630, Player.CurrentWeapon.Sprite.Width, Player.CurrentWeapon.Sprite.Height), Color.White);
                spriteBatch.DrawString(GameWorld.Instance.BFont, "WEAPON: ", new Vector2(310, 590), Color.Black);
                spriteBatch.DrawString(GameWorld.Instance.BFont, "TOTAL AMMO: " + Player.CurrentWeapon.TotalAmmo, new Vector2(510, 590), Color.Black);

                if (Player.CurrentWeapon.IsReloading && Player.CurrentWeapon.TotalAmmo > 0)
                {
                    spriteBatch.DrawString(GameWorld.Instance.BFont, "AMMO: RELOADING", new Vector2(510, 630), Color.Red);
                    spriteBatch.Draw(pixel, new Rectangle(510, 650, Player.CurrentWeapon.CurrentReloadTime / 10, 5), Color.Red);
                }
                else if (Player.CurrentWeapon.IsReloading && Player.CurrentWeapon.TotalAmmo == 0 && Player.CurrentWeapon.Ammo == 0)
                {
                    spriteBatch.DrawString(GameWorld.Instance.BFont, "NO MORE AMMO", new Vector2(510, 630), Color.Red);
                }
                else
                {
                    spriteBatch.DrawString(GameWorld.Instance.BFont, "AMMO: " + Player.CurrentWeapon.Ammo, new Vector2(510, 630), Color.Black);
                    string ammo = "";
                    int i = 0;
                    while (i < Player.CurrentWeapon.Ammo)
                    {
                        ammo += "!";
                        i++;
                    }
                    spriteBatch.DrawString(GameWorld.Instance.CFont, ammo, new Vector2(510, 650), Color.DarkOrange);
                }
                spriteBatch.DrawString(GameWorld.Instance.BFont, "HEALTH: " + Player.Health, new Vector2(20, 590), Color.Black);
                spriteBatch.DrawString(GameWorld.Instance.BFont, "SCORE: " + Player.Scores, new Vector2(20, 630), Color.Black);
                DrawBorder(spriteBatch, new Rectangle(130, 590, 101, 15), 1, Color.DarkSlateGray);
                if (Player.Health >= 30)
                    spriteBatch.Draw(pixel, new Rectangle(130, 591, Player.Health, 13), Color.Green);
                else spriteBatch.Draw(pixel, new Rectangle(130, 591, Player.Health, 13), Color.Red);
            }
            if (GameObject.GetComponent("PowerUpObject") is PowerUpObject)
            {
                spriteBatch.DrawString(GameWorld.Instance.CFont, (GameObject.GetComponent("PowerUpObject") as PowerUpObject).Name, new Vector2(GameObject.Transform.Position.X + 17, GameObject.Transform.Position.Y + 10), Color.Yellow);
            }

            DrawBorder(spriteBatch, new Rectangle(10, 580, 240, 100), 2, Color.DarkBlue);
            DrawBorder(spriteBatch, new Rectangle(280, 580, 440, 100), 2, Color.DarkGreen);
            DrawBorder(spriteBatch, new Rectangle(750, 580, 270, 100), 2, Color.DarkRed);
        }

        public void LoadContent(ContentManager content)
        {
            Sprite = content.Load<Texture2D>(spriteName);
            this.Rectangle = new Rectangle(0, 0, Sprite.Width, Sprite.Height);
        }

        public void DrawBorder(SpriteBatch spriteBatch, Rectangle rectangleToDraw, int thicknessOfBorder, Color borderColor)
        {
            // Draw top line
            spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, rectangleToDraw.Width, thicknessOfBorder), borderColor);
            // Draw left line
            spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X - thicknessOfBorder, rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);
            // Draw right line
            spriteBatch.Draw(pixel, new Rectangle((rectangleToDraw.X + rectangleToDraw.Width - thicknessOfBorder), rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);
            // Draw bottom line
            spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y + rectangleToDraw.Height - thicknessOfBorder, rectangleToDraw.Width, thicknessOfBorder), borderColor);
        }
    }
}
