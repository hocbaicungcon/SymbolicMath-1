namespace Barbar.SymbolicMath.Optimization.Linear
{

    /// <summary>
    /// Types of relationships between two cells in a Solver
    /// </summary>
    public class Relationship
    {
        /// <summary>
        /// Equality relationship.
        /// </summary>
        public static readonly Relationship EQ = new Relationship("=");

        /// <summary>
        /// Lesser than or equal relationship.
        /// </summary>
        public static readonly Relationship LEQ = new Relationship("<=");

        /// <summary>
        /// Greater than or equal relationship.
        /// </summary>
        public static readonly Relationship GEQ = new Relationship(">=");

        private string stringValue;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="stringValue"></param>
        private Relationship(string stringValue)
        {
            this.stringValue = stringValue;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return stringValue;
        }
    }
}