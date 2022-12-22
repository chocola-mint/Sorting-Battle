using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame
{
    /// <summary>
    /// A data container holding a collection of <see cref="AudioClip"></see>s.
    /// </summary>
    [CreateAssetMenu(fileName = "TrackList", menuName = "SortGame/TrackList", order = 1)]
    public class TrackList : ScriptableObject
    {
        [SerializeField] private AudioClip[] clips;
        public AudioClip GetRandomClip() => clips[Random.Range(0, clips.Length)];
    }
}
