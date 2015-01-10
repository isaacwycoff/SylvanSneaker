using SylvanSneaker.Core;
using SylvanSneaker.Environment;
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

    [Flags]
    public enum Action
    {
        Idle = 1,
        Walk = 2,
        Attack = 4,
        Flinch = 8,
        Die = 16,
    }

    public class BasicEntity: Entity
    {
        Direction CurrentDirection;
        Action CurrentAction;

        private EntityType Type;
        public float MapX { get; private set; }
        public float MapY { get; private set; }
        public Camera Camera { private get; set; }

        private int WalkSpeed { get; set; }

        private ElementManager ElementManager { get; set; }
        private AnimatedElement Element { get; set; }
        // we need access to the content manager

        public BasicEntity(EntityType type, float mapX, float mapY, ElementManager elementManager, Direction direction = Direction.South)
        {
            this.Type = type;
            this.MapX = mapX;
            this.MapY = mapY;
            this.ElementManager = elementManager;

            this.CurrentDirection = direction;
            this.CurrentAction = Action.Idle;
            this.WalkSpeed = 10;

            var texture = TextureName.KNIGHT;

            this.Element = this.ElementManager.Add(MapX, MapY, texture);
        }

        public void Update(TimeSpan timeDelta)
        {
            if (this.CurrentAction == Action.Walk)
            {
                Walk(timeDelta);
            }
            else
            {
                Idle(timeDelta);
            }

            this.Element.MapX = this.MapX;
            this.Element.MapY = this.MapY;
        }

        private void Idle(TimeSpan timeDelta)
        {
            if (CurrentDirection.HasFlag(Direction.South))
            {
                Element.CurrentAnimation = AnimationId.IdleSouth;
            }
            else if (CurrentDirection.HasFlag(Direction.North))
            {
                Element.CurrentAnimation = AnimationId.IdleNorth;
            }
            else if (CurrentDirection.HasFlag(Direction.East))
            {
                Element.CurrentAnimation = AnimationId.IdleEast;
            }
            else if (CurrentDirection.HasFlag(Direction.West))
            {
                Element.CurrentAnimation = AnimationId.IdleWest;
            }
        }

        private void Walk(TimeSpan timeDelta)
        {
            // this whole thing makes me feel gross:
            const float Cos45 = (float)0.707;

            var walkDistance = (float)(WalkSpeed * timeDelta.TotalSeconds);

            if (CurrentDirection.HasFlag(Direction.South))
            {
                Element.CurrentAnimation = AnimationId.WalkSouth;
                if (CurrentDirection.HasFlag(Direction.East))
                {
                    this.MapY += (walkDistance * Cos45);
                    this.MapX += (walkDistance * Cos45);
                }
                else if (CurrentDirection.HasFlag(Direction.West))
                {
                    this.MapY += (walkDistance * Cos45);
                    this.MapX -= (walkDistance * Cos45);
                }
                else
                {
                    this.MapY += walkDistance;
                }
            }
            else if (CurrentDirection.HasFlag(Direction.North))
            {
                Element.CurrentAnimation = AnimationId.WalkNorth;
                if (CurrentDirection.HasFlag(Direction.East))
                {
                    this.MapY -= (walkDistance * Cos45);
                    this.MapX += (walkDistance * Cos45);
                }
                else if (CurrentDirection.HasFlag(Direction.West))
                {
                    this.MapY -= (walkDistance * Cos45);
                    this.MapX -= (walkDistance * Cos45);
                }
                else
                {
                    this.MapY -= walkDistance;
                }
            }
            else if (CurrentDirection.HasFlag(Direction.East))
            {
                Element.CurrentAnimation = AnimationId.WalkEast;
                this.MapX += walkDistance;
            }
            else if (CurrentDirection.HasFlag(Direction.West))
            {
                Element.CurrentAnimation = AnimationId.WalkWest;
                this.MapX -= walkDistance;
            }
        }

        public void SendCommand(EntityCommand command)
        {
            var isWalking = false;
            var isFiring = false;

            Direction newDirection = 0;

            if (command.HasFlag(EntityCommand.MoveSouth))
            {
                isWalking = true;
                newDirection = newDirection | Direction.South;
            }

            if (command.HasFlag(EntityCommand.MoveEast))
            {
                isWalking = true;
                newDirection = newDirection | Direction.East;
            }

            if (command.HasFlag(EntityCommand.MoveNorth))
            {
                isWalking = true;
                newDirection = (newDirection | Direction.North) & ~Direction.South;
            }

            if (command.HasFlag(EntityCommand.MoveWest))
            {
                isWalking = true;
                newDirection = (newDirection | Direction.West) & ~Direction.East;
            }

            if (isWalking)
            {
                this.CurrentAction = Action.Walk;
            }
            else
            {
                this.CurrentAction = Action.Idle;
            }

            if (newDirection != 0) { this.CurrentDirection = newDirection; }
        }
    }
}
