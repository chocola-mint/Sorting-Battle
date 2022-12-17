using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame
{
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
