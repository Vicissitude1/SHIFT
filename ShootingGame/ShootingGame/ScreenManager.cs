using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ShootingGame
{
    public class ScreenManager
    {
        private static ScreenManager instance;
        public Vector2 Dimensions { private set; get; }

        public static ScreenManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new ScreenManager();

                return instance;
            }
        }

        private ScreenManager()
        {
            Dimensions = new Vector2(1300, 700);
        }

        public void LoadContent(ContentManager content)
        {

        }

        public void UnloadContent()
        {

        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
