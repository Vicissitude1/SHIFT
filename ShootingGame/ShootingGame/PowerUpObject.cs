using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShootingGame
{
    /// <summary>
    /// Types of powerup
    /// </summary>
    enum PowerUpType { Health, Score, Ammo }

    /// <summary>
    /// Represents the PowerUpObject
    /// </summary>
    class PowerUpObject : Component, ICollisionEnter, ILoadable
    {
        /// <summary>
        /// The PowerUpObject's movement speed
        /// </summary>
        int speed;

        /// <summary>
        /// The PowerUpObject's ingame timer
        /// </summary>
        int inGameTimer;

        /// <summary>
        /// The PowerUpObject's outgame timer
        /// </summary>
        int outGameTimer;

        /// <summary>
        /// The player's bonus value
        /// </summary>
        int bonusValue;

        /// <summary>
        /// Checks if PowerUpObject is in game
        /// </summary>
        bool isInGame;

        /// <summary>
        /// Checks if the PowerUpObject's time is expired
        /// </summary>
        bool isDefeat;

        /// <summary>
        /// The type of PowerUpObject
        /// </summary>
        PowerUpType currentPowerUp;

        /// <summary>
        /// The PowerUpObject's sound effect
        /// </summary>
        SoundEffect effect;

        /// <summary>
        /// The PowerUpObject's translation
        /// </summary>
        Vector2 translation;

        /// <summary>
        /// The name of bonus
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The PowerUpObject's thread
        /// </summary>
        public Thread T { get; private set; }

        /// <summary>
        /// The PowerUpObject's constructor
        /// </summary>
        /// <param name="gameObject"></param>
        public PowerUpObject(GameObject gameObject) : base(gameObject)
        {
            isDefeat = false;
            isInGame = false;
            inGameTimer = 0;
            outGameTimer = 500;
            speed = 5;
            T = new Thread(Update);
            T.IsBackground = true;
            currentPowerUp = (PowerUpType)GameWorld.Instance.Rnd.Next(3);
            Name = currentPowerUp.ToString().Substring(0, 1);
        }

        /// <summary>
        /// Loads the PowerUpObject's content
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            effect = content.Load<SoundEffect>("powerUp");
        }

        /// <summary>
        /// Updates the PowerUpObject's movement
        /// </summary>
        public void Update()
        {
            while (true)
            {
                Thread.Sleep(30);
                if (GameWorld.Instance.PlayGame && !GameWorld.Instance.StopGame)
                    Move();
            }
        }

        public void Move()
        {
            // When the PowerUpObject is hit
            if (isDefeat)
            {
                // Gets from data base a corresponding value
                bonusValue = DataBaseClass.Instance.GetBonusValue(currentPowerUp.ToString(), GameWorld.Instance.Rnd.Next(1,6));
                // Gives to the player bonus
                switch (currentPowerUp)
                {
                    case PowerUpType.Health:
                        Player.Health += bonusValue;
                        GameWorld.Instance.Scores.Add(new Score("Health +" + bonusValue, (GameObject.GetComponent("Transform") as Transform).Position, Color.Yellow, GameWorld.Instance.BFont));
                        break;
                    case PowerUpType.Score:
                        Player.Scores += bonusValue;
                        GameWorld.Instance.Scores.Add(new Score("Score +" + bonusValue, (GameObject.GetComponent("Transform") as Transform).Position, Color.Yellow, GameWorld.Instance.BFont));
                        break;
                    default:
                        Player.CurrentWeapon.TotalAmmo += bonusValue;
                        GameWorld.Instance.Scores.Add(new Score("Ammo +" + bonusValue, (GameObject.GetComponent("Transform") as Transform).Position, Color.Yellow, GameWorld.Instance.BFont));
                        break;
                }
                effect.Play();
                // Replaces the PowerUpObject
                Replace();
            }
            // Performs the PowerUpObject's movement when it is ingame
            else if (isInGame)
            {
                if (GameObject.Transform.Position.Y < 150)
                {
                    translation = Vector2.Zero;
                    translation += new Vector2(0, 1);
                }
                else translation = Vector2.Zero;

                inGameTimer--;

                if (inGameTimer <= 0)
                {
                    isInGame = false;
                    outGameTimer = GameWorld.Instance.Rnd.Next(100, 300);
                }
            }
            // Moves the PowerUpObject up if it was not hit 
            else
            {
                if (GameObject.Transform.Position.Y > -100)
                {
                    translation = Vector2.Zero;
                    translation += new Vector2(0, -1);
                }
                else translation = Vector2.Zero;

                outGameTimer--;

                // Replaces the PowerUpObject when the outgame timer is expired
                if (outGameTimer <= 0)
                {
                    isInGame = true;
                    inGameTimer = 200;
                    GameObject.Transform.Position = new Vector2(GameWorld.Instance.Rnd.Next(100, 1200), -100);
                    (GameObject.GetComponent("Collider") as Collider).DoCollisionCheck = true;
                    currentPowerUp = (PowerUpType)GameWorld.Instance.Rnd.Next(3);
                    Name = currentPowerUp.ToString().Substring(0, 1);
                }
            }
            // Performs the PowerUpObject's movement
            GameObject.Transform.Position += translation * speed;
        }

        /// <summary>
        /// Replaces the PowerUpObject when it is hit or the game is restarted
        /// </summary>
        public void Replace()
        {
            GameObject.Transform.Position = new Vector2(100, -100);
            isInGame = false;
            isDefeat = false;
            outGameTimer = 500;
        }

        /// <summary>
        /// Makes sure that the PowerUpObject is getting hit by collided player's bullet and the corresponding bullet will be deleted
        /// </summary>
        /// <param name="other"></param>
        public void OnCollisionEnter(Collider other)
        {
            // Only if the PowerUpObject is colliding with player's bullet
            if (other.GameObject.GetComponent("PlayerBullet") is PlayerBullet)
            {
                // Makes sure that PlayerBullet will be deleted from the game
                (other.GameObject.GetComponent("PlayerBullet") as PlayerBullet).IsRealesed = true;

                // Stops the collision check
                (GameObject.GetComponent("Collider") as Collider).DoCollisionCheck = false;

                // Makes sure that PoawerUpObject will be replaced
                isDefeat = true;
            }
        }
    }
}
