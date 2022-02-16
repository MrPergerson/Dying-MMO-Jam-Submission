using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public interface IAudioPlayer
{
    public void PlayAudioClip(AudioClip clip, AudioMixerGroup mixer);
}
