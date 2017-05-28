using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    /// <summary>
    /// Represents the Component
    /// </summary>
    abstract class Component
    {
        /// <summary>
        /// This component's parent GameObject
        /// </summary>
        private GameObject gameObject;

        public GameObject GameObject
        {
            get
            {
                return gameObject;
            }
        }

        /// <summary>
        /// The components constructor
        /// </summary>
        /// <param name="gameObject">The parent GameObject</param>
        public Component(GameObject gameObject)
        {
            //Sets the parent
            this.gameObject = gameObject;
        }
        public Component()
        {

        }
    }
}
