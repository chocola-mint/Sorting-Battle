using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame
{
    /// <summary>
    /// Interface to allow <see cref="RandomScope"></see> to reference an object's
    /// <see cref="Random.State"></see>.
    /// </summary>
    public interface IRandomThread
    {
        public Random.State randomState { get; set; }
    }
    /// <summary>
    /// A utility object that can apply a temporary <see cref="Random.State"></see> scope.
    /// <br></br>
    /// Usage: <code>using(new RandomScope(randomThread)){ ... }</code>
    /// </summary>
    public struct RandomScope : System.IDisposable
    {
        private IRandomThread randomThread;
        private Random.State stateBefore;
        public RandomScope(IRandomThread randomThread)
        {
            this.randomThread = randomThread;
            // Load state.
            stateBefore = Random.state;
            // Apply override.
            Random.state = randomThread.randomState;
        }
        public void Dispose()
        {
            // Save state.
            randomThread.randomState = Random.state;
            // Revert override.
            Random.state = stateBefore;
        }
    }
}
