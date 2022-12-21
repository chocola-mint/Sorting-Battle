using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using TransitionOverlay = SortGame.UI.TransitionOverlay;
namespace SortGame
{
    public class GameOverOverlay : MonoBehaviour
    {
        [SerializeField] TransitionOverlay transitionOverlay;
        [SerializeField] Object titleScreenScene;
        [SerializeField] TMP_Text resultText;
        public void SetResultText(string content)
            => resultText.text = content;
        public void Restart()
        {
            EventSystem.current.enabled = false;
            transitionOverlay.RestartScene();
        }
        public void ReturnToTitleScreen()
        {
            EventSystem.current.enabled = false;
            transitionOverlay.MoveToScene(titleScreenScene.name);
        }
    }
}
