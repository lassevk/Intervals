using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

// ReSharper disable RedundantEnumerableCastCall
// ReSharper disable TestFileNameWarning
// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable PossibleNullReferenceException
// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable IteratorMethodResultIsIgnored
// ReSharper disable CyclomaticComplexity

namespace Intervals.Tests
{
    using System.Collections;

    [TestFixture]
    public class IntervalCollectionTests : TestCaseTestsBase
    {
        [TestCase(01,
            "   |-0----|    |-3--|     ?" +
            "          |-2--|          ?" +
            "      |-1--------|        ?",
            "   aaabbbbcccccddeee      ",
            "a=0|b=01|c=12|d=13|e=3")]
        [TestCase(02,
            "   |-0------| |-2-----|   ?" +
            "   |-1----------------|   ?",
            "   aaaaaaaaabbcccccccc    ",
            "a=01|b=1|c=12")]
        [TestCase(03,
            "      |-1-------------|           ?" +
            "         |-2--------------|       ?" +
            "            |-3----|              ?" +
            "               |-4------------|   ?" +
            "   |-0----------|                  ",
            "   aaabbbcccdddefffggghhhhiiii     ",
            "a=0|b=01|c=012|d=0123|e=01234|f=1234|g=124|h=24|i=4")]
        [TestCase(04,
            "|-0-------|               ?" +
            "          |-2---------|   ?" +
            "     |-1-------|           ",
            "aaaaabbbbbcccccddddddd     ",
            "a=0|b=01|c=12|d=2")]
        [TestCase(05,
            "|-0---------|            ?" +
            "         |-1---------|    ",
            "aaaaaaaaabbbccccccccc     ",
            "a=0|b=01|c=1")]
        [TestCase(06,
            "|-0---------------|   ?" +
            "       |-1--------|    ",
            "aaaaaaabbbbbbbbbbb     ",
            "a=0|b=01")]
        [TestCase(07,
            "       |-1--------|   ?" +
            "|-0---------------|   ?",
            "aaaaaaabbbbbbbbbbb     ",
            "a=0|b=01")]
        [TestCase(08,
            "|-1---------------|   ?" +
            "|-0------|             ",
            "aaaaaaaaabbbbbbbbb     ",
            "a=01|b=1")]
        [TestCase(09,
            "|-0------|            ?" +
            "|-1---------------|   ?",
            "aaaaaaaaabbbbbbbbb     ",
            "a=01|b=1")]
        [TestCase(10,
            "|-0---------------|   ?" +
            "|-1---------------|   ?",
            "aaaaaaaaaaaaaaaaaa     ",
            "a=01")]
        [TestCase(11,
            "|-0---------------|    ",
            "aaaaaaaaaaaaaaaaaa     ",
            "a=0")]
        [TestCase(12, "                       ", "                       ", "")]
        public void SlicePatterns(int testIndex, string input, string output, string outputExplanation)
        {
            var inputIntervals = new List<IInterval<int>>(GetIntervals(input).OrderBy(i => i, IntervalComparer<int>.Default));
            Slice<int>[] slices = inputIntervals.Slice().OfType<Slice<int>>().ToArray();
            CompareInputAndOutput(inputIntervals, output, outputExplanation, slices);
        }

        [TestCase(01,
            "    |-0----|              ?" +
            "        |-1-----|          ",
            "    aaaaaaaaaaaa           ",
            "a=01",
            IntervalMergeBehavior.Default)]
        [TestCase(02,
            "    |-0----|              ?" +
            "           |-1-----|       ",
            "    aaaaaaaaaaaaaaa        ",
            "a=01",
            IntervalMergeBehavior.Default)]
        [TestCase(03,
            "    |-0----|              ?" +
            "            |-1-----|      ",
            "    aaaaaaa bbbbbbbb       ",
            "a=0|b=1",
            IntervalMergeBehavior.Default)]
        [TestCase(04,
            "    |-0----|              ?" +
            "           |-1-----|       ",
            "    aaaaaaabbbbbbbb        ",
            "a=0|b=1",
            IntervalMergeBehavior.Overlapping)]
        public void MergePatterns(int testIndex, string input, string output, string outputExplanation, IntervalMergeBehavior behavior)
        {
            List<IInterval<int>> inputIntervals = GetIntervals(input).ToList();
            var slices = inputIntervals.Merge(behavior).OfType<Slice<int>>().ToArray();
            CompareInputAndOutput(inputIntervals, output, outputExplanation, slices);
        }

        private static void CompareInputAndOutput(List<IInterval<int>> inputRanges, string output, string outputExplanation, Slice<int>[] resultsOfOperation)
        {
            // Disect explanation-string
            var explanation = new Dictionary<string, List<IInterval<int>>>();
            foreach (string explanationPart in outputExplanation.Split('|'))
            {
                if (explanationPart == String.Empty)
                    continue;

                int positionOfEqualSign = explanationPart.IndexOf('=');
                Debug.Assert(positionOfEqualSign > 0, "positionOfEqualSign must be greater than 0 here");

                string letter = explanationPart.Substring(0, positionOfEqualSign);
                var explanationRanges = explanationPart.Substring(positionOfEqualSign + 1).Select(digit => inputRanges[digit - '0']).ToList();

                explanation[letter] = explanationRanges;
            }

            // Disect output-string
            var outputRanges = new List<Slice<int>>();
            var index = 0;
            while (index < output.Length)
            {
                if (output[index] == ' ')
                    index++;
                else
                {
                    int startIndex = index;
                    int endIndex = index;
                    index++;
                    while (index < output.Length)
                    {
                        if (output[index] != output[startIndex])
                        {
                            endIndex = index;
                            break;
                        }
                        else
                            index++;
                    }

                    outputRanges.Add(new Slice<int>(startIndex, endIndex, explanation[output.Substring(startIndex, 1)]));
                }
            }

            // Now compare
            Assert.That(resultsOfOperation.Length, Is.EqualTo(outputRanges.Count));
            for (int sliceIndex = 0; sliceIndex < resultsOfOperation.Length; sliceIndex++)
            {
                Assert.That(resultsOfOperation[sliceIndex], Is.EqualTo(outputRanges[sliceIndex]).Using(IntervalEqualityComparer<int>.Default));
                CollectionAssert.AreEquivalent(resultsOfOperation[sliceIndex].IntervalsInSlice, outputRanges[sliceIndex].IntervalsInSlice);
            }
        }

        [Test]
        public void Slice_NoIntervals_EmptyResult()
        {
            IEnumerable<Interval<int>> input = new Interval<int>[0];
            IEnumerable<Slice<int>> output = input.Slice();

            Assert.That(output.ToArray().Length, Is.EqualTo(0));
        }

        [Test]
        public void TestSlice_Null()
        {
            IEnumerable<Interval<int>> collection = null;

            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    collection.Slice();
                });
        }
    }
}