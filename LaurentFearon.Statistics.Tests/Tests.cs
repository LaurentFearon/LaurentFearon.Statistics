///
/// Created by Laurent Fearon
/// Contact Info: Laurent.Fearon@gmail.com
/// Project Site: https://github.com/LaurentFearon/LaurentFearon.Statistics
/// Apache License 2.0
/// See License file: https://github.com/LaurentFearon/LaurentFearon.Statistics/blob/master/LICENSE.md
///

using System;
using NUnit.Framework;
using System.Collections.Generic;
using FluentAssertions;
using System.Linq;

namespace LaurentFearon.Statistics.Tests
{
    [TestFixture]
    public class Tests
    {
        const string TESTDATA = @"1890
1844
1850
1820
1896
1936
1760
1928
1822
1985
2069
1944
1790
1936
1882
1920
1831
1852
1782
1915
1787
1955
1782
1790
1874
1836
1822
2091
2129
1809
1793
1869
2015
1963
1793
1953
1736
1980
1801
1944
2216
1831
1893
1763
1844
2004
1925
1844
1855
1999
1888
2088
1944
2001
1809
1844
2069
1915
1907
2037
1901
1936
1779
1925
1855
2007
2007
1925
1706
1820
1860
1833
1855
1869
2037
1771
1828
1812
1917
1755
1689
1888
1912
1882
1765
1944
1679
2037
1757
2056
1996
1760
1782
1888
1817
2015
1757
1955
1828
1831
1774
2023
1828
1738
1939
1928
1828
1901
1738
1896
1736
2020
1847
1939
1888
1668
1915
1765
1944
1795
1825
1996
1755
1784
1888
";
        private static List<double> GetTestData()
        {
            List<double> rMesswerte = new List<double>();

            string[] rValuesAsString = TESTDATA.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            rValuesAsString.Length.Should().Be(125);
            foreach (string s in rValuesAsString)
            {
                rMesswerte.Add(double.Parse(s));
            }

            rMesswerte.Count.Should().Be(125);

            return rMesswerte;
        }

        //[Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
        [Test]
        public void CalcClasses_Test()
        {
            var rMesswerte = GetTestData();
            double range = Calculations.CalculateRange(rMesswerte);
            double classWidth = Calculations.CalculateClassWidth(rMesswerte, range);
            var rClasses = Calculations.CalculateClasses(rMesswerte, classWidth, 16);

            rClasses.ElementAt(0).Should().BeApproximately(1593.2727, 0.0001);
            rClasses.ElementAt(1).Should().BeApproximately(1643.0909, 0.0001);
            rClasses.ElementAt(2).Should().BeApproximately(1692.9090, 0.0001);
            rClasses.ElementAt(3).Should().BeApproximately(1742.7272, 0.0001);
            rClasses.ElementAt(4).Should().BeApproximately(1792.5454, 0.0001);
            rClasses.ElementAt(5).Should().BeApproximately(1842.3636, 0.0001);
            rClasses.ElementAt(6).Should().BeApproximately(1892.1818, 0.0001);
            rClasses.ElementAt(7).Should().BeApproximately(1942, 0.0001);
            rClasses.ElementAt(8).Should().BeApproximately(1991.8181, 0.0001);
            rClasses.ElementAt(9).Should().BeApproximately(2041.6363, 0.0001);
            rClasses.ElementAt(10).Should().BeApproximately(2091.4545, 0.0001);
            rClasses.ElementAt(11).Should().BeApproximately(2141.2727, 0.0001);
            rClasses.ElementAt(12).Should().BeApproximately(2191.0909, 0.0001);
            rClasses.ElementAt(13).Should().BeApproximately(2240.9090, 0.0001);
            rClasses.ElementAt(14).Should().BeApproximately(2290.7272, 0.0001);
            rClasses.ElementAt(15).Should().BeApproximately(2340.5454, 0.0001);
        }

        //[Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
        [Test]
        public void CalcRange_Test()
        {
            var rMesswerte = GetTestData();

            double range = Calculations.CalculateRange(rMesswerte);
            range.Should().Be(548);
        }

