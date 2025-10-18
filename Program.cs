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
}
