using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SortGame.Sound
{
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(SFXSource))]
    public class ButtonSFXBinder : MonoBehaviour
    {
        [SerializeField] private SoundWrapper SFX;
        // Start is called before the first frame update
        void Start()
        {
            Button button = GetComponent<Button>();
            SFXSource source = GetComponent<SFXSource>();
            button.onClick.AddListener(() => source.PlaySFX(SFX));
        }
    }
}
