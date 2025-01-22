namespace Intervals;

/// <summary>
/// A pair of tags, used in the return type of <see cref="Interval{TBoundary,TTag}.TryGetOverlappingInterval{TOtherTag}"/>
/// </summary>
/// <param name="Tag1">
/// The tag from the first interval.
/// </param>
/// <param name="Tag2">
/// The tag from the second interval.
/// </param>
/// <typeparam name="TTag1">
/// The type of tag associated with the first interval.
/// </typeparam>
/// <typeparam name="TTag2">
/// The type of tag associated with the second interval.
/// </typeparam>
public readonly record struct TagPair<TTag1, TTag2>(TTag1 Tag1, TTag2 Tag2);