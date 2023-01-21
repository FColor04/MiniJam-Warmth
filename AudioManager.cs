using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using MiniJam_Warmth;
using System.Collections.Generic;

namespace AudioManagementUtil
{
    public class AudioManager
    {
        private Dictionary<string, SoundEffect> _sounds;
        private AudioEngine _audioEngine;
        private SoundBank _soundBank;
        private Cue _backgroundMusicCue;

        public AudioManager()
        {
            _sounds = new Dictionary<string, SoundEffect>();
            _audioEngine = new AudioEngine("Content/Audio/MyGameAudio.xgs");
            _soundBank = new SoundBank(_audioEngine, "Content/Audio/MyGameSoundBank.xsb");
        }

        public void AddSound(string name, SoundEffect sound)
        {
            if (!_sounds.ContainsKey(name))
                _sounds.Add(name, sound);
        }

        public void RemoveSound(string name)
        {
            if (_sounds.ContainsKey(name))
                _sounds.Remove(name);
        }

        public void PlaySoundEffect(string name)
        {
            if (_sounds.ContainsKey(name))
                _sounds[name].Play();
        }

        public void PlayBackgroundMusic(string cueName)
        {
            if (_backgroundMusicCue != null)
                _backgroundMusicCue.Stop(AudioStopOptions.AsAuthored);

            _backgroundMusicCue = _soundBank.GetCue(cueName);
            _backgroundMusicCue.Play();
        }

        public void StopBackgroundMusic()
        {
            if (_backgroundMusicCue != null)
                _backgroundMusicCue.Stop(AudioStopOptions.AsAuthored);
        }

        public void OnUpdate()
        {
            if (Input.IsKeyPressed(Keys.Space))
                PlaySoundEffect("shoot");
        }
    }
}
