using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame.Sound
{
    /// <summary>
    /// A data container holding audio settings.
    /// </summary>
    [CreateAssetMenu(fileName = "GameAudioSettings", menuName = "SortGame/GameAudioSettings", order = 1)]
    public class GameAudioSettings : ScriptableObject
    {
        public float? BGMVolume { get; private set; } = 0.5f;
        public float? SFXVolume { get; private set; } = 1.0f;
        public void SetBGMVolume(float value) => BGMVolume = value;
        public void SetSFXVolume(float value) => SFXVolume = value;
    }
}
