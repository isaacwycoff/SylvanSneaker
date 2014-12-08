using SylvanSneaker.Core;
using SylvanSneaker.Sandbox;
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
        private EntityType Type;
        public float MapX { get; private set; }
        public float MapY { get; private set; }
        private Controller Controller;
        public Camera Camera { private get; set; }

        private ElementManager ElementManager { get; set; }
        private AnimatedElement Element { get; set; }
        // we need access to the content manager

        public BasicEntity(EntityType type, float mapX, float mapY, ElementManager elementManager, Controller controller)
        {
            this.Type = type;
            this.MapX = mapX;
            this.MapY = mapY;
            this.Controller = controller;
            this.ElementManager = elementManager;

            var texture = TextureName.KNIGHT;

            this.Element = this.ElementManager.Add(MapX, MapY, texture);
        }

        public void Update(TimeSpan timeDelta)
        {
            this.Element.MapX = this.MapX;
            this.Element.MapY = this.MapY;
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
