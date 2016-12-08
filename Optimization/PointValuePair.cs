using System;

namespace Barbar.SymbolicMath.Optimization
{
    /// <summary>
    /// This class holds a point and the value of an objective function at that point.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PointValuePair<T>
    {
        private readonly T _value;
        private readonly T[] _point;

        /// <summary>
        /// Builds a point/objective function value pair.
        /// </summary>
        /// <param name="point">Point coordinates. This instance will store a copy of the array, not the array passed as argument.</param>
        /// <param name="value">Value of the objective function at the point.</param>
        public PointValuePair(T[] point,
                               T value) : this(point, value, true)
        {
        }

        /// <summary>
        /// Builds a point/objective function value pair.
        /// </summary>
        /// <param name="point">Point coordinates.</param>
        /// <param name="value">Value of the objective function at the point.</param>
        /// <param name="copyArray">if true, the input array will be copied, otherwise it will be referenced.</param>
        public PointValuePair(T[] point,
                              T value,
                              bool copyArray)
        {
            _value = value;
            if (copyArray && point != null)
            {
                _point = new T[point.Length];
                Array.Copy(point, _point, point.Length);
            }
            else
            {
                _point = point;
            }
        }

        /// <summary>
        /// Gets the point.
        /// </summary>
        /// <value>
        /// a copy of the stored point.
        /// </value>
        public T[] Point
        {
            get
            {
                if (_point == null)
                {
                    return null;
                }
                var result = new T[_point.Length];
                Array.Copy(_point, result, _point.Length);
                return result;

            }
        }

        /// <summary>
        /// Gets a reference to the point.
        /// </summary>
        /// <value>
        /// a reference to the internal array storing the point.
        /// </value>
        public T[] PointRef { get { return _point; } }

    }
}
