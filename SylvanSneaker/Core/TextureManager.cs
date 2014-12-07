using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SylvanSneaker.Core
{
    public class TextureManager
    {
        private IDictionary<int, Texture2D> Textures;
        private ContentManager Content;

        public TextureManager(ContentManager content)
        {
            this.Content = content;
            this.Textures = new Dictionary<int, Texture2D>();
        }

        public Texture2D this[int id]
        {
            get { return Textures[id]; }
        }

        private void LoadTexture(int id)
        {
            if (Textures.Keys.Contains(id))
            {
                return;
            }
        }

        // eventually, this should load from a file. for now, we're just going to hard-code it
        private class TextureLookup
        {
            private IDictionary<int, string> Textures;

            public TextureLookup()
            {
                this.Textures = new Dictionary<int, string>();
                this.Initialize();
            }

            private void Initialize()
            {
                Textures[TextureName.KNIGHT] = "Textures/knight_sword_REPLACE";
                Textures[TextureName.GROUND] = "Textures/tile_jungle_REPLACE";
            }
        }
    }


    // in lieu of loading this data from disk, we're going to use an enum:
    public static class TextureName
    {
        public const int KNIGHT = 1;
        public const int GROUND = 2;
    }

}
