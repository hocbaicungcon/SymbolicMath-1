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
        /// <param name="rowIndex"></param>
        /// <param name="rowValues"></param>
        public void SetRow(int rowIndex, T[] rowValues)
        {
            if (rowIndex < 0 || rowIndex >= Height)
            {
                throw new ArgumentOutOfRangeException("rowIndex");
            }
            if (rowValues == null)
            {
                throw new ArgumentNullException("rowValues");
            }
            if (rowValues.Length != Width)
            {
                throw new ArgumentException("Array size doesn't match columns count.", "rowValues");
            }
            Array.Copy(rowValues, 0, m_Data, rowIndex * Width, Width);
        }

        /// <summary>
        /// Copy one row from matrix into an array
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="rowValues"></param>
        public void GetRow(int rowIndex, T[] rowValues)
        {
            if (rowIndex < 0 || rowIndex >= Height)
            {
                throw new ArgumentOutOfRangeException("rowIndex");
            }
            if (rowValues == null)
            {
                throw new ArgumentNullException("rowValues");
            }
            if (rowValues.Length != Width)
            {
                throw new ArgumentException("Array size doesn't match columns count.", "rowValues");
            }
            Array.Copy(m_Data, rowIndex * Width, rowValues, 0, Width);
        }

        public T[] GetRow(int rowIndex)
        {
            var result = new T[Width];
            Array.Copy(m_Data, rowIndex * Width, result, 0, Width);
            return result;
        }

        /// <summary>
        /// Copy one column in matrix into an array
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="columnValues"></param>
        public void GetColumn(int columnIndex, T[] columnValues)
        {
            if (columnIndex < 0 || columnIndex >= Width)
            {
                throw new ArgumentOutOfRangeException("columnIndex");
            }
            if (columnValues == null)
            {
                throw new ArgumentNullException("columnValues");
            }
            if (columnValues.Length != Height)
            {
                throw new ArgumentException("Array size doesn't match rows count.", "columnValues");
            }
            int j = 0;
            for(var i = columnIndex; i < m_Data.Length; i+=Width)
            {
                columnValues[j++] = m_Data[i];
            }
        }

        /// <summary>
        /// Set one column in matrix from an array
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="columnValues"></param>
        public void SetColumn(int columnIndex, T[] columnValues)
        {
            if (columnIndex < 0 || columnIndex >= Width)
            {
                throw new ArgumentOutOfRangeException("rowIndex");
            }
            if (columnValues == null)
            {
                throw new ArgumentNullException("columnValues");
            }
            if (columnValues.Length != Width)
            {
                throw new ArgumentException("Array size doesn't match rows count.", "columnValues");
            }
            int j = 0;
            for (var i = columnIndex; i < m_Data.Length; i += Width)
            {
                m_Data[i] = columnValues[j++];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)m_Data).GetEnumerator();
        }
    }
}
