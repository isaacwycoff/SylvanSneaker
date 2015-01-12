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
                return (AttachedTo.MapCoordinates.X - ((ScreenColumns - 2) / 2));
            }
        }

        private float MapY
        {
            get { 
                return (AttachedTo.MapCoordinates.Y - ((ScreenRows - 2) / 2));
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
            this.Scale = 2.0f;

            this.UpdateZoom();
        }

        public int TileSize { get; private set; }
        public int ScreenRows { get; private set; }
        public int ScreenColumns { get; private set; }

        private PixelCoordinates TileAnchor = new PixelCoordinates(0, 0);

        private void UpdateZoom()
        {
            this.TileSize = Convert.ToInt32(TILE_SIZE * this.Scale);
            this.ScreenRows = (this.Height / this.TileSize) + 1;
            this.ScreenColumns = (this.Width / this.TileSize) + 1;
        }

        private PixelCoordinates GetScreenCoordinates(MapCoordinates mapCoordinates)
        {
            int x = (int)((mapCoordinates.X - MapX) * TileSize);
            int y = (int)((mapCoordinates.Y - MapY) * TileSize);
            return new PixelCoordinates(x, y);
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
            DrawCollisionBoxes(timeElapsed);
        }

        private void DrawGround()
        {
            // TODO: split up this method into more intelligible, smaller methods
            int OffsetX = (int)MapX;
            int OffsetY = (int)MapY;

            int minX = Math.Max(0, -OffsetX);
            int minY = Math.Max(0, -OffsetY);

            int maxX = Math.Min(this.ScreenColumns, World.Ground.MapWidth - OffsetX);
            int maxY = Math.Min(this.ScreenRows, World.Ground.MapHeight - OffsetY);

            for (int y = minY; y < maxY; ++y)
            {
                for (int x = minX; x < maxX; ++x)
                {
                    var tile = World.Ground.Map[x + OffsetX, y + OffsetY];

                    var sourceRect = World.TileSet.GetRectangle(tile.DefinitionId);

                    var tint = new Color(tile.Lighting.Red, tile.Lighting.Green, tile.Lighting.Blue, 255);

                    DrawTile(World.TileSet.Texture, sourceRect, x + OffsetX, y + OffsetY, tint);
                }
            }
        }

        public void DrawElements(TimeSpan timeDelta)            // TODO: maybe take in a list of elements so we can combine with DrawCollisionBoxes?
        {
            var elements = World.ElementManager.GetElementsInArea(left: MapX - 1, top: MapY - 1, width: this.ScreenColumns, height: this.ScreenRows); // int top, int left, int width, int height);

            foreach (var element in elements)
            {
                DrawAnimatedElement(element);
            }
        }

        public void DrawCollisionBoxes(TimeSpan timeDelta)
        {
            var elements = World.ElementManager.GetElementsInArea(left: MapX - 1, top: MapY - 1, width: this.ScreenColumns, height: this.ScreenRows); // int top, int left, int width, int height);

            foreach (var element in elements)
            {
                DrawCollisionBox(element);
            }
        }

        private void DrawAnimatedElement(AnimatedElement element)
        {
            var frame = element.CurrentFrame;

            var tint = GetTint((int)element.MapCoordinates.X, (int)element.MapCoordinates.Y);
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

            var sourceRect = new Rectangle(294, 34, 1, 1);  // JANKOTRONICS - this refers to a specific pixel in knight_sword
            var white = new Color(1f, 1f, 1f, 0.3f);
            var frame = element.CurrentFrame;

            var anchor = new PixelCoordinates(0, 0);
            var destRect = GetScreenRectangle(element.MapCoordinates, anchor, 20, 20);

            DrawSprite(element.Texture, sourceRect, destRect, white); 
        }

        private void DrawFrame(Texture2D texture, AnimationFrame frame, MapCoordinates mapCoordinates, Color tint)  // float mapX, float mapY, Color tint)
        {
            var sourceRect = frame.Rectangle;
            var destRect = GetScreenRectangle(mapCoordinates, frame.Anchor, sourceRect.Width, sourceRect.Height);
            DrawSprite(texture, sourceRect, destRect, tint, frame.Flipped);
        }

        private Rectangle GetScreenRectangle(MapCoordinates mapCoordinates, PixelCoordinates anchor, int rawWidth, int rawHeight)
        {
            var screenX = GetScreenX(mapCoordinates.X) - anchor.X;
            var screenY = GetScreenY(mapCoordinates.Y) - anchor.Y;

            var screenWidth = (int)(rawWidth * this.Scale);
            var screenHeight = (int)(rawHeight * this.Scale);

            return new Rectangle(screenX, screenY, screenWidth, screenHeight);
        }

        private void DrawTile(Texture2D texture, Rectangle sourceRect, int mapX, int mapY, Color tint)
        {
            var mapCoordinates = new MapCoordinates(mapX, mapY);
            var destRect = GetScreenRectangle(mapCoordinates, TileAnchor, TileSize, TileSize);
            DrawSprite(texture, sourceRect, destRect, tint);
        }

        private void DrawSprite(Texture2D texture, Rectangle sourceRect, Rectangle destRect, Color tint, bool flipped = false)
        {
            var effect = flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            this.SpriteBatch.Draw(texture, drawRectangle: destRect, sourceRectangle: sourceRect, color: tint, effect: effect);
        }
    }
}
