using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

using NMatrix.Decompositions;

namespace NMatrix
{
    /// <summary>
    /// Represents a matrix class.
    /// </summary>
    public class Matrix : IEquatable<Matrix>, IEnumerable
    {
        #region Private fields

        private readonly double[,] _buffer;
        private readonly int _rows;
        private readonly int _columns;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new matrix.
        /// </summary>
        /// <param name="rows">Rows.</param>
        /// <param name="columns">Columns.</param>
        public Matrix(int rows, int columns)
        {
            CheckSize(rows, columns);

            _rows = rows;
            _columns = columns;
            _buffer = new double[_rows, _columns];
        }

        /// <summary>
        /// Creates a new matrix.
        /// </summary>
        /// <param name="rows">Rows.</param>
        /// <param name="columns">Columns.</param>
        /// <param name="buffer">Matrix buffer.</param>
        public Matrix(int rows, int columns, double[,] buffer)
        {
            CheckSize(rows, columns);
            CheckBufferSize(rows, columns, buffer);
            _rows = rows;
            _columns = columns;
            _buffer = buffer;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a number of rows.
        /// </summary>
        public int Rows => _rows;

        /// <summary>
        /// Gets a number of columns.
        /// </summary>
        public int Columns => _columns;

        /// <summary>
        /// Gets a flag if this matrix is square or not.
        /// </summary>
        public bool IsSquare => _rows == _columns;

        /// <summary>
        /// Gets a flag if this matrix is symmetric or not.
        /// </summary>
        public bool IsSymmetric => GetIsSymmetric();

        /// <summary>
        /// Gets or sets a value corresponding to a row and a column of the matrix.
        /// </summary>
        /// <param name="row">Value row index.</param>
        /// <param name="column">Value column index.</param>
        /// <returns>
        /// Returns matrix value.
        /// </returns>
        public double this[int row, int column]
        {
            get
            {
                return _buffer[row, column];
            }
            set
            {
                _buffer[row, column] = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a matrix.
        /// </summary>
        /// <param name="matrix">A matrix.</param>
        /// <returns>
        /// Returns a result of addition of two matrices.
        /// </returns>
        public Matrix Add(Matrix matrix)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException(nameof(matrix), "Matrix could not be null.");
            }

            if (Rows != matrix.Rows || Columns != matrix.Columns)
            {
                throw new MatrixInconsistencyException(
                    $"Matrix one [{Rows}x{Columns}] does not correspond to Matrix Two [{matrix.Rows}x{matrix.Columns}].");
            }

            var result = new double[Rows, Columns];

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    result[i, j] = _buffer[i, j] + matrix[i, j];
                }
            }
            return new Matrix(Rows, Columns, result);
        }

        /// <summary>
        /// Adds an Array to the Matrix
        /// </summary>
        /// <returns>The add.</returns>
        /// <param name="array">Array.</param>
        public Matrix Add(Double[] array)
        {
            Matrix arrayAsMatrix = new Matrix(1,array.Length);
            for(int i = 0; i < array.Length; i++)
            {
                arrayAsMatrix[0, i] = array[i];
            }
            return this.Add(arrayAsMatrix);
        }

