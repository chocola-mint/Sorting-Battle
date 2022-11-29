using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame
{
    public class GameGridState
    {
        public readonly int rowCount, columnCount;
        public readonly GameTileState[,] grid;
        public GameGridState(int rowCount, int columnCount)
        {
            this.rowCount = rowCount;
            this.columnCount = columnCount;
            this.grid = new GameTileState[rowCount, columnCount];
            for(int i = 0; i < rowCount; ++i)
                for(int j = 0; j < columnCount; ++j) 
                    this.grid[i, j] = new(-1);
        }
        public GameGridState(GameGridState from) : this(from.rowCount, from.columnCount)
        {
            InplaceCopy(from);
        }
        public void InplaceCopy(GameGridState from) 
        {
            for(int i = 0; i < rowCount; ++i)
                for(int j = 0; j < columnCount; ++j) 
                    this.grid[i, j] = from.grid[i, j];
        }
        public string Serialize()
        {
            System.Text.StringBuilder stringBuilder = new();
            for(int i = 0; i < rowCount; ++i)
            {
                List<int> row = new();
                for(int j = 0; j < columnCount; ++j) 
                    row.Add(Get(new(i, j)));
                Csv.WriteLine(stringBuilder, row);
            }
            return stringBuilder.ToString();
        }
        public static GameGridState Deserialize(string source)
        {
            int columnCount = 0;
            List<List<int>> mat = new();
            foreach(var line in Csv.GetAllLines(source))
            {
                List<int> row = Csv.ReadLineAsInt(line);
                columnCount = Mathf.Max(columnCount, row.Count);
                mat.Add(row);
            }
            int rowCount = mat.Count;
            GameGridState state = new(rowCount, columnCount);
            for(int i = 0; i < mat.Count; ++i)
                for(int j = 0; j < mat[i].Count; ++j)
                    state.Set(new(i, j), mat[i][j]);
            return state;
        }

        public int Get(Vector2Int coord) => grid[coord.x, coord.y].number;
        public bool IsEmpty(Vector2Int coord) => grid[coord.x, coord.y].IsEmpty();
        public bool IsGarbage(Vector2Int coord) => grid[coord.x, coord.y].IsGarbage();
        public bool IsNumber(Vector2Int coord) => grid[coord.x, coord.y].IsNumber();
        public void Set(Vector2Int coord, int value) => grid[coord.x, coord.y].number = value;
        public void Clear()
        {
            for(int i = 0; i < rowCount; ++i)
                for(int j = 0; j < columnCount; ++j) 
                    grid[i, j] = new(-1);
        }
        public void LoadRandom(int minInclusive = 0, int maxExclusive = 100)
        {
            for(int i = 0; i < rowCount; ++i)
                for(int j = 0; j < columnCount; ++j)
                    Set(new(i, j), Random.Range(minInclusive, maxExclusive));
        }
        public void LoadRow(int rowId, int[] rowValues)
        {
            for(int j = 0; j < columnCount; ++j)
                Set(new(rowId, j), rowValues[j]);
        }
        public void LoadColumn(int columnId, int[] columnValues)
        {
            for(int i = 0; i < rowCount; ++i)
                Set(new(i, columnId), columnValues[i]);
        }
        public struct SwapOp
        {
            public Vector2Int a, b;
        }
        public void Swap(Vector2Int a, Vector2Int b)
        {
            var temp = grid[a.x, a.y];
            grid[a.x, a.y] = grid[b.x, b.y];
            grid[b.x, b.y] = temp;
        }
        public List<SwapOp> SwapAndPullDown(Vector2Int a, Vector2Int b)
        {
            bool wasAEmpty = grid[a.x, a.y].IsEmpty();
            bool wasBEmpty = grid[b.x, b.y].IsEmpty();
            Swap(a, b);
            List<SwapOp> result = new();
            if(wasAEmpty ^ wasBEmpty) 
            {
                result.AddRange(PullDown(a.y));
                result.AddRange(PullDown(b.y));
            }
            return result;
        }
        public List<SwapOp> PullDown(int column)
        {
            List<SwapOp> swaps = new();
            int bottom;
            for(bottom = rowCount - 1; bottom >= 0 && !grid[bottom, column].IsEmpty(); --bottom);
            int offset;
            for(offset = 0; bottom - offset >= 0 && grid[bottom - offset, column].IsEmpty(); ++offset);

            for(int i = bottom - offset; i >= 0 && !grid[i, column].IsEmpty(); --i)
            {
                (Vector2Int a, Vector2Int b) = (new(i, column), new(i + offset, column));
                Swap(a, b);
                swaps.Add(new(){a = a, b = b});
            }
            return swaps;
        }
        public List<SwapOp> PushUp(int column, int number)
        {
            List<SwapOp> swaps = new();
            int top;
            for(top = 0; top < rowCount && grid[top, column].IsEmpty(); ++top);
            for(int i = top; i < rowCount; ++i)
            {
                (Vector2Int a, Vector2Int b) = (new(i, column), new(i - 1, column));
                if(i != 0) Swap(a, b); // Don't actually swap when i is 0, because row -1 doesn't exist.
                swaps.Add(new(){a = a, b = b});
            }
            Set(new(rowCount - 1, column), number);
            return swaps;
        }
        public List<SwapOp> RemoveTile(Vector2Int coord)
        {
            Set(coord, GameTileState.Empty);
            return PullDown(coord.y);
        }
        public List<SwapOp> RemoveTiles(Vector2Int[] coords)
        {
            List<SwapOp> swaps = new();
            foreach(var coord in coords)
                Set(coord, GameTileState.Empty);
            foreach(var coord in coords)
                swaps.AddRange(PullDown(coord.y));
            return swaps;
        }
        public bool ContentEqual(GameGridState other)
        {
            if(rowCount != other.rowCount || columnCount != other.columnCount) return false;
            
            for(int i = 0; i < rowCount; ++i)
                for(int j = 0; j < columnCount; ++j)
                    if(Get(new(i, j)) != other.Get(new(i, j))) 
                        return false;
            return true;
        }
    }
}
