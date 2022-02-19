using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "UIAudioData", menuName = "Data/UIAudioData")]
public class UIAudioData : ScriptableObject
{
    [Header("UI menu")]
    [SerializeField] private AudioClip _alert;
    [SerializeField] private AudioClip _chatPing;
    [SerializeField] private List<AudioClip> _buttonAudioSet;
    [Title("Audio Mixers", "Tie all sounds to one audio mixer. Use the overrides to target specific sounds.", bold: true)]
    [SerializeField] private AudioMixerGroup _audioMixer;
    [SerializeField] private AudioMixerGroup _alertOverride;
    [SerializeField] private AudioMixerGroup _chatPingOverride;
    [SerializeField] private AudioMixerGroup _buttonAudioSetOverride;

    public AudioClip Alert { get { return _alert; } }
    public AudioClip ChatPing { get { return _chatPing; } }
    public List<AudioClip> ButtonAudioSet { get { return _buttonAudioSet; } }
    public AudioMixerGroup AudioMixer { get { return _audioMixer; } }
    public AudioMixerGroup AlertAudioMixerOverride { get { return _alertOverride; } }
    public AudioMixerGroup ChatPingAudioMixerOverride { get { return _chatPingOverride; } }
    public AudioMixerGroup ButtonAudioSetAudioMixerOverride { get { return _buttonAudioSetOverride; } }
}
