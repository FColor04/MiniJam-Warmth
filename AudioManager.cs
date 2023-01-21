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
        private bool songPlaying = false;

        public AudioManager()
        {
            _songs = new Dictionary<string, Song>();
            MainGame.OnUpdate += OnUpdate;
        }

        ~AudioManager()
        {
            MainGame.OnUpdate -= OnUpdate;
        }

        public void AddSong(string name, string path)
        {
            if (!_songs.ContainsKey(name))
                _songs.Add(name, MainGame.content.Load<Song>(path));
        }

        public void RemoveSong(string name)
        {
            if (_songs.ContainsKey(name))
                _songs.Remove(name);
        }

        public void PlaySong(string name)
        {
            if (_songs.ContainsKey(name))
            {
                MediaPlayer.Play(_songs[name]);
                MediaPlayer.Volume = 0.1f;
                songPlaying = true;
            }
        }

        public void StopSong()
        {
            songPlaying = false;
            MediaPlayer.Stop();
        }

        private void OnUpdate(float deltaTime)
        {
            
        }
    }
}
