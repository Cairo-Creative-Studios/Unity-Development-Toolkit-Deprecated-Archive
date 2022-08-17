using System;

namespace NMatrix
{
    /// <summary>
    /// Represents inconsistency exception between two matrix.
    /// </summary>
    public class MatrixInconsistencyException : Exception
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of an exception.
        /// </summary>
        /// <param name="message">An error message.</param>
        public MatrixInconsistencyException(string message)
            : base(message)
        {

        }

        #endregion
    }
}
