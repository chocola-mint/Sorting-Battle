using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChocoUtil.Algorithms;

namespace SortGame
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
                if(settings.BGMVolume != null) audioSource.volume = (float) settings.BGMVolume;
            }
            if(playOnAwake) PlayRandom();
        }
        public void PlayRandom()
        {
            audioSource.clip = trackList.GetRandomClip();
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
