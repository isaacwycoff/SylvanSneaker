using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SylvanSneaker.Sandbox;
using System;

namespace SylvanSneaker.Core
{
    class PlayerCamera: Camera
    {
        private IWorld World { get; set; }
        private Entity AttachedTo { get; set; }
        private SpriteBatch SpriteBatch { get; set; }

        private const int TILE_SIZE = 32;

        private float Scale { get; set; }

        private float MapX
        {
            get {
                return AttachedTo.MapCoordinates.X;         //  - ((ScreenColumns - 2) / 2));
            }
        }

        private float MapY
        {
            get { 
                return AttachedTo.MapCoordinates.Y;         //  - ((ScreenRows - 2) / 2));
            }
        }

        private int Width { get; set; }
        private int Height { get; set; }

        public PlayerCamera(IWorld world, Entity attachedTo, SpriteBatch spriteBatch, int width, int height)
        {
            this.World = world;
            this.AttachedTo = attachedTo;
            this.SpriteBatch = spriteBatch;
            this.Width = width;
            this.Height = height;
            this.Scale = 1.0f;

            this.UpdateZoom();
        }

        public float TileSize { get; private set; }
/*        public int ScreenRows { get; private set; }
        public int ScreenColumns { get; private set; } */

        private PixelCoordinates TileAnchor = new PixelCoordinates(0, 0);

        private void UpdateZoom()
        {
            this.TileSize = Convert.ToInt32(TILE_SIZE * this.Scale);
        }

        /*
        private PixelCoordinates GetScreenCoordinates(MapCoordinates mapCoordinates)
        {
            int x = (int)((mapCoordinates.X - MapX) * TileSize);
            int y = (int)((mapCoordinates.Y - MapY) * TileSize);
            return new PixelCoordinates(x, y);
        }
        */

        private int GetScreenX(float mapX)
        {
            return (int)((mapX - MapX) * TileSize);
        }

        private int GetScreenY(float mapY)
        {
            return (int)((mapY - MapY) * TileSize);
        }

        public void Draw(GameTime gameTime)
        {
            var cameraTranslation = this.GetCameraTranslation();        //  Matrix.CreateTranslation(-(this.AttachedTo.MapCoordinates.X * 2f), -(this.AttachedTo.MapCoordinates.Y * 2f), 0f);
            var zoomTranslation = Matrix.CreateScale(2f);

            SpriteBatch.Begin(sortMode: SpriteSortMode.Deferred,          // TODO: Research
                blendState: BlendState.AlphaBlend,              // blend alphas - i.e., transparencies
                samplerState: SamplerState.PointClamp,                                // samplerState: SamplerState.PointClamp,            // turn off magnification blurring
                depthStencilState: DepthStencilState.Default,          //
                rasterizerState: RasterizerState.CullNone,
                effect: null,
                transformMatrix: zoomTranslation * cameraTranslation);     // Matrix.CreateTranslation(0f, 0f, 0f));

            var timeElapsed = gameTime.ElapsedGameTime;
            DrawGround();
            DrawElements(timeElapsed);
            DrawCollisionBoxes(timeElapsed);

            SpriteBatch.End();
        }

        private Matrix GetCameraTranslation()
        {
            var xOffset = AttachedTo.MapCoordinates.X - (float)this.Width / 4f; // - (this.Width);       // (this.ScreenColumns / 2 * TileSize);  // 100f;  // (this.ScreenColumns / 2);
            var yOffset = AttachedTo.MapCoordinates.Y - (float)this.Height / 4f; //  -(this.Height);          // (this.ScreenRows / 2 * TileSize);   //  100f;  //  (this.ScreenRows / 2);

            // var matrix = Matrix.CreateTranslation(-xOffsetX * 2, -YOffsetX * 

            return Matrix.CreateTranslation(new Vector3(-xOffset * 2f, -yOffset * 2f, 0f));
        }

