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
	var a = Interval.Create(0, 10);
    var b = Interval.Create(5, 15);
    var c = Interval.Create(10, 20);
    
    new[] { a, b, c }.Slice().Dump();
}

// Define other methods and classes here