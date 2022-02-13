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

    private void Awake()
    {
        audioSourceContainer = new GameObject("Agent AudioSource");
        audioSourceContainer.transform.parent = this.transform;
        audioSourceContainer.transform.position = Vector3.zero + Vector3.up;
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
        if (clip == null) Debug.LogError(this + " -> PlayAudio(): Audio clip recieved was null");
        if (mixer == null) Debug.LogError(this + " -> PlayAudio(): Audio Mixer recieved was null");

        if(!audioSources.ContainsKey(mixer))
        {
            CreateNewAudioSource(mixer);
        }

        audioSources[mixer].PlayOneShot(clip);
    }

    void PlayStepSound()
    {
        // WIP

        /*
        if (agentAudioData.FootStepAudio.defaultFootsteps == null || agentAudioData.FootStepAudio.defaultFootsteps.Count == 0)
        {
            Debug.LogWarning("There are no footstep sounds to play in defaultFootSteps.");

        }
        else
        {
            while (isMoving)
            {
                var footstepSounds = new List<AudioClip>(agentAudioData.FootStepAudio.defaultFootsteps);
                int randomIndex = Random.Range(0, footstepSounds.Count);

                if (audioData.FootstepAudioMixerOverride == null)
                {
                    audioSource.outputAudioMixerGroup = audioData.AudioMixer;
                }
                else
                {
                    audioSource.outputAudioMixerGroup = audioData.FootstepAudioMixerOverride;
                }
                audioSource.clip = footstepSounds[randomIndex];
                audioSource.Play();

                yield return new WaitForSeconds(footstepSpeed);
            }
        }
        */
        // do coroutines stop automatically if they reac 
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
}
