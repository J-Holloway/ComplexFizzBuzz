using Microsoft.Extensions.Logging;

namespace ComplexFizzBuzz;

public class FizzBuzzService(
    IRandomService randomService,
    IShuffleService shuffleService,
    ILogger<FizzBuzzService> logger)
    : IFizzBuzzService
{

    public async Task<IEnumerable<string>> GenerateFizzBuzzAsync()
    {
        var numbers = Enumerable.Range(1, 100).ToList();
        numbers = shuffleService.Shuffle(numbers).ToList();
        var results = new List<string>();

        foreach (var number in numbers)
        {
            string output = "";

            if (number % 3 == 0) output += "Fizz";
            if (number % 5 == 0) output += "Buzz";
            if (string.IsNullOrEmpty(output)) output = number.ToString();

            results.Add(output);
            logger.LogInformation(output);

            if (number == 42)
                logger.LogInformation("Did you know? The answer to life, the universe, and everything is 42.");
        }

        return await Task.FromResult(results);
    }
}