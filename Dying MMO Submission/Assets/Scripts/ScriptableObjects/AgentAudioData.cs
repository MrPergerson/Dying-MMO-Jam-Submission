using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AgentAudioData", menuName = "Data/AgentAudioData")]
public class AgentAudioData : ScriptableObject
{
    [System.Serializable]
    public struct FootstepSet
    {
        public List<AudioClip> defaultFootsteps;
        public List<AudioClip> grassFootsteps;
        public List<AudioClip> stoneFootsteps;
    }
    [SerializeField] private FootstepSet footsteps;
    [SerializeField] private List<AudioClip> attack;
    [SerializeField] private List<AudioClip> damaged;
    [SerializeField] private List<AudioClip> death;

    public FootstepSet FootStepAudio { get { return footsteps; } }
    public List<AudioClip> AttackAudio { get { return attack; } }
    public List<AudioClip> DamagedAudio { get { return damaged; } }
    public List<AudioClip> DeathAudio { get { return death; } }


}
