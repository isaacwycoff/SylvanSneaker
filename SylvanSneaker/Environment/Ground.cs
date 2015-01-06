using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SylvanSneaker.Environment
{
    [Flags]
    public enum Direction {
        North = 1,
        East = 2,
        South = 4,
        West = 8,
    }

    public interface Ground: WorldElement, WorldLighting
    {
        int ScreenRows { get; }
        int ScreenColumns { get; }
        int MapWidth { get; }
        int MapHeight { get; }
        Tile[,] Map { get; }
    }

    internal class BasicGround: Ground
    {
        public Tile[,] Map { get; private set; }

        public int MapWidth
        {
            get {
                return Map.GetLength(0);
            }
        }

        public int MapHeight
        {
            get
            {
                return Map.GetLength(1);
            }
        }

        public int ScreenRows { get; private set; }
        public int ScreenColumns { get; private set; }

        private int TileSize { get; set; }

        private int OffsetX { get; set; }
        private int OffsetY { get; set; }

        private int ScreenX { get; set; }
        private int ScreenY { get; set; }

        public float MapX { get; private set; }         // may not be necessary, in which case we should not be a WorldElement
        public float MapY { get; private set; }         // or WorldElement shouldn't have these

        // public Camera Camera { private get; set; }

        public BasicGround(Tile[,] map)
        {
            /*
            if (tileSet == null)
            {
                throw new Exception("Missing a tileSet!");
            } 
            if (spriteBatch == null)
            {
                throw new Exception("Missing a SpriteBatch for Ground");
            } */
            if (map == null)
            {
                throw new Exception("Map was null!");
            }

            this.Map = map;
            
            this.ScreenRows = 20;
            this.ScreenColumns = 28;
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


        public void Draw(TimeSpan timeDelta, Camera camera)
        {
            throw new NotImplementedException();
        }

        public void Update(TimeSpan timeDelta)
        {
            // throw new NotImplementedException();
        }

        public LightLevel GetLightLevel(int x, int y)
        {
            if (x < 0) x = 0;
            if (x > Map.GetLength(0) - 1) x = Map.GetLength(0) - 1;
            if (y < 0) y = 0;
            if (y > Map.GetLength(1) - 1) y = Map.GetLength(1) - 1;

            return Map[x, y].Lighting;
        }
    }

    public class LightLevel
    {
        public byte Red { get; private set; }
        public byte Green { get; private set; }
        public byte Blue { get; private set; }

        public LightLevel(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }
    }

    public class Tile
    {
        public int DefinitionId { get; private set; }
        public LightLevel Lighting { get; private set; }

        public Tile(int definitionId, LightLevel lighting)
        {
            this.DefinitionId = definitionId;
            this.Lighting = lighting;
        }
    }

    public class TileDefinition
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

    public class TileSet
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
