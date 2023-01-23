using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MiniJam_Warmth;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.Xna.Framework.Audio;

namespace AudioManagementUtil
{
    public class AudioManager
    {
        public enum Sfx
        {
            Error,
            Place
        }

        public enum SongTrack
        {
            Warmer,
            Desert,
        }

        public float sfxVolume = 0.6f;
        public float musicVolume = 0.4f;
        
        private Dictionary<Sfx, SoundEffect> _sfx;
        private Dictionary<SongTrack, SoundEffect> _songs;

        private float nextSongChange;
        private ReadOnlyCollection<SongTrack?> _musicControllerSongs;

        private SongTrack? _songPlaying;
        public SongTrack? SongPlaying
        {
            get => _songPlaying;
            set
            {
                if (_songPlaying == value) return;
                _songPlaying = value;
                FadeTo(value, 16);
            }
        }

        /// <summary>
        /// This is main song slot
        /// </summary>
        private SoundEffectInstance _songSlot1;
        /// <summary>
        /// This slot always contains fading out song
        /// </summary>
        private SoundEffectInstance _songSlot2;
        private float _songSlot2FadeProgress;
        private float fadeDuration = 16;

        public AudioManager()
        {
            var Content = MainGame.content;
            _sfx = new Dictionary<Sfx, SoundEffect>();
            _songs = new Dictionary<SongTrack, SoundEffect>();
            
            _sfx.Add(Sfx.Error, Content.Load<SoundEffect>("Error"));
            _sfx.Add(Sfx.Place, Content.Load<SoundEffect>("Place"));
            //Songs are kept as SoundEffect too to allow fade transitions
            _songs.Add(SongTrack.Warmer, Content.Load<SoundEffect>("Warmer"));
            _songs.Add(SongTrack.Desert, Content.Load<SoundEffect>("DesertAmbience"));

            _musicControllerSongs = new List<SongTrack?>()
            {
                null,
                SongTrack.Warmer,
                SongTrack.Desert
            }.AsReadOnly();
            
            PlaySong(SongTrack.Warmer);
            nextSongChange = Random.Shared.Next(71, 142);
            
            MainGame.OnUpdate += OnUpdate;
        }

        ~AudioManager()
        {
            MainGame.OnUpdate -= OnUpdate;
        }

        /// <summary>
        /// Instantly switches song
        /// </summary>
        /// <param name="key">New song</param>
        /// <param name="isLooped">Is looped</param>
        public void PlaySong(SongTrack key, bool isLooped = true)
        {
            _songSlot1?.Dispose();
            _songSlot1 = _songs[key].CreateInstance();
            _songSlot1.Volume = musicVolume;
            _songSlot1.IsLooped = isLooped;
            _songSlot1.Play();
        }

        public void FadeTo(SongTrack? key, float fadeDuration, bool isLooped = true)
        {
            _songSlot2?.Dispose();
            _songSlot2 = _songSlot1;
            _songSlot2FadeProgress = 0;
            this.fadeDuration = fadeDuration;
            if (!key.HasValue)
            {
                _songSlot1 = null;
                return;
            }
            _songSlot1 = _songs[key.Value].CreateInstance();
            _songSlot1.Volume = 0;
            _songSlot1.IsLooped = isLooped;
            _songSlot1.Play();
            //Fade is controlled in update method.
        }

        public void PlaySfx(Sfx key, float volume = 1f, float pitch = 1f, float pan = 0)
        {
            //This should throw because we keep keys in enum
            _sfx[key].Play(volume * sfxVolume, pitch, pan);
        }

        private void OnUpdate(float deltaTime)
        {
            ControlMusic();
            UpdateFade(deltaTime);
        }
        
        private void ControlMusic()
        {
            if (nextSongChange > Time.UnscaledTotalTime) return;
            nextSongChange = Time.UnscaledTotalTime;
            var oldSong = SongPlaying;
            while(oldSong == SongPlaying)
                SongPlaying = _musicControllerSongs.Random();
            switch (SongPlaying)
            {
                case SongTrack.Warmer:
                    nextSongChange += Random.Shared.Next(71, 142);
                    break;
                case null:
                    nextSongChange += Random.Shared.Next(20, 50);
                    break;
                default:
                    nextSongChange += Random.Shared.Next(40, 120);
                    break;
            }
        }

        private void UpdateFade(float deltaTime)
        {
            if (_songSlot2FadeProgress >= 1)
            {
                _songSlot2?.Dispose();
                if(_songSlot1 != null)
                    _songSlot1.Volume = musicVolume;
                return;
            }

            _songSlot2FadeProgress += deltaTime / fadeDuration;
            
            if(_songSlot1 != null)
                _songSlot1.Volume = MathHelper.Lerp(_songSlot1.Volume, musicVolume, _songSlot2FadeProgress);
            
            if(_songSlot2 != null)
                _songSlot2.Volume = MathHelper.Lerp(_songSlot2.Volume, 0, _songSlot2FadeProgress);

        }
    }
}
