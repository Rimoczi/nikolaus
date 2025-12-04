using System.Globalization;

namespace nikolaus;

internal static class SvgGenerator
{
    private static readonly Dictionary<PointsEnum, (double X, double Y)> Coordinates = new()
    {
        { PointsEnum.LB, (30, 150) },
        { PointsEnum.RB, (170, 150) },
        { PointsEnum.LT, (30, 70) },
        { PointsEnum.RT, (170, 70) },
        { PointsEnum.T, (100, 10) }
    };

    private const double StartDelay = 1.0;
    private const double LineDuration = 0.625; // 5 seconds total for 8 lines
    private const string StrokeColor = "#333";
    private const double StrokeWidth = 3;
    private const double HouseWidth = 200;
    private const double HouseHeight = 175;

    public static string Generate(Drawing drawing, int number)
    {
        var content = GenerateHouseContent(drawing, number, 0, 0, 0);

        return $"""
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 {HouseWidth} {HouseHeight}" width="400" height="320">
              <rect width="{HouseWidth}" height="{HouseHeight}" fill="#f5f5f5"/>
              {content}
            </svg>
            """;
    }

    public static string GenerateCombined(List<Drawing> drawings, int columns = 11)
    {
        int rows = (int)Math.Ceiling(drawings.Count / (double)columns);
        double totalWidth = columns * HouseWidth;
        double totalHeight = rows * HouseHeight;

        var houses = new List<string>();

        for (int i = 0; i < drawings.Count; i++)
        {
            int col = i % columns;
            int row = i / columns;
            double offsetX = col * HouseWidth;
            double offsetY = row * HouseHeight;

            var houseContent = GenerateHouseContent(drawings[i], i + 1, offsetX, offsetY, 0);
            houses.Add($"""
                <g transform="translate({F(offsetX)},{F(offsetY)})">
                  <rect width="{HouseWidth}" height="{HouseHeight}" fill="{(row % 2 == col % 2 ? "#f5f5f5" : "#e8e8e8")}"/>
                  {houseContent}
                </g>
                """);
        }

        return $"""
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 {F(totalWidth)} {F(totalHeight)}" width="{totalWidth}" height="{totalHeight}">
              {string.Join("\n  ", houses)}
            </svg>
            """;
    }

    private static string GenerateHouseContent(Drawing drawing, int number, double offsetX, double offsetY, double timeOffset)
    {
        var path = $"{drawing.StartPoint} → {string.Join(" → ", drawing.Points)}";
        var elements = new List<string>
        {
            $"""<text x="10" y="20" font-family="Arial, sans-serif" font-size="14" fill="#666">#{number}</text>""",
            $"""<text x="100" y="170" font-family="Arial, sans-serif" font-size="8" fill="#888" text-anchor="middle">{path}</text>"""
        };

        // Add red dots for each point
        foreach (var (point, (x, y)) in Coordinates)
        {
            elements.Add($"""<circle cx="{F(x)}" cy="{F(y)}" r="4" fill="#c33"/>""");
        }

        double currentTime = timeOffset + StartDelay;
        var currentPoint = drawing.StartPoint;

        foreach (var line in drawing.Lines)
        {
            var (x1, y1) = Coordinates[line.A];
            var (x2, y2) = Coordinates[line.B];

            if (line.B == currentPoint)
            {
                (x1, y1, x2, y2) = (x2, y2, x1, y1);
                currentPoint = line.A;
            }
            else
            {
                currentPoint = line.B;
            }

            var length = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
            elements.Add(GenerateLine(x1, y1, x2, y2, length, currentTime));
            currentTime += LineDuration;
        }

        return string.Join("\n  ", elements);
    }

    private static string GenerateLine(double x1, double y1, double x2, double y2, double length, double beginTime)
    {
        return $"""
            <line x1="{F(x1)}" y1="{F(y1)}" x2="{F(x2)}" y2="{F(y2)}"
                  stroke="{StrokeColor}" stroke-width="{StrokeWidth}" stroke-linecap="round"
                  stroke-dasharray="{F(length)}" stroke-dashoffset="{F(length)}">
              <animate attributeName="stroke-dashoffset" from="{F(length)}" to="0"
                       dur="{F(LineDuration)}s" begin="{F(beginTime)}s" fill="freeze"/>
            </line>
            """;
    }

    private static string F(double value) => value.ToString("F1", CultureInfo.InvariantCulture);
}
