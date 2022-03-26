using WaterTrans.GlyphLoader;
using WaterTrans.GlyphLoader.Geometry;

string fontPath = System.IO.Path.Combine(Environment.CurrentDirectory, "fonts/Roboto-Bold.ttf");
using var fontStream = System.IO.File.OpenRead(fontPath);
// Initialize stream only
Typeface tf = new Typeface(fontStream);

var svg = new System.Text.StringBuilder();
double unit = 100;
double x = 20;
double y = 20;
string text = "ABCD";
svg.AppendLine("<svg width='440' height='140' viewBox='0 0 440 140' xmlns='http://www.w3.org/2000/svg' version='1.1'>");

foreach (char c in text)
{
    // Get glyph index
    ushort glyphIndex = tf.CharacterToGlyphMap[(int)c];

    // Get glyph outline
    var geometry = tf.GetGlyphOutline(glyphIndex, unit);

    // Get advanced width
    double advanceWidth = tf.AdvanceWidths[glyphIndex] * unit;

    // Get advanced height
    double advanceHeight = tf.AdvanceHeights[glyphIndex] * unit;

    // Get baseline
    double baseline = tf.Baseline * unit;

    // Convert to path mini-language
    string miniLanguage = RandomizePath(geometry).Figures.ToString(x, y + baseline);

    svg.AppendLine($"<path d='{miniLanguage}' fill='#46DBC4' stroke='#46DBC4' stroke-width='0' />");
    x += advanceWidth;
}

svg.AppendLine("</svg>");
Console.WriteLine(svg.ToString());

PathGeometry RandomizePath(PathGeometry path)
{
    var newPath = new PathGeometry();
    Random rnd = new Random();
    var r = rnd.NextDouble() * 0.4 - 0.2;
    foreach (var figure in path.Figures)
    {
        var newFigure = new PathFigure();
        foreach (var segment in figure.Segments)
        {
            // Type C
            if (segment is BezierSegment)
            {
                var seg = segment as BezierSegment;
                if (seg != null)
                {
                    seg.Point1 = ShiftPoint(seg.Point1, r);
                    seg.Point2 = ShiftPoint(seg.Point2, r);
                    seg.Point3 = ShiftPoint(seg.Point3, r);
                }
                newFigure.Segments.Add(seg);
            }
            // Type Q
            if (segment is QuadraticBezierSegment)
            {
                var seg = segment as QuadraticBezierSegment;
                if (seg != null)
                {
                    seg.Point1 = ShiftPoint(seg.Point1, r);
                    seg.Point2 = ShiftPoint(seg.Point2, r);
                }
                newFigure.Segments.Add(seg);
            }
            // Type L
            if (segment is LineSegment)
            {
                var seg = segment as LineSegment;
                if (seg != null)
                {
                    seg.Point = ShiftPoint(seg.Point, r);
                }
                newFigure.Segments.Add(seg);
            }

        }
        newFigure.StartPoint = ShiftPoint(figure.StartPoint, r);
        newFigure.IsClosed = figure.IsClosed;
        newFigure.IsFilled = figure.IsFilled;
        newPath.Figures.Add(newFigure);
    }
    return newPath;
}

Point ShiftPoint(Point p, double r)
{
    return new Point(p.X + r, p.Y + r);
}