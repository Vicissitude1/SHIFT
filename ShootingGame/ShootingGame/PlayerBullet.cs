using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShootingGame
{
    class PlayerBullet : Component, ILoadable, IAnimateable
    {
        int speed;
        bool animationDone;
        Vector2 translation;
        Vector2 aimPosition;
        Animator animator;
        public int DamageLevel { get; private set; }
        public Thread T { get; set; }
        public bool IsRealesed { get; set; }

        public PlayerBullet(GameObject gameObject) : base(gameObject)
        {
            speed = 10;
            DamageLevel = Player.CurrentWeapon.DamageLevel;
            IsRealesed = false;
            animationDone = false;
            aimPosition = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);
            T = new Thread(Update);
            T.IsBackground = true;
            T.Start();
        }

        public void Update()
        {
            while (true)
                Move();
        }

        public void Move()
        {
            if (animationDone)
            {
                GameWorld.Instance.ObjectsToRemove.Add(GameObject);
                T.Abort();
            }
            Thread.Sleep(30);
            
            (GameObject.GetComponent("SpriteRenderer") as SpriteRenderer).Scale = 1.2f - 400 / GameObject.Transform.Position.Y / 3;
            translation = Vector2.Zero;
            translation += new Vector2(0, -1);
            GameObject.Transform.Position += translation * speed;

            if (GameWorld.Instance.StopGame)
            {
                if (GameObject.Transform.Position.Y < 120 || GameObject.Transform.Position.Y <= aimPosition.Y)
                {
                    speed = 0;
                    animationDone = true;
                }
                else speed = 20;
            }
            else if (IsRealesed)
            {
                speed = GameObject.Transform.Position.Y < 120 ? 0 : 1;
                animator.PlayAnimation("Expl");
            }
            else if (GameObject.Transform.Position.Y < 120 || GameObject.Transform.Position.Y < aimPosition.Y)
            {
                IsRealesed = true;
            }
        }

        public void LoadContent(ContentManager content)
        {
            animator = (Animator)GameObject.GetComponent("Animator");
            CreateAnimation();
        }

        public void CreateAnimation()
        {
            animator.CreateAnimation("Idle", new Animation(1, 0, 0, 15, 15, 1, Vector2.Zero));
            animator.CreateAnimation("Expl", new Animation(3, 19, 0, 16, 20, 15, Vector2.Zero));
            animator.PlayAnimation("Idle");
        }

        public void OnAnimationDone(string animationName)
        {
            if (animationName.Contains("Expl"))
                animationDone = true;
        }
    }
}
