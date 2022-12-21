using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace SortGame
{
    public class SelectableGroup : MonoBehaviour
    {
        private Selectable[] selectables;
        // Start is called before the first frame update
        void Start()
        {
            selectables = GetComponentsInChildren<Selectable>();
        }
        public void SetInteractable(bool isInteractable)
        {
            foreach(var selectable in selectables)
                selectable.interactable = isInteractable;
        }
    }
}
