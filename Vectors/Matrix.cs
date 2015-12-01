using System;
using System.Collections;
using System.Collections.Generic;

namespace Barbar.SymbolicMath.Vectors
{
    /// <summary>
    /// Represent matrix
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Matrix<T> : IEnumerable<T>
    {
        private T[] m_Data;

        /// <summary>
        /// Width of matrix
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Height of matrix
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Gets or sets element in matrix
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public T this[int x, int y]
        {
            get { return m_Data[x + y * Width]; }
            set { m_Data[x + y * Width] = value; }
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Matrix(int width, int height)
        {
            if (width <= 0)
                throw new ArgumentOutOfRangeException("width");
            if (height <= 0)
                throw new ArgumentOutOfRangeException("height");

            Width = width;
            Height = height;
            m_Data = new T[Width * Height];
        }

        /// <summary>
        /// ctor, creates new instance of matrix based on source
        /// </summary>
        /// <param name="source"></param>
        public Matrix(Matrix<T> source)
        {
            Width = source.Width;
            Height = source.Height;
            m_Data = new T[Width * Height];
            Array.Copy(source.m_Data, m_Data, m_Data.Length);
        }

        /// <summary>
        /// Enumerates all items in matrix
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)m_Data).GetEnumerator();
        }

        /// <summary>
        /// Set one row in matrix from an array
        /// </summary>
        /// <param name="row"></param>
        /// <param name="parts"></param>
        public void SetRow(int row, T[] parts)
        {
            if (row < 0 || row >= Height)
            {
                throw new ArgumentOutOfRangeException("row");
            }
            if (parts == null)
            {
                throw new ArgumentNullException("parts");
            }
            if (parts.Length != Width)
            {
                throw new ArgumentException("Array size doesn't match columns count.", "parts");
            }
            Array.Copy(parts, 0, m_Data, row * Width, Width);
        }

        /// <summary>
        /// Copy one row from matrix into an array
        /// </summary>
        /// <param name="row"></param>
        /// <param name="parts"></param>
        public void GetRow(int row, T[] parts)
        {
            if (row < 0 || row >= Height)
            {
                throw new ArgumentOutOfRangeException("row");
            }
            if (parts == null)
            {
                throw new ArgumentNullException("parts");
            }
            if (parts.Length != Width)
            {
                throw new ArgumentException("Array size doesn't match columns count.", "parts");
            }
            Array.Copy(m_Data, row * Width, parts, 0, Width);
        }

        /// <summary>
        /// Copy one column in matrix into an array
        /// </summary>
        /// <param name="column"></param>
        /// <param name="parts"></param>
        public void GetColumn(int column, T[] parts)
        {
            if (column < 0 || column >= Height)
            {
                throw new ArgumentOutOfRangeException("row");
            }
            if (parts == null)
            {
                throw new ArgumentNullException("parts");
            }
            if (parts.Length != Height)
            {
                throw new ArgumentException("Array size doesn't match rows count.", "parts");
            }
            int j = 0;
            for(var i = column; i < m_Data.Length; i+=Width)
            {
                parts[j++] = m_Data[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)m_Data).GetEnumerator();
        }
    }
}
