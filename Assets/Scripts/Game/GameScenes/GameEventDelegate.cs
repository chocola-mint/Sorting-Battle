using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SortGame
{
    public class GameEventDelegate : MonoBehaviour
    {
        public UnityEvent onGameOverStart, onGameOverEnd;
    }
}