        //[Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
        [Test]
        public void CalcFrequencies_Test()
        {
            var rMesswerte = GetTestData();

            double range = Calculations.CalculateRange(rMesswerte);
            double classWidth = Calculations.CalculateClassWidth(rMesswerte, range);
            var rClasses = Calculations.CalculateClasses(rMesswerte, classWidth, 16);
            var rFrequencies = Calculations.GetClassFrequencies(rClasses, rMesswerte);

            rFrequencies.ElementAt(0).Value.Should().Be(0);
            rFrequencies.ElementAt(1).Value.Should().Be(0);
            rFrequencies.ElementAt(2).Value.Should().Be(3);
            rFrequencies.ElementAt(3).Value.Should().Be(5);
            rFrequencies.ElementAt(4).Value.Should().Be(19);
            rFrequencies.ElementAt(5).Value.Should().Be(22);
            rFrequencies.ElementAt(6).Value.Should().Be(22);
            rFrequencies.ElementAt(7).Value.Should().Be(22);
            rFrequencies.ElementAt(8).Value.Should().Be(11);
            rFrequencies.ElementAt(9).Value.Should().Be(14);
            rFrequencies.ElementAt(10).Value.Should().Be(5);
            rFrequencies.ElementAt(11).Value.Should().Be(1);
            rFrequencies.ElementAt(12).Value.Should().Be(0);
            rFrequencies.ElementAt(13).Value.Should().Be(1);
            rFrequencies.ElementAt(14).Value.Should().Be(0);
            rFrequencies.ElementAt(15).Value.Should().Be(0);
        }

        //[Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
        [Test]
        public void CalcStandardDistribution_Test()
        {
            var rMesswerte = GetTestData();

            double range = Calculations.CalculateRange(rMesswerte);
            double classWidth = Calculations.CalculateClassWidth(rMesswerte, range);
            var rClasses = Calculations.CalculateClasses(rMesswerte, classWidth, 16);

            double mean = rMesswerte.Average();
            double standardDev = Calculations.CalculateStandardDeviation_Population(rMesswerte, mean);

            Calculations.CalculateNormalDistribution(rClasses.ElementAt(0), mean, standardDev).Should().BeApproximately(0.000072251, 0.00001);
            Calculations.CalculateNormalDistribution(rClasses.ElementAt(1), mean, standardDev).Should().BeApproximately(0.000256378, 0.00001);
            Calculations.CalculateNormalDistribution(rClasses.ElementAt(2), mean, standardDev).Should().BeApproximately(0.000715156, 0.00001);
            Calculations.CalculateNormalDistribution(rClasses.ElementAt(3), mean, standardDev).Should().BeApproximately(0.001568199, 0.00001);
            Calculations.CalculateNormalDistribution(rClasses.ElementAt(4), mean, standardDev).Should().BeApproximately(0.002703220, 0.00001);
            Calculations.CalculateNormalDistribution(rClasses.ElementAt(5), mean, standardDev).Should().BeApproximately(0.003663042, 0.00002);
            Calculations.CalculateNormalDistribution(rClasses.ElementAt(6), mean, standardDev).Should().BeApproximately(0.003901958, 0.00002);
            Calculations.CalculateNormalDistribution(rClasses.ElementAt(7), mean, standardDev).Should().BeApproximately(0.003267408, 0.00001);
            Calculations.CalculateNormalDistribution(rClasses.ElementAt(8), mean, standardDev).Should().BeApproximately(0.002150822, 0.00001);
            Calculations.CalculateNormalDistribution(rClasses.ElementAt(9), mean, standardDev).Should().BeApproximately(0.001112976, 0.00001);
            Calculations.CalculateNormalDistribution(rClasses.ElementAt(10), mean, standardDev).Should().BeApproximately(0.000452738, 0.00001);
            Calculations.CalculateNormalDistribution(rClasses.ElementAt(11), mean, standardDev).Should().BeApproximately(0.000144773, 0.00001);
            Calculations.CalculateNormalDistribution(rClasses.ElementAt(12), mean, standardDev).Should().BeApproximately(0.000036392, 0.00001);
            Calculations.CalculateNormalDistribution(rClasses.ElementAt(13), mean, standardDev).Should().BeApproximately(0.000007191, 0.000001);
            Calculations.CalculateNormalDistribution(rClasses.ElementAt(14), mean, standardDev).Should().BeApproximately(0.000001117, 0.000001);
            Calculations.CalculateNormalDistribution(rClasses.ElementAt(15), mean, standardDev).Should().BeApproximately(0.000000136, 0.0000001);
        }

        //[Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
        [Test]
        public void CalcClassWidth_Test()
        {
            var rMesswerte = GetTestData();

            double range = Calculations.CalculateRange(rMesswerte);
            double classWidth = Calculations.CalculateClassWidth(rMesswerte, range);
            classWidth.Should().BeApproximately(49.8181, 0.0001);
        }

        //[Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
        [Test]
        public void CalcStandardDeviation_Test()
        {
            var rMesswerte = GetTestData();

            double standDev = Calculations.CalculateStandardDeviation_Population(rMesswerte, rMesswerte.Average());
            standDev.Should().BeApproximately(101.143, 0.001);

            standDev = Calculations.CalculateStandardDeviation_Sample(rMesswerte, rMesswerte.Average());
            standDev.Should().BeApproximately(101.550, 0.001);
        }

