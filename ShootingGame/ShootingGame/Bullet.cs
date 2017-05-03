using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    class Bullet : Component, IUpdateable
    {
        private Vector2 direction;
        private Vector2 position;
        int speed;
        Vector2 translation;

        public Bullet(GameObject gameObject) : base(gameObject)
        {
            speed = 200;
            Vector2 p = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);
            direction = new Vector2(p.X, p.Y) - this.position;
        }

        public void Update()
        {
            Move();
        }

        public void Move()
        {
            translation = Vector2.Zero;
            translation += new Vector2(1, 0);
            GameObject.Transform.Translate(translation * GameWorld.Instance.DeltaTime * speed);
        }
    }
}
