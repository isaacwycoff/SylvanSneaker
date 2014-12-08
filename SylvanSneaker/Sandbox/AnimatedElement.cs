using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        private AnimationId CurrentAnimation;
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

        private SpriteBatch SpriteBatch { get; set; }

        public AnimatedElement(Texture2D texture, SpriteBatch spriteBatch)
        {
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

            // walk south animation:
            var walkSouth = new Animation(
                new AnimationFrame[] {
                    new AnimationFrame(30, 669, 64, 53, 250),
                    new AnimationFrame(95, 669, 63, 53, 250),
                    new AnimationFrame(161, 670, 63, 53, 250),
                    new AnimationFrame(95, 669, 63, 53, 250),
                    }
                );

            this.AnimationLookup = new Dictionary<AnimationId, Animation>();
            this.AnimationLookup[AnimationId.Testing] = walkSouth;

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

            Color tint = new Color(255, 255, 255, 255);

            this.SpriteBatch.Draw(this.Texture, destRect, sourceRect, tint);  
        }

        private class AnimationFrame
        {
            public int Left { get; private set; }
            public int Top { get; private set; }
            public int Width { get; private set; }
            public int Height { get; private set; }
            public int Duration { get; private set; }

            public AnimationFrame(int left, int top, int width, int height, int duration)
            {
                this.Left = left;
                this.Top = top;
                this.Width = width;
                this.Height = height;
                this.Duration = duration;
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
