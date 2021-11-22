using System;
using System.IO;
using System.Linq;
using DataSet = DataModel.DataSet;

namespace DataAnalysis.Regression.Block1
{
    static class Program
    {
        static void Main(string[] args)
        {
            var dataSet = InputData("../../../lines.csv");
            var setC = dataSet.GetSetC();
            var setCAverage = setC.Average();
            
            
            var setT = dataSet.GetSetT();
            var firstCoefficient = CalculateCoefficient(setT, setC);
            var firstFreeMember = CalculateFreeMember(setT, setC, firstCoefficient);
            var firstDeterminationCoefficient =
                CalculateDeterminationCoefficient(setT, setC, firstCoefficient, firstFreeMember, setCAverage);
            Console.Write($"1.Formula: {firstCoefficient}x {GetSign(firstFreeMember)} {Math.Abs(firstFreeMember)}");
            Console.WriteLine($", Determination coefficient = {firstDeterminationCoefficient}");
            
            var setO = dataSet.GetSetO();
            var secondCoefficient = CalculateCoefficient(setO, setC);
            var secondFreeMember = CalculateFreeMember(setO, setC, secondCoefficient);
            var secondDeterminationCoefficient =
                CalculateDeterminationCoefficient(setO, setC, secondCoefficient, secondFreeMember, setCAverage);
            Console.Write($"2.Formula: {secondCoefficient}x {GetSign(secondFreeMember)} {Math.Abs(secondFreeMember)}");
            Console.WriteLine($", Determination coefficient = {secondDeterminationCoefficient}");
            
            var setL = dataSet.GetSetL();
            var thirdCoefficient = CalculateCoefficient(setL, setC);
            var thirdFreeMember = CalculateFreeMember(setL, setC, thirdCoefficient);
            var thirdDeterminationCoefficient =
                CalculateDeterminationCoefficient(setL, setC, thirdCoefficient, thirdFreeMember, setCAverage);
            Console.Write($"3.Formula: {thirdCoefficient}x {GetSign(thirdFreeMember)} {Math.Abs(thirdFreeMember)}");
            Console.WriteLine($", Determination coefficient = {thirdDeterminationCoefficient}");
            
            var setH = dataSet.GetSetH();
            var fourthCoefficient = CalculateCoefficient(setH, setC);
            var fourthFreeMember = CalculateFreeMember(setH, setC, fourthCoefficient);
            var fourthDeterminationCoefficient =
                CalculateDeterminationCoefficient(setH, setC, fourthCoefficient, fourthFreeMember, setCAverage);
            Console.Write($"4.Formula: {fourthCoefficient}x {GetSign(fourthFreeMember)} {Math.Abs(fourthFreeMember)}");
            Console.WriteLine($", Determination coefficient = {fourthDeterminationCoefficient}");
        }

        static double CalculateDeterminationCoefficient(
            double[] setX, double[] setY, double coefficient, double freeMember, double yAverage)
            => Math.Sqrt(setX.Sum(x => Math.Pow(coefficient * x + freeMember - yAverage, 2)) /
                         setY.Sum(y => Math.Pow(y - yAverage, 2)));
        
        static string GetSign(double t)
            => t < 0 ? "-" : "+";
        
        static double CalculateCoefficient(double[] setX, double[] setY)
            => (setX.Length * setX.Select((x, i) => x * setY[i]).Sum() - setX.Sum() * setY.Sum()) /
               (setX.Length * setX.Sum(x => x * x) - setX.Sum() * setX.Sum());

        static double CalculateFreeMember(double[] setX, double[] setY, double coefficient)
            => (setY.Sum() - coefficient * setX.Sum()) / setX.Length;

        static DataSet InputData(string path)
            => new (File.ReadAllLines(path));
    }
}
