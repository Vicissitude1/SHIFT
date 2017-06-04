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
    /// <summary>
    /// Represents the EnemyBullet
    /// </summary>
    class EnemyBullet : Component
    {
        /// <summary>
        /// The EnemyBullet's movement speed
        /// </summary>
        int speed;

        /// <summary>
        /// The EnemyBullet's translation
        /// </summary>
        Vector2 translation;

        /// <summary>
        /// The Enemybullet's aim position
        /// </summary>
        Vector2 aimPosition;

        /// <summary>
        /// The EnemyBullet thread
        /// </summary>
        public Thread T { get; set; }

        /// <summary>
        /// Checks if the EnemyBullet has to be deleted
        /// </summary>
        public bool IsRealesed { get; set; }

        /// <summary>
        /// The EnemyBullet's constructor
        /// </summary>
        /// <param name="gameObject"></param>
        public EnemyBullet(GameObject gameObject) : base(gameObject)
        {
            speed = 10;
            IsRealesed = false;
            aimPosition = new Vector2(Mouse.GetState().Position.X, 560);
            translation = aimPosition - GameObject.Transform.Position;
            translation.Normalize();
            T = new Thread(Update);
            T.IsBackground = true;
            T.Start();
        }

        /// <summary>
        /// Updates the EnemyBullet functionality
        /// </summary>
        public void Update()
        {
            while (true)
                Move();
        }

        /// <summary>
        /// Performs the EnemyBullet's movement
        /// </summary>
        public void Move()
        {
            // Aborts the thread, so it has to be deleted from the game
            if (IsRealesed) T.Abort();
            // Makes the movement speed faster when the game is over
            if (GameWorld.Instance.StopGame) speed = 50;

            Thread.Sleep(30);
            // Changes the EnemyBullet size according to the position
            (GameObject.GetComponent("SpriteRenderer") as SpriteRenderer).Scale = 1.2f - 400 / GameObject.Transform.Position.Y / 3;
            GameObject.Transform.Position += translation * speed;
            // makes sure that EnemyBullet will not move down, if Y-position is more than 550
            if (GameObject.Transform.Position.Y > 550)
                IsRealesed = true;
        }

        public void RestartThread(Vector2 position)
        {
            speed = 8;
            IsRealesed = false;
            aimPosition = new Vector2(Mouse.GetState().Position.X, 560);
            GameObject.Transform.Position = position;
            translation = aimPosition - GameObject.Transform.Position;
            translation.Normalize();
            T = new Thread(Update);
            T.IsBackground = true;
            T.Start();
        }
    }
}
