using SylvanSneaker.Core;
using System;
using System.Collections.Generic;

namespace SylvanSneaker
{
    public class EntityManager
    {
        List<Entity> Entities;
        ElementManager ElementManager;

        public EntityManager(ElementManager elementManager)
        {
            Entities = new List<Entity>();
            this.ElementManager = elementManager;
        }

        public Entity Add(EntityType type, float mapX, float mapY, Controller controller, ActionResolver resolver)
        {
            Entity entity = new BasicEntity(type, mapX, mapY, resolver, this.ElementManager);
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
