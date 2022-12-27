using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChocoUtil.Algorithms;

namespace SortGame.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class Jukebox : MonoBehaviour
    {
        [SerializeField] private GameAudioSettings settings;
        [SerializeField] TrackList trackList;
        [SerializeField] private bool playOnAwake = true;
        private AudioSource audioSource;
        private void Awake() 
        {
            audioSource = GetComponent<AudioSource>();
            if(settings != null)
            {
                // Apply overrides.
                audioSource.volume = settings.BGMVolume;
                settings.onBGMVolumeChanged += SetVolume;
            }
            if(playOnAwake) PlayRandom();
        }
        private void OnDestroy() 
        {
            if(settings != null)
            {
                settings.onBGMVolumeChanged -= SetVolume;
            }
        }
        private void SetVolume(float volume) => audioSource.volume = volume;
        public void PlayRandom()
        {
            PlayClip(trackList.GetRandomClip());
        }
        public void PlayClip(AudioClip clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
        public void Stop(float falloffDuration)
        {
            StopAllCoroutines();
            StartCoroutine(FadeVolume(falloffDuration));
        }
        private IEnumerator FadeVolume(float falloffDuration)
        {
            yield return null;
            float startVolume = audioSource.volume;
            foreach(var t in Interp.GetSteps(falloffDuration, Ease.Linear))
            {
                audioSource.volume = Mathf.Lerp(startVolume, 0, t);
                yield return null;
            }
            audioSource.volume = 0;
        }
    }
}
