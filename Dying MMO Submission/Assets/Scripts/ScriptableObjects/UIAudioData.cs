using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "UIAudioData", menuName = "Data/UIAudioData")]
public class UIAudioData : ScriptableObject
{
    [Header("UI menu")]
    [SerializeField] private AudioClip _alert;
    [SerializeField] private AudioClip _chatPing;
    [SerializeField] private List<AudioClip> _buttonAudioSet;
    [SerializeField] private AudioMixerGroup _audioMixer;

    public AudioClip Alert { get { return _alert; } }
    public AudioClip ChatPing { get { return _chatPing; } }
    public List<AudioClip> ButtonAudioSet { get { return _buttonAudioSet; } }
    public AudioMixerGroup AudioMixer { get { return _audioMixer; } }
}
