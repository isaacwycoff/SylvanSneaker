using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SylvanSneaker.Environment;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

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

    public class AnimatedElement : WorldElement
    {
        private int TileSize = 32;

        private Texture2D Texture { get; set; }

        public Camera Camera
        {
            set { throw new NotImplementedException(); }
        }

        public float MapX { get; set; }

        public float MapY { get; set; }

        private Dictionary<AnimationId, Animation> AnimationLookup { get; set; }

        public AnimationId CurrentAnimation { get; set; }

        public int AnimationTime { get; private set; }              // time in milliseconds since start of animation

        private int ScreenX
        {
            get
            {
                return (int)(MapX * TileSize);
            }
        }

        private int ScreenY
        {
            get
            {
                return (int)(MapY * TileSize);
            }
        }

        public Color Tint {
            get
            {
                var lighting = this.Lighting.GetLightLevel((int)MapX, (int)MapY);
                return new Color(lighting.Red, lighting.Green, lighting.Blue);
            }
        }

        private WorldLighting Lighting { get; set; }

        private SpriteBatch SpriteBatch { get; set; }

        public AnimatedElement(Texture2D texture, SpriteBatch spriteBatch, WorldLighting lighting)
        {
            if (lighting == null)
            {
                throw new Exception("Missing WorldLighting!");
            }
            if (texture == null)
            {
                throw new Exception("Missing a texture!");
            }
            if (spriteBatch == null)
            {
                throw new Exception("SpriteBatch wasn't initialized!");
            }

            this.Texture = texture;
            this.SpriteBatch = spriteBatch;
            this.Lighting = lighting;

            // walk south animation:
            var walkSouth = new Animation(
                new AnimationFrame[] {
                    new AnimationFrame(30, 669, 64, 53, 250),
                    new AnimationFrame(95, 669, 63, 53, 250),
                    new AnimationFrame(161, 670, 63, 53, 250),
                    new AnimationFrame(95, 669, 63, 53, 250),
                    }
                );

            var walkEast = new Animation(
                new AnimationFrame[] {
                    new AnimationFrame(24, 148, 39, 53, 250),
                    new AnimationFrame(62, 148, 39, 53, 250),
                    new AnimationFrame(105, 148, 39, 53, 250),
                    new AnimationFrame(152, 148, 39, 53, 250),
                    new AnimationFrame(62, 148, 39, 53, 250),
                    }
                );

            var walkNorth = new Animation(
                new AnimationFrame[] {
                    new AnimationFrame(29, 400, 55, 63, 250),
                    new AnimationFrame(82, 400, 55, 63, 250),
                    new AnimationFrame(140, 400, 55, 63, 250),
                    new AnimationFrame(196, 400, 55, 63, 250),
                    new AnimationFrame(82, 400, 55, 63, 250),
                    }
                );

            var walkWest = new Animation(
                new AnimationFrame[] {
                    new AnimationFrame(24, 148, 39, 53, 250, true),
                    new AnimationFrame(62, 148, 39, 53, 250, true),
                    new AnimationFrame(105, 148, 39, 53, 250, true),
                    new AnimationFrame(152, 148, 39, 53, 250, true),
                    new AnimationFrame(62, 148, 39, 53, 250, true),
                    }
                );


            this.AnimationLookup = new Dictionary<AnimationId, Animation>();
            this.AnimationLookup[AnimationId.Testing] = walkSouth;
            this.AnimationLookup[AnimationId.WalkSouth] = walkSouth;
            this.AnimationLookup[AnimationId.WalkEast] = walkEast;
            this.AnimationLookup[AnimationId.WalkNorth] = walkNorth;
            this.AnimationLookup[AnimationId.WalkWest] = walkWest;

            this.CurrentAnimation = AnimationId.Testing;
            this.AnimationTime = 0;

        }

        public void Draw(TimeSpan timeDelta)
        {
            AnimationTime += timeDelta.Milliseconds;

            var animation = AnimationLookup[CurrentAnimation];

            // loop around -- in REALITY we would probably send a signal to some other object to get the next animation
            if (AnimationTime > animation.Duration)
            {
                AnimationTime = AnimationTime - animation.Duration;
            }

            var index = animation.GetFrameIndex(this.AnimationTime);
            var frame = animation[index];

            Rectangle sourceRect = new Rectangle(frame.Left, frame.Top, frame.Width, frame.Height);
            Rectangle destRect = new Rectangle(ScreenX, ScreenY, sourceRect.Width, sourceRect.Height);

            // Color tint = new Color(255, 255, 255, 255);

            var effects = frame.Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            this.SpriteBatch.Draw(this.Texture, drawRectangle: destRect, sourceRectangle: sourceRect, color: Tint, effect: effects);
        }

        private class AnimationFrame
        {
            public int Left { get; private set; }
            public int Top { get; private set; }
            public int Width { get; private set; }
            public int Height { get; private set; }
            public int Duration { get; private set; }
            public bool Flipped { get; private set; }

            public AnimationFrame(int left, int top, int width, int height, int duration, bool flipped = false)
            {
                this.Left = left;
                this.Top = top;
                this.Width = width;
                this.Height = height;
                this.Duration = duration;
                this.Flipped = flipped;
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
//            throw new NotImplementedException();
        }
    }
}
