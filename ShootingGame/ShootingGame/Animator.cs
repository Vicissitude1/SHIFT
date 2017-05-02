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
        SpriteRenderer spriteRenderer;
        int currentIndex;
        float timeElapsed;
        float fps;
        Rectangle[] rectangles;
        string animationName;
        Dictionary<string, Animation> animations;

        public Animator(GameObject gameObject) : base(gameObject)
        {
            fps = 5;
            this.spriteRenderer = (SpriteRenderer)gameObject.GetComponent("SpriteRenderer");
            animations = new Dictionary<string, Animation>();
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
        public void CreateAnimation(string name, Animation animation)
        {
            animations.Add(name, animation);

        }
        public void PlayAnimation(string animationName)
        {
            //Checks if it’s a new animation
            if (this.animationName != animationName)
            {
                //Sets the rectangles
                this.rectangles = animations[animationName].Rectangles;
                //Resets the rectangle
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
