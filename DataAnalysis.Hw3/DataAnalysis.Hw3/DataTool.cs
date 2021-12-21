using System.Collections.Generic;
using System.Linq;

namespace DataAnalysis.Hw3
{
    public static class DataTool
    {
        public static IEnumerable<double[]> PrepareInput(this double[] data)
        {
            for (var i = 0; i < data.Length - 13; i++)
            {
                yield return data.Skip(i).Take(10).ToArray();
            }
        }
        
        public static IEnumerable<double> PrepareAnswers(this double[] data)
        {
            for (var i = 10; i < data.Length - 3; i++)
            {
                yield return GetTrendDirection(
                    data.Skip(i).Take(3).Select((_, x) => x).ToList(),
                    data.Skip(i).Take(3).Select((y, _) => y).ToList());
            }
        }

        private static double GetTrendDirection(List<int> x, List<double> y)
        {
            var xAverage = x.Average();
            var yAverage = y.Average();
            var xyAverage = y.Select((t, i) => x[i] * t).Sum() / y.Count;
            var x2 = x.Select(e => e * e).Sum() / x.Count;
            return (xyAverage - xAverage * yAverage) / (x2 - xAverage * xAverage) > 0 ? 1 : 0;
        }
    }
}