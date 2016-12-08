using Barbar.SymbolicMath.Policies;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Barbar.SymbolicMath.Vectors
{
    /// <summary>
    /// Represent matrix
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TPolicy"></typeparam>
    public class Matrix<T, TPolicy> : IEnumerable<T> where TPolicy : IPolicy<T>, new()
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
        public Matrix(Matrix<T, TPolicy> source)
        {
            Width = source.Width;
            Height = source.Height;
            m_Data = new T[Width * Height];
            Array.Copy(source.m_Data, m_Data, m_Data.Length);
        }

        /// <summary>
        /// Create matrix from 2 dimensional array
        /// First dimension is row index, second dimension is column index
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Matrix<T, TPolicy> CreateMatrixRowIndexFirst(T[][] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            int height = data.Length;
            if (height <= 0)
            {
                throw new ArgumentOutOfRangeException("data", "data.Length");
            }

            int width = data[0].Length;
            if (width <= 0)
            {
                throw new ArgumentOutOfRangeException("data", "data[0].Length");
            }
            var result = new Matrix<T, TPolicy>(width, height);
            for (var y = 0; y < height; y++)
            {
                if (data[y].Length != width)
                {
                    throw new ArgumentException(string.Format("Invalid size - data[{0}].Length", y), "data");
                }
                for (var x = 0; x < width; x++)
                {
                    result[x, y] = data[y][x];
                }
            }
            return result;
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

        /// <summary>
        /// Get clone of one row from matrix
        /// </summary>
        /// <param name="rowIndex"></param>
        public T[] GetRow(int rowIndex)
        {
            var result = new T[Width];
            Array.Copy(m_Data, rowIndex * Width, result, 0, Width);
            return result;
        }

        /// <summary>
        /// Get one row from matrix in form of Vector
        /// </summary>
        /// <param name="rowIndex"></param>
        public Vector<T, TPolicy> GetRowVector(int rowIndex)
        {
            return new Vector<T, TPolicy>(GetRow(rowIndex), false);
        }

        /// <summary>
        /// Copy vector to row values of given index
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="vector"></param>
        public void SetRowVector(int rowIndex, Vector<T, TPolicy> vector)
        {
            SetRow(rowIndex, vector.Data);
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

        /// <inheritdoc />
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("<table>");
            for (var y = 0; y < Height; y++)
            {
                builder.Append("<tr>");
                for (var x = 0; x < Width; x++)
                {
                    builder.Append("<td>");
                    builder.Append(this[x, y]);
                    builder.Append("</td>");
                }
                builder.Append("</tr>");
            }
            builder.Append("</table>");
            return builder.ToString();
        }
    }
}
