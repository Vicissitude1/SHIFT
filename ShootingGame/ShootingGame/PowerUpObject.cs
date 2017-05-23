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
    enum PowerUpType { Health, Score, Ammo }

    class PowerUpObject : Component, ICollisionEnter, ILoadable
    {
        int speed;
        Vector2 translation;
        int inGameTimer;
        int outGameTimer;
        bool isInGame;
        int bonusValue;
        DataBaseClass database;
        bool isDefeat;
        PowerUpType currentPowerUp;
        SoundEffect effect;
        public string Name { get; private set; }
        public Thread T { get; private set; }

        public PowerUpObject(GameObject gameObject) : base(gameObject)
        {
            isDefeat = false;
            isInGame = false;
            inGameTimer = 0;
            outGameTimer = 200;
            speed = 5;
            database = new DataBaseClass();
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
            if (isDefeat)
            {
                bonusValue = database.GetBonusValue(currentPowerUp.ToString(), GameWorld.Instance.Rnd.Next(1,6));
                switch (currentPowerUp)
                {
                    case PowerUpType.Health:
                        Player.Health += bonusValue;
                        GameWorld.Instance.Scores.Add(new Score("Helath +" + bonusValue, (GameObject.GetComponent("Transform") as Transform).Position, Color.Yellow, GameWorld.Instance.BFont));
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
                GameObject.Transform.Position = new Vector2(GameObject.Transform.Position.X, -100);
                isInGame = false;
                isDefeat = false;
                outGameTimer = GameWorld.Instance.Rnd.Next(100, 300);
            }
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
            else
            {
                if (GameObject.Transform.Position.Y > -100)
                {
                    translation = Vector2.Zero;
                    translation += new Vector2(0, -1);
                }
                else translation = Vector2.Zero;

                outGameTimer--;
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
            GameObject.Transform.Position += translation * speed;
        }

        public void Replace()
        {
            GameObject.Transform.Position = new Vector2(200, -100);
            (GameObject.GetComponent("Collider") as Collider).DoCollisionCheck = true;
            isInGame = false;
            outGameTimer = 100;
        }

        public void OnCollisionEnter(Collider other)
        {
            if (other.GameObject.GetComponent("PlayerBullet") is PlayerBullet)
            {
                Player.Health += 5;
                (other.GameObject.GetComponent("PlayerBullet") as PlayerBullet).IsRealesed = true;
                (GameObject.GetComponent("Collider") as Collider).DoCollisionCheck = false;
                isDefeat = true;
            }
        }
    }
}
