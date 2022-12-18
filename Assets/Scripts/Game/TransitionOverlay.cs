using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
namespace SortGame
{
    public class TransitionOverlay : MonoBehaviour
    {
        // ! These objects should set themselves to inactive once they are done.
        [Tooltip("GameObjects that contain transition effects. Should turn themselves inactive after they end.")]
        [SerializeField] List<GameObject> entrance, exit;
        public UnityEvent onEntranceTransitionOver;
        private void Start() 
        {
            StartCoroutine(CoroEnterScene());    
        }
        private IEnumerator ProcessTransitions(List<GameObject> transitionList)
        {
            foreach(var transitionPrefab in transitionList)
            {
                var transition = Instantiate(transitionPrefab);
                yield return new WaitUntil(() => !transition.activeSelf);
                Destroy(transition);
            }
        }
        private IEnumerator CoroEnterScene()
        {
            if(entrance.Count > 0)
                yield return ProcessTransitions(entrance);
            onEntranceTransitionOver.Invoke();
        }
        public void RestartScene() => MoveToScene(SceneManager.GetActiveScene().name);
        public void MoveToScene(string sceneName)
        {
            StopAllCoroutines();
            StartCoroutine(CoroMoveToScene(sceneName));
        }
        private IEnumerator CoroMoveToScene(string sceneName)
        {
            var task = SceneManager.LoadSceneAsync(sceneName);
            task.allowSceneActivation = false;
            if(exit != null)
            {
                if(exit.Count > 0)
                    yield return ProcessTransitions(exit);
            }
            // Then wait for the load scene operation to be done.
            task.allowSceneActivation = true;
            yield return task;
        }
    }
}
