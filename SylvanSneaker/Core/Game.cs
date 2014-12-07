﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SylvanSneaker.Core;
using SylvanSneaker.Environment;
using SylvanSneaker.Sandbox;
using System;

namespace SylvanSneaker
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager Graphics;
        SpriteBatch SpriteBatch;

        ElementManager ElementManager;
        TextureManager TextureManager;

        Song CurrentSong;

        SpriteFont DevFont;

        // Throw-away variables:
        Texture2D TempTexture;
        Texture2D TempGroundTexture;

        AnimatedElement Element;
        Ground Ground;

        Camera Camera;

        int ScreenWidth = 800;
        int ScreenHeight = 600;
        Boolean IsFullScreen = false;

        TimeSpan LastUpdate;

        public Game(): base()
        {
            Graphics = new GraphicsDeviceManager(this);

            this.Graphics.PreferredBackBufferWidth = ScreenWidth;
            this.Graphics.PreferredBackBufferHeight = ScreenHeight;
            this.Graphics.IsFullScreen = IsFullScreen;

            Content.RootDirectory = "Content";

            LastUpdate = new TimeSpan(0);
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
            base.Initialize();

            this.TextureManager = new TextureManager(this.Content);
            this.ElementManager = new ElementManager(this.SpriteBatch, this.TextureManager);

            this.Element = new AnimatedElement(TempTexture, SpriteBatch);

            this.Camera = new PlayerCamera(this.Element, ScreenWidth, ScreenHeight);

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

            // TODO: put things that are game init stuff in their own function
            // that happens after LoadContent
            MediaPlayer.IsRepeating = true;
            // MediaPlayer.Play(currentSong);
            MediaPlayer.Volume = 0.5f;          // FIXME: get this from Settings
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

            // TODO: Add your update logic here

            // this.Ground.ShiftCamera(Direction.East);

            this.Ground.Update(timeElapsed);

            this.Element.Update(timeElapsed);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            TimeSpan timeElapsed = gameTime.ElapsedGameTime;

            GraphicsDevice.Clear(Color.CornflowerBlue);

            // begin drawing - XNA 4.0 code -
            SpriteBatch.Begin(SpriteSortMode.Deferred,          // TODO: Research
                            BlendState.AlphaBlend,              // blend alphas - i.e., transparencies
                            SamplerState.PointClamp,            // turn off magnification blurring
                            DepthStencilState.Default,          //
                            RasterizerState.CullNone);

            // debugging text -- FIXME: put this in its own function
            string output = String.Format("Element MS: {0}", Element.AnimationTime);

            Ground.Draw(timeElapsed);

            Element.Draw(timeElapsed);

            // Find the center of the string
            Vector2 FontOrigin = DevFont.MeasureString(output) / 2;
            // Draw the string
            SpriteBatch.DrawString(DevFont, output, new Vector2(21, 501), Color.DarkBlue);          // y 571
            SpriteBatch.DrawString(DevFont, output, new Vector2(20, 500), Color.WhiteSmoke);        // y 570

            // end drawing:
            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
