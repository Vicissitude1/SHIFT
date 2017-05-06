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
            if (GameObject.GetComponent("Enemy") is Enemy)
            {
                spriteBatch.DrawString(GameWorld.Instance.AFont, ((int)(GameObject.GetComponent("Enemy") as Enemy).EnemyHealth).ToString(), new Vector2(GameObject.Transform.Position.X + 10, GameObject.Transform.Position.Y - 16), Color.DarkBlue);
                Scale = 1.5f - 400 / (GameObject.GetComponent("Transform") as Transform).Position.Y / 5;
                //scale = 1;
                DrawBorder(spriteBatch, new Rectangle((int)(GameObject.GetComponent("Transform") as Transform).Position.X - 5, (int)(GameObject.GetComponent("Transform") as Transform).Position.Y - 5, 50, 5), 1, Color.Black);
                if ((GameObject.GetComponent("Enemy") as Enemy).EnemyHealth >= 30)
                    spriteBatch.Draw(pixel, new Rectangle((int)(GameObject.GetComponent("Transform") as Transform).Position.X - 5, (int)(GameObject.GetComponent("Transform") as Transform).Position.Y - 4, (int)(GameObject.GetComponent("Enemy") as Enemy).EnemyHealth / 2, 3), Color.Green);
                else spriteBatch.Draw(pixel, new Rectangle((int)(GameObject.GetComponent("Transform") as Transform).Position.X - 5, (int)(GameObject.GetComponent("Transform") as Transform).Position.Y - 4, (int)(GameObject.GetComponent("Enemy") as Enemy).EnemyHealth / 2, 3), Color.Red);
            }

            else if (GameObject.GetComponent("Player") is Player)
            {
                spriteBatch.DrawString(GameWorld.Instance.BFont, "Health: " + Player.Health, new Vector2(400, 600), Color.Black);
                spriteBatch.DrawString(GameWorld.Instance.BFont, "Score: " + Player.Scores, new Vector2(400, 640), Color.Black);
                DrawBorder(spriteBatch, new Rectangle(500, 600, 102, 20), 1, Color.Black);
                if(Player.Health >= 30)
                    spriteBatch.Draw(pixel, new Rectangle(500, 601, Player.Health,18), Color.Green);
                else spriteBatch.Draw(pixel, new Rectangle(500, 601, Player.Health, 18), Color.Red);

            }

            spriteBatch.Draw(Sprite, GameObject.Transform.Position + Offset, Rectangle, Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, layerDepth);
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
