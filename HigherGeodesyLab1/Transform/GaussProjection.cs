using System;
using HigherGeodesyLab1.Helper;

namespace HigherGeodesyLab1.Transform
{
    public class GaussProjection : GeneralFunctionality
    {
        private const double OneRad = 57.29577951;

        /// <summary>
        /// номер шестиградусной зоны в проекции Гаусса-Крюгера
        /// </summary>
        /// <param name="L">широта</param>
        public int NumberSixDegreesZone(double L)
        {
            return (int) Math.Round((3 + RadToDeg(L)) / 6, 0);
        }

        /// <summary>
        /// расстояние от определяемой точки до осевого меридиана зоны,
        /// выраженное в радианной мере
        /// </summary>
        /// <param name="L">широта в радианах</param>
        /// <param name="n">номер шестиградусной зоны <see cref="NumberSixDegreesZone"/>></param>
        public double GetValueRangeL(double L, double n)
        {
            return (RadToDeg(L) - (3 + 6 * (n - 1))) / OneRad;
        }

        /// <summary>
        /// Плоская координата Х в проекции Гаусса-Крюгера
        /// </summary>
        /// <param name="b">широта в радианах</param>
        /// <param name="l"><see cref="GetValueRangeL"/>></param>
        public double GetValueXGaussProjection(double b, double l)
        {
            return
                6367558.4968 * b - Math.Sin(2 * b) *
                (16002.89 + 66.9707 * Math.Pow(Math.Sin(b), 2) + 0.3515 * Math.Pow(Math.Sin(b), 4))
                + Math.Pow(l, 2) * Math.Sin(2 * b) * (1594561.25 + 5336.535 * Math.Pow(Math.Sin(b), 2) +
                                                      26.790 * Math.Pow(Math.Sin(b), 4) +
                                                      0.149 * Math.Pow(Math.Sin(b), 6))
                + Math.Pow(l, 4) * Math.Sin(2 * b) * (672483.4 - 811219 * Math.Pow(Math.Sin(b), 2) +
                                                      5420 * Math.Pow(Math.Sin(b), 4) -
                                                      16010 * Math.Pow(Math.Sin(b), 6))
                + Math.Pow(l, 8) * Math.Sin(2 * b) * (109500 - 574700 * Math.Pow(Math.Sin(b), 2) +
                                                      863700 * Math.Pow(Math.Sin(b), 4) -
                                                      398600 * Math.Pow(Math.Sin(b), 6));
        }

        /// <summary>
        /// Плоская координата Y в проекции Гаусса-Крюгера
        /// </summary>
        /// <param name="n"><see cref="NumberSixDegreesZone"/>></param>
        /// <param name="b">широта в радианах</param>
        /// <param name="l"><see cref="GetValueRangeL"/>></param>
        public double GetValueYGaussProjection(int n, double b, double l)
        {
            return
                (5 + 10 * n) * Math.Pow(10, 5)
                + l * Math.Cos(b) * (6378245 + 21346.1415 * Math.Pow(Math.Sin(b), 2) +
                                     107.159 * Math.Pow(Math.Sin(b), 4) +
                                     0.5977 * Math.Pow(Math.Sin(b), 6))
                + Math.Pow(l, 3) * Math.Cos(b) * (1070204.16 - 2136826.66 * Math.Pow(Math.Sin(b), 2) +
                                                  17.98 * Math.Pow(Math.Sin(b), 4) - 11.99 * Math.Pow(Math.Sin(b), 6))
                + Math.Pow(l, 5) * Math.Cos(b) * (270806 - 1523417 * Math.Pow(Math.Sin(b), 2) +
                                                  1327645 * Math.Pow(Math.Sin(b), 4) - 21701 * Math.Pow(Math.Sin(b), 6))
                + Math.Pow(l, 5) * Math.Cos(b) * (79690 - 866190 * Math.Pow(Math.Sin(b), 2) +
                                                  1730360 * Math.Pow(Math.Sin(b), 4) -
                                                  945460 * Math.Pow(Math.Sin(b), 6));
        }
    }
}