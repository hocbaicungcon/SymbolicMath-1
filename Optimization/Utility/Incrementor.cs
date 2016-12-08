using Barbar.SymbolicMath.Optimization.Linear;
using System;

namespace Barbar.SymbolicMath.Optimization.Utility
{
    /// <summary>
    /// Incrementor
    /// </summary>
    public class Incrementor
    {
        /// <summary>
        /// Function called at counter exhaustion.
        /// </summary>
        private readonly Action<int> maxCountCallback;

        /// <summary>
        /// ctor
        /// </summary>
        public Incrementor() : this(0)
        {
        }

        /// <summary>
        /// Defines a maximal count.
        /// </summary>
        /// <param name="max"></param>
        public Incrementor(int max) : this(max, (maxValue) =>
        {
            throw new MaxCountExceededException(maxValue);
        })
        {

        }

        /// <summary>
        /// Defines a maximal count and a callback method to be triggered at counter exhaustion.
        /// </summary>
        /// <param name="max"></param>
        /// <param name="cb"></param>
        public Incrementor(int max, Action<int> cb)
        {
            if (cb == null)
            {
                throw new ArgumentNullException();
            }
            MaximalCount = max;
            maxCountCallback = cb;
        }

        /// <summary>
        /// Gets the upper limit of the counter.
        /// </summary>
        public int MaximalCount { get; set; }

        /// <summary>
        /// Gets the current count.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Checks whether a single increment is allowed.
        /// </summary>
        public bool CanIncrement
        {
            get { return Count < MaximalCount; }
        }

        /// <summary>
        /// Performs multiple increments.
        /// </summary>
        /// <param name="value"></param>
        public void IncrementCount(int value)
        {
            for (int i = 0; i < value; i++)
            {
                IncrementCount();
            }
        }

        /// <summary>
        /// Adds one to the current iteration count.
        /// </summary>
        public void IncrementCount()
        {
            if (++Count > MaximalCount)
            {
                maxCountCallback(MaximalCount);
            }
        }

        /// <summary>
        /// Resets the counter to 0.
        /// </summary>
        public void ResetCount()
        {
            Count = 0;
        }
    }
}
