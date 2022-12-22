using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame
{
    /// <summary>
    /// A data container holding audio settings.
    /// </summary>
    [CreateAssetMenu(fileName = "GameAudioSettings", menuName = "SortGame/GameAudioSettings", order = 1)]
    public class GameAudioSettings : ScriptableObject
    {
        public float? BGMVolume { get; private set; } = null;
        public float? SFXVolume { get; private set; } = null;
        public void SetBGMVolume(float value) => BGMVolume = value;
        public void SetSFXVolume(float value) => SFXVolume = value;
    }
}
