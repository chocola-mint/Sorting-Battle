using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame
{
    public interface IOnShowReceiver
    {
        public void OnShow();
    }
    public interface IOnHideReceiver
    {
        public void OnHide();
    }
    // TODO: Flatten the Animator structure on Title Canvas to ease UI extension.
    public class TitleManager : MonoBehaviour
    {
        [SerializeField] string showTrigger, hideTrigger;
        private int showTriggerHash, hideTriggerHash;
        private List<Animator> animators = new();
        private void Awake() 
        {
            showTriggerHash = Animator.StringToHash(showTrigger);
            hideTriggerHash = Animator.StringToHash(hideTrigger);
            if(TryGetComponent<Returnable>(out var returnable)) returnable.onReturn.AddListener(Return);
        }
        public void GoTo(Animator next)
        {
            Debug.Log($"Go to {next.gameObject.name}");
            if(animators.Count > 0) Hide(animators[^1]);
            Show(next);
            animators.Add(next);
        }
        public void Return()
        {
            if(animators.Count > 0)
            {
                Hide(animators[^1]);
                animators.RemoveAt(animators.Count - 1);
            }
            if(animators.Count > 0)
            {
                Show(animators[^1]);
            }
        }
        private void Show(Animator animator)
        {
            animator.SetTrigger(showTriggerHash);
            foreach(var receiver in animator.GetComponents<IOnShowReceiver>())
                receiver.OnShow();
        }
        private void Hide(Animator animator)
        {
            animator.SetTrigger(hideTriggerHash);
            foreach(var receiver in animator.GetComponents<IOnHideReceiver>())
                receiver.OnHide();
        }
    }
}
