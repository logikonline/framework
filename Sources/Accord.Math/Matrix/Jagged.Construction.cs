﻿// Accord Math Library
// The Accord.NET Framework
// http://accord-framework.net
//
// Copyright © César Souza, 2009-2015
// cesarsouza at gmail.com
//
//    This library is free software; you can redistribute it and/or
//    modify it under the terms of the GNU Lesser General Public
//    License as published by the Free Software Foundation; either
//    version 2.1 of the License, or (at your option) any later version.
//
//    This library is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//    Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public
//    License along with this library; if not, write to the Free Software
//    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
//

namespace Accord.Math
{
    using Accord.Math.Comparers;
    using Accord.Math.Random;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    /// <summary>
    ///   Jagged matrices.
    /// </summary>
    /// 
    /// <seealso cref="Matrix"/>
    /// <seealso cref="Vector"/>
    /// 
    public static partial class Jagged
    {
        /// <summary>
        ///   Creates a zero-valued matrix.
        /// </summary>
        /// 
        /// <typeparam name="T">The type of the matrix to be created.</typeparam>
        /// <param name="rows">The number of rows in the matrix.</param>
        /// <param name="columns">The number of columns in the matrix.</param>
        /// 
        /// <returns>A matrix of the specified size.</returns>
        /// 
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T[][] Zeros<T>(int rows, int columns)
        {
            T[][] matrix = new T[rows][];
            for (int i = 0; i < matrix.Length; i++)
                matrix[i] = new T[columns];
            return matrix;
        }

        /// <summary>
        ///   Creates a zero-valued matrix.
        /// </summary>
        /// 
        /// <typeparam name="T">The type of the matrix to be created.</typeparam>
        /// <param name="rows">The number of rows in the matrix.</param>
        /// <param name="columns">The number of columns in the matrix.</param>
        /// 
        /// <returns>A matrix of the specified size.</returns>
        /// 
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T[][] Ones<T>(int rows, int columns)
            where T : struct
        {
            var one = (T)System.Convert.ChangeType(1, typeof(T));
            return Create<T>(rows, columns, one);
        }

        /// <summary>
        ///   Creates a zero-valued matrix.
        /// </summary>
        /// 
        /// <param name="rows">The number of rows in the matrix.</param>
        /// <param name="columns">The number of columns in the matrix.</param>
        /// 
        /// <returns>A vector of the specified size.</returns>
        /// 
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static double[][] Zeros(int rows, int columns)
        {
            return Zeros<double>(rows, columns);
        }

        /// <summary>
        ///   Creates a zero-valued matrix.
        /// </summary>
        /// 
        /// <param name="rows">The number of rows in the matrix.</param>
        /// <param name="columns">The number of columns in the matrix.</param>
        /// 
        /// <returns>A vector of the specified size.</returns>
        /// 
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static double[][] Ones(int rows, int columns)
        {
            return Ones<double>(rows, columns);
        }


        /// <summary>
        ///   Creates a jagged matrix with all values set to a given value.
        /// </summary>
        /// 
        /// <param name="rows">The number of rows in the matrix.</param>
        /// <param name="columns">The number of columns in the matrix.</param>
        /// <param name="value">The initial values for the vector.</param>
        /// 
        /// <returns>A matrix of the specified size.</returns>
        /// 
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T[][] Create<T>(int rows, int columns, T value)
        {
            var matrix = new T[rows][];
            for (int i = 0; i < rows; i++)
            {
                var row = matrix[i] = new T[columns];
                for (int j = 0; j < row.Length; j++)
                    row[j] = value;
            }

            return matrix;
        }

        /// <summary>
        ///   Creates a jagged matrix with all values set to a given value.
        /// </summary>
        /// 
        /// <param name="size">The number of rows and columns in the matrix.</param>
        /// <param name="value">The initial values for the matrix.</param>
        /// 
        /// <returns>A matrix of the specified size.</returns>
        /// 
        /// <seealso cref="Matrix.Create{T}(int, int, T)"/>
        /// 
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T[][] Square<T>(int size, T value)
        {
            return Create(size, size, value);
        }

