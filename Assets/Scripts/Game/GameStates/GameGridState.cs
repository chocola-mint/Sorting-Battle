using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame
{
    /// <summary>
    /// A class that manages a game grid.
    /// </summary>
    public class GameGridState
    {
        public readonly int rowCount, columnCount;
        public readonly GameTileState[,] grid;
        public event System.Action<Vector2Int> onNewBlock;
        private readonly int numberUpperBound = 100;
        public GameGridState(int rowCount, int columnCount, int numberUpperBound = 100)
        {
            this.numberUpperBound = numberUpperBound;
            this.rowCount = rowCount;
            this.columnCount = columnCount;
            this.grid = new GameTileState[rowCount, columnCount];
            for(int i = 0; i < rowCount; ++i)
                for(int j = 0; j < columnCount; ++j) 
                    this.grid[i, j] = new(-1);
        }
        public GameGridState(GameGridState from, int numberUpperBound = 100) 
        : this(from.rowCount, from.columnCount, numberUpperBound)
        {
            InplaceCopy(from);
        }
        public void InplaceCopy(GameGridState from) 
        {
            for(int i = 0; i < rowCount; ++i)
                for(int j = 0; j < columnCount; ++j) 
                    this.grid[i, j] = new(from.grid[i, j].number);
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
        public bool IsTrash(Vector2Int coord) => grid[coord.x, coord.y].IsTrash();
        public bool IsNumber(Vector2Int coord) => grid[coord.x, coord.y].IsNumber();
        public bool IsOnGrid(Vector2Int coord) => coord.x < rowCount && coord.x >= 0 && coord.y < columnCount && coord.y >= 0;
        public void Set(Vector2Int coord, int value) => grid[coord.x, coord.y].number = value;
        public void SetNew(Vector2Int coord, int value)
        {
            Set(coord, value);
            onNewBlock?.Invoke(coord);
        }
        public void Clear()
        {
            for(int i = 0; i < rowCount; ++i)
                for(int j = 0; j < columnCount; ++j) 
                    grid[i, j] = new(-1);
        }
        public void LoadRandom(float rowPercentage = 1.0f)
        {
            for(int i = Mathf.FloorToInt(rowCount * rowPercentage); i < rowCount; ++i)
                for(int j = 0; j < columnCount; ++j)
                    SetNew(new(i, j), Random.Range(0, numberUpperBound));
        }
        public void RegisterBlockCallbacks(Vector2Int coord, System.Action<Vector2Int> onMove, System.Action onRemove)
        {
            grid[coord.x, coord.y].onBlockMove += onMove;
            grid[coord.x, coord.y].onBlockRemove += onRemove;
        }
        public void RegisterTileCallbacks(Vector2Int coord, System.Action onSelect, System.Action onDeselect)
        {
            grid[coord.x, coord.y].onTileSelect += onSelect;
            grid[coord.x, coord.y].onTileDeselect += onDeselect;
        }
        public void Select(Vector2Int coord)
        {
            grid[coord.x, coord.y].Select();
        }
        public void Deselect(Vector2Int coord)
        {
            grid[coord.x, coord.y].Deselect();
        }
        public void Swap(Vector2Int a, Vector2Int b)
        {
            GameTileState.Swap(grid[a.x, a.y], a, grid[b.x, b.y], b);
            // var temp = grid[a.x, a.y];
            // grid[a.x, a.y] = grid[b.x, b.y];
            // grid[b.x, b.y] = temp;
        }
        public void SwapAndPullDown(Vector2Int a, Vector2Int b)
        {
            bool wasAEmpty = grid[a.x, a.y].IsEmpty();
            bool wasBEmpty = grid[b.x, b.y].IsEmpty();
            Swap(a, b);
            if(wasAEmpty ^ wasBEmpty)
            {
                PullDown(a.y);
                PullDown(b.y);
            }
        }
        public void PullDown(int column)
        {
            int bottom;
            for(bottom = rowCount - 1; bottom >= 0 && !grid[bottom, column].IsEmpty(); --bottom);
            int offset;
            for(offset = 0; bottom - offset >= 0 && grid[bottom - offset, column].IsEmpty(); ++offset);

            for(int i = bottom - offset; i >= 0 && !grid[i, column].IsEmpty(); --i)
            {
                (Vector2Int a, Vector2Int b) = (new(i, column), new(i + offset, column));
                Swap(a, b);
            }
        }
        public bool PushUp(int column)
        {
            return PushUp(column, Random.Range(0, numberUpperBound));
        }
        public bool PushUp(int column, int number)
        {
            bool overflow = false;
            int top;
            for(top = 0; top < rowCount && grid[top, column].IsEmpty(); ++top);
            for(int i = top; i < rowCount; ++i)
            {
                (Vector2Int a, Vector2Int b) = (new(i, column), new(i - 1, column));
                if(i != 0) Swap(a, b); // Don't actually swap when i is 0, because row -1 doesn't exist.
                else overflow = true; // Mark overflows.
            }
            grid[rowCount - 1, column].Remove();
            SetNew(new(rowCount - 1, column), number);
            return overflow;
        }
        public void RemoveTile(Vector2Int coord, bool pullDown = true)
        {
            grid[coord.x, coord.y].Remove();
            // Set(coord, GameTileState.Empty);
            if(pullDown) PullDown(coord.y);
        }
        public void RemoveTiles(Vector2Int[] coords, bool pullDown = true)
        {
            foreach(var coord in coords)
                grid[coord.x, coord.y].Remove();
                // Set(coord, GameTileState.Empty);
            if(pullDown)
                foreach(var coord in coords)
                    PullDown(coord.y);
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
