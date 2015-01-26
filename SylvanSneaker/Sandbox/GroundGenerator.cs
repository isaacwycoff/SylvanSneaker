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
        // todo: this will eventually take in a filename or an XML structure
        public GroundGenerator()
        {

        }

        public TileMap Generate()
        {
            var mapSize = 256;

            var map = new Tile[mapSize, mapSize];

            for (int y = 0; y < mapSize; ++y)
            {
                for (int x = 0; x < mapSize; ++x)
                {
                    var lighting = (byte)Math.Min(x * 16 + 32, 255);

                    var lightLevel = new LightLevel(lighting, lighting, lighting);

                    map[x, y] = new Tile(
                        floorDefinitionId: x % 4, 
                        wallDefinitionId: 0, 
                        collision: CollisionType.None, 
                        lighting: lightLevel);
                }
            }

            return new BasicTileMap(map);
        }

    }
}
