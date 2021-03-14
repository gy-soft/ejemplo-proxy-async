using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProxieAsync
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            using (var logger = new Logger(CsvRow.HEADER))
            {
                logger.AddRow(LogLevel.Debug, new CsvRow(
                    "Starting execution",
                    string.Empty
                ));
                RunAsync(logger).GetAwaiter().GetResult();
                logger.AddRow(LogLevel.Debug, new CsvRow(
                    "Execution completed",
                    string.Empty
                ));
            }
        }

        private static async Task RunAsync(Logger logger)
        {
            Random rand = new Random();
            RandomValues randValues = new RandomValues(rand, logger);
            var tasks = Enumerable.Range(0,99)
                .Select(_ => randValues.GenerateRandomTextAsync());
            await Task.WhenAll(tasks);
        }
    }
}
