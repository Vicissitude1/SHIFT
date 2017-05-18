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
    class Enemy : Component, ILoadable, IAnimateable, IUpdateable
    {
        int speed;
        Vector2 translation;
        Animator animator;
        float timer;
        int counter = 0;
        Random rnd = new Random();
        int randomHolder = 0;
        public int EnemyHealth { get; set; }
        public Thread T { get; private set; }


        public Enemy(GameObject gameObject) : base(gameObject)
        {
            timer = 0;
            speed = 1;
            EnemyHealth = 100;
            T = new Thread(UpdateHealth);
            T.IsBackground = true;
            randomHolder = rnd.Next(0, 4);
        }

        public void Update()
        {
            UpdateHealth();
            Move();
            Thread.Sleep(10);
        }

        public void Move()
        {
            translation = Vector2.Zero;
            if (EnemyHealth <= 0)
            {
            animator.PlayAnimation("Die");
            }
            else if (counter<=599 && EnemyHealth > 0)
            {
                switch (randomHolder)
                {
                    case 0:
                        translation += new Vector2(0, -1);
                        animator.PlayAnimation("WalkBack");
                        counter++;
                        break;
                    case 1:
                        translation += new Vector2(0, 30);
                        animator.PlayAnimation("WalkFront");
                        counter++;
                        break;
                    case 2:
                        translation += new Vector2(-1, 0);
                        animator.PlayAnimation("WalkLeft");
                        counter++;
                        break;
                    case 3:
                        translation += new Vector2(1, 0);
                        animator.PlayAnimation("WalkRight");
                        counter++;
                        break;
                    default:
                        counter++;
                        break;
                }
                GameObject.Transform.Position += translation * speed;
            }
            else if (counter >= 600 && EnemyHealth > 0)
            {
                animator.PlayAnimation("Shoot");
            }
            if (counter==150 && EnemyHealth > 0 || counter==300 && EnemyHealth > 0 || counter==450 && EnemyHealth > 0)
            {
                randomHolder = rnd.Next(0, 4);
            }


            /*
                //A reference to the current keyboard state
                KeyboardState keyState = Keyboard.GetState();
            
            //The current translation of the player
            //We are resetting it to make sure that he stops moving if not keys are pressed
            translation = Vector2.Zero;


            //checks for input and adds it to the translation
            /*
            else if (keyState.IsKeyDown(Keys.W))
            {
                translation += new Vector2(0, -1);
                animator.PlayAnimation("WalkBack");
            }
            else if (keyState.IsKeyDown(Keys.A))
            {
                translation += new Vector2(-1, 0);
                animator.Pltranslation += new Vector2(0, 1);
                animator.PlayAnimation("WalkFront");ayAnimation("WalkLeft");
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
            }*/
            /*
            else animator.PlayAnimation("Shoot");

            //animator.PlayAnimation("Shoot");
            //Move the player's gameobject framerate independent
            //GameObject.Transform.Translate(translation * speed * GameWorld.Instance.DeltaTime);
            //GameObject.Transform.Position += translation * speed * (GameObject.GetComponent("SpriteRenderer") as SpriteRenderer).Scale;
            //GameObject.Transform.Position += translation * speed;
             */
        }

        public void UpdateHealth()
        {
            while(true)
            {

                if (Mouse.GetState().LeftButton == ButtonState.Pressed && Mouse.GetState().Position.X >= GameObject.Transform.Position.X && Mouse.GetState().Position.Y >= GameObject.Transform.Position.Y)
                {
                    //if(Mouse.GetState().Position.X <= (GameObject.Transform.Position.X + (GameObject.GetComponent("SpriteRenderer") as SpriteRenderer).Sprite.Width) && Mouse.GetState().Position.Y <= (GameObject.Transform.Position.Y + (GameObject.GetComponent("SpriteRenderer") as SpriteRenderer).Sprite.Height))
                    if (Mouse.GetState().Position.X <= (GameObject.Transform.Position.X + 40) && Mouse.GetState().Position.Y <= (GameObject.Transform.Position.Y + 60))
                    {
                        //timer += GameWorld.Instance.DeltaTime;
                        if (EnemyHealth > 0)
                        {
                            //Thread.Sleep(100);
                            EnemyHealth -= 10;
                            //timer = 0;
                        }
                    }
                }
            }
        }

        public void LoadContent(ContentManager content)
        {
            animator = (Animator)GameObject.GetComponent("Animator");
            CreateAnimation();
        }

        public void CreateAnimation()
        {
            animator.CreateAnimation("WalkBack", new Animation(1, 0, 8, 33, 60, 6, Vector2.Zero));
            animator.CreateAnimation("WalkLeft", new Animation(5, 140, 1, 50, 60, 10, Vector2.Zero));
            animator.CreateAnimation("WalkRight", new Animation(4, 140, 7, 53, 60, 10, Vector2.Zero));
            animator.CreateAnimation("WalkFront", new Animation(5, 0, 0, 40, 60, 10, Vector2.Zero));
            animator.CreateAnimation("Shoot", new Animation(3, 285, 10, 35, 60, 6, Vector2.Zero));
            animator.CreateAnimation("Die", new Animation(5, 288, 0, 50, 60, 4, Vector2.Zero));
            animator.PlayAnimation("Shoot");
        }

        public void OnAnimationDone(string animationName)
        {
            if (animationName.Contains("Die"))
            {
                EnemyHealth = 100;
                counter = 0;
                GameObject.Transform.Position = new Vector2(GameWorld.Instance.WindowEdges[rnd.Next(0, 2)], GameWorld.Instance.Rnd.Next(100, 400));
            }
            else if (animationName.Contains("Shoot"))
            {
                if(Player.Health > 0)
                Player.Health -= 1;
            }
        }
    }
}
