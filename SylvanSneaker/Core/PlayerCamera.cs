using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SylvanSneaker.Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SylvanSneaker.Core
{
    class PlayerCamera: Camera
    {
        private IWorld World { get; set; }
        public Entity AttachedTo { get; set; }
        private SpriteBatch SpriteBatch { get; set; }

        private const int TILE_SIZE = 32;

        public float Scale { get; private set; }

        public float MapX
        {
            get {
                // calculate based on AttachedTo
                return (AttachedTo.MapX - ((ScreenColumns - 2) / 2));
            }
        }

        public float MapY
        {
            get { 
                return (AttachedTo.MapY - ((ScreenRows - 2) / 2));
            }
        }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public PlayerCamera(IWorld world, Entity attachedTo, SpriteBatch spriteBatch, int width, int height)
        {
            this.World = world;
            this.AttachedTo = attachedTo;
            this.SpriteBatch = spriteBatch;
            this.Width = width;
            this.Height = height;
            this.Scale = 2.0f;

            this.UpdateZoom();
        }

        public int TileSize { get; private set; }
        public int ScreenRows { get; private set; }
        public int ScreenColumns { get; private set; }


        private void UpdateZoom()
        {
            this.TileSize = Convert.ToInt32(TILE_SIZE * this.Scale);
            this.ScreenRows = (this.Height / this.TileSize) + 1;
            this.ScreenColumns = (this.Width / this.TileSize) + 1;
        }

        private int GetScreenX(float mapX)
        {
            return (int)((mapX - MapX) * TileSize);          // what about offset?
        }

        private int GetScreenY(float mapY)
        {
            return (int)((mapY - MapY) * TileSize);          // what about offset?
        }

        public void Draw(GameTime gameTime)
        {
            var timeElapsed = gameTime.ElapsedGameTime;
            DrawGround();
            DrawElements(timeElapsed);
        }

        private void DrawGround()
        {
            int OffsetX = 0;
            int OffsetY = 0;

            int maxX = Math.Min(World.Ground.ScreenColumns, World.Ground.MapWidth - OffsetX);
            int maxY = Math.Min(World.Ground.ScreenRows, World.Ground.MapHeight - OffsetY);

            for (int y = 0; y < maxY; ++y)
            {
                for (int x = 0; x < maxX; ++x)
                {
                    var tile = World.Ground.Map[x + OffsetX, y + OffsetY];

                    var sourceRect = World.TileSet.GetRectangle(tile.DefinitionId);

                    var tint = new Color(tile.Lighting.Red, tile.Lighting.Green, tile.Lighting.Blue, 255);

                    DrawTile(World.TileSet.Texture, sourceRect, x + OffsetX, y + OffsetY, tint);
                }
            }
        }

        public void DrawElements(TimeSpan timeDelta)
        {
            // var elements = World.ElementManager.AnimatedElements;

            var elements = World.ElementManager.GetElementsInArea(left: MapX - 1, top: MapY - 1, width: this.ScreenColumns, height: this.ScreenRows); // int top, int left, int width, int height);

            foreach (var element in elements)
            {
                DrawAnimatedElement(timeDelta, element);
            }
        }

        private void DrawAnimatedElement(TimeSpan timeDelta, AnimatedElement element)
        {
            var frame = element.CurrentFrame;

            var tint = GetTint((int)element.MapX, (int)element.MapY);
            DrawFrame(element.Texture, frame, element.MapX, element.MapY, tint);
        }

        private Color GetTint(int mapX, int mapY)
        {
            var lighting = World.Ground.GetLightLevel(mapX, mapY);
            return new Color(lighting.Red, lighting.Green, lighting.Blue);
        }

        private void DrawFrame(Texture2D texture, AnimationFrame frame, float mapX, float mapY, Color tint)
        {
            var sourceRect = frame.Rectangle;
            var destRect = GetScreenRectangle(mapX, mapY, sourceRect.Width, sourceRect.Height);
            DrawSprite(texture, sourceRect, destRect, tint, frame.Flipped);
        }

        private Rectangle GetScreenRectangle(float mapX, float mapY, int rawWidth, int rawHeight)
        {
            var screenX = GetScreenX(mapX);
            var screenY = GetScreenY(mapY);

            var screenWidth = (int)(rawWidth * this.Scale);
            var screenHeight = (int)(rawHeight * this.Scale);

            return new Rectangle(screenX, screenY, screenWidth, screenHeight);
        }

        private void DrawTile(Texture2D texture, Rectangle sourceRect, int mapX, int mapY, Color tint)
        {
            var destRect = GetScreenRectangle(mapX, mapY, TileSize, TileSize);
            DrawSprite(texture, sourceRect, destRect, tint);
        }

        private void DrawSprite(Texture2D texture, Rectangle sourceRect, Rectangle destRect, Color tint, bool flipped = false)
        {
            var effect = flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            this.SpriteBatch.Draw(texture, drawRectangle: destRect, sourceRectangle: sourceRect, color: tint, effect: effect);
        }
    }
}
