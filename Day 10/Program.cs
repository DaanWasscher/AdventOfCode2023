﻿using System.Drawing;

var grid = GetInput();
var startingPoint = GetStart(grid);

long steps = 1;
Point startToSouthOrEastCurrentPoint = new Point(startingPoint.X, startingPoint.Y + 1);
Point startToSouthOrEastPreviousPoint = startingPoint;

Point startToNorthOrWestCurrentPoint = new Point(startingPoint.X, startingPoint.Y - 1);
Point startToNorthOrWestPreviousPoint = startingPoint;

// part 2 new grid double size 280x280
var grid3 = GetDoubledGrid();

//starting point and initial step in both directions are pre-filled in the new grid
grid3[startingPoint.Y * 2][startingPoint.X * 2] = 'X';
grid3[(startingPoint.Y * 2) - 1][startingPoint.X * 2] = 'X';
grid3[(startingPoint.Y * 2) + 1][startingPoint.X * 2] = 'X';
grid3[startToNorthOrWestCurrentPoint.Y * 2][startToNorthOrWestCurrentPoint.X * 2] = 'X';
grid3[startToSouthOrEastCurrentPoint.Y * 2][startToSouthOrEastCurrentPoint.X * 2] = 'X';

do
{
    var startToNorthOrWestNextPoint = GetNextPoint(startToNorthOrWestCurrentPoint, startToNorthOrWestPreviousPoint);
    startToNorthOrWestPreviousPoint = startToNorthOrWestCurrentPoint;
    startToNorthOrWestCurrentPoint = startToNorthOrWestNextPoint;
    grid3[startToNorthOrWestCurrentPoint.Y * 2][startToNorthOrWestCurrentPoint.X * 2] = grid[startToNorthOrWestCurrentPoint.Y][startToNorthOrWestCurrentPoint.X];
    var grid3Filler = GetDoubledGridFiller(startToNorthOrWestPreviousPoint, startToNorthOrWestCurrentPoint);
    grid3[grid3Filler.point.Y][grid3Filler.point.X] = grid3Filler.fillerChar;

    var startToSouthOrEastNextPoint = GetNextPoint(startToSouthOrEastCurrentPoint, startToSouthOrEastPreviousPoint);
    startToSouthOrEastPreviousPoint = startToSouthOrEastCurrentPoint;
    startToSouthOrEastCurrentPoint = startToSouthOrEastNextPoint;
    grid3[startToSouthOrEastCurrentPoint.Y * 2][startToSouthOrEastCurrentPoint.X * 2] = grid[startToSouthOrEastCurrentPoint.Y][startToSouthOrEastCurrentPoint.X];
    grid3Filler = GetDoubledGridFiller(startToSouthOrEastPreviousPoint, startToSouthOrEastCurrentPoint);
    grid3[grid3Filler.point.Y][grid3Filler.point.X] = grid3Filler.fillerChar;

    steps++;
}
while (startToNorthOrWestCurrentPoint != startToSouthOrEastCurrentPoint);

(Point point, char fillerChar) GetDoubledGridFiller(Point start, Point end)
{
    if (start.X == end.X)
    {
        // vertical movement
        if (start.Y > end.Y)
        {
            // move north
            return (new Point(start.X * 2, (start.Y * 2) - 1), '|');
        }
        else
        {
            // move south
            return (new Point(start.X * 2, (start.Y * 2) + 1), '|');
        }
    }
    else
    {
        // horizontal movement
        if (start.X > end.X)
        {
            // move west
            return (new Point((start.X * 2) - 1, start.Y * 2), '-');
        }
        else
        {
            // move east
            return (new Point((start.X * 2) + 1, start.Y * 2), '-');
        }
    }
}

// part 1
Console.WriteLine($"Steps: {steps}");

// part 2
int enclosedArea = 0;

var grid3Colors = GetDoubledColorGrid();
//cleanup starts at the edges and works its way inwards and removes all connecting filler chars
//the remaining filler chars are truly enclosed in the plot
Cleanup(grid3, grid3Colors);

// count . chars, those are the enclosed area but only on even lines and columns (to undo the doubling of the grid)
for (int i = 0; i < 280; i++)
{
    for (int ii = 0; ii < 280; ii++)
    {
        if (i % 2 == 0 && ii % 2 == 0)
        {
            if (grid3[i][ii] == '.')
            {
                enclosedArea++;
            }
        }
    }
}

Console.WriteLine($"Enclosed area: {enclosedArea}");

char[][] GetDoubledGrid()
{
    var doubledGrid = new char[280][];
    for (int i = 0; i < 280; i++)
    {
        doubledGrid[i] = string.Empty.PadRight(280, '.').ToCharArray();
    }

    return doubledGrid;
}

ConsoleColor[][] GetDoubledColorGrid()
{
    var doubledGrid = new ConsoleColor[280][];
    for (int i = 0; i < 280; i++)
    {
        doubledGrid[i] = new ConsoleColor[280];
    }

    return doubledGrid;
}

void Cleanup(char[][] grid, ConsoleColor[][] coloredGrid)
{
    var cleanedSomething = false;
    int gridsize = grid.Length;
    int color = 2;
    int runs = 0;
    do
    {
        cleanedSomething = false;

        // cleanup top to bottom
        for (int i = 0; i < gridsize; i++)
        {
            for (int ii = 0; ii < gridsize; ii++)
            {
                if ((i == 0 || grid[i - 1][ii] == 'o') && grid[i][ii] == '.')
                {
                    grid[i][ii] = 'o';
                    coloredGrid[i][ii] = (ConsoleColor)color;
                    cleanedSomething = true;
                }
            }
        }

        // cleanup left to right
        for (int ii = 0; ii < gridsize; ii++)
        {
            for (int i = 0; i < gridsize; i++)
            {
                if ((ii == 0 || grid[i][ii - 1] == 'o') && grid[i][ii] == '.')
                {
                    grid[i][ii] = 'o';
                    coloredGrid[i][ii] = (ConsoleColor)color;
                    cleanedSomething = true;
                }
            }
        }

        // cleanup bottom to top
        for (int i = gridsize - 1; i >= 0; i--)
        {
            for (int ii = gridsize - 1; ii >= 0; ii--)
            {
                if ((i == gridsize - 1 || grid[i + 1][ii] == 'o') && grid[i][ii] == '.')
                {
                    grid[i][ii] = 'o';
                    coloredGrid[i][ii] = (ConsoleColor)color;
                    cleanedSomething = true;
                }
            }
        }

        // right to left 
        for (int ii = gridsize - 1; ii >= 0; ii--)
        {
            for (int i = gridsize - 1; i >= 0; i--)
            {
                if ((ii == gridsize - 1 || grid[i][ii + 1] == 'o') && grid[i][ii] == '.')
                {
                    grid[i][ii] = 'o';
                    coloredGrid[i][ii] = (ConsoleColor)color;
                    cleanedSomething = true;
                }
            }
        }

        color++;
        if (color > 4)
        {
            color = 2;
        }
        runs++;


    } while (cleanedSomething);


    PrintGrid(grid, coloredGrid);
}


