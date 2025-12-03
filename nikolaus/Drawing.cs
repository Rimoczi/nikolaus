namespace nikolaus;
internal class Drawing(PointsEnum startPoint, PointsEnum currentPoint)
{
    public PointsEnum StartPoint => startPoint;
    public PointsEnum CurrentPoint => currentPoint;
    public List<Line> Lines = [];
    public List<PointsEnum> Points = [];
    public bool IsSucceeded => Lines.Count == 8 && Lines.DistinctBy(l => (Start: l.A, End: l.B)).Count() == 8;

    public void DrawLine(Line line)
    {
        if (Lines.Contains(line)) throw new ArgumentException("line is already drawn!");
        Lines.Add(line);
        currentPoint = currentPoint == line.A ? line.B : line.A;
        Points.Add(currentPoint);
    }

    public Drawing GetCopy()
    {
        return new Drawing(startPoint, currentPoint)
        {
            Lines = [.. Lines],
            Points = [.. Points]
        };
    }

    public List<Line> GetUndrawnLinesFrom(PointsEnum point)
    {
        var linesFromPoint = House.GetLinesFrom(point);
        return linesFromPoint.Where(l => !Lines.Contains(l)).ToList();
    }

    public override string ToString()
    {
        var path = string.Join(" -> ", Points);
        return $"{(IsSucceeded ? "SUCCESS" : "FAIL")}: {startPoint} -> {path}";
    }
}

