using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace NMatrix
{
    /// <summary>
    /// Represents a vector.
    /// </summary>
    public class Vector : IEquatable<Vector>, IEnumerable
    {
        #region Private fields

        private readonly double[] _buffer;
        private readonly int _size;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new vecor with currnet size.
        /// </summary>
        /// <param name="size">Vector's size.</param>
        public Vector(int size)
            : this(size, new double[size])
        {
        }

        /// <summary>
        /// Creates a new vecor with currnet size and buffer.
        /// </summary>
        /// <param name="size">Vector's size.</param>
        /// <param name="buffer">Vector's buffer.</param>
        public Vector(int size, double[] buffer)
        {
            _size = size;
            _buffer = buffer;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets vector's size.
        /// </summary>
        public int Size => _size;

        /// <summary>
        /// Gets or sets a value corresponding to an index of the vector.
        /// </summary>
        /// <param name="index">Value index.</param>
        /// <returns>
        /// Returns vector value.
        /// </returns>
        public double this[int index]
        {
            get
            {
                return _buffer[GetIndex(index)];
            }
            set
            {
                _buffer[GetIndex(index)] = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a vector.
        /// </summary>
        /// <param name="vector">A vector.</param>
        /// <returns>
        /// Returns a result of addition of two vectors.
        /// </returns>
        public Vector Add(Vector vector)
        {
            CheckSize(vector);

            var buffer = new double[Size];

            for (int i = 0; i < Size; i++)
            {
                buffer[i] += this[i] + vector[i];
            }

            return new Vector(Size, buffer);
        }

        /// <summary>
        /// Adds a scalar.
        /// </summary>
        /// <param name="scalar">A scalar.</param>
        /// <returns>
        /// Returns a result of addition of a scalar to a vector.
        /// </returns>
        public Vector Add(double scalar)
        {
            var buffer = new double[Size];

            for (int i = 0; i < Size; i++)
            {
                buffer[i] += this[i] + scalar;
            }

            return new Vector(Size, buffer);
        }

        /// <summary>
        /// Subtracts a vector.
        /// </summary>
        /// <param name="vector">A vector.</param>
        /// <returns>
        /// Returns a result of subtraction of two vectors.
        /// </returns>
        public Vector Subtract(Vector vector)
        {
            CheckSize(vector);

            var buffer = new double[Size];

            for (int i = 0; i < Size; i++)
            {
                buffer[i] += this[i] - vector[i];
            }

            return new Vector(Size, buffer);
        }

        /// <summary>
        /// Subtracts a scalar.
        /// </summary>
        /// <param name="scalar">A scalar.</param>
        /// <returns>
        /// Returns a result of subtraction of a scalar from a vector.
        /// </returns>
        public Vector Subtract(double scalar)
        {
            var buffer = new double[Size];

            for (int i = 0; i < Size; i++)
            {
                buffer[i] += this[i] - scalar;
            }

            return new Vector(Size, buffer);
        }

        /// <summary>
        /// Creates a new vector with reversed sign elements.
        /// </summary>
        /// <returns>
        /// Returns a new vector with reversed sign elements.
        /// </returns>
        public Vector Negate()
        {
            var buffer = new double[Size];
            Array.Copy(_buffer, buffer, Size);

            for (int i = 0; i < Size; i++)
            {
                buffer[i] = -buffer[i];
            }

            return Vector.From(buffer);
        }

        /// <summary>
        /// Calculates a scalar multiplication of two vectors.
        /// </summary>
        /// <param name="vector">A vector.</param>
        /// <returns>
        /// Returns a dot product of two vectors.
        /// </returns>
        public double DotProduct(Vector vector)
        {
            CheckSize(vector);

            var sum = 0.0;
            for (int i = 0; i < Size; i++)
            {
                sum += this[i] * vector[i];
            }
            return sum;
        }

        /// <summary>
        /// Multiplies by a scalar.
        /// </summary>
        /// <param name="scalar">A scalar.</param>
        /// <returns>
        /// Returns a result of multiplication of a vector by a scalar.
        /// </returns>
        public Vector Multiply(double scalar)
        {
            var buffer = new double[Size];

            for (int i = 0; i < Size; i++)
            {
                buffer[i] += this[i] * scalar;
            }

            return new Vector(Size, buffer);
        }

        /// <summary>
        /// Divides by a scalar.
        /// </summary>
        /// <param name="scalar">A scalar.</param>
        /// <returns>
        /// Returns a result of division of a vector by a scalar.
        /// </returns>
        public Vector Divide(double scalar)
        {
            return Multiply(1 / scalar);
        }

        /// <summary>
        /// Calculates vector's norm.
        /// </summary>
        /// <returns>
        /// Returns vector's norm.
        /// </returns>
        public double Norm()
        {
            return Norm(2);
        }

        /// <summary>
        /// Calculates vector's p-norm.
        /// </summary>
        /// <returns>
        /// Returns vector's p-norm.
        /// </returns>
        public double Norm(double p)
        {
            if (p < 1)
                throw new ArgumentException("p-norm cannot be less than 1.");

            if (p == double.PositiveInfinity)
                return _buffer.Max();
            else
            {
                var sum = 0.0;
                for (int i = 0; i < Size; i++)
                {
                    sum += Math.Pow(Math.Abs(this[i]), p);
                }

                return Math.Pow(sum, 1 / p);
            }
        }

        /// <summary>
        /// Normalizes the vector.
        /// </summary>
        /// <returns>
        /// Returns a normalized vector.
        /// </returns>
        public Vector Normalize()
        {
            return this / Norm();
        }

        /// <summary>
        /// Checks if two represented vectors are equal or not.
        /// </summary>
        /// <param name="other">Othe vector.</param>
        /// <returns>
        /// Returns true if two vectors are equal, otherwise - false.
        /// </returns>
        public bool Equals(Vector other)
        {
            if (other == null)
            {
                return false;
            }
            else
            {
                if (Size != other.Size)
                {
                    return false;
                }
                else
                {
                    for (int i = 0; i < Size; i++)
                    {
                        if (this[i] != other[i])
                            return false;
                    }
                    return true;
                }
            }
        }

        private int GetIndex(int index)
        {
            if (index < 0)
            {
                return Size + index;
            }
            return index;
        }

        private void CheckSize(Vector vector)
        {
            if (vector == null)
            {
                throw new ArgumentNullException(nameof(vector), "Vector could not be null.");
            }

            if (Size != vector.Size)
            {
                throw new VectorInconsistencyException(
                    $"Vector one S:{Size} does not correspond vector Two S:{vector.Size}.");
            }
        }

        #region Operators

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="v1">Vector one.</param>
        /// <param name="v2">Vector two.</param>
        /// <returns>
        /// Returns a result of addition of two vectors.
        /// </returns>
        public static Vector operator +(Vector v1, Vector v2) => v1.Add(v2);

        /// <summary>
        /// Adds a scalar to a vector.
        /// </summary>
        /// <param name="v">A vector.</param>
        /// <param name="s">A scalar.</param>
        /// <returns>
        /// Returns a result of addition of a scalar to a vector.
        /// </returns>
        public static Vector operator +(Vector v, double s) => v.Add(s);

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="v1">Vector one.</param>
        /// <param name="v2">Vector two.</param>
        /// <returns>
        /// Returns a result of subtraction of two vectors.
        /// </returns>
        public static Vector operator -(Vector v1, Vector v2) => v1.Subtract(v2);

        /// <summary>
        /// Subtracts a scalar from a vector.
        /// </summary>
        /// <param name="v">A vector.</param>
        /// <param name="s">A scalar.</param>
        /// <returns>
        /// Returns a result of subtraction of a scalar from a vector.
        /// </returns>
        public static Vector operator -(Vector v, double s) => v.Subtract(s);

        /// <summary>
        /// Negates a vector.
        /// </summary>
        /// <param name="v">A vector.</param>
        /// <returns>
        /// Returns a new vector with reversed sign elements.
        /// </returns>
        public static Vector operator -(Vector v) => v.Negate();

        /// <summary>
        /// Calculates a scalar multiplication of two vectors.
        /// </summary>
        /// <param name="v1">Vector one.</param>
        /// <param name="v2">Vector two.</param>
        /// <returns>
        /// Returns a dot product of two vectors.
        /// </returns>
        public static double operator *(Vector v1, Vector v2) => v1.DotProduct(v2);

        /// <summary>
        /// Multiplies by a scalar.
        /// </summary>
        /// <param name="v">A vector.</param>
        /// <param name="scalar">A scalar.</param>
        /// <returns>
        /// Returns a result of multiplication of a vector by a scalar.
        /// </returns>
        public static Vector operator *(Vector v, double scalar) => v.Multiply(scalar);

        /// <summary>
        /// Divides a vector by a scalar.
        /// </summary>
        /// <param name="v">A vector.</param>
        /// <param name="s">A scalar.</param>
        /// <returns>
        /// Returns a result of division of a vector by a scalar.
        /// </returns>
        public static Vector operator /(Vector v, double s) => v.Divide(s);

        #endregion

        #region Factories

        /// <summary>
        /// Creates a vector from existing buffer.
        /// </summary>
        /// <param name="buffer">A buffer.</param>
        /// <returns>
        /// Returns a new vector.
        /// </returns>
        public static Vector From(double[] buffer)
        {
            return new Vector(buffer.Length, buffer);
        }

        #endregion

        /// <summary>
        /// Gets a vector string representation.
        /// </summary>
        /// <returns>
        /// Returns a vector string representation.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder(string.Format("Items: {0}", Size));
            sb.Append("\n");

            sb.Append("[ ");
            for (int i = 0; i < Size; i++)
            {
                sb.AppendFormat("{0} ", this[i]);
            }
            sb.Append("]");

            return sb.ToString();
        }

        /// <summary>
        /// Copies vector to array.
        /// </summary>
        /// <returns>
        /// Returns vector elements as an array.
        /// </returns>
        public double[] ToArray()
        {
            var result = new double[Size];
            _buffer.CopyTo(result, 0);

            return result;
        }

        /// <summary>
        /// Gets a vector enumerator.
        /// </summary>
        /// <returns>
        /// Returns a vector enumerator.
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            return _buffer.GetEnumerator();
        }

        #endregion
    }
}
