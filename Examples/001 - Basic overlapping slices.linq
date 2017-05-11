<Query Kind="Program">
  <Reference Relative="..\Intervals\bin\Debug\Intervals.dll">C:\dev\vs.net\Intervals\Intervals\bin\Debug\Intervals.dll</Reference>
  <Namespace>Intervals</Namespace>
</Query>

// *****************************************************
// *
// * Calculates and dumps the slices of 3 intervals.
// *
// ***********************

void Main()
{
	var a = 0.IntervalTo(10);
    var b = 5.IntervalTo(15);
    var c = 10.IntervalTo(20);
    
    new[] { a, b, c }.Slice().Dump();
}

// Define other methods and classes here