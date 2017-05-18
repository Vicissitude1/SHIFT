using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShootingGame
{
    class Aim : Component
    {
        Vector2 mouseCurrentPosition;
        public Thread T { get; private set; }

        public Aim(GameObject gameObject) : base(gameObject)
        {
            T = new Thread(Update);
            T.IsBackground = true;
        }

        public void Update()
        {
            while(true)
            {
                Thread.Sleep(20);
                if (GameWorld.Instance.PlayGame && !GameWorld.Instance.StopGame)
                    Move();
            }
        }

        public void Move()
        {
            if (Mouse.GetState().Position.Y <= 450)
                mouseCurrentPosition = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);
            else mouseCurrentPosition = new Vector2(Mouse.GetState().Position.X, 450);

            GameObject.Transform.Position = new Vector2(mouseCurrentPosition.X - 50, mouseCurrentPosition.Y - 50);
        }
    }
}
