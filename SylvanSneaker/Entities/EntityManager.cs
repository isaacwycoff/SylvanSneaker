using SylvanSneaker.Core;
using System;
using System.Collections.Generic;

namespace SylvanSneaker
{
    class EntityManager
    {
        List<Entity> Entities;
        TextureManager TextureManager;
        ElementManager ElementManager;

        public EntityManager(TextureManager textureManager, ElementManager elementManager)
        {
            Entities = new List<Entity>();
            this.TextureManager = textureManager;
            this.ElementManager = elementManager;
        }

        public Entity Add(EntityType type, float mapX, float mapY, Controller controller)
        {
            Entity entity = new BasicEntity(type, mapX, mapY, this.ElementManager);
            controller.ControlledEntity = entity;

            Entities.Add(entity);
            return entity;
        }



        public void Update(TimeSpan timeDelta)
        {
            foreach(Entity entity in this.Entities) {
                entity.Update(timeDelta);
            }
        }

    }
}
