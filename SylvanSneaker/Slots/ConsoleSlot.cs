using SylvanSneaker.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SylvanSneaker.Slots
{
    public static class ConsoleSlot
    {
        private static IDevConsole Console;

        public static void Initialize(IDevConsole console)
        {
            Console = console;
        }

        public static void ShutDown()
        {

        }

        public static void SetDebugLine(string line)
        {
            Console.SetDebugLine(line);
        }

        public static void SetExpanded(bool isExpanded)
        {
            Console.Expanded = isExpanded;
        }

        public static void Draw(TimeSpan timeDelta)
        {
            Console.Draw(timeDelta);
        }
    }
}
