using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame
{
    /// <summary>
    /// Data wrapper for different controller prefabs. 
    /// Makes sure that <see cref="GameBoard"></see> is always provided to GameControllers.
    /// </summary>
    [CreateAssetMenu(fileName = "PlayerType", menuName = "SortGame/PlayerType", order = 1)]
    public class PlayerType : ScriptableObject
    {
        [SerializeField] private GameController gameControllerPrefab;
        public GameController CreatePrefabInstance(GameBoard gameBoardComponent)
        {
            var instance = Instantiate(gameControllerPrefab);
            instance.gameBoardComponent = gameBoardComponent;
            return instance;
        }
    }
}
