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
    class Score
    {
        float lifeTimer;
        string bonus;
        Vector2 position;
        float speed;
        Vector2 translation;
        Thread t;

        public Score(string bonus, Vector2 position)
        {
            this.bonus = bonus;
            this.position = position;
            lifeTimer = 2000;
            speed = 100;
            t = new Thread(Move);
            t.IsBackground = true;
            t.Start();
        }

        public void Move()
        {
            while(true)
            {
                translation = Vector2.Zero;
                translation += new Vector2(0, -1);
                position += translation * speed / 20;
                lifeTimer -= 100;

                if (lifeTimer <= 0)
                {
                    GameWorld.Instance.ScoresToRemove.Add(this);
                    t.Abort();
                }
                Thread.Sleep(100);
            } 
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(GameWorld.Instance.BFont, bonus, position, Color.Yellow);
        }
    }
}
