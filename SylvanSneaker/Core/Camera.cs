using Microsoft.Xna.Framework;

namespace SylvanSneaker
{
    public interface Camera
    {
        float Scale { get; }
        float MapX { get; }             // MapX and MapY in Tiles
        float MapY { get; }
        int Width { get; }            // width and height in pixels
        int Height { get; }

        int TileSize { get; }
        int ScreenRows { get; }
        int ScreenColumns { get; }

        void Draw(GameTime gameTime);
    }
}
