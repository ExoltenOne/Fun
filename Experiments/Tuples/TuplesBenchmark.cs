using System;
using BenchmarkDotNet.Attributes;

namespace Tuples
{
    [MemoryDiagnoser]
    public class TuplesBenchmark
    {
        private const int NumberOfIterations = 10000;

        [Benchmark]
        public void ReferenceTuple()
        {
            long counter = 0L;
            for (var n = 0; n < NumberOfIterations; n++)
            {
                for (int i = 0; i < 100; i++)
                {
                    var tuple = Tuple.Create("string", 10L, 777m, 45.45);

                    counter += tuple.Item2;
                }
            }

            Console.WriteLine(counter);
        }

        [Benchmark]
        public void ValueTuple()
        {
            long counter = 0L;
            for (var n = 0; n < NumberOfIterations; n++)
            {
                for (int i = 0; i < 100; i++)
                {
                    var tuple = ("string", 10L, 777m, 45.45);

                    counter += tuple.Item2;
                }
            }

            Console.WriteLine(counter);
        }
    }
}
