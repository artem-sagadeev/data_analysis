using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using DataSet = DataModel.DataSet;

namespace DataAnalysis.Regression.Block2
{
    static class Program
    {
        static void Main(string[] args)
        {
            var dataSet = InputData("../../../lines.csv");
            var setC = dataSet.GetSetC();
            var setH = dataSet.GetSetH();
            var setL = dataSet.GetSetL();
            var setO = dataSet.GetSetO();
            
            GetFifthModel(setC, setH, setO);
            GetSixthModel(setC, setH, setL, setO);
        }

        static void GetFifthModel(double[] setC, double[] setH, double[] setO)
        {
            var cAverage = setC.Average();
            var inputsCount = setC.Length;
            var variablesCount = 2d;

            var mainMatrix = new DenseMatrix(3)
            {
                [0, 0] = inputsCount, [0, 1] = setH.Sum(), [0, 2] = setO.Sum(),
                [1, 0] = setH.Sum(), [1, 1] = setH.SquaresSum(), [1, 2] = setH.MultipliesSum(setO),
                [2, 0] = setO.Sum(), [2, 1] = setH.MultipliesSum(setO), [2, 2] = setO.SquaresSum()
            };
            var b0Matrix = new DenseMatrix(3)
            {
                [0, 0] = setC.Sum(), [0, 1] = setH.Sum(), [0, 2] = setO.Sum(),
                [1, 0] = setC.MultipliesSum(setH), [1, 1] = setH.SquaresSum(), [1, 2] = setH.MultipliesSum(setO),
                [2, 0] = setC.MultipliesSum(setO), [2, 1] = setH.MultipliesSum(setO), [2, 2] = setO.SquaresSum()
            };
            var b1Matrix = new DenseMatrix(3)
            {
                [0, 0] = inputsCount, [0, 1] = setC.Sum(), [0, 2] = setO.Sum(),
                [1, 0] = setH.Sum(), [1, 1] = setC.MultipliesSum(setH), [1, 2] = setH.MultipliesSum(setO),
                [2, 0] = setO.Sum(), [2, 1] = setC.MultipliesSum(setO), [2, 2] = setO.SquaresSum()
            };
            var b2Matrix = new DenseMatrix(3)
            {
                [0, 0] = inputsCount, [0, 1] = setH.Sum(), [0, 2] = setC.Sum(),
                [1, 0] = setH.Sum(), [1, 1] = setH.SquaresSum(), [1, 2] = setC.MultipliesSum(setH),
                [2, 0] = setO.Sum(), [2, 1] = setH.MultipliesSum(setO), [2, 2] = setC.MultipliesSum(setO)
            };

            var mainMatrixDeterminant = mainMatrix.Determinant();
            var b0 = b0Matrix.Determinant() / mainMatrixDeterminant;
            var b1 = b1Matrix.Determinant() / mainMatrixDeterminant;
            var b2 = b2Matrix.Determinant() / mainMatrixDeterminant;
            var determinationCoefficient = CalculateDeterminationCoefficient(setC, setH, setO, b0, b1, b2, cAverage);
            var adjustedDeterminationCoefficient =
                CalculateAdjustedDeterminationCoefficient(determinationCoefficient, inputsCount, variablesCount);

            Console.WriteLine($"5.Model: {b0} {GetSign(b1)} {Math.Abs(b1)} * h {GetSign(b2)} {Math.Abs(b2)} * o");
            Console.WriteLine($"Determination coefficient: {determinationCoefficient}");
            Console.WriteLine($"Adjusted determination coefficient: {adjustedDeterminationCoefficient}");
        }

