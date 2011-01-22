using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Intervals.Tests
{
    public class TestCaseTestsBase
    {
        protected IInterval<int>[] GetIntervals(string input)
        {
            var result = new List<IInterval<int>>();
            int index = 0;
            int intervalStart = -1;

            while (index < input.Length)
            {
                if (input[index] == '|')
                {
                    if (intervalStart == -1)
                        intervalStart = index;
                    else
                    {
                        result.Add(new Interval<int>(intervalStart, index));
                        if (index + 1 < input.Length && input[index + 1] == '-')
                            intervalStart = index;
                        else
                            intervalStart = -1;
                    }
                }

                index++;
            }

            return result.ToArray();
        }
    }
}