using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataAnalysis.Hw3
{
    static class Program
    {
        static void Main(string[] args)
        {
            var educationalData = InputData("../../../EducationalData.csv");
            var educationalInputs = educationalData.PrepareInput().ToArray();
            var educationalAnswers = educationalData.PrepareAnswers().ToArray();
            
            var testData = InputData("../../../TestData.csv");
            var testInputs = testData.PrepareInput().ToArray();
            var testAnswers = testData.PrepareAnswers().ToArray();
            
            var perceptron = new DoublePerceptron(10, 0.0001d, 5000);
            perceptron.Study(educationalInputs, educationalAnswers);
            
            var testError = perceptron.GetErrorsCount(testInputs, testAnswers);
            
            Console.WriteLine($"Errors count on test dataset: {testError}");
        }

        static double[] InputData(string path)
            => File
                .ReadAllLines(path)
                .Select(line => line.Split(';')[7])
                .Select(double.Parse)
                .ToArray();
    }
}