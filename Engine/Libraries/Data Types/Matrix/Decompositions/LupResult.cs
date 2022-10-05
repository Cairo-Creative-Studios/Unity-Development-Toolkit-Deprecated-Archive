using System;

namespace NMatrix.Decompositions
{
    /// <summary>
    /// Represents a LUP decomposition result.
    /// </summary>
    public sealed class LupResult
    {
        #region Private fields

        private readonly Matrix _c;
        private readonly Matrix _p;
            
        #endregion
        
        #region Constructors

        /// <summary>
        /// Creates a LUP result of matrix decomposition.
        /// </summary>
        /// <param name="c">C matrix (C = L + U - E)</param>
        /// <param name="p">P permitation matrix.</param>
        public LupResult(Matrix c, Matrix p)
        {
            _c = c;
            _p = p;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets C matrix (C = L + U - E).
        /// </summary>
        public Matrix C => _c;

        /// <summary>
        /// Gets peramiutation matrix.
        /// </summary>
        public Matrix P => _p;

        #endregion

        #region Methods

        /// <summary>
        /// Deconstructs a result into a tuple.
        /// </summary>
        /// <param name="c">C matrix (C = L + U - E)</param>
        /// <param name="p">Peramiutation matrix</param>
        public void Deconstruct(out Matrix c, out Matrix p)
        {
            c = C;
            p = P;
        }

        #endregion
    }
}