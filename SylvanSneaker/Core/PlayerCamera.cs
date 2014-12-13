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

        public PlayerCamera(Entity attachedTo, SpriteBatch spriteBatch, int width, int height)
        {
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
            // throw new NotImplementedException();
        }


        // we want everything from the frame EXCEPT the duration ...
        public void DrawFrame(Texture2D texture, AnimationFrame frame, float mapX, float mapY, Color tint)
        {
            var screenX = GetScreenX(mapX);
            var screenY = GetScreenY(mapY);

            Rectangle sourceRect = new Rectangle(frame.Left, frame.Top, frame.Width, frame.Height);
            Rectangle destRect = new Rectangle(screenX - frame.AnchorX, screenY - frame.AnchorY, sourceRect.Width, sourceRect.Height);

            var effect = frame.Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            this.SpriteBatch.Draw(texture, drawRectangle: destRect, sourceRectangle: sourceRect, color: tint, effect: effect);
        }

        public void DrawTile(Texture2D texture, Rectangle sourceRect, int mapX, int mapY, Color tint)
        {
            var screenX = GetScreenX(mapX);
            var screenY = GetScreenY(mapY);

            // Rectangle destRect = new Rectangle(screenX, screenY);

            var destRect = new Rectangle(screenX, screenY, TileSize, TileSize);

            SpriteBatch.Draw(texture, destRect, sourceRect, tint);
        }
    }
}