void PrintGrid(char[][] grid, ConsoleColor[][]? gridColor = null)
{
    for (int i = 0; i < grid.Length; i++)
    {
        for (int ii = 0; ii < grid[i].Length; ii++)
        {
            if (gridColor is not null)
            {
                Console.ForegroundColor = gridColor[i][ii] == 0 ? ConsoleColor.Black : (ConsoleColor)gridColor[i][ii];
            }

            if (Console.ForegroundColor == ConsoleColor.Black && grid[i][ii] != '.')
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write('█');
            }
            else
            {
                Console.Write(grid[i][ii]);
            }
            //Console.Write(grid[i][ii]);
        }
        Console.WriteLine();
    }

    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine();
    Console.WriteLine("--------------------------------------------------------------------");
    Console.WriteLine();
}

Point GetNextPoint(Point currentPoint, Point previousPoint)
{
    var currentPointChar = grid[currentPoint.Y][currentPoint.X];
    if (currentPointChar == '|')
    {
        var northPoint = new Point(currentPoint.X, currentPoint.Y - 1);
        var southPoint = new Point(currentPoint.X, currentPoint.Y + 1);
        return previousPoint == northPoint ? southPoint : northPoint;
    }

    if (currentPointChar == '-')
    {
        var westPoint = new Point(currentPoint.X - 1, currentPoint.Y);
        var eastPoint = new Point(currentPoint.X + 1, currentPoint.Y);
        return previousPoint == westPoint ? eastPoint : westPoint;
    }

    if (currentPointChar == 'L')
    {
        var northPoint = new Point(currentPoint.X, currentPoint.Y - 1);
        var eastPoint = new Point(currentPoint.X + 1, currentPoint.Y);
        return previousPoint == northPoint ? eastPoint : northPoint;
    }

    if (currentPointChar == 'J')
    {
        var northPoint = new Point(currentPoint.X, currentPoint.Y - 1);
        var westPoint = new Point(currentPoint.X - 1, currentPoint.Y);
        return previousPoint == northPoint ? westPoint : northPoint;
    }

    if (currentPointChar == '7')
    {
        var southPoint = new Point(currentPoint.X, currentPoint.Y + 1);
        var westPoint = new Point(currentPoint.X - 1, currentPoint.Y);
        return previousPoint == southPoint ? westPoint : southPoint;
    }

    if (currentPointChar == 'F')
    {
        var southPoint = new Point(currentPoint.X, currentPoint.Y + 1);
        var eastPoint = new Point(currentPoint.X + 1, currentPoint.Y);
        return previousPoint == southPoint ? eastPoint : southPoint;
    }

    throw new InvalidOperationException();

    /*
     
    | is a vertical pipe connecting north and south.
- is a horizontal pipe connecting east and west.
L is a 90-degree bend connecting north and east.
J is a 90-degree bend connecting north and west.
7 is a 90-degree bend connecting south and west.
F is a 90-degree bend connecting south and east.
     */
}