        /// <summary>
        ///   Creates a matrix with all values set to a given value.
        /// </summary>
        /// 
        /// <param name="rows">The number of rows in the matrix.</param>
        /// <param name="columns">The number of columns in the matrix.</param>
        /// <param name="values">The initial values for the matrix.</param>
        /// 
        /// <returns>A matrix of the specified size.</returns>
        /// 
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T[][] Create<T>(int rows, int columns, params T[] values)
        {
            if (values.Length == 0)
                return Zeros<T>(rows, columns);
            return values.Reshape(rows, columns).ToJagged();
        }

        /// <summary>
        ///   Creates a matrix with the given rows.
        /// </summary>
        /// 
        /// <param name="rows">The row vectors in the matrix.</param>
        /// 
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T[][] Create<T>(params T[][] rows)
        {
            return rows;
        }

        /// <summary>
        ///   Creates a matrix with the given values.
        /// </summary>
        /// 
        /// <param name="values">The values in the matrix.</param>
        /// 
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T[][] Create<T>(T[,] values)
        {
            return values.ToJagged();
        }

        /// <summary>
        ///   Creates a matrix of one-hot vectors, where all values at each row are 
        ///   zero except for the ones in the positions where <paramref name="mask"/>
        ///   are true, which are set to one.
        /// </summary>
        /// 
        /// <typeparam name="T">The data type for the matrix.</typeparam>
        /// 
        /// <param name="mask">The boolean mask determining where ones will be placed.</param>
        /// 
        /// <returns>A matrix containing one-hot vectors where only a single position
        ///   is one and the others are zero.</returns>
        /// 
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T[][] OneHot<T>(bool[] mask)
        {
            return OneHot<T>(mask, Jagged.Create<T>(mask.Length, 2));
        }

        /// <summary>
        ///   Creates a matrix of one-hot vectors, where all values at each row are 
        ///   zero except for the indicated <paramref name="indices"/>, which is set to one.
        /// </summary>
        /// 
        /// <typeparam name="T">The data type for the matrix.</typeparam>
        /// 
        /// <param name="indices">The rows's dimension which will be marked as one.</param>
        /// 
        /// <returns>A matrix containing one-hot vectors where only a single position
        /// is one and the others are zero.</returns>
        /// 
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T[][] OneHot<T>(int[] indices)
        {
            return OneHot<T>(indices, indices.DistinctCount());
        }

        /// <summary>
        ///   Creates a matrix of one-hot vectors, where all values at each row are 
        ///   zero except for the indicated <paramref name="indices"/>, which is set to one.
        /// </summary>
        /// 
        /// <param name="indices">The rows's dimension which will be marked as one.</param>
        /// 
        /// <returns>A matrix containing one-hot vectors where only a single position
        /// is one and the others are zero.</returns>
        /// 
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static double[][] OneHot(int[] indices)
        {
            return OneHot(indices, indices.DistinctCount());
        }

        /// <summary>
        ///   Creates a matrix of one-hot vectors, where all values at each row are 
        ///   zero except for the indicated <paramref name="indices"/>, which is set to one.
        /// </summary>
        /// 
        /// <typeparam name="T">The data type for the matrix.</typeparam>
        /// 
        /// <param name="indices">The rows's dimension which will be marked as one.</param>
        /// <param name="columns">The size (length) of the vectors (columns of the matrix).</param>
        /// 
        /// <returns>A matrix containing one-hot vectors where only a single position
        /// is one and the others are zero.</returns>
        /// 
        public static T[][] OneHot<T>(int[] indices, int columns)
        {
            return OneHot<T>(indices, Jagged.Create<T>(indices.Length, columns));
        }

        /// <summary>
        ///   Creates a matrix of one-hot vectors, where all values at each row are 
        ///   zero except for the indicated <paramref name="indices"/>, which is set to one.
        /// </summary>
        /// 
        /// <param name="indices">The rows's dimension which will be marked as one.</param>
        /// <param name="columns">The size (length) of the vectors (columns of the matrix).</param>
        /// 
        /// <returns>A matrix containing one-hot vectors where only a single position
        /// is one and the others are zero.</returns>
        /// 
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static double[][] OneHot(int[] indices, int columns)
        {
            return OneHot(indices, Jagged.Create<double>(indices.Length, columns));
        }

