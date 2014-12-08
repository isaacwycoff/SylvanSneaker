using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SylvanSneaker.Audio;
using SylvanSneaker.Core;
using SylvanSneaker.Environment;
using SylvanSneaker.Sandbox;
using SylvanSneaker.UI;
using System;

namespace SylvanSneaker
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager Graphics;
        SpriteBatch SpriteBatch;

        TextureManager TextureManager;
        ElementManager ElementManager;
        EntityManager EntityManager;
        AudioManager AudioManager;

        Song CurrentSong;

        SpriteFont DevFont;

        Entity Player;
        PlayerController Controller;

        DevConsole DefaultConsole;

        // Throw-away variables:
        Texture2D TempTexture;
        Texture2D TempGroundTexture;

        // AnimatedElement Element;
        Ground Ground;

        Camera Camera;

        int ScreenWidth = 800;
        int ScreenHeight = 600;
        Boolean IsFullScreen = false;

        public Game(): base()
        {
            Graphics = new GraphicsDeviceManager(this);

            this.Graphics.PreferredBackBufferWidth = ScreenWidth;
            this.Graphics.PreferredBackBufferHeight = ScreenHeight;
            this.Graphics.IsFullScreen = IsFullScreen;

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            this.AudioManager = new AudioManager();
            this.TextureManager = new TextureManager(this.Content);
            this.ElementManager = new ElementManager(this.SpriteBatch, this.TextureManager);


            // this.AudioManager.PlaySong(CurrentSong);

            this.Controller = new PlayerController();

            this.SetupWorld();


            this.DefaultConsole = new DevConsole(SpriteBatch, DevFont);
        }

        private void SetupWorld()
        {
            this.EntityManager = new EntityManager(this.TextureManager, this.ElementManager);

            this.Player = EntityManager.Add(EntityType.Knight, 1, 1, this.Controller);

            this.Camera = new PlayerCamera(this.Player, ScreenWidth, ScreenHeight);

            var generator = new GroundGenerator(SpriteBatch);
            this.Ground = generator.Generate(groundTexture: TempGroundTexture, tileSize: 32, camera: this.Camera);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            DevFont = Content.Load<SpriteFont>("Fonts/DevFont");

            CurrentSong = Content.Load<Song>("Songs/trim_loop2");

            TempTexture = Content.Load<Texture2D>("Textures/knight_sword_REPLACE");
            TempGroundTexture = Content.Load<Texture2D>("Textures/tile_jungle_REPLACE");

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            var timeElapsed = gameTime.ElapsedGameTime;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // this.Ground.ShiftCamera(Direction.East);

            this.Ground.Update(timeElapsed);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            var timeElapsed = gameTime.ElapsedGameTime;

            GraphicsDevice.Clear(Color.CornflowerBlue);

            // begin drawing - XNA 4.0 code -
            SpriteBatch.Begin(SpriteSortMode.Deferred,          // TODO: Research
                            BlendState.AlphaBlend,              // blend alphas - i.e., transparencies
                            SamplerState.PointClamp,            // turn off magnification blurring
                            DepthStencilState.Default,          //
                            RasterizerState.CullNone);

            Ground.Draw(timeElapsed);

            ElementManager.Draw(timeElapsed);

            DefaultConsole.WriteLine("TESTING");
            DefaultConsole.Draw(timeElapsed);

            // end drawing:
            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
