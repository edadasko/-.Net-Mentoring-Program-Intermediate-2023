using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using PasswordHash;

namespace Benchmark
{
    public class Program
    {
        public class PasswordHashBenchmark
        {
            private const string SampleText = "test_password_test_password_test_password";
            private static readonly byte[] Salt = GetRandomByteArray(16);
            private static readonly Rfc2898Hash Hash = new();


            [Benchmark(Description = "InitialHashAlgorithm")]
            public string InitialAlgorithm()
            {
                return Hash.GeneratePasswordHashUsingSalt(SampleText, Salt);
            }

            [Benchmark(Description = "UpdatedHashAlgorithm")]
            public string UpdatedAlgorithm()
            {
                return Hash.OptimizedGeneratePasswordHashUsingSalt(SampleText, Salt);
            }

            private static byte[] GetRandomByteArray(int size)
            {
                Random random = new();
                byte[] b = new byte[size];
                random.NextBytes(b);
                return b;
            }
        }

        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<PasswordHashBenchmark>();
            Console.ReadKey();
        }
    }
}