        /// <summary>
        ///   Creates a matrix of one-hot vectors, where all values at each row are 
        ///   zero except for the ones in the positions where <paramref name="mask"/>
        ///   are true, which are set to one.
        /// </summary>
        /// 
        /// <typeparam name="T">The data type for the matrix.</typeparam>
        /// 
        /// <param name="mask">The boolean mask determining where ones will be placed.</param>
        /// <param name="result">The matrix where the one-hot should be marked.</param>
        /// 
        /// <returns>A matrix containing one-hot vectors where only a single position
        ///   is one and the others are zero.</returns>
        /// 
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T[][] OneHot<T>(bool[] mask, T[][] result)
        {
            var one = (T)System.Convert.ChangeType(1, typeof(T));
            for (int i = 0; i < mask.Length; i++)
                if (mask[i])
                    result[i][0] = one;
                else
                    result[i][1] = one;
            return result;
        }

        /// <summary>
        ///   Creates a matrix of one-hot vectors, where all values at each row are 
        ///   zero except for the indicated <paramref name="indices"/>, which is set to one.
        /// </summary>
        /// 
        /// <typeparam name="T">The data type for the matrix.</typeparam>
        /// 
        /// <param name="indices">The rows's dimension which will be marked as one.</param>
        /// <param name="result">The matrix where the one-hot should be marked.</param>
        /// 
        /// <returns>A matrix containing one-hot vectors where only a single position
        /// is one and the others are zero.</returns>
        /// 
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T[][] OneHot<T>(int[] indices, T[][] result)
        {
            var one = (T)System.Convert.ChangeType(1, typeof(T));
            for (int i = 0; i < indices.Length; i++)
                result[i][indices[i]] = one;
            return result;
        }

        /// <summary>
        ///   Creates a matrix of one-hot vectors, where all values at each row are 
        ///   zero except for the indicated <paramref name="indices"/>, which is set to one.
        /// </summary>
        /// 
        /// <param name="indices">The rows's dimension which will be marked as one.</param>
        /// <param name="result">The matrix where the one-hot should be marked.</param>
        /// 
        /// <returns>A matrix containing one-hot vectors where only a single position
        /// is one and the others are zero.</returns>
        /// 
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static double[][] OneHot(int[] indices, double[][] result)
        {
            for (int i = 0; i < indices.Length; i++)
                result[i][indices[i]] = 1;
            return result;
        }


        /// <summary>
        ///   Creates a matrix of k-hot vectors, where all values at each row are 
        ///   zero except for the ones in the positions where <paramref name="mask"/>
        ///   are true, which are set to one.
        /// </summary>
        /// 
        /// <typeparam name="T">The data type for the matrix.</typeparam>
        /// 
        /// <param name="mask">The boolean mask determining where ones will be placed.</param>
        /// 
        /// <returns>A matrix containing one-hot vectors where only a single position
        ///   is one and the others are zero.</returns>
        /// 
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T[][] KHot<T>(bool[][] mask)
        {
            return KHot<T>(mask, Jagged.CreateAs<bool, T>(mask));
        }

        /// <summary>
        ///   Creates a matrix of k-hot vectors, where all values at each row are 
        ///   zero except for the indicated <paramref name="indices"/>, which are set to one.
        /// </summary>
        /// 
        /// <typeparam name="T">The data type for the matrix.</typeparam>
        /// 
        /// <param name="indices">The rows's dimension which will be marked as one.</param>
        /// <param name="columns">The size (length) of the vectors (columns of the matrix).</param>
        /// 
        /// <returns>A matrix containing k-hot vectors where only elements at the indicated 
        ///   <paramref name="indices"/> are set to one and the others are zero.</returns>
        /// 
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T[][] KHot<T>(int[][] indices, int columns)
        {
            return KHot<T>(indices, Jagged.Create<T>(indices.Length, columns));
        }

