using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShootingGame
{
    /// <summary>
    /// Represents the Score
    /// </summary>
    class Score
    {
        /// <summary>
        /// The Score's motion speed
        /// </summary>
        int speed;

        /// <summary>
        /// The Score's timer
        /// </summary>
        int lifeTimer;

        /// <summary>
        /// The Player's bonus
        /// </summary>
        string bonus;

        /// <summary>
        /// The Score's position
        /// </summary>
        Vector2 position;

        /// <summary>
        /// The Score's font color
        /// </summary>
        Color color;

        /// <summary>
        /// The Score's font
        /// </summary>
        SpriteFont font;

        /// <summary>
        /// The Score's translation
        /// </summary>
        Vector2 translation;

        /// <summary>
        /// The Score's thread
        /// </summary>
        public Thread T { get; private set; }

        /// <summary>
        /// The Score's constructor
        /// </summary>
        /// <param name="bonus">The Player's bonus</param>
        /// <param name="position">The score's start position</param>
        /// <param name="color">Score's font color</param>
        /// <param name="font">Score's font</param>
        public Score(string bonus, Vector2 position, Color color, SpriteFont font)
        {
            this.bonus = bonus;
            this.position = position;
            this.color = color;
            this.font = font;
            lifeTimer = 2000;
            speed = 100;
            T = new Thread(Move);
            T.IsBackground = true;
            T.Start();
        }

        /// <summary>
        /// The motion functionality
        /// </summary>
        public void Move()
        {
            while(true)
            {
                translation = Vector2.Zero;
                translation += new Vector2(0, -1);
                position += translation * speed / 20;
                lifeTimer -= 100;
                if (lifeTimer <= 0) T.Abort();
                Thread.Sleep(100);
            } 
        }

        /// <summary>
        /// Draws the Score
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, bonus, position, color);
        }
    }
}
