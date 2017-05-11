<Query Kind="Program">
  <Reference Relative="..\Intervals\bin\Debug\Intervals.dll">C:\dev\vs.net\Intervals\Intervals\bin\Debug\Intervals.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Windows.Forms.dll</Reference>
  <Namespace>Intervals</Namespace>
  <Namespace>System.Windows.Forms</Namespace>
  <Namespace>System.Drawing</Namespace>
</Query>

// *****************************************************
// *
// * This example creates a lot of random rectangles
// * then each time the form is painted, it calculates
// * slices that overlap, both horizontally and vertically,
// * and then draws a grayscale color inside the corresponding
// * rectangle, with a brightness equal to the number of
// * rectangles that overlapped in that area.
// *
// ***********************

const int RectangleCount = 200;
const int Resolution = 10000;

void Main()
{
    Application.Run(new F());
}

public class F : Form
{
    private List<Rectangle> _Rectangles;
    
    public F()
    {
        var r = new Random();
        _Rectangles = new List<Rectangle>();
        for (int index = 0; index < RectangleCount; index++)
        {
            int x = r.Next(Resolution);
            int y = r.Next(Resolution);
            int w = r.Next(Resolution - x - 1) + 1;
            int h = r.Next(Resolution - y - 1) + 1;
            _Rectangles.Add(new Rectangle(x, y, w, h));
        }
        
        _Rectangles = _Rectangles.OrderBy(rr => rr.Left).ToList();
        
        WindowState = FormWindowState.Maximized;
        Paint += PaintForm;
        SizeChanged += (s, e) => Invalidate();
        this.SetStyle(ControlStyles.Opaque, true);
        this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
    }
    
    public void PaintForm(object sender, PaintEventArgs e)
    {
        Form f = (Form)sender;
        e.Graphics.FillRectangle(Brushes.Black, 0, 0, f.Width, f.Height);
        
        var horizontalIntervals =
            from rect in _Rectangles
            let left = Convert.ToInt32((rect.Left / (Resolution + 0.0)) * f.ClientRectangle.Width)
            let right = Convert.ToInt32((rect.Right / (Resolution + 0.0)) * f.ClientRectangle.Width)
            where left < right
            select Interval.Create(left, right, rect);
        var horizontalSlices = horizontalIntervals.Slice();
        
        foreach (var horizontalSlice in horizontalSlices)
        {
            var verticalRectangles =
                from TaggedInterval<int> interval in horizontalSlice.IntervalsInSlice
                select (Rectangle)interval.Tag;
            var verticalIntervals =
                from rect in verticalRectangles
                let top = Convert.ToInt32((rect.Top / (Resolution + 0.0)) * f.ClientRectangle.Height)
                let bottom = Convert.ToInt32((rect.Bottom / (Resolution + 0.0)) * f.ClientRectangle.Height)
                where top < bottom
                select Interval.Create(top, bottom, rect);
            var verticalSlices = verticalIntervals.OrderBy(i => i, IntervalComparer<int>.Default).Slice();
            
            foreach (var verticalSlice in verticalSlices)
            {
                var grayScaleValue = Math.Min(255, Convert.ToInt32((verticalSlice.IntervalsInSlice.Count / (RectangleCount / 5.0)) * 255));
                using (var brush = new SolidBrush(Color.FromArgb(grayScaleValue, grayScaleValue, grayScaleValue)))
                {
                    e.Graphics.FillRectangle(brush,
                        horizontalSlice.Start, verticalSlice.Start,
                        horizontalSlice.GetSpan(), verticalSlice.GetSpan());
                }
            }
        }
    }
}