        private void DrawGround()
        {
            for (int y = 0; y < 20; ++y)            // World.Ground.MapWidth; ++y)
            {
                for (int x = 0; x < 20; ++x)            // World.Ground.MapHeight; ++x)
                {
                    var tile = World.Ground.Map[x, y];

                    var sourceRect = World.TileSet.GetRectangle(tile.DefinitionId);

                    var tint = new Color(tile.Lighting.Red, tile.Lighting.Green, tile.Lighting.Blue, 255);

                    DrawTile(World.TileSet.Texture, sourceRect, x, y, tint);
                }
            }
        }

        public void DrawElements(TimeSpan timeDelta)            // TODO: maybe take in a list of elements so we can combine with DrawCollisionBoxes?
        {
            var elements = World.ElementManager.AnimatedElements;

            // var elements = World.ElementManager.GetElementsInArea(left: MapX - this.Width / 4, top: MapY - this.Height / 4, width: this.Width, height: this.Height);       // this.ScreenRows); // int top, int left, int width, int height);

            foreach (var element in elements)
            {
                DrawAnimatedElement(element);
            }
        }

        public void DrawCollisionBoxes(TimeSpan timeDelta)
        {
            /*
            var elements = World.ElementManager.GetElementsInArea(left: MapX - 1, top: MapY - 1, width: this.ScreenColumns, height: this.ScreenRows); // int top, int left, int width, int height);

            foreach (var element in elements)
            {
                DrawCollisionBox(element);
            } */
        }

        private void DrawAnimatedElement(AnimatedElement element)
        {
            var frame = element.CurrentFrame;

            var tint = GetTint((int)(element.MapCoordinates.X / TileSize), (int)(element.MapCoordinates.Y / TileSize));
            DrawFrame(element.Texture, frame, element.MapCoordinates, tint);
        }

        private Color GetTint(int mapX, int mapY)
        {
            var lighting = World.Ground.GetLightLevel(mapX, mapY);
            return new Color(lighting.Red, lighting.Green, lighting.Blue);
        }

        private Color GetTint(MapCoordinates mapCoordinates)
        {
            return GetTint((int)mapCoordinates.X, (int)mapCoordinates.Y);
        }

        private void DrawCollisionBox(AnimatedElement element)
        {
            /*
            var sourceRect = new Rectangle(294, 34, 1, 1);  // JANKOTRONICS - this refers to a specific pixel in knight_sword
            var white = new Color(1f, 1f, 1f, 0.3f);
            var frame = element.CurrentFrame;

            var anchor = new PixelCoordinates(0, 0);
            var destRect = GetScreenRectangle(element.MapCoordinates, anchor, 20, 20);

            DrawSprite(element.Texture, sourceRect, destRect, white); 
            */
        }

        private void DrawFrame(Texture2D texture, AnimationFrame frame, MapCoordinates mapCoordinates, Color tint)  // float mapX, float mapY, Color tint)
        {
            var sourceRect = frame.Rectangle;
            DrawSprite(texture, new Vector2(mapCoordinates.X, mapCoordinates.Y), sourceRect, tint, frame.Flipped); 
        }

        private Rectangle GetScreenRectangle(MapCoordinates mapCoordinates, PixelCoordinates anchor, int rawWidth, int rawHeight)
        {
            var screenX = GetScreenX(mapCoordinates.X) - anchor.X;
            var screenY = GetScreenY(mapCoordinates.Y) - anchor.Y;

            var screenWidth = (int)(rawWidth * this.Scale);
            var screenHeight = (int)(rawHeight * this.Scale);

            return new Rectangle(screenX, screenY, screenWidth, screenHeight);
        }

        private void DrawTile(Texture2D texture, Rectangle sourceRect, float mapX, float mapY, Color tint)
        {
            DrawSprite(texture, new Vector2(mapX * TileSize, mapY * TileSize), sourceRect, tint); 
        }

        private void DrawSprite(Texture2D texture, Vector2 position, Rectangle sourceRect, Color tint, bool flipped = false)
        {
            var effect = flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            var depth = 0f;

            this.SpriteBatch.Draw(
                texture: texture,
                position: position,
                sourceRectangle: sourceRect,
                color: tint, rotation: 0f,
                origin: Vector2.Zero,
                scale: 1.00f,
                effect: effect,
                depth: depth);
        }
    }
}
