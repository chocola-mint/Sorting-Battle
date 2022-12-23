using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SortGame.Core;

namespace SortGame
{
    public static class GameBoardClearExtensions
    {
        public static IEnumerator CoroAnimateClearTilesOneByOne(this GameBoard gameBoard, float waitSecondsBetweenEachRemove = 0.1f)
        {
            GameBoardState state = gameBoard.state;
            var wait = new WaitForSeconds(waitSecondsBetweenEachRemove);
            for(int i = state.gameGridState.rowCount - 1; i >= 0; --i)
                for(int j = 0; j < state.gameGridState.columnCount; ++j)
                {
                    if(state.gameGridState.IsEmpty(new(i, j))) continue;
                    state.gameGridState.RemoveTile(new(i, j), pullDown: false);
                    yield return wait;
                }
        }
        public static IEnumerator CoroAnimateClearTilesRowByRow(this GameBoard gameBoard, float waitSecondsBetweenEachRemove = 0.4f)
        {
            GameBoardState state = gameBoard.state;
            var wait = new WaitForSeconds(waitSecondsBetweenEachRemove);
            for(int i = state.gameGridState.rowCount - 1; i >= 0; --i)
            {
                bool rowEmpty = true;
                for(int j = 0; j < state.gameGridState.columnCount; ++j)
                {
                    bool isEmpty = state.gameGridState.IsEmpty(new(i, j));
                    rowEmpty &= isEmpty;
                    state.gameGridState.RemoveTile(new(i, j), pullDown: false);
                }
                if(rowEmpty) break;
                yield return wait;
            }
        }
        public static IEnumerator CoroAnimateClearTilesPullDown(this GameBoard gameBoard, float waitSecondsBetweenEachRemove = 0.1f)
        {
            GameBoardState state = gameBoard.state;
            var wait = new WaitForSeconds(waitSecondsBetweenEachRemove);
            bool rowEmpty = true;
            int j = -1;
            do
            {
                rowEmpty = true;
                if(j == -1)
                    for(j = 0; j < state.gameGridState.columnCount; ++j)
                    {
                        bool isEmpty = state.gameGridState.IsEmpty(new(state.gameGridState.rowCount - 1, j));
                        rowEmpty &= isEmpty;
                        if(!isEmpty)
                        {
                            state.gameGridState.RemoveTile(new(state.gameGridState.rowCount - 1, j), pullDown: true);
                            yield return wait;
                        }
                    }
                else
                {
                    for(j = state.gameGridState.columnCount - 1; j >= 0; --j)
                    {
                        bool isEmpty = state.gameGridState.IsEmpty(new(state.gameGridState.rowCount - 1, j));
                        rowEmpty &= isEmpty;
                        if(!isEmpty)
                        {
                            state.gameGridState.RemoveTile(new(state.gameGridState.rowCount - 1, j), pullDown: true);
                            yield return wait;
                        }
                    }
                }
            } while (!rowEmpty);
        }

    }
}
