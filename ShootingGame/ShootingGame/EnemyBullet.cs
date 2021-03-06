﻿using Microsoft.Xna.Framework;
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
            speed = 10;
            IsRealesed = false;
            aimPosition = new Vector2(Mouse.GetState().Position.X, 560);
            //direction = aimPosition - GameObject.Transform.Position;
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
            Thread.Sleep(20);
            translation = Vector2.Zero;
            //translation += direction;
            translation += new Vector2(0, 1);
            //translation = aimPosition - GameObject.Transform.Position;
            GameObject.Transform.Position += translation * speed;

            if (GameObject.Transform.Position.Y > 550)
                IsRealesed = true;

            if (IsRealesed)
            {
                GameWorld.Instance.ObjectsToRemove.Add(GameObject);
                T.Abort();
            }

        }
    }
}
