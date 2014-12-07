using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SylvanSneaker.Core
{
    class PlayerCamera: Camera
    {
        public WorldElement AttachedTo { get; set; }

        public float Scale
        {
            get
            {
                return 1.0f;            // should be calculated?
            }
        }

        public float MapX
        {
            get {
                // calculate based on AttachedTo            
                return 0.0f;
            }
        }

        public float MapY
        {
            get { 
                // calculate based on AttachedTo
                return 0.0f;
            }
        }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public PlayerCamera(WorldElement attachedTo, int width, int height)
        {
            this.AttachedTo = attachedTo;
            this.Width = width;
            this.Height = height;
        }

        private int _TileSize;
        public int TileSize
        {
            get { return _TileSize; }
        }

        private int _ScreenRows;
        public int ScreenRows
        {
            get { return _ScreenRows; }
        }

        private int _ScreenColumns;
        public int ScreenColumns
        {
            get { return _ScreenColumns; }
        }


        private void UpdateZoom()
        {
            const int TILE_SIZE = 64;

            this._TileSize = Convert.ToInt32(TILE_SIZE * this.Scale);
            this._ScreenRows = (this.Height / this.TileSize) + 1;
            this._ScreenColumns = (this.Width / this.TileSize) + 1;
        }



    }
}
