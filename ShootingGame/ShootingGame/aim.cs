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
        Vector2 translation;
        KeyboardState keyState;
        public Thread T { get; private set; }

        public Aim(GameObject gameObject) : base(gameObject)
        {
            T = new Thread(Update);
            T.IsBackground = true;
        }

        public void Update()
        {
            while(true)
            Move();
        }

        public void Move()
        {
            translation = Vector2.Zero;
            keyState = Keyboard.GetState();
            GameObject.Transform.Position = new Vector2(Mouse.GetState().Position.X-50, Mouse.GetState().Position.Y-50);
            //position.X = Mouse.GetState().Position.X;
        }
    }
}
