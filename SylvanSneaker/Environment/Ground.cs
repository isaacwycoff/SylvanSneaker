using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SylvanSneaker.Environment
{
    public enum Direction {
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest,
    }

    class Ground: Element
    {
        private SpriteBatch SpriteBatch;

        private TileSet TileSet;

        private Tile[,] Map;

        private int ScreenRows { get; set; }
        private int ScreenColumns { get; set; }

        private int TileSize { get; set; }

        private int OffsetX { get; set; }
        private int OffsetY { get; set; }

        private int ScreenX { get; set; }
        private int ScreenY { get; set; }

        public Ground(TileSet tileSet, SpriteBatch spriteBatch, Tile[,] map)
        {
            if (tileSet == null)
            {
                throw new Exception("Missing a tileSet!");
            }
            if (spriteBatch == null)
            {
                throw new Exception("Missing a SpriteBatch for Ground");
            }
            if (map == null)
            {
                throw new Exception("Map was null!");
            }

            this.TileSet = tileSet;
            this.SpriteBatch = spriteBatch;
            this.Map = map;

            // FIXME: should be programmatically determined and should be able to change AFTER the game has started:
            this.ScreenRows = 10;
            this.ScreenColumns = 14;
            this.TileSize = 64;
            this.OffsetX = 0;
            this.OffsetY = 0;
            this.ScreenX = 0;
            this.ScreenY = 0;
        }

        // FIXME: test method. Eventually I'll tie this to a Camera object
        public void ShiftCamera(Direction direction)
        {
            var speed = 4;

            switch (direction)
            {
                case Direction.North:
                    this.ScreenY = this.ScreenY - speed;
                    if (this.ScreenY < -TileSize)
                    {
                        this.ScreenY = this.ScreenY + TileSize;
                        this.OffsetY = this.OffsetY - 1;
                    }
                    break;
                case Direction.East:                                // this one seems to work properly
                    this.ScreenX = this.ScreenX - speed;
                    if (this.ScreenX < -TileSize)
                    {
                        this.ScreenX = this.ScreenX + TileSize;
                        this.OffsetX = this.OffsetX + 1;
                    }
                    break;
                case Direction.South:
                    this.ScreenY = this.ScreenY + speed;
                    if (this.ScreenY > TileSize)
                    {
                        this.ScreenY = this.ScreenY - TileSize;
                        this.OffsetY = this.OffsetY + 1;
                    }
                    break;
                case Direction.West:
                    this.ScreenX = this.ScreenX - speed;
                    if (this.ScreenX < -TileSize)
                    {
                        this.ScreenX = this.ScreenX + TileSize;
                        this.OffsetX = this.OffsetX - 1;
                    }
                    break;
                default:
                    break;
            }

        }

        public void Draw(TimeSpan timeDelta)
        {
            int maxX = Math.Min(ScreenColumns, Map.GetLength(0) - OffsetX);
            int maxY = Math.Min(ScreenColumns, Map.GetLength(1) - OffsetY);

            for (int y = 0; y < maxY; ++y)
            {
                for (int x = 0; x < maxX; ++x)
                {
                    var tile = Map[x + OffsetX, y + OffsetY];

                    var sourceRect = TileSet.GetRectangle(tile.DefinitionId);
                    var destRect = new Rectangle((x * TileSize) + ScreenX, (y * TileSize) + ScreenY, TileSize, TileSize);

                    var brightness = Math.Min(tile.LightLevel, 255);

                    var tint = new Color(brightness, brightness, brightness, 255);
                    
                    SpriteBatch.Draw(TileSet.Texture, destRect, sourceRect, tint);
                }
            }
        }

    }

    class Tile
    {
        public int DefinitionId { get; private set; }
        public int LightLevel { get; private set; }             // might want to sub-divide? idunno.

        public Tile(int definitionId, int lightLevel)
        {
            this.DefinitionId = definitionId;
            this.LightLevel = lightLevel;
        }
    }

    class TileDefinition
    {
        public int Row { get; private set; }
        public int Column { get; private set; }
        // various flags - damage, etcetera

        public TileDefinition(int row, int column)
        {
            this.Row = row;
            this.Column = column;
        }
    }

    class TileSet
    {
        private TileDefinition [] Definitions { get; set; }
        private Rectangle[] SourceRectangles { get; set; }
        public Texture2D Texture { get; private set; }
        public int TileSize { get; private set; }              // size in pixels of each individual tile

        public TileSet (Texture2D texture, TileDefinition [] definitions, int tileSize)
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
            get {
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
