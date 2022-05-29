using System;

namespace NMatrix
{
    /// <summary>
    /// Represents inconsistency exception between two vectors.
    /// </summary>
    public class VectorInconsistencyException : Exception
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of an exception.
        /// </summary>
        /// <param name="message">An error message.</param>
        public VectorInconsistencyException(string message)
            : base(message)
        {

        }

        #endregion
    }
}
