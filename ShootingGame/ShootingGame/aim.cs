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
    /// <summary>
    /// Represents the Aim
    /// </summary>
    class Aim : Component
    {
        /// <summary>
        /// Mouse's position
        /// </summary>
        Vector2 mouseCurrentPosition;

        /// <summary>
        /// The Aim's thread
        /// </summary>
        public Thread T { get; private set; }

        /// <summary>
        /// The Aim's constructor
        /// </summary>
        /// <param name="gameObject"></param>
        public Aim(GameObject gameObject) : base(gameObject)
        {
            T = new Thread(Update);
            T.IsBackground = true;
        }

        /// <summary>
        /// Updates the Aim's functioanility
        /// </summary>
        public void Update()
        {
            while(true)
            {
                Thread.Sleep(20);
                if (GameWorld.Instance.PlayGame && !GameWorld.Instance.StopGame)
                    Move();
            }
        }

        /// <summary>
        /// Performs the Aim's movement
        /// </summary>
        public void Move()
        {
            if (Mouse.GetState().Position.Y <= 450)
                mouseCurrentPosition = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);
            else mouseCurrentPosition = new Vector2(Mouse.GetState().Position.X, 450);

            GameObject.Transform.Position = new Vector2(mouseCurrentPosition.X - 50, mouseCurrentPosition.Y - 50);
        }
    }
}
