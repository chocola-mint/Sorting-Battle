using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SortGame.UI
{
    /// <summary>
    /// Utility component that enables holding Esc for <see cref="confirmTime"></see> seconds
    /// to quit the game anywhere.
    /// Only enabled on non-web platforms.
    /// </summary>
    public class EscapeToQuitGame : MonoBehaviour
    {
        private float waitEndTime = float.PositiveInfinity;
        private const float confirmTime = 2.0f;
        [RuntimeInitializeOnLoadMethod]
        public static void Init()
        {
            if(Application.platform == RuntimePlatform.WebGLPlayer)
            {
                Debug.Log($"{nameof(EscapeToQuitGame)} disabled because this is a WebGL build.");
                return;
            }
            GameObject observer = new($"{nameof(EscapeToQuitGame)} Observer");
            observer.AddComponent<EscapeToQuitGame>();
            DontDestroyOnLoad(observer);
        }
        // Update is called once per frame
        void Update()
        {
            if(Keyboard.current.escapeKey.wasPressedThisFrame)
                waitEndTime = Time.unscaledTime + confirmTime;
            if(Keyboard.current.escapeKey.wasReleasedThisFrame)
                waitEndTime = float.PositiveInfinity;

            if(Time.unscaledTime > waitEndTime)
            {
#if UNITY_EDITOR
                Debug.Log($"Quitting game via {nameof(EscapeToQuitGame)}. (Simulated as ExitPlaymode)");
                UnityEditor.EditorApplication.ExitPlaymode();
#else
                Application.Quit();
#endif
            }
        }
    }
}
