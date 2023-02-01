using System;
using MainGameFramework;

namespace AudioManagement
{
    /// <summary>
    /// Static utility class to ease managing of multiple audio mixers
    /// </summary>
    /// <seealso cref="AudioManager.AudioMixers"/>
    /// <seealso cref="AudioMixerGroup"/>
    public static class AudioManager
    {
        /// <summary>
        /// Collection of audio mixers.
        /// </summary>
        /// <example>
        /// Play a soundEffect on music mixer.
        /// <code>
        ///     AudioManager.AudioMixers[AudioMixerGroup.Music].FadeTo(mySfx, duration: 4f);
        /// </code>
        /// </example>
        /// <seealso cref="AudioManager"/>
        /// <seealso cref="AudioMixerGroup"/>
        public static readonly AudioMixers AudioMixers = new();

        static AudioManager()
        {
            foreach (var group in Enum.GetValues<AudioMixerGroup>())
            {
                AudioMixers.Add(new AudioMixer(group));
            }

            MainGame.OnUpdate += FadeUpdate;
        }

        private static void FadeUpdate(float deltaTime)
        {
            foreach (var mixer in AudioMixers)
            {
                mixer.Update(deltaTime);
            }
        }


    }
}
