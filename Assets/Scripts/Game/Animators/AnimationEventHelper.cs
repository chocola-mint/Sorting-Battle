using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame
{
    public class AnimationEventHelper : MonoBehaviour
    {
        public void EndTransition() => gameObject.AddComponent<SortGame.UI.TransitionDoneMarker>();
        public void Destroy() => Destroy(gameObject);
    }
}
