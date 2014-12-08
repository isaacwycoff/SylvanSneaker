using Microsoft.Xna.Framework.Graphics;
using SylvanSneaker.Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SylvanSneaker.Core
{
    public class ElementManager
    {
        private IList<Element> Elements;
        private TextureManager TextureManager;
        private SpriteBatch Batch;

        public ElementManager(SpriteBatch spriteBatch, TextureManager textureManager)
        {
            this.Batch = spriteBatch;
            this.TextureManager = textureManager;
            Elements = new List<Element>();
        }

        public AnimatedElement Add(float mapX, float mapY, int textureId)
        {
            var texture = TextureManager[textureId];
            var element = new AnimatedElement(texture, Batch);
            Elements.Add(element);
            return element;
            // throw new NotImplementedException();
        }

        public void Draw(TimeSpan timeDelta)
        {
            foreach (var element in Elements)
            {
                element.Draw(timeDelta);
            }
        }

        public void Update(TimeSpan timeDelta)
        {
            foreach(var element in Elements)
            {
                element.Update(timeDelta);
            }
        }
    }
}
