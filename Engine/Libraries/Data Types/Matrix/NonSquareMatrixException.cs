using System;

namespace NMatrix
{
    /// <summary>
    /// Represents a non-square matrix exception.
    /// </summary>
    public sealed class NonSquareMatrixException : Exception
    {
        #region Constructors

        /// <summary>
        /// Creates a new non-square matrix exception.
        /// </summary>
        public NonSquareMatrixException()
            : base("Matrix is not square.")
        {

        }

        /// <summary>
        /// Creates a new non-square matrix exception.
        /// </summary>
        /// <param name="message">An exception message.</param>
        public NonSquareMatrixException(string message)
            : base(message)
        {

        }

        #endregion
    }
}
