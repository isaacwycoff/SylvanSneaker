using Microsoft.Xna.Framework;
using SylvanSneaker.Environment;
using SylvanSneaker.Slots;
using System;

namespace SylvanSneaker.Core
{
    public interface IWorld
    {
        TileMap Ground { get; }
        TileSet TileSet { get; }
        ElementManager ElementManager { get; }
        EntityManager EntityManager { get; }
        void Update(GameTime gameTime);
        Entity AddEntity(EntityType type, float mapX, float mapY, Controller controller);
    }

    public class World : IWorld
    {
        public TileMap Ground { get; private set; }
        public TileSet TileSet { get; private set; }

        public ElementManager ElementManager { get; private set; }
        public EntityManager EntityManager { get; private set; }

        public World()
        {
            var tileSize = 32;

            var tileDefinitions = new TileDefinition[5] {
                new TileDefinition(2, 3),
                new TileDefinition(3, 3),
                new TileDefinition(0, 2),
                new TileDefinition(0, 3),
                new TileDefinition(0, 4),
            };
            this.TileSet = new TileSet(TextureSlot.GetTexture(TextureName.GROUND), tileDefinitions, tileSize);

            var generator = new GroundGenerator();
            this.Ground = generator.Generate();

            this.ElementManager = new ElementManager();

            this.EntityManager = new EntityManager(this.ElementManager);

            PhysicsSlot.Initialize(new BasicActionResolver(this.EntityManager, this.Ground, tileSize));         // TODO: move this somewhere more obvious
        }

        public void Update(GameTime gameTime)
        {
            var timeElapsed = gameTime.ElapsedGameTime;
            EntityManager.Update(timeElapsed);
            ElementManager.Update(timeElapsed);
            Ground.Update(timeElapsed);
        }

        public Entity AddEntity(EntityType type, float mapX, float mapY, Controller controller)
        {
            return EntityManager.Add(type, mapX, mapY, controller);
        }

        public Entity AddEntity(EntityType type, float mapX, float mapY, Func<Controller> getController)
        {
            var controller = getController();
            return EntityManager.Add(type, mapX, mapY, controller);
        }
    }
}
