using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame
{
    /// <summary>
    /// Controller base class for AI controllers.
    /// </summary>
    public abstract class AIController : GameController
    {
        [SerializeField] private int decisionPeriod = 10;
        private static WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

        private IEnumerator Start()
        {
            Init();
            while(gameBoard.state.status == GameBoardState.Status.Active)
            {
                var waitConstraintAfterAction = OnAction();
                if(waitConstraintAfterAction != null) 
                    yield return waitConstraintAfterAction;
                yield return CoroWaitDecisionPeriod();
            }
        }
        private IEnumerator CoroWaitDecisionPeriod()
        {
            for(int i = 0; i < decisionPeriod; ++i)
                yield return waitForFixedUpdate;
        }
        protected virtual void Init() {}
        protected abstract IEnumerator OnAction();
    }
}
