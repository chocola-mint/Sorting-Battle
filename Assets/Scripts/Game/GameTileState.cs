using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame
{
    [System.Serializable]
    public struct GameTileState
    {
        public int number;
        public GameTileState(int val = -1) {
            number = val;
        }
        public bool IsEmpty() => number < 0;
    }
}
