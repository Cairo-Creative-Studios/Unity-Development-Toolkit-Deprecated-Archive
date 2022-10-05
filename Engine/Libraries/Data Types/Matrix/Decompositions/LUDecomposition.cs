using System;

namespace NMatrix.Decompositions
{
    /// <summary>
    /// LU-decomposition.
    /// </summary>
    public class LUDecomposition : IDecomposition<LUResult>
    {
        #region Methods

        /// <summary>
        /// Decomposes an initial matrix into matrices L and U. 
        /// </summary>
        /// <param name="matrix">An initial matrix</param>
        /// <returns>
        /// Returns LU decomposition result.
        /// </returns>
        public LUResult Decompose(Matrix matrix)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException("Original matrix cannot be null");
            }

            if (!matrix.IsSquare)
            {
                throw new NonSquareMatrixException("LU decomposition cannot apply to non-square matrices.");
            }

            var lower = new Matrix(matrix.Rows, matrix.Columns);
            var upper = new Matrix(matrix.Rows, matrix.Columns);

            for (int j = 0; j < matrix.Columns; j++)
            {
                upper[0, j] = matrix[0, j];
                lower[j, 0] = matrix[j, 0] / upper[0, 0];
            }

            for (int i = 1; i < matrix.Rows; i++)
            {
                for (int j = i; j < matrix.Columns; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < i; k++)
                    {
                        sum += lower[i, k] * upper[k, j];
                    }
                    upper[i, j] = matrix[i, j] - sum;

                    sum = 0;

                    for (int k = 0; k < i; k++)
                    {
                        sum += lower[j, k] * upper[k, i];
                    }

                    lower[j, i] = (1 / upper[i, i]) * (matrix[j, i] - sum);
                }
            }

            return new LUResult(lower, upper);
        }

        #endregion
    }
}
