using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MiniJam_Warmth;
using System;

namespace AudioManagementUtil
{
    public class AudioManager
    {
        private Dictionary<string, Song> _songs;
        private ContentManager _contentManager;
        private delegate void print(string name);
        private print p = Console.WriteLine;
        private bool songPlaying = false;

        public AudioManager(Game game)
        {
            _songs = new Dictionary<string, Song>();
            _contentManager = new ContentManager(game.Content.ServiceProvider, "Content");
        }

        public void AddSong(string name, string path)
        {
            if (!_songs.ContainsKey(name))
                _songs.Add(name, _contentManager.Load<Song>(path));
        }

        public void RemoveSong(string name)
        {
            if (_songs.ContainsKey(name))
                _songs.Remove(name);
        }

        public void PlaySong(string name)
        {
            if (_songs.ContainsKey(name))
                MediaPlayer.Play(_songs[name]);
        }

        public void StopSong()
        {
            MediaPlayer.Stop();
        }

        private void OnUpdate()
        {
            if (Input.WasPressedThisFrame(Keys.Space) && !songPlaying)
            {
                this.PlaySong("Warmer_");
                songPlaying = true;
            }
            else if (Input.WasReleasedThisFrame(Keys.Space))
            {
                songPlaying = false;
            }
            if (songPlaying == true && Input.WasPressedThisFrame(Keys.Space))
            {
                StopSong();
            }
        }
    }
}
