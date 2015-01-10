using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SylvanSneaker.Environment
{
    public class TileSet
    {
        private TileDefinition[] Definitions { get; set; }
        private Rectangle[] SourceRectangles { get; set; }
        public Texture2D Texture { get; private set; }
        public int TileSize { get; private set; }              // size in pixels of each individual tile

        public TileSet(Texture2D texture, TileDefinition[] definitions, int tileSize)
        {
            if (texture == null)
            {
                throw new Exception("Ground texture was null!");
            }
            if (definitions == null)
            {
                throw new Exception("There were no ground texture definitions!");
            }

            this.Texture = texture;
            this.Definitions = definitions;
            this.TileSize = tileSize;
            this.SourceRectangles = GenerateSourceRectangles();
        }

        public TileDefinition this[int i]
        {
            get
            {
                return Definitions[i];
            }
        }

        public Rectangle GetRectangle(int i)
        {
            return this.SourceRectangles[i];
        }

        private Rectangle[] GenerateSourceRectangles()
        {
            var rectangles = new Rectangle[Definitions.Length];

            for (int i = 0; i < Definitions.Length; ++i)
            {
                var definition = Definitions[i];
                rectangles[i] = new Rectangle(definition.Column * TileSize, definition.Row * TileSize, TileSize, TileSize);
            }
            return rectangles;
        }
    }
}
