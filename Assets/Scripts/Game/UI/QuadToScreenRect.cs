using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame.UI
{
    /// <summary>
    /// Utility component that transforms a Rect mesh or a Square sprite so that it covers
    /// the Main Camera's view.
    /// </summary>
    public class QuadToScreenRect : MonoBehaviour
    {
        [SerializeField][Min(0)] float distanceFromCamera = 0;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            Snap();
        }

        [ContextMenu("Snap to Camera")]
        public void Snap()
        {
            var min = Camera.main.ViewportToWorldPoint(Vector2.zero);
            var max = Camera.main.ViewportToWorldPoint(Vector2.one);
            Vector2 size = max - min;
            transform.localScale = size;
            transform.position = new Vector3(){
                x = Camera.main.transform.position.x,
                y = Camera.main.transform.position.y,
                z = Camera.main.transform.position.z + Camera.main.nearClipPlane + distanceFromCamera
            };
        }
    }
}
