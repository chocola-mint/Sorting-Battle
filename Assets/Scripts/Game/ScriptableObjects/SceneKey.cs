using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Eflatun.SceneReference;

namespace SortGame
{
    /// <summary>
    /// Data wrapper around <see cref="SceneReference"></see> for easy access via UnityEvents.
    /// This approach is still preferred over direct reference via <see cref="Object></see> because
    /// the <see cref="SceneReference"></see> does additional checks for us.
    /// </summary>
    [CreateAssetMenu(fileName = "SceneKey", menuName = "SortGame/SceneKey", order = 1)]
    public class SceneKey : ScriptableObject
    {
        [SerializeField] private SceneReference sceneReference;
        /// <summary>
        /// Get the scene's name.
        /// </summary>
        /// <returns>The scene's name.</returns>
        public string GetName() => sceneReference.Name;
    }
}
