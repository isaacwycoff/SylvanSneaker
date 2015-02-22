using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SylvanSneaker.Audio;
using SylvanSneaker.Controllers;
using SylvanSneaker.Core;
using SylvanSneaker.Input;
using SylvanSneaker.Slots;
using SylvanSneaker.UI;
using System;
using System.Threading;

namespace SylvanSneaker
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager Graphics;
        SpriteBatch SpriteBatch;

        Song CurrentSong;

        SpriteFont DevFont;

        Entity Player;
        InputManager Controller;

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

            this.IsFixedTimeStep = false;

            this.Graphics.SynchronizeWithVerticalRetrace = false;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();

            AudioSlot.Initialize(new AudioManager(this.Content));
            TextureSlot.Initialize(new TextureManager(this.Content));
            ConsoleSlot.Initialize(new DevConsole(SpriteBatch, DevFont));

            this.Controller = new InputManager();
            UserInputSlot.Initialize(this.Controller);

            this.SetupWorld();
        }

        private void SetupWorld()
        {
            WorldSlot.Initialize(new World());

            this.Player = WorldSlot.AddEntity(type: EntityType.Knight, mapX: 1f, mapY: 1f, controller: this.Controller);
            WorldSlot.AddEntity(type: EntityType.Zombie, mapX: 3f, mapY: 3f, controller: new MonsterController());
            WorldSlot.AddEntity(type: EntityType.Zombie, mapX: 3f, mapY: 5f, controller: new MonsterController());

            this.Camera = new PlayerCamera(
                                attachedTo: this.Player, 
                                spriteBatch: this.SpriteBatch, 
                                width: ScreenWidth, 
                                height: ScreenHeight, 
                                zoom: 2f);
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            DevFont = Content.Load<SpriteFont>("Fonts/DevFont");

            CurrentSong = Content.Load<Song>("Songs/trim_loop2");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {

            var timeElapsed = gameTime.ElapsedGameTime;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            UserInputSlot.UpdateGame(gameTime);

            WorldSlot.Update(gameTime);

            base.Update(gameTime);

            Thread.Sleep(1);
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
                samplerState: SamplerState.PointWrap,            // turn off magnification blurring
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone);

            var framesPerSecond = (1000 / (timeElapsed.Milliseconds + 1));

            ConsoleSlot.SetDebugLine(String.Format("Frames per Second: {0}", framesPerSecond));
            ConsoleSlot.Draw(timeElapsed);

            SpriteBatch.End();

            // end drawing:
            base.Draw(gameTime);
        }
    }
}