        static void GetSixthModel(double[] setC, double[] setH, double[] setL, double[] setO)
        {
            var cAverage = setC.Average();
            var inputsCount = setC.Length;
            var variablesCount = 3d;

            var mainMatrix = new DenseMatrix(4)
            {
                [0, 0] = inputsCount, [0, 1] = setH.Sum(), [0, 2] = setL.Sum(), [0, 3] = setO.Sum(),
                [1, 0] = setH.Sum(), [1, 1] = setH.SquaresSum(), [1, 2] = setH.MultipliesSum(setL), [1, 3] = setH.MultipliesSum(setO),
                [2, 0] = setL.Sum(), [2, 1] = setL.MultipliesSum(setH), [2, 2] = setL.SquaresSum(), [2, 3] = setL.MultipliesSum(setO),
                [3, 0] = setO.Sum(), [3, 1] = setO.MultipliesSum(setH), [3, 2] = setO.MultipliesSum(setL), [3, 3] = setO.SquaresSum()
            };
            var b0Matrix = new DenseMatrix(4)
            {
                [0, 0] = setC.Sum(), [0, 1] = setH.Sum(), [0, 2] = setL.Sum(), [0, 3] = setO.Sum(),
                [1, 0] = setC.MultipliesSum(setH), [1, 1] = setH.SquaresSum(), [1, 2] = setH.MultipliesSum(setL), [1, 3] = setH.MultipliesSum(setO),
                [2, 0] = setC.MultipliesSum(setL), [2, 1] = setL.MultipliesSum(setH), [2, 2] = setL.SquaresSum(), [2, 3] = setL.MultipliesSum(setO),
                [3, 0] = setC.MultipliesSum(setO), [3, 1] = setO.MultipliesSum(setH), [3, 2] = setO.MultipliesSum(setL), [3, 3] = setO.SquaresSum()
            };
            var b1Matrix = new DenseMatrix(4)
            {
                [0, 0] = inputsCount, [0, 1] = setC.Sum(), [0, 2] = setL.Sum(), [0, 3] = setO.Sum(),
                [1, 0] = setH.Sum(), [1, 1] = setC.MultipliesSum(setH), [1, 2] = setH.MultipliesSum(setL), [1, 3] = setH.MultipliesSum(setO),
                [2, 0] = setL.Sum(), [2, 1] = setC.MultipliesSum(setL), [2, 2] = setL.SquaresSum(), [2, 3] = setL.MultipliesSum(setO),
                [3, 0] = setO.Sum(), [3, 1] = setC.MultipliesSum(setO), [3, 2] = setO.MultipliesSum(setL), [3, 3] = setO.SquaresSum()
            };
            var b2Matrix = new DenseMatrix(4)
            {
                [0, 0] = inputsCount, [0, 1] = setH.Sum(), [0, 2] = setC.Sum(), [0, 3] = setO.Sum(),
                [1, 0] = setH.Sum(), [1, 1] = setH.SquaresSum(), [1, 2] = setC.MultipliesSum(setH), [1, 3] = setH.MultipliesSum(setO),
                [2, 0] = setL.Sum(), [2, 1] = setL.MultipliesSum(setH), [2, 2] = setC.MultipliesSum(setL), [2, 3] = setL.MultipliesSum(setO),
                [3, 0] = setO.Sum(), [3, 1] = setO.MultipliesSum(setH), [3, 2] = setC.MultipliesSum(setO), [3, 3] = setO.SquaresSum()
            };
            var b3Matrix = new DenseMatrix(4)
            {
                [0, 0] = inputsCount, [0, 1] = setH.Sum(), [0, 2] = setL.Sum(), [0, 3] = setC.Sum(),
                [1, 0] = setH.Sum(), [1, 1] = setH.SquaresSum(), [1, 2] = setH.MultipliesSum(setL), [1, 3] = setC.MultipliesSum(setH),
                [2, 0] = setL.Sum(), [2, 1] = setL.MultipliesSum(setH), [2, 2] = setL.SquaresSum(), [2, 3] = setC.MultipliesSum(setL),
                [3, 0] = setO.Sum(), [3, 1] = setO.MultipliesSum(setH), [3, 2] = setO.MultipliesSum(setL), [3, 3] = setC.MultipliesSum(setO)
            };

            var mainMatrixDeterminant = mainMatrix.Determinant();
            var b0 = b0Matrix.Determinant() / mainMatrixDeterminant;
            var b1 = b1Matrix.Determinant() / mainMatrixDeterminant;
            var b2 = b2Matrix.Determinant() / mainMatrixDeterminant;
            var b3 = b3Matrix.Determinant() / mainMatrixDeterminant;
            var determinationCoefficient =
                CalculateDeterminationCoefficient(setC, setH, setL, setO, b0, b1, b2, b3, cAverage);
            var adjustedDeterminationCoefficient =
                CalculateAdjustedDeterminationCoefficient(determinationCoefficient, inputsCount, variablesCount);

            Console.WriteLine($"6.Model: {b0} {GetSign(b1)} {Math.Abs(b1)} * h {GetSign(b2)} {Math.Abs(b2)} * l {GetSign(b3)} {Math.Abs(b3)} * o");
            Console.WriteLine($"Determination coefficient: {determinationCoefficient}");
            Console.WriteLine($"Adjusted determination coefficient: {adjustedDeterminationCoefficient}");
        }
        
        static double CalculateAdjustedDeterminationCoefficient(
            double determinationCoefficient, double inputsCount, double variablesCount)
            => Math.Sqrt(1 - (1 - determinationCoefficient * determinationCoefficient) 
                * (inputsCount - 1) / (inputsCount - variablesCount));
        
        static double CalculateDeterminationCoefficient(
            double[] setC, double[] setH, double[] setL, double[] setO, double b0, double b1, double b2, double b3, double cAverage)
            => Math.Sqrt(setH.Select((h, i) => Math.Pow(b0 + b1 * h + b2 * setL[i] + b3 * setO[i] - cAverage, 2)).Sum() /
                         setC.Sum(c => Math.Pow(c - cAverage, 2)));
        
        static double CalculateDeterminationCoefficient(
            double[] setC, double[] setH, double[] setO, double b0, double b1, double b2, double cAverage)
            => Math.Sqrt(setH.Select((h, i) => Math.Pow(b0 + b1 * h + b2 * setO[i] - cAverage, 2)).Sum() /
                         setC.Sum(c => Math.Pow(c - cAverage, 2)));

        static string GetSign(double t)
            => t < 0 ? "-" : "+";
        
        static double SquaresSum(this double[] set)
            => set.Sum(t => t * t);
        
        static double MultipliesSum(this double[] firstSet, double[] secondSet)
            => firstSet.Select((t, i) => t * secondSet[i]).Sum();

        static DataSet InputData(string path)
            => new (File.ReadAllLines(path));
    }
}