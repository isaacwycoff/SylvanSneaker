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

        public float Scale
        {
            get
            {
                return 1.0f;            // should be calculated?
            }
        }

        public float MapX
        {
            get {
                // calculate based on AttachedTo            
                return 0.0f;
            }
        }

        public float MapY
        {
            get { 
                // calculate based on AttachedTo
                return 0.0f;
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
            this._TileSize = Convert.ToInt32(TILE_SIZE * this.Scale);
        }

        private int _TileSize;
        public int TileSize
        {
            get { return _TileSize; }
        }

        private int _ScreenRows;
        public int ScreenRows
        {
            get { return _ScreenRows; }
        }

        private int _ScreenColumns;
        public int ScreenColumns
        {
            get { return _ScreenColumns; }
        }


        private void UpdateZoom()
        {
            this._TileSize = Convert.ToInt32(TILE_SIZE * this.Scale);
            this._ScreenRows = (this.Height / this.TileSize) + 1;
            this._ScreenColumns = (this.Width / this.TileSize) + 1;
        }

        private int GetScreenX(float mapX)
        {
            return (int)(mapX * TileSize);
            // throw new NotImplementedException();
        }

        private int GetScreenY(float mapY)
        {
            return (int)(mapY * TileSize);
        }

        public void Draw(GameTime gameTime)
        {
            var timeElapsed = gameTime.ElapsedGameTime;
            DrawGround();
            // World.ElementManager.Draw(timeElapsed, this);
            DrawElements(timeElapsed);
        }

        private void DrawGround()
        {
            int OffsetX = 0;
            int OffsetY = 0;
            int ScreenX = 0;
            int ScreenY = 0;

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
            var elements = World.ElementManager.AnimatedElements;

            foreach (var element in elements)
            {
                DrawAnimatedElement(timeDelta, element);
            }
        }

        private void DrawAnimatedElement(TimeSpan timeDelta, AnimatedElement element)
        {
            element.IncrementAnimationTime(timeDelta.Milliseconds);
            var frame = element.CurrentFrame;

            var tint = GetTint((int)element.MapX, (int)element.MapY);
            // var lighting = this.World.GetLightLevel((int)element.MapX, (int)element.MapY);
            DrawFrame(element.Texture, frame, element.MapX, element.MapY, tint);
        }

        private Color GetTint(int mapX, int mapY)
        {
            var lighting = World.Ground.GetLightLevel(mapX, mapY);
            return new Color(lighting.Red, lighting.Green, lighting.Blue);
        }

        // we want everything from the frame EXCEPT the duration ...
        private void DrawFrame(Texture2D texture, AnimationFrame frame, float mapX, float mapY, Color tint)
        {
            var screenX = GetScreenX(mapX);
            var screenY = GetScreenY(mapY);

            Rectangle sourceRect = new Rectangle(frame.Left, frame.Top, frame.Width, frame.Height);
            Rectangle destRect = new Rectangle(screenX - frame.AnchorX, screenY - frame.AnchorY, sourceRect.Width, sourceRect.Height);

            var effect = frame.Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            this.SpriteBatch.Draw(texture, drawRectangle: destRect, sourceRectangle: sourceRect, color: tint, effect: effect);
        }

        private void DrawTile(Texture2D texture, Rectangle sourceRect, int mapX, int mapY, Color tint)
        {
            var screenX = GetScreenX(mapX);
            var screenY = GetScreenY(mapY);

            // Rectangle destRect = new Rectangle(screenX, screenY);

            var destRect = new Rectangle(screenX, screenY, TileSize, TileSize);

            SpriteBatch.Draw(texture, destRect, sourceRect, tint);
        }
    }
}
