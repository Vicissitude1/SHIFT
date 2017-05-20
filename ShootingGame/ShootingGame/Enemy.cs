using Microsoft.Xna.Framework;
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
    enum Direction { Up, Down, Right, Left}

    class Enemy : Component, ILoadable, IAnimateable, ICollisionStay, ICollisionEnter, ICollisionExit
    {
        int speed;
        Vector2 translation;
        Vector2 mouseCurrentPosition;
        Animator animator;
        bool isSpawned;
        bool canMove;
        int moveTimer;
        int shootsAmount;
        Direction currentDirection;
        public int EnemyHealth { get; set; }
        public Thread T { get; private set; }

        public Enemy(GameObject gameObject) : base(gameObject)
        {
            isSpawned = true;
            speed = 2;
            EnemyHealth = 100;
            T = new Thread(Update);
            T.IsBackground = true;
            canMove = true;
            moveTimer = GameWorld.Instance.Rnd.Next(100, 300);
            shootsAmount = 2;
            if (GameObject.Transform.Position.X < 90)
                currentDirection = Direction.Right;
            else currentDirection = Direction.Left;
        }

        public void Update()
        {
            while(true)
            {
                Thread.Sleep(20);
                if (GameWorld.Instance.PlayGame && !GameWorld.Instance.StopGame)
                Move();
            }
        }

        public void Replace()
        {
            isSpawned = true;
            EnemyHealth = 100;
            moveTimer = GameWorld.Instance.Rnd.Next(100, 200);
            canMove = true;
            if (GameWorld.Instance.Rnd.Next(2) == 0)
            {
                GameObject.Transform.Position = new Vector2(-50, GameWorld.Instance.Rnd.Next(100, 400));
                currentDirection = Direction.Right;
                animator.PlayAnimation("WalkRight");
                
            }
            else
            {
                GameObject.Transform.Position = new Vector2(1350, GameWorld.Instance.Rnd.Next(100, 400));
                currentDirection = Direction.Right;
                animator.PlayAnimation("WalkLeft");
            }
            (GameObject.GetComponent("Collider") as Collider).DoCollisionCheck = true;
        }

        public void Move()
        {
            if (EnemyHealth <= 0)
            {
                translation = Vector2.Zero;
                animator.PlayAnimation("Die");
            }
                
            else if (canMove)
            {
                if(isSpawned)
                {
                    if (GameObject.Transform.Position.X > 100 && currentDirection == Direction.Right)
                        isSpawned = false;
                    else if (GameObject.Transform.Position.X < 1100 && currentDirection == Direction.Left)
                        isSpawned = false;
                }

                switch (currentDirection)
                {
                    case Direction.Down:
                        if (GameObject.Transform.Position.Y > 400)
                            currentDirection = Direction.Up;
                        translation = Vector2.Zero;
                        translation += new Vector2(0, 1);
                        animator.PlayAnimation("WalkFront");
                        break;
                    case Direction.Up:
                        if (GameObject.Transform.Position.Y < 100)
                            currentDirection = Direction.Down;
                        translation = Vector2.Zero;
                        translation += new Vector2(0, -1);
                        animator.PlayAnimation("WalkBack");
                        break;
                    case Direction.Left:
                        if (GameObject.Transform.Position.X < 100 && !isSpawned)
                            currentDirection = Direction.Right;
                        translation = Vector2.Zero;
                        translation += new Vector2(-1, 0);
                        animator.PlayAnimation("WalkLeft");
                        break;
                    default:
                        if (GameObject.Transform.Position.X > 1200 && !isSpawned)
                            currentDirection = Direction.Left;
                        translation = Vector2.Zero;
                        translation += new Vector2(1, 0);
                        animator.PlayAnimation("WalkRight");
                        break;
                }
                moveTimer--;

                if (moveTimer == 0)
                {
                    canMove = false;
                    shootsAmount = GameWorld.Instance.Rnd.Next(1,3);
                }        
            }
            else
            {
                translation = Vector2.Zero;
                animator.PlayAnimation("Shoot");

                if (shootsAmount == 0)
                {
                    UpdateDirection();
                    canMove = true;
                    moveTimer = GameWorld.Instance.Rnd.Next(20, 60);
                }
            }

                /*
                //A reference to the current keyboard state
                KeyboardState keyState = Keyboard.GetState();        
                //The current translation of the player
                //We are restting it to make sure that he stops moving if not keys are pressed
                translation = Vector2.Zero;
                //checks for input and adds it to the translation
                if (EnemyHealth <= 0)
                    animator.PlayAnimation("Die");

                else if (keyState.IsKeyDown(Keys.W))
                {
                    translation += new Vector2(0, -1);
                    animator.PlayAnimation("WalkBack");
                }
                else if (keyState.IsKeyDown(Keys.A))
                {
                    translation += new Vector2(-1, 0);
                    animator.PlayAnimation("WalkLeft");
                }
                else if (keyState.IsKeyDown(Keys.S))
                {
                    translation += new Vector2(0, 1);
                    animator.PlayAnimation("WalkFront");
                }
                else if (keyState.IsKeyDown(Keys.D))
                {
                    translation += new Vector2(1, 0);
                    animator.PlayAnimation("WalkRight");
                }
                else animator.PlayAnimation("Shoot");*/

                //animator.PlayAnimation("Shoot");
                //Move the player's gameobject framerate independent
            (GameObject.GetComponent("SpriteRenderer") as SpriteRenderer).Scale = 1.5f - 400 / GameObject.Transform.Position.Y / 5;
            GameObject.Transform.Position += translation * speed * (GameObject.GetComponent("SpriteRenderer") as SpriteRenderer).Scale;
        }

        public void UpdateDirection()
        {
            currentDirection = (Direction)GameWorld.Instance.Rnd.Next(5);
        }

        public void LoadContent(ContentManager content)
        {
            animator = (Animator)GameObject.GetComponent("Animator");
            CreateAnimation();
        }

        public void CreateAnimation()
        {
            animator.CreateAnimation("WalkBack", new Animation(4, 0, 6, 52, 60, 10, Vector2.Zero));
            animator.CreateAnimation("WalkLeft", new Animation(5, 140, 1, 50, 60, 10, Vector2.Zero));
            animator.CreateAnimation("WalkRight", new Animation(4, 140, 6, 53, 60, 10, Vector2.Zero));
            animator.CreateAnimation("WalkFront", new Animation(5, 0, 0, 40, 60, 10, Vector2.Zero));
            animator.CreateAnimation("Shoot", new Animation(3, 285, 10, 35, 60, 5, Vector2.Zero));
            animator.CreateAnimation("Die", new Animation(5, 288, 0, 50, 60, 4, Vector2.Zero));
            animator.PlayAnimation("Shoot");
        }

        public void OnAnimationDone(string animationName)
        {
            if (animationName.Contains("Die"))
            {
                GameWorld.Instance.Scores.Add(new Score("+5", (GameObject.GetComponent("Transform") as Transform).Position));
                Player.Scores += 5;
                Replace();
            }
            else if (animationName.Contains("Shoot"))
            {
                //if(Player.Health > 0)
                //Player.Health -= 1;
                shootsAmount--;
                GameWorld.Instance.EnemyBulletsPositions.Add(new Vector2(GameObject.Transform.Position.X + 5, GameObject.Transform.Position.Y + 10));
            }
        }

        public void OnCollisionStay(Collider other)
        {
            // (other.GameObject.GetComponent("SpriteRenderer") as SpriteRenderer).Color = Color.Red;
        }

        public void OnCollisionEnter(Collider other)
        {
            if (other.GameObject.GetComponent("PlayerBullet") is PlayerBullet)
            {
                EnemyHealth -= (other.GameObject.GetComponent("PlayerBullet") as PlayerBullet).DamageLevel;
                if (EnemyHealth < 0)
                {
                    EnemyHealth = 0;
                    (GameObject.GetComponent("Collider") as Collider).DoCollisionCheck = false;
                }
                (other.GameObject.GetComponent("PlayerBullet") as PlayerBullet).IsRealesed = true;
            }
            else if (other.GameObject.GetComponent("Enemy") is Enemy)
            {
                if(canMove)
                {
                    canMove = false;
                    shootsAmount = GameWorld.Instance.Rnd.Next(1, 3);
                }
            }
        }

        public void OnCollisionExit(Collider other)
        {
            //(other.GameObject.GetComponent("SpriteRenderer") as SpriteRenderer).Color = Color.White;
        }
    }
}
