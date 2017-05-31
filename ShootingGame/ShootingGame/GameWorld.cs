using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using System;
using Microsoft.Xna.Framework.Media;
using System.Threading;
using ShootingGame.Interfaces;

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
        /// Referance to the Director class
        /// </summary>
        Director director;

        public bool IsTesting { get; set; } = false;

        public int Reserve { get; set; }

        public int Current { get; set; }

        /// <summary>
        /// The list of the gameobjects
        /// </summary>
        List<GameObject> gameObjects;

        /// <summary>
        /// The list of the gamebjects to remove
        /// </summary>
        List<GameObject> objectsToRemove;

        /// <summary>
        /// The list of the collider components
        /// </summary>
        List<Collider> colliders;

        /// <summary>
        /// The list of the collider components to remove
        /// </summary>
        List<Collider> collidersToRemove;

        /// <summary>
        /// The score list
        /// </summary>
        List<Score> scores;

        /// <summary>
        /// The score list used in the game loop
        /// </summary>
        List<Score> tempScores;

        /// <summary>
        /// The score list to remove
        /// </summary>
        List<Score> scoresToRemove;

        /// <summary>
        /// The list of positions where to place new EnemyBullets
        /// </summary>
        List<Vector2> enemyBulletsPositions;

        /// <summary>
        /// The list of positions of new EnemyBollets used in the game loop
        /// </summary>
        List<Vector2> tempEnemyBulletsPositions;

        /// <summary>
        /// The background image
        /// </summary>
        Texture2D background;

        /// <summary>
        /// The sky image
        /// </summary>
        Texture2D sky;

        /// <summary>
        /// The grass image
        /// </summary>
        Texture2D grass;

        /// <summary>
        /// Referance to the Main Menu
        /// </summary>
        Menu menu;

        /// <summary>
        /// Referance to the Score Menu
        /// </summary>
        ScoreMenu scoreMenu;

        /// <summary>
        /// The dice timer thread
        /// </summary>
        Thread diceTimerThread;

        /// <summary>
        /// Checks if player can roll the dice
        /// </summary>
        bool canRollDice;

        /// <summary>
        /// The dice roll timer
        /// </summary>
        int diceTimerCounter;
        public KeyboardState UpPressed { get; set; }
        private KeyboardState downPressed;
        public bool HasPressed { get; set; } = false;
        public List<IDice> Dies { get; set; }
        public int DiceResult { get; set; }

        /// <summary>
        /// Checks if necassery to replace some GameObjects when the game is restarted
        /// </summary>
        public bool ReplaceObjects { get; set; }

        /// <summary>
        /// Pixel to draw
        /// </summary>
        public Texture2D Pixel { get; private set; }

        /// <summary>
        /// Singletone pattern
        /// </summary>
        private static GameWorld instance;

        /// <summary>
        /// Checks if necasery to pause the game
        /// </summary>
        public bool StopGame { get; set; }

        /// <summary>
        /// Checks if player can play game
        /// </summary>
        public bool PlayGame { get; set; }

        /// <summary>
        /// Checks if necassey to show Score Menu
        /// </summary>
        public bool ShowScoreMenu { get; set; }

        /// <summary>
        /// Cheks if necassey to add new GameObject PlayerBullet
        /// </summary>
        public bool CanAddPlayerBullet { get; set; }

        /// <summary>
        /// The DeltaTime
        /// </summary>
        public float DeltaTime { get; private set; }

        /// <summary>
        /// The font with size 8
        /// </summary>
        public SpriteFont AFont { get; private set; }

        /// <summary>
        /// The font with size 12
        /// </summary>
        public SpriteFont BFont { get; private set; }

        /// <summary>
        /// The font with size 16
        /// </summary>
        public SpriteFont CFont { get; private set; }

        /// <summary>
        /// The font with size 24
        /// </summary>
        public SpriteFont DFont { get; private set; }

        /// <summary>
        /// Referance to the Random class
        /// </summary>
        public Random Rnd { get; private set; }

        internal List<GameObject> ObjectsToAdd { get; set; }

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

        internal List<Score> Scores
        {
            get
            {
                lock (scores)
                    return scores;
            }
        }

        public int CurrentDice { get; set; }

        public List<Vector2> EnemyBulletsPositions
        {
            get
            {
                lock (enemyBulletsPositions)
                    return enemyBulletsPositions;
            }
        }

        /// <summary>
        /// The GameWorld's constructor
        /// </summary>
        private GameWorld()
        {
            scoreMenu = new ScoreMenu();
            gameObjects = new List<GameObject>();
            objectsToRemove = new List<GameObject>();
            colliders = new List<Collider>();
            collidersToRemove = new List<Collider>();
            scores = new List<Score>();
            tempScores = new List<Score>();
            scoresToRemove = new List<Score>();
            Dies = new List<IDice>();
            ObjectsToAdd = new List<GameObject>();
            enemyBulletsPositions = new List<Vector2>();
            tempEnemyBulletsPositions = new List<Vector2>();
        }

        public void Setup()
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
            PlayGame = false;
            ReplaceObjects = false;
            ShowScoreMenu = false;
            CanAddPlayerBullet = false;
            StopGame = true;
            menu = new Menu();

            Pixel = new Texture2D(GraphicsDevice, 1, 1);
            Pixel.SetData(new[] { Color.White });
            DataBaseClass.Instance.CreateTables();
            diceTimerThread = new Thread(DiceTimer);
            canRollDice = true;
            diceTimerCounter = 0;
            // Adds the GameObjects to the game
            director = new Director(new EnemyBuilder());
            gameObjects.Add(director.Construct(new Vector2(-50, 100)));

            director = new Director(new EnemyBuilder());
            gameObjects.Add(director.Construct(new Vector2(1350, 200)));

            director = new Director(new EnemyBuilder());
            gameObjects.Add(director.Construct(new Vector2(-50, 300)));

            director = new Director(new EnemyBuilder());
            gameObjects.Add(director.Construct(new Vector2(1350, 400)));

            /*
            for (int i = 0; i < 2; i++)
            {
                director = new Director(new EnemyBuilder());
                gameObjects.Add(director.Construct(new Vector2(Rnd.Next(100, 900), Rnd.Next(100, 400))));
            }*/
            director = new Director(new PowerUpObjectBuilder());
            gameObjects.Add(director.Construct(new Vector2(200, -100)));
            director = new Director(new AimBuilder());
            gameObjects.Add(director.Construct(new Vector2(200, 200)));
            director = new Director(new PlayerBuilder());
            gameObjects.Add(director.Construct(new Vector2(600, 500)));

            director = new Director(new DiceBuilder());
            GameObject d1 = director.Construct(new Vector2(800, 590));
            Dice dice1 = (Dice)d1.GetComponent("Dice");
            gameObjects.Add(d1);
            GameObject d2 = director.Construct(new Vector2(850, 590));
            Dice dice2 = (Dice)d2.GetComponent("Dice");
            gameObjects.Add(d2);
            GameObject d3 = director.Construct(new Vector2(900, 590));
            Dice dice3 = (Dice)d3.GetComponent("Dice");
            gameObjects.Add(d3);
            Dies.Add(dice1);
            Dies.Add(dice2);
            Dies.Add(dice3);

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
            AFont = Content.Load<SpriteFont>("AFont"); // 8
            BFont = Content.Load<SpriteFont>("BFont"); // 12
            CFont = Content.Load<SpriteFont>("CFont"); // 16
            DFont = Content.Load<SpriteFont>("DFont"); // 24
            //background = Content.Load<Texture2D>("DesertCity");
            background = Content.Load<Texture2D>("sand");
            sky = Content.Load<Texture2D>("sky");
            grass = Content.Load<Texture2D>("grass");

            menu.LoadContent(Content);
            scoreMenu.LoadContent(Content);

            foreach (GameObject go in gameObjects)
            {
                go.LoadContent(Content);
            }

            // Starts threads for some GameObjects
            foreach (GameObject go in gameObjects)
            {
                if (go.GetComponent("Enemy") is Enemy)
                    (go.GetComponent("Enemy") as Enemy).T.Start();
                else if (go.GetComponent("Player") is Player)
                    (go.GetComponent("Player") as Player).T.Start();
                else if (go.GetComponent("Aim") is Aim)
                    (go.GetComponent("Aim") as Aim).T.Start();
                else if (go.GetComponent("PowerUpObject") is PowerUpObject)
                    (go.GetComponent("PowerUpObject") as PowerUpObject).T.Start();
            }
            diceTimerThread.IsBackground = true;
            diceTimerThread.Start();
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

            if (Keyboard.GetState().IsKeyDown(Keys.M) && PlayGame)
            {
                PlayGame = false;
                StopGame = true;
                Player.CanStartShoot = false;
            }
            // Makes sure that animation doesn't play when the game is paused
            DeltaTime = !StopGame ? (float)gameTime.ElapsedGameTime.TotalSeconds : 0;

            if (PlayGame)
            {
                // Replaces some GameObjects when the game is restarted
                if (ReplaceObjects)
                {
                    foreach (GameObject go in gameObjects)
                    {
                        if (go.GetComponent("Enemy") is Enemy)
                            (go.GetComponent("Enemy") as Enemy).Replace();
                        else if (go.GetComponent("Player") is Player)
                            (go.GetComponent("Player") as Player).Replace();
                        else if (go.GetComponent("PowerUpObject") is PowerUpObject)
                            (go.GetComponent("PowerUpObject") as PowerUpObject).Replace();
                    }
                    Player.CanStartShoot = true;
                    ReplaceObjects = false;
                    diceTimerCounter = 0;
                }
                if (this.IsMouseVisible) this.IsMouseVisible = false;

                // Updates the gameObjects
                foreach (GameObject go in gameObjects)
                {
                    go.Update();
                }
                // Checks if player rolls the dice
                if (!StopGame && canRollDice)
                    UpdateDiceUI();

                // Checks if player made a shot
                UpdatePlayerShot();

                // Checks if enemies made any shots
                UpdateEnemyShot();
            }
            else if (ShowScoreMenu)
            {
                if (!this.IsMouseVisible) this.IsMouseVisible = true;
                scoreMenu.UpdateUI();
            }
            else
            {
                if (!this.IsMouseVisible) this.IsMouseVisible = true;
                menu.UpdateUI();
            }

            UpdateLists();

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
            if (PlayGame)
            {
                spriteBatch.Draw(sky, new Rectangle(0, 0, 1300, 100), Color.White);
                spriteBatch.Draw(background, new Rectangle(0, 100, 1300, 470), Color.White);
                spriteBatch.Draw(grass, new Rectangle(0, 65, 300, 70), Color.White);
                spriteBatch.Draw(grass, new Rectangle(500, 65, 300, 70), Color.White);
                spriteBatch.Draw(grass, new Rectangle(1000, 65, 300, 70), Color.White);

                // Draws the gameObjects
                foreach (GameObject go in gameObjects)
                {
                    go.Draw(spriteBatch);
                }

                // Draws the scores
                if (tempScores.Count > 0)
                {
                    foreach (Score s in tempScores)
                    {
                        s.Draw(spriteBatch);
                    }
                }

                spriteBatch.DrawString(BFont, "RESERV: " + Reserve, new Vector2(800, 650), Color.Black);
                spriteBatch.DrawString(BFont, "[M] - exit to the MAIN MENU", new Vector2(1100, 620), Color.Black);
                spriteBatch.DrawString(BFont, "[Esc] - exit game", new Vector2(1100, 650), Color.Black);
                spriteBatch.Draw(Pixel, new Rectangle(800, 640, diceTimerCounter, 5), Color.Blue);

                if (StopGame)
                {
                    spriteBatch.DrawString(DFont, "GAME OVER!", new Vector2(530, 170), Color.DarkMagenta);
                    spriteBatch.DrawString(DFont, "Press [M] to exit to the MAIN MENU", new Vector2(400, 230), Color.DarkMagenta);
                }
            }
            else if (ShowScoreMenu) scoreMenu.ShowScoreTable(spriteBatch);
            else menu.ShowMainMenu(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Updates all the lists
        /// </summary>
        public void UpdateLists()
        {
            // Removes scores from list scores to tempscores, it gives more time to access to the list scores
            lock (scores)
            {
                if (scores.Count > 0)
                {
                    tempScores.AddRange(scores);
                    scores.Clear();
                }
            }
            // Checks if there are scores with aborted threads in the list tempScores and adds them to the list scoresToRemove
            if (tempScores.Count > 0)
            {
                foreach (Score s in tempScores)
                {
                    if (!s.T.IsAlive) scoresToRemove.Add(s);
                }
            }
            // Removes scores with aborted threads from the list tempScores
            if (scoresToRemove.Count > 0)
            {
                foreach (Score s in scoresToRemove)
                {
                    tempScores.Remove(s);
                }
                scoresToRemove.Clear();
            }
            // Checks if there are gameObjects with components with aborted threads and adds them to the list objectToRemove
            foreach (GameObject go in gameObjects)
            {
                if (go.GetComponent("EnemyBullet") is EnemyBullet)
                {
                    if (!((go.GetComponent("EnemyBullet") as EnemyBullet).T.IsAlive))
                        objectsToRemove.Add(go);
                }
                else if (go.GetComponent("PlayerBullet") is PlayerBullet)
                {
                    if (!((go.GetComponent("PlayerBullet") as PlayerBullet).T.IsAlive))
                        objectsToRemove.Add(go);
                }
            }
            // Removes gameObjects with components with aborted threads from the lists gameObject and colliders
            if (objectsToRemove.Count > 0)
            {
                foreach (GameObject go in objectsToRemove)
                {
                    if ((go.GetComponent("PlayerBullet") is PlayerBullet) || (go.GetComponent("EnemyBullet") is EnemyBullet))
                        collidersToRemove.Add(go.GetComponent("Collider") as Collider);
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

        /// <summary>
        /// Checks if player made a shot, and adds new PlayerBullet gameObject, if necassery
        /// </summary>
        public void UpdatePlayerShot()
        {
            if (CanAddPlayerBullet)
            {
                director = new Director(new PlayerBulletBuilder());
                GameObject go = director.Construct(new Vector2(Mouse.GetState().Position.X, 470));
                go.LoadContent(Content);
                gameObjects.Add(go);
                CanAddPlayerBullet = false;
            }
        }

        /// <summary>
        /// Checks if enemy made a shot, and adds new EnemyBullet object, if necassery
        /// </summary>
        public void UpdateEnemyShot()
        {
            // Removes content from the list enmyBulletsPositions to tempEnemyBulletPositions,
            // it gives more time to access to the list enemyBulletsPositions
            lock (enemyBulletsPositions)
            {
                if (EnemyBulletsPositions.Count > 0)
                {
                    tempEnemyBulletsPositions.AddRange(enemyBulletsPositions);
                    EnemyBulletsPositions.Clear();
                }
            }
            // Adds new EnemyBullet gameObjects, if it is necassery
            if (tempEnemyBulletsPositions.Count > 0)
            {
                foreach (Vector2 position in tempEnemyBulletsPositions)
                {
                    director = new Director(new EnemyBulletBuilder());
                    GameObject go = director.Construct(position);
                    go.LoadContent(Content);
                    gameObjects.Add(go);
                }
                tempEnemyBulletsPositions.Clear();
            }
        }

        public void UpdateDiceUI()
        {
            KeyboardState k = Keyboard.GetState();
            if (k.IsKeyDown(Keys.Up))
            {
                if (!UpPressed.IsKeyDown(Keys.Up))
                {
                    High();
                    canRollDice = false;
                    diceTimerCounter = 130;
                }

            }
            else if (UpPressed.IsKeyDown(Keys.Up))
            {

            }
            UpPressed = k;

            if (k.IsKeyDown(Keys.Down))
            {
                if (!downPressed.IsKeyDown(Keys.Down))
                {
                    Low();
                    canRollDice = false;
                    diceTimerCounter = 130;
                }

            }
            else if (downPressed.IsKeyDown(Keys.Down))
            {

            }
            downPressed = k;
        }
        

        public void High()
        {
            HasPressed = true;
            Current = 0;

            Current = Result;
            Result = 0;
            foreach (Dice dice in Dies)
            {
                CurrentDice = dice.Roll();
                Result += CurrentDice;
                dice.UpdateDice(CurrentDice);
            }

            if (Current < Result)
            {
                Player.CurrentWeapon.TotalAmmo += Current + Reserve;
                if (Reserve > 0)
                {
                    Reserve = 0;
                }

            }
            if (Current > Result)
            {
                Reserve += Current;
            }
        }

        public void Low()
        {
            HasPressed = true;
            Current = 0;

            Current = Result;
            Result = 0;
            foreach (Dice dice in Dies)
            {
                CurrentDice = dice.Roll();
                Result += CurrentDice;
                dice.UpdateDice(CurrentDice);
            }

            if (Current > Result)
            {
                Player.CurrentWeapon.TotalAmmo += Current + Reserve;
                if (Reserve > 0)
                {
                    Reserve = 0;
                }
            }
            if (Current < Result)
            {
                Reserve += Current;
            }
        }

        /// <summary>
        /// Updates the dice timer, the timer starts every time, when player rolls the dice
        /// </summary>
        public void DiceTimer()
        {
            while (true)
            {
                Thread.Sleep(50);
                if (diceTimerCounter > 0) diceTimerCounter--;
                else if (!canRollDice) canRollDice = true;
            }
        }
    }
}
