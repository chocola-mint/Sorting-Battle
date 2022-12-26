using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame.UI
{
    public class QuadToCanvasRect : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        private readonly Vector3[] corners = new Vector3[4];
        // Update is called once per frame
        void Update()
        {
            Snap();
        }
        [ContextMenu("Snap to RectTransform")]
        public void Snap()
        {
            rectTransform.GetWorldCorners(corners);
            Vector3 min = corners[0];
            Vector3 max = corners[2];
            Vector2 size = max - min;
            transform.localScale = size;
            transform.position = new Vector3(){
                x = rectTransform.position.x,
                y = rectTransform.position.y,
                z = transform.position.z
            };
        }
    }
}
