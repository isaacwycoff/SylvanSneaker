using Microsoft.Xna.Framework.Graphics;
using SylvanSneaker.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SylvanSneaker.Environment
{
    public class GroundGenerator
    {
        SpriteBatch Batch;

        // todo: this will eventually take in a filename or an XML structure
        public GroundGenerator(SpriteBatch batch)   // Texture2D groundTexture, int tileSize)
        {
            this.Batch = batch;
        }

        public Ground Generate(Texture2D groundTexture, int tileSize, Camera camera)
        {
            var tileDefinitions = new TileDefinition[5] {
                new TileDefinition(0, 2),
                new TileDefinition(0, 1),
                new TileDefinition(0, 2),
                new TileDefinition(0, 3),
                new TileDefinition(0, 4),
            };

            var tileSet = new TileSet(groundTexture, tileDefinitions, tileSize);

            var mapSize = 32;

            var map = new Tile[mapSize, mapSize];

            for (int y = 0; y < mapSize; ++y)
            {
                for (int x = 0; x < mapSize; ++x)
                {
                    var lighting = (byte)Math.Min(x * 16 + 32, 255);

                    map[x, y] = new Tile(0, new LightLevel(lighting, lighting, lighting));
                }
            }

            return new BasicGround(tileSet, Batch, map, camera);            
        }

    }
}
