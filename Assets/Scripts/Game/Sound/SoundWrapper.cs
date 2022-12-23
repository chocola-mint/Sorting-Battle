using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame.Sound
{
    /// <summary>
    /// A data container holding a <see cref="AudioClip"></see> and its volume scale.
    /// </summary>
    [CreateAssetMenu(fileName = "SoundWrapper", menuName = "SortGame/SoundWrapper", order = 1)]
    public class SoundWrapper : ScriptableObject
    {
        [Range(0, 1)]
        public float minVolumeScale = 1.0f, maxVolumeScale = 1.0f;
        public float volumeScale => Random.Range(minVolumeScale, maxVolumeScale);
        public AudioClip clip;
        private void OnValidate() 
        {
            maxVolumeScale = Mathf.Max(minVolumeScale, maxVolumeScale);
            minVolumeScale = Mathf.Min(minVolumeScale, maxVolumeScale);
        }
        
    }
}
