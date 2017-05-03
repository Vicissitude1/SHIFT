using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    class AimBuilder : IBuilder
    {
        GameObject gameObject;

        public void BuildGameObject(Vector2 position)
        {
            gameObject = new GameObject(position);
            gameObject.AddComponent(new SpriteRenderer(gameObject, "SHIFT Crosshair", 0));
            gameObject.AddComponent(new Aim(gameObject));
        }

        public GameObject GetResult()
        {
            return gameObject;
        }
    }
}
