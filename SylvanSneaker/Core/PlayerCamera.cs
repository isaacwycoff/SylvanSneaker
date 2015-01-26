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

        public PlayerCamera(IWorld world, Entity attachedTo, SpriteBatch spriteBatch, int width, int height, float zoom)
        {
            this.World = world;
            this.AttachedTo = attachedTo;
            this.SpriteBatch = spriteBatch;
            this.Width = width;
            this.Height = height;
            this.Zoom = zoom;
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

            DrawDumbTriangles(zoomTranslation * cameraTranslation);
        }

        private float DumbRotation = 0f;

        private void DrawDumbTriangles(Matrix cameraTranslation)
        {
            var GraphicsDevice = SpriteBatch.GraphicsDevice;

            VertexBuffer vertexBuffer;

            BasicEffect basicEffect;
            Matrix world = Matrix.CreateRotationZ(DumbRotation) * cameraTranslation;           //  Matrix.CreateTranslation(0, 0, 0);   

            Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 100), new Vector3(0, 0, 0), new Vector3(0, 1, 0));

            Matrix projection = Matrix.CreateOrthographicOffCenter(0, this.Width, this.Height, 0, 100f, -100f);

            basicEffect = new BasicEffect(GraphicsDevice);

            var red = new Color(Color.Red, 1.0f);
            var green = new Color(Color.DarkOrange, 0.0f);
            var blue = new Color(Color.Blue, 1.0f);

            VertexPositionColor[] vertices = new VertexPositionColor[] {
                new VertexPositionColor(new Vector3(MapX, MapY, 0), red),
                new VertexPositionColor(new Vector3(+100f + MapX, 200f + MapY, 0), green),
                new VertexPositionColor(new Vector3(-100f + MapX, 200f + MapY, 0), green),
            };

            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), vertices.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionColor>(vertices);

            basicEffect.World = world;
            basicEffect.View = view;
            basicEffect.Projection = projection;
            basicEffect.VertexColorEnabled = true;

            GraphicsDevice.SetVertexBuffer(vertexBuffer);

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;
            GraphicsDevice.BlendState = BlendState.Additive;

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 1);
            }
        }

        private Matrix GetCameraTranslation(float zoom)
        {
            var xOffset = (this.MapX * zoom) - (float)this.Width / 2f;
            var yOffset = (this.MapY * zoom) - (float)this.Height / 2f;

            return Matrix.CreateTranslation(new Vector3(-xOffset * 1f, -yOffset * 1f, 0f));
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
