using Microsoft.Xna.Framework.Graphics;
using SylvanSneaker.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SylvanSneaker.Slots
{
    public static class TextureSlot
    {
        private static ITextureManager Manager;

        public static void Initialize(ITextureManager manager)
        {
            Manager = manager;
        }

        public static void Shutdown()
        {

        }

        public static Texture2D GetTexture(int id)
        {
            return Manager[id];
        }
    }
}
