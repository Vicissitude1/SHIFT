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
    class SpriteRenderer : Component, IDrawable, ILoadable
    {
        public Rectangle Rectangle { get; set; }

        public Texture2D Sprite { get; private set; }

        public Vector2 Offset { get; set; }

        private string spriteName;

        private float layerDepth;

        public float Scale { get; private set; }

        Texture2D pixel;

        public SpriteRenderer(GameObject gameObject, string spriteName, float layerDepth) : base(gameObject)
        {
            this.spriteName = spriteName;
            this.layerDepth = layerDepth;
            Scale = 1;
            pixel = new Texture2D(GameWorld.Instance.GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, GameObject.Transform.Position + Offset, Rectangle, Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, layerDepth);

            if (GameObject.GetComponent("Enemy") is Enemy)
            {
                spriteBatch.DrawString(GameWorld.Instance.AFont, ((int)(GameObject.GetComponent("Enemy") as Enemy).EnemyHealth).ToString(), new Vector2(GameObject.Transform.Position.X + 10, GameObject.Transform.Position.Y - 16), Color.Black);
                Scale = 1.5f - 400 / (GameObject.GetComponent("Transform") as Transform).Position.Y / 5;
                //scale = 1;
                DrawBorder(spriteBatch, new Rectangle((int)(GameObject.GetComponent("Transform") as Transform).Position.X - 5, (int)(GameObject.GetComponent("Transform") as Transform).Position.Y - 5, 50, 5), 1, Color.Gray);
                if ((GameObject.GetComponent("Enemy") as Enemy).EnemyHealth >= 30)
                    spriteBatch.Draw(pixel, new Rectangle((int)(GameObject.GetComponent("Transform") as Transform).Position.X - 5, (int)(GameObject.GetComponent("Transform") as Transform).Position.Y - 4, (int)(GameObject.GetComponent("Enemy") as Enemy).EnemyHealth / 2, 3), Color.Green);
                else spriteBatch.Draw(pixel, new Rectangle((int)(GameObject.GetComponent("Transform") as Transform).Position.X - 5, (int)(GameObject.GetComponent("Transform") as Transform).Position.Y - 4, (int)(GameObject.GetComponent("Enemy") as Enemy).EnemyHealth / 2, 3), Color.Red);
            }

            if (GameObject.GetComponent("PlayerBullet") is PlayerBullet || GameObject.GetComponent("EnemyBullet") is EnemyBullet)
            {
                Scale = 1.2f - 400 / (GameObject.GetComponent("Transform") as Transform).Position.Y/3;
            }

            else if (GameObject.GetComponent("Player") is Player)
            {
                spriteBatch.Draw(pixel, new Rectangle(0, 570, 1300, 150), Color.LightGray);
                if(Player.CurrentWeapon.Name== "GUN")
                    spriteBatch.Draw(Player.CurrentWeapon.Sprite, new Rectangle(170, 590, 50, 30), Color.White);
                else if (Player.CurrentWeapon.Name == "RIFLE")
                    spriteBatch.Draw(Player.CurrentWeapon.Sprite, new Rectangle(150, 590, 140, 40), Color.White);
                else if (Player.CurrentWeapon.Name == "MACHINEGUN")
                    spriteBatch.Draw(Player.CurrentWeapon.Sprite, new Rectangle(150, 565, 130, 100), Color.White);

                //spriteBatch.Draw(Player.CurrentWeapon.Sprite, new Rectangle(100, 600, Player.CurrentWeapon.Sprite.Width, Player.CurrentWeapon.Sprite.Height), Color.White);

                spriteBatch.DrawString(GameWorld.Instance.BFont, "WEAPON: ", new Vector2(50, 600), Color.Black);

                if (Player.CurrentWeapon.IsReloading)
                {
                    spriteBatch.DrawString(GameWorld.Instance.BFont, "AMMO: RELOADING", new Vector2(50, 640), Color.Red);
                    spriteBatch.Draw(pixel, new Rectangle(50, 660, Player.CurrentWeapon.CurrentReloadTime / 10, 5), Color.Red);
                }
                else
                {
                    spriteBatch.DrawString(GameWorld.Instance.BFont, "AMMO: " + Player.CurrentWeapon.Ammo, new Vector2(50, 640), Color.Black);
                    string ammo = "";
                    int i = 0;
                    while(i<Player.CurrentWeapon.Ammo)
                    {
                        ammo += "!";
                        i++;
                    }
                    spriteBatch.DrawString(GameWorld.Instance.CFont, ammo, new Vector2(50, 660), Color.DarkOrange);
                }
                spriteBatch.DrawString(GameWorld.Instance.BFont, "Health: " + Player.Health, new Vector2(400, 600), Color.Black);
                spriteBatch.DrawString(GameWorld.Instance.BFont, "Score: " + Player.Scores, new Vector2(400, 640), Color.Black);
                spriteBatch.DrawString(GameWorld.Instance.ResultFont, " = " + GameWorld.Instance.Result, new Vector2(925, 625), Color.Black);
                DrawBorder(spriteBatch, new Rectangle(500, 600, 102, 20), 1, Color.Black);
                if (Player.Health >= 30)
                    spriteBatch.Draw(pixel, new Rectangle(500, 601, Player.Health, 18), Color.Green);
                else spriteBatch.Draw(pixel, new Rectangle(500, 601, Player.Health, 18), Color.Red);

            }
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
