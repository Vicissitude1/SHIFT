using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShootingGame
{
    /// <summary>
    /// Represents the Enemy
    /// </summary>
    class Enemy : Component, ILoadable, IAnimateable, ICollisionEnter
    {
        /// <summary>
        /// The Enemy's movement speed
        /// </summary>
        int speed;

        /// <summary>
        /// The Enemy's translation
        /// </summary>
        Vector2 translation;

        /// <summary>
        /// The reference to the player's animator
        /// </summary>
        Animator animator;

        /// <summary>
        /// Checks if Enemy can move
        /// </summary>
        bool canMove;

        /// <summary>
        /// The Enemy's movement timer
        /// </summary>
        int moveTimer;
        
        /// <summary>
        /// The Enemy's shot sound effect
        /// </summary>
        SoundEffect effect;

        /// <summary>
        /// 
        /// </summary>
        int counter = 0;

        /// <summary>
        /// 
        /// </summary>
        int randomHolder;

        /// <summary>
        /// The Enemy's health level
        /// </summary>
        public int EnemyHealth { get; set; }

        /// <summary>
        /// The Enemy's thread
        /// </summary>
        public Thread T { get; private set; }

        /// <summary>
        /// The Enemy's constructor
        /// </summary>
        /// <param name="gameObject"></param>
        public Enemy(GameObject gameObject) : base(gameObject)
        {
            speed = 1;
            EnemyHealth = 100;
            T = new Thread(Update);
            T.IsBackground = true;
            canMove = true;
            moveTimer = GameWorld.Instance.Rnd.Next(100, 300);
            randomHolder = GameWorld.Instance.Rnd.Next(0, 4);
            counter += GameWorld.Instance.Rnd.Next(1, 21);
        }

        /// <summary>
        /// Updates the Enemy's functionality
        /// </summary>
        public void Update()
        {
            while(true)
            {
                Thread.Sleep(20);
                if (GameWorld.Instance.PlayGame && !GameWorld.Instance.StopGame)
                Move();
            }
        }

        /// <summary>
        /// Replaces the Enemy when the game is restarted
        /// </summary>
        public void Replace()
        {
            EnemyHealth = 100;
            counter = 0;
            moveTimer = GameWorld.Instance.Rnd.Next(100, 200);
            canMove = true;
            (GameObject.GetComponent("Collider") as Collider).DoCollisionCheck = true;

            if (GameWorld.Instance.Rnd.Next(2) == 0)
            {
                GameObject.Transform.Position = new Vector2(-50, GameWorld.Instance.Rnd.Next(100, 400));
                animator.PlayAnimation("WalkRight");

            }
            else
            {
                GameObject.Transform.Position = new Vector2(1350, GameWorld.Instance.Rnd.Next(100, 400));
                animator.PlayAnimation("WalkLeft");
            }
            (GameObject.GetComponent("Collider") as Collider).DoCollisionCheck = true;
        }

        /// <summary>
        /// The Enemy's movement
        /// </summary>
        public void Move()
        {
            if (EnemyHealth <= 0)
            {
                translation = Vector2.Zero;
                animator.PlayAnimation("Die");
            }

            else if (canMove)
            {
                translation = Vector2.Zero;
                if (EnemyHealth <= 0)
                {
                    animator.PlayAnimation("Die");
                }
                else if (counter <= 599 && EnemyHealth > 0)
                {
                    Walk();
                }
                else if (counter >= 600 && EnemyHealth > 0)
                {
                    if (counter == 750)
                    {
                        randomHolder = GameWorld.Instance.Rnd.Next(0, 4);
                    }
                    if (counter > 750)
                    {
                        Walk();
                        if (counter == 900)
                        {
                            randomHolder = GameWorld.Instance.Rnd.Next(0, 4);
                            counter = 600;
                        }
                    }
                    else
                    {
                        animator.PlayAnimation("Shoot");
                        counter++;
                    }
                }
                if (counter == 150 && EnemyHealth > 0 || counter == 300 && EnemyHealth > 0 || counter == 450 && EnemyHealth > 0)
                {
                    randomHolder = GameWorld.Instance.Rnd.Next(0, 4);
                }

            // Changes the sprite's size and enemy's movement speed according to the position
            (GameObject.GetComponent("SpriteRenderer") as SpriteRenderer).Scale = 1.5f - 400 / GameObject.Transform.Position.Y / 5;
                GameObject.Transform.Position += translation * speed * (GameObject.GetComponent("SpriteRenderer") as SpriteRenderer).Scale;
            }
        }

        /// <summary>
        /// Loads the Enemy's content
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            effect = content.Load<SoundEffect>("rifleshot");
            animator = (Animator)GameObject.GetComponent("Animator");
            CreateAnimation();
        }

        /// <summary>
        /// Creats the Enemy's animations
        /// </summary>
        public void CreateAnimation()
        {
            animator.CreateAnimation("WalkBack", new Animation(4, 68, 0, 35, 60, 12, Vector2.Zero));
            animator.CreateAnimation("WalkLeft", new Animation(4, 140, 0, 42, 60, 10, Vector2.Zero));
            animator.CreateAnimation("WalkRight", new Animation(4, 215, 0, 42, 60, 10, Vector2.Zero));
            animator.CreateAnimation("WalkFront", new Animation(4, 0, 0, 35, 60, 12, Vector2.Zero));
            animator.CreateAnimation("Shoot", new Animation(2, 285, 11, 35, 60, 5, Vector2.Zero));
            animator.CreateAnimation("Die", new Animation(5, 288, 0, 50, 60, 4, Vector2.Zero));
            animator.PlayAnimation("Shoot");
        }

        /// <summary>
        /// The functionality, when the animations "Die" and "Shoot" are done
        /// </summary>
        /// <param name="animationName"></param>
        public void OnAnimationDone(string animationName)
        {
            // Adds score to Player when the enemy dies
            if (animationName.Contains("Die"))
            {
                GameWorld.Instance.Scores.Add(new Score("+5", (GameObject.GetComponent("Transform") as Transform).Position, Color.White, GameWorld.Instance.CFont));
                Player.Scores += 5;
                Replace();
            }
            // Adds the new Enemybullet object to the game, when the enemy performs a shot
            else if (animationName.Contains("Shoot"))
            {
                effect.Play();
                GameWorld.Instance.EnemyBulletsPositions.Add(new Vector2(GameObject.Transform.Position.X + 5, GameObject.Transform.Position.Y + 10));
            }
        }

        /// <summary>
        /// Cheks if there is a collision with PLayerBullets
        /// </summary>
        /// <param name="other"></param>
        public void OnCollisionEnter(Collider other)
        {
            if (other.GameObject.GetComponent("PlayerBullet") is PlayerBullet)
            {
                // Reduces the enemy health by current Playerbullet's damage level value
                EnemyHealth -= (other.GameObject.GetComponent("PlayerBullet") as PlayerBullet).DamageLevel;

                // Stops the collision when Enemy is dead
                if (EnemyHealth < 0)
                {
                    EnemyHealth = 0;
                    (GameObject.GetComponent("Collider") as Collider).DoCollisionCheck = false;
                }
                // Makes sure that the PlayerBullet will be deleted from the game
                (other.GameObject.GetComponent("PlayerBullet") as PlayerBullet).IsRealesed = true;
            }
        }

        public void Walk()
        {
            switch (randomHolder)
            {
                case 0:
                    if (GameObject.Transform.Position.Y < 105)
                    {
                        translation += new Vector2(0, 1);
                        animator.PlayAnimation("WalkFront");
                        randomHolder = 1;
                        counter++;
                        break;
                    }
                    else
                    {
                        translation += new Vector2(0, -1);
                        animator.PlayAnimation("WalkBack");
                        counter++;
                        break;
                    }
                case 1:
                    if (GameObject.Transform.Position.Y > 395)
                    {
                        translation += new Vector2(0, -1);
                        animator.PlayAnimation("WalkBack");
                        randomHolder = 0;
                        counter++;
                        break;
                    }
                    else
                    {
                        translation += new Vector2(0, 1);
                        animator.PlayAnimation("WalkFront");
                        counter++;
                        break;
                    }
                case 2:
                    if (GameObject.Transform.Position.X < 5)
                    {
                        translation += new Vector2(1, 0);
                        animator.PlayAnimation("WalkRight");
                        randomHolder = 3;
                        counter++;
                        break;
                    }
                    else
                    {
                        translation += new Vector2(-1, 0);
                        animator.PlayAnimation("WalkLeft");
                        counter++;
                        break;
                    }
                case 3:
                    if (GameObject.Transform.Position.X > 1250)
                    {
                        translation += new Vector2(-1, 0);
                        animator.PlayAnimation("WalkLeft");
                        randomHolder = 2;
                        counter++;
                        break;
                    }
                    else
                    {
                        translation += new Vector2(1, 0);
                        animator.PlayAnimation("WalkRight");
                        counter++;
                        break;
                    }
                default:
                    counter++;
                    break;
            }
            //GameObject.Transform.Position += translation * speed;
        }
    }
}
