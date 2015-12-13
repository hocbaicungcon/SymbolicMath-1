using System.Linq;

namespace Barbar.SymbolicMath.SimplificationRules
{
    internal sealed class AddSumConstantsRule : TransformationRule<Add>
    {
        public override SymMathNode Apply(Add add)
        {
            var sum = Constant.Factory.Create(0);
            var result = new Add();

            foreach (var node in GetDifferentNodes(add))
            {
                var constant = node as Constant;
                if (constant != null)
                {
                    sum += constant;
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
                result = new Add(result, null);
            }

            if (result.A == null && result.B == null)
            {
                return sum;
            }

            if (result.B == null)
            {
                result.B = sum;
                return result;
            }

            return new Add(result, sum);
        }

        public override bool IsApplicable(Add add)
        {
            return GetDifferentNodes(add).OfType<Constant>().Count() > 1;
        }
    }
}
