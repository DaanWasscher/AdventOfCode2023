/*
Time:        40     70     98     79
Distance:   215   1051   2147   1005
 */

var races = new List<Race>
{
    new(40,215),
    new(70,1051),
    new(98,2147),
    new(79,1005)
};

long sum = 1;
foreach (var race in races)
{
    sum *= race.GetNumberOfWaysToBeat();
}

Console.WriteLine(sum);
//answer 1084752

// part 2
var bigRace = new Race(40_70_98_79, 215_1051_2147_1005);
Console.WriteLine(bigRace.GetNumberOfWaysToBeat());
//answer 28228952

public record Race(long Time, long RecordDistance)
{
    public long GetNumberOfWaysToBeat()
    {
        long distancePerSecondPoweredUp = 0;
        Parallel.For(0, Time + 1, secondsPoweredUp =>
        {
            var speedPerRemainingSecond = secondsPoweredUp;
            var remainingSeconds = (Time - secondsPoweredUp);
            var distanceTraveled = speedPerRemainingSecond * remainingSeconds;

            if (RecordDistance < distanceTraveled)
            {
                Interlocked.Increment(ref distancePerSecondPoweredUp);
            }
        });

        return distancePerSecondPoweredUp;
    }
}