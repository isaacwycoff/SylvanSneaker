using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SylvanSneaker.Core;
using SylvanSneaker.Environment;
using SylvanSneaker.Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SylvanSneaker.Slots
{
    public static class WorldSlot
    {
        private static IWorld GameWorld;

        public static void Initialize(IWorld world)
        {
            GameWorld = world;

        }

        public static void Shutdown()
        {

        }

        public static void Update(GameTime gameTime)
        {
            GameWorld.Update(gameTime);
        }

        public static Entity AddEntity(EntityType type, float mapX, float mapY, Controller controller)
        {
            return GameWorld.AddEntity(type, mapX, mapY, controller);
        }

        public static LightLevel GetLightLevel(MapCoordinates coordinates)
        {
            return GameWorld.Ground.GetLightLevel((int)(coordinates.X), (int)(coordinates.Y));
        }

        public static IList<AnimatedElement> GetElementsInArea(float left, float top, int width, int height)
        {
            return GameWorld.ElementManager.GetElementsInArea(left, top, width, height);
        }

        public static Texture2D GetFloorTexture()
        {
            return GameWorld.TileSet.Texture;
        }

        public static Rectangle GetTileSourceRect(int id)
        {
            return GameWorld.TileSet.GetRectangle(id);
        }

        public static Tile[,] Map
        {
            get
            {
                return GameWorld.Ground.Map;
            }
        }

        public static int TileWidth
        {
            get
            {
                return GameWorld.Ground.MapWidth;
            }
        }

        public static int TileHeight
        {
            get
            {
                return GameWorld.Ground.MapHeight;
            }
        }
    }
}
