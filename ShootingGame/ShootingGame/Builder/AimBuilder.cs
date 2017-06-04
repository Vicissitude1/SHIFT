using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    /// <summary>
    /// Represents the AimBuilder
    /// </summary>
    class AimBuilder : IBuilder
    {
        /// <summary>
        /// The component's parent GameObject
        /// </summary>
        GameObject gameObject;

        /// <summary>
        /// Builds the GameObject
        /// </summary>
        /// <param name="position"></param>
        public void BuildGameObject(Vector2 position)
        {
            gameObject = new GameObject(position);
            gameObject.AddComponent(new SpriteRenderer(gameObject, "SHIFT Crosshair", 0.9f));
            gameObject.AddComponent(new Aim(gameObject));
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
