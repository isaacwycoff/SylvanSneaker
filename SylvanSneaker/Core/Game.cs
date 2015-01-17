using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SylvanSneaker.Audio;
using SylvanSneaker.Controllers;
using SylvanSneaker.Core;
using SylvanSneaker.Input;
using SylvanSneaker.UI;
using System;

namespace SylvanSneaker
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager Graphics;
        SpriteBatch SpriteBatch;

        TextureManager TextureManager;
        AudioManager AudioManager;
        InputManager InputManager;

        IWorld World;

        Song CurrentSong;

        SpriteFont DevFont;

        Entity Player;
        PlayerController Controller;

        DevConsole DefaultConsole;

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
            // this.AudioManager.PlaySong(CurrentSong);

            this.InputManager = new InputManager();

            this.Controller = new PlayerController();

            this.SetupWorld();

            this.DefaultConsole = new DevConsole(SpriteBatch, DevFont);
        }

        private void SetupWorld()
        {
            this.World = new World(this.TextureManager);

            this.Player = World.AddEntity(type: EntityType.Knight, mapX: 1f, mapY: 1f, controller: this.Controller);
            World.AddEntity(type: EntityType.Zombie, mapX: 3f, mapY: 3f, controller: new MonsterController());
            World.AddEntity(type: EntityType.Zombie, mapX: 3f, mapY: 5f, controller: new MonsterController());

            this.Camera = new PlayerCamera(this.World, this.Player, this.SpriteBatch, ScreenWidth, ScreenHeight);
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

            UpdateControls(gameTime);

            World.Update(gameTime);

            base.Update(gameTime);
        }

        private void UpdateControls(GameTime gameTime)
        {
            var keys = Keyboard.GetState().GetPressedKeys();
            Controller.SendCommand(InputManager.UpdateGame(gameTime));
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            var timeElapsed = gameTime.ElapsedGameTime;

            GraphicsDevice.Clear(Color.Crimson);            // CornflowerBlue);

            // begin drawing - XNA 4.0 code 

            Camera.Draw(gameTime);

            SpriteBatch.Begin(sortMode: SpriteSortMode.Deferred,          // TODO: Research
                blendState: BlendState.AlphaBlend,              // blend alphas - i.e., transparencies
                samplerState: SamplerState.PointClamp,            // turn off magnification blurring
                depthStencilState: DepthStencilState.Default,          //
                rasterizerState: RasterizerState.CullNone);

            // SpriteBatch.DrawString(this.DevFont, timeElapsed.Milliseconds.ToString(), Vector2.Zero, Color.WhiteSmoke);

            DefaultConsole.SetDebugLine(String.Format("ms Elapsed: {0}", timeElapsed.Milliseconds));

            if (gameTime.ElapsedGameTime.TotalSeconds > 5.0f && timeElapsed.Milliseconds != 16)
            {
                var derp = "DERP!";
            }

            
            DefaultConsole.WriteLine("TESTING");
            DefaultConsole.Draw(timeElapsed);

            SpriteBatch.End();

            // end drawing:


            base.Draw(gameTime);
        }


    }
}
