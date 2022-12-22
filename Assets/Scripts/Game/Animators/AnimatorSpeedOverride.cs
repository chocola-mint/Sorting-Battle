using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorSpeedOverride : MonoBehaviour
    {
        [SerializeField][Min(0)] private float speed = 1;
        // Start is called before the first frame update
        void Start()
        {
            GetComponent<Animator>().speed = speed;
        }
    }
}
