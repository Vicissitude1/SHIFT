using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    class PlayerBuilder
    {
        GameObject gameObject;

        public void BuildGameObject(Vector2 position)
        {
            gameObject = new GameObject(position);
            gameObject.AddComponent(new SpriteRenderer(gameObject, "dragon", 0));
            gameObject.AddComponent(new Animator(gameObject));
            gameObject.AddComponent(new Player(gameObject));
            gameObject.AddComponent(new Collider(gameObject));
        }

        public GameObject GetResult()
        {
            return gameObject;
        }
    }
}
