///
/// Created by Laurent Fearon
/// Contact Info: Laurent.Fearon@gmail.com
/// Project Site: https://github.com/LaurentFearon/LaurentFearon.Statistics
/// Apache License 2.0
/// See License file: https://github.com/LaurentFearon/LaurentFearon.Statistics/blob/master/LICENSE.md
///

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace LaurentFearon.Statistics
{
    public static class Calculations
    {
        /// <summary>
        /// Calculates the start of each class based on specified number of classes and class width
        /// </summary>
        /// <param name="values">Collection of vales</param>
        /// <param name="classWidth">class width</param>
        /// <param name="numberOfClasses">number of classes</param>
        /// <returns></returns>
        public static IEnumerable<double> CalculateClasses(IEnumerable<double> values, double classWidth, int numberOfClasses)
        {
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");
            Contract.Requires<ArgumentException>(numberOfClasses > 1, "number of classes must not be greater than 1!");

            List<double> classValues = new List<double>();
            double startValue = values.Min() - classWidth / 2.0 - classWidth;
            double lastValue = startValue;

            classValues.Add(startValue);
            for (int i = 0; i < numberOfClasses-1; i++)
            {
                double value = lastValue + classWidth;
                classValues.Add(value);
                lastValue = value;
            }

            return classValues;
        }

        /// <summary>
        /// Calculates the start of each class based on specified number of classes and class width
        /// </summary>
        /// <typeparam name="T">Type of values</typeparam>
        /// <param name="values">Collection of vales</param>
        /// <param name="classWidth">class width</param>
        /// <param name="numberOfClasses">number of classes</param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IEnumerable<double> CalculateClasses<T>(IEnumerable<T> values, double classWidth, int numberOfClasses, Func<T, double> selector)
        {
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");
            Contract.Requires<ArgumentException>(selector != null, "selector must not be null!");

            return CalculateClasses(values.Select(selector), classWidth, numberOfClasses);
        }

        public static List<KeyValuePair<double, int>> GetClassFrequencies(IEnumerable<double> classes, IEnumerable<double> values)
        {
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");
            Contract.Requires<ArgumentException>(classes != null && classes.Count() > 0, "classes must not be null or empty!");

            List<KeyValuePair<double, int>> classFrequencies = new List<KeyValuePair<double, int>>();

            for (int i = 0; i < classes.Count(); i++)
            {
                int count = -1;

                if(i == 0)
                {
                    count = values.Where(x => x < classes.ElementAt(i)).Count();
                }
                else if (i == classes.Count() -1)
                {
                    count = values.Where(x => x >= classes.ElementAt(i)).Count();
                }
                else
                {
                   count = values.Where(x => x < classes.ElementAt(i) && x >= classes.ElementAt(i - 1)).Count();
                }

                classFrequencies.Add(new KeyValuePair<double, int>(classes.ElementAt(i), count));
            }

            return classFrequencies;
        }

        public static List<KeyValuePair<double, int>> GetClassFrequencies<T>(IEnumerable<double> classes, IEnumerable<T> values, Func<T, double> selector)
        {
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");
            Contract.Requires<ArgumentException>(selector != null, "selector must not be null!");

            return GetClassFrequencies(classes, values.Select(selector));
        }

        public static double CalculateRange(IEnumerable<double> values)
        {
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");

            return values.Max() - values.Min();
        }

        public static double CalculateRange<T>(IEnumerable<T> values, Func<T, double> selector)
        {
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");
            Contract.Requires<ArgumentException>(selector != null, "selector must not be null!");

            return CalculateRange(values.Select(selector));
        }

        public static double CalculateClassWidth(IEnumerable<double> values, double range)
        {
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");

            int k = (int)Math.Round(Math.Sqrt(values.Count()), 0);

            return range / (double)k;
        }

        public static double CalculateClassWidth<T>(IEnumerable<T> values, double range, Func<T, double> selector)
        {
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");
            Contract.Requires<ArgumentException>(selector != null, "selector must not be null!");

            return CalculateClassWidth(values.Select(selector), range);
        }

        public static double CalculateNormalDistribution(double value, double mean, double standardDeviation)
        {
            Contract.Requires<ArgumentException>(standardDeviation != 0, "standardDeviation must not be zero!");

            return 1 / (standardDeviation * Math.Sqrt(2 * Math.PI)) * Math.Exp(-Math.Pow(value - mean, 2) / (2 * standardDeviation * standardDeviation));
        }

        public static double CalculateStandardDeviation_Population(IEnumerable<double> values, double mean)
        {
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");

            return Math.Sqrt(CalculateVariance_Population(values, mean));
        }

        public static double CalculateStandardDeviation_Population<T>(IEnumerable<T> values, double mean, Func<T, double> selector)
        {
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");
            Contract.Requires<ArgumentException>(selector != null, "selector must not be null!");

            return CalculateStandardDeviation_Population(values.Select(selector), mean);
        }

        public static double CalculateVariance_Population(IEnumerable<double> values, double mean)
        {
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");

            double sum = 0;
            foreach (double value in values)
            {
                sum += Math.Pow(value - mean, 2);
            }

            return sum / ((double)values.Count());
        }

        public static double CalculateVariance_Population<T>(IEnumerable<T> values, double mean, Func<T, double> selector)
        {
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");
            Contract.Requires<ArgumentException>(selector != null, "selector must not be null!");

            return CalculateVariance_Population(values.Select(selector), mean);
        }

        public static double CalculateStandardDeviation_Sample(IEnumerable<double> values, double mean)
        {
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");

            return Math.Sqrt(CalculateVariance_Sample(values, mean));
        }

        public static double CalculateStandardDeviation_Sample<T>(IEnumerable<T> values, double mean, Func<T, double> selector)
        {
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");
            Contract.Requires<ArgumentException>(selector != null, "selector must not be null!");

            return CalculateStandardDeviation_Sample(values.Select(selector), mean);
        }

        public static double CalculateVariance_Sample(IEnumerable<double> values, double mean)
        {
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");

            double sum = 0;
            foreach (double value in values)
            {
                sum += Math.Pow(value - mean, 2);
            }

            return sum / ((double)values.Count() - 1);
        }

        public static double CalculateVariance_Sample<T>(IEnumerable<T> values, double mean, Func<T, double> selector)
        {
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");
            Contract.Requires<ArgumentException>(selector != null, "selector must not be null!");

            return CalculateVariance_Sample(values.Select(selector), mean);
        }

        public static List<KeyValuePair<double, int>> GetCumulativeClassFrequencies(IEnumerable<double> classes, IEnumerable<double> values)
        {
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");
            Contract.Requires<ArgumentException>(classes != null && classes.Count() > 0, "classes must not be null or empty!");

            List<KeyValuePair<double, int>> classFrequencies = GetClassFrequencies(classes, values);

            List<KeyValuePair<double, int>> rCumulativeFrequencies = new List<KeyValuePair<double, int>>();

            for (int i=0; i< classFrequencies.Count; i++)
            {
                if (i > 0)
                {
                    rCumulativeFrequencies.Add(new KeyValuePair<double, int>(classFrequencies.ElementAt(i).Key, classFrequencies.ElementAt(i).Value + rCumulativeFrequencies.ElementAt(i - 1).Value));
                }
                else
                {
                    rCumulativeFrequencies.Add(new KeyValuePair<double, int>(classFrequencies.ElementAt(i).Key, classFrequencies.ElementAt(i).Value));
                }
            }

            return rCumulativeFrequencies;
        }

        public static List<KeyValuePair<double, int>> GetCumulativeClassFrequencies<T>(IEnumerable<double> classes, IEnumerable<T> values, Func<T, double> selector)
        {
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");
            Contract.Requires<ArgumentException>(selector != null, "selector must not be null!");

            return GetCumulativeClassFrequencies(classes, values.Select(selector));
        }

        public static LineFormula GetRegressionLine(IEnumerable<double> valuesX, IEnumerable<double> valuesY)
        {
            Contract.Requires<ArgumentException>(valuesX != null && valuesX.Count() > 0, "valuesX must not be null or empty!");
            Contract.Requires<ArgumentException>(valuesY != null && valuesY.Count() > 0, "valuesY must not be null or empty!");
            Contract.Requires<ArgumentException>(valuesX.Count() == valuesY.Count(), "valuesX and valuesY must have same amount of values!");

            LineFormula lf = new LineFormula();

            double avgX = valuesX.Average();
            double avgY = valuesY.Average();
            double avgXY = 0;

            for(int i=0; i<valuesX.Count(); i++)
            {
                avgXY += (valuesX.ElementAt(i) * valuesY.ElementAt(i));
            }
            avgXY = avgXY / valuesX.Count();

            double avgXX = 0;
            for (int i = 0; i < valuesX.Count(); i++)
            {
                avgXX += (valuesX.ElementAt(i) * valuesX.ElementAt(i));
            }
            avgXX = avgXX / valuesX.Count();

            double slope = (avgX * avgY - avgXY) / (avgX * avgX - avgXX);
            double intercept = avgY - slope * avgX;

            lf.Intercept = intercept;
            lf.Slope = slope;
            return lf;
        }

        public static LineFormula GetRegressionLine<T>(IEnumerable<T> values, Func<T, double> selectorX, Func<T, double> selectorY)
        {
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");
            Contract.Requires<ArgumentException>(selectorX != null, "selector must not be null!");
            Contract.Requires<ArgumentException>(selectorY != null, "selector must not be null!");

            return GetRegressionLine(values.Select(selectorX), values.Select(selectorY));
        }

        public static LineFormula GetRegressionLine<T,U>(IEnumerable<T> valuesX, IEnumerable<U> valuesY, Func<T, double> selectorX, Func<U, double> selectorY)
        {
            Contract.Requires<ArgumentException>(valuesX != null && valuesX.Count() > 0, "valuesX must not be null or empty!");
            Contract.Requires<ArgumentException>(valuesY != null && valuesY.Count() > 0, "valuesY must not be null or empty!");
            Contract.Requires<ArgumentException>(selectorX != null, "selector must not be null!");
            Contract.Requires<ArgumentException>(selectorY != null, "selector must not be null!");

            return GetRegressionLine(valuesX.Select(selectorX), valuesY.Select(selectorY));
        }

        private static System.Windows.Forms.DataVisualization.Charting.Chart chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
        public static double CalculateInverseNormalDistribution(double value)
        {
            return chart.DataManipulator.Statistics.InverseNormalDistribution(value);
        }

        public static double Average(IEnumerable<double> values)
        {
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");

            return values.Average();
        }

        public static double Average<T>(IEnumerable<T> values, Func<T, double> selector)
        {
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");
            Contract.Requires<ArgumentException>(selector != null, "selector must not be null!");

            return values.Average(selector);
        }

        public static double Median(IEnumerable<double> values)
        {//todo: Test
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");

            var orderedValues = values.OrderBy(x => x);
            if(orderedValues.Count() % 2 == 0)
            {
                int middle = orderedValues.Count() / 2;
                double dbl1 = orderedValues.ElementAt(middle - 1);
                double dbl2 = orderedValues.ElementAt(middle);

                return (dbl1 + dbl2) / 2.0;
            }
            else
            {
                int middle = (int)(orderedValues.Count() / 2.0);
                return orderedValues.ElementAt(middle);
            }
        }

        public static double Median<T>(IEnumerable<T> values, Func<T, double> selector)
        {
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");
            Contract.Requires<ArgumentException>(selector != null, "selector must not be null!");

            return Median(values.Select(selector));
        }

        public static double Mode(IEnumerable<double> values)
        {
            //todo: Test
            //todo: mode can have more than one solution
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");

            var distinctValues = values.Distinct();
            KeyValuePair<double, int> kvpHighestCount = new KeyValuePair<double, int>();

            foreach(var value in distinctValues)
            {
                int count = values.Count(x => x == value);
                if(count > kvpHighestCount.Value)
                {
                    kvpHighestCount = new KeyValuePair<double, int>(value, count);
                }
            }

            return kvpHighestCount.Key;
        }

        public static double Mode<T>(IEnumerable<T> values, Func<T, double> selector)
        {
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");
            Contract.Requires<ArgumentException>(selector != null, "selector must not be null!");

            return Mode(values.Select(selector));
        }

        public static double Min(IEnumerable<double> values)
        {
            //todo: Test
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");

            return values.Min();
        }

        public static double Min<T>(IEnumerable<T> values, Func<T, double> selector)
        {
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");
            Contract.Requires<ArgumentException>(selector != null, "selector must not be null!");

            return Min(values.Select(selector));
        }

        public static double Max(IEnumerable<double> values)
        {
            //todo: Test
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");

            return values.Max();
        }

        public static double Max<T>(IEnumerable<T> values, Func<T, double> selector)
        {
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");
            Contract.Requires<ArgumentException>(selector != null, "selector must not be null!");

            return Max(values.Select(selector));
        }

        public static double QuartileEnd<T>(IEnumerable<T> values, Func<T, double> selector, int quartile)
        {
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");
            Contract.Requires<ArgumentException>(selector != null, "selector must not be null!");
            Contract.Requires<ArgumentException>(quartile >= 1 && quartile <= 4, "quartile must be between 1 and 4!");

            return QuartileEnd(values.Select(selector), quartile);
        }

        public static double QuartileEnd(IEnumerable<double> values, int quartile)
        {
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");
            Contract.Requires<ArgumentException>(quartile >=1 && quartile <= 4, "quartile must be between 1 and 4!");

            //todo: test

            double median = LaurentFearon.Statistics.Calculations.Median(values);
            var lowerHalf = values.Where(x => x <= median);
            var upperHalf = values.Where(x => x > median);

            double lowerMedian = LaurentFearon.Statistics.Calculations.Median(lowerHalf);
            var firstQuartile = lowerHalf.Where(x => x <= lowerMedian);
            var secondQuartile = lowerHalf.Where(x => x > lowerMedian);

            double upperMedian = LaurentFearon.Statistics.Calculations.Median(upperHalf);
            var thirdQuartile = upperHalf.Where(x => x <= upperMedian);
            var fourthQuartile = upperHalf.Where(x => x > upperMedian);

            switch (quartile)
            {
                case 1:
                    return firstQuartile.Max();
                case 2:
                    return secondQuartile.Max();
                case 3:
                    return thirdQuartile.Max();
                case 4:
                    return fourthQuartile.Max();
                default:
                    throw new InvalidOperationException();
            }
        }

        public static double QuartileStart<T>(IEnumerable<T> values, Func<T, double> selector, int quartile)
        {
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");
            Contract.Requires<ArgumentException>(selector != null, "selector must not be null!");
            Contract.Requires<ArgumentException>(quartile >= 1 && quartile <= 4, "quartile must be between 1 and 4!");

            return QuartileStart(values.Select(selector), quartile);
        }

        public static double QuartileStart(IEnumerable<double> values, int quartile)
        {
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");
            Contract.Requires<ArgumentException>(quartile >= 1 && quartile <= 4, "quartile must be between 1 and 4!");

            //todo: test

            double median = LaurentFearon.Statistics.Calculations.Median(values);
            var lowerHalf = values.Where(x => x <= median);
            var upperHalf = values.Where(x => x > median);

            double lowerMedian = LaurentFearon.Statistics.Calculations.Median(lowerHalf);
            var firstQuartile = lowerHalf.Where(x => x <= lowerMedian);
            var secondQuartile = lowerHalf.Where(x => x > lowerMedian);

            double upperMedian = LaurentFearon.Statistics.Calculations.Median(upperHalf);
            var thirdQuartile = upperHalf.Where(x => x <= upperMedian);
            var fourthQuartile = upperHalf.Where(x => x > upperMedian);

            switch (quartile)
            {
                case 1:
                    return firstQuartile.Min();
                case 2:
                    return secondQuartile.Min();
                case 3:
                    return thirdQuartile.Min();
                case 4:
                    return fourthQuartile.Min();
                default:
                    throw new InvalidOperationException();
            }
        }

        #region Shortcuts

        public static double VarP(IEnumerable<double> values, double mean)
        {
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");

            return CalculateVariance_Population(values, mean);
        }

        public static double VarP<T>(IEnumerable<T> values, double mean, Func<T, double> selector)
        {
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");
            Contract.Requires<ArgumentException>(selector != null, "selector must not be null!");

            return VarP(values.Select(selector), mean);
        }

        public static double VarS(IEnumerable<double> values, double mean)
        {
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");

            return CalculateVariance_Sample(values, mean);
        }

        public static double VarS<T>(IEnumerable<T> values, double mean, Func<T, double> selector)
        {
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");
            Contract.Requires<ArgumentException>(selector != null, "selector must not be null!");

            return VarS(values.Select(selector), mean);
        }

        public static double StdDevP(IEnumerable<double> values, double mean)
        {
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");

            return CalculateStandardDeviation_Population(values, mean);
        }

        public static double StdDevP<T>(IEnumerable<T> values, double mean, Func<T, double> selector)
        {
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");
            Contract.Requires<ArgumentException>(selector != null, "selector must not be null!");

            return StdDevP(values.Select(selector), mean);
        }


        public static double StdDevS(IEnumerable<double> values, double mean)
        {
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");

            return CalculateStandardDeviation_Sample(values, mean);
        }

        public static double StdDevS<T>(IEnumerable<T> values, double mean, Func<T, double> selector)
        {
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");
            Contract.Requires<ArgumentException>(selector != null, "selector must not be null!");

            return StdDevS(values.Select(selector), mean);
        }

        public static LineFormula LinReg(IEnumerable<double> valuesX, IEnumerable<double> valuesY)
        {
            return GetRegressionLine(valuesX, valuesY);
        }

        public static LineFormula LinReg<T>(IEnumerable<T> values, Func<T, double> selectorX, Func<T, double> selectorY)
        {
            Contract.Requires<ArgumentException>(values != null && values.Count() > 0, "values must not be null or empty!");
            Contract.Requires<ArgumentException>(selectorX != null, "selector must not be null!");
            Contract.Requires<ArgumentException>(selectorY != null, "selector must not be null!");

            return LinReg(values.Select(selectorX), values.Select(selectorY));
        }

        public static LineFormula LinReg<T, U>(IEnumerable<T> valuesX, IEnumerable<U> valuesY, Func<T, double> selectorX, Func<U, double> selectorY)
        {
            Contract.Requires<ArgumentException>(valuesX != null && valuesX.Count() > 0, "valuesX must not be null or empty!");
            Contract.Requires<ArgumentException>(valuesY != null && valuesY.Count() > 0, "valuesY must not be null or empty!");
            Contract.Requires<ArgumentException>(selectorX != null, "selector must not be null!");
            Contract.Requires<ArgumentException>(selectorY != null, "selector must not be null!");

            return LinReg(valuesX.Select(selectorX), valuesY.Select(selectorY));
        }

        public static double NormSInv(double value)
        {
            return CalculateInverseNormalDistribution(value);
        }

        public static double NormDist(double value, double mean, double standardDeviation)
        {
            return CalculateNormalDistribution(value, mean, standardDeviation);
        }

        #endregion  
        

        //todo: correlation
        //todo: outlier
        //todo: quartile
        //todo: percentile
        //todo: exponential regression
        //todo: logarithmic regression


    }

    public struct LineFormula
    {
        public double Slope { get; set; }
        public double Intercept { get; set; }

        public override string ToString()
        {
            return string.Format("f(x) = {0}x + {1}", this.Slope, this.Intercept);
        }
    }
}
