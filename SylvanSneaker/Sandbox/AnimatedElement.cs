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
            private get;
            set;
        }

        public float MapX { get; set; }

        public float MapY { get; set; }

        private Dictionary<AnimationId, Animation> AnimationLookup { get; set; }

        public AnimationId CurrentAnimation { get; set; }

        public int AnimationTime { get; private set; }              // time in milliseconds since start of animation

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
                    new AnimationFrame(30, 669, 64, 53, 32, 53, 250),
                    new AnimationFrame(95, 669, 63, 53, 32, 53, 250),
                    new AnimationFrame(161, 669, 63, 53, 32, 53, 250),
                    new AnimationFrame(95, 669, 63, 53, 32, 53, 250),
                    }
                );

            var idleSouth = new Animation(
                new AnimationFrame[] {
                    new AnimationFrame(30, 669, 64, 53, 32, 53, 1000),
                    });

            var walkEast = new Animation(
                new AnimationFrame[] {
                    new AnimationFrame(24, 148, 39, 53, 20, 53, 250),
                    new AnimationFrame(62, 148, 39, 53, 20, 53, 250),
                    new AnimationFrame(105, 148, 39, 53, 20, 53, 250),
                    new AnimationFrame(152, 148, 39, 53, 20, 53, 250),
                    new AnimationFrame(62, 148, 39, 53, 20, 53, 250),
                    }
                );

            var idleEast = new Animation(
                new AnimationFrame[] {
                    new AnimationFrame(24, 148, 39, 53, 20, 53, 250),
                    });

            var walkNorth = new Animation(
                new AnimationFrame[] {
                    new AnimationFrame(29, 400, 55, 63, 27, 63, 250),
                    new AnimationFrame(82, 400, 55, 63, 27, 63, 250),
                    new AnimationFrame(140, 400, 55, 63, 27, 63, 250),
                    new AnimationFrame(196, 400, 55, 63, 27, 63, 250),
                    new AnimationFrame(82, 400, 55, 63, 27, 63, 250),
                    }
                );

            var idleNorth = new Animation(
                new AnimationFrame[] {
                    new AnimationFrame(29, 400, 55, 63, 27, 63, 250),
                    });

            var walkWest = new Animation(
                new AnimationFrame[] {
                    new AnimationFrame(24, 148, 39, 53, 20, 53, 250, true),
                    new AnimationFrame(62, 148, 39, 53, 20, 53, 250, true),
                    new AnimationFrame(105, 148, 39, 53, 20, 53, 250, true),
                    new AnimationFrame(152, 148, 39, 53, 20, 53, 250, true),
                    new AnimationFrame(62, 148, 39, 53, 20, 53, 250, true),
                    }
                );

            var idleWest = new Animation(
                new AnimationFrame[] {
                    new AnimationFrame(24, 148, 39, 53, 20, 53, 250, true),
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

        public void Draw(TimeSpan timeDelta, Camera camera)
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

            // var destX = ScreenX - frame.AnchorX;    // -(this.Camera.MapX * this.Camera.Scale);
            // var destY = ScreenY - frame.AnchorY;    // -(this.Camera.MapY * this.Camera.Scale);

            // Rectangle sourceRect = new Rectangle(frame.Left, frame.Top, frame.Width, frame.Height);
            // Rectangle destRect = new Rectangle(ScreenX - frame.AnchorX, ScreenY - frame.AnchorY, sourceRect.Width, sourceRect.Height);

            // Color tint = new Color(255, 255, 255, 255);

            // var effects = frame.Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            camera.DrawFrame(this.Texture, frame, MapX, MapY, Tint);

            // this.SpriteBatch.Draw(this.Texture, drawRectangle: destRect, sourceRectangle: sourceRect, color: Tint);
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

    public class AnimationFrame
    {
        public int Left { get; private set; }
        public int Top { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Duration { get; private set; }
        public bool Flipped { get; private set; }

        public int AnchorX { get; private set; }
        public int AnchorY { get; private set; }

        public AnimationFrame(int left, int top, int width, int height, int anchorX, int anchorY, int duration, bool flipped = false)
        {
            this.Left = left;
            this.Top = top;
            this.Width = width;
            this.Height = height;
            this.AnchorX = anchorX;
            this.AnchorY = anchorY;
            this.Duration = duration;
            this.Flipped = flipped;
        }
    }

}
