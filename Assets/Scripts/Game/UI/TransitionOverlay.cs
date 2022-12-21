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
        // ! These objects should set themselves to inactive once they are done.
        [Tooltip("Sequence of GameObjects that contain transition effects. Should turn themselves inactive after they end.")]
        [SerializeField] List<GameObject> entrance, exit;
        public UnityEvent onEntranceTransitionOver;
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
                // Then wait for the transition object to turn itself off.
                yield return new WaitUntil(() => !transition.activeSelf);
                // Finally, destroy the transition object.
                Destroy(transition);
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
            StopAllCoroutines();
            StartCoroutine(CoroMoveToScene(sceneName));
        }
        /// <summary>
        /// Shortcut that takes in the scene asset instead. Will use the name of the asset as scene name.
        /// </summary>
        /// <param name="sceneAsset"></param>
        public void MoveToScene(Object sceneAsset)
        {
            MoveToScene(sceneAsset.name);
        }
        private IEnumerator CoroMoveToScene(string sceneName)
        {
            // Start the async load scene task.
            var task = SceneManager.LoadSceneAsync(sceneName);
            task.allowSceneActivation = false;
            // Process exit transitions.
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
