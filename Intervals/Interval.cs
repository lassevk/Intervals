using System;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Intervals;

/// <summary>
/// This implements a basic interval. Am interval is a span of values that includes the
/// <see cref="Start"/>, but not the <see cref="End"/>, instead including every value up to, but
/// not including the <b>End</b> value.
/// </summary>
/// <typeparam name="TBoundary">
/// The type of values denoting the boundaries of this interval.
/// </typeparam>
/// <typeparam name="TTag">
/// The type of tag associated with this interval.
/// </typeparam>
[DebuggerDisplay("Interval [{Start}, {End}) [{Tag}]")]
public readonly record struct Interval<TBoundary, TTag> : IComparable<Interval<TBoundary, TTag>>
    where TBoundary : struct, IComparable<TBoundary>
{
    /// <summary>
    /// Constructs a new instance of <see cref="Interval{TBoundary,TTag}"/>.
    /// </summary>
    /// <param name="start">
    /// The starting value for the new interval.
    /// </param>
    /// <param name="end">
    /// The ending value for the new interval.
    /// </param>
    /// <param name="tag">
    /// The tag associated with this interval.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <para><paramref name="start"/> has a higher value than <paramref name="end"/>.</para>
    /// </exception>
    [JsonConstructor]
    public Interval(TBoundary start, TBoundary end, TTag tag = default!)
    {
        if (end.CompareTo(start) < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(end), $"end must be greater than start ({start}..{end})");
        }

        Start = start;
        End = end;
        Tag = tag;
    }

    /// <summary>
    /// The starting value of the interval. This value is considered to be part of the interval.
    /// </summary>
    [JsonPropertyName("start")]
    public TBoundary Start { get; }

    /// <summary>
    /// The ending value of the interval. This value is not considered to be part of the interval.
    /// </summary>
    [JsonPropertyName("end")]
    public TBoundary End { get; }

    /// <summary>
    /// The tag associated with this interval. Can be used to store references to objects relevant for this interval.
    /// </summary>
    [JsonPropertyName("tag")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    // [DefaultValue(null)]
    public TTag Tag { get; }

    /// <summary>
    /// Implicitly convert a tuple containing start and end values, and a tag, to an interval.
    /// </summary>
    /// <param name="tuple">
    /// The tuple, containing the start, end, and tag values.
    /// </param>
    /// <returns>
    /// The interval constructed from the individual tuple values.
    /// </returns>
    public static implicit operator Interval<TBoundary, TTag>((TBoundary start, TBoundary end, TTag tag) tuple) => new(tuple.start, tuple.end, tuple.tag);

    /// <summary>
    /// Implicitly convert a tuple containing start and end values to an interval.
    /// </summary>
    /// <param name="tuple">
    /// The tuple, containing the start and end values.
    /// </param>
    /// <returns>
    /// The interval constructed from the individual tuple values.
    /// </returns>
    public static implicit operator Interval<TBoundary, TTag>((TBoundary start, TBoundary end) tuple) => new(tuple.start, tuple.end);

    /// <summary>
    /// Deconstruct this interval into separate boundary and tag variables.
    /// </summary>
    /// <param name="start">
    /// The variable for the <see cref="Start"/> value.
    /// </param>
    /// <param name="end">
    /// The variable for the <see cref="End"/> value.
    /// </param>
    /// <param name="tag">
    /// The variable for the <see cref="Tag"/> value.
    /// </param>
    public void Deconstruct(out TBoundary start, out TBoundary end, out TTag tag) => (start, end, tag) = (Start, End, Tag);

    /// <summary>
    /// Deconstruct this interval into separate boundary variables.
    /// </summary>
    /// <param name="start">
    /// The variable for the <see cref="Start"/> value.
    /// </param>
    /// <param name="end">
    /// The variable for the <see cref="End"/> value.
    /// </param>
    public void Deconstruct(out TBoundary start, out TBoundary end) => (start, end) = (Start, End);

    /// <inheritdoc />
    public int CompareTo(Interval<TBoundary, TTag> other)
    {
        int startComparison = Start.CompareTo(other.Start);
        if (startComparison != 0)
        {
            return startComparison;
        }

        return End.CompareTo(other.End);
    }

    /// <summary>
    /// Determines if the specified value is considered to be part of the interval.
    /// </summary>
    /// <param name="value">
    /// The value to determine whether it is inside the interval or not.
    /// </param>
    /// <returns>
    /// <c>true</c> if the <paramref name="value"/> is considered to be part of the interval,
    /// otherwise, <c>false</c>.
    /// </returns>
    public bool Contains(TBoundary value) => Start.CompareTo(value) <= 0 && End.CompareTo(value) > 0;

    /// <summary>
    /// Determines if the two intervals overlap. Overlap means that there must exist at least one value that
    /// is considered to be part of both intervals.
    /// </summary>
    /// <param name="other">
    /// The interval to compare to this.
    /// </param>
    /// <returns>
    /// <c>true</c> if the two intervals overlap; otherwise <c>false</c>.
    /// </returns>
    public bool IsOverlapping<TOtherTag>(Interval<TBoundary, TOtherTag> other) => Start.CompareTo(other.End) < 0 && End.CompareTo(other.Start) > 0;

    /// <summary>
    /// Attempts to get the overlapping part between two intervals, meaning it will return a new interval that contains the portion
    /// of the two intervals that are in common / overlap. If the two intervals does not overlap this method will return <c>null</c>
    /// instead.
    /// </summary>
    /// <param name="other">
    /// The second interval to compare to this.
    /// </param>
    /// <returns>
    /// A new <see cref="Interval{TBoundary,TTag}"/> containing the overlapping part between the two intervals or
    /// <c>null</c> if the two intervals does not overlap. The tags will be combined into a pair of tags.
    /// </returns>
    public Interval<TBoundary, TagPair<TTag, TOtherTag>>? TryGetOverlappingInterval<TOtherTag>(Interval<TBoundary, TOtherTag> other)
    {
        TBoundary start = Start;
        if (other.Start.CompareTo(start) > 0)
        {
            start = other.Start;
        }

        TBoundary end = End;
        if (other.End.CompareTo(end) < 0)
        {
            end = other.End;
        }

        if (start.CompareTo(end) >= 0)
        {
            return null;
        }

        return Interval.Create(start, end, new TagPair<TTag, TOtherTag>(Tag, other.Tag));
    }

    /// <summary>
    /// Attempts to get the overlapping part between two intervals, meaning it will return a new interval that contains the portion
    /// of the two intervals that are in common / overlap. If the two intervals does not overlap this method will return <c>null</c>
    /// instead.
    /// </summary>
    /// <param name="other">
    /// The second interval to compare to this.
    /// </param>
    /// <param name="tagOperator">
    /// A function that will be called to combine the two tags involved into one for the resulting interval.
    /// </param>
    /// <returns>
    /// A new <see cref="Interval{TBoundary,TTag}"/> containing the overlapping part between the two intervals or
    /// <c>null</c> if the two intervals does not overlap. The tags will be combined into a pair of tags.
    /// </returns>
    public Interval<TBoundary, TResultTag>? TryGetOverlappingInterval<TOtherTag, TResultTag>(Interval<TBoundary, TOtherTag> other, Func<TTag, TOtherTag, TResultTag> tagOperator)
    {
        TBoundary start = Start;
        if (other.Start.CompareTo(start) > 0)
        {
            start = other.Start;
        }

        TBoundary end = End;
        if (other.End.CompareTo(end) < 0)
        {
            end = other.End;
        }

        if (start.CompareTo(end) >= 0)
        {
            return null;
        }

        return Interval.Create(start, end, tagOperator(Tag, other.Tag));
    }

    /// <summary>
    /// Attempts to get the overlapping part between two intervals, meaning it will return a new interval that contains the portion
    /// of the two intervals that are in common / overlap. If the two intervals does not overlap this method will return <c>null</c>
    /// instead.
    /// </summary>
    /// <param name="other">
    /// The second interval to compare to this.
    /// </param>
    /// <param name="tag">
    /// The tag to associate with the resulting interval.
    /// </param>
    /// <returns>
    /// A new <see cref="Interval{TBoundary,TTag}"/> containing the overlapping part between the two intervals or
    /// <c>null</c> if the two intervals does not overlap. The tags will be combined into a pair of tags.
    /// </returns>
    public Interval<TBoundary, TResultTag>? TryGetOverlappingInterval<TOtherTag, TResultTag>(Interval<TBoundary, TOtherTag> other, TResultTag tag)
    {
        TBoundary start = Start;
        if (other.Start.CompareTo(start) > 0)
        {
            start = other.Start;
        }

        TBoundary end = End;
        if (other.End.CompareTo(end) < 0)
        {
            end = other.End;
        }

        if (start.CompareTo(end) >= 0)
        {
            return null;
        }

        return Interval.Create(start, end, tag);
    }

    /// <summary>
    /// Gets the overlapping part between two intervals, meaning it will return a new interval that contains the portion
    /// of the two intervals that are in common / overlap. If the two intervals does not overlap this method will throw
    /// <see cref="InvalidOperationException"/>.
    /// instead.
    /// </summary>
    /// <param name="other">
    /// The second interval to compare.
    /// </param>
    /// <returns>
    /// A new <see cref="Interval{TBoundary,TTag}"/> containing the overlapping part between the two intervals. The tags will be combined.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The two intervals does not overlap.
    /// </exception>
    /// <remarks>
    /// Note that if either or both interval is <c>null</c> this method will also throw <see cref="InvalidOperationException"/>.
    /// </remarks>
    public Interval<TBoundary, TagPair<TTag, TOtherTag>> GetOverlappingInterval<TOtherTag>(Interval<TBoundary, TOtherTag> other)
    {
        Interval<TBoundary, TagPair<TTag, TOtherTag>>? result = TryGetOverlappingInterval(other);
        if (result == null)
        {
            throw new InvalidOperationException($"Unable to get overlapping interval between {this} and {other}");
        }

        return result.Value;
    }

    /// <summary>
    /// Gets the overlapping part between two intervals, meaning it will return a new interval that contains the portion
    /// of the two intervals that are in common / overlap. If the two intervals does not overlap this method will throw
    /// <see cref="InvalidOperationException"/>.
    /// instead.
    /// </summary>
    /// <param name="other">
    /// The second interval to compare.
    /// </param>
    /// <param name="tagOperator">
    /// A function that will be called to combine the tags of the two intervals involved into the tag to associate
    /// with the resulting interval.
    /// </param>
    /// <returns>
    /// A new <see cref="Interval{TBoundary,TTag}"/> containing the overlapping part between the two intervals. The tags will be combined.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The two intervals does not overlap.
    /// </exception>
    /// <remarks>
    /// Note that if either or both interval is <c>null</c> this method will also throw <see cref="InvalidOperationException"/>.
    /// </remarks>
    public Interval<TBoundary, TResultTag> GetOverlappingInterval<TOtherTag, TResultTag>(Interval<TBoundary, TOtherTag> other, Func<TTag, TOtherTag, TResultTag> tagOperator)
    {
        Interval<TBoundary, TResultTag>? result = TryGetOverlappingInterval(other, tagOperator);
        if (result == null)
        {
            throw new InvalidOperationException($"Unable to get overlapping interval between {this} and {other}");
        }

        return result.Value;
    }

    /// <summary>
    /// Gets the overlapping part between two intervals, meaning it will return a new interval that contains the portion
    /// of the two intervals that are in common / overlap. If the two intervals does not overlap this method will throw
    /// <see cref="InvalidOperationException"/>.
    /// instead.
    /// </summary>
    /// <param name="other">
    /// The second interval to compare.
    /// </param>
    /// <param name="tag">
    /// The tag to associate with the resulting interval.
    /// </param>
    /// <returns>
    /// A new <see cref="Interval{TBoundary,TTag}"/> containing the overlapping part between the two intervals. The tags will be combined.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The two intervals does not overlap.
    /// </exception>
    /// <remarks>
    /// Note that if either or both interval is <c>null</c> this method will also throw <see cref="InvalidOperationException"/>.
    /// </remarks>
    public Interval<TBoundary, TResultTag> GetOverlappingInterval<TOtherTag, TResultTag>(Interval<TBoundary, TOtherTag> other, TResultTag tag)
    {
        Interval<TBoundary, TResultTag>? result = TryGetOverlappingInterval(other, tag);
        if (result == null)
        {
            throw new InvalidOperationException($"Unable to get overlapping interval between {this} and {other}");
        }

        return result.Value;
    }

    /// <summary>
    /// Determines if the two intervals are adjacent, meaning that where one interval starts the other ends, or vice versa.
    /// </summary>
    /// <typeparam name="TOtherTag">
    /// The type of tag in the other interval.
    /// </typeparam>
    /// <param name="other">
    /// The second interval to compare to this.
    /// </param>
    /// <returns>
    /// <c>true</c> if the two intervals are ajdacent; otherwise, <c>false</c>.
    /// </returns>
    public bool IsAdjacentTo<TOtherTag>(Interval<TBoundary, TOtherTag> other) => Start.CompareTo(other.End) == 0 || End.CompareTo(other.Start) == 0;

    /// <summary>
    /// Attempts to calculate the union of the overlapping or adjacent intervals, meaning it will return a new interval that
    /// contains the outer boundaries of the two intervals combined. If the two intervals does not overlap
    /// and aren't adjacent this method will return <c>null</c>.
    /// instead.
    /// </summary>
    /// <typeparam name="TOtherTag">
    /// The type of tag associated with the other interval.
    /// </typeparam>
    /// <param name="other">
    /// The second interval to compare.
    /// </param>
    /// <returns>
    /// A new <see cref="Interval{TBoundary,TTag}"/> containing the outermost boundaries of the two overlapping or adjacent
    /// intervals; or <c>null</c> if the two intervals does not overlap and aren't adjacent.
    /// </returns>
    /// <remarks>
    /// Note that if either or both interval is <c>null</c> this method will also return <c>null</c>.
    /// </remarks>
    public Interval<TBoundary, TagPair<TTag, TOtherTag>>? TryGetUnion<TOtherTag>(Interval<TBoundary, TOtherTag> other)
    {
        if (!IsOverlapping(other) && !IsAdjacentTo(other))
        {
            return null;
        }

        return Interval.Create(Min(Start, other.Start), Max(End, other.End), new TagPair<TTag, TOtherTag>(Tag, other.Tag));
    }

    /// <summary>
    /// Attempts to calculate the union of the overlapping or adjacent intervals, meaning it will return a new interval that
    /// contains the outer boundaries of the two intervals combined. If the two intervals does not overlap
    /// and aren't adjacent this method will return <c>null</c>.
    /// instead.
    /// </summary>
    /// <typeparam name="TOtherTag">
    /// The type of tag associated with the other interval.
    /// </typeparam>
    /// <typeparam name="TResultTag">
    /// The type of tag to associate with the resulting interval.
    /// </typeparam>
    /// <param name="other">
    /// The second interval to compare.
    /// </param>
    /// <param name="tagOperator">
    /// A function that will be called to combine the tags of the two intervals involved into the tag to associate
    /// with the resulting interval.
    /// </param>
    /// <returns>
    /// A new <see cref="Interval{TBoundary,TTag}"/> containing the outermost boundaries of the two overlapping or adjacent
    /// intervals; or <c>null</c> if the two intervals does not overlap and aren't adjacent.
    /// </returns>
    /// <remarks>
    /// Note that if either or both interval is <c>null</c> this method will also return <c>null</c>.
    /// </remarks>
    public Interval<TBoundary, TResultTag>? TryGetUnion<TOtherTag, TResultTag>(Interval<TBoundary, TOtherTag> other, Func<TTag, TOtherTag, TResultTag> tagOperator)
    {
        if (!IsOverlapping(other) && !IsAdjacentTo(other))
        {
            return null;
        }

        return Interval.Create(Min(Start, other.Start), Max(End, other.End), tagOperator(Tag, other.Tag));
    }

    /// <summary>
    /// Attempts to calculate the union of the overlapping or adjacent intervals, meaning it will return a new interval that
    /// contains the outer boundaries of the two intervals combined. If the two intervals does not overlap
    /// and aren't adjacent this method will return <c>null</c>.
    /// instead.
    /// </summary>
    /// <typeparam name="TOtherTag">
    /// The type of tag associated with the other interval.
    /// </typeparam>
    /// <typeparam name="TResultTag">
    /// The type of tag to associate with the resulting interval.
    /// </typeparam>
    /// <param name="other">
    /// The second interval to compare.
    /// </param>
    /// <param name="tag">
    /// The tag to associate with the resulting interval.
    /// </param>
    /// <returns>
    /// A new <see cref="Interval{TBoundary,TTag}"/> containing the outermost boundaries of the two overlapping or adjacent
    /// intervals; or <c>null</c> if the two intervals does not overlap and aren't adjacent.
    /// </returns>
    /// <remarks>
    /// Note that if either or both interval is <c>null</c> this method will also return <c>null</c>.
    /// </remarks>
    public Interval<TBoundary, TResultTag>? TryGetUnion<TOtherTag, TResultTag>(Interval<TBoundary, TOtherTag> other, TResultTag tag)
    {
        if (!IsOverlapping(other) && !IsAdjacentTo(other))
        {
            return null;
        }

        return Interval.Create(Min(Start, other.Start), Max(End, other.End), tag);
    }

    /// <summary>
    /// Calculates the union of the overlapping or adjacent intervals, meaning it will return a new interval that
    /// contains the outer boundaries of the two intervals combined. If the two intervals does not overlap
    /// and aren't adjacent this method will throw <see cref="InvalidOperationException"/>.
    /// instead.
    /// </summary>
    /// <typeparam name="TOtherTag">
    /// The type of tag associated with the other interval.
    /// </typeparam>
    /// <param name="other">
    /// The second interval to compare to this.
    /// </param>
    /// <returns>
    /// A new <see cref="Interval{TBoundary,TTag}"/> containing the outermost boundaries of the two overlapping or adjacent
    /// intervals.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The two intervals doesn't overlap and aren't adjacent.
    /// </exception>
    /// <remarks>
    /// Note that if either or both interval is <c>null</c> this method will also throw <see cref="InvalidOperationException"/>.
    /// </remarks>
    public Interval<TBoundary, TagPair<TTag, TOtherTag>> GetUnion<TOtherTag>(Interval<TBoundary, TOtherTag> other)
    {
        Interval<TBoundary, TagPair<TTag, TOtherTag>>? result = TryGetUnion(other);
        if (result == null)
        {
            throw new InvalidOperationException($"Unable to get the union of {this} and {other} as they do not overlap nor are they adjacent");
        }

        return result.Value;
    }

    /// <summary>
    /// Calculates the union of the overlapping or adjacent intervals, meaning it will return a new interval that
    /// contains the outer boundaries of the two intervals combined. If the two intervals does not overlap
    /// and aren't adjacent this method will throw <see cref="InvalidOperationException"/>.
    /// instead.
    /// </summary>
    /// <typeparam name="TOtherTag">
    /// The type of tag associated with the other interval.
    /// </typeparam>
    /// <typeparam name="TResultTag">
    /// The type of tag to associate with the resulting interval.
    /// </typeparam>
    /// <param name="other">
    /// The second interval to compare to this.
    /// </param>
    /// <param name="tagOperator">
    /// A function that will be called to combine the tags of the two intervals involved into the tag to associate
    /// with the resulting interval.
    /// </param>
    /// <returns>
    /// A new <see cref="Interval{TBoundary,TTag}"/> containing the outermost boundaries of the two overlapping or adjacent
    /// intervals.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The two intervals doesn't overlap and aren't adjacent.
    /// </exception>
    /// <remarks>
    /// Note that if either or both interval is <c>null</c> this method will also throw <see cref="InvalidOperationException"/>.
    /// </remarks>
    public Interval<TBoundary, TResultTag> GetUnion<TOtherTag, TResultTag>(Interval<TBoundary, TOtherTag> other, Func<TTag, TOtherTag, TResultTag> tagOperator)
    {
        Interval<TBoundary, TResultTag>? result = TryGetUnion(other, tagOperator);
        if (result == null)
        {
            throw new InvalidOperationException($"Unable to get the union of {this} and {other} as they do not overlap nor are they adjacent");
        }

        return result.Value;
    }

    /// <summary>
    /// Calculates the union of the overlapping or adjacent intervals, meaning it will return a new interval that
    /// contains the outer boundaries of the two intervals combined. If the two intervals does not overlap
    /// and aren't adjacent this method will throw <see cref="InvalidOperationException"/>.
    /// instead.
    /// </summary>
    /// <typeparam name="TOtherTag">
    /// The type of tag associated with the other interval.
    /// </typeparam>
    /// <typeparam name="TResultTag">
    /// The type of tag to associate with the resulting interval.
    /// </typeparam>
    /// <param name="other">
    /// The second interval to compare to this.
    /// </param>
    /// <param name="tag">
    /// The tag to associate with the resulting interval.
    /// </param>
    /// <returns>
    /// A new <see cref="Interval{TBoundary,TTag}"/> containing the outermost boundaries of the two overlapping or adjacent
    /// intervals.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The two intervals doesn't overlap and aren't adjacent.
    /// </exception>
    /// <remarks>
    /// Note that if either or both interval is <c>null</c> this method will also throw <see cref="InvalidOperationException"/>.
    /// </remarks>
    public Interval<TBoundary, TResultTag> GetUnion<TOtherTag, TResultTag>(Interval<TBoundary, TOtherTag> other, TResultTag tag)
    {
        Interval<TBoundary, TResultTag>? result = TryGetUnion(other, tag);
        if (result == null)
        {
            throw new InvalidOperationException($"Unable to get the union of {this} and {other} as they do not overlap nor are they adjacent");
        }

        return result.Value;
    }

    /// <summary>
    /// Determines if the specified interval is empty or not. An empty interval is an interval where there exists no
    /// value at all that is considered part of the interval. Technically this means that <see cref="Start"/>
    /// equals <see cref="End"/>.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the <see cref="Start"/> property equals the <see cref="End"/> property.
    /// </returns>
    public bool IsEmpty() => Start.Equals(End);

    /// <summary>
    /// Compares this interval to the other interval. Note that the tag is not relevant for comparison.
    /// </summary>
    /// <param name="other">
    /// The interval to compare to.
    /// </param>
    /// <returns>
    /// <c>true</c> if the <see cref="Start"/> and <see cref="End"/> properties of the two intervals are equal;
    /// otherwise, <c>false</c>.
    /// </returns>
    public bool Equals(Interval<TBoundary, TTag> other) => Start.Equals(other.Start) && End.Equals(other.End);

    /// <inheritdoc />
    public override int GetHashCode()
    {
        unchecked
        {
            int result = 17;
            result = result * 23 + Start.GetHashCode();
            result = result * 23 + End.GetHashCode();
            return result;
        }
    }

    /// <inheritdoc />
    public override string ToString() => Tag is null ? $"[{Start}, {End})" : $"[{Start}, {End}) [{Tag}]";

    public Interval<TBoundary, TNewTag> Select<TNewTag>(TNewTag tag) => new(Start, End, tag);
    public Interval<TBoundary, TNewTag> Select<TNewTag>(Func<TTag, TNewTag> tagSelector) => new(Start, End, tagSelector(Tag));
    public Interval<TNewBoundary, TTag> Select<TNewBoundary>(Func<TBoundary, TNewBoundary> boundarySelector)
        where TNewBoundary : struct, IComparable<TNewBoundary>
        => new(boundarySelector(Start), boundarySelector(End), Tag);

    private static TBoundary Min(TBoundary a, TBoundary b) => a.CompareTo(b) < 0 ? a : b;
    private static TBoundary Max(TBoundary a, TBoundary b) => a.CompareTo(b) > 0 ? a : b;
}