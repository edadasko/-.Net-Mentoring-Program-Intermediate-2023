using BenchmarkDotNet.Attributes;
using PasswordHash;

namespace PasswordHashBenchmark
{
    public class PasswordHashBenchmark
    {
        private const string SampleText = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private const int SaltSize = 8;

        private static readonly Random Random = new();

        private static readonly Rfc2898Hash Hash = new();


        [Benchmark(Description = "InitialHashAlgorithm")]
        public string InitialAlgorithm()
        {
            var salt = GetRandomByteArray(SaltSize);
            return Hash.GeneratePasswordHashUsingSalt(SampleText, salt);
        }


        private byte[] GetRandomByteArray(int size)
        {
            byte[] b = new byte[size];
            Random.NextBytes(b);
            return b;
        }
    }
}
