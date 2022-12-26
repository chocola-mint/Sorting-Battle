using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SortGame.Core;

namespace SortGame
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class DangerIndicator : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        [SerializeField] private GameBoard gameBoard;
        [SerializeField, Min(0)] private float warnStartPercentage = 0.7f;
        [SerializeField] Gradient gradient;
        [SerializeField, Range(0, 1)] float maxAlpha = 0.8f;
        private void Awake() 
        {
            spriteRenderer = GetComponent<SpriteRenderer>();    
            spriteRenderer.color = Color.clear;
        }
        // Update is called once per frame
        void Update()
        {
            bool gameBoardActive = gameBoard.state.status == GameBoardState.Status.Active;
            float fillPercentage = (float)gameBoard.state.GetBoardHeight() / gameBoard.state.gameGridState.rowCount;
            Color color = gameBoardActive ? 
                gradient.Evaluate(Mathf.Lerp(0, 1, fillPercentage - warnStartPercentage))
                : Color.clear;
            if(fillPercentage >= warnStartPercentage && gameBoardActive)
                color.a = maxAlpha;
            else
                color.a = 0;
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, color, Time.deltaTime * 4);
        }
    }
}
