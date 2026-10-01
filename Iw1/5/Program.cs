
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

class Program
{
    static long result;

    static void Main()
    {
        const int repetitions = 20;

        using (StreamWriter file = new StreamWriter("cache_results.csv"))
        {
            file.WriteLine("SizeBytes,AveragePassMs");

            for (long size = 4 * 1024;
                 size <= 32L * 1024 * 1024;
                 size *= 2)
            {
                int n = (int)(size / sizeof(long));
                long[] arr = new long[n];

                for (int i = 0; i < n; i++)
                    arr[i] = i % 101;

                // Прогрівання
                long sum = 0;
                for (int i = 0; i < n; i++)
                    sum += arr[i];
                result = sum;

                Stopwatch sw = new Stopwatch();
                double totalMs = 0;

                for (int r = 0; r < repetitions; r++)
                {
                    sum = 0;
                    sw.Restart();

                    for (int i = 0; i < n; i++)
                        sum += arr[i];

                    sw.Stop();
                    result = sum;
                    totalMs += sw.Elapsed.TotalMilliseconds;
                }

                double avg = totalMs / repetitions;

                Console.WriteLine(
                    $"{size},{avg.ToString("F6", CultureInfo.InvariantCulture)}");

                file.WriteLine(
                    $"{size},{avg.ToString("F6", CultureInfo.InvariantCulture)}");
            }
        }

        Console.WriteLine("\nРезультати збережено у cache_results.csv");
        Console.WriteLine($"Контрольна сума: {result}");
    }
}