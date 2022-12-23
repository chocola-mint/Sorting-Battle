using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame
{
    [RequireComponent(typeof(AudioSource))]
    public class SFXSource : MonoBehaviour
    {
        [SerializeField] private GameAudioSettings settings;
        private AudioSource audioSource;
        private void Awake() 
        {
            audioSource = GetComponent<AudioSource>();
            if(settings != null)
            {
                // Apply overrides.
                if(settings.BGMVolume != null) audioSource.volume = (float) settings.BGMVolume;
            }
        }
        public void SetPitch(float pitch) => audioSource.pitch = pitch;
        public void PlaySFX(SoundWrapper sound)
        {
            if(settings != null && settings.SFXVolume != null)
                audioSource.PlayOneShot(sound.clip, sound.volumeScale * (float) settings.SFXVolume);
            else audioSource.PlayOneShot(sound.clip, sound.volumeScale);
        }
    }
}
