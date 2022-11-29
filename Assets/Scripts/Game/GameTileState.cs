using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame
{
    [System.Serializable]
    public struct GameTileState
    {
        public int number;
        public const int Empty = -1, Garbage = -2;
        public GameTileState(int val = Empty) {
            number = val;
        }
        public bool IsEmpty() => number == Empty;
        public bool IsGarbage() => number == Garbage;
        public bool IsNumber() => number >= 0;
    }
}
