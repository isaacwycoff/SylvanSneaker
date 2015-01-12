using SylvanSneaker.Sandbox;
using System;
using System.Collections.Generic;

namespace SylvanSneaker.Core
{
    public class ElementManager
    {
        public List<AnimatedElement> AnimatedElements { get; private set; }
        private TextureManager TextureManager;

        public ElementManager(TextureManager textureManager)
        {
            this.TextureManager = textureManager;
            AnimatedElements = new List<AnimatedElement>();
        }

        public IList<AnimatedElement> GetElementsInArea(float left, float top, int width, int height)
        {
            AnimatedElements.Sort();            // might be sorting multiple times. Need to do this in collision detection as well.

            var elementsInArea = new List<AnimatedElement>();

            foreach (var element in AnimatedElements)
            {
                if(element.MapCoordinates.Y > top + height)
                {
                    break;          // we've passed all of the elemnts within the area
                }

                if (element.MapCoordinates.Y > top && element.MapCoordinates.X > left && element.MapCoordinates.X < left + width)
                {
                    elementsInArea.Add(element);
                }
            }
            return elementsInArea;
        }

        public AnimatedElement Add(MapCoordinates mapCoordinates, int textureId)
        {
            var texture = TextureManager[textureId];
            var element = new AnimatedElement(texture, mapCoordinates);      // mapCoordinates.X, mapCoordinates.Y);
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
