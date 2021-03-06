﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SylvanSneaker.Core;
using System;
using System.Collections.Generic;

namespace SylvanSneaker.Sandbox
{
    public enum AnimationId
    {
        Testing,
        WalkSouth,
        WalkEast,
        WalkNorth,
        WalkWest,
        IdleSouth,
        IdleEast,
        IdleWest,
        IdleNorth,
    }

    public class AnimatedElement : WorldElement, IComparable<AnimatedElement>
    {
        public Texture2D Texture { get; private set; }

        public MapCoordinates MapCoordinates { get; set; }

        private Dictionary<AnimationId, Animation> AnimationLookup { get; set; }

        public AnimationId CurrentAnimation { get; set; }

        public int AnimationTime { get; private set; }              // time in milliseconds since start of animation

        public AnimatedElement(Texture2D texture, MapCoordinates mapCoordinates)        // float mapX, float mapY)
        {
            this.Texture = texture;
            this.MapCoordinates = mapCoordinates;

            var animSpeed = 75;

            // walk south animation:
            var walkSouth = new Animation(
                new AnimationFrame[] {
                    new AnimationFrame(30, 669, 64, 53, 32, 43, animSpeed),
                    new AnimationFrame(95, 669, 63, 53, 32, 43, animSpeed),
                    new AnimationFrame(161, 669, 63, 53, 32, 43, animSpeed),
                    new AnimationFrame(95, 669, 63, 53, 32, 43, animSpeed),
                    }
                );

            var idleSouth = new Animation(
                new AnimationFrame[] {
                    new AnimationFrame(30, 669, 64, 53, 32, 43, 1000),
                    });

            var walkEastFrames = new AnimationFrame[]
            {
                new AnimationFrame(24, 148, 39, 53, 20,  43, animSpeed),
                new AnimationFrame(62, 148, 39, 53, 20,  43, animSpeed),
                new AnimationFrame(105, 148, 39, 53, 20, 43, animSpeed),
                new AnimationFrame(152, 148, 39, 53, 20, 43, animSpeed),
            };

            var walkEast = new Animation(
                new AnimationFrame[] {
                        walkEastFrames[0],
                        walkEastFrames[2],
                        walkEastFrames[3],
                        walkEastFrames[1],
                    }
                );

            var idleEast = new Animation(
                new AnimationFrame[] {
                    new AnimationFrame(24, 148, 39, 53, 20, 43, animSpeed),
                    });

            var walkNorth = new Animation(
                new AnimationFrame[] {
                    new AnimationFrame(29, 400, 55, 63, 27,  43, animSpeed),
                    new AnimationFrame(82, 400, 55, 63, 27,  43, animSpeed),
                    new AnimationFrame(140, 400, 55, 63, 27, 43, animSpeed),
                    new AnimationFrame(196, 400, 55, 63, 27, 43, animSpeed),
                    new AnimationFrame(82, 400, 55, 63, 27,  43, animSpeed),
                    }
                );

            var idleNorth = new Animation(
                new AnimationFrame[] {
                    new AnimationFrame(29, 400, 55, 63, 27, 43, animSpeed),
                    });

            var walkWest = new Animation(
                new AnimationFrame[] {
                    new AnimationFrame(24, 148, 39, 53, 20,  43, animSpeed, true),
                    new AnimationFrame(62, 148, 39, 53, 20,  43, animSpeed, true),
                    new AnimationFrame(105, 148, 39, 53, 20, 43, animSpeed, true),
                    }
                );

            var idleWest = new Animation(
                new AnimationFrame[] {
                    new AnimationFrame(24, 148, 39, 53, 20, 43, 250, true),
                    });

            this.AnimationLookup = new Dictionary<AnimationId, Animation>();
            this.AnimationLookup[AnimationId.Testing] = walkSouth;
            this.AnimationLookup[AnimationId.WalkSouth] = walkSouth;
            this.AnimationLookup[AnimationId.WalkEast] = walkEast;
            this.AnimationLookup[AnimationId.WalkNorth] = walkNorth;
            this.AnimationLookup[AnimationId.WalkWest] = walkWest;

            this.AnimationLookup[AnimationId.IdleSouth] = idleSouth;
            this.AnimationLookup[AnimationId.IdleEast] = idleEast;
            this.AnimationLookup[AnimationId.IdleNorth] = idleNorth;
            this.AnimationLookup[AnimationId.IdleWest] = idleWest;

            this.CurrentAnimation = AnimationId.Testing;
            this.AnimationTime = 0;
        }

        public void IncrementAnimationTime(int milliseconds)
        {
            AnimationTime += milliseconds;

            var animation = AnimationLookup[CurrentAnimation];

            if (AnimationTime > animation.Duration)
            {
                AnimationTime = AnimationTime - animation.Duration;
            }
        }

        public AnimationFrame CurrentFrame
        {
            get
            {
                var animation = AnimationLookup[CurrentAnimation];
                var index = animation.GetFrameIndex(AnimationTime);
                return animation[index];
            }
        }

        private class Animation
        {
            private AnimationFrame[] Frames { get; set; }

            public int Duration { get; private set; }

            public Animation(AnimationFrame[] frames)
            {
                this.Frames = frames;
                this.CalculateDuration();
            }

            public AnimationFrame this[int i]
            {
                get
                {
                    return Frames[i];
                }
            }

            private void CalculateDuration()
            {
                this.Duration = 0;

                foreach (var frame in this.Frames)
                {
                    this.Duration = this.Duration + frame.Duration;
                }
            }

            public int GetFrameIndex(int milliseconds)
            {
                var index = 0;
                var millisecondsTally = 0;

                do {
                    millisecondsTally = millisecondsTally + this.Frames[index].Duration;

                    index += 1;

                    if (index == this.Frames.Length)
                    {
                        index = 0;
                    }
                }
                while (millisecondsTally < milliseconds);

                if (index == 0) {
                    return this.Frames.Length - 1;
                }
                return index - 1;
            }
        }

        public void Update(TimeSpan timeDelta)
        {
            this.IncrementAnimationTime(timeDelta.Milliseconds);
        }

        public int CompareTo(AnimatedElement other)
        {
            // If other is not a valid object reference, this instance is greater. 
            if (other == null) return 1;

            return this.MapCoordinates.Y.CompareTo(other.MapCoordinates.Y);
        }
    }

    public class AnimationFrame
    {
        public Rectangle Rectangle { get; private set; }
        public int Duration { get; private set; }
        public bool Flipped { get; private set; }

        public PixelCoordinates Anchor { get; private set; }

        public AnimationFrame(int left, int top, int width, int height, int anchorX, int anchorY, int duration, bool flipped = false)
        {
            this.Rectangle = new Rectangle(left, top, width, height);
            this.Anchor = new PixelCoordinates(anchorX, anchorY);
            this.Duration = duration;
            this.Flipped = flipped;
        }
    }

    public class PixelCoordinates
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public PixelCoordinates(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }

}
