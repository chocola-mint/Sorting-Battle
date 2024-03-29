using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;
namespace SortGame
{
    public class GameOverOverlay : MonoBehaviour
    {
        [SerializeField] TMP_Text resultText;
        [SerializeField] UnityEvent onRestart, onReturnToTitleScreen;
        public void SetResultText(string content)
            => resultText.text = content;
        public void Restart()
        {
            EventSystem.current.enabled = false;
            onRestart.Invoke();
        }
        public void ReturnToTitleScreen()
        {
            EventSystem.current.enabled = false;
            onReturnToTitleScreen.Invoke();
        }
    }
}
