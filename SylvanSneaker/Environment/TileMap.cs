﻿using SylvanSneaker.Core;
using System;

namespace SylvanSneaker.Environment
{
    [Flags]
    public enum Direction {
        North = 1,
        East = 2,
        South = 4,
        West = 8,
    }

    public interface TileMap: WorldElement, WorldLighting
    {
        int MapWidth { get; }
        int MapHeight { get; }
        Tile[,] Map { get; }
    }

    internal class BasicTileMap: TileMap
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

        private MapCoordinates _MapCoordinates = new MapCoordinates(0f, 0f);
        public MapCoordinates MapCoordinates { get { return _MapCoordinates; } }

        // public float MapX { get { return 0f; } }
        // public float MapY { get { return 0f; } }

        public BasicTileMap(Tile[,] map)
        {
            this.Map = map;
        }

        public LightLevel GetLightLevel(int x, int y)
        {
            if (x < 0) x = 0;
            if (x > Map.GetLength(0) - 1) x = Map.GetLength(0) - 1;
            if (y < 0) y = 0;
            if (y > Map.GetLength(1) - 1) y = Map.GetLength(1) - 1;

            return Map[x, y].Lighting;
        }

        public void Update(TimeSpan timeDelta)
        {
            // throw new NotImplementedException();
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
        public int FloorDefinitionId { get; private set; }
        public int WallDefinitionId { get; private set; }
        public CollisionType Collision { get; private set; }
        public LightLevel Lighting { get; private set; }

        public Tile(int floorDefinitionId, int wallDefinitionId, CollisionType collision, LightLevel lighting)
        {
            this.FloorDefinitionId = floorDefinitionId;
            this.WallDefinitionId = wallDefinitionId;
            this.Collision = collision;
            this.Lighting = lighting;
        }
    }

    public enum CollisionType
    {
        None = 0,
        BlockGround = 1,
        BlockAir = 2
    };


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
}
