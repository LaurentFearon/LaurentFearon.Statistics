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
        public static IEnumerable<double> CalculateClasses(IEnumerable<double> rValues, double classWidth, int numberOfClasses)
        {
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");
            Contract.Requires<ArgumentException>(numberOfClasses > 1, "number of classes must not be greater than 1!");

            List<double> rClassValues = new List<double>();
            double startValue = rValues.Min() - classWidth / 2.0 - classWidth;
            double lastValue = startValue;

            rClassValues.Add(startValue);
            for (int i = 0; i < numberOfClasses-1; i++)
            {
                double value = lastValue + classWidth;
                rClassValues.Add(value);
                lastValue = value;
            }

            return rClassValues;
        }

        public static IEnumerable<double> CalculateClasses<T>(IEnumerable<T> rValues, double classWidth, int numberOfClasses, Func<T, double> selector)
        {
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");
            Contract.Requires<ArgumentException>(selector != null, "selector must not be null!");

            return CalculateClasses(rValues.Select(selector), classWidth, numberOfClasses);
        }

        public static List<KeyValuePair<double, int>> GetClassFrequencies(IEnumerable<double> rClasses, IEnumerable<double> rValues)
        {
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");
            Contract.Requires<ArgumentException>(rClasses != null && rClasses.Count() > 0, "rClasses must not be null or empty!");

            List<KeyValuePair<double, int>> rClassFrequencies = new List<KeyValuePair<double, int>>();

            for (int i = 0; i < rClasses.Count(); i++)
            {
                int count = -1;

                if(i == 0)
                {
                    count = rValues.Where(x => x < rClasses.ElementAt(i)).Count();
                }
                else if (i == rClasses.Count() -1)
                {
                    count = rValues.Where(x => x >= rClasses.ElementAt(i)).Count();
                }
                else
                {
                   count = rValues.Where(x => x < rClasses.ElementAt(i) && x >= rClasses.ElementAt(i - 1)).Count();
                }

                rClassFrequencies.Add(new KeyValuePair<double, int>(rClasses.ElementAt(i), count));
            }

            return rClassFrequencies;
        }

        public static List<KeyValuePair<double, int>> GetClassFrequencies<T>(IEnumerable<double> rClasses, IEnumerable<T> rValues, Func<T, double> selector)
        {
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");
            Contract.Requires<ArgumentException>(selector != null, "selector must not be null!");

            return GetClassFrequencies(rClasses, rValues.Select(selector));
        }

        public static double CalculateRange(IEnumerable<double> rValues)
        {
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");

            return rValues.Max() - rValues.Min();
        }

        public static double CalculateRange<T>(IEnumerable<T> rValues, Func<T, double> selector)
        {
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");
            Contract.Requires<ArgumentException>(selector != null, "selector must not be null!");

            return CalculateRange(rValues.Select(selector));
        }

        public static double CalculateClassWidth(IEnumerable<double> rValues, double range)
        {
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");

            int k = (int)Math.Round(Math.Sqrt(rValues.Count()), 0);

            return range / (double)k;
        }

        public static double CalculateClassWidth<T>(IEnumerable<T> rValues, double range, Func<T, double> selector)
        {
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");
            Contract.Requires<ArgumentException>(selector != null, "selector must not be null!");

            return CalculateClassWidth(rValues.Select(selector), range);
        }

        public static double CalculateNormalDistribution(double value, double mean, double standardDeviation)
        {
            Contract.Requires<ArgumentException>(standardDeviation != 0, "standardDeviation must not be zero!");

            return 1 / (standardDeviation * Math.Sqrt(2 * Math.PI)) * Math.Exp(-Math.Pow(value - mean, 2) / (2 * standardDeviation * standardDeviation));
        }

        public static double CalculateStandardDeviation_Population(IEnumerable<double> rValues, double mean)
        {
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");

            return Math.Sqrt(CalculateVariance_Population(rValues, mean));
        }

        public static double CalculateStandardDeviation_Population<T>(IEnumerable<T> rValues, double mean, Func<T, double> selector)
        {
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");
            Contract.Requires<ArgumentException>(selector != null, "selector must not be null!");

            return CalculateStandardDeviation_Population(rValues.Select(selector), mean);
        }

        public static double CalculateVariance_Population(IEnumerable<double> rValues, double mean)
        {
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");

            double sum = 0;
            foreach (double value in rValues)
            {
                sum += Math.Pow(value - mean, 2);
            }

            return sum / ((double)rValues.Count());
        }

        public static double CalculateVariance_Population<T>(IEnumerable<T> rValues, double mean, Func<T, double> selector)
        {
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");
            Contract.Requires<ArgumentException>(selector != null, "selector must not be null!");

            return CalculateVariance_Population(rValues.Select(selector), mean);
        }

        public static double CalculateStandardDeviation_Sample(IEnumerable<double> rValues, double mean)
        {
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");

            return Math.Sqrt(CalculateVariance_Sample(rValues, mean));
        }

        public static double CalculateStandardDeviation_Sample<T>(IEnumerable<T> rValues, double mean, Func<T, double> selector)
        {
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");
            Contract.Requires<ArgumentException>(selector != null, "selector must not be null!");

            return CalculateStandardDeviation_Sample(rValues.Select(selector), mean);
        }

        public static double CalculateVariance_Sample(IEnumerable<double> rValues, double mean)
        {
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");

            double sum = 0;
            foreach (double value in rValues)
            {
                sum += Math.Pow(value - mean, 2);
            }

            return sum / ((double)rValues.Count() - 1);
        }

        public static double CalculateVariance_Sample<T>(IEnumerable<T> rValues, double mean, Func<T, double> selector)
        {
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");
            Contract.Requires<ArgumentException>(selector != null, "selector must not be null!");

            return CalculateVariance_Sample(rValues.Select(selector), mean);
        }

        public static List<KeyValuePair<double, int>> GetCumulativeClassFrequencies(IEnumerable<double> rClasses, IEnumerable<double> rValues)
        {
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");
            Contract.Requires<ArgumentException>(rClasses != null && rClasses.Count() > 0, "rClasses must not be null or empty!");

            List<KeyValuePair<double, int>> rClassFrequencies = GetClassFrequencies(rClasses, rValues);

            List<KeyValuePair<double, int>> rCumulativeFrequencies = new List<KeyValuePair<double, int>>();

            for (int i=0; i< rClassFrequencies.Count; i++)
            {
                if (i > 0)
                {
                    rCumulativeFrequencies.Add(new KeyValuePair<double, int>(rClassFrequencies.ElementAt(i).Key, rClassFrequencies.ElementAt(i).Value + rCumulativeFrequencies.ElementAt(i - 1).Value));
                }
                else
                {
                    rCumulativeFrequencies.Add(new KeyValuePair<double, int>(rClassFrequencies.ElementAt(i).Key, rClassFrequencies.ElementAt(i).Value));
                }
            }

            return rCumulativeFrequencies;
        }

        public static List<KeyValuePair<double, int>> GetCumulativeClassFrequencies<T>(IEnumerable<double> rClasses, IEnumerable<T> rValues, Func<T, double> selector)
        {
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");
            Contract.Requires<ArgumentException>(selector != null, "selector must not be null!");

            return GetCumulativeClassFrequencies(rClasses, rValues.Select(selector));
        }

        public static LineFormula GetRegressionLine(IEnumerable<double> rValuesX, IEnumerable<double> rValuesY)
        {
            Contract.Requires<ArgumentException>(rValuesX != null && rValuesX.Count() > 0, "rValuesX must not be null or empty!");
            Contract.Requires<ArgumentException>(rValuesY != null && rValuesY.Count() > 0, "rValuesY must not be null or empty!");
            Contract.Requires<ArgumentException>(rValuesX.Count() == rValuesY.Count(), "rValuesX and rValuesY must have same amount of values!");

            LineFormula lf = new LineFormula();

            double avgX = rValuesX.Average();
            double avgY = rValuesY.Average();
            double avgXY = 0;

            for(int i=0; i<rValuesX.Count(); i++)
            {
                avgXY += (rValuesX.ElementAt(i) * rValuesY.ElementAt(i));
            }
            avgXY = avgXY / rValuesX.Count();

            double avgXX = 0;
            for (int i = 0; i < rValuesX.Count(); i++)
            {
                avgXX += (rValuesX.ElementAt(i) * rValuesX.ElementAt(i));
            }
            avgXX = avgXX / rValuesX.Count();

            double slope = (avgX * avgY - avgXY) / (avgX * avgX - avgXX);
            double intercept = avgY - slope * avgX;

            lf.Intercept = intercept;
            lf.Slope = slope;
            return lf;
        }

        public static LineFormula GetRegressionLine<T>(IEnumerable<T> rValues, Func<T, double> selectorX, Func<T, double> selectorY)
        {
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");
            Contract.Requires<ArgumentException>(selectorX != null, "selector must not be null!");
            Contract.Requires<ArgumentException>(selectorY != null, "selector must not be null!");

            return GetRegressionLine(rValues.Select(selectorX), rValues.Select(selectorY));
        }

        public static LineFormula GetRegressionLine<T,U>(IEnumerable<T> rValuesX, IEnumerable<U> rValuesY, Func<T, double> selectorX, Func<U, double> selectorY)
        {
            Contract.Requires<ArgumentException>(rValuesX != null && rValuesX.Count() > 0, "rValuesX must not be null or empty!");
            Contract.Requires<ArgumentException>(rValuesY != null && rValuesY.Count() > 0, "rValuesY must not be null or empty!");
            Contract.Requires<ArgumentException>(selectorX != null, "selector must not be null!");
            Contract.Requires<ArgumentException>(selectorY != null, "selector must not be null!");

            return GetRegressionLine(rValuesX.Select(selectorX), rValuesY.Select(selectorY));
        }

        private static System.Windows.Forms.DataVisualization.Charting.Chart chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
        public static double CalculateInverseNormalDistribution(double value)
        {
            return chart.DataManipulator.Statistics.InverseNormalDistribution(value);
        }

        public static double Average(IEnumerable<double> rValues)
        {
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");

            return rValues.Average();
        }

        public static double Average<T>(IEnumerable<T> rValues, Func<T, double> selector)
        {
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");
            Contract.Requires<ArgumentException>(selector != null, "selector must not be null!");

            return rValues.Average(selector);
        }

        public static double Median(IEnumerable<double> rValues)
        {//todo: Test
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");

            var orderedValues = rValues.OrderBy(x => x);
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

        public static double Median<T>(IEnumerable<T> rValues, Func<T, double> selector)
        {
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");
            Contract.Requires<ArgumentException>(selector != null, "selector must not be null!");

            return Median(rValues.Select(selector));
        }

        public static double Mode(IEnumerable<double> rValues)
        {
            //todo: Test
            //todo: mode can have more than one solution
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");

            var distinctValues = rValues.Distinct();
            KeyValuePair<double, int> kvpHighestCount = new KeyValuePair<double, int>();

            foreach(var value in distinctValues)
            {
                int count = rValues.Count(x => x == value);
                if(count > kvpHighestCount.Value)
                {
                    kvpHighestCount = new KeyValuePair<double, int>(value, count);
                }
            }

            return kvpHighestCount.Key;
        }

        public static double Mode<T>(IEnumerable<T> rValues, Func<T, double> selector)
        {
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");
            Contract.Requires<ArgumentException>(selector != null, "selector must not be null!");

            return Mode(rValues.Select(selector));
        }

        public static double Min(IEnumerable<double> rValues)
        {
            //todo: Test
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");

            return rValues.Min();
        }

        public static double Min<T>(IEnumerable<T> rValues, Func<T, double> selector)
        {
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");
            Contract.Requires<ArgumentException>(selector != null, "selector must not be null!");

            return Min(rValues.Select(selector));
        }

        public static double Max(IEnumerable<double> rValues)
        {
            //todo: Test
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");

            return rValues.Max();
        }

        public static double Max<T>(IEnumerable<T> rValues, Func<T, double> selector)
        {
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");
            Contract.Requires<ArgumentException>(selector != null, "selector must not be null!");

            return Max(rValues.Select(selector));
        }

        #region Shortcuts

        public static double VarP(IEnumerable<double> rValues, double mean)
        {
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");

            return CalculateVariance_Population(rValues, mean);
        }

        public static double VarP<T>(IEnumerable<T> rValues, double mean, Func<T, double> selector)
        {
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");
            Contract.Requires<ArgumentException>(selector != null, "selector must not be null!");

            return VarP(rValues.Select(selector), mean);
        }

        public static double VarS(IEnumerable<double> rValues, double mean)
        {
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");

            return CalculateVariance_Sample(rValues, mean);
        }

        public static double VarS<T>(IEnumerable<T> rValues, double mean, Func<T, double> selector)
        {
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");
            Contract.Requires<ArgumentException>(selector != null, "selector must not be null!");

            return VarS(rValues.Select(selector), mean);
        }

        public static double StdDevP(IEnumerable<double> rValues, double mean)
        {
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");

            return CalculateStandardDeviation_Population(rValues, mean);
        }

        public static double StdDevP<T>(IEnumerable<T> rValues, double mean, Func<T, double> selector)
        {
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");
            Contract.Requires<ArgumentException>(selector != null, "selector must not be null!");

            return StdDevP(rValues.Select(selector), mean);
        }


        public static double StdDevS(IEnumerable<double> rValues, double mean)
        {
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");

            return CalculateStandardDeviation_Sample(rValues, mean);
        }

        public static double StdDevS<T>(IEnumerable<T> rValues, double mean, Func<T, double> selector)
        {
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");
            Contract.Requires<ArgumentException>(selector != null, "selector must not be null!");

            return StdDevS(rValues.Select(selector), mean);
        }

        public static LineFormula LinReg(IEnumerable<double> rValuesX, IEnumerable<double> rValuesY)
        {
            return GetRegressionLine(rValuesX, rValuesY);
        }

        public static LineFormula LinReg<T>(IEnumerable<T> rValues, Func<T, double> selectorX, Func<T, double> selectorY)
        {
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");
            Contract.Requires<ArgumentException>(selectorX != null, "selector must not be null!");
            Contract.Requires<ArgumentException>(selectorY != null, "selector must not be null!");

            return LinReg(rValues.Select(selectorX), rValues.Select(selectorY));
        }

        public static LineFormula LinReg<T, U>(IEnumerable<T> rValuesX, IEnumerable<U> rValuesY, Func<T, double> selectorX, Func<U, double> selectorY)
        {
            Contract.Requires<ArgumentException>(rValuesX != null && rValuesX.Count() > 0, "rValuesX must not be null or empty!");
            Contract.Requires<ArgumentException>(rValuesY != null && rValuesY.Count() > 0, "rValuesY must not be null or empty!");
            Contract.Requires<ArgumentException>(selectorX != null, "selector must not be null!");
            Contract.Requires<ArgumentException>(selectorY != null, "selector must not be null!");

            return LinReg(rValuesX.Select(selectorX), rValuesY.Select(selectorY));
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
