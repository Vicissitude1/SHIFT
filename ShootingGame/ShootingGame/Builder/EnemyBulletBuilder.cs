using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    /// <summary>
    /// Represents the EnemyBulletBuilder
    /// </summary>
    class EnemyBulletBuilder : IBuilder
    {
        /// <summary>
        /// The GameObject to build
        /// </summary>
        GameObject gameObject;

        /// <summary>
        /// Builds the GameObject
        /// </summary>
        /// <param name="position"></param>
        public void BuildGameObject(Vector2 position)
        {
            gameObject = new GameObject(position);
            gameObject.AddComponent(new SpriteRenderer(gameObject, "bullet1", 0.5f));
            //gameObject.AddComponent(new Animator(gameObject));
            gameObject.AddComponent(new EnemyBullet(gameObject));
            gameObject.AddComponent(new Collider(gameObject));
        }

        /// <summary>
        /// Returns the GameObject
        /// </summary>
        /// <returns></returns>
        public GameObject GetResult()
        {
            return gameObject;
        }
    }
}
