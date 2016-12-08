using Barbar.SymbolicMath.Policies;
using Barbar.SymbolicMath.Rationals;

namespace Barbar.SymbolicMath.Vectors
{
    /// <summary>
    /// Solve system of linear equations using gauss elimination method
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TPolicy"></typeparam>
    public class GaussElimination<T, TPolicy> where TPolicy : IPolicy<T>, new()
    {
        /// <summary>
        /// Solve system of linear equations
        /// </summary>
        /// <param name="rows">
        /// Matrix
        /// M11*x + M12*y + M13*z = M14
        /// M21*x + M22*y + M23*z = M24
        /// M31*x + M32*y + M33*z = M34
        /// </param>
        /// <returns></returns>
        public Rational<T, TPolicy>[] SolveLinearEquations(Matrix<Rational<T, TPolicy>, RationalPolicy<T, TPolicy>> rows)
        {

            for (int i = 0; i < rows.Height - 1; i++)
            {
                if (rows[i,i].IsZero() && !Swap(rows, i, i))
                {
                    return null;
                }

                for (int j = i; j < rows.Height; j++)
                {
                    Rational<T, TPolicy>[] d = new Rational<T, TPolicy>[rows.Width];
                    for (int x = 0; x < rows.Width; x++)
                    {
                        d[x] = rows[x,j];
                        if (!rows[i, j].IsZero())
                        {
                            d[x] = d[x] / rows[i, j];
                        }
                    }
                    rows.SetRow(j, d);
                }

                for (int y = i + 1; y < rows.Height; y++)
                {
                    Rational<T, TPolicy>[] f = new Rational<T, TPolicy>[rows.Width];
                    for (int g = 0; g < rows.Width; g++)
                    {
                        f[g] = rows[g, y];
                        if (!rows[i, y].IsZero())
                        {
                            f[g] = f[g] - rows[g, i];
                        }

                    }
                    rows.SetRow(y, f);
                }
            }

            return CalculateResult(rows);
        }

        private bool Swap(Matrix<Rational<T, TPolicy>, RationalPolicy<T, TPolicy>> rows, int row, int column)
        {
            bool swapped = false;
            for (int z = rows.Height - 1; z > row; z--)
            {
                if (!rows[row, z].IsZero())
                {
                    var temp = rows.GetRow(z);
                    rows.SetRow(z, rows.GetRow(column));
                    rows.SetRow(column, temp);
                    swapped = true;
                }
            }

            return swapped;
        }
        private Rational<T, TPolicy>[] CalculateResult(Matrix<Rational<T, TPolicy>, RationalPolicy<T, TPolicy>> rows)
        {
            Rational<T, TPolicy> val;
            Rational<T, TPolicy>[] result = new Rational<T, TPolicy>[rows.Height];
            for (int i = rows.Height - 1; i >= 0; i--)
            {
                val = rows[rows.Width - 1, i];
                for (int x = rows.Width - 2; x > i - 1; x--)
                {
                    val -= rows[x, i] * result[x];
                }
                result[i] = (val / rows[i, i]).Normalize();

                if (result[i].IsNan())
                {
                    return null;
                }
            }
            return result;
        }
    }
}
