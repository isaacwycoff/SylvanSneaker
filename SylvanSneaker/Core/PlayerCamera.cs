using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SylvanSneaker.Sandbox;
using SylvanSneaker.Slots;
using System;

namespace SylvanSneaker.Core
{
    class PlayerCamera: Camera
    {
        // private IWorld World { get; set; }
        private Entity AttachedTo { get; set; }
        private SpriteBatch SpriteBatch { get; set; }

        private DumbTriangleDrawer TriangleDrawer { get; set; }

        private float MapX
        {
            get {
                return AttachedTo.MapCoordinates.X;
            }
        }

        private float MapY
        {
            get { 
                return AttachedTo.MapCoordinates.Y;
            }
        }

        private int Width { get; set; }
        private int Height { get; set; }

        private float Zoom { get; set; }

        public PlayerCamera(Entity attachedTo, SpriteBatch spriteBatch, int width, int height, float zoom)
        {
            // this.World = world;
            this.AttachedTo = attachedTo;
            this.SpriteBatch = spriteBatch;
            this.Width = width;
            this.Height = height;
            this.Zoom = zoom;

            this.TriangleDrawer = new DumbTriangleDrawer(spriteBatch);
        }

        public float TileSize = 32;     //  { get { return TILE_SIZE; } }  

        private PixelCoordinates TileAnchor = new PixelCoordinates(0, 0);

        public void Draw(GameTime gameTime)
        {
            var cameraTranslation = this.GetCameraTranslation(Zoom);
            var zoomTranslation = Matrix.CreateScale(Zoom);

            SpriteBatch.Begin(sortMode: SpriteSortMode.Deferred,          // TODO: Research
                blendState: BlendState.AlphaBlend,              // blend alphas - i.e., transparencies
                samplerState: SamplerState.PointClamp,                                // samplerState: SamplerState.PointClamp,            // turn off magnification blurring
                depthStencilState: DepthStencilState.Default,          //
                rasterizerState: RasterizerState.CullNone,
                effect: null,
                transformMatrix: zoomTranslation * cameraTranslation
                );

            var timeElapsed = gameTime.ElapsedGameTime;
            DrawGround();
            DrawElements(timeElapsed);

            SpriteBatch.End();

            this.TriangleDrawer.DrawDumbTriangles(zoomTranslation * cameraTranslation, this.Width, this.Height);

            // DrawDumbTriangles(zoomTranslation * cameraTranslation);
        }

        private Matrix GetCameraTranslation(float zoom)
        {
            var xOffset = (this.MapX * zoom) - (float)this.Width / 2f;
            var yOffset = (this.MapY * zoom) - (float)this.Height / 2f;

            return Matrix.CreateTranslation(new Vector3(-xOffset * 1f, -yOffset * 1f, 0f));
        }

        private void DrawGround()
        {
            int minY = Math.Max((int)((MapY - this.Height / 2) / TileSize), 0);
            int minX = Math.Max((int)((MapX - this.Width / 2) / TileSize), 0);
            int maxX = Math.Min((int)((MapX + this.Width / 2) / TileSize), WorldSlot.TileWidth);
            int maxY = Math.Min((int)((MapY + this.Height / 2) / TileSize), WorldSlot.TileHeight);

            for (int y = minY; y < maxY; ++y)            // World.Ground.MapWidth; ++y)
            {
                for (int x = minX; x < maxX; ++x)            // World.Ground.MapHeight; ++x)
                {
                    var tile = WorldSlot.Map[x, y];

                    var sourceRect = WorldSlot.GetTileSourceRect(tile.FloorDefinitionId);

                    // var sourceRect = World.TileSet.GetRectangle(tile.FloorDefinitionId);

                    var tint = new Color(tile.Lighting.Red, tile.Lighting.Green, tile.Lighting.Blue, 255);

                    DrawTile(WorldSlot.GetFloorTexture(), sourceRect, x, y, tint);
                }
            }
        }

        public void DrawElements(TimeSpan timeDelta)            // TODO: maybe take in a list of elements so we can combine with DrawCollisionBoxes?
        {
            var elements = WorldSlot.GetElementsInArea(left: MapX - this.Width / 2, top: MapY - this.Height / 2, width: this.Width, height: this.Height);       // this.ScreenRows); // int top, int left, int width, int height);

            foreach (var element in elements)
            {
                DrawAnimatedElement(element);
            }
        }

        private void DrawAnimatedElement(AnimatedElement element)
        {
            var frame = element.CurrentFrame;

            var tint = GetTint(new MapCoordinates(element.MapCoordinates.X / (float)TileSize, element.MapCoordinates.Y / (float)TileSize));
            // var tint = GetTint((int)(element.MapCoordinates.X / TileSize), (int)(element.MapCoordinates.Y / TileSize));
            DrawFrame(element.Texture, frame, element.MapCoordinates, tint);
        }

        private Color GetTint(MapCoordinates coordinates)
        {
            var lighting = WorldSlot.GetLightLevel(coordinates);
            return new Color(lighting.Red, lighting.Green, lighting.Blue);
        }

        private void DrawFrame(Texture2D texture, AnimationFrame frame, MapCoordinates mapCoordinates, Color tint)  // float mapX, float mapY, Color tint)
        {
            var sourceRect = frame.Rectangle;
            DrawSprite(texture, new Vector2(mapCoordinates.X, mapCoordinates.Y), sourceRect, new Vector2(frame.Anchor.X, frame.Anchor.Y), tint, frame.Flipped); 
        }

        private void DrawTile(Texture2D texture, Rectangle sourceRect, float mapX, float mapY, Color tint)
        {
            DrawSprite(texture, new Vector2(mapX * TileSize, mapY * TileSize), sourceRect, Vector2.Zero, tint); 
        }

        private void DrawSprite(Texture2D texture, Vector2 position, Rectangle sourceRect, Vector2 origin, Color tint, bool flipped = false)
        {
            var effect = flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            var depth = 0f;

            this.SpriteBatch.Draw(
                texture: texture,
                position: position,
                sourceRectangle: sourceRect,
                color: tint, rotation: 0f,
                origin: origin,
                scale: 1.00f,
                effect: effect,
                depth: depth);
        }
    }
}
