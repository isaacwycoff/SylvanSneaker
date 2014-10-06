using System;

namespace SylvanSneaker
{
    public enum EntityType
    {
        Knight,
        Door,
        Zombie,
        HealingPotion
    }

    public class BasicEntity: Entity
    {
        EntityType Type;
        float MapX, MapY;
        Controller Controller;

        // we need access to the content manager


        public BasicEntity(EntityType type, float mapX, float mapY, Controller controller)
        {
            this.Type = type;
            this.MapX = mapX;
            this.MapY = mapY;
            this.Controller = controller;
        }

        public void Draw(TimeSpan timeDelta)
        {

        }

        public void Update(TimeSpan timeDelta)
        {

        }

        public void SendCommand(EntityCommand command)
        {
            if (command.HasFlag(EntityCommand.MoveNorth) && command.HasFlag(EntityCommand.MoveEast))
            {

            }

            if (command.HasFlag(EntityCommand.MoveNorth & EntityCommand.MoveEast))
            {

            }
        }
    }
}
