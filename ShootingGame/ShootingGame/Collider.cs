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
    /// Represents the Collider
    /// </summary>
    class Collider : Component, IDrawable, ILoadable, IUpdateable
    {
        /// <summary>
        /// A reference to the colliders texture
        /// </summary>
        Texture2D texture2D;

        /// <summary>
        /// A reference to the boxcolliders spriterenderer
        /// </summary>
        SpriteRenderer spriteRenderer;

        /// <summary>
        /// Indicates if this collider needs to check for collisions
        /// </summary>
        bool doCollisionCheck;

        List<Collider> otherColliders = new List<Collider>();

        
        public bool DoCollisionCheck
        {
            set
            {
                doCollisionCheck = value;
            }
        }

        /// <summary>
        /// The colliders collisionbox
        /// </summary>
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


        /// <summary>
        /// The Collider\s constructor
        /// </summary>
        /// <param name="gameObject"></param>
        public Collider(GameObject gameObject) : base(gameObject)
        {
            GameWorld.Instance.Colliders.Add(this);
            doCollisionCheck = true;
        }

        /// <summary>
        /// Draws the collision box
        /// </summary>
        /// <param name="spriteBatch"></param>
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

        /// <summary>
        /// Loads the content
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            spriteRenderer = (SpriteRenderer)GameObject.GetComponent("SpriteRenderer");
            texture2D = content.Load<Texture2D>("CollisionTexture");
        }

        /// <summary>
        /// Makes sure that CheckCollision method is called
        /// </summary>
        public void Update()
        {
            CheckCollision();
        }

        /// <summary>
        /// Checks the collision
        /// </summary>
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
