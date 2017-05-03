﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    class Explosion : Component, IUpdateable, ILoadable, IAnimateable
    {
        Animator animator;
        public static bool PlayAnimation { get; set; }

        public Explosion(GameObject gameObject) : base(gameObject)
        {
            PlayAnimation = false;
        }

        public void Update()
        {
            if (PlayAnimation)
            {
                GameObject.Transform.Position = new Vector2(Mouse.GetState().Position.X-64, Mouse.GetState().Position.Y-38);
                animator.PlayAnimation("Explode");
            }
            else animator.PlayAnimation("Idle");
        }

        public void LoadContent(ContentManager content)
        {
            this.animator = (Animator)GameObject.GetComponent("Animator");
            CreateAnimation();
        }

        public void CreateAnimation()
        {
            //animator.CreateAnimation("Explode", new Animation(7, 20, 0, 74, 40, 10, Vector2.Zero));
            animator.CreateAnimation("Explode", new Animation(3, 25, 0, 128, 65, 15, Vector2.Zero));
            animator.CreateAnimation("Idle", new Animation(1, 0, 0, 10, 110, 0, Vector2.Zero));
            animator.PlayAnimation("Idle");
        }

        public void OnAnimationDone(string animationName)
        {
            if (animationName.Contains("Explode"))
            {
                PlayAnimation = false;
            }
        }
    }
}
