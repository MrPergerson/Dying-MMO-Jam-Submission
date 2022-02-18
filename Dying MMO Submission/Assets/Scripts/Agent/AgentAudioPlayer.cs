using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AgentAudioPlayer : MonoBehaviour, IAudioPlayer
{
    [Title("Audio Data")]
    [SerializeField] private AgentAudioData agentAudioData;
    
    [TitleGroup("Debug")]
    [SerializeField, ReadOnly] int currentAudioSourceCount = 0;

    private Dictionary<AudioMixerGroup, AudioSource> audioSources = new Dictionary<AudioMixerGroup, AudioSource>();
    private GameObject audioSourceContainer;

    private LayerMask groundMask;

    private void Awake()
    {
        audioSourceContainer = new GameObject("Agent AudioSource");
        audioSourceContainer.transform.parent = this.transform;
        audioSourceContainer.transform.position = Vector3.zero + Vector3.up;
        groundMask = LayerMask.GetMask("Ground");
    }

    private void CreateNewAudioSource(AudioMixerGroup mixer)
    {
        var audioSource = audioSourceContainer.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = mixer;
        audioSources.Add(mixer, audioSource);

        currentAudioSourceCount++;

        // there is no limit to how many audio sources can be created
        if (currentAudioSourceCount > 8)
            Debug.LogWarning(this + " has more than 8 audio sources. Current count is " + currentAudioSourceCount);
    }

    public void PlayAudioClip(AudioClip clip, AudioMixerGroup mixer)
    {
        if (mixer == null)
        {
            Debug.LogError(this + " -> PlayAudio(): Audio Mixer recieved was null");
            return;
        }

        if(!audioSources.ContainsKey(mixer))
        {
            CreateNewAudioSource(mixer);
        }

        audioSources[mixer].PlayOneShot(clip);
    }

    void PlayStepSound()
    {
        List<AudioClip> footstepSounds = new List<AudioClip>();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.up * -1, out hit, 2, groundMask))
        {
            var tag = hit.collider.tag;
            if (tag == "Ground_Grass")
            {
                var grassSounds = agentAudioData.FootStepAudio.grassFootsteps;
                if (grassSounds == null) Debug.LogWarning("GrassFootSteps in " + agentAudioData.name + " is unassigned.");
                footstepSounds = new List<AudioClip>(grassSounds);
            }
            else if (tag == "Ground_Stone")
            {
                var stoneSounds = agentAudioData.FootStepAudio.stoneFootsteps;
                if (stoneSounds == null) Debug.LogWarning("StoneFootSteps in " + agentAudioData.name + " is unassigned.");
                footstepSounds = new List<AudioClip>(stoneSounds);
            }
        }

        // should default to default footstep sounds if no other sounds were found
        if(footstepSounds.Count == 0)
        {
            var defaultSounds = agentAudioData.FootStepAudio.defaultFootsteps;
            if (defaultSounds == null) Debug.LogWarning("defaultFootSteps in " + agentAudioData.name + " is unassigned.");
            footstepSounds = new List<AudioClip>(defaultSounds);
        }


        int randomIndex = Random.Range(0, footstepSounds.Count);

        var clip = footstepSounds[randomIndex];
        var mixer = agentAudioData.FootstepAudioMixerOverride == null ? agentAudioData.AudioMixer : agentAudioData.FootstepAudioMixerOverride;

        PlayAudioClip(clip, mixer);
    }

    [TitleGroup("Debug")]
    [Button()]
    private void PlayTestAudio()
    {
        var clip = agentAudioData.FootStepAudio.defaultFootsteps[0];
        AudioMixerGroup mixer = agentAudioData.AudioMixer;

        if (!audioSources.ContainsKey(mixer))
        {
            CreateNewAudioSource(mixer);
        }

        audioSources[mixer].PlayOneShot(clip);  
    }

    public void playDamagedSound()
    {
        if(agentAudioData != null && agentAudioData.DamagedAudio.Count > 0)
        {
            var damageSound = agentAudioData.DamagedAudio[0];
            var damageMixer = agentAudioData.DamagedAudioMixerOverride!=null? agentAudioData.DamagedAudioMixerOverride: agentAudioData.AudioMixer;
            if(damageSound != null && damageMixer != null)
            {
                PlayAudioClip(damageSound, damageMixer);

            }
        }
    }

}
