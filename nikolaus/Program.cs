using nikolaus;

Console.WriteLine("Start to drawing the houses of Nikolaus");
var draw = new Draw();
draw.DrawHouses();
draw.PrintReport();
draw.GenerateSvgAnimations("output");
Console.WriteLine("Finished drawing the houses of Nikolaus");
