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
    class PlayerBullet : Component
    {
        Vector2 direction;
        int speed;
        Vector2 translation;
        Vector2 aimPosition;
        public int DamageLevel { get; private set; }
        public Thread T { get; set; }
        public bool IsRealesed { get; set; }

        public PlayerBullet(GameObject gameObject) : base(gameObject)
        {
            speed = 10;
            DamageLevel = Player.CurrentWeapon.DamageLevel;
            IsRealesed = false;
            aimPosition = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);
            T = new Thread(Update);
            T.IsBackground = true;
            T.Start();
        }

        public void Update()
        {
            while(true)
            Move();
        }

        public void Move()
        {
            Thread.Sleep(20);
            translation = Vector2.Zero;
            translation += new Vector2(0, -1);
            GameObject.Transform.Position += translation * speed;
            
            if (GameObject.Transform.Position.Y < 85 || GameObject.Transform.Position.Y < aimPosition.Y)
                IsRealesed = true;

            if (IsRealesed)
            {
                GameWorld.Instance.ObjectsToRemove.Add(GameObject);
                T.Abort(); 
            }
                
        }
    }
}
