using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using System;
using Microsoft.Xna.Framework.Media;

namespace ShootingGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameWorld : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        /// <summary>
        /// The object that is going to be locked
        /// </summary>
        static object thisLock = new object();
        Director director;
        private SpriteFont resultFont;
        List<GameObject> gameObjects;
        List<GameObject> objectsToRemove;
        List<Collider> colliders;
        List<Collider> collidersToRemove;
        List<Score> scores;
        List<Score> scoresToRemove;
        Texture2D background;
        Texture2D sky;
        Texture2D grass;
        private SoundEffect effect;
        private static GameWorld instance;
        bool playSound;
        public bool CanAddPlayerBollet { get; set; }
        public float DeltaTime { get; private set; }
        public SpriteFont AFont { get; private set; }
        public SpriteFont BFont { get; private set; }
        public SpriteFont CFont { get; private set; }
        public Random Rnd { get; private set; }
        internal List<GameObject> ObjectsToAdd { get; set; }
        internal List<Vector2> EnemyBulletsPositions { get; set; }
        public int Result { get; set; }
        internal List<Collider> Colliders
        {
            get
            {
                return colliders;
            }
        }
        public static GameWorld Instance
        {
            get { return instance ?? (instance = new GameWorld()); }
        }

        internal List<GameObject> ObjectsToRemove
        {
            get
            {
                return objectsToRemove;
            }

            set
            {
                objectsToRemove = value;
            }
        }

        internal List<Score> Scores
        {
            get
            {
                return scores;
            }

            set
            {
                scores = value;
            }
        }

        internal List<Score> ScoresToRemove
        {
            get
            {
                return scoresToRemove;
            }

            set
            {
                scoresToRemove = value;
            }
        }

        private GameWorld()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1300;
            graphics.PreferredBackBufferHeight = 700;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Rnd = new Random();
            playSound = false;
            CanAddPlayerBollet = false;
            gameObjects = new List<GameObject>();
            objectsToRemove = new List<GameObject>();
            colliders = new List<Collider>();
            collidersToRemove = new List<Collider>();
            scores = new List<Score>();
            scoresToRemove = new List<Score>();
            ObjectsToAdd = new List<GameObject>();
            EnemyBulletsPositions = new List<Vector2>();

            director = new Director(new EnemyBuilder());
            gameObjects.Add(director.Construct(new Vector2(500, 100)));
            /*
            director = new Director(new EnemyBuilder());
            gameObjects.Add(director.Construct(new Vector2(500, 200)));
            director = new Director(new EnemyBuilder());
            gameObjects.Add(director.Construct(new Vector2(500, 300)));
            director = new Director(new EnemyBuilder());
            gameObjects.Add(director.Construct(new Vector2(500, 400)));

            director = new Director(new DiceBuilder());
            GameObject d1 = director.Construct(new Vector2(650, 600));
            Dice dice1 = (Dice)d1.GetComponent("Dice");
            gameObjects.Add(d1);
            GameObject d2 = director.Construct(new Vector2(750, 600));
            Dice dice2 = (Dice)d1.GetComponent("Dice");
            gameObjects.Add(d2);
            GameObject d3 = director.Construct(new Vector2(850, 600));
            Dice dice3 = (Dice)d1.GetComponent("Dice");
            gameObjects.Add(d3);

            */
            /*
            for (int i = 0; i < 2; i++)
            {
                director = new Director(new EnemyBuilder());
                gameObjects.Add(director.Construct(new Vector2(Rnd.Next(100, 900), Rnd.Next(100, 400))));
            }*/

            //director = new Director(new ExplosionBuilder());
            //gameObjects.Add(director.Construct(new Vector2(100, 100)));
            director = new Director(new AimBuilder());
            gameObjects.Add(director.Construct(new Vector2(200, 200)));
            director = new Director(new PlayerBuilder());
            gameObjects.Add(director.Construct(new Vector2(600, 470)));
            
            

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            
            AFont = Content.Load<SpriteFont>("AFont");
            BFont = Content.Load<SpriteFont>("BFont");
            CFont = Content.Load<SpriteFont>("CFont");
            resultFont = Content.Load<SpriteFont>("resultFont");
            //background = Content.Load<Texture2D>("DesertCity");
            background = Content.Load<Texture2D>("sand");
            sky = Content.Load<Texture2D>("sky");
            grass = Content.Load<Texture2D>("grass");
            //shootSound = Content.Load<Song>("gunShot");

            foreach (GameObject go in gameObjects)
            {
                go.LoadContent(Content);
            }
            foreach (GameObject go in gameObjects)
            {
                if (go.GetComponent("Enemy") is Enemy)
                    (go.GetComponent("Enemy") as Enemy).T.Start();
                else if (go.GetComponent("Player") is Player)
                    (go.GetComponent("Player") as Player).T.Start();
                else if (go.GetComponent("Aim") is Aim)
                    (go.GetComponent("Aim") as Aim).T.Start();
                else if (go.GetComponent("Explosion") is Explosion)
                    (go.GetComponent("Explosion") as Explosion).T.Start();
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            /*
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && playSound)
            {
                effect = Content.Load<SoundEffect>("gunShot");
                float volume = 0.5f;
                float pitch = 0.0f;
                float pan = 0.0f;
                effect.Play(volume, pitch, pan);
                playSound = false;
            }*/
            /*
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && playSound)
            {
                MediaPlayer.Play(shootSound);
                playSound = false;
            }
            if (Mouse.GetState().LeftButton == ButtonState.Released && !playSound)
            {
                playSound = true;
            }*/

            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (GameObject go in gameObjects)
            {
                go.Update();
            }

            UpdatePlayerShoot();
            UpdateEnemyShoot();
            ClearLists();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            //spriteBatch.Draw(sky, new Vector2(0, 0), new Rectangle(0, 0, 1300, 100), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            /*
            spriteBatch.Draw(sky, new Vector2 (0, 0), new Rectangle(0, 0, 1300, 100), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.Draw(background, new Vector2(0, 100), new Rectangle(0, 100, 1300, 470), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.Draw(grass, new Vector2(0, 65), new Rectangle(0, 65, 300, 70), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1f);
            spriteBatch.Draw(grass, new Vector2(500, 65), new Rectangle(500, 65, 300, 70), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.1f);
            spriteBatch.Draw(grass, new Vector2(1000, 65), new Rectangle(1000, 65, 300, 70), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.1f);
            */
            spriteBatch.Draw(sky, new Rectangle(0, 0, 1300, 100), Color.White);
            spriteBatch.Draw(background, new Rectangle(0, 100, 1300, 470), Color.White);
            spriteBatch.Draw(grass, new Rectangle(0, 65, 300, 70), Color.White);
            spriteBatch.Draw(grass, new Rectangle(500, 65, 300, 70), Color.White);
            spriteBatch.Draw(grass, new Rectangle(1000, 65, 300, 70), Color.White);
            
            spriteBatch.DrawString(resultFont, " = " + Result, new Vector2(875, 600), Color.Black);

            foreach (GameObject go in gameObjects)
            {
                go.Draw(spriteBatch);
            }
            if (scores.Count > 0)
            {
                foreach (Score s in scores)
                {
                    s.Draw(spriteBatch);
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void ClearLists()
        {
            if (scoresToRemove.Count > 0)
            {
                foreach (Score s in scoresToRemove)
                {
                    scores.Remove(s);
                }
                scoresToRemove.Clear();
            } 

            if (objectsToRemove.Count > 0)
            {
                foreach (GameObject go in objectsToRemove)
                {
                    collidersToRemove.Add(go.GetComponent("Collider") as Collider);
                }

                foreach (GameObject go in objectsToRemove)
                {
                    gameObjects.Remove(go);
                }
                objectsToRemove.Clear();
            }

            if (collidersToRemove.Count > 0)
            {
                foreach (Collider c in collidersToRemove)
                {
                    colliders.Remove(c);
                }
                collidersToRemove.Clear();
            }
        }

        public void UpdatePlayerShoot()
        {
            if(CanAddPlayerBollet)
            {
                director = new Director(new PlayerBulletBuilder());
                GameObject go = director.Construct(new Vector2(Mouse.GetState().Position.X, 470));
                go.LoadContent(Content);
                gameObjects.Add(go);
                CanAddPlayerBollet = false;
            }
        }

        public void UpdateEnemyShoot()
        {
            if (EnemyBulletsPositions.Count > 0)
            {
                foreach (Vector2 position in EnemyBulletsPositions)
                {
                    director = new Director(new EnemyBulletBuilder());
                    GameObject go = director.Construct(position);
                    go.LoadContent(Content);
                    gameObjects.Add(go);
                }
                EnemyBulletsPositions.Clear();
            }
        }
    }
}
