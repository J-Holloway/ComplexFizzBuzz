namespace ComplexFizzBuzz;

public interface IShuffleService
{
    IEnumerable<int> Shuffle(IEnumerable<int> source);
}