using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SylvanSneaker.Audio
{
    public interface IAudioManager
    {
        void PlaySong(string name);
        void StopSong();
    }

    public class AudioManager : IAudioManager
    {
        private ContentManager Content;

        public AudioManager (ContentManager content)
	    {
            this.Content = content;

            // TODO: put things that are game init stuff in their own function
            // that happens after LoadContent
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.5f;          // FIXME: get this from Settings
	    }

        public void PlaySong(string name)
        {
            Song song = Content.Load<Song>(String.Format("Songs/{0}", name));

            MediaPlayer.Play(song);
        }

        public void StopSong()
        {
            MediaPlayer.Stop();
        }
    }
}
