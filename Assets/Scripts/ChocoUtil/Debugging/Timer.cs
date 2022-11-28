using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace ChocoUtil.Debugging
{
    /// <summary>
    /// Helper data structure that can keep track of a code block's execution time.
    /// <br></br>
    /// When the timer is disposed (after the end of a code block), 
    /// the time spent is logged.
    /// <br></br>
    /// The default logger is <see cref="Debug.Log(object)"/>.
    /// <br></br><br></br>
    /// Example: <code>using(new Timer("Label")) { /* Code block */ }</code>
    /// </summary>
    public struct Timer : IDisposable
    {
        private readonly string label;
        private readonly double startTime;
        private readonly Action<string> logger;
        /// <summary>
        /// Get the current time as double type.
        /// </summary>
        public double currentTimeAsDouble => Time.realtimeSinceStartupAsDouble - startTime;
        /// <summary>
        /// Get the current time as float type.
        /// </summary>
        public float currentTimeAsFloat => (float)currentTimeAsDouble;
        /// <summary>
        /// Timer constructor.
        /// </summary>
        /// <param name="label">The label to be used for the code block being timed.</param>
        /// <param name="logger">The logger function to output the formatted results to.</param>
        public Timer(string label, Action<string> logger){
            this.label = label;
            this.startTime = Time.realtimeSinceStartupAsDouble;
            this.logger = logger;
        }
        /// <summary>
        /// Timer constructor. The default logger (<see cref="Debug.Log(object)"/>) is used.
        /// </summary>
        /// <param name="label">The label to be used for the code block being timed.</param>
        public Timer(string label) : this(label, Debug.Log)
        {
        }
        /// <summary>
        /// Logs the final time to the logger function.
        /// </summary>
        public void Dispose()
        {
            logger($"[Timer] {label} took {currentTimeAsFloat} seconds.");
        }
    }
}
