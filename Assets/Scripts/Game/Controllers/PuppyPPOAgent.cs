using System.Collections;
using System.Collections.Generic;
using Unity.Barracuda;
using UnityEngine;
using SortGame.Core;
using System.Linq;
namespace SortGame
{
    public class PuppyPPOAgent : NNController
    {
        private enum ActionType
        {
            Swap, 
            Select
        }
        [SerializeField] 
        private int tickPerClick = 10;
        protected override IEnumerator ExecuteModelOutput(Tensor modelOutput)
        {
            var modelAction = modelOutput.ToReadOnlyArray();
            List<float> actionWeights = new();
            List<(ActionType, List<Vector2Int>)> actionValues = new();
            for(int actionId = 0; actionId < modelAction.Length; ++actionId)
            {
                ActionIdToAction(actionId, out var actionType, out var moveToCoords);
                if(moveToCoords.Any(coord => !gameBoard.state.gameGridState.IsOnGrid(coord) 
                || !gameBoard.state.gameGridState.IsNumber(coord))) 
                    continue;
                // if(actionType == ActionType.Select && moveToCoords.Count < 3) continue;
                if(actionType == ActionType.Select && SimulateSelect(moveToCoords) < 3) continue;
                actionWeights.Add(modelAction[actionId]);
                actionValues.Add((actionType, moveToCoords));
            }
            if(actionWeights.Count == 0) yield break; // Early return if all moves are illegal.
            int maxIndex = actionWeights.ArgMax();
            var (selectedActionType, steps) = actionValues[maxIndex];
            switch(selectedActionType)
            {
                case ActionType.Select:
                    selector.BeginSelection();
                    foreach(var step in steps)
                    {
                        selector.Select(step);
                        yield return WaitForTicks(tickPerClick);
                    }
                    selector.EndSelection();
                    break;
                case ActionType.Swap:
                    swapper.StartSwapping(steps[0]);
                    swapper.SwapTo(steps[1]);
                    swapper.EndSwapping();
                    yield return WaitForTicks(tickPerClick);
                    break;
            }
        }
        private static void ActionIdToAction(int actionId, out ActionType actionType, out List<Vector2Int> moveSequence)
        {
            moveSequence = new();
            int positionId = actionId / 32;
            int moveId = actionId % 32;
            int moveDirection = moveId / 8;
            // Convert moveDirection (0~31 / 8 -> 0~3) to offset vector.
            Vector2Int move = Vector2Int.zero;
            switch (moveDirection)
            {
                case 0:
                    move = new(0, -1);
                    break;
                case 1:
                    move = new(0, 1);
                    break;
                case 2:
                    move = new(-1, 0);
                    break;
                case 3:
                default:
                    move = new(1, 0);
                    break;
            }
            // Convert moveId to moveType.
            int moveType = moveId % 8;
            if (moveType == 0)
            {
                actionType = ActionType.Swap;
                Vector2Int a = new(positionId / 5, positionId % 5);
                Vector2Int b = a + move;
                moveSequence.AddRange(new[] { a, b });
            }
            else
            {
                actionType = ActionType.Select;
                moveSequence.Add(new(positionId / 5, positionId % 5));
                for (int step = 1; step < moveType + 2; ++step)
                    moveSequence.Add(moveSequence[^1] + move);
            }
        }
        protected override void GetInputTensors(in Dictionary<string, Tensor> inputs)
        {
            var allTilesAsFloat = gameBoard.state.GetAllTilesAsFloat(rowMajor: true).ToArray();
            inputs[runtimeModel.inputs[0].name] = new Tensor(
                1, 1, 1, allTilesAsFloat.Length, // n, h, w, c
                allTilesAsFloat); // Tensor content (flattened)
        }
        protected override void NNInit()
        {
            Debug.Assert(runtimeModel.inputs.Count == 1);
        }
    }
}
