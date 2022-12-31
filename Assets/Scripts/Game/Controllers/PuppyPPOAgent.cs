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
            Select,
            Push,
        }
        [SerializeField] 
        private int tickPerClick = 10;
        [SerializeField]
        private int tickPerPush = 30;
        private List<(ActionType, List<Vector2Int>)> actionTable = null;
        private (ActionType, List<Vector2Int>) previousActionCache = (ActionType.Push, null);
        protected override IEnumerator ExecuteModelOutput(Tensor modelOutput)
        {
            var modelAction = modelOutput.ToReadOnlyArray();

            List<float> actionWeights = new();
            List<(ActionType, List<Vector2Int>)> actionValues = new();
            for(int actionId = 0; actionId < modelAction.Length; ++actionId)
            {
                ActionIdToAction(actionId, out var actionType, out var moveToCoords);
                if(moveToCoords != null && moveToCoords.Any(x => !gameBoard.state.gameGridState.IsNumber(x))) continue;
                if(actionType == ActionType.Select && SimulateSelect(moveToCoords) < 3) continue;
                if(previousActionCache.Item1 == ActionType.Swap 
                && actionType == ActionType.Swap
                && ((moveToCoords[0] == previousActionCache.Item2[1] 
                    && moveToCoords[1] == previousActionCache.Item2[0])
                    || (moveToCoords[0] == previousActionCache.Item2[0] 
                    && moveToCoords[1] == previousActionCache.Item2[1]))) 
                    continue;
                actionWeights.Add(modelAction[actionId]);
                actionValues.Add((actionType, moveToCoords));
            }
            if(actionWeights.Count == 0) 
            {
                yield break; // Early return if all moves are illegal.
            } 
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
                case ActionType.Push:
                    Push();
                    yield return WaitForTicks(tickPerPush);
                    break;
            }
            previousActionCache = (selectedActionType, steps);
        }
        private void ActionIdToAction(int actionId, out ActionType actionType, out List<Vector2Int> moveSequence)
        {
            (actionType, moveSequence) = actionTable[actionId];
        }
        protected override void GetInputTensors(in Dictionary<string, Tensor> inputs)
        {
            var allTilesAsFloat = gameBoard.state.GetAllTilesAsFloat(normalize: true).ToArray();
            inputs[runtimeModel.inputs[0].name] = new Tensor(
                1, 1, 1, allTilesAsFloat.Length, // n, h, w, c
                allTilesAsFloat); // Tensor content (flattened)
        }
        protected override void NNInit()
        {
            Debug.Assert(runtimeModel.inputs.Count == 1);
            // Generate action table. (See: action_table_generator.py)
            actionTable = new();
            int rowCount = gameBoard.state.gameGridState.rowCount;
            int columnCount = gameBoard.state.gameGridState.columnCount;
            Vector2Int[] directions = new[]{ 
                Vector2Int.up, 
                Vector2Int.down, 
                Vector2Int.right, 
                Vector2Int.left 
            };
            for(int i = 0; i < rowCount; ++i)
                for(int j = 0; j < columnCount; ++j)
                    foreach(var dir in directions)
                    {
                        Vector2Int a = new(i, j);
                        Vector2Int b = a + dir;
                        if(gameBoard.state.gameGridState.IsOnGrid(b))
                            actionTable.Add((ActionType.Swap, new(){ a, b }));
                    }
            
            for(int i = 0; i < rowCount; ++i)
                for(int j = 0; j < columnCount; ++j)
                    foreach(var dir in directions)
                        for(int length = 2; length < 9; ++length)
                        {
                            Vector2Int start = new(i, j);
                            Vector2Int end = start + dir * length;
                            if(gameBoard.state.gameGridState.IsOnGrid(end))
                            {
                                List<Vector2Int> coords = new(){ start };
                                for(int k = 0; k < length; ++k)
                                    coords.Add(coords[^1] + dir);
                                actionTable.Add((ActionType.Select, coords));
                            }
                        }
            actionTable.Add((ActionType.Push, null));
        }
    }
}
