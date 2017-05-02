using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    class Aim : Component, IUpdateable
    {
        Vector2 translation;
        KeyboardState keyState;

        public Aim(GameObject gameObject) : base(gameObject)
        {

        }

        public void Update()
        {
            Move();
        }

        public void Move()
        {
            translation = Vector2.Zero;
            keyState = Keyboard.GetState();
            GameObject.Transform.Position = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);
            //position.X = Mouse.GetState().Position.X;
        }
    }
}
