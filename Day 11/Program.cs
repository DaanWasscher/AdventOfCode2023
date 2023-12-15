using Day_11;

var grid = InputConstants.GetGrid();
var galaxies = InputConstants.GetGalaxiesFromGrid(grid);
var galaxyDistances = new Dictionary<string, int>();

foreach (var galaxy in galaxies)
{
    foreach (var otherGalaxy in galaxies)
    {
        if (galaxy == otherGalaxy)
        {
            continue;
        }

        if (galaxyDistances.ContainsKey($"{otherGalaxy.Id}_{galaxy.Id}"))
        {
            // we already have the distance from the other direction
            continue;
        }

        var xDistance = Math.Abs(galaxy.Location.X - otherGalaxy.Location.X);
        var yDistance = Math.Abs(galaxy.Location.Y - otherGalaxy.Location.Y);
        var distance = xDistance + yDistance;

        galaxyDistances.Add($"{galaxy.Id}_{otherGalaxy.Id}", distance);
    }
}

Console.WriteLine($"Total distance: {galaxyDistances.Values.Sum()}");

//part 2

(char[,] grid2, int[] emptyRowIndexes, int[] emptyColumnIndexes) = InputConstants.GetPartTwoGrid();
var galaxyDistances2 = new Dictionary<string, long>();
var galaxies2 = InputConstants.GetGalaxiesFromGrid(grid2);

foreach (var galaxy in galaxies2)
{
    foreach (var otherGalaxy in galaxies2)
    {
        if (galaxy == otherGalaxy)
        {
            continue;
        }

        if (galaxyDistances2.ContainsKey($"{otherGalaxy.Id}_{galaxy.Id}"))
        {
            // we already have the distance from the other direction
            continue;
        }

        var xDistance = Math.Abs(galaxy.Location.X - otherGalaxy.Location.X);
        var yDistance = Math.Abs(galaxy.Location.Y - otherGalaxy.Location.Y);

        var yDistanceEmptyRows = emptyRowIndexes.Intersect(Enumerable.Range(galaxy.Location.Y, (otherGalaxy.Location.Y - galaxy.Location.Y))).Count();


        var xDistanceEmptyColumns = otherGalaxy.Location.X - galaxy.Location.X >= 0
            ? emptyColumnIndexes.Intersect(Enumerable.Range(galaxy.Location.X, otherGalaxy.Location.X - galaxy.Location.X)).Count()
            : emptyColumnIndexes.Intersect(Enumerable.Range(otherGalaxy.Location.X, galaxy.Location.X - otherGalaxy.Location.X)).Count();

        var distance = xDistance + yDistance + (yDistanceEmptyRows * 999999) + (xDistanceEmptyColumns * 999999);

        galaxyDistances2.Add($"{galaxy.Id}_{otherGalaxy.Id}", distance);
    }
}

Console.WriteLine($"Total distance massive expansion: {galaxyDistances2.Values.Sum()}");