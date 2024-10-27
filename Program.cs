using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ComplexFizzBuzz
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging(config => config.AddConsole())
                .AddSingleton<IRandomService, RandomService>()
                .AddSingleton<IShuffleService, ShuffleService>()
                .AddSingleton<IFizzBuzzService, FizzBuzzService>()
                .BuildServiceProvider();

            var logger = serviceProvider.GetService<ILogger<Program>>();
            var fizzBuzzService = serviceProvider.GetService<IFizzBuzzService>();

            if (fizzBuzzService != null)
            {
                var results = await fizzBuzzService.GenerateFizzBuzzAsync();
                foreach (var result in results)
                {
                    Console.WriteLine(result);
                }
            }
            else
            {
                logger?.LogError("FizzBuzzService is not available.");
            }
        }
    }

    public interface IFizzBuzzService
    {
        Task<IEnumerable<string>> GenerateFizzBuzzAsync();
    }

    public class FizzBuzzService : IFizzBuzzService
    {
        private readonly IRandomService _randomService;
        private readonly IShuffleService _shuffleService;
        private readonly ILogger<FizzBuzzService> _logger;

        public FizzBuzzService(IRandomService randomService, IShuffleService shuffleService, ILogger<FizzBuzzService> logger)
        {
            _randomService = randomService;
            _shuffleService = shuffleService;
            _logger = logger;
        }

        public async Task<IEnumerable<string>> GenerateFizzBuzzAsync()
        {
            var numbers = Enumerable.Range(1, 100).ToList();
            numbers = _shuffleService.Shuffle(numbers).ToList();
            var results = new List<string>();

            foreach (var number in numbers)
            {
                string output = "";

                if (number % 3 == 0) output += "Fizz";
                if (number % 5 == 0) output += "Buzz";
                if (string.IsNullOrEmpty(output)) output = number.ToString();

                results.Add(output);
                _logger.LogInformation(output);

                if (number == 42)
                    _logger.LogInformation("Did you know? The answer to life, the universe, and everything is 42.");
            }

            return await Task.FromResult(results);
        }
    }

    public interface IRandomService
    {
        int Next(int minValue, int maxValue);
    }

    public class RandomService : IRandomService
    {
        private readonly Random _random = new Random();

        public int Next(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }
    }

    public interface IShuffleService
    {
        IEnumerable<int> Shuffle(IEnumerable<int> source);
    }

    public class ShuffleService : IShuffleService
    {
        private readonly IRandomService _randomService;

        public ShuffleService(IRandomService randomService)
        {
            _randomService = randomService;
        }

        public IEnumerable<int> Shuffle(IEnumerable<int> source)
        {
            var list = source.ToList();
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = _randomService.Next(0, i + 1);
                var temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
            return list;
        }
    }
}
