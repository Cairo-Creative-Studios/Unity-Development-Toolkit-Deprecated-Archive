namespace NMatrix.Decompositions
{
    /// <summary>
    /// Represents LU decomposition result.
    /// </summary>
    public class LUResult
    {
        #region Private fields

        private readonly Matrix _l;
        private readonly Matrix _u;

        #endregion
        
        #region Constructors

        /// <summary>
        /// Creates a LU result of matrix decompostition.
        /// </summary>
        /// <param name="l">A lower matrix.</param>
        /// <param name="u">An upper matrix.</param>
        public LUResult(Matrix l, Matrix u)
        {
            _l = l;
            _u = u;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a lower matrix.
        /// </summary>
        public Matrix L => _l;

        /// <summary>
        /// Gets an upper matrix.
        /// </summary>
        public Matrix U => _u;

        #endregion

        #region Methods

        /// <summary>
        /// Deconstructs LU result into a tuple.
        /// </summary>
        /// <param name="l">A lower matrix.</param>
        /// <param name="u">An upper matrix.</param>
        public void Deconstruct(out Matrix l, out Matrix u)
        {
            l = L;
            u = U;
        }

        #endregion
    }
}