        /// <summary>
        /// Subtracts a matrix.
        /// </summary>
        /// <param name="matrix">A matrix.</param>
        /// <returns>
        /// Returns a result of subtraction of two matrices.
        /// </returns>
        public Matrix Subtract(Matrix matrix)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException(nameof(matrix), "Matrix could not be null.");
            }

            if (Columns != matrix.Columns || Rows != matrix.Rows)
            {
                throw new MatrixInconsistencyException(
                    $"Matrix one [{Rows}x{Columns}] does not correspond to Matrix Two [{matrix.Rows}x{matrix.Columns}].");
            }

            var result = new double[Rows, Columns];

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    result[i, j] = _buffer[i, j] - matrix[i, j];
                }
            }
            return new Matrix(Rows, Columns, result);
        }

        /// <summary>
        /// Multiply a matrix.
        /// </summary>
        /// <param name="matrix">A matrix.</param>
        /// <returns>
        /// Returns a result of multiplication of two matrices.
        /// </returns>
        public Matrix Multiply(Matrix matrix)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException(nameof(matrix), "Matrix could not be null.");
            }

            if (Columns != matrix.Rows)
            {
                throw new MatrixInconsistencyException(
                    $"Matrix one [{Rows}x{Columns}] does not correspond to Matrix Two [{matrix.Rows}x{matrix.Columns}].");
            }

            var result = new double[Rows, matrix.Columns];

            for (int r = 0; r < Columns; r++)
            {
                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < matrix.Columns; j++)
                    {
                        result[i, j] += this[i, r] * matrix[r, j];
                    }
                }
            }

            return new Matrix(Rows, matrix.Columns, result);
        }

        /// <summary>
        /// Multiply a scalar.
        /// </summary>
        /// <param name="x">Scalar.</param>
        /// <returns>
        /// Returns a new matrix multiplied by this scalar.
        /// </returns>
        public Matrix Multiply(double x)
        {
            var resultBuffer = new double[Rows, Columns];

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    resultBuffer[i, j] = _buffer[i, j] * x;
                }
            }

            return new Matrix(Rows, Columns, resultBuffer);
        }

        /// <summary>
        /// Multiplies a vector.
        /// </summary>
        /// <param name="x">A vector.</param>
        /// <returns>
        /// Returns a new vector.
        /// </returns>
        public Vector Multiply(Vector x)
        {
            if (x == null)
            {
                throw new ArgumentNullException("Vector cannot be null");
            }

            if (Rows != x.Size)
            {
                throw new VectorInconsistencyException(
                    "The matrix and multiplied vector have an inconsistance size.");
            }

            var result = new Vector(x.Size);

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    result[i] += this[i, j] * x[j];
                }
            }

            return result;
        }

        /// <summary>
        /// Get the Dot Product of two Matrices
        /// </summary>
        /// <returns>The dot.</returns>
        /// <param name="m">M.</param>
        //public double Dot(Matrix other)
        //{

        //    Matrix m = new Matrix(0,0);
        //    for(int i = 0; i < Rows; i++)
        //    {
        //        for(int j = 0; j < Columns; j++)
        //        {

        //        }
        //    }
        //}

        /// <summary>
        /// Divides on a scalar.
        /// </summary>
        /// <param name="x">Scalar.</param>
        /// <returns>
        /// Returns a new matrix divided by this scalar.
        /// </returns>
        public Matrix Divide(double x)
        {
            return Multiply(1f / x);
        }

        /// <summary>
        /// Gets a minor matrix for corresponding matrix element.
        /// </summary>
        /// <param name="row">Element's row index.</param>
        /// <param name="column">Element's column index.</param>
        /// <returns>
        /// Returns the minor matrix.
        /// </returns>
        public Matrix Minor(int row, int column)
        {
            if (row < 0)
            {
                throw new ArgumentException("Row has to be more or equal zero.");
            }

            if (column < 0)
            {
                throw new ArgumentException("Column has to be more or equal zero.");
            }

            if (Rows <= 2 || Columns <= 2)
            {
                throw new MatrixInconsistencyException("In order to get a minor matrix the orginal matrix has to be more or equal than 3x3.");
            }

            var minorRows = Rows - 1;
            var minorColumns = Columns - 1;
            var minorBuffer = new double[minorRows, minorColumns];
            int i, j;

            for (int mi = 0; mi < minorRows; mi++)
            {
                if (mi >= row)
                    i = mi + 1;
                else
                    i = mi;

                for (int mj = 0; mj < minorColumns; mj++)
                {
                    if (mj >= column)
                        j = mj + 1;
                    else
                        j = mj;

                    minorBuffer[mi, mj] = this[i, j];
                }
            }

            var minorMatrix = new Matrix(Rows - 1, Columns - 1, minorBuffer);
            return minorMatrix;
        }

        /// <summary>
        /// Gets a matrix determinant.
        /// </summary>
        /// <returns>
        /// Returns the matrix determinant.
        /// </returns>
        public double Determinant()
        {
            if (!IsSquare)
            {
                throw new NonSquareMatrixException("Attempt to find a determinant of a non square matrix");
            }

            if (Rows == 2 && Columns == 2)
            {
                return Determinant2x2();
            }

            if (Rows == 3 && Columns == 3)
            {
                return Determinant3x3();
            }

            double determinant = 1;

            var luDecomposition = new LupDecomposition();

            var (c, _) = luDecomposition.Decompose(this);

            for (int i = 0; i < c.Rows; i++)
            {
                determinant *= c[i, i];
            }

            return determinant;
        }

        /// <summary>
        /// Gets a matrix determinant for matrices 2x2.
        /// </summary>
        /// <returns>
        /// Returns a matrix determinant for matrices 2x2.
        /// </returns>
        private double Determinant2x2()
        {
            return _buffer[0, 0] * _buffer[1, 1] - _buffer[0, 1] * _buffer[1, 0];
        }

        /// <summary>
        /// Gets a matrix determinant for matrices 3x3 using rule of Sarrus.
        /// </summary>
        /// <returns>
        /// Returns a matrix determinant for matrices 3x3.
        /// </returns>
        /// <remarks>
        /// Rule of Sarrus https://en.wikipedia.org/wiki/Rule_of_Sarrus.
        /// </remarks>
        private double Determinant3x3()
        {
            return 
                  _buffer[0, 0] * _buffer[1, 1] * _buffer[2, 2]
                + _buffer[0, 1] * _buffer[1, 2] * _buffer[2, 0]
                + _buffer[0, 2] * _buffer[1, 0] * _buffer[2, 1]
                - _buffer[0, 2] * _buffer[1, 1] * _buffer[2, 0]
                - _buffer[0, 0] * _buffer[1, 2] * _buffer[2, 1]
                - _buffer[0, 1] * _buffer[1, 0] * _buffer[2, 2];
        }

        /// <summary>
        /// Transposes a matrix.
        /// </summary>
        /// <returns>
        /// Returns a transposed matrix.
        /// </returns>
        public Matrix Transpose()
        {
            var transposedMatrix = new Matrix(Columns, Rows);

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    transposedMatrix[j, i] = _buffer[i, j];
                }
            }

            return transposedMatrix;
        }

        /// <summary>
        /// Gets an adjoint matrix of the current matrix.
        /// </summary>
        /// <returns>
        /// Returns an adjoint matrix of the current matrix.
        /// </returns>
        public Matrix Adjoint()
        {
            if (Rows != Columns)
            {
                throw new NonSquareMatrixException("Attempt to find an adjoint matrix for the non square matrix");
            }

            if (Rows < 2 && Columns < 2)
            {
                throw new MatrixInconsistencyException("Matrix size could not be less than 2x2.");
            }

            Matrix minor = null;
            var adjointMatrix = new Matrix(Columns, Rows);

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    minor = Minor(i, j);
                    adjointMatrix[j, i] = Math.Pow(-1, i + j) * minor.Determinant();
                }
            }

            return adjointMatrix;
        }

        /// <summary>
        /// Inverses a matrix.
        /// </summary>
        /// <returns>
        /// Returns an inverted matrix.
        /// </returns>
        public Matrix Inverse()
        {
            if (Determinant() == 0)
            {
                throw new MatrixInconsistencyException("Attempt to invert a singular matrix");
            }

            return Adjoint() / Determinant();
        }

        /// <summary>
        /// Checks if this matrix is symmetric or not.
        /// </summary>
        /// <returns>
        /// Returns true if this matrix is symmetric, othewise - false.
        /// </returns>
        private bool GetIsSymmetric()
        {
            if (!IsSquare)
            {
                return false;
            }

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    if (this[i, j] != this[j, i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void CheckSize(int rows, int columns)
        {
            if (rows < 1)
            {
                throw new ArgumentException("Rows could not be less 1.", nameof(rows));
            }

            if (columns < 1)
            {
                throw new ArgumentException("Columns could not be less 1.", nameof(rows));
            }
        }

        private void CheckBufferSize(int rows, int columns, double[,] buffer)
        {
            int bufferRows = buffer.GetLength(0);
            int bufferColumns = buffer.GetLength(1);

            if (bufferRows != rows || bufferColumns != columns)
            {
                throw new ArgumentException(
                    $"Buffer has size {bufferRows}x{bufferColumns} instead of {rows}x{columns}", nameof(buffer));
            }
        }

        /// <summary>
        /// Calcalates matrix trace.
        /// </summary>
        /// <returns>
        /// Returns matrix trace.
        /// </returns>
        public double Trace()
        {
            if (!IsSquare)
                throw new NonSquareMatrixException("Cannot calculate trace for non-square matrices");

            var trace = 0d;
            for (int i = 0; i < Rows; i++)
            {
                trace += _buffer[i, i];
            }

            return trace;
        }

        #region Operators

        /// <summary>
        /// Adds two matrices.
        /// </summary>
        /// <param name="m1">Matrix one.</param>
        /// <param name="m2">Matrix one.</param>
        /// <returns>
        /// Returns a result of addition of two matrices.
        /// </returns>
        public static Matrix operator +(Matrix m1, Matrix m2) => m1.Add(m2);

        /// <summary>
        /// Subtracts two matrices.
        /// </summary>
        /// <param name="m1">Matrix one.</param>
        /// <param name="m2">Matrix one.</param>
        /// <returns>
        /// Returns a result of subtraction of two matrices.
        /// </returns>
        public static Matrix operator -(Matrix m1, Matrix m2) => m1.Subtract(m2);

        /// <summary>
        /// Multiplies two matrices.
        /// </summary>
        /// <param name="m1">Matrix one.</param>
        /// <param name="m2">Matrix one.</param>
        /// <returns>
        /// Returns a result of multiplication of two matrices.
        /// </returns>
        public static Matrix operator *(Matrix m1, Matrix m2) => m1.Multiply(m2);

        /// <summary>
        /// Multiplies a matrix by a scalar.
        /// </summary>
        /// <param name="m1">A matrix.</param>
        /// <param name="scalar">A scalar.</param>
        /// <returns>
        /// Returns a result of multiplication of a matrix by a scalar.
        /// </returns>
        public static Matrix operator *(Matrix m1, double scalar) => m1.Multiply(scalar);

        /// <summary>
        /// Multiplies a matrix by a vector.
        /// </summary>
        /// <param name="m1">A matrix.</param>
        /// <param name="vector">A vector.</param>
        /// <returns>
        /// Returns a result of multiplication of a matrix by a vector.
        /// </returns>
        public static Vector operator *(Matrix m1, Vector vector) => m1.Multiply(vector);

        /// <summary>
        /// Divides a matrix on a scalar.
        /// </summary>
        /// <param name="m1">A matrix.</param>
        /// <param name="scalar">A scalar.</param>
        /// <returns>
        /// Returns a result of division of matrix by a scalar.
        /// </returns>
        public static Matrix operator /(Matrix m1, double scalar) => m1.Divide(scalar);

        /// <summary>
        /// Transposes a matrix.
        /// </summary>
        /// <param name="m1">A matrix.</param>
        /// <returns>
        /// Returns a transposed matrix.
        /// </returns>
        public static Matrix operator ~(Matrix m1) => m1.Transpose();

        /// <summary>
        /// Inverses a matrix.
        /// </summary>
        /// <param name="m1">A matrix.</param>
        /// <returns>
        /// Returns an inversed matrix.
        /// </returns>
        public static Matrix operator !(Matrix m1) => m1.Inverse();

        #endregion

        #region Matrix norms


        /// <summary>
        /// Calculates matrix p1-norm.
        /// </summary>
        /// <returns>
        /// Returns a matrix p1-norm.
        /// </returns>
        public double NormOne()
        {
            var result = 0d;
            for (int j = 0; j < Columns; j++)
            {
                var sum = 0d;
                for (int i = 0; i < Rows; i++)
                {
                    sum += _buffer[i, j];
                }
               
                result = Math.Max(result, sum);
            }

            return result;
        }

        /// <summary>
        /// Calculates matrix p∞-norm.
        /// </summary>
        /// <returns>
        /// Returns a matrix p∞-norm.
        /// </returns>
        public double NormInfinity()
        {
            var result = 0d;
            for (int i = 0; i < Rows; i++)
            {
                var sum = 0d;
                for (int j = 0; j < Columns; j++)
                {
                    sum += _buffer[i, j];
                }
               
                result = Math.Max(result, sum);
            }

            return result;
        }

        /// <summary>
        /// Calculates matrix p2-norm.
        /// </summary>
        /// <returns>
        /// Returns a matrix p2-norm.
        /// </returns>
        public double NormTwo()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates Frobenius norm of matrix.
        /// </summary>
        /// <returns>
        /// Returns Frobenius norm of matrix.
        /// </returns>
        public double FrobeniusNorm()
        {
            var result = 0d;
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    result += Math.Pow(_buffer[i, j], 2);
                }
            }

            return Math.Sqrt(result);
        }

        #endregion

        #region Factories

        /// <summary>
        /// Creates a new idenity matrix with size n x n.
        /// </summary>
        /// <param name="n">Matrix size.</param>
        /// <returns>
        /// Returns a new identity matrix.
        /// </returns>
        public static Matrix Identity(int n)
        {
            if (n < 2)
            {
                throw new ArgumentException("Size of a square matrix could not be less than 2.", nameof(n));
            }

            var buffer = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                buffer[i, i] = 1;
            }

            return new Matrix(n, n, buffer);
        }

        /// <summary>
        /// Creates a new ones matrix with size n x n.
        /// </summary>
        /// <param name="n">Matrix size.</param>
        /// <returns>
        /// Returns a new ones matrix.
        /// </returns>
        public static Matrix Ones(int n)
        {
            if (n < 2)
            {
                throw new ArgumentException("Size of a square matrix could not be less than 2.", nameof(n));
            }

            var buffer = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    buffer[i, j] = 1;
                }
            }

            return new Matrix(n, n, buffer);
        }

        /// <summary>
        /// Creates a new matrix from buffer.
        /// </summary>
        /// <param name="buffer">A buffer.</param>
        /// <returns>
        /// Returns a new matrix.
        /// </returns>
        public static Matrix From(double[,] buffer)
        {
            var rows = buffer.GetLength(0);
            var columns = buffer.GetLength(1);
            return new Matrix(rows, columns, buffer);
        }

        #endregion

        /// <summary>
        /// Checks if two matrices are equal or not.
        /// </summary>
        /// <param name="other">Matrix.</param>
        /// <returns>
        /// Returns true if two matrices are identical, otherwise - false.
        /// </returns>
        public bool Equals(Matrix other)
        {
            if (other == null)
            {
                return false;
            }
            else
            {
                if (Rows != other.Rows || Columns != other.Columns)
                {
                    return false;
                }
                else
                {
                    for (int i = 0; i < Rows; i++)
                    {
                        for (int j = 0; j < Columns; j++)
                        {
                            if (_buffer[i, j] != other[i, j])
                            {
                                return false;
                            }
                        }
                    }
                    return true;
                }
            }
        }

        /// <summary>
        /// Get the Matrix as an Array
        /// </summary>
        /// <returns>The array.</returns>
        //public double[,] ToArray()
        //{
        //    return _buffer;
        //}

        /// <summary>
        /// Get the Matrix as Multidimensional Array
        /// </summary>
        /// <returns>The multidimensional array.</returns>
        public double[][] ToArray()
        {
            double[][] multidimensionalArray = new double[Rows][];

            for(int i = 0; i < Rows; i++)
            {
                multidimensionalArray[i] = new double[Columns];

                for (int j = 0; j < Columns; j++)
                {
                    multidimensionalArray[i][j] = this[i,j];
                }
            }

            return multidimensionalArray;
        }

        /// <summary>
        /// Get an entire Column as an Array
        /// </summary>
        /// <returns>The column array.</returns>
        /// <param name="column">Column.</param>
        public double[] GetColumnArray(int column)
        {
            List<double> doubleList = new List<double>();

            for(int i = 0; i < Rows; i++)
            {
                doubleList.Add(this[i, column]);
            }

            return doubleList.ToArray();
        }

        /// <summary>
        /// Get an entire Row as an Array
        /// </summary>
        /// <returns>The column array.</returns>
        /// <param name="row">Row.</param>
        public double[] GetRowArray(int row)
        {
            List<double> doubleList = new List<double>();

            for (int i = 0; i < Columns; i++)
            {
                doubleList.Add(this[row, i]);
            }

            return doubleList.ToArray();
        }

        /// <summary>
        /// Gets a matrix string representation.
        /// </summary>
        /// <returns>
        /// Returns a matrix string representation.
        /// </returns>
        public override string ToString()
        {
            var columnSizes = new int[Columns];

            for (int j = 0; j < Columns; j++)
            {
                var maxColumnSize = 0;

                for (int i = 0; i < Rows; i++)
                {
                    columnSizes[j] = Math.Max(maxColumnSize, _buffer[i, j]
                        .ToString(CultureInfo.InvariantCulture).Length);
                    maxColumnSize = columnSizes[j];
                }
            }

            var sb = new StringBuilder(string.Format("X = {0} x {1}", Rows, Columns));
            sb.Append("\n");

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    sb.Append(_buffer[i, j].ToString().PadRight(columnSizes[j]));
                    if (j != Columns - 1)
                    {
                        sb.Append("  ");
                    }
                }
                if (i != Rows - 1)
                {
                    sb.Append("\n");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Clones current matrix.
        /// </summary>
        /// <returns>
        /// Returns a matrix clone.
        /// </returns>
        public Matrix Clone()
        {
            var buffer = new double[Rows, Columns];
            Array.Copy(_buffer, buffer, _buffer.Length);

            return Matrix.From(buffer);
        }

        /// <summary>
        /// Gets an enumerator for inner matrix buffer.
        /// </summary>
        /// <returns>
        /// Returns an enumerator.
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            return _buffer.GetEnumerator();
        }

        #endregion
        public void Fill(double value)
        {
            for(int i = 0; i < Rows; i++)
            {
                for(int j = 0; j < Columns; j++)
                {
                    this[i, j] = value;
                }
            }
        }

        /// <summary>
        /// Fills the whole Row with the Value
        /// </summary>
        /// <param name="row">Row.</param>
        /// <param name="value">Value.</param>
        public void FillRow(int row, double value)
        {
            for(int i = 0; i < Columns; i++)
            {
                this[row, i] = value;
            }
        }

        /// <summary>
        /// Fills the whole Column with the Value
        /// </summary>
        /// <param name="row">Row.</param>
        /// <param name="value">Value.</param>
        public void FillColumn(int column, double value)
        {
            for (int i = 0; i < Rows; i++)
            {
                this[i, column] = value;
            }
        }
    }
}
