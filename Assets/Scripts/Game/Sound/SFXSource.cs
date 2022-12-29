using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class SFXSource : MonoBehaviour
    {
        [SerializeField] private GameAudioSettings settings;
        private AudioSource audioSource;
        private bool isInitialized = false;
        private void Awake() 
        {
            if(!isInitialized) Init();
        }
        private void Init()
        {
            audioSource = GetComponent<AudioSource>();
            if(settings != null)
            {
                // Apply overrides.
                audioSource.volume = settings.SFXVolume;
                settings.onSFXVolumeChanged += SetVolume;
            }
            isInitialized = true;
        }
        private void OnDestroy() 
        {
            if(settings != null)
            {
                settings.onSFXVolumeChanged -= SetVolume;
            }
        }
        private void SetVolume(float volume) => audioSource.volume = volume;
        public void PlaySFX(SoundWrapper sound)
        {
            if(!isInitialized) Init();
            if(settings != null)
                audioSource.PlayOneShot(sound.clip, sound.volumeScale * settings.SFXVolume);
            else audioSource.PlayOneShot(sound.clip, sound.volumeScale);
        }
    }
}
