namespace NMatrix.Solvers
{
    /// <summary>
    /// Represents a solver of a system of linear equations.
    /// </summary>
    public interface ISolver
    {
        #region Methods

        /// <summary>
        /// Solves a system of linear equations, <b>Ax = b</b>.
        /// </summary>
        /// <param name="factor">A coefficient matrix A.</param>
        /// <param name="right">A right-hand vector b.</param>
        /// <returns>
        /// Returns a vector x that is a solution of the system of linear equations, <b>Ax = b</b>.
        /// </returns>
        Vector Solve(Matrix factor, Vector right);

        #endregion
    }
}
