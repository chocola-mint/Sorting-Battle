using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame
{
    public class GameTileState
    {
        public int number;
        public const int Empty = -1, Garbage = -2;
        public event System.Action<Vector2Int> onBlockMove;
        public event System.Action onBlockRemove, onTileSelect, onTileDeselect;
        public GameTileState(int val = Empty) {
            number = val;
            onBlockMove = null;
            onBlockRemove = onTileSelect = onTileDeselect = null;
        }
        public static void Swap(GameTileState sourceTile, Vector2Int sourceCoord, GameTileState destTile, Vector2Int destCoord)
        {
            (sourceTile.number, destTile.number) = (destTile.number, sourceTile.number);
            sourceTile.onBlockMove?.Invoke(destCoord);
            destTile.onBlockMove?.Invoke(sourceCoord);
            (sourceTile.onBlockMove, destTile.onBlockMove) = (destTile.onBlockMove, sourceTile.onBlockMove);
            (sourceTile.onBlockRemove, destTile.onBlockRemove) = (destTile.onBlockRemove, sourceTile.onBlockRemove);
        }
        public void Remove()
        {
            number = Empty;
            onBlockRemove?.Invoke();
            onBlockMove = null;
            onBlockRemove = null;
        }
        public void Select()
        {
            onTileSelect?.Invoke();
        }
        public void Deselect()
        {
            onTileDeselect?.Invoke();
        }
        public bool IsEmpty() => number == Empty;
        public bool IsGarbage() => number == Garbage;
        public bool IsNumber() => number >= 0;
    }
}
