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

        public Ground Generate()
        {
            var mapSize = 256;

            var map = new Tile[mapSize, mapSize];

            for (int y = 0; y < mapSize; ++y)
            {
                for (int x = 0; x < mapSize; ++x)
                {
                    var lighting = (byte)Math.Min(x * 16 + 32, 255);

                    map[x, y] = new Tile(x % 4, new LightLevel(lighting, lighting, lighting));
                }
            }

            return new BasicGround(map);
        }

    }
}
