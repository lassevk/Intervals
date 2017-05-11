namespace Intervals
{
    /// <summary>
    /// This enum is used by the Merge methods of <see cref="IntervalExtensions"/> to specify how to merge intervals.
    /// </summary>
    public enum IntervalMergeBehavior
    {
        /// <summary>
        /// Merge overlapping intervals only, adjacent intervals will appear as separate intervals.
        /// </summary>
        Overlapping,

        /// <summary>
        /// Merge overlapping and adjacent intervals, there must be a gap between two intervals to
        /// make them separate.
        /// </summary>
        OverlappingAndAdjacent,

        /// <summary>
        /// The default is to merge overlapping and adjacent, same as <see cref="OverlappingAndAdjacent"/>.
        /// </summary>
        Default = OverlappingAndAdjacent
    }
}