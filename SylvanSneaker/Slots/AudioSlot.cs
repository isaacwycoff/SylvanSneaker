using Microsoft.Xna.Framework.Media;
using SylvanSneaker.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SylvanSneaker.Slots
{
    public static class AudioSlot
    {
        private static AudioManager Manager;

        public static void Initialize(AudioManager manager)
        {            
            Manager = manager;
        }

        public static void Shutdown()
        {

        }

        public static void PlaySong(string name)
        {
            Manager.PlaySong(name);
        }

        public static void StopSong()
        {
            Manager.StopSong();
        }
    }
}
