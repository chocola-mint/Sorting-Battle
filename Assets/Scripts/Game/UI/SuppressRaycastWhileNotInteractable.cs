using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
namespace SortGame
{
    [RequireComponent(typeof(Button))]
    public class SuppressRaycastWhileNotInteractable : MonoBehaviour
    {
        private Button button;
        private Graphic[] graphics;
        private void Awake() 
        {
            button = GetComponent<Button>();
            graphics = GetComponentsInChildren<Graphic>().Where(x => x.raycastTarget).ToArray();
        }
        // Update is called once per frame
        void Update()
        {
            foreach(var graphic in graphics) 
                if(graphic.raycastTarget ^ button.interactable)
                    graphic.raycastTarget = button.interactable;
        }
    }
}
