using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame
{
    [RequireComponent(typeof(SFXSource))]
    public class GameGridSelectorSFXSource : MonoBehaviour
    {
        [SerializeField] private GameGridSelector gameGridSelector;
        [SerializeField] private SoundWrapper selectSFX;
        [SerializeField] private SoundWrapper selectFinishSFX;
        [Min(0)]
        [SerializeField] private float initialPitch = 1.0f;
        [SerializeField] private float pitchStep = 0.1f;
        private SFXSource source;
        // Start is called before the first frame update
        void Start()
        {
            source = GetComponent<SFXSource>();
            gameGridSelector.selectEvent += OnSelect;
            gameGridSelector.selectFinishEvent += OnSelectFinish;
        }
        private void OnDestroy() 
        {
            gameGridSelector.selectEvent -= OnSelect;
            gameGridSelector.selectFinishEvent -= OnSelectFinish;
        }
        private void OnSelect(int count)
        {
            source.SetPitch(initialPitch + (count - 1) * pitchStep);
            source.PlaySFX(selectSFX);
        }
        private void OnSelectFinish(int count)
        {
            if(count == 0) return;
            source.SetPitch(initialPitch);
            source.PlaySFX(selectFinishSFX);
        }
    }
}
