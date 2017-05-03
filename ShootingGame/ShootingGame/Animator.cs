using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    class Animator : Component, IUpdateable
    {
        /// <summary>
        /// The current index of the animation
        /// </summary>
        private int currentIndex;

        /// <summary>
        /// Time elapsed for the current animation
        /// </summary>
        private float timeElapsed;

        /// <summary>
        /// The framerate of the animation
        /// </summary>
        private float fps;

        /// <summary>
        /// The rectangle on the spritesheet
        /// </summary>
        private Rectangle[] rectangles;

        /// <summary>
        /// A reference to the spriteRenderer
        /// </summary>
        private SpriteRenderer spriteRenderer;

        public Dictionary<string, Animation> animations;

        private string animationName;

        public Animator(GameObject gameObject) : base(gameObject)
        {
            fps = 5;

            this.spriteRenderer = (SpriteRenderer)gameObject.GetComponent("SpriteRenderer");

            this.animations = new Dictionary<string, Animation>();

        }

        public void Update()
        {
            timeElapsed += GameWorld.Instance.DeltaTime;

            currentIndex = (int)(timeElapsed * fps);

            if (currentIndex > rectangles.Length - 1)
            {
                GameObject.OnAnimationDone(animationName);
                timeElapsed = 0;
                currentIndex = 0;
            }

            spriteRenderer.Rectangle = rectangles[currentIndex];
        }

        /// <summary>
        /// Adds a new animation
        /// </summary>
        /// <param name="name">Animation name</param>
        /// <param name="animation">The animation to add</param>
        public void CreateAnimation(string name, Animation animation)
        {
            //Adds a new animation to the dictionary
            animations.Add(name, animation);
        }

        /// <summary>
        /// Plays an animation
        /// </summary>
        /// <param name="animationName">Name of animation to play</param>
        public void PlayAnimation(string animationName)
        {
            //Checks if the animation is player
            if (this.animationName != animationName)
            {
                //Sets the rectangles
                this.rectangles = animations[animationName].Rectangles;

                //Sets the size of the rectangle
                this.spriteRenderer.Rectangle = rectangles[0];

                //Sets the offset
                this.spriteRenderer.Offset = animations[animationName].Offset;

                //Sets the animation name
                this.animationName = animationName;

                //Sets the fps
                this.fps = animations[animationName].Fps;

                //Resets the animation
                timeElapsed = 0;

                currentIndex = 0;
            }
        }
    }
}