        //[Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
        [Test]
        public void CalcVariance_Test()
        {
            var rMesswerte = GetTestData();

            double variance = Calculations.CalculateVariance_Population(rMesswerte, rMesswerte.Average());
            variance.Should().BeApproximately(10229.9401, 0.0001);

            variance = Calculations.CalculateVariance_Sample(rMesswerte, rMesswerte.Average());
            variance.Should().BeApproximately(10312.4396, 0.0001);
        }

        //[Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
        [Test]
        public void GetCumulativeClassFrequencies_Test()
        {
            var rMesswerte = GetTestData();

            double range = Calculations.CalculateRange(rMesswerte);
            double classWidth = Calculations.CalculateClassWidth(rMesswerte, range);
            var rClasses = Calculations.CalculateClasses(rMesswerte, classWidth, 16);
            var rFrequencies = Calculations.GetCumulativeClassFrequencies(rClasses, rMesswerte);

            rFrequencies.ElementAt(0).Value.Should().Be(0);
            rFrequencies.ElementAt(1).Value.Should().Be(0);
            rFrequencies.ElementAt(2).Value.Should().Be(3);
            rFrequencies.ElementAt(3).Value.Should().Be(8);
            rFrequencies.ElementAt(4).Value.Should().Be(27);
            rFrequencies.ElementAt(5).Value.Should().Be(49);
            rFrequencies.ElementAt(6).Value.Should().Be(71);
            rFrequencies.ElementAt(7).Value.Should().Be(93);
            rFrequencies.ElementAt(8).Value.Should().Be(104);
            rFrequencies.ElementAt(9).Value.Should().Be(118);
            rFrequencies.ElementAt(10).Value.Should().Be(123);
            rFrequencies.ElementAt(11).Value.Should().Be(124);
            rFrequencies.ElementAt(12).Value.Should().Be(124);
            rFrequencies.ElementAt(13).Value.Should().Be(125);
            rFrequencies.ElementAt(14).Value.Should().Be(125);
            rFrequencies.ElementAt(15).Value.Should().Be(125);
        }

        //[Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
        [Test]
        public void CalculateInverseNormalDistribution_Test()
        {
            Calculations.CalculateInverseNormalDistribution(0.024).Should().BeApproximately(-1.97737, 0.001);
            Calculations.CalculateInverseNormalDistribution(0.064).Should().BeApproximately(-1.52204, 0.001);
            Calculations.CalculateInverseNormalDistribution(0.216).Should().BeApproximately(-0.78577, 0.001);
            Calculations.CalculateInverseNormalDistribution(0.392).Should().BeApproximately(-0.27411, 0.001);
            Calculations.CalculateInverseNormalDistribution(0.568).Should().BeApproximately(0.17128, 0.001);
            Calculations.CalculateInverseNormalDistribution(0.744).Should().BeApproximately(0.655573, 0.001);
            Calculations.CalculateInverseNormalDistribution(0.832).Should().BeApproximately(0.96210, 0.001);
            Calculations.CalculateInverseNormalDistribution(0.944).Should().BeApproximately(1.58927, 0.001);
            Calculations.CalculateInverseNormalDistribution(0.984).Should().BeApproximately(2.14441, 0.001);
            Calculations.CalculateInverseNormalDistribution(0.992).Should().BeApproximately(2.40892, 0.001);
        }

        //[Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
        [Test]
        public void CalcRegressionLine_Test()
        {
            List<double> rX = new List<double>() { 9, 12, 14, 12, 12, 13, 10, 11, 12, 15 };
            List<double> rY = new List<double>() { 1216, 1300, 1356, 1288, 1276, 1292, 1260, 1244, 1288, 1360 };

            LineFormula lf = Calculations.GetRegressionLine(rX, rY);

            lf.Slope.Should().BeApproximately(24, 0.0000001);
            lf.Intercept.Should().BeApproximately(1000, 0.0000001);
        }

        //[Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
        [Test]
        public void CalcRegressionLine_Test2()
        {
            List<double> rX = new List<double>() { 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            List<double> rY = new List<double>() { -1.977368428, -1.522036242, -0.785773832, -0.274110116, 0.171284586, 0.655726679, 0.962098754, 1.589267557, 2.144410621, 2.408915546 };

            LineFormula lf = Calculations.GetRegressionLine(rX, rY);

            lf.Slope.Should().BeApproximately(0.492181572424858, 0.0000001);
            lf.Intercept.Should().BeApproximately(-3.35412028069202, 0.0000001);
        }
    }
}
