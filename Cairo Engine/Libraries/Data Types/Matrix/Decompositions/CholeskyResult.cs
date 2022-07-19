namespace NMatrix.Decompositions
{
    /// <summary>
    /// Represents Cholesky decimposition result.
    /// </summary>
    public class CholeskyResult
    {
        #region Private fields

        private readonly Matrix _lower;
        private readonly Matrix _lowerTransposed;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates Cholesky decomposition result.
        /// </summary>
        /// <param name="lower">A lower matrix.</param>
        /// <param name="lowerTransposed">A lower transposed matrix.</param>
        public CholeskyResult(Matrix lower, Matrix lowerTransposed)
        {
            _lower = lower;
            _lowerTransposed = lowerTransposed;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets L (lower) matrix.
        /// </summary>
        public Matrix L => _lower;

        /// <summary>
        /// Gets LT (L* lower transposed) matrix. 
        /// </summary>
        public Matrix LT => _lowerTransposed;

        #endregion

        #region Methods

        /// <summary>
        /// Deconstructs Cholesky decimposition result into a tuple.
        /// </summary>
        /// <param name="l">L (lower) matrix</param>
        /// <param name="lt">LT (L* lower transposed) matrix</param>
        public void Deconstruct(out Matrix l, out Matrix lt)
        {
            l = L;
            lt = LT;
        }

        #endregion
    }
}