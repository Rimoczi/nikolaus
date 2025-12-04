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

    public void GenerateSvgAnimations(string outputDir)
    {
        Directory.CreateDirectory(outputDir);

        var successfulDrawings = _drawings
            .Where(d => d.IsSucceeded)
            .OrderBy(d => d.ToString())
            .ToList();

        // Generate individual SVG files
        for (int i = 0; i < successfulDrawings.Count; i++)
        {
            var drawing = successfulDrawings[i];
            var svg = SvgGenerator.Generate(drawing, i + 1);
            var fileName = $"house_{i + 1:D3}_{drawing.StartPoint}.svg";
            var filePath = Path.Combine(outputDir, fileName);
            File.WriteAllText(filePath, svg);
        }

        // Generate combined SVG with all houses
        var combinedSvg = SvgGenerator.GenerateCombined(successfulDrawings);
        File.WriteAllText(Path.Combine(outputDir, "all_houses.svg"), combinedSvg);

        Console.WriteLine($"Generated {successfulDrawings.Count} individual SVGs and 'all_houses.svg' in '{outputDir}' folder");
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

        Console.WriteLine($"SUCCESS:");
        var succeed = _drawings.Where(d => d.IsSucceeded).OrderBy(d => d.ToString());
        foreach (var drawing in succeed)
        {
            Console.WriteLine(drawing);
        }
    }
}
