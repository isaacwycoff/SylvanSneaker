using System;


namespace SylvanSneaker
{
    public interface Element
    {
        void Draw(TimeSpan timeDelta);
        void Update(TimeSpan timeDelta);
    }
}
