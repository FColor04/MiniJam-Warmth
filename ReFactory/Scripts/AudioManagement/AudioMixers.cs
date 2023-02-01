using System;
using System.Collections.ObjectModel;

namespace AudioManagement;

/// <summary>
/// Collection of AudioMixers with AudioMixerGroup as a key.
/// </summary>
/// <example>
/// Play a soundEffect on music mixer.
/// <code>
///     AudioManager.AudioMixers[AudioMixerGroup.Music].FadeTo(mySfx, duration: 4f);
/// </code>
/// </example>
/// <seealso cref="AudioManager"/>
/// <seealso cref="AudioManager.AudioMixers"/>
/// <seealso cref="AudioMixerGroup"/>
public class AudioMixers : KeyedCollection<AudioMixerGroup, AudioMixer>
{
    protected override AudioMixerGroup GetKeyForItem(AudioMixer item) => item.Group;
}