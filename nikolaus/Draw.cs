namespace nikolaus;
internal class Draw
{
    private List<Drawing> _drawings = [];
    public void DrawHouses()
    {
        foreach (var startPoint in Enum.GetValues<PointsEnum>())
        {
            var drawing = new Drawing(startPoint, startPoint);
            _drawings.Add(drawing);
            ContinueDrawing(drawing);
        }
    }

    private void ContinueDrawing(Drawing drawing)
    {
        var undrawnLines = drawing.GetUndrawnLinesFrom(drawing.CurrentPoint);
        if (undrawnLines.Count == 0)
        {
            Console.WriteLine(drawing);
            return;
        }

        var firstLine = undrawnLines.First();
        undrawnLines.Remove(firstLine);

        foreach (var lineToStartOtherDrawing in undrawnLines)
        {
            var newDrawing = drawing.GetCopy();
            _drawings.Add(newDrawing);
            newDrawing.DrawLine(lineToStartOtherDrawing);
            ContinueDrawing(newDrawing);
        }

        drawing.DrawLine(firstLine);
        ContinueDrawing(drawing);
    }

    public void PrintReport()
    {
        Console.WriteLine();
        Console.WriteLine($"Drawings count: {_drawings.Count}");
        Console.WriteLine($"Succeeded: {_drawings.Count(d => d.IsSucceeded)} ");
        Console.WriteLine($"Fails:  {_drawings.Count(d => !d.IsSucceeded)} ");

        foreach (var startPoint in Enum.GetValues<PointsEnum>())
        {
            var succeededDrawings = _drawings.Count(d => d.IsSucceeded && d.StartPoint == startPoint);
            if (succeededDrawings == 0)
            {
                Console.WriteLine($"Started from {startPoint}: can't draw the house of Nicolaus");
            }
        }
    }
}
