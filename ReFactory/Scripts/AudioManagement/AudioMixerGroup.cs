namespace AudioManagement;

/// <summary>
/// Enumeration of audio mixer groups, one mixer is instantiated per one
/// mixer group and assigned in static constructor.
/// </summary>
/// <seealso cref="AudioManager"/>
/// <seealso cref="AudioMixerGroup"/>
public enum AudioMixerGroup
{
    Music,
    SoundEffects,
    Ambient,
    Custom
}