using Microsoft.Xna.Framework.Graphics;
using SylvanSneaker.Environment;
using SylvanSneaker.Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SylvanSneaker.Core
{
    public class ElementManager
    {
        private IList<WorldElement> WorldElements;
        private TextureManager TextureManager;
        private Ground Ground;
        private SpriteBatch Batch;

        public ElementManager(SpriteBatch spriteBatch, TextureManager textureManager, Ground ground)
        {
            this.Batch = spriteBatch;
            this.TextureManager = textureManager;
            this.Ground = ground;
            WorldElements = new List<WorldElement>();
        }

        public AnimatedElement Add(float mapX, float mapY, int textureId)
        {
            var texture = TextureManager[textureId];
            var element = new AnimatedElement(texture, Batch, Ground);
            WorldElements.Add(element);
            return element;
            // throw new NotImplementedException();
        }

        public void Draw(TimeSpan timeDelta, Camera camera)
        {
            foreach (var element in WorldElements)
            {
                element.Draw(timeDelta, camera);
            }
        }

        public void Update(TimeSpan timeDelta)
        {
            foreach(var element in WorldElements)
            {
                element.Update(timeDelta);
            }
        }
    }
}
