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
        public int level { get; private set; } = 0;
        private readonly SortedList<int, System.Action> scheduler = new(new DuplicateKeyComparer<int>());
        public event System.Action onGameOver;
        protected int tick { get; private set; } = 0;
        public bool isGameOver { get; private set; } = false;
        public void Tick(int increment = 1)
        {
            if(increment <= 0) throw new System.ArgumentException("Increment must be greater than 0.");
            UpdateScheduler(tick + increment);
        }
        public void SkipToNextEvent()
        {
            if(scheduler.Count > 0)
            {
                UpdateScheduler(scheduler.Keys[0]);
            }
        }
        private void UpdateScheduler(int targetTick)
        {
            while(scheduler.Count > 0 && scheduler.Keys[0] <= targetTick)
            {
                tick = scheduler.Keys[0]; // So PushEvent sees the current tick.
                System.Action @event = scheduler.Values[0];
                scheduler.RemoveAt(0);
                @event();
            }
            tick = targetTick;
        }
        /// <summary>
        /// Invoke this method to set up initial events.
        /// <br></br>
        /// Can be overridden to push more events. 
        /// The default implementation pushes the LevelUpEvent 
        /// and should always be invoked via base.InitEvents().
        /// </summary>
        protected virtual void InitEvents()
        {
            // Level up event is pushed here.
            PushEvent(GetTickBetweenEachLevelUp(), LevelUpEvent);
            PushEvent(GetTickBetweenEachNewRow(), PushNewRowEvent);
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
        /// The level up event.
        /// </summary>
        private void LevelUpEvent()
        {
            ++level;
            Debug.Log($"Level: {level}");
            PushEvent(GetTickBetweenEachLevelUp(), LevelUpEvent);
        }
        protected abstract void PushNewRowEvent();
        /// <summary>
        /// Invoke to trigger Game Over event.
        /// This also clears the scheduler.
        /// </summary>
        protected void GameOver() 
        { 
            scheduler.Clear();
            isGameOver = true;
            onGameOver?.Invoke();
        }
        protected void Restart()
        {
            scheduler.Clear();
            isGameOver = false;
            InitEvents();
        }
        /// <summary>
        /// Number of ticks to wait between each new row on each board.
        /// Can be overridden by child classes of GameState.
        /// </summary>
        protected virtual int GetTickBetweenEachNewRow()
            => (int)Mathf.Lerp(150, 60, Ease.InQuad(level / 20.0f));
        /// <summary>
        /// Number of ticks to wait between level-ups.
        /// Can be overridden by child classes of GameState.
        /// </summary>
        protected virtual int GetTickBetweenEachLevelUp()
            => 300;
    }
}