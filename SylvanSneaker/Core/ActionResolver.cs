using SylvanSneaker.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SylvanSneaker.Core
{
    public interface ActionResolver
    {
        MapCoordinates AttemptToMove(MapCoordinates currentCoordinates, MapCoordinates difference);     // TODO: how do we know that we're not colliding with ourself? or a friendly object that we should be able to walk through?
    }

    public class BasicActionResolver : ActionResolver
    {
        private EntityManager EntityManager { get; set; }
        private TileMap Ground { get; set; }
        private int TileSize { get; set; }

        public BasicActionResolver(EntityManager entityManager, TileMap ground, int tileSize)
        {
            this.EntityManager = entityManager;
            this.Ground = ground;
            this.TileSize = tileSize;
        }

        public MapCoordinates AttemptToMove(MapCoordinates currentCoordinates, MapCoordinates difference)
        {
            var attemptedPosition = currentCoordinates.Move(difference);

            float actualX = attemptedPosition.X;
            float actualY = attemptedPosition.Y;

            if (attemptedPosition.X < 0f)
            {
                actualX = 0f;
            }
            else if (attemptedPosition.X > Ground.MapWidth * TileSize)
            {
                actualX = Ground.MapWidth * TileSize;
            }
            if (attemptedPosition.Y < 0f)
            {
                actualY = 0f;
            }
            else if (attemptedPosition.Y > Ground.MapHeight * TileSize)
            {
                actualY = Ground.MapHeight * TileSize;
            }

            return new MapCoordinates(actualX, actualY);
            // return currentCoordinates.Move(difference);
        }
    }

    public class MapCoordinates
    {
        public float X { get; private set; }
        public float Y { get; private set; }

        public MapCoordinates(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public MapCoordinates Move(MapCoordinates difference)
        {
            return new MapCoordinates(X + difference.X, Y + difference.Y);
        }
    }

    public class FloatRectangle
    {
        float Left;
        float Top;
        float Width;
        float Height;

    }
}
