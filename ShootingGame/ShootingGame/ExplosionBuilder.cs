using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    class ExplosionBuilder : IBuilder
    {
        GameObject gameObject;

        public void BuildGameObject(Vector2 position)
        {
            gameObject = new GameObject(position);
            gameObject.AddComponent(new SpriteRenderer(gameObject, "expl", 0.8f));
            gameObject.AddComponent(new Animator(gameObject));
            gameObject.AddComponent(new Explosion(gameObject));
        }

        public GameObject GetResult()
        {
            return gameObject;
        }
    }
}
