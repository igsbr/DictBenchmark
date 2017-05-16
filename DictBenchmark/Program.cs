using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DictBenchmark
{
    public struct SimpleStruct
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }

    public class SimpleStructEqComparer: IEqualityComparer<SimpleStruct>
    {
        public bool Equals(SimpleStruct x, SimpleStruct y)
        {
            return x.Name == y.Name && x.Id == y.Id;
        }

        public int GetHashCode(SimpleStruct obj)
        {
            return obj.Name.GetHashCode() ^ obj.Id.GetHashCode();
        }
    }

    public struct EquatableStruct : IEquatable<EquatableStruct>
    {
        public string Name { get; set; }
        public int Id { get; set; }

        public bool Equals(EquatableStruct other)
        {
            return Name == other.Name && Id == other.Id;
        }
    }

    public class SimpleClass
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }

    public class SimpleClassEqComparer : IEqualityComparer<SimpleClass>
    {
        public bool Equals(SimpleClass x, SimpleClass y)
        {
            return x.Name == y.Name && x.Id == y.Id;
        }

        public int GetHashCode(SimpleClass obj)
        {
            return obj.Name.GetHashCode() ^ obj.Id.GetHashCode();
        }
    }

    public class Test
    {
        const int N = 100;
        //const int Choosed = 5;

        // structs

        private SimpleStruct[] simpleStructs = new SimpleStruct[N];
        private EquatableStruct[] equatableStructs = new EquatableStruct[N];

        private Dictionary<SimpleStruct, int> simpleStructDictionary;
        private Dictionary<SimpleStruct, int> simpleStructDictionaryWithComparer;
        private Dictionary<EquatableStruct, int> equatableStructDictionary;

        // classes

        private SimpleClass[] simpleClasses = new SimpleClass[N];
        private Dictionary<SimpleClass, int> simpleClassDictionary;
        private Dictionary<SimpleClass, int> simpleClassDictionaryWithComparer;

        // picked for test

        //private SimpleStruct choosedSimpleStruct;
        //private EquatableStruct choosedEquatableStruct;
        //private SimpleClass choosedSimpleClass;

        public Test()
        {
            for (int i = 0; i < N; i++)
            {
                //var name = string.Join("", Enumerable.Repeat("a", i + 1));
                var name = $"hello{i}";
                simpleStructs[i] = new SimpleStruct() { Name = name, Id = i };
                equatableStructs[i] = new EquatableStruct() { Name = name, Id = i};
                simpleClasses[i] = new SimpleClass() { Name = name, Id = i };
            }

            simpleStructDictionary = new Dictionary<SimpleStruct, int>();
            simpleStructDictionaryWithComparer = new Dictionary<SimpleStruct, int>(new SimpleStructEqComparer());

            foreach (var s in simpleStructs)
            {
                simpleStructDictionary.Add(s, s.Id);
                simpleStructDictionaryWithComparer.Add(s, s.Id);
            }

            simpleClassDictionary = new Dictionary<SimpleClass, int>();
            simpleClassDictionaryWithComparer = new Dictionary<SimpleClass, int>(new SimpleClassEqComparer());

            foreach (var s in simpleClasses)
            {
                simpleClassDictionary.Add(s, s.Id);
                simpleClassDictionaryWithComparer.Add(s, s.Id);
            }

            equatableStructDictionary = new Dictionary<EquatableStruct, int>();

            foreach (var e in equatableStructs)
            {
                equatableStructDictionary.Add(e, e.Id);
            }

            //choosedSimpleStruct = simpleStructs[Choosed];
            //choosedEquatableStruct = equatableStructs[Choosed];
            //choosedSimpleClass = simpleClasses[Choosed];
        }

        [Benchmark]
        public int SimpleStructTest()
        {
            int sum = 0;

            for (int i = 0; i < simpleStructs.Length; i++)
            {
                sum += simpleStructDictionary[simpleStructs[i]];
            }

            return sum;

            //return simpleStructDictionary[choosedSimpleStruct];
        }

        [Benchmark]
        public int SimpleStructWithComparerTest()
        {
            int sum = 0;

            for (int i = 0; i < simpleStructs.Length; i++)
            {
                sum += simpleStructDictionaryWithComparer[simpleStructs[i]];
            }

            return sum;

            //return simpleStructDictionaryWithComparer[choosedSimpleStruct];
        }

        [Benchmark]
        public int EquatableStructTest()
        {
            int sum = 0;

            for (int i = 0; i < equatableStructs.Length; i++)
            {
                sum += equatableStructDictionary[equatableStructs[i]];
            }

            return sum;

            //return equatableStructDictionary[choosedEquatableStruct];
        }

        [Benchmark]
        public int SimpleClassTest()
        {
            int sum = 0;

            for (int i = 0; i < simpleClasses.Length; i++)
            {
                sum += simpleClassDictionary[simpleClasses[i]];
            }

            return sum;

            //return simpleClassDictionary[choosedSimpleClass];
        }

        [Benchmark]
        public int SimpleClassWithComparerTest()
        {
            int sum = 0;

            for (int i = 0; i < simpleClasses.Length; i++)
            {
                sum += simpleClassDictionaryWithComparer[simpleClasses[i]];
            }

            return sum;

            //return simpleClassDictionaryWithComparer[choosedSimpleClass];
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<Test>();

            Console.ReadKey(true);
        }
    }
}
