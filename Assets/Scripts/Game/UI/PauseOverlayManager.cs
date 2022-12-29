using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SortGame.UI
{
    public class PauseOverlayManager : UIManager
    {
        [SerializeField] private Button pauseButton;
        [SerializeField] private PauseManager pauseManager;
        [SerializeField] private GameEventDelegate gameEventDelegate;
        [SerializeField] private TransitionOverlay transitionOverlay;
        private new void Awake() 
        {
            base.Awake();
            if(gameEventDelegate)
                gameEventDelegate.onGameOverStart.AddListener(() => pauseButton.interactable = false);
            if(transitionOverlay) 
                transitionOverlay.onEntranceTransitionOver.AddListener(() => pauseButton.interactable = true);
            pauseButton.interactable = false;
        }
        private void OnValidate() 
        {
            gameEventDelegate = FindObjectOfType<GameEventDelegate>();
            transitionOverlay = FindObjectOfType<TransitionOverlay>();
        }
        public void Unpause()
        {
            ReturnAll();
            pauseManager.Unpause();
        }
        public void Restart()
        {
            Unpause();
            transitionOverlay.RestartScene();
        }
        public void GoToScene(SceneKey sceneKey)
        {
            Unpause();
            transitionOverlay.MoveToScene(sceneKey);
        }
    }
}
