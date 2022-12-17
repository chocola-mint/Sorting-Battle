using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame
{
    public class Player : MonoBehaviour
    {
        public PlayerType playerType;
        [SerializeField] private GameBoard gameBoard;
        private void Awake() 
        {
            playerType.CreatePrefabInstance(gameBoard).transform.SetParent(transform);
        }
    }
}
