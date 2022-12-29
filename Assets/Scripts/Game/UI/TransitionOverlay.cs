using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
namespace SortGame.UI
{
    /// <summary>
    /// Component that handles transition effects for the scene it is in.
    /// <br></br>
    /// Will trigger "Entrance" transition on the first frame of activation.
    /// The "Exit" transition will happen when <see cref="RestartScene"></see> or
    /// <see cref="MoveToScene"></see> is invoked.
    /// </summary>
    public class TransitionOverlay : MonoBehaviour
    {
        public enum TransitionCleanupMode
        {
            Destroy,
            Disable,
            DoNothing,
        }
        [SerializeField] TransitionCleanupMode cleanupMode = TransitionCleanupMode.DoNothing;
        // ! These objects should set themselves to inactive once they are done.
        [Tooltip("Sequence of GameObjects that contain transition effects. Should use TransitionDoneMarker to mark the end of a transition")]
        [SerializeField] List<GameObject> entrance, exit;
        public UnityEvent onEntranceTransitionOver, onExitTransitionStart;
        private void Start() 
        {
            StartCoroutine(CoroEnterScene());    
        }
        private IEnumerator ProcessTransitions(List<GameObject> transitionList)
        {
            // Process transitions in sequential order.
            foreach(var transitionPrefab in transitionList)
            {
                // Instantiate transition object.
                var transition = Instantiate(transitionPrefab);
                // Then wait for the transition to end.
                yield return new WaitUntil(() => transition.TryGetComponent<TransitionDoneMarker>(out _));
                // Finally, cleanup.
                switch(cleanupMode)
                {
                    case TransitionCleanupMode.Destroy:
                        Destroy(transition);
                        break;
                    case TransitionCleanupMode.Disable:
                        transition.SetActive(false);
                        break;
                    case TransitionCleanupMode.DoNothing:
                        default:
                        break;
                }
            }
        }
        private IEnumerator CoroEnterScene()
        {
            if(entrance.Count > 0)
                yield return ProcessTransitions(entrance);
            onEntranceTransitionOver.Invoke();
        }
        /// <summary>
        /// Reload the current scene, triggering transitions.
        /// </summary>
        public void RestartScene() => MoveToScene(SceneManager.GetActiveScene().name);
        /// <summary>
        /// Load the given scene, triggering transitions.
        /// </summary>
        public void MoveToScene(string sceneName)
        {
            onExitTransitionStart.Invoke();
            StopAllCoroutines();
            StartCoroutine(CoroMoveToScene(sceneName));
        }
        /// <summary>
        /// Shortcut that takes in the scene asset instead. Will use the name of the asset as scene name.
        /// </summary>
        public void MoveToScene(Object sceneAsset)
        {
            MoveToScene(sceneAsset.name);
        }
        /// <summary>
        /// Shortcut that takes in a <see cref="SceneKey"></see> ScriptableObject.
        /// </summary>
        /// <param name="sceneKey"></param>
        public void MoveToScene(SceneKey sceneKey)
        {
            MoveToScene(sceneKey.GetName());
        }
        private IEnumerator CoroMoveToScene(string sceneName)
        {
            // Start the async load scene task.
            var task = SceneManager.LoadSceneAsync(sceneName);
            task.allowSceneActivation = false;
            // Process exit transitions.
            cleanupMode = TransitionCleanupMode.DoNothing;
            if(exit != null)
            {
                if(exit.Count > 0)
                    yield return ProcessTransitions(exit);
            }
            // Then wait for the load scene operation to be done.
            task.allowSceneActivation = true;
            yield return task;
        }
        public void Quit()
        {
            onExitTransitionStart.Invoke();
            StopAllCoroutines();
            StartCoroutine(CoroQuitApplication());
        }
        private IEnumerator CoroQuitApplication()
        {
            // Process exit transitions.
            cleanupMode = TransitionCleanupMode.DoNothing;
            if(exit != null)
            {
                if(exit.Count > 0)
                    yield return ProcessTransitions(exit);
            }
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
            #else
            Application.Quit();
            #endif
        }
    }
}
