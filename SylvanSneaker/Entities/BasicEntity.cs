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
        public Camera Camera { private get; set; }

        private ElementManager ElementManager { get; set; }
        private AnimatedElement Element { get; set; }
        // we need access to the content manager

        public BasicEntity(EntityType type, float mapX, float mapY, ElementManager elementManager)
        {
            this.Type = type;
            this.MapX = mapX;
            this.MapY = mapY;
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
            if (command == EntityCommand.MoveSouth)
            {
                this.MapY += (float)0.1;
                this.Element.CurrentAnimation = AnimationId.WalkSouth;
            }

            if (command == EntityCommand.MoveEast)
            {
                this.MapX += (float)0.1;
                this.Element.CurrentAnimation = AnimationId.WalkEast;
            }

            if (command == EntityCommand.MoveNorth)
            {
                this.MapY -= (float)0.1;
                this.Element.CurrentAnimation = AnimationId.WalkNorth;
            }

            if (command == EntityCommand.MoveWest)
            {
                this.MapX -= (float)0.1;
                this.Element.CurrentAnimation = AnimationId.WalkWest;
            }
            if (command.HasFlag(EntityCommand.MoveNorth) && command.HasFlag(EntityCommand.MoveEast))
            {

            }

            if (command.HasFlag(EntityCommand.MoveNorth & EntityCommand.MoveEast))
            {

            }
        }
    }
}