        /// <summary>
        ///   Creates a matrix of k-hot vectors, where all values at each row are 
        ///   zero except for the indicated <paramref name="indices"/>, which are set to one.
        /// </summary>
        /// 
        /// <param name="indices">The rows's dimension which will be marked as one.</param>
        /// <param name="columns">The size (length) of the vectors (columns of the matrix).</param>
        /// 
        /// <returns>A matrix containing k-hot vectors where only elements at the indicated 
        ///   <paramref name="indices"/> are set to one and the others are zero.</returns>
        /// 
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static double[][] KHot(int[][] indices, int columns)
        {
            return KHot(indices, Jagged.Create<double>(indices.Length, columns));
        }

        /// <summary>
        ///   Creates a matrix of k-hot vectors, where all values at each row are 
        ///   zero except for the ones in the positions where <paramref name="mask"/>
        ///   are true, which are set to one.
        /// </summary>
        /// 
        /// <param name="mask">The boolean mask determining where ones will be placed.</param>
        /// <param name="columns">The size (length) of the vectors (columns of the matrix).</param>
        /// 
        /// <returns>A matrix containing one-hot vectors where only a single position
        ///   is one and the others are zero.</returns>
        /// 
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static double[][] KHot(bool[][] mask, int columns)
        {
            return KHot(mask, Jagged.Create<double>(mask.Length, columns));
        }

        /// <summary>
        ///   Creates a matrix of one-hot vectors, where all values at each row are 
        ///   zero except for the ones in the positions where <paramref name="mask"/>
        ///   are true, which are set to one.
        /// </summary>
        /// 
        /// <typeparam name="T">The data type for the matrix.</typeparam>
        /// 
        /// <param name="mask">The boolean mask determining where ones will be placed.</param>
        /// <param name="result">The matrix where the one-hot should be marked.</param>
        /// 
        /// <returns>A matrix containing one-hot vectors where only a single position
        ///   is one and the others are zero.</returns>
        /// 
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T[][] KHot<T>(bool[][] mask, T[][] result)
        {
            var one = (T)System.Convert.ChangeType(1, typeof(T));
            for (int i = 0; i < mask.Length; i++)
                for (int j = 0; j < mask[0].Length; j++)
                    if (mask[i][j])
                        result[i][j] = one;
            return result;
        }

        /// <summary>
        ///   Creates a matrix of k-hot vectors, where all values at each row are 
        ///   zero except for the indicated <paramref name="indices"/>, which are set to one.
        /// </summary>
        /// 
        /// <typeparam name="T">The data type for the matrix.</typeparam>
        /// 
        /// <param name="indices">The rows's dimension which will be marked as one.</param>
        /// <param name="result">The matrix where the one-hot should be marked.</param>
        /// 
        /// <returns>A matrix containing k-hot vectors where only elements at the indicated 
        ///   <paramref name="indices"/> are set to one and the others are zero.</returns>
        /// 
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T[][] KHot<T>(int[][] indices, T[][] result)
        {
            var one = (T)System.Convert.ChangeType(1, typeof(T));
            for (int i = 0; i < indices.Length; i++)
                for (int j = 0; j < indices[0].Length; j++)
                    result[i][indices[i][j]] = one;
            return result;
        }

        /// <summary>
        ///   Creates a matrix of k-hot vectors, where all values at each row are 
        ///   zero except for the indicated <paramref name="indices"/>, which are set to one.
        /// </summary>
        /// 
        /// <param name="indices">The rows's dimension which will be marked as one.</param>
        /// <param name="result">The matrix where the one-hot should be marked.</param>
        /// 
        /// <returns>A matrix containing k-hot vectors where only elements at the indicated 
        ///   <paramref name="indices"/> are set to one and the others are zero.</returns>
        /// 
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static double[][] KHot(int[][] indices, double[][] result)
        {
            for (int i = 0; i < indices.Length; i++)
                for (int j = 0; j < indices[i].Length; j++)
                    result[i][indices[i][j]] = 1;
            return result;
        }




