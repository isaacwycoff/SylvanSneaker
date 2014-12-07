using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SylvanSneaker.Core
{
    class ElementManager
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

        public Element NewElement(float mapX, float mapY, string textureName)
        {
            throw new NotImplementedException();
        }

        public void Draw(TimeSpan timeDelta)
        {
            foreach (var element in Elements)
            {
                element.Update(timeDelta);
            }
            throw new NotImplementedException();
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
