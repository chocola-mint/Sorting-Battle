using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SortGame.Sound;

namespace SortGame.UI
{
    public class VolumeSliderInitializer : MonoBehaviour
    {
        [SerializeField] private Slider sliderForBGM, sliderForSFX;
        [SerializeField] private GameAudioSettings audioSettings;
        // Start is called before the first frame update
        void Start()
        {
            sliderForBGM.value = audioSettings.BGMVolume;
            sliderForSFX.value = audioSettings.SFXVolume;
        }
    }
}
