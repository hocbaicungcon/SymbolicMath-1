using Barbar.SymbolicMath.Policies;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Barbar.SymbolicMath.Vectors
{
    /// <summary>
    /// Represents a vector
    /// </summary>
    /// <typeparam name="T">Vector item type</typeparam>
    /// <typeparam name="TPolicy">Policy for counting vector items</typeparam>
    public class Vector<T,TPolicy> : IEnumerable<T> where TPolicy : IPolicy<T>, new()
    {
        private T[] m_Data;
        private static readonly TPolicy Policy = new TPolicy();

        /// <summary>
        /// Vector size
        /// </summary>
        public int Count
        {
            get { return m_Data.Length; }
        }

        /// <summary>
        /// Vector data (direct reference)
        /// </summary>
        public T[] Data
        {
            get { return m_Data; }
        }

        /// <summary>
        /// Gets or sets item at 0-based index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            get { return m_Data[index]; }
            set { m_Data[index] = value; }
        }

        /// <summary>
        /// ctor, creates new instance of vector based on given size
        /// </summary>
        /// <param name="size"></param>
        public Vector(int size)
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException("size");

            m_Data = new T[size];
        }

        /// <summary>
        /// ctor, creates new instance of vector based on source
        /// </summary>
        /// <param name="source"></param>
        public Vector(Vector<T,TPolicy> source)
        {
            m_Data = new T[source.Count];
            Array.Copy(source.m_Data, m_Data, m_Data.Length);
        }

        /// <summary>
        /// ctor, creates new instance of vector based on given items
        /// </summary>
        /// <param name="source"></param>
        public Vector(T[] source) : this(source, true)
        {
        }

        /// <summary>
        /// ctor, creates new instance of vector based on given items
        /// </summary>
        /// <param name="source"></param>
        /// <param name="copyArray">if true, array is copied, if false vector is directly using the array</param>
        public Vector(T[] source, bool copyArray)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (copyArray)
            {
                m_Data = new T[source.Length];
                Array.Copy(source, m_Data, m_Data.Length);
            }
            else
            {
                m_Data = source;
            }
        }

        /// <summary>
        /// Vector substraction
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector<T, TPolicy> operator -(Vector<T, TPolicy> a, Vector<T, TPolicy> b)
        {
            if (a == null)
            {
                throw new ArgumentNullException("a");
            }
            if (b == null)
            {
                throw new ArgumentNullException("b");
            }
            if (a.Count != b.Count)
            {
                throw new Exception("Different vector sizes.");
            }
            var result = new Vector<T, TPolicy>(a.Count);
            for (var i = 0; i < a.Count; i++)
            {
                result[i] = Policy.Sub(a[i], b[i]);
            }
            return result;
        }

        /// <summary>
        /// Multiply all items of vector by given scalar
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="scalar"></param>
        /// <returns></returns>
        public static Vector<T, TPolicy> operator *(Vector<T, TPolicy> vector, T scalar)
        {
            if (vector == null)
            {
                throw new ArgumentNullException("vector");
            }
            var result = new Vector<T, TPolicy>(vector.Count);
            for (var i = 0; i < vector.Count; i++)
            {
                result[i] = Policy.Mul(vector[i], scalar);
            }
            return result;
        }

        /// <summary>
        /// Calculates dot product
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public T DotProduct(Vector<T, TPolicy> v)
        {
            if (v.Count != Count)
            {
                throw new ArgumentException("Invalid vector size.", "v");
            }
            T dot = Policy.Zero();
            for (int i = 0; i < v.Data.Length; i++)
            {
                dot = Policy.Add(dot, Policy.Mul(m_Data[i], v[i]));
            }
            return dot;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)m_Data).GetEnumerator();
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)m_Data).GetEnumerator();
        }
    }
}