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
                if(settings.BGMVolume != null) audioSource.volume = (float) settings.BGMVolume;
            }
            isInitialized = true;
        }
        public void PlaySFX(SoundWrapper sound)
        {
            if(!isInitialized) Init();
            if(settings != null && settings.SFXVolume != null)
                audioSource.PlayOneShot(sound.clip, sound.volumeScale * (float) settings.SFXVolume);
            else audioSource.PlayOneShot(sound.clip, sound.volumeScale);
        }
    }
}
