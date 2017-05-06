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
        List<GameObject> gameObjects;
        List<GameObject> objectsToRemove;
        Texture2D background;
        Texture2D sky;
        private SoundEffect effect;
        private static GameWorld instance;
        List<Collider> colliders;
        bool playSound;
        Song shootSound;
        public float DeltaTime { get; private set; }
        public SpriteFont AFont { get; private set; }
        public SpriteFont BFont { get; private set; }
        public Random Rnd { get; private set; }
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
            gameObjects = new List<GameObject>();
            objectsToRemove = new List<GameObject>();
            colliders = new List<Collider>();
            
            director = new Director(new EnemyBuilder());
            gameObjects.Add(director.Construct(new Vector2(500, 100)));
            director = new Director(new EnemyBuilder());
            gameObjects.Add(director.Construct(new Vector2(500, 200)));
            director = new Director(new EnemyBuilder());
            gameObjects.Add(director.Construct(new Vector2(500, 300)));
            director = new Director(new EnemyBuilder());
            gameObjects.Add(director.Construct(new Vector2(500, 400)));

            /*
            for (int i = 0; i < 2; i++)
            {
                director = new Director(new EnemyBuilder());
                gameObjects.Add(director.Construct(new Vector2(Rnd.Next(100, 900), Rnd.Next(100, 400))));
            }*/

            director = new Director(new ExplosionBuilder());
            gameObjects.Add(director.Construct(new Vector2(100, 100)));
            director = new Director(new AimBuilder());
            gameObjects.Add(director.Construct(new Vector2(200, 200)));
            director = new Director(new PlayerBuilder());
            gameObjects.Add(director.Construct(new Vector2(600, 470)));
            MediaPlayer.IsRepeating = false;

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
            //background = Content.Load<Texture2D>("DesertCity");
            background = Content.Load<Texture2D>("sand");
            sky = Content.Load<Texture2D>("sky");
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
            
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && playSound)
            {
                effect = Content.Load<SoundEffect>("gunShot");
                float volume = 0.5f;
                float pitch = 0.0f;
                float pan = 0.0f;
                effect.Play(volume, pitch, pan);
                playSound = false;
            }
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
            /*
            if(objectsToRemove.Count > 0)
            {
                foreach (GameObject go in objectsToRemove)
                {
                    gameObjects.Remove(go);
                }
                objectsToRemove.Clear();
            }*/

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
            
            spriteBatch.Draw(sky, new Rectangle(0, 0, 1300, 100), Color.White);
            spriteBatch.Draw(background, new Rectangle(0, 100, 1300, 470), Color.White);

            foreach (GameObject go in gameObjects)
            {
                go.Draw(spriteBatch);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
