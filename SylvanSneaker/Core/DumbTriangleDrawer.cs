using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SylvanSneaker.Core
{
    class DumbTriangleDrawer
    {
        private SpriteBatch SpriteBatch;

        public DumbTriangleDrawer(SpriteBatch spriteBatch)
        {
            this.SpriteBatch = spriteBatch;
        }

        public void DrawDumbTriangles(Matrix cameraTranslation, int width, int height)
        {

            var GraphicsDevice = SpriteBatch.GraphicsDevice;

            VertexBuffer vertexBuffer;

            BasicEffect basicEffect;
            Matrix world = Matrix.CreateRotationZ(0f) * cameraTranslation;           //  Matrix.CreateTranslation(0, 0, 0);   

            Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 100), new Vector3(0, 0, 0), new Vector3(0, 1, 0));

            Matrix projection = Matrix.CreateOrthographicOffCenter(0, width, height, 0, 100f, -100f);

            basicEffect = new BasicEffect(GraphicsDevice);

            var red = new Color(Color.Red, 1.0f);
            var green = new Color(Color.DarkOrange, 0.0f);
            var blue = new Color(Color.Blue, 1.0f);

            VertexPositionColor[] vertices = new VertexPositionColor[] {
                new VertexPositionColor(new Vector3(0, 0, 0), red),
                new VertexPositionColor(new Vector3(+100f, 200f, 0), green),
                new VertexPositionColor(new Vector3(-100f, 200f, 0), green),
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
    }
}
