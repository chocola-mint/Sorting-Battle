using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame
{
    /// <summary>
    /// Represents an ingame player.
    /// </summary>
    public class Player : MonoBehaviour
    {
        /// <summary>
        /// The type of player (human, CPU, etc.)
        /// Intentionally made public, so MonoBehaviours with higher execution priority
        /// can inject player type if necessary.
        /// </summary>
        public PlayerType playerType;
        [SerializeField] private GameBoard gameBoard;
        private void Awake() 
        {
            playerType.CreatePrefabInstance(gameBoard).transform.SetParent(transform);
        }
    }
}
