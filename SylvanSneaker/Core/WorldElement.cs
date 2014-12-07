using SylvanSneaker.Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SylvanSneaker
{
    public interface WorldElement: Element
    {
        float MapX { get; }
        float MapY { get; }

        Camera Camera { set; }
    }
}
