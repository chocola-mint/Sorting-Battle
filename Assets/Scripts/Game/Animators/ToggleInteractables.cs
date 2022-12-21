using UnityEngine;
using UnityEngine.UI;

namespace SortGame
{
    public class ToggleInteractables : StateMachineBehaviour
    {
        [System.Serializable]
        private enum BehaviourType
        {
            None,
            ToggleOff,
            ToggleOn,
            Flip,
        }
        [SerializeField]
        private BehaviourType onEnter, onExit;
        private void ExecuteToggleBehaviour(BehaviourType behaviourType, Animator animator)
        {
            switch (behaviourType)
            {
                case BehaviourType.ToggleOff:
                    foreach (var selectable in animator.GetComponentsInChildren<Selectable>())
                        selectable.interactable = false;
                    break;
                case BehaviourType.ToggleOn:
                    foreach (var selectable in animator.GetComponentsInChildren<Selectable>())
                        selectable.interactable = true;
                    break;
                case BehaviourType.Flip:
                    foreach (var selectable in animator.GetComponentsInChildren<Selectable>())
                        selectable.interactable ^= true; // XOR flip.
                    break;
                case BehaviourType.None:
                default:
                    break;
            }
        }
    
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            ExecuteToggleBehaviour(onEnter, animator);
        }
        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
           ExecuteToggleBehaviour(onExit, animator);
        }

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
    }
}
