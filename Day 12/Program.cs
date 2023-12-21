using Day_12;

var inputs = InputParser.GetSpringArrangements();
var possibleArrangements = 0;

foreach (var input in inputs)
{
    possibleArrangements+= input.GetNumberOfPossibleArrangements();
}


Console.WriteLine($"There are {possibleArrangements} possible arrangements in part 1");


//part 2

var inputs2 = InputParser.GetSpringArrangementsPart2();
long possibleArrangementsPart2 = 0;

foreach (var input in inputs2)
{
    possibleArrangements += input.GetNumberOfPossibleArrangementsPart2();
}


Console.WriteLine($"There are {possibleArrangementsPart2} possible arrangements in part 2");