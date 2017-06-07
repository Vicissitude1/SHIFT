using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    /// <summary>
    /// Represents the EnemyObjectPool
    /// </summary>
    class EnemyBulletPool
    {
        private static List<GameObject> inactive = new List<GameObject>();

        private static List<GameObject> active = new List<GameObject>();

        private static Director director = new Director(new EnemyBulletBuilder());

        public static GameObject Create(Vector2 position, ContentManager content)
        {
            if (inactive.Count > 0)
            {
                GameObject enemyBullet = inactive[0];
                active.Add(enemyBullet);
                inactive.RemoveAt(0);
                
                return enemyBullet;
            }
            else
            {
                GameObject enemyBullet = director.Construct(position);
                enemyBullet.LoadContent(content);
                active.Add(enemyBullet);
                return enemyBullet;
            }
        }

        public static void ReleaseObject(GameObject enemyBullet)
        {
            CleanUp();
            inactive.Add(enemyBullet);
            active.Remove(enemyBullet);
        }

        private static void CleanUp()
        {
            //Reset data, remove references etc.
        }
    }
}
