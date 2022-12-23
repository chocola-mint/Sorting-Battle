using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using SortGame.Core;

namespace SortGame
{
    [RequireComponent(typeof(GraphicRaycaster))]
    public abstract class GameGridOperatorBase : MonoBehaviour
    {
        protected GraphicRaycaster raycaster;
        protected GameGrid gameGrid;
        private List<RaycastResult> raycastResults = new();
        protected GameControllerState gameControllerState { get; private set; }
        protected void Awake() 
        {
            raycaster = GetComponent<GraphicRaycaster>();    
            gameGrid = GetComponentInChildren<GameGrid>();
            gameControllerState = GetComponent<GameBoard>().state.gameControllerState;
        }        
        protected IEnumerable<RaycastResult> Raycast(Vector2 screenPosition)
        {
            raycastResults.Clear();
            raycaster.Raycast(
                new PointerEventData(EventSystem.current){ 
                    position = screenPosition,
                }, 
                raycastResults);
            return raycastResults.AsEnumerable();
        }
        protected IEnumerable<T> Raycast<T>(Vector2 screenPosition) where T : MonoBehaviour
        {
            raycastResults.Clear();
            raycaster.Raycast(
                new PointerEventData(EventSystem.current){ 
                    position = screenPosition,
                }, 
                raycastResults);
            foreach(var raycastResult in raycastResults)
                if(TryGetComponentInParent<T>(raycastResult.gameObject, out T component))
                    yield return component;
        }
        private static bool TryGetComponentInParent<T>(GameObject GO, out T component) where T : MonoBehaviour
        {
            component = GO.GetComponentInParent<T>();
            if(component) return true;
            else return false;
        }
        
    }
}
