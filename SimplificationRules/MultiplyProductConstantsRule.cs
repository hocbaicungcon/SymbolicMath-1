using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barbar.SymbolicMath.SimplificationRules
{
    internal class MultiplyProductConstantsRule : TransformationRule<Multiply>
    {

        public override SymMathNode Apply(Multiply mul)
        {
            var product = Constant.Factory.Create(1);
            var result = new Multiply();

            foreach (var node in GetDifferentNodes(mul))
            {
                var constant = node as Constant;
                if (constant != null)
                {
                    product *= constant;
                    continue;
                }
                if (result.A == null)
                {
                    result.A = node;
                    continue;
                }
                if (result.B == null)
                {
                    result.B = node;
                    continue;
                }
                result = new Multiply(result, null);
            }

            if (result.A == null && result.B == null)
            {
                return product;
            }

            if (result.B == null)
            {
                result.B = product;
                return result;
            }

            return new Multiply(result, product);
        }

        public override bool IsApplicable(Multiply mul)
        {
            return GetDifferentNodes(mul).OfType<Constant>().Count() > 1;
        }
    }
}
