using System.Collections;
using System.Diagnostics;
using System.Text;

namespace BugTrackerApp
{
    public sealed class GenericCollection<T> : IList<T>
    {
        private readonly List<T> _items = new();

        public T this[int index]
        {
            get => _items[index];
            set => _items[index] = value;
        }

        public int Count => _items.Count;
        public bool IsReadOnly => false;

        public void Add(T item) => _items.Add(item);
        public void Clear() => _items.Clear();
        public bool Contains(T item) => _items.Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);
        public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();
        public int IndexOf(T item) => _items.IndexOf(item);
        public void Insert(int index, T item) => _items.Insert(index, item);
        public bool Remove(T item) => _items.Remove(item);
        public void RemoveAt(int index) => _items.RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public static class CollectionBenchmark
    {
        private const int RecordCount = 1_000_000;

        public static string BuildReport()
        {
            var output = new StringBuilder();
            output.AppendLine("Collection Benchmark: 1,000,000 Records");
            output.AppendLine($"Target collection size: {RecordCount:N0} items");
            output.AppendLine(new string('=', 70));
            output.AppendLine("BENCHMARK RESULTS");
            output.AppendLine(new string('-', 70));
            output.AppendLine($"{"Operation",-26}{"Time",12}");
            output.AppendLine(new string('-', 70));

            var arrayList = new ArrayList();
            var genericCollection = new GenericCollection<BugReport>();
            var arrayListLookupItem = new BugReport("LookupDev", "LOOKUP-000000", "Lookup summary");
            var genericLookupItem = new BugReport("LookupDev", "LOOKUP-000000", "Lookup summary");

            AppendMeasurement(output, "ArrayList.Add", () =>
            {
                for (var i = 0; i < RecordCount; i++)
                {
                    arrayList.Add(new BugReport($"Dev{i}", $"BUG-{i}", $"Summary {i}"));
                }
            });

            AppendMeasurement(output, "GenericCollection.Add", () =>
            {
                for (var i = 0; i < RecordCount; i++)
                {
                    genericCollection.Add(new BugReport($"Dev{i}", $"BUG-{i}", $"Summary {i}"));
                }
            });

            arrayList.Add(arrayListLookupItem);
            genericCollection.Add(genericLookupItem);

            AppendMeasurement(output, "ArrayList.Contains", () =>
            {
                var found = arrayList.Contains(arrayListLookupItem);
                output.AppendLine($"  Result: {found}");
            });

            AppendMeasurement(output, "GenericCollection.Contains", () =>
            {
                var found = genericCollection.Contains(genericLookupItem);
                output.AppendLine($"  Result: {found}");
            });

            AppendMeasurement(output, "ArrayList.IndexOf", () =>
            {
                var index = arrayList.IndexOf(arrayListLookupItem);
                output.AppendLine($"  Result: {index}");
            });

            AppendMeasurement(output, "GenericCollection.IndexOf", () =>
            {
                var index = genericCollection.IndexOf(genericLookupItem);
                output.AppendLine($"  Result: {index}");
            });

            output.AppendLine(new string('-', 70));
            output.AppendLine("Benchmark complete.");
            return output.ToString();
        }

        public static void Run()
        {
            Console.WriteLine(BuildReport());
        }

        private static void AppendMeasurement(StringBuilder output, string label, Action action)
        {
            var stopwatch = Stopwatch.StartNew();
            action();
            stopwatch.Stop();
            output.AppendLine($"{label,-26} {stopwatch.Elapsed.TotalMilliseconds,12:N2} ms");
        }
    }
}
