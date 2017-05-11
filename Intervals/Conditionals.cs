using System;
using System.Diagnostics;
using JetBrains.Annotations;

// ReSharper disable InconsistentNaming

namespace Intervals
{
    internal static class Conditionals
    {
        [Conditional("DEBUG")]
        [ContractAnnotation("false => halt")]
        public static void assume(bool expression)
        {
        }
    }
}
