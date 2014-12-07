using System;
using System.Collections.Generic;

namespace SylvanSneaker
{
    class EntityManager
    {
        List<Entity> Entities;

        public EntityManager()
        {
            Entities = new List<Entity>();
        }

        public Entity NewEntity(EntityType type, float mapX, float mapY, Controller controller)
        {
            Entity entity = new BasicEntity(type, mapX, mapY, controller);
            Entities.Add(entity);
            return entity;            
        }

        public void Update(TimeSpan timeDelta)
        {
            foreach(Entity entity in this.Entities) {
                entity.Update(timeDelta);
            }
        }

        // only ELEMENTS should be drawn

        /*
        public void Draw(TimeSpan timeDelta)
        {
            foreach (Entity entity in this.Entities) {
                entity.Draw(timeDelta);
            }
            
        }
        */
    }
}
