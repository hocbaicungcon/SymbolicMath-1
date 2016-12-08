using System;

namespace Barbar.SymbolicMath.Optimization.Linear
{
    /// <summary>
    /// Exception to be thrown when some counter maximum value is exceeded.
    /// </summary>
    public class MaxCountExceededException : Exception
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="maxium"></param>
        public MaxCountExceededException(int maxium) : base(string.Format("Maximal count exceeded. Maximum is {0}.", maxium))
        {
        }
    }
}