char[][] GetInput()
{
    string rawInput = @"
F-F-JJF7.F7.F-LJ7-|77.L|.FJ7--7LFL|-|F-.|--7.F7-F-.F|-FL7.FF7.F77-FLLJFLF--.FL--FF--7..FJ-L-LL-J..F-J77FF|-FF77FL|-|7J-7--7J-L77.FL.FL|-F7F-
LJ|.L-|-FF|J|L|||.L7J.FJF7-77.|7||L7F7-F77-|7|J.-.F-7F|L---L77..--|FL.F|L7FFF.FFF7..L77F.F|7.J7L7-JF.JJF-7.F--L7.7-L7..|L--7.L-.L|FFL-JL|F-J
L.|FL7|F|||LJFL-LJ-JJF7JF|-7-FJF7J-L.7.F|-7L7||L.FJFJ7J7L7.LFL-LJ.|F|--|.F7F7-FFJL7--LFJFLLF.L||L7|.7L-|7F7F|||.FF7LL7F7J.|-F7-7J.L.|7L.-L-|
LLF7-J7FL|..J-FLJL----JL-.L|JF7-J7F|7J.L|LJ.LLJJFL7L7F7F7777J.L|FFL|.|F|F||||-FL-7L7J.JF7.LL-7|7.7-FJ7LF-J|F7F-7FJ|7||FL.FF7LF-LJLJ7FJ.|-LF-
FL|F-LFJ7LJ7.F-..L|-JL-7LJLL-JLFLLFJL7--L7|.FJ|LF7L7||||L7|F-7.L7-JJLF7F-JLJ|-|F7|FJ7F-JL-J|7JLL7|.L|J-L-7|||L7LJFJF7L77F-|L-F7JF||LJJ|-7LLJ
FF|J7-LJ7L||FJ7...F-L-|-|J.LL|-777|F-F7|.-77.FFFJ|FJLJLJFJ-JFLF-JLFF7||L---7L7FJLJ|77JF-7...F7L7J7F.J7.F-J||||L-7|.|L-7JJJ|JFLJ|L7||FL7J|-||
FJJJF-|L7F777-|FFFJ.L-J|.FF.|.F-JJ-7.||FJL||FF-JFJL7F---JJ.|F..FF|L|FJL7-F-JFJL--7L7F7-F77-|.-J|.F|L.|7L-7||L7F7|L7|F-JJJ|JFJLL|F--LJF7FL.|-
|J.FL7J7|J|L|F-F-LJ--|-JFJ7--7|L.|.L-|F7JJF-7|F7|F7||F7JFL-F-7.FJFF7L-7L7L-7|F-7-L7|||L||7-LL7.-7L|JF-7F-J|L7|||L7LJL-77|.FLJ7-L7JL|||LFJFJ.
L--7LJLJ7-J-JJ.L|J7.||-L7--7|.||FF-JLFJ|7F|FJLJ|||||LJ|JF-|JL-7.F7F7F7L7|LFJLJFJFL||||FJL7.|F-JJLF7.L7|L-7L-J|||FJF7F-JF77-|-JJFJ-FLF7.L7|.F
..|LL|.|J.|.JJL-|FL-JJ-L|7FLF7.FF-7F-L7L7FJL7JFJ||LJF-J7J.FFJ-F-||||||FJL7|F--JF7FJ||LJF-J-7FL7FFJL7-||F7L-7FJ|||FJLJF7-L-7L7..|..F-|.F-|7-|
.FLF-L-FF7L7L-|7F--|FL..|F7L||.L7LFL.FJFJL7FJFJFJL-7|F--77||LFF7|||||||F7LJ|F77|||FJ|F-J-F7F7LF-L-7|FJLJL7FJL-J|||F--JL-7FF7J7-JFL-J..L|L-.|
.|.L-LJ|77.|FLL-JL-FJFJJ-F7FJ|7F-7|LFL7||FJL7L7L7F7|||F-JF7JL||LJLJ|||LJL7FJ||FJLJL-JL-7L|LJ|-F7F7||L7F7FJL-7F-J|||F----JFJ|7F7J|FF-J.FL|JFF
7L7|F|7LJF-J-J||J.|.JJ7F||||FJ7-F|7-F-JL7L-7|FJFJ|LJ||L-7||J.FL---7||L-7.|L7||L-7F7F7F-J-L-7L-J||LJL7LJ||F-7||F7||||F7|F7L7|F7-7FL-.LF.JJ7F7
L.L77777FJL|.--|.LF7|.F--J||L-7F7|F7L--7L--J||FJ-L-7LJF7LJL-7F---7||L-7L7|FJ||JFJ||||L--7F7L7F-JL7F-JLFJ|L7LJ||LJLJLJL-JL-J|||.L7.|7|LF|LFJ|
FL7L|.LFJ.-FJ|.JF|JFL-L--7|L7FJ|L-J|F-7L--7FJ|L--7JL7FJL----JL--7LJL--JFJ|L-JL7L7|LJL---J||FJ||F-JL7F7|FJFL-7LJF-7F-7F-7F--J|L7L|F7-77|..J--
FF-7JF7FJ7J..L-LLL-JLF.F-JL-J|FL-7FJL7L---J|FJF--JF7|L-7|F77F7F7|F--7F7L7L---7|FJL7F7F7.L||L7L7L--7||||L7F--JF7L7||FJ|FJL7F-JFJ---J|LF7-J-LJ
L-J7-LJ|.JJ-F|-.|FFJ||FL7F--7L7F7||F7L----7|L7|F7-||L-7L7|L7||||LJF-J|L7|F--7||L7FJ|LJL7FJ|FJFJJF-JLJ||FJL--7|L-JLJL-JL--J|F-J-|7|F7FJ.JLF.|
L7JF.L|7.|.-7FJ--7FJ|LF|LJF7L7||||LJ|F7F-7||-|||L7|L--JFJ|FJ|LJL7-L-7L7LJL-7LJL7|L7|F--JL7|L7|F7L---7||||F7FJL---------7F7|L---7F|7.L-|JF77L
FJ.|..J-7FJ--JJFLJL77LFF-7||FJ||LJF-J||L7LJ|FJLJFJL-7F-JFJL7L7F-J|F7L7|F--7L--7||FJ|L--7FJL-JLJL7F-7|||L7|||F----------J|||F---JJ-7..|.-JLFJ
L7FJF-L||77L|7J||FJ.LF7L7LJ||FJL-7|F7||FJF-JL--7L-7FJL7LL7FJFJL-7FJL7|||F-JF7FJ|||J|F--JL-7F-7F-JL7LJ|L7LJ||||F7F7JF7F7FJLJL---7J|J.-77|J.|J
LLJL7J--L7--F7F7-7LLFJL7L-7||L7F-J|||||L7||F7F-JF-JL-7L7FJL7|F7FJL-7LJLJL--J|L7||L7||F7JF7LJFJ|F7|L-7|F|F7|||FJLJL7||||L-7F----J7|7FJ7---7.|
FJFFJ.-.|.FF-77J|-7LL-7L--J||FJL-7LJLJL7|L7||L-7L-7F7|FJ|F-J||LJF7F|F7F7F7F7L-J|L7LJLJL-JL7FJFJ||FF-JL7LJ|||||F---J||||F7||F7JLF--77JLFJ||F-
|-77FL-LJ-FJF-.FF.F7F-JF--7|||F7FL----7||FJ|L7FJF-J|LJL7||F7|L7FJL7LJLJ||LJL--7|FJF7F7F-7FJL7|J|L7L-7FJJFJ||LJL7F--J||LJ||LJL-7L-7L7..|L|F7|
F-7-77FJ|-F--.F7|FF7L--JF7||||||F-7F7FJLJL7|FJL7L77L-7FJ|||||FJL-7|F7F7||FF--7|LJFJLJ|L7LJF-J|FJFJF7||F7L7|L7F-J|F--J|F-J|F--7L7FJFJ7-LFL-L-
|LL|L-JLFJ|F|F7-F7|L7F--JLJLJLJ|L7||||F--7LJL-7|FJF7||L-JLJLJL7F7|LJ|||LJFJF7|L7FJ.F7L-JF7L-7||FJFJLJ|||FJ|FJL--JL--7|L--J|F7L-JL7L-7.7|--J.
|LF--7JFJFF--J|F|LJFJL--------7|FJLJ|LJF7L7F--J|L7|L7|F-------J||L7FJ|L-7L7||L7||F7|L--7||F7|||L7L--7||LJFJL7F---7F-J|F---J||F7F-JF7|F-J-F7.
7F||LL.L--L--7L7L-7L--7F7F7.F-J|L--7|F7|L-J|F7-L7LJFJ||F7F7F-77||FJ|FJF7|FJ|L7LJLJ||F--J||||||L7L7F-J|L-7||FJL-7|LJF7|L7F7L|LJLJF7|LJJ7.|L7-
LF7F7.|7F-F-7|FJF7L--7LJLJL7L-7|F7FJLJ|L--7||L7LL-7L7|LJ|||L7L7||L7||FJLJL7|FL-7F-J||F7.||||||FJFJL-7||FJL7L7F-JF--J|L7|||FJF7F7|LJJL7F-F7|L
|FJJJFJF.FL7LJ|||L7F7L----7L-7|LJ|L-7FJF7FJ||FJF--JFJ|F-J|L7L7|||FJ||L7|F7LJF7FJL7FJLJL7||||||L7L-7FJL-JF7|FJL-7L-7FJFJLJLJFJLJLJF|||LJ.LL||
||FLJL7.F7-L-7L7L7|||JF7F7L-7||F-JF7||FJ|L7LJL7L--7|FJ|F7L7L7||||L7|L7L7||.FJ|L7FJL7F7FJ|||||L7|F-JL--7FJ|||F--JF7||FJF----JF7F|-|FL|F7-F-L-
|77.FLLFJL7F7L7|FJ|||FJLJ|F7|||L7FJ|||L7|FJF--JJF7|||FJ||F|FJ|LJ|FJL7|FJ||FJFJ||L-7||LJFJLJLJFJ||F7F7-||J||||F7|||||L7|F7F7FJL-7LJ-LJ7J.7-LJ
LJ-J-F.L-7LJL-J|L7LJLJF-7||LJ||FJL7LJ|FJ||FJ.F-7|LJLJ|FJL7|||L-7||F7||L7||L7L7FJF7||L-7|F---7|FJ||||L7|L7LJ|||L-J||L-JLJLJLJF--J7|JL|J|FL7.|
|7|..F--7L----7|LL----JFJLJF7|||F7L-7|L7LJ|F7L7|L---7|L7FJ||F-7|LJ|LJL7|||FJFJL7|LJ|F-JLJF7-||L7||||FJ|FJF7|LJF7FJL7F7F7F--7L-7F7F77JF-77.-L
.L-F7|F7L-7F--J|F-7F-7.L---J||||||F-JL7L-7||L7||F-7FJL-JL7|||FJL-7L7F7|||||FJF-JL7FJL7F7FJL7|L7|||||L7||FJ||F-JLJF7||LJLJF7|F-J||||J-F-J7-LL
FL---LJ|F7LJF-7LJFJL7L-7F7F7|||LJ|L-7FJF7|||FJ||L7|L--7F-J||||.F7|FJ||||||||FL-7FJ|F-J||L7FJ|FJ||||L7||||FJ|L7JF-JLJL----J||L--JLJ|JFL-|.F|.
-FJJ|.L|||F-JJL-7|F7|F-J|LJ|||L-7|7FJL7|LJ|||FJL7|L7F-J|F7|LJ|FJ|||FJ||||||L7F7|L7||F7||FJL7|L7||||FJ||||L7|FJFJF7F7F--7F7|L-7F7F-J77|L|7F-7
.J-L7FLLJLJF7F7FJLJLJL-7L-7LJL--JL-JF-JL7FJ||L7L||FLJF-J|||7FJ|FJ|||-LJ||LJFJ|||FJ|||LJ||F-JL7||||LJFLJ||FJ|L-JFJ||LJF7LJ|L-7LJLJ.LJJ7F-7|.J
L7F-7-F7F7FJLJLJF7F7F-7L--JF7F7F7F-7L-7FJL7||FJFJL--7L7FJ|L7L7||FJ|L-7FJL7FJFJLJL7||L-7||L7F7|LJ|L--7.FJ||J|F7FJFJL--JL-7|F7L--7F77.FL7.-J-|
||J-F-JLJLJF7F-7|LJ||-L7F7FJ||||LJ|L7FJL7||||L7|F7F-JF||FJFJF||LJFJF7||F-J|FJJF--J||F7|||FJ|||F-JF-7L7L-JL-J|LJLL7F7F---JLJ|F7FJF7777|FF.F7|
7.|.L7F-7F-JLJFLJF7LJF-J|||FJ|LJF7F-JL-7L7LJ|FJ||||F--J|L7|F-JL7FJFJLJ|L7FJL-7|F-7|||||||L7||||F7L7|FJLF---7L-7L|LJ|L-----7||||FF-7LFL-F7L7J
-7F-7LJFJ|F---7F7||F7L--J||L-JF-J|L--7|L7L-7LJL|||||F7FJFJ|L7F7|L7L-7L|FJL7F7||L7||||LJ|L7|||||||FJ||F7L-7LL-7L7F7FJF-----JLJ|L-JFJLFJL|F-||
LF|.LF-JFJL--7LJLJLJ|F7F7LJF-7L-7L---JF7L7FJF--J|LJ||||FL7|LLJ||||F7|FJ|F7LJ|||FJ|||L-7|FJ||LJ||||JLJ||F7|F7.L7LJ|L7L---7F7F7|F-7L-7L-7FJF|7
F7LFJL--JF---JF7F--7LJLJL7JL7|F7L7F---JL7LJ7L--7L-7LJ||F7|L7F-JL7||LJL7LJ|7FJ||L7||L7FJ||7|L7FJ|||F--JLJ|LJL-7L7FJFJF7F7LJ|||LJF|F-J-L-|-LFJ
LL7J7J.L-|F--7|LJF7|F---7L--JLJ||LJF-7F7L----7FJF-JF7|||LJFJL7F7||L--7L7FJFJFJL7|LJFJL7|L7|FJL7||||F7F-7L--7FJ7|L7L-JLJ|F-J|L--7LJ.L||L|7JJJ
F7JFF--J.LJF-J||FJLJL7F-JF--7F7L---J.LJ|F7F--JL7L-7||||L7FJLFJ|LJL7F7L7||FL7L7FJL-7L-7||FJ|L7.LJLJLJLJFL--7|L7FJFJF--7|LJF-JF-7L7-F-|7LL|-77
7LLFJ|.F-LFJF7L7|F7F-JL--JF7LJL--7F---7LJ|L7.F7|F7LJ|LJFJL-7L7L-7FJ|L-J|L-7|FJ|F-7L7FJ||||L-JF----7|F7F---JL-JL7L-JF7|F--JF7L7L-J7|||77.-.LF
|F7L-7FLJ-L-JL7|LJLJF-----JL-----J|F--JF7L-JFJLJ|L-7L7-L7F7L7|F-JL7L7F7L-7|||L|L7|FJL7||L-7F-JF7F7|FJLJF------7L---JLJ|F--JL-JF--7J-LF7FL.FJ
LF--||||-7.F|.LJF7F7L-7F-7F-7F-7F-JL7F7|L7F-JF7FJLFJFJF-J|L7|LJF7FL7LJL7FJ||L7L7||L77LJ|F-J|F7||||LJF-7|F-----JF7FF7F-JL---7F-JF-J7.L|JFJ-|J
LLJLLLJJ-77|-F--JLJL7F||7LJJLJFJ|F--J|LJFJL-7||L7FJFJ7|F7|FJL--JL-7L7F7||FJ|FJJ||L-JF--JL-7LJLJLJL--JFJ|L7F7F7J|L7|LJF-----JL-7L7LF77JFF-F|.
7L-77.|FL-7.FL-----7L7LJF---7-L-JL---JF-JF7FJ||FJL7L-7||||L7F-7F--JFJ||||L7|L-7LJ.F-JF7F7FJ-F7F7F----JFJ.LJLJL7|FJ|F-JF7F7F-7FJFJFJL7J||FJJ.
|7|LL.LF7L-F7F--7F7L7L-7L--7L-7F------JF7||L7||||FJF7||||L7|L7|L-7FJFJ|||FJ|F7L--7|F7|||||F7|LJ|L7F--7|F--7F-7LJL-JL-7||||L7||FJF|F-JJ|77.L7
L|F|.|L7JL|F-JF7LJL-JF7L---JF7LJF7F----JLJL7LJLJFJFJ||||L7||FJ|F-J|FJ|LJ||FJ|L--7||||||||||LJF7L7LJF7LJL-7|L7L7F7F7F-J|LJL7||||F-J|J..|LF7.J
L|--J7-|7--L--JL-----JL-----J|F7|||F-------JF7F7L-JFLJ||FJ||L7||F7||F7-FJ|L7L--7||||||||||L7FJL7L7FJL----JL-J.|||||L7L|F--J|||LJF-J-F77F.J-7
LJL|JFF777LLJ-F7F7F7F-7F7F7F7LJLJLJL----7F7FJ||L-----7|||FJL7||LJ||||L7L7L7L7F-JLJ||||||||7||F7L7|L--7F7F7F7F7LJLJL-JFJL7F7|LJF7L--7JLF-7J.|
FL.|-JJL-J---FJLJLJ|L7||LJLJL--7F7LF---7LJ||FJ|F-----JLJLJLFJ|L7FJ|LJFJFL-JJ|L-7FFJ|LJLJ||FJ||L-JL---J|LJLJLJL-7F7F--JF7LJLJF-JL7F7|7-LJJF--
-J7J.|7.LFJ|7L----7L-J|L--7F7F7LJ|FJ|F7L-7LJL-JL----7F-7F--JFJFJL7L7FJLF----JF-J-|FJLF7FJ||FJL----7F7FJF7F-----J|||F--JL7F--JF7JLJLJJ|FF-|7|
L-J-J--J7F7F-7F7F7L7F7L-7JLJLJL-7|L--JL-7L7F-7F7F---J|FJL-7FJ|L7FJFJL7FJF7F-7L-7FLJF-J|L-JLJF-----J|||FJLJ7F7|F7|LJL7F77LJF--JL7F7JFFJ7J|L77
F||FF|J7F||L7||||L-J|L-7|F7F7F--JL--7F--JFJL7|||L----JL7F-JL--7||.L7FJL-J||FJF7L-7.L-7|F7F-7L----7FJLJL----JL7|||F--J||F--JF7F7LJ|--JLL-JJ|-
L--L7-7-FJL-JLJLJF--JF7|||||LJF7F--7|L--7|F7|LJL--7F--7|L-7F7FJ||F-J|.LLL||L7||F-JF-7|LJLJFJLF7F7LJF--------7LJLJL-77|LJF--JLJ|F-J7.LF-7J|||
L|.|.FF-L--------J|F-JLJLJLJF7|||F-JL---JLJ|L----7|L-7|L7FJ|LJJLJ|F7L-7JL||FJ|||F7L7|L---7L7FJLJL-7L-7F--7F7L--7F--JFJF7L----7||JF-7J|FJ7L7L
7J.L-|.FF---------7L-7F-7F-7|LJLJL-7-F--7F7L-7F-7||F7||FJL7L--7|7||L7FJ7-LJL-JLJ|L-J|F7F7L7|L--7F7|F7LJF-J||F7-LJF7.L7|L---7FJLJ.|FJL77F|LJJ
-7|JJLF-L--7F7F--7L--JL7LJFJL-7F---JFJF7LJL-7|L7||LJLJLJJ.L7F7L-7||-||F||L|L-JJFJF-7||LJL7||F-7LJ|LJ|F-JF-JLJ|F7FJL--JL7F7||L7F7FJL-7|F-LFJ7
L-77-LJJL7LLJLJ7FJF7F-7L-7L--7|L----JFJL-7F7|L-JLJF---7|F-7||L7FJLJJ||7JLJFJ7L||FJJ|LJF-7LJ||FJF7L-7|L--JF7F7LJLJF--7F7LJL7L-J|LJF--JF7F-7L-
LJ.7.-F7JL7|LF-7|FJLJFL--JF-7LJF7F7F7L--7||||F----JF-7L7L7|LJ.S|.L-|LJ|F|.LLJFFLJF-JF7L7L-7LJL7||F-JL----JLJL-7F7L7LLJL--7L-7FL-7|F7.|LJFJF|
|F|-7F|J.FL--L7LJ|F-------JFJF7|||LJL---JLJLJ|F--7FJ7L7L-JL7F-||-|LJJJL-.F.|FLFF-JF-JL7|F7L--7|||L--------7F-7LJL-JF----7|F-JF7||||L7|F-JLF-
-J|.J---FF|.|FL7FJL-------7|J||||L---------7FJL-7LJF7FJF7F7|7.||7---L7JJ-FFJ---L--J7F7LJ||F--J||L---------J|FJLF-7FJF7F-J|L--JL-JLJFJ||F7-JJ
LJJ7|LFF-7.F-F-J|JF---7F--JL-JLJL7LF----7F-JL7F7L--J|L-JLJ||LLLJ|7-77J|.FLJ|FLJF----J|F7||L7F7LJF-7F7F-----JL--JFJ|FJLJF-JF--7F7F7FJJ|LJ|L|F
|.L7-FJLFLJ7-L--J7L--7|L7F-7F-7F7L-JF7F7LJF7F||L7F-7L-7F-7LJJ||J|FF-F--7L||L7.-L----7LJLJL7LJL7FJ.LJLJF7F----7F7L-JL--7|F7L7FJ|LJ|L--JF7|-J7
L7-.LFJ-77J|F-LF7FF--JL-JL7LJJLJL7F7|||L--JL-JL7LJJL--J|FJFJ7-L7LF-.|-LL-FJJ|-LJLF--JF---7L---J|F--7F-J|L---7LJ|F7F7F7|||L7|L7L-7L----J||FLL
LF.L.|JF|J-|JF-|L7L--7F-7FJF-7F-7LJLJLJF------7L7F77F7.||F7|FJLJJJ|7L|F|.L|-7-|FFJF--JFF-JF-7F7|L-7|L-7L---7L-7LJLJLJLJLJ-LJJL-7L----7.LJJ|L
.|JF--7J||FJJLLL7L-7FJ|FLJ.L7|L7|F----7L-----7L7LJL-JL7|LJL7JFJ.F7L-FFL7-|J.J-FFL-JF--7L--J-LJLJF7||F7L7F7FL-7L7F7F-77F7F7F7F-7L7F-7FJ7-LJFL
JLF||F7-J-L7-77FL-7LJFJF-77FJL7||L---7L------J.|F-----J|F--J7L--F-7J|LLL7|.-|-J||||L-7L----7F77FJLJLJL7LJL--7|FJ|||FJFJ||LJLJFJ.LJ-||||-|7J|
|-|L7J.LFLF7FLF--7L--JFL7|FJF7LJ|F7F7L---7F7|F7|L----7||L7F77LLF77F-L--LF7|7||-J7FF7.L----7LJL-JF---7FJF7F--J|L-JLJ|.|FJL7F--J7F--7||77.L-..
F-7JJ-|.|--7--L-7L-7F-7FJ||FJL-7LJLJL-7F7LJL-J|L-----JFJFJ||F77FF-77-L77L-L--J7.-FJL----7LL-7F-7|F--JL-JLJF7L|F7F-7L-JL-7||F7F7|F-J|L7JJ|--F
-J||L77-|7-J-F--JF7|L7|L7|LJF7FJF--7F7LJL--7F7L-7F-7F7L7L7||||F7|FJ.||.L-F|J.|J7LL-7F7F7L---JL7LJ|F-7F7F--JL7LJ|L7|F-7F-J|LJLJLJL7FJFJ7LJJ.J
LFF|-|FJLJ|.|L---J|L-JL-J|F-JLJFJF-J|L----7|||F7|L7|||||FJ||||||||F7F77LF.||FF77||FLJLJL-7F7F7L7FJ|FLJLJF--7L-7|FJ|L7|L--JF7F-7F-JL-JF-7..-.
FLF|FJJ77|L.LF----JF7F7F7|L---7|FJF7|F----JLJLJLJFJLJL-JL-JLJ||||LJLJL7.F.|JL||FF7F7F--7FJ|LJL-JL-JF7F7FL-7|F-JLJFJFJ|F-7FJ|L7|L7-F7F|7J|.|7
F-LJ7..FL|.FLL-----JLJLJ|L----J|L-JLJL-----7JF77FJF7F7F--7F7FJ|LJF-7F7L-7F7JFJL-J||LJF-JL-J|F------J|||F7FJ|L--7-L-JJ|||LJFL-JL-JFJL--77J7L7
|JF-7F77J|F7LLF7F7F---7FJF7F--7|F------7F--JFJ|FJFJLJLJF7LJLJ|L7FJ-LJL--J|L7L--7FJL7FJF7F7F7L-7F7F-7LJLJLJJL--7L7F7FFJ|F-----7F7||F--7L7FL7.
|F-JJL|.-FJL7F||||L--7LJFJLJF7LJ|F-7F-7LJF7L|FJ|FJLF7F-JL-77F7F||F7FF7F-7|FJF7FJL-7|L7|LJ|||F7LJLJFL-----7F--7L7LJ|FJFJ|F----J|L7||F7L7|FJ-7
FJ7L--J7LL-7L-J|||F7LL-7|F--J|F7LJ7LJFJF-JL7|L-JL--JLJF--7L7||FJLJL-JLJFJ||FJ|L--7||FJL-7|||||F7F7F77F7F7LJF-JFJF-JL-J.||F7F7-|FJ|LJL7LJ7JJF
.L-7J7L|L|FL7F7LJLJ|F7JLJL--7LJL-----J||F--J|F--7F----JF-JFJ||L-7F-----JFJ||FJLF-J|||F7FJLJLJLJLJLJL-J||L--JF7L-JF7F77FJLJLJL7||-|F--JF-7.|F
-7LL7FLJ7|F7LJL-7F7LJ|F-----JF7F---7F7FJ|F7L|L-7|L----7L-7L7||F-J|-F-7F7L7LJL7FJF7||||LJF--------7F7F7LJF-7.||F-7||||FJF7F7F7LJL-JL-7FJFJ7-L
.FFJL.|.|7|L7-F-J|L-7LJF7F7F7|LJFF-J||L7|||FJF7||F-7F-JF7L-J||L-7L7|FJ|L7|F--JL-J||||L--JF-------J|LJL--JFJFJLJFJ|||||FJLJ||L-7F-7F-J|FJJLFJ
F-J7J7F-F7L7L-JF-JF-JF-J||LJLJFF7L--JL7|||||FJLJLJ|LJF7||JF7|L7-L7||L7L7|||F77F7FJ||L--7FJF----7F-JF-7F--JFJF-7L-JLJLJL-7|LJ7FJL7LJF-JL-7F7J
L7.J|FF-J|-L7F7|F-JF7|F-J|F----JL-7F-7LJ||LJL-7F--7F-JLJ|FJ||FJF7|||FJFJLJLJ|FJLJFJ|F7FJL-JF-7FJ|F7L7|L---JFJ7|F7F-7F7F7L---7|F7L--JF---J||J
F77LLFL-7L-7LJLJL--JLJL-7||F-----7LJJ|F7LJF---J|F-JL7F-7|L7LJL-JLJLJL7L7F---JL--7L7||LJF7F-JFJL7|||FJ|F7F--JF7LJLJFJ|||L---7||||F--7L----JL7
FJL7||F|L-7L7F--------7-||LJF-7F7L--7||L-7|.F7FJL---JL7LJ7L-7F7F--7F7|FJL-7F----JFJ|L7FJ|L-7L-7LJ||L7|||L---JL---7L7||L---7LJ|||L-7|F-7F-7FJ
F-|-F-----JFJL-----7F7L7LJF7L7||L---JLJF-JL-JLJF-----7L77F-7LJLJF7||||L7F-JL----7|FJFJL7|F-JF-JF-JL-JLJL-----7F7FJ7|||F---JF7||L7FJ||FJL7LJ7
F-JFL-----7|F7F--77LJL7L7FJ|FJLJF7F--7FJF-7F--7L----7L7L-JFJF7F7|LJ|LJFJ|F7F7F--JLJFJF-J|L-7||FJF7F7F7F7F7-F7LJLJ|FJ||L7F--J|||JLJ.LJL7FJFL|
LL-.F---7FJ|||L-7|F--7L7||FJL---JLJF-JL-JL|L-7|F-7F7L7L7F7|FJLJ|L-7|F-JFJ|LJ|L--7F-JFL-7L-7|L7L-JLJLJLJLJL-JL-7.F7L7|L-J|F--JLJLF----7LJL77|
J-L.L--7|L7|||F7||L7FJFJLJ|F------7L---7F-JF-J||-LJL-JL||LJL--7|F-J||F7L7|F-J.F7||F-7F7L-7||FJF7F7F7F-7F---7F7L7||7LJF-7|L--7F7FJF---J-J7F-J
L7.-F-7|L-JLJLJLJ|FJL7L--7LJF---7FJF-7FJL-7L-7LJF7F7FF7LJF7F7FJ|L-7|LJL7LJL7F7|LJ||FJ|L7FJLJL-J||LJ|L7|L-77LJL7LJL-7L|FJ|F--J|||FJLF7J-7F|7.
L|77L7LJF--7F-7F7|L7FJF--JF7|FF-JL-JLLJF7FJF7L7FJLJL7||F-JLJ|L7|7FJL7F7L7F-J||L7FJ|L7L7||F-----J|F7L-JL7FL--7-L----JFJL-JL-7|||||F7||J.FJF77
|.77FJF-JF-JL7|||L-JL-JF-7|||FJF-7F----J||FJL-J|F---J||L---7|FJL7L-7LJ|F|L-7||FJL7L7|FJLJL---7F7LJL---7|F--7L----7F-JF-7F--JFJLJLJLJL--7-||7
L-J-|FJJ|L-7FJLJL7F7F7FJFJ|||L7|FJ|F----JLJF7F-J|F7F7|L7F--JLJF7L--JF-JFJF7LJ|L-7L-J||F-7F---J||FF7F7-||L-7|F7F--JL7FJFJL---JF7F7F-----JL|L7
|.|.LJJLF--J|F-7-LJ|||L7L7|||FLJL-JL-----7FJ|L7FJ|LJLJFJL--7F-JL---7L7FJFJL--JF7L--7|LJ-||F7FFJL-JLJ|FJL--JLJ|L--7FJL7L--7F7FJ|||L-------JFJ
F.7-FJ..L7F7||FJF7FJ||FJFJ|LJF7F---------J|FJFJ|7|F-7FJF7F7||F7F-7FJFJL7L--7F-J|F7FJL77FJ||L7|F7F--7LJF7F--7JL---J|F7L--7LJLJFLJL7F7F7F7F7|J
L7.FJJ77FJ||||L-JLJFJ|L7L7L7FJLJF7F----7F-JL7L7L7|L7||FJ|||LJ||L7|L7L--JF--JL-7||||F7L7|FJ|FJ||||F-JF-JLJF7L----7FJ||F-7L--7F--7FJ|LJLJ||LJJ
FLL7L||FL-JLJL----7|FL7|FJFJL-7FJLJF---JL--7L-JFJ|FJLJ|FJ|L7FJ|FJ|.|F--7L7F7F7||||LJ|FJ|L7|L7LJLJL--J7F7FJL-----JL7|LJLL7F7LJF-J|FJ|LJ-LJJJL
||.|JLLJLFF-----7FJ|F-J|L-JF--J|F-7L--7F-77L7F7L7|L7|FJL7|FJL7|L7L7||F7L7LJLJ|||||FFJ|FJFJL7|F7F7LF--7||L-7F--7F7FJL---7LJL-7L-7LJF7.F7|||LJ
LLJ-F7LJ|FL7F7F7LJFJL7FJF-7L---JL7L-7FJL7|F7LJL7||FJFJF-J|L7L||FJFJLJ|||L7F7FJ|LJL-JFJL7L7FJ||||L7L-7||L--J|F-J|||F---7L---7L7FJF-JL7F77-J.|
-7|-|--.LJLLJLJL7FJF7LJ7L7L---7-FJF7LJF7|||||F-J||L7L7|F7|FJFJLJFJF7F|L--J|LJ-L---7FJF-JFJL7|||L7|F7||L----JL-7|LJ|F7.L7F--JFJL-JF-7L-77|LF7
F|J.|7F|.7-F----JL7|L-7F7L-7F7L-JFJL7FJLJ||L7L-7LJFJFJ|||||FJF--JFJL7L-7F7L7F--7F7|L7L-7L7FJ|||FJLJ|||F7F--7F-JL7FJ|L-7LJF7|L--7FJL|F-J77L-J
L|77.F7|-|.L---7F-JL-7||L--J|L--7|F-JL7F-J|FJF7|F-JFL7|||||L7||F7L-7|F7LJ|FJL-7||||FJF7|FJ|FJ||L--7|||||L-7||F7||L-JF7L--JL---7|L-7|L7-LJLJ7
LL-J7L-L7FF----J|F---J|L----JF7FJ|L-7FJ|F7||FJ|||F-7FJLJ||L7||FJL7FJ|||F7|L-7FJ||||L7|LJL7|L-JL7F7|||LJ|F7||LJL-JF7FJL-7F-7F7FJ|F-J|FJJ7.F-7
-.LL7|LF77|F7F-7|L7F-7|FF7-F-JLJFJF7|L7||LJ||7||||.LJF7FJ|FJ||L-7||LLJ||LJF-J|7|||L7|L7|FJL-7F-J|LJ||F-J|LJ|F7F7FJLJF7JLJJ|||L7|L-7LJ|.F7F.|
|.|.L|-JJFJ|LJ-LJ7LJFJL-JL7L-7F7|FJLJFJ|L-7||FJ||L--7||L7||FJ|F-J||F--JL7J|F7L7LJL7|L7|FJF7FJL7FJLFJ|L-7L-7||LJLJF--JL---7LJ|FJ|F-J.|F-L|J7F
|--F7|JLLL-JLF------JF---7|F7||LJ|F7FJFJF7|||L7||F-7||L-J||L7|L-7||L7F-7L7||L7L--7||7|||FJ||F-JL7FJFJF7|F7|||F7F-JF-7F--7L-7|L7|L7--FF-7-7-7
LJ-|-FF-JFJ..L--7F7F7|F--JLJ|LJF-J||L7L-JLJ|L7LJ||J||L7F7|L-J|F7|||7||-|FJ||F|F--J|L7|||L7LJL-7FJ|FJ-|LJ||||LJLJF-JFLJF7L--J|FJL-J7-L-.J.F.|
|77L-FJ7LJ7|7LF-J|||||L--7F7L-7L7FJ|FJ7F7-FJFJF-JL7|L7LJ|L--7LJ||||FJ|FJ|FJ|FJL-7FJFJ||L7|F---J|FJL7FJF7|||L-7F7L-----JL---7|L7LF7J.|F-.FJ7F
|F||||||.F|LJ-L7FJ||LJFF-J||F7L-JL7||.FJL-JFJ7L-7FJL-JF7|F--JF7LJ||L7|L7|L-J|F7FJL7L7LJF||L7F7FJL7FJ|FJLJ|L7FJ|L7F---7F7F7FJL-J-|L7--J.F|7FJ
LFLJL|L|.F|F7.F||FJL7|FJF7||||F7F7||L7L--7FJF7F7|L--7FJLJL-7FJL7L||L|L7LJ-F7||LJF7L7L--7|L7||||F-J|FJL7F-JFJL7|FJL--7LJLJLJF7F7F|FJJ7L7LF777
|.||FJ-LF-7LFF7LJ|F-JFJFJ||LJ||||LJ|FJF7L|L7|LJLJF7FJ|F7F-7LJF7L7LJFJFJ-F-JLJL--J|FJF7FJ|FJ|||||F7||F7||F7L-7LJL7F-7L--7.F-JLJL-JL7FL.|.LLJ-
FF|L7JJLJF|F|LF7LLJF7L7||||.FJ|||F-J|FJL-JFJL--7FJLJJ||LJFL7FJL7|JFJFJF-JF-7F-7F-JL-J|L7||FJ|||||||LJ|||||F7L-7FJL7|F-7L-JF--7F-7FJJ.JL-LF77
7L|L-JFLFJ-7.FJL7F7|L-J|FJ|FJFJ|||F7||F7F7|7F--JL-7F-JL-7F7||F7|L7L7L7L7FJFJL7|L7F--7L7|||L7|||||||F-J|LJ||L7FJL7FJLJL|F7FJF7|L7LJJ-|.7LL7LJ
|FJFJ.|-|FLL7L-7LJ||F7FJ|FJ|FJFLJ||||||||||FJF7F-7|L---7LJ||LJ|L7L7|FJFJ|JL7FJL7LJF-J|LJLJFJ|||||||L7FJF7||L||F7|L-7F7|||L-J||FJFJJF|FLF.||J
LJLL7LJFF-L-|.LL-7LJ|||FJL7||F---J||||||||||FJ||FJL--7FJF-J|F-JFJFJLJFJFJF7||-||F-J7F77F-7L7|||||||FJ|J|LJL7|LJ|L7FJ||LJ|F-7|LJFJ|LFF-FL-J||
LL7F|7.LF.|-L-7F-JF7|LJL7FJLJ|F-7FJ||LJ||LJ|L7|||F7F-JL7L-7|L-7L7L--7L7|FJLJL7FJL--7|L-JFJFJ|LJ||||L7|FJF-7||F-JFJL-JL-7|L7||7L|L|-FF-77-LF7
.LF--|-|F7..---L7FJLJF-7|L7LFJ|FJ|FJ|F-JL-7L-J||LJ|L7-FJF-J|F-JFJF7FJ.LJ|F7F-JL7F7FJL-7FJJ|FJF-J|LJFJ|L7|FLJ||F-JF7F7F7||FJLJ-7LFJ|||.LJ-|L|
.JLL|JFF|-FF--7-LJ.F7L7||FJFJFJL7||FJ|F7F-JF--JL-7L7L7L7L-7|L7FJFJLJF7F7LJLJF--J|LJF--JL-7LJ-|F7L7J|FJF||F--J|L7FJ||LJLJ|L-7LLL7L-J-7J7JF-77
L-7.L.|LL7L|7.JJFF-JL-JLJL7L7|F-J||L7|||L7JL7F7F-J.L7L7L7FJ|FJ|FJ-F-JLJL----JF-7L--JF7F-7L7F7LJL7L7||F-J||F-7|FJ|FJL---7L7FJF|FLJJL-|.J-|-|7
|FJ.L7|F-|FL|.|7F|F7F-7F7FJFJ|L7FJL-J|||FJF7LJ|L-7F7L7L7|L7LJFJL7.L--------7FJ||F7F7||L7L7LJL-7F|FJ||L7FJLJFJ|L7|L7F---JLLJL--||-..|J.L-J-|7
|-F-F-J7-7JFJ-|FFJ||L7|||L7L-J.|L---7LJ|L-JL7||F-J||FJFJL7L-7L-7L7F7F------JL-7LJ|||||FJ-L7F-7|FJ||LJFJL--7L7L7||F|L-7-L..L|.LJL-7F|LL-FL7.|
|-L7||--FJJJ.|L-L-J|FJ||L-JF7F7|F7F-JLFJF-7FJFJL--J|L7L-7L7FJF-JFJ|LJF----7F-7L-7LJ|||L-7FJL7LJL7L--7L7F-7L7L7|||FJF-J7L|-F-J.||FJ-7-.F|7|-7
-77L-|-LJJ.|LJJLFJJLJ.LJ-LFJLJLJ|LJLF-JFJ||L7|F7F--JFJF7L7|L7L7FJJ|F7|F---J|7L-7L7FJ||F7|L7FJF--JF7FJ7LJ.L7|-LJ||L7L--7-.F7|JF7LF7F|F7-J-|.|
L77JLJ77.F--7|JF--FLJ7L7F.L7F7F7L--7L-7|F-JFJLJ|L--7L7||FJ|FJFJL-7LJLJL-7F7L--7|FJ|FJLJLJFJL7L-7FJ|L--7L|-||LF-JL7|F-7|JF|L|J||FF--JFLJ|.F-L
LLJJJ|L7-7FJ7|-|7F7J|J7FF--J|LJ|F--JF7||L7FJ7F-JF7FJFJ||L7|L7|F7FJF-----J||F7FJLJFJ|F----JF7L-7LJFJF7FJ.LLLJ-L7F7|||LLJ--F-|L|7J.7|F|J.77JL7
||JF-7FJ|L.L-J.|F|.|LJ|-|F--JF7|L-7FJLJ|FJL-7|F7|LJ-L7||FJL-JLJ||-L----7FJ|||L--7L7|L7F7F7||F7L7-|FJLJ--77L|J|||||||.LL7L|-|.F--7.LF7J.-77J|
FL7L.LL||L-JF|-J7|F-7.--LJJF-JLJF7||F-7|L7F-J|||L--7FLJ|L7JJ.J|LJJLF---J|FJ|L7F7L7LJJLJ||||||L-JJLJ-JJ|LJ7|L-LLJLJ||JLJ-JL|F7J-7.J-L7.FL.L77
7F7JL77|JL|-FFFJ|F77|F|.|J-|F-7FJ||||FLJFJL-7||L7F7L--7|FJJ|77F|7.|L---7|L7L7||L7L----7||||LJLJ|LFJ|7L-7L--J.LJLFLLJJFL.F7FLJ.F|F|7L||J|--|J
-|L|LLL77FF-|FJJJ|JF7--7L|FLJ.|L7||||F--JF-7|LJFJ|L7F-J|L7F|FJ777.JJLF-J|FJFJLJ.|F7F-7|LJ||7J7-J.|.FJ.L--FJ7JLJL7JFJJF-7L-|L--LL-L7.FJL-JL||
FL7F--7L|-JF--L-FFJFJL--7LF---JFJLJLJL--7L7|L7.L7|FJ||.L-J7|FL-J7.L|-|F7|L7L--7FJ|LJFLJJFLJJ---F-J-|.FLL-7-J7-.7J.JF-J-L77JF..FJ7|F7|F7.FFL|
JL7JF-JL||L-LJL77J-L7JF|-FL7F--J|..|.F--JFJL-JFFJ|L7L77.LJLJJ7|L|J.-J||||FJF-7||FJJF77.L-|.|.L-JJ7-LF7.|.-J7F-----J-LFFJL-FLJ-J.--7-F-|JJJLL
.FJ-JF|7F7|.F-7LLJ.L||||.|FLJJJ|J-|LFL--7L-7-|-L7|FJFJ77JF|-FL7JLF77.||LJ|FJFJ||||FJ|77|L|7F7L|L|7..||7|FL.FF7L.|LL7LJ--7.|LJ77LJ|J-LFJ|F|.L
LLJJ.F7-J|-FJ.L7L-F-.|J7.||F|7-||.|F----JF7L---7||L7|JLLF77L7L|JF|.77LJJ-LJ.|FJ|L-JFJL|7-FFL--L-FF-7-|-F77L-|7J-|..|7|J||F-7LJ7LL7J|.|.FJF||
||77FJ|F7.L7|FL||FJ77|J|---7|--J-7-L-7F-7|L--7FJLJFLJ.F.LFL-..-FFJLJJ|L.FJJ-LJFJF7FJJ7L|LJ7J7.|-LJ7.FLLJ-7L7L7.FJ-FJF7||.7--7J7L----|77|-LJ|
-|-|--F7|7..|J-L|-LJ7FFJF..L|--|-J7|7LJ-|L--7LJ.|7--|-|7L|FJ-7L7L7F|FJ--L-JJLLL7|LJ7|JJJF-JLL7FF7-7-JFJ|7LF|.|.L--F-J-J-FJF7|LJ-JJLL-JFJ|FFF
LL.|||.F|L77.L-JL|L|7-|JL--.L-7L-L|JFF--JF-7L--7JJ.FF.|||.|7LL.-7|J7|FF.|J.7LL-LJ-L-F7|FL-||FLFJ7-|J.J7L--|7FJ777.F-7|---7LL7||FLJFJ|-7-|--J
LJ-JJL-FLJ|J-JL7.7LLJ-F..-.-J.7LLL7-JL---J-L---JJ.F.F7.LJ.LJLJ.J-J.|.JL7-.J.JJFL|J.|JF-J-L|-LJF-F--FJJLLJLJJ-L|JLLJL7--J.J..|-F7JL--L-J.|JFJ";

    char[][] grid = rawInput
        .Split("\r\n", StringSplitOptions.RemoveEmptyEntries)
        .Select(x => x.ToCharArray()).ToArray();

    return grid;
}

Point GetStart(char[][] grid)
{
    for (int y = 0; y < grid.Length; y++)
    {
        for (int x = 0; x < grid[y].Length; x++)
        {
            if (grid[y][x] == 'S')
            {
                return new Point(x, y);
            }
        }
    }

    throw new Exception("No start found");
}


