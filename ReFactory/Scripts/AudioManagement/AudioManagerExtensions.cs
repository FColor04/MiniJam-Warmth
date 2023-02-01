namespace AudioManagement;

public static class AudioManagerExtensions
{
    /// <summary>
    /// Get mixer by group
    /// </summary>
    /// <param name="group">Audio Mixer Group to get</param>
    /// <returns>AudioMixer</returns>
    public static AudioMixer GetMixer(this AudioMixerGroup group) => AudioManager.AudioMixers[group];
}