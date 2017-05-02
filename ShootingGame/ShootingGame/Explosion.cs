using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    class Explosion : Component, IUpdateable, ILoadable, IAnimateable
    {
        Vector2 translation;
        Animator animator;
        float timer;

        public Explosion(GameObject gameObject) : base(gameObject)
        {
            timer = 1;
        }

        public void Update()
        {
            //timer -= GameWorld.Instance.DeltaTime;
            if(timer < 0)
            {
                GameWorld.Instance.ObjectsToRemove.Add(GameObject);
            }
            animator.PlayAnimation("IdleFront");
        }

        public void LoadContent(ContentManager content)
        {
            this.animator = (Animator)GameObject.GetComponent("Animator");
            CreateAnimation();
        }

        public void CreateAnimation()
        {
            animator.CreateAnimation("IdleFront", new Animation(5, 0, 0, 125, 110, 6, Vector2.Zero));
            animator.PlayAnimation("IdleFront");
        }

        public void OnAnimationDone(string animationName)
        {
            
        }
    }
}
