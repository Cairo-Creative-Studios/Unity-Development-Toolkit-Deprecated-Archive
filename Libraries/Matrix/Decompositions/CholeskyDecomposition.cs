using System;

namespace NMatrix.Decompositions
{
    /// <summary>
    /// Represents Cholesky-decomposition.
    /// </summary>
    public class CholeskyDecomposition : IDecomposition<CholeskyResult>
    {
        #region Methods

        /// <summary>
        /// Decomposes an initial matrix into matrices L and L*. 
        /// </summary>
        /// <param name="matrix">An initial matrix</param>
        /// <returns>
        /// Returns Cholesky decomposition result.
        /// </returns>
        public CholeskyResult Decompose(Matrix matrix)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException("Original matrix could not be null.");
            }

            if (!matrix.IsSymmetric)
            {
                throw new NonSymmetricMatrixException(
                    "Cholesky decomposition cannot apply to non-symmetric matrices.");
            }

            var lower = new Matrix(matrix.Rows, matrix.Columns);

            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Columns; j++)
                {
                    var sum = 0d;

                    if (j < i)
                    {
                        for (int k = 0; k < i; k++)
                        {
                            sum += lower[i, k] * lower[j, k];
                        }
                        lower[i, j] = (1 / lower[j, j]) * (matrix[i, j] - sum);
                    }

                    sum = 0d;

                    for (int k = 0; k < i; k++)
                    {
                        sum += lower[i, k] * lower[i, k];
                    }

                    lower[i, i] = Math.Sqrt(matrix[i, i] - sum);
                }
            }

            var lowerTransposed = lower.Transpose();

            return new CholeskyResult(lower, lowerTransposed);
        }

        #endregion
    }
}
