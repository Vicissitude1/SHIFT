using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ShootingGame
{
    /// <summary>
    /// Represents the DiceBuilder
    /// </summary>
    class DiceBuilder : IBuilder
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
            gameObject.AddComponent(new SpriteRenderer(gameObject, "dice", 0));
            gameObject.AddComponent(new Dice(gameObject));
            gameObject.AddComponent(new Animator(gameObject));
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
