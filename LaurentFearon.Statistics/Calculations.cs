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

        public static List<KeyValuePair<double, int>> GetClassFrequencies(IEnumerable<double> rClasses, IEnumerable<double> rValues)
        {
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

        public static double CalculateRange(IEnumerable<double> rValues)
        {
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");

            return rValues.Max() - rValues.Min();
        }

        public static double CalculateClassWidth(IEnumerable<double> rValues, double range)
        {
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");

            int k = (int)Math.Round(Math.Sqrt(rValues.Count()), 0);

            return range / (double)k;
        }

        public static double CalculateStandardDistribution(double value, double mean, double standardDeviation)
        {
            Contract.Requires<ArgumentException>(standardDeviation != 0, "standardDeviation must not be zero!");

            return 1 / (standardDeviation * Math.Sqrt(2 * Math.PI)) * Math.Exp(-Math.Pow(value - mean, 2) / (2 * standardDeviation * standardDeviation));
        }

        public static double CalculateStandardDeviation_Population(IEnumerable<double> rValues, double mean)
        {
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");

            return Math.Sqrt(CalculateVariance_Population(rValues, mean));
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

        public static double CalculateStandardDeviation_Sample(IEnumerable<double> rValues, double mean)
        {
            Contract.Requires<ArgumentException>(rValues != null && rValues.Count() > 0, "rValues must not be null or empty!");

            return Math.Sqrt(CalculateVariance_Sample(rValues, mean));
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

        private static System.Windows.Forms.DataVisualization.Charting.Chart chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
        public static double CalculateInverseNormalDistribution(double value)
        {
            return chart.DataManipulator.Statistics.InverseNormalDistribution(value);
        }

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
