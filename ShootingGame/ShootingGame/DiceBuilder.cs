using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ShootingGame
{
    class DiceBuilder : IBuilder
    {
        GameObject gameObject;

        public void BuildGameObject(Vector2 position)
        {
            gameObject = new GameObject(position);
            gameObject.AddComponent(new SpriteRenderer(gameObject, "dice", 0));
            gameObject.AddComponent(new Dice(gameObject));
        }

        public GameObject GetResult()
        {
            return gameObject;
        }
    }
}
