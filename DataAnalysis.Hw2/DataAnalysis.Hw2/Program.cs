using System;
using System.IO;
using System.Linq;

namespace DataAnalysis.Hw2
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = InputData();
            var answers = lines.Select(line => line[0] == 4 ? 1 : 0).ToArray();

            for (var i = 1; i <= 4; i++)
            {
                var inputs = lines.Select(line => line.Skip(1).Take(i).ToArray()).ToArray();
                var perceptron = new SinglePerceptron(i, 0.000001d);
                perceptron.Fit(inputs, answers);
            }
        }

        private static int[][] InputData()
            => File
                .ReadAllLines("../../../vk_perc.csv")
                .Select(e => e
                    .Split(';')
                    .Select(int.Parse)
                    .ToArray())
                .ToArray();
    }
}