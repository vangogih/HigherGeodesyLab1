using System;
using System.Collections.Generic;
using System.Linq;
using HigherGeodesyLab1.Transform;

namespace HigherGeodesyLab1.Helper
{
    /// <summary>
    /// Статический класс для общих методов
    /// </summary>
    public class GeneralFunctionality
    {
        /// <summary>
        /// константное значение знаменателя масштаба
        /// </summary>
        private const double AcceptableAccuracy = 0.00000000000001;

        #region ProtectedMethods

        /// <summary>
        /// Итеративный метод для получение значение широты и необходимых парметров для получения долготы и геодезической высоты
        /// </summary>
        /// <param name="x">координата по оси Х</param>
        /// <param name="y">координата по оси Y</param>
        /// <param name="z">координата по оси Z</param>
        /// <param name="powe">квадрат первого эксцентриситета</param>
        /// <param name="a">горизонтальная полуось</param>
        /// <param name="d">итерационный параметр</param>
        /// <returns>значение широты</returns>
        protected static double Iteration(double x, double y, double z, double powe, double a)
        {
            double _r = GetValueR(x, y, z);
            double _p = GetValueP(powe, a, _r);
            double _c = GetValueC(z, _r);

            double[] s = {0, 0};
            double epsilon = double.MaxValue, biteration = 0;
            while (epsilon > AcceptableAccuracy)
            {
                biteration = _c + s[0];
                s[1] = GetValueS(_p, biteration, powe);
                epsilon = Math.Abs(s[1] - s[0]);
                s[0] = s[1];
            }
            return biteration;
        }

        /// <summary>
        /// Метод для получения значения долготы
        /// </summary>
        /// <param name="y">координата по оси Y</param>
        /// <param name="d">итерационный параметр</param>
        protected static readonly Func<double, double, double> GetValueL = (y, d) => Math.Asin(Math.Abs(y / d));

        /// <summary>
        /// Метод получения геодезической высоты после выполнения итерационного процесса
        /// </summary>
        /// <param name="d">итерационный параметр</param>
        /// <param name="b">итерационный параметр</param>
        /// <param name="z">координата по оси Z</param>
        /// <param name="a">экваториальная полуось</param>
        /// <param name="powe">квадрат первого эксцентриситета</param>
        /// <returns>Значение геодезической высоты</returns>
        protected static readonly Func<double, double, double, double, double, double> GetValueH = (d, b, z, a, powe) =>
            d * Math.Cos(b) + z * Math.Sin(b) - a * Math.Sqrt(1 - powe * Math.Pow(Math.Sin(b), 2));

        /// <summary>
        /// Метод получения параметра D для итеративного процесса
        /// </summary>
        /// <param name="x">координата по оси Х</param>
        /// <param name="y">координата по оси Y</param>
        /// <returns>Значение параметра D</returns>
        protected static readonly Func<double, double, double> GetValueD = (x, y) =>
            Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));

        /// <summary>
        /// Метод который формирует новую коллекцию со значениями второго эллипсоида
        /// </summary>
        /// <param name="list">исходная коллекция</param>
        /// <returns>новая коллекция с параметрами второго эллипсоида</returns>
        protected static List<T> SelectSecond<T>(List<T> list)
        {
            var newlist = new List<T>();
            for (int i = 0; i < list.Count; i++)
                if (i % 2 != 0) newlist.Add(list[i]);
            return newlist;
        }

        #region TryParse

        /// <summary>
        /// Protected метод, который парсит строку в число если это возможно.
        /// </summary>
        /// <returns>число если преобразование удалось и 0 если нет</returns>
        protected static double TryParse(string str)
        {
            try
            {
                return double.Parse(str);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        #endregion

        #region TranslateMethods

        /// <summary>
        /// Метод, который преобразует секунды в радианы
        /// </summary>
        /// <param name="sec">значение секунд</param>
        /// <returns>радианы секунд</returns>
        protected static readonly Func<double, double> SecToRad = sec => Math.PI * (sec / 3600) / 180.0;

        /// <summary>
        /// Метод который преобразует значение из угловой меры в радианную.
        /// </summary>
        /// <param name="grad">Количество градусов</param>
        /// <param name="sec">Количество секунд</param>
        /// <returns>Значение в градусах</returns>
        protected readonly Func<double, double, double> DegToRad = (grad, min) => Math.PI * (grad + min / 60) / 180.0;

        #endregion

        #endregion

        #region PrivateMathMethods

        /// <summary>
        /// Метод получения параметра R для итеративного процесса
        /// </summary>
        /// <param name="x">координата по оси Х</param>
        /// <param name="y">координата по оси Y</param>
        /// <param name="z">координата по оси Z</param>
        /// <returns>Значение параметра R</returns>
        private static readonly Func<double, double, double, double> GetValueR = (x, y, z) =>
            Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2));

        /// <summary>
        /// Метод получения параметра C для итеративного процесса
        /// </summary>
        /// <param name="z">координата по оси Z</param>
        /// <param name="r">координата по оси Y</param>
        /// <returns>Значение параметра С</returns>
        private static readonly Func<double, double, double> GetValueC = (z, r) => Math.Asin(z / r);

        /// <summary>
        /// Метод получения параметра P для итеративного процесса
        /// </summary>
        /// <param name="powe">квадрат первого эксцентриситета</param>
        /// <param name="a">эваториальная полуось</param>
        /// <param name="r">итерационный параметр</param>
        /// <returns>Значение параметра P</returns>
        private static readonly Func<double, double, double, double> GetValueP = (powe, a, r) => powe * a / (2 * r);

        /// <summary>
        /// Метод получения параметра S для итеративного процесса
        /// </summary>
        /// <param name="p">итерационный параметр</param>
        /// <param name="b">итерационный параметр</param>
        /// <param name="powe">квадрат первого эксцентриситета</param>
        /// <returns>Значение параметра S</returns>
        private static readonly Func<double, double, double, double> GetValueS = (p, b, powe) =>
            Math.Asin(p * Math.Sin(2 * b) / Math.Sqrt(1 - powe * Math.Pow(Math.Sin(b), 2)));

        #endregion
    }
}