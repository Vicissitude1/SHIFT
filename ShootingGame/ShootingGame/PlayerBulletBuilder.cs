using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    /// <summary>
    /// Represents the PlayerBulletBuilder
    /// </summary>
    class PlayerBulletBuilder : IBuilder
    {
        GameObject gameObject;

        /// <summary>
        /// Builds the GameObject
        /// </summary>
        /// <param name="position"></param>
        public void BuildGameObject(Vector2 position)
        {
            gameObject = new GameObject(position);
            gameObject.AddComponent(new SpriteRenderer(gameObject, "bulletexpl", 0.5f));
            gameObject.AddComponent(new Animator(gameObject));
            gameObject.AddComponent(new PlayerBullet(gameObject));
            gameObject.AddComponent(new Collider(gameObject));
        }

        public GameObject GetResult()
        {
            return gameObject;
        }
    }
}
