using System;

namespace NMatrix.Decompositions
{
    /// <summary>
    /// Represents a decomposition algorithm.
    /// </summary>
    /// <typeparam name="T">A decomposition result type.</typeparam>
    public interface IDecomposition<T>
    {
        /// <summary>
        /// Decomposes an initial matrix into several matrices.
        /// </summary>
        /// <param name="matrix">An initial matrix.</param>
        /// <returns>
        /// Returns a decomposition of the matrix.
        /// </returns>
        public T Decompose(Matrix matrix);
    }
}