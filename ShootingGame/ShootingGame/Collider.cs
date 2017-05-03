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
    class Collider : Component, IDrawable, ILoadable, IUpdateable
    {
        Texture2D texture2D;
        SpriteRenderer spriteRenderer;
        bool doCollisionCheck;
        List<Collider> otherColliders = new List<Collider>();
        public bool DoCollisionCheck
        {
            set
            {
                doCollisionCheck = value;
            }
        }

        public Collider(GameObject gameObject) : base(gameObject)
        {
            GameWorld.Instance.Colliders.Add(this);
            doCollisionCheck = true;
        }
        public Rectangle CollisionBox
        {
            get
            {
                return new Rectangle
                ((int)(GameObject.Transform.Position.X + spriteRenderer.Offset.X),
                (int)(GameObject.Transform.Position.Y + spriteRenderer.Offset.Y), spriteRenderer.Rectangle.Width,
                spriteRenderer.Rectangle.Height);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle topLine = new Rectangle(CollisionBox.X, CollisionBox.Y, CollisionBox.Width, 1);
            Rectangle bottomLine = new Rectangle(CollisionBox.X, CollisionBox.Y + CollisionBox.Height, CollisionBox.Width, 1);
            Rectangle rightLine = new Rectangle(CollisionBox.X + CollisionBox.Width, CollisionBox.Y, 1, CollisionBox.Height);
            Rectangle leftLine = new Rectangle(CollisionBox.X, CollisionBox.Y, 1, CollisionBox.Height);
            spriteBatch.Draw(texture2D, topLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(texture2D, bottomLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(texture2D, rightLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(texture2D, leftLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
        }

        public void LoadContent(ContentManager content)
        {
            spriteRenderer = (SpriteRenderer)GameObject.GetComponent("SpriteRenderer");
            texture2D = content.Load<Texture2D>("CollisionTexture");
        }

        public void Update()
        {
            CheckCollision();
        }

        public void CheckCollision()
        {
            if (doCollisionCheck)
            {
                foreach (Collider other in GameWorld.Instance.Colliders)
                {
                    if (other != this)
                    {
                        if (CollisionBox.Intersects(other.CollisionBox))
                        {
                            GameObject.OnCollisionStay(other);
                            if (!otherColliders.Contains(other))
                            {
                                otherColliders.Add(other);
                                GameObject.OnCollisionEnter(other);
                            }
                        }
                        else if (otherColliders.Contains(other))
                        {
                            otherColliders.Remove(other);
                            GameObject.OnCollisionExit(other);
                        }
                    }
                }
            }
        }
    }
}
