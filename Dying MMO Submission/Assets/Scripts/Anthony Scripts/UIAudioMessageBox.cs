using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Sirenix.OdinInspector;

public class UIAudioMessageBox : MonoBehaviour, IAudioPlayer
{
    [Title("UI Audio Data")]
    [SerializeField] private UIAudioData uiAudioData;

    private Dictionary<AudioMixerGroup, AudioSource> audioSources = new Dictionary<AudioMixerGroup, AudioSource>();
    private GameObject audioSourceContainer;

    [TitleGroup("Debug")]
    [SerializeField, ReadOnly] int currentAudioSourceCount = 0;

    public void PlayAudioClip(AudioClip clip, AudioMixerGroup mixer)
    {
        if (mixer == null)
        {
            Debug.LogError(this + " -> PlayAudio(): Audio Mixer recieved was null");
            return;
        }

        if (!audioSources.ContainsKey(mixer))
        {
            CreateNewAudioSource(mixer);
        }

        audioSources[mixer].PlayOneShot(clip);
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

    public void UIPlayAlertSound()
    {
        if (uiAudioData != null)
        {
            var alertSound = uiAudioData.Alert;
            var alertMixer = uiAudioData.AlertAudioMixerOverride != null ? uiAudioData.AlertAudioMixerOverride : uiAudioData.AudioMixer;
            if (alertSound != null && alertMixer != null)
            {
                PlayAudioClip(alertSound, alertMixer);
            }
        }
    }

    public void UIPlayChatPingSound()
    {
        if (uiAudioData != null)
        {
            var chatPingSound = uiAudioData.ChatPing;
            var chatPingMixer = uiAudioData.ChatPingAudioMixerOverride != null ? uiAudioData.ChatPingAudioMixerOverride : uiAudioData.AudioMixer;
            if (chatPingSound != null && chatPingMixer != null)
            {
                PlayAudioClip(chatPingSound, chatPingMixer);
            }
        }
    }

    public void UIPlayButtonSound(string buttonName)
    {
        if (uiAudioData != null && uiAudioData.ButtonAudioSet.Count > 0)
        {
            var chatButtonSound = uiAudioData.ButtonAudioSet[0];
            for (int x=0; x<uiAudioData.ButtonAudioSet.Count; x++)
            {
                if (uiAudioData.ButtonAudioSet[x].name == buttonName)
                {
                    chatButtonSound = uiAudioData.ButtonAudioSet[x];
                }
            }
            //print(chatButtonSound.name);
            var chatButtonMixer = uiAudioData.ButtonAudioSetAudioMixerOverride != null ? uiAudioData.ButtonAudioSetAudioMixerOverride : uiAudioData.AudioMixer;
            if (chatButtonSound != null && chatButtonMixer != null)
            {
                PlayAudioClip(chatButtonSound, chatButtonMixer);
            }
            else if(chatButtonSound == null)
            {
                Debug.LogError(this + ": audio button sound is empty.");
            }
        }
    }
}
