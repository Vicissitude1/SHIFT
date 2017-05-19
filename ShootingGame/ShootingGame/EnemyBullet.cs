using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShootingGame
{
    class EnemyBullet : Component
    {
        Vector2 direction;
        int speed;
        Vector2 translation;
        Vector2 aimPosition;
        public Thread T { get; set; }
        public bool IsRealesed { get; set; }

        public EnemyBullet(GameObject gameObject) : base(gameObject)
        {
            speed = 8;
            IsRealesed = false;
            aimPosition = new Vector2(Mouse.GetState().Position.X, 560);
            translation = aimPosition - GameObject.Transform.Position;
            translation.Normalize();
            T = new Thread(Update);
            T.IsBackground = true;
            T.Start();
        }

        public void Update()
        {
            while (true)
                Move();
        }

        public void Move()
        {
            if (IsRealesed)
            {
                GameWorld.Instance.ObjectsToRemove.Add(GameObject);
                T.Abort();
            }
            Thread.Sleep(30);
            (GameObject.GetComponent("SpriteRenderer") as SpriteRenderer).Scale = 1.2f - 400 / GameObject.Transform.Position.Y / 3;
            GameObject.Transform.Position += translation * speed;

            //if (GameObject.Transform.Position.Y > 550)
            if (GameObject.Transform.Position.Y > 550 || GameWorld.Instance.StopGame || !GameWorld.Instance.PlayGame)
                IsRealesed = true;
        }
    }
}
