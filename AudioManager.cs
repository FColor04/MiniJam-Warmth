using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MiniJam_Warmth;
using System;
using Microsoft.Xna.Framework.Audio;

namespace AudioManagementUtil
{
    public class AudioManager
    {
        private Dictionary<string, SoundEffect> _sfx;
        private Dictionary<string, Song> _songs;
        private bool songPlaying = false;

        public AudioManager()
        {
            _sfx = new Dictionary<string, SoundEffect>();
            _songs = new Dictionary<string, Song>();
            MainGame.OnUpdate += OnUpdate;
        }

        ~AudioManager()
        {
            MainGame.OnUpdate -= OnUpdate;
        }

        public void AddSfx(string name, string path)
        {
            if(!_sfx.ContainsKey(name))
                _sfx.Add(name, MainGame.content.Load<SoundEffect>(path));
        }

        public void RemoveSfx(string name)
        {
            if (_sfx.ContainsKey(name))
                _sfx.Remove(name);
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

        public void PlaySfx(string name, float volume = 1f, float pitch = 1f, float pan = 0)
        {
            if (_sfx.ContainsKey(name))
            {
                _sfx[name].Play(volume, pitch, pan);
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
