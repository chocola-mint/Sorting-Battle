using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SortGame
{
    [RequireComponent(typeof(Button))]
    public class ButtonLock : MonoBehaviour
    {
        private Button button;
        private void Awake() 
        {
            button = GetComponent<Button>();
        }
        void LateUpdate()
        {
            button.interactable = false;
        }
    }
}
