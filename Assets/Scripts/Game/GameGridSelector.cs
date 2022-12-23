using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

namespace SortGame
{
    public interface IOnSelectReceiver
    {
        void OnSelect();
    }
    public interface IOnDeselectReceiver
    {
        void OnDeselect();
    }
    public class GameGridSelector : GameGridOperatorBase
    {
        private int selectionCount = 0;
        public event System.Action<int> selectEvent;
        public event System.Action<int> selectFinishEvent;
        public void Select(Vector2 screenPosition)
        {
            if(!enabled) return;
            foreach(var numberBlock in Raycast<NumberBlock>(screenPosition))
            {
                Select(numberBlock.gameTile.gridCoord);
                return;
            }
        }
        public void Select(Vector2Int tileCoord)
        {
            if(!enabled) return;
            if(gameControllerState.Select(tileCoord))
                selectEvent?.Invoke(++selectionCount);
        }
        public void BeginSelection()
        {
            selectionCount = 0;
            gameControllerState.BeginSelection();
        }
        public void EndSelection()
        {
            selectionCount = 0;
            gameControllerState.EndSelection();
        }
        private void OnDisable() 
        {
            EndSelection();
        }
        private void OnDestroy() 
        {
            gameControllerState.onBeginSelection -= Enable;
            gameControllerState.onEndSelection -= Disable;
            gameControllerState.onRemove -= selectFinishEvent;
        }
        private void Enable() => enabled = true;
        private void Disable() => enabled = false;
        // Start is called before the first frame update
        void Start()
        {
            // Make "enabled" reflect selection state.
            gameControllerState.onBeginSelection += Enable;
            gameControllerState.onEndSelection += Disable;
            gameControllerState.onRemove += selectFinishEvent;
            enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
