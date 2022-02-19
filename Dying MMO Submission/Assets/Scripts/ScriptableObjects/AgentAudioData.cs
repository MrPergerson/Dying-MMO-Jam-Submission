using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "AgentAudioData", menuName = "Data/AgentAudioData")]
public class AgentAudioData : ScriptableObject
{
    [Title("Audio", "Lists can support any number of sounds. The scripts will randomly choose sounds from the lists.", bold: true)]
    [System.Serializable]
    public struct FootstepSet
    {
        public List<AudioClip> defaultFootsteps;
        public List<AudioClip> grassFootsteps;
        public List<AudioClip> stoneFootsteps;
    }
    [SerializeField] private FootstepSet footsteps;
    [SerializeField] private List<AudioClip> aggro;
    [SerializeField] private List<AudioClip> attack;
    [SerializeField] private List<AudioClip> punch;
    [SerializeField] private List<AudioClip> damaged;
    [SerializeField] private List<AudioClip> death;
    [Title("Audio Mixers", "Tie all sounds to one audio mixer. Use the overrides to target specific sounds.", bold: true)]
    [SerializeField] private AudioMixerGroup audioMixer;
    [SerializeField] private AudioMixerGroup footstepOverride;
    [SerializeField] private AudioMixerGroup aggroOverride;
    [SerializeField] private AudioMixerGroup attackOverride;
    [SerializeField] private AudioMixerGroup punchOverride;
    [SerializeField] private AudioMixerGroup damagedOverride;
    [SerializeField] private AudioMixerGroup deathOverride;

    public FootstepSet FootStepAudio { get { return footsteps; } }
    public List<AudioClip> AggroAudio { get { return aggro; } }
    public List<AudioClip> AttackAudio { get { return attack; } }
    public List<AudioClip> PunchAudio { get { return punch; } }
    public List<AudioClip> DamagedAudio { get { return damaged; } }
    public List<AudioClip> DeathAudio { get { return death; } }
    public AudioMixerGroup AudioMixer { get { return audioMixer; } }
    public AudioMixerGroup FootstepAudioMixerOverride { get { return footstepOverride; } }
    public AudioMixerGroup AttackAudioMixerOverride { get { return attackOverride; } }
    public AudioMixerGroup PunchAudioMixerOverride { get { return punchOverride; } }
    public AudioMixerGroup DamagedAudioMixerOverride { get { return damagedOverride; } }
    public AudioMixerGroup DeathAudioMixerOverride { get { return deathOverride; } }


}
