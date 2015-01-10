using Microsoft.Xna.Framework;
using SylvanSneaker.Environment;

namespace SylvanSneaker.Core
{
    public interface IWorld
    {
        Ground Ground { get; }
        TileSet TileSet { get; }
        ElementManager ElementManager { get; }
        EntityManager EntityManager { get; }
        void Update(GameTime gameTime);
        Entity AddEntity(EntityType type, float mapX, float mapY, Controller controller);
    }

    public class World : IWorld
    {
        public Ground Ground { get; private set; }
        public TileSet TileSet { get; private set; }

        public ElementManager ElementManager { get; private set; }
        public EntityManager EntityManager { get; private set; }

        public World(TileSet tileSet, Ground ground, ElementManager elementManager, EntityManager entityManager)
        {
            this.TileSet = tileSet;
            this.Ground = ground;
            this.ElementManager = elementManager;
            this.EntityManager = entityManager;
        }

        public World(TextureManager textureManager)
        {
            var tileSize = 32;

            var tileDefinitions = new TileDefinition[5] {
                new TileDefinition(0, 2),
                new TileDefinition(0, 1),
                new TileDefinition(0, 2),
                new TileDefinition(0, 3),
                new TileDefinition(0, 4),
            };
            this.TileSet = new TileSet(textureManager[TextureName.GROUND], tileDefinitions, tileSize);

            var generator = new GroundGenerator();
            this.Ground = generator.Generate();

            this.ElementManager = new ElementManager(textureManager);

            this.EntityManager = new EntityManager(this.ElementManager);
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

    }
}