        /// <summary>
        ///   Creates a new multidimensional matrix with the same shape as another matrix.
        /// </summary>
        /// 
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T[][] CreateAs<T>(T[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            T[][] r = new T[rows][];
            for (int i = 0; i < r.Length; i++)
                r[i] = new T[cols];
            return r;
        }

        /// <summary>
        ///   Returns a new multidimensional matrix.
        /// </summary>
        /// 
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T[][] CreateAs<T>(T[][] matrix)
        {
            T[][] r = new T[matrix.Length][];
            for (int i = 0; i < r.Length; i++)
                r[i] = new T[matrix[i].Length];
            return r;
        }

        /// <summary>
        ///   Creates a 1xN matrix with a single row vector of size N.
        /// </summary>
        /// 
        public static T[][] RowVector<T>(params T[] values)
        {
            return new T[][] { values };
        }

        /// <summary>
        ///   Creates a Nx1 matrix with a single column vector of size N.
        /// </summary>
        /// 
        public static T[][] ColumnVector<T>(params T[] values)
        {
            T[][] column = new T[values.Length][];
            for (int i = 0; i < column.Length; i++)
                column[i] = new[] { values[i] };

            return column;
        }

        /// <summary>
        ///   Creates a square matrix with ones across its diagonal.
        /// </summary>
        /// 
        public static double[][] Identity(int size)
        {
            return Diagonal(size, 1.0);
        }

        /// <summary>
        ///   Creates a jagged magic square matrix.
        /// </summary>
        /// 
        public static double[][] Magic(int size)
        {
            return Matrix.Magic(size).ToJagged();
        }

        #region Diagonal matrices
        /// <summary>
        ///   Returns a square diagonal matrix of the given size.
        /// </summary>
        /// 
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T[][] Diagonal<T>(int size, T value)
        {
            return Diagonal(size, value, Jagged.Create<T>(size, size));
        }

        /// <summary>
        ///   Returns a square diagonal matrix of the given size.
        /// </summary>
        /// 
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T[][] Diagonal<T>(int size, T value, T[][] result)
        {
            for (int i = 0; i < size; i++)
                result[i][i] = value;
            return result;
        }

        /// <summary>
        ///   Returns a matrix of the given size with value on its diagonal.
        /// </summary>
        /// 
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T[][] Diagonal<T>(int rows, int cols, T value)
        {
            return Diagonal(rows, cols, value, Jagged.Create<T>(rows, cols));
        }

        /// <summary>
        ///   Returns a matrix of the given size with value on its diagonal.
        /// </summary>
        /// 
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T[][] Diagonal<T>(int rows, int cols, T value, T[][] result)
        {
            int min = Math.Min(rows, cols);
            for (int i = 0; i < min; i++)
                result[i][i] = value;
            return result;
        }

        /// <summary>
        ///   Return a square matrix with a vector of values on its diagonal.
        /// </summary>
        /// 
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T[][] Diagonal<T>(T[] values)
        {
            return Diagonal(values, Jagged.Create<T>(values.Length, values.Length));
        }

        /// <summary>
        ///   Return a square matrix with a vector of values on its diagonal.
        /// </summary>
        /// 
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T[][] Diagonal<T>(T[] values, T[][] result)
        {
            for (int i = 0; i < values.Length; i++)
                result[i][i] = values[i];
            return result;
        }

        /// <summary>
        ///   Return a square matrix with a vector of values on its diagonal.
        /// </summary>
        /// 
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T[][] Diagonal<T>(int size, T[] values)
        {
            return Diagonal(size, size, values);
        }

        /// <summary>
        ///   Return a square matrix with a vector of values on its diagonal.
        /// </summary>
        /// 
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T[][] Diagonal<T>(int size, T[] values, T[][] result)
        {
            return Diagonal(size, size, values, result);
        }

        /// <summary>
        ///   Returns a matrix with a vector of values on its diagonal.
        /// </summary>
        /// 
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T[][] Diagonal<T>(int rows, int cols, T[] values)
        {
            return Diagonal(rows, cols, values, Jagged.Create<T>(rows, cols));
        }

        /// <summary>
        ///   Returns a matrix with a vector of values on its diagonal.
        /// </summary>
        /// 
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T[][] Diagonal<T>(int rows, int cols, T[] values, T[][] result)
        {
            int size = Math.Min(rows, Math.Min(cols, values.Length));
            for (int i = 0; i < size; i++)
                result[i][i] = values[i];
            return result;
        }
        #endregion


        /// <summary>
        ///   Returns a new multidimensional matrix.
        /// </summary>
        /// 
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TOutput[][] CreateAs<TInput, TOutput>(TInput[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            var r = new TOutput[rows][];
            for (int i = 0; i < r.Length; i++)
                r[i] = new TOutput[cols];
            return r;
        }

        /// <summary>
        ///   Returns a new multidimensional matrix.
        /// </summary>
        /// 
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TOutput[][] CreateAs<TInput, TOutput>(TInput[][] matrix)
        {
            var r = new TOutput[matrix.Length][];
            for (int i = 0; i < r.Length; i++)
                r[i] = new TOutput[matrix[i].Length];
            return r;
        }


        /// <summary>
        ///   Transforms a vector into a matrix of given dimensions.
        /// </summary>
        /// 
        public static T[][] Reshape<T>(T[] array, int rows, int cols, MatrixOrder order = MatrixOrder.Default)
        {
            return Jagged.Reshape(array, rows, cols, Jagged.Create<T>(rows, cols), order);
        }

        /// <summary>
        ///   Transforms a vector into a matrix of given dimensions.
        /// </summary>
        /// 
        public static T[][] Reshape<T>(this T[] array, int rows, int cols, T[][] result, MatrixOrder order = MatrixOrder.Default)
        {
            if (order == MatrixOrder.CRowMajor)
            {
                int k = 0;
                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < cols; j++)
                        result[i][j] = array[k++];
            }
            else
            {
                int k = 0;
                for (int j = 0; j < cols; j++)
                    for (int i = 0; i < rows; i++)
                        result[i][j] = array[k++];
            }

            return result;
        }

        /// <summary>
        ///   Creates a vector containing every index that can be used to
        ///   address a given jagged <paramref name="array"/>, in order. 
        /// </summary>
        /// 
        /// <param name="array">The array whose indices will be returned.</param>
        /// 
        /// <seealso cref="Matrix.GetIndices"/>
        /// 
        public static Tuple<int, int>[] GetIndices<T>(this T[][] array)
        {
            var list = new List<Tuple<int, int>>();
            for (int i = 0; i < array.Length; i++)
                for (int j = 0; j < array[i].Length; j++)
                    list.Add(Tuple.Create(i, j));
            return list.ToArray();
        }



        #region Random matrices
        /// <summary>
        ///   Creates a square matrix matrix with random data.
        /// </summary>
        /// 
        public static T[][] Random<T>(int size, IRandomNumberGenerator<T> generator,
            bool symmetric = false, T[][] result = null)
        {
            if (result == null)
                result = Jagged.Create<T>(size, size);

            if (!symmetric)
            {
                for (int i = 0; i < size; i++)
                    result[i] = generator.Generate(size);
            }
            else
            {
                for (int i = 0; i < size; i++)
                {
                    T[] row = generator.Generate(size / 2, result[i]);
                    for (int start = 0, end = size - 1; start < size / 2; start++, end--)
                        row[end] = row[start];
                }
            }

            return result;
        }

        /// <summary>
        ///   Creates a rows-by-cols matrix with random data.
        /// </summary>
        /// 
        public static T[][] Random<T>(int rows, int cols,
            IRandomNumberGenerator<T> generator, T[][] result = null)
        {
            if (result == null)
                result = Jagged.Create<T>(rows, cols);

            for (int i = 0; i < rows; i++)
                result[i] = generator.Generate(cols);
            return result;
        }




        /// <summary>
        ///   Creates a rows-by-cols matrix random data drawn from a given distribution.
        /// </summary>
        /// 
        public static double[][] Random(int rows, int cols, double minValue = 0, double maxValue = 1)
        {
            return Random<double>(rows, cols, new ZigguratUniformGenerator(minValue, maxValue));
        }
        #endregion

    }
}
