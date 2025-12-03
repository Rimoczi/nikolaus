namespace nikolaus;
internal static class House
{
    public static IReadOnlyList<Line> Lines;

    static House()
    {
        Lines =
        [
            new Line(PointsEnum.LB, PointsEnum.LT),
            new Line(PointsEnum.LT, PointsEnum.T),
            new Line(PointsEnum.T, PointsEnum.RT),
            new Line(PointsEnum.RT, PointsEnum.RB),
            new Line(PointsEnum.RB, PointsEnum.LB),
            new Line(PointsEnum.LB, PointsEnum.RT),
            new Line(PointsEnum.LT, PointsEnum.RB),
            new Line(PointsEnum.LT, PointsEnum.RT)
        ];
    }

    public static List<Line> GetLinesFrom(PointsEnum point)
    {
        return Lines.Where(l => l.A == point || l.B == point).ToList();
    }
}

