using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Serialization;

namespace ShootingGame
{
    public class Image
    {
        public float Alpha;
        public string Text, FontName, Path;
        public Vector2 Position, scale;
        public Rectangle SourceRect;

        public Texture2D Texture;
        Vector2 origin;
        ContentManager content;
        RenderTarget2D renderTarget;

        public Image()
        {
            Path = Text = string.Empty;
            FontName = "Orbitron";
            Position = Vector2.Zero;
            scale = Vector2.One;
            Alpha = 1.0f;
            SourceRect = Rectangle.Empty;
        }

        public void LoadContent()
        {
            content = new ContentManager(
                ScreenManager.Instance.Content.ServiceProvider, "Content");
            if (Path != string.Empty)
                Texture = content.Load<Texture2D>(Path);

            if (SourceRect == Rectangle.Empty)
        }

        public void UnloadContant()
        {
            content.Unload();
        }

        public void update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            origin = Vector2(SourceRect.Width / 2,
                SourceRect.Height / 2);
            spriteBatch.Draw(Texture, Position + origin, SourceRect, color.white = Alpha,
                0.0f, origin, scale, spriteBatch.None, 0.0f);
        }
    }
}
