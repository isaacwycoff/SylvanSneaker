using Microsoft.Xna.Framework;
using SylvanSneaker.Collision;
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
        public MapCoordinates MapCoordinates { get; private set; }
        public Camera Camera { private get; set; }

        private float WalkSpeed { get; set; }

        private ActionResolver Resolver { get; set; }
        private ElementManager ElementManager { get; set; }
        private AnimatedElement Element { get; set; }
        private CollisionBox CollisionBox { get; set; }
        // we need access to the content manager

        public BasicEntity(EntityType type, float mapX, float mapY, ActionResolver resolver, ElementManager elementManager, Direction direction = Direction.South)
        {
            this.Type = type;
            this.MapCoordinates = new MapCoordinates(mapX, mapY);
            this.Resolver = resolver;
            this.ElementManager = elementManager;

            this.CurrentDirection = direction;
            this.CurrentAction = Action.Idle;
            this.WalkSpeed = 0.200f;

            var texture = TextureName.KNIGHT;

            this.Element = this.ElementManager.Add(this.MapCoordinates, texture);
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

            this.Element.MapCoordinates = MapCoordinates;
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

            var walkDistance = WalkSpeed * (float)(timeDelta.Milliseconds);

            var diffMapX = 0f;
            var diffMapY = 0f;

            if (CurrentDirection.HasFlag(Direction.South))
            {
                Element.CurrentAnimation = AnimationId.WalkSouth;
                if (CurrentDirection.HasFlag(Direction.East))
                {
                    diffMapX = walkDistance * Cos45;
                    diffMapY = walkDistance * Cos45;
                }
                else if (CurrentDirection.HasFlag(Direction.West))
                {
                    diffMapX = -(walkDistance * Cos45);
                    diffMapY = walkDistance * Cos45;
                }
                else
                {
                    diffMapY = walkDistance;
                }
            }
            else if (CurrentDirection.HasFlag(Direction.North))
            {
                Element.CurrentAnimation = AnimationId.WalkNorth;
                if (CurrentDirection.HasFlag(Direction.East))
                {
                    diffMapX = walkDistance * Cos45;
                    diffMapY = -(walkDistance * Cos45);
                }
                else if (CurrentDirection.HasFlag(Direction.West))
                {
                    diffMapX = -(walkDistance * Cos45);
                    diffMapY = -(walkDistance * Cos45);
                }
                else
                {
                    diffMapY = -walkDistance;
                }
            }
            else if (CurrentDirection.HasFlag(Direction.East))
            {
                Element.CurrentAnimation = AnimationId.WalkEast;
                diffMapX = walkDistance;
            }
            else if (CurrentDirection.HasFlag(Direction.West))
            {
                Element.CurrentAnimation = AnimationId.WalkWest;
                diffMapX = -walkDistance;
            }

            AttemptToMoveInDirection(diffMapX, diffMapY);
        }

        private void AttemptToMoveInDirection(float diffMapX, float diffMapY)
        {
            var diffCoordinates = new MapCoordinates(diffMapX, diffMapY);
            this.MapCoordinates = this.Resolver.AttemptToMove(this.MapCoordinates, diffCoordinates);
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
