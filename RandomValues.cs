using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace ProxieAsync
{
    class RandomValues
    {
        private readonly Random random;
        private readonly Logger logger;

        public RandomValues(Random random, Logger logger)
        {
            this.random = random;
            this.logger = logger;
        }

        public async Task GenerateRandomTextAsync()
        {
            byte[] buffer = new byte[256];
            random.NextBytes(buffer);
            using (var originalStream = new MemoryStream(buffer))
            using (var compressedStream = new MemoryStream())
            {
                using (DeflateStream compressionStream = new DeflateStream(compressedStream, CompressionLevel.Optimal))
                {
                    await originalStream.CopyToAsync(compressionStream);
                }
                logger.AddRow(
                    LogLevel.Info,
                    new CsvRow(
                        Convert.ToBase64String(buffer),
                        Convert.ToBase64String(compressedStream.ToArray())
                    )
                );
            }
        }
    }
}