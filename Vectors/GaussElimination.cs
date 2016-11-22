using Barbar.SymbolicMath.Policies;
using Barbar.SymbolicMath.Rationals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barbar.SymbolicMath.Vectors
{
    public class GaussElimination<T, TPolicy> where TPolicy : IPolicy<T>, new()
    {

        public Rational<T, TPolicy>[] SolveLinearEquations(Matrix<Rational<T, TPolicy>> rows)
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

        private bool Swap(Matrix<Rational<T, TPolicy>> rows, int row, int column)
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
        private Rational<T, TPolicy>[] CalculateResult(Matrix<Rational<T, TPolicy>> rows)
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
