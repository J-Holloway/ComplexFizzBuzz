namespace ComplexFizzBuzz;

public interface IFizzBuzzService
{
    Task<IEnumerable<string>> GenerateFizzBuzzAsync();
}