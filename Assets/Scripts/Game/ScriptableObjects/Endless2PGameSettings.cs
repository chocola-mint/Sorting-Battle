using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame
{
    /// <summary>
    /// Data wrapper for different controller prefabs. 
    /// Makes sure that <see cref="GameBoard"></see> is always provided to GameControllers.
    /// </summary>
    [CreateAssetMenu(fileName = "Endless2PGameSettings", menuName = "SortGame/GameSettings/Endless2PGameSettings", order = 1)]
    public class Endless2PGameSettings : ScriptableObject
    {
        public PlayerType p1PlayerTypeOverride { get; set; } = null;
        public PlayerType p2PlayerTypeOverride { get; set; } = null;
    }
}
