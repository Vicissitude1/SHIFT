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

        public SpriteRenderer(GameObject gameObject, string spriteName, float layerDepth) : base(gameObject)
        {
            this.spriteName = spriteName;
            this.layerDepth = layerDepth;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (GameObject.GetComponent("Enemy") is Enemy)
                spriteBatch.DrawString(GameWorld.Instance.AFont, ((int)(GameObject.GetComponent("Enemy") as Enemy).EnemyHealth).ToString(), new Vector2(GameObject.Transform.Position.X + 10, GameObject.Transform.Position.Y - 10), Color.DarkBlue);

            if (GameObject.GetComponent("Enemy") is Enemy)
                spriteBatch.DrawString(GameWorld.Instance.BFont, "Health: " + Player.Health, new Vector2(100, 600), Color.Black);

            spriteBatch.Draw(Sprite, GameObject.Transform.Position + Offset, Rectangle, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, layerDepth);
        }

        public void LoadContent(ContentManager content)
        {
            Sprite = content.Load<Texture2D>(spriteName);
            this.Rectangle = new Rectangle(0, 0, Sprite.Width, Sprite.Height);
        }
    }
}
