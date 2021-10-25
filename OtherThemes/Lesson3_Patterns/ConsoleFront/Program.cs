using System;

using static System.Console;

WriteLine("Chose a shape");
var shapes = new (string name, Func<Shape> ctor)[]
{
    ("Rectangle", () => new Rectangle()),
    ("Circle", () => new Circle())
};

for (var i = 0; i < shapes.Length; i++)
{
    WriteLine($"{i + 1}. {shapes[i].name}");
}

var shapeNumber = GetInteger(1, shapes.Length, "Choose number of shape");

var shape = shapes[shapeNumber - 1].ctor();

var drawer = new ConsoleDrawer('*');

shape.Apply(drawer);

ReadLine();


int GetInteger(int min, int max, string repeatMessage)
{
    while (true)
    {
        var input = ReadLine();
        if (int.TryParse(input, out var number) && number >= min && number <= max)
        {
            return number;
        }

        WriteLine(repeatMessage);
    }
}

internal abstract class Shape
{
    public abstract void Apply(Drawer drawer);
}

internal class Rectangle : Shape
{
    public int Width { get; set; }
    public int Height { get; set; }
    public int X { get; set; }

    public Rectangle()
    {
        Width = 20;
        Height = 4;
        X = 8;
    }

    public override void Apply(Drawer drawer)
    {
        drawer.DrawRectangle(this);
    }
}

internal class Circle : Shape
{
    public double Radius { get; set; }
    public double Thickness { get; set; }

    public Circle()
    {
        Radius = 4;
        Thickness = 1;
    }

    public override void Apply(Drawer drawer)
    {
        drawer.DrawCircle(this);
    }
}

internal abstract class Drawer
{
    public abstract void DrawRectangle(Rectangle rectangle);
    public abstract void DrawCircle(Circle circle);
}

internal class ConsoleDrawer : Drawer
{
    private readonly char _symbol;

    public ConsoleDrawer(char symbol)
    {
        _symbol = symbol;
    }

    public override void DrawRectangle(Rectangle rectangle)
    {
        string s = _symbol.ToString();
        string space = "";
        string temp = "";
        for (int i = 0; i < rectangle.Width; i++)
        {
            space += " ";
            s += _symbol;
        }

        for (int j = 0; j < rectangle.X; j++)
            temp += " ";

        s += _symbol + "\n";

        for (int i = 0; i < rectangle.Height; i++)
            s += temp + _symbol + space + _symbol + "\n";

        s += temp + _symbol;
        for (int i = 0; i < rectangle.Width; i++)
            s += "═";

        s += _symbol + "\n";

        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.CursorLeft = rectangle.X;
        Console.Write(s);
        Console.ResetColor();
    }

    public override void DrawCircle(Circle circle)
    {
        var radius = circle.Radius;
        var thickness = circle.Thickness;
        double rIn = radius - thickness, rOut = radius + thickness;

        for (double y = radius; y >= -radius; --y)
        {
            for (double x = -radius; x < rOut; x += 0.5)
            {
                double value = x * x + y * y;
                if (value >= rIn * rIn && value <= rOut * rOut)
                {
                    Write(_symbol);
                }
                else
                {
                    Write(" ");
                }
            }
            WriteLine();
        }
    }
}