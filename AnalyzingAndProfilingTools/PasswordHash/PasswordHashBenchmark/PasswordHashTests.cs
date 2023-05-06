using BenchmarkDotNet.Running;

namespace PasswordHashBenchmark
{
    public class PasswordHashTests
    {
        [Test]
        public void Benchmark()
        {

            BenchmarkRunner.Run<PasswordHashBenchmark>();
            Console.ReadKey();
        }
    }
}