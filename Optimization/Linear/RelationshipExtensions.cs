namespace Barbar.SymbolicMath.Optimization.Linear
{
    /// <summary>
    /// Extensions for <see cref="Relationship"/> class
    /// </summary>
    public static class RelationshipExtensions
    {
        /// <summary>
        /// Gets the relationship obtained when multiplying all coefficients by -1.
        /// </summary>
        /// <param name="relationship"></param>
        /// <returns>opposite relationship</returns>
        public static Relationship oppositeRelationship(this Relationship relationship)
        {
            if (relationship == Relationship.LEQ)
            {
                return Relationship.GEQ;
            }
            if (relationship == Relationship.GEQ)
            {
                return Relationship.LEQ;
            }

            return Relationship.EQ;
        }
    }
}
