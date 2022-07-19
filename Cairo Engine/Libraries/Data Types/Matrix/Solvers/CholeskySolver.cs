using System;

using NMatrix.Decompositions;

namespace NMatrix.Solvers
{
    /// <summary>
    /// Represents Cholesky solver.
    /// </summary>
    public sealed class CholeskySolver : ISolver
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
        public Vector Solve(Matrix factor, Vector right)
        {
            if (factor == null)
            {
                throw new ArgumentNullException("Factor matrix cannot be null.");
            }

            if (right == null)
            {
                throw new ArgumentNullException("Right vector cannot be null.");
            }

            if (factor.Rows != right.Size)
            {
                throw new ArgumentException("Factor matrix and right vector have inconsistence size.");
            }

            var decomposition = new CholeskyDecomposition();
            var(l, lt) = decomposition.Decompose(factor);

            var y = SolveLower(l, right);
            var x = SolveUpper(lt, y);

            return x;
        }

        /// <summary>
        /// Solves a system of linear equations, <b>Ly = b</b>.
        /// </summary>
        /// <param name="lower">A lower diagonal matrix.</param>
        /// <param name="b">A right-hand vector b.</param>
        /// <returns>
        /// Returns a vector y as a result of solving a system of linear equations, <b>Ay = b</b>.
        /// </returns>
        private Vector SolveLower(Matrix lower, Vector b)
        {
            var y = new Vector(b.Size);

            for (int i = 0; i < lower.Rows; i++)
            {
                var bi = b[i];
                for (int j = 0; j <= i; j++)
                {
                    if (i == j)
                    {
                        y[i] = bi / lower[i, j];
                    }
                    else
                    {
                        bi -= y[j] * lower[i, j];
                    }
                }
            }

            return y;
        }

        /// <summary>
        /// Solves a system of linear equations, <b>Ux = y</b>.
        /// </summary>
        /// <param name="upper">A lower diagonal matrix.</param>
        /// <param name="y">A right-hand vector y.</param>
        /// <returns>
        /// Returns a vector x as a result of solving a system of linear equations, <b>Ux = y</b>.
        /// </returns>
        private Vector SolveUpper(Matrix upper, Vector y)
        {
            var x = new Vector(y.Size);

            for (int i = upper.Rows - 1; i >= 0; i--)
            {
                var yi = y[i];
                for (int j = upper.Columns - 1; j >= i; j--)
                {
                    if (i == j)
                    {
                        x[i] = yi / upper[i, j];
                    }
                    else
                    {
                        yi -= x[j] * upper[i, j];
                    }
                }
            }

            return x;
        }

        #endregion
    }
}
