namespace ComplexFizzBuzz;

public class ShuffleService(IRandomService randomService) : IShuffleService
{
    public IEnumerable<int> Shuffle(IEnumerable<int> source)
    {
        var list = source.ToList();
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = randomService.Next(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
        return list;
    }
}