using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    class EnemyBulletBuilder : IBuilder
    {
        GameObject gameObject;

        public void BuildGameObject(Vector2 position)
        {
            gameObject = new GameObject(position);
            gameObject.AddComponent(new SpriteRenderer(gameObject, "bullet", 0.5f));
            //gameObject.AddComponent(new Animator(gameObject));
            gameObject.AddComponent(new EnemyBullet(gameObject));
            gameObject.AddComponent(new Collider(gameObject));
        }

        public GameObject GetResult()
        {
            return gameObject;
        }
    }
}
