namespace Intervals;

/// <summary>
/// A pair of tags, used in the return type of <see cref="Interval{TBoundary,TTag}.TryGetOverlappingInterval{TOtherTag}"/>
/// </summary>
/// <param name="Tag1"></param>
/// <param name="Tag2"></param>
/// <typeparam name="TTag1"></typeparam>
/// <typeparam name="TTag2"></typeparam>
public readonly record struct TagPair<TTag1, TTag2>(TTag1 Tag1, TTag2 Tag2);