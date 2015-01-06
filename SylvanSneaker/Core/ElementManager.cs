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
        public IList<AnimatedElement> AnimatedElements { get; private set; }
        private TextureManager TextureManager;

        public ElementManager(TextureManager textureManager)
        {
            this.TextureManager = textureManager;
            AnimatedElements = new List<AnimatedElement>();
        }

        public AnimatedElement Add(float mapX, float mapY, int textureId)
        {
            var texture = TextureManager[textureId];
            var element = new AnimatedElement(texture, mapX, mapY);
            AnimatedElements.Add(element);
            return element;
        }

        public void Update(TimeSpan timeDelta)
        {
            foreach(var element in AnimatedElements)
            {
                element.Update(timeDelta);
            }
        }
    }
}
