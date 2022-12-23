using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame
{
    [RequireComponent(typeof(AudioSource))]
    public class DestroyGameObjectOnAudioEnd : MonoBehaviour
    {
        IEnumerator Start()
        {
            var audioSource = GetComponent<AudioSource>();
            yield return new WaitWhile(() => audioSource.isPlaying);
            Destroy(gameObject);
        }
    }
}
