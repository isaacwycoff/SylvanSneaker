using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SylvanSneaker.Audio
{
    class AudioManager
    {

        public AudioManager ()
	    {
            // TODO: put things that are game init stuff in their own function
            // that happens after LoadContent
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.5f;          // FIXME: get this from Settings
	    }

        public void PlaySong(Song song)
        {
            MediaPlayer.Play(song);
        }

        public void StopSong()
        {
            MediaPlayer.Stop();
        }
    }
}
