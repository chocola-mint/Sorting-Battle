using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame.UI
{
    public class DestroyGameObjectIfPlatform : MonoBehaviour
    {
        [SerializeField] private RuntimePlatform runtimePlatform;
        private void Awake() 
        {
            if(Application.platform == runtimePlatform) DestroyImmediate(gameObject);
        }
    }
}
