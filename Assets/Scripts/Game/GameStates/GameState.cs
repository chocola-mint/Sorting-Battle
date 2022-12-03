using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChocoUtil.Algorithms;

namespace SortGame
{
    /// <summary>
    /// Abstract base class for all game modes.
    /// Provides basic functionalities like a tick-based event scheduler.
    /// </summary>
    public abstract class GameState
    {
        // This comparer makes the scheduler tolerate duplicate keys.
        private class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : System.IComparable
        {
            public int Compare(TKey x, TKey y)
            {
                int result = x.CompareTo(y);

                if (result == 0)
                    return 1; // Handle equality as being greater. Note: this will break Remove(key) or
                else          // IndexOfKey(key) since the comparer never returns 0 to signal key equality
                    return result;
            }
        }
        private readonly SortedList<int, System.Action> scheduler = new(new DuplicateKeyComparer<int>());
        public event System.Action onGameOver;
        protected int tick { get; private set; } = 0;
        public void Tick(int increment = 1)
        {
            tick += increment;
            while(scheduler.Count > 0 && scheduler.Keys[0] <= tick)
            {
                System.Action @event = scheduler.Values[0];
                scheduler.RemoveAt(0);
                @event();
            }
        }
        /// <summary>
        /// Push an event to the internal tick-based scheduler.
        /// The event is registered to occur after the given delay passes.
        /// </summary>
        protected void PushEvent(int delayTicks, System.Action @event)
        {
            scheduler.Add(tick + delayTicks, @event);
        }
        /// <summary>
        /// Invoke to trigger Game Over event.
        /// This also clears the scheduler.
        /// </summary>
        protected void GameOver() 
        { 
            scheduler.Clear();
            onGameOver?.Invoke();
        }
    }
}