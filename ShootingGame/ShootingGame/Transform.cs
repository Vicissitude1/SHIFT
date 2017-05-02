using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    class Transform : Component
    {
        Vector2 position;

        public Vector2 Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }
        public Transform(GameObject gameObject, Vector2 position) : base(gameObject)
        {
            this.position = position;

        }

        public void Translate(Vector2 translation)
        {
            position += translation;
        }
    }
}
