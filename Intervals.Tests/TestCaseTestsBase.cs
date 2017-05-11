using System;
using System.Collections.Generic;

namespace Intervals.Tests
{
    public class TestCaseTestsBase
    {
        protected IInterval<int>[] GetIntervals(string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            var result = new List<IInterval<int>>();
            int index = 0;
            int intervalStart = -1;
            int position = 0;

            while (index < input.Length)
            {
                if (input[index] == '?')
                    position = -1; // will be increased to 0 at end of loop
                else if (input[index] == '|')
                {
                    if (intervalStart == -1)
                        intervalStart = position;
                    else
                    {
                        result.Add(new Interval<int>(intervalStart, position));
                        if (index + 1 < input.Length && input[index + 1] == '-')
                            intervalStart = position;
                        else
                            intervalStart = -1;
                    }
                }

                index++;
                position++;
            }

            return result.ToArray();
        }
    }
}