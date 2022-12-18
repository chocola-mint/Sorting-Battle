using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame
{
    public class AnimationEventHelper : MonoBehaviour
    {
        public void EndTransition() => gameObject.SetActive(false);
        public void Destroy() => Destroy(gameObject);
    }
}
