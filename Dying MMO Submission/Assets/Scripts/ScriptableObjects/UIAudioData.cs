using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UIAudioData", menuName = "Data/UIAudioData")]
public class UIAudioData : ScriptableObject
{
    [Header("UI menu")]
    [SerializeField] private AudioClip _alert;
    [SerializeField] private AudioClip _chatPing;
    [SerializeField] private List<AudioClip> _buttonAudioSet;

    public AudioClip Alert { get { return _alert; } }
    public AudioClip ChatPing { get { return _chatPing; } }
    public List<AudioClip> ButtonAudioSet { get { return _buttonAudioSet; } }


}
