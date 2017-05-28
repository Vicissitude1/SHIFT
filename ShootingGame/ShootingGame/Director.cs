using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    /// <summary>
    /// Represents the Director
    /// </summary>
    class Director
    {
        IBuilder builder;

        /// <summary>
        /// The Director's constructor
        /// </summary>
        /// <param name="builder"></param>
        public Director(IBuilder builder)
        {
            this.builder = builder;
        }

        /// <summary>
        /// Creates the GameObject
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public GameObject Construct(Vector2 position)
        {
            builder.BuildGameObject(position);
            return builder.GetResult();
        }
    }
}
