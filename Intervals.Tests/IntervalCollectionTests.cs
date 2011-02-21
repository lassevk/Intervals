using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace Intervals.Tests
{
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
            var inputIntervals = new List<IInterval<int>>(GetIntervals(input).Ordered());
            IInterval<int, IInterval<int>[]>[] sliced = inputIntervals.Slice().ToArray();
            CompareInputAndOutput(inputIntervals, output, outputExplanation, sliced);
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
            IInterval<int, IInterval<int>[]>[] merged = inputIntervals.Merge(behavior).ToArray();
            CompareInputAndOutput(inputIntervals, output, outputExplanation, merged);
        }

        private static void CompareInputAndOutput(List<IInterval<int>> inputRanges, string output, string outputExplanation, IInterval<int, IInterval<int>[]>[] resultsOfOperation)
        {
            int index;

            // Disect explanation-string
            var explanation = new Dictionary<string, List<IInterval<int>>>();
            foreach (string explanationPart in outputExplanation.Split('|'))
            {
                if (explanationPart == String.Empty)
                    continue;

                int positionOfEqualSign = explanationPart.IndexOf('=');
                Debug.Assert(positionOfEqualSign > 0, "positionOfEqualSign must be greater than 0 here");

                string letter = explanationPart.Substring(0, positionOfEqualSign);
                var explanationRanges = new List<IInterval<int>>();
                foreach (char digit in explanationPart.Substring(positionOfEqualSign + 1))
                {
                    explanationRanges.Add(inputRanges[digit - '0']);
                }

                explanation[letter] = explanationRanges;
            }

            // Disect output-string
            var outputRanges = new List<IInterval<int, List<IInterval<int>>>>();
            index = 0;
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

                    outputRanges.Add(Interval.Create(startIndex, endIndex, explanation[output.Substring(startIndex, 1)]));
                }
            }

            // Now compare
            Assert.That(resultsOfOperation.Length, Is.EqualTo(outputRanges.Count));
            for (int sliceIndex = 0; sliceIndex < resultsOfOperation.Length; sliceIndex++)
            {
                Assert.That(resultsOfOperation[sliceIndex].Start, Is.EqualTo(outputRanges[sliceIndex].Start));
                Assert.That(resultsOfOperation[sliceIndex].End, Is.EqualTo(outputRanges[sliceIndex].End));
                Assert.That(resultsOfOperation[sliceIndex].Data.Length, Is.EqualTo(outputRanges[sliceIndex].Data.Count));

                foreach (var a in resultsOfOperation[sliceIndex].Data)
                    Assert.That(outputRanges[sliceIndex].Data.Contains(a), Is.True);
                foreach (var a in outputRanges[sliceIndex].Data)
                    Assert.That(resultsOfOperation[sliceIndex].Data.Contains(a), Is.True);
            }
        }

        [Test]
        public void Reduce_NullConverterDelegate_ThrowsArgumentNullException()
        {
            IEnumerable<IInterval<int, int>> Intervals = new IInterval<int, int>[0];
            Converter<int, int> converter = null;
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    IEnumerable<IInterval<int, int>> result = Intervals.Reduce(converter);
                });
        }

        [Test]
        public void Reduce_NullIntervals_ThrowsArgumentNullException()
        {
            IEnumerable<IInterval<int, int>> Intervals = null;
            Converter<int, int> converter = a => a;
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    IEnumerable<IInterval<int, int>> result = Intervals.Reduce(converter);
                });
        }

        [Test]
        public void Reduce_TwoToOne_ProducesRightResult()
        {
            var Intervals = new IInterval<int, int>[]
            {
                Interval.Create(0, 10, 2), Interval.Create(1, 11, 0), Interval.Create(2, 12, 3), Interval.Create(3, 13, 2),
            };

            IInterval<int, int>[] reduced = Intervals.Reduce((int i) => (i == 2) ? 1 : i).ToArray();

            Assert.That(reduced.Length, Is.EqualTo(4));

            Assert.That(reduced[0].Equals(Intervals[0]), Is.True);
            Assert.That(reduced[1].Equals(Intervals[1]), Is.True);
            Assert.That(reduced[2].Equals(Intervals[2]), Is.True);
            Assert.That(reduced[3].Equals(Intervals[3]), Is.True);

            Assert.That(reduced[0].Data, Is.EqualTo(1));
            Assert.That(reduced[1].Data, Is.EqualTo(0));
            Assert.That(reduced[2].Data, Is.EqualTo(3));
            Assert.That(reduced[3].Data, Is.EqualTo(1));
        }

        [Test]
        public void Slice_NoIntervals_EmptyResult()
        {
            IEnumerable<IInterval<int>> input = new IInterval<int>[0];
            IEnumerable<IInterval<int, IInterval<int>[]>> output = input.Slice();

            Assert.That(output.ToArray().Length, Is.EqualTo(0));
        }

        [Test]
        public void TestSlice_Null()
        {
            IEnumerable<IInterval<int>> collection = null;

            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    IEnumerable<IInterval<int, IInterval<int>[]>> x = collection.Slice();
                });
        }
    }
}