using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SortGame
{
    public class GameTile : MonoBehaviour
    {
        private GameBoard board;
        private Vector2Int coord;
        public Vector2Int gridCoord => coord;
        private void Awake() 
        {
            board = GetComponentInParent<GameBoard>();
            coord = new Vector2Int{
                x = transform.parent.GetSiblingIndex(),
                y = transform.GetSiblingIndex()
            };
            board.state.gameGridState.RegisterTileCallbacks(coord, Select, Deselect);
        }
        public void Select()
        {
            foreach(var receiver in GetComponents<IOnSelectReceiver>())
                receiver.OnSelect();
        }
        public void Deselect()
        {
            foreach(var receiver in GetComponents<IOnDeselectReceiver>())
                receiver.OnDeselect();
        }
    }

}