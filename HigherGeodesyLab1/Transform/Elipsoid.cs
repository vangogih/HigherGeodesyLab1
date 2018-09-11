using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using HigherGeodesyLab1.Helper;

namespace HigherGeodesyLab1.Transform
{
    /// <inheritdoc />
    /// <summary>
    /// Публичный класс для параметров элипсоида
    /// </summary>
    public class Ellipsoid : GeneralFunctionality
    {
        #region Encapsulation

        /// <summary>
        /// Имя элипсоида
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// экваториальная полуось
        /// </summary>
        public double A { get; private set; }

        /// <summary>
        /// полярная полуось
        /// </summary>
        public double B { get; private set; }

        /// <summary>
        /// сжатие элипсоида
        /// </summary>
        public double Alf { get; private set; }

        /// <summary>
        /// первый эксцентриситет
        /// </summary>
        public double E { get; private set; }

        /// <summary>
        /// второй эксцентриситет
        /// </summary>
        public double EAmp { get; private set; }

        /// <summary>
        /// квадрат первого эксцентриситета
        /// </summary>
        public double PowE { get; private set; }

        /// <summary>
        /// квадрат второго эксцентриситета
        /// </summary>
        public double PowEAmp { get; private set; }

        #endregion

        #region Ctor

        /// <summary>
        /// Конструктор для параметров элипсоида
        /// </summary>
        public Ellipsoid(string name, string a, string b, string alf, string e, string eAmp, string powE,
            string powEAmp)
        {
            Name = name;
            A = TryParse(a);
            B = TryParse(b);
            Alf = TryParse(alf);
            E = TryParse(e);
            EAmp = TryParse(eAmp);
            PowE = TryParse(powE);
            PowEAmp = TryParse(powEAmp);
        }

        #endregion

        #region TransformMethod

        /// <summary>
        /// Основной метод определяющий все параметры элипсоида из исходных данных
        /// </summary>
        /// <param name="dataList">Коллекция с параметрами элипсоида</param>
        public static void CalculateParamsEllipsoid(List<Ellipsoid> dataList)
        {
            foreach (var item in dataList)
            {
                if (item.A != 0 && item.B != 0 && item.Alf == 0 && item.E == 0 && item.EAmp == 0 && item.PowE == 0 &&
                    item.PowEAmp == 0)
                {
                    item.Alf = GetValueAlf(item.A, item.B);
                    item.E = GetValueE(item.A, item.B);
                    item.EAmp = GetValueEAmp(item.A, item.B);
                    item.PowE = GetValuePowE(item.A, item.B);
                    item.PowEAmp = GetValuePowEAmp(item.A, item.B);
                }

                else if (item.A != 0 && item.B == 0 && item.Alf != 0 && item.E == 0 && item.EAmp == 0 &&
                         item.PowE == 0 &&
                         item.PowEAmp == 0)
                {
                    item.PowE = GetValuePowE(item.Alf);
                    item.B = GetValueB(item.A, item.PowE);
                    item.E = GetValueE(item.A, item.B);
                    item.EAmp = GetValueEAmp(item.A, item.B);
                    item.PowEAmp = GetValuePowEAmp(item.PowE);
                }

                else if (item.A != 0 && item.B == 0 && item.Alf == 0 && item.E == 0 && item.EAmp == 0 &&
                         item.PowE != 0 &&
                         item.PowEAmp == 0)
                {
                    item.B = GetValueB(item.A, item.PowE);
                    item.Alf = GetValueAlf(item.PowE);
                    item.E = GetValueE(item.A, item.B);
                    item.EAmp = GetValueEAmp(item.A, item.B);
                    item.PowEAmp = GetValuePowEAmp(item.PowE);
                }
            }
        }

        #endregion

        #region MathFunction

        /// <summary>
        /// Метод для определения первого эксценриситета
        /// </summary>
        /// <param name="a">экваториальная полуось</param>
        /// <param name="b">полярная полуось</param>
        /// <returns>значение первого эксцентриситета</returns>
        private static readonly Func<double, double, double> GetValueE = (a, b) =>
            Math.Sqrt(Math.Pow(a, 2) - Math.Pow(b, 2)) / a;

        /// <summary>
        /// Метод для определения второго эксценриситета
        /// </summary>
        /// <param name="a">экваториальная полуось</param>
        /// <param name="b">полярная полуось</param>
        /// <returns>значение второго эксцентриситета</returns>
        private static readonly Func<double, double, double> GetValueEAmp = (a, b) =>
            Math.Sqrt(Math.Pow(a, 2) - Math.Pow(b, 2)) / b;

        /// <summary>
        /// Метод определющий экваториальную полуось
        /// </summary>
        /// <param name="b">полярная полуось</param>
        /// <param name="powEAmp">квадрат второго эксцентриситета</param>
        /// <returns></returns>
        private Func<double, double, double> _getValueA = (b, powEAmp) => b * Math.Sqrt(1 + powEAmp);

        /// <summary>
        /// Метод определяющий полярную полуось
        /// </summary>
        /// <param name="a">экваториальная полось</param>
        /// <param name="powe">квадрат первого эксцентриситета</param>
        /// <returns>значение полярной полуоси</returns>
        private static readonly Func<double, double, double> GetValueB = (a, powE) => a * Math.Sqrt(1 - powE);

        /// <summary>
        /// Метод определяющий сжатие элипсоида
        /// </summary>
        /// <param name="a">экваториальная полуось</param>
        /// <param name="b">полярная полуось</param>
        /// <returns>значение сжатия элипсоида</returns>
        public static double GetValueAlf(double a, double b)
        {
            return (a - b) / a;
        }

        /// <summary>
        /// Метод определяющий сжатие элипсоида
        /// </summary>
        /// <param name="powE">первый эксцентриситет</param>
        /// <returns>сжатие элипсоида</returns>
        private static double GetValueAlf(double powE)
        {
            return 1 - Math.Sqrt(1 - powE);
        }

        /// <summary>
        /// Метод для определения квадрата первого эксцентриситета элипсоида
        /// </summary>
        /// <param name="a">экваториальная полуось</param>
        /// <param name="b">полярная полуось</param>
        /// <returns>квадрат первого эксцентриситета элипсоида</returns>
        public static double GetValuePowE(double a, double b)
        {
            return 1 - Math.Pow(b, 2) / Math.Pow(a, 2);
        }

        /// <summary>
        /// Метод для определения квадрата первого эксцентриситета элипсоида
        /// </summary>
        /// <param name="alf">сжатие элипсоида</param>
        /// <returns>квадрат первого эксцентриситета элипсоида</returns>
        public static double GetValuePowE(double alf)
        {
            return 2 * alf - Math.Pow(alf, 2);
        }

        /// <summary>
        /// Метод для определения квадрата второго эксцентриситета  
        /// </summary>
        /// <param name="a">экваториальная полуось</param>
        /// <param name="b">полярная полуось</param>
        /// <returns>квадрат второго эксцентриситета</returns>
        private static double GetValuePowEAmp(double a, double b)
        {
            return Math.Pow(a, 2) / Math.Pow(b, 2) - 1;
        }

        /// <summary>
        /// Метод для определения квадрата второго эксцентриситета 
        /// </summary>
        /// <param name="powE">первый эксцентриситет</param>
        /// <returns>квадрат второго эксцентриситета</returns>
        private static double GetValuePowEAmp(double powE)
        {
            return powE / (1 - powE);
        }

        #endregion

    }
}