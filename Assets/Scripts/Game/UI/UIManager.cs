using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace SortGame.UI
{
    public interface IOnShowReceiver
    {
        public void OnShow();
    }
    public interface IOnHideReceiver
    {
        public void OnHide();
    }
    /// <summary>
    /// Manages UI flow using <see cref="GoTo"></see> and <see cref="Return"></see>.
    /// </summary>
    [RequireComponent(typeof(Returnable))]
    public class UIManager : MonoBehaviour
    {
        [SerializeField] string showTrigger = "Show", hideTrigger = "Hide";
        private int showTriggerHash, hideTriggerHash;
        private List<Animator> animators = new();
        private Coroutine waitHandle;
        [SerializeField, Tooltip("The default animator to go to on Awake.")] 
        private Animator defaultAnimator;
        private int minimumAnimatorCount = 0;
        private Selectable[] childSelectables;
        protected Returnable returnable { get; private set; }
        public UnityEvent onTransitionStart;
        protected void Awake() 
        {
            showTriggerHash = Animator.StringToHash(showTrigger);
            hideTriggerHash = Animator.StringToHash(hideTrigger);
            childSelectables = GetComponentsInChildren<Selectable>(true);
            returnable = GetComponent<Returnable>();
            returnable.onReturn.AddListener(Return);
            if(defaultAnimator) 
            {
                GoTo(defaultAnimator);
                minimumAnimatorCount = 1;
            }
            else minimumAnimatorCount = 0;
        }
        public void GoTo(Animator next)
        {
            Debug.Log($"Go to {next.gameObject.name}");
            List<Animator> activeAnimators = new();
            if(animators.Count > 0) 
            {
                Hide(animators[^1]);
                activeAnimators.Add(animators[^1]);
            }
            Show(next);
            activeAnimators.Add(next);
            animators.Add(next);
            HandleReturnButtonState(activeAnimators);
        }
        public void Return()
        {
            List<Animator> activeAnimators = new();
            if(animators.Count > 0)
            {
                Hide(animators[^1]);
                activeAnimators.Add(animators[^1]);
                animators.RemoveAt(animators.Count - 1);
            }
            if(animators.Count > 0)
            {
                Show(animators[^1]);
                activeAnimators.Add(animators[^1]);
            }
            if(activeAnimators.Count > 0) HandleReturnButtonState(activeAnimators);
        }
        public void ReturnAll()
        {
            for(int i = animators.Count - 1; i >= 1; --i) Hide(animators[i]);
            if(animators.Count > 0) 
            {
                Show(animators[0]);
                HandleReturnButtonState(new List<Animator>(animators));
            }
            animators.Clear();
            animators.Add(defaultAnimator);
        }
        public void End()
        {
            // End means all buttons are unselectable.
            foreach(var selectable in childSelectables)
                selectable.interactable = false;
        }
        private void Show(Animator animator)
        {
            onTransitionStart.Invoke();
            animator.ResetTrigger(hideTriggerHash);
            animator.SetTrigger(showTriggerHash);
            foreach(var receiver in animator.GetComponents<IOnShowReceiver>())
                receiver.OnShow();
        }
        private void Hide(Animator animator)
        {
            onTransitionStart.Invoke();
            animator.ResetTrigger(showTriggerHash);
            animator.SetTrigger(hideTriggerHash);
            foreach(var receiver in animator.GetComponents<IOnHideReceiver>())
                receiver.OnHide();
        }
        private void HandleReturnButtonState(List<Animator> activeAnimators)
        {
            if(waitHandle != null) StopCoroutine(waitHandle);
            waitHandle = StartCoroutine(CoroWaitForActiveAnimators(activeAnimators));
        }
        private IEnumerator CoroWaitForActiveAnimators(List<Animator> activeAnimators)
        {
            foreach(var selectable in childSelectables)
                selectable.interactable = false;

            foreach(var animator in activeAnimators) 
                yield return animator.WaitUntilCurrentStateIsDone();

            // This makes it so the return button cannot be used 
            // to make the animator count go below the minimum. (Root level)
            if(animators.Count > minimumAnimatorCount)
                foreach(var selectable in childSelectables)
                    selectable.interactable = true;
        }
    }
}
