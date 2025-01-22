using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

// ReSharper disable InconsistentNaming

namespace Intervals;

internal static class Conditionals
{
    [Conditional("DEBUG")]
    public static void assume([DoesNotReturnIf(false)] bool expression)
    {
    }
}