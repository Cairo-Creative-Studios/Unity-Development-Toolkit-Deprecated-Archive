using System;

namespace NMatrix
{
    /// <summary>
    /// Represents a non-symmetric matrix exception.
    /// </summary>
    public sealed class NonSymmetricMatrixException : Exception
    {
        #region Constructors

        /// <summary>
        /// Creates a new non-symmetric matrix exception.
        /// </summary>
        public NonSymmetricMatrixException()
            : base("Matrix is not symmetric.")
        {

        }

        /// <summary>
        /// Creates a new non-symmetric matrix exception.
        /// </summary>
        /// <param name="message">An exception message.</param>
        public NonSymmetricMatrixException(string message)
            : base(message)
        {

        }

        #endregion
    }
}
