using System;
using System.Collections.Generic;
using System.Linq;
using HigherGeodesyLab1.Helper;
using HigherGeodesyLab1.Transform;

namespace HigherGeodesyLab1.Transform
{
    /// <summary>
    /// класс для преобразование (трансформирование) эллипсоидальных координат
    /// </summary>
    public class SC2Expression : GeneralFunctionality
    {
        #region Encapsulation

        public string Name { get; set; }

        /// <summary>
        /// Параметр M
        /// </summary>
        public double M { get; private set; }

        /// <summary>
        /// Параметр N
        /// </summary>
        public double N { get; private set; }

        /// <summary>
        /// разность экваториальных полуосей
        /// </summary>
        public double DeltaA { get; private set; }

        /// <summary>
        /// разность квадратов экцентриситетов
        /// </summary>
        public double DeltaPowE { get; private set; }

        /// <summary>
        /// среднее значение экваториальных полуосей
        /// </summary>
        public double ASc2 { get; private set; }

        /// <summary>
        /// среднее значение квадратов экцентриситето
        /// </summary>
        public double PowESc2 { get; private set; }

        /// <summary>
        /// значение широты в системе Sc2
        /// </summary>
        public double BSc2 { get; private set; }

        /// <summary>
        /// значение долготы в системе Sc2
        /// </summary>
        public double LSc2 { get; private set; }

        /// <summary>
        /// значение геодезической высоты в системе Sc2
        /// </summary>
        public double HSc2 { get; private set; }

        /// <summary>
        /// приращение широты при переходе в систему Sc2
        /// </summary>
        public double DeltaBSc2 { get; private set; }

        /// <summary>
        /// приращение долготы при переходе в систему Sc2
        /// </summary>
        public double DeltaLSc2 { get; private set; }

        /// <summary>
        /// приращение геодезической высоты при переходе в систему Sc2
        /// </summary>
        public double DeltaHSc2 { get; private set; }

        #endregion

        /// <summary>
        /// Константа ро в радианах
        /// </summary>
        private const double Po = 1;//Math.PI * (206234.8062 / 3600) / 180.0;

        #region Ctor

        /// <summary>
        /// Конструктор по умолчанию для параметров трансформирования для значений долготы, широты и геодезической высоты из СК1 в СК2 и обратно
        /// </summary>
        /// <param name="deltaa"></param>
        /// <param name="deltapowe"></param>
        /// <param name="aSc2"></param>
        /// <param name="powESc2Powe"></param>
        private SC2Expression(string name, double deltaa, double deltapowe, double aSc2, double powESc2Powe)
        {
            Name = name;
            DeltaA = deltaa;
            DeltaPowE = deltapowe;
            ASc2 = aSc2;
            PowESc2 = powESc2Powe;
        }

        #endregion

        #region TransformMethods

        /// <summary>
        /// Основной метод который преобразует значения долготы, широты и геодезической высоты из СК1 в СК2
        /// </summary>
        /// <param name="listEllipsoid"></param>
        /// <param name="newlistSc1/param>
        /// <param name="listSc2Matrix"></param>
        /// <param name="listSc2Expression"></param>
        public static void Blh1ToBlh2(List<Ellipsoid> listEllipsoid, List<SC1> listSc1,
            List<SC2> listSc2Matrix, List<SC2Expression> listSc2Expression)
        {
            FillingNewCol1to2(listEllipsoid, listSc1, listSc2Expression);

            List<SC1> newlistSc1 = SelectSecond(listSc1);

            var seq = listSc2Matrix.Zip(newlistSc1, (sc2Matrix, sc1) => new Tuple<SC2, SC1>(sc2Matrix, sc1));
            var mainseq = seq.Zip(listSc2Expression,
                (s, s2E) => new Tuple<Tuple<SC2, SC1>, SC2Expression>(s, s2E));

            foreach (var t in mainseq)
            {
                t.Item2.Name = t.Item1.Item1.Name + " BLH=>SC2";

                t.Item2.M = GetValueM(t.Item2.ASc2, t.Item2.PowESc2, t.Item1.Item2.B); //формула верна
                t.Item2.N = GetValueN(t.Item2.ASc2, t.Item2.PowESc2, t.Item1.Item2.B); //формула верна

                t.Item2.DeltaBSc2 = _getValueDeltaB(t.Item2.M, t.Item2.N, //SC2Expression
                    t.Item1.Item2.H, t.Item1.Item2.B, t.Item1.Item2.L, //SC1
                    t.Item2.ASc2, t.Item2.PowESc2, t.Item2.DeltaA, t.Item2.DeltaPowE, //SC2Expression
                    t.Item1.Item1.Wx, t.Item1.Item1.Wy, t.Item1.Item1.Dm, t.Item1.Item1.Dx, //SC2Matrix
                    t.Item1.Item1.Dy, t.Item1.Item1.Dz);//формула верна

                t.Item2.DeltaLSc2 = _getValueDeltaL(t.Item2.N, t.Item1.Item2.H, t.Item1.Item2.B, t.Item1.Item2.L,
                    t.Item2.PowESc2, t.Item1.Item1.Wx, t.Item1.Item1.Wy, t.Item1.Item1.Wz, t.Item1.Item1.Dx,
                    t.Item1.Item1.Dy);//формула верна

                t.Item2.DeltaHSc2 = _getValueDeltaH(t.Item2.N, //SC2Expression
                    t.Item1.Item2.H, t.Item1.Item2.B, t.Item1.Item2.L, //SC1
                    t.Item2.ASc2, t.Item2.PowESc2, t.Item2.DeltaA, t.Item2.DeltaPowE, //SC2Expression
                    t.Item1.Item1.Wx, t.Item1.Item1.Wy, t.Item1.Item1.Dm, t.Item1.Item1.Dx, //SC2Matrix
                    t.Item1.Item1.Dy, t.Item1.Item1.Dz);//формула верна

                t.Item2.BSc2 = t.Item1.Item2.B + t.Item2.DeltaBSc2;
                t.Item2.LSc2 = t.Item1.Item2.L + t.Item2.DeltaLSc2;
                t.Item2.HSc2 = t.Item1.Item2.H + t.Item2.DeltaHSc2;
            }
        }

        public static void Blh2ToBlh1(List<Ellipsoid> listEllipsoid, List<SC1> listSc1,
            List<SC2> listSc2Matrix, List<SC2Expression> listSc2Expression)
        {
            FillingNewCol2to1(listEllipsoid, listSc1, listSc2Expression);

            List<SC1> newlistSc1 = SelectSecond(listSc1);

            var seq = listSc2Matrix.Zip(newlistSc1, (sc2Matrix, sc1) => new Tuple<SC2, SC1>(sc2Matrix, sc1));
            var mainseq = seq.Zip(listSc2Expression,
                (s, s2E) => new Tuple<Tuple<SC2, SC1>, SC2Expression>(s, s2E));

            foreach (var t in mainseq)
            {
                t.Item2.Name = t.Item2.Name + " BLH=>SC1";

                t.Item2.M = GetValueM(t.Item2.ASc2, t.Item2.PowESc2, t.Item1.Item2.B);
                t.Item2.N = GetValueN(t.Item2.ASc2, t.Item2.PowESc2, t.Item1.Item2.B); //t.Item1.Item2.B

                t.Item2.DeltaBSc2 = _getValueDeltaB(t.Item2.M, t.Item2.N, //SC2Expression
                    t.Item1.Item2.H, t.Item1.Item2.B, t.Item1.Item2.L, //SC1
                    t.Item2.ASc2, t.Item2.PowESc2, t.Item2.DeltaA, t.Item2.DeltaPowE, //SC2Expression
                    t.Item1.Item1.Wx * -1, t.Item1.Item1.Wy * -1, t.Item1.Item1.Dm * -1,
                    t.Item1.Item1.Dx * -1, //SC
                    t.Item1.Item1.Dy * -1, t.Item1.Item1.Dz * -1);

                t.Item2.DeltaLSc2 = _getValueDeltaL(t.Item2.N, t.Item1.Item2.H, t.Item1.Item2.B, t.Item1.Item2.L,
                    t.Item2.PowESc2, t.Item1.Item1.Wx * -1, t.Item1.Item1.Wy * -1, t.Item1.Item1.Wz * -1,
                    t.Item1.Item1.Dx * -1,
                    t.Item1.Item1.Dy * -1);

                t.Item2.DeltaHSc2 = _getValueDeltaH(t.Item2.N, //SC2Expression
                    t.Item1.Item2.H, t.Item1.Item2.B, t.Item1.Item2.L, //SC1
                    t.Item2.ASc2, t.Item2.PowESc2, t.Item2.DeltaA, t.Item2.DeltaPowE, //SC2Expression
                    t.Item1.Item1.Wx * -1, t.Item1.Item1.Wy * -1, t.Item1.Item1.Dm * -1,
                    t.Item1.Item1.Dx * -1, //SC2
                    t.Item1.Item1.Dy * -1, t.Item1.Item1.Dz * -1);

                t.Item2.BSc2 = t.Item2.BSc2 + t.Item2.DeltaBSc2;
                t.Item2.LSc2 = t.Item2.LSc2 + t.Item2.DeltaLSc2;
                t.Item2.HSc2 = t.Item2.HSc2 + t.Item2.DeltaHSc2;
            }
        }

        #endregion

        #region PrivateMethods

        private static void FillingNewCol1to2(List<Ellipsoid> listEllipsoid, List<SC1> listSc1, List<SC2Expression> listSc2Expression)
        {
            for (int i = 0; i < listEllipsoid.Count; i += 2)
                listSc2Expression.Add(new SC2Expression(listSc1.ElementAt(i).Name,
                    Residual(listEllipsoid.ElementAt(i + 1).A, listEllipsoid.ElementAt(i).A),
                    Residual(listEllipsoid.ElementAt(i + 1).PowE, listEllipsoid.ElementAt(i).PowE),
                    Average(listEllipsoid.ElementAt(i + 1).A, listEllipsoid.ElementAt(i).A),
                    Average(listEllipsoid.ElementAt(i + 1).PowE, listEllipsoid.ElementAt(i).PowE)));
        }

        private static void FillingNewCol2to1(List<Ellipsoid> listEllipsoid, List<SC1> listSc1, List<SC2Expression> listSc2Expression)
        {
            for (int i = 0; i < listEllipsoid.Count; i += 2)
                listSc2Expression.Add(new SC2Expression(listSc1.ElementAt(i).Name,
                    Residual(listEllipsoid.ElementAt(i).A, listEllipsoid.ElementAt(i + 1).A),
                    Residual(listEllipsoid.ElementAt(i).PowE, listEllipsoid.ElementAt(i + 1).PowE),
                    Average(listEllipsoid.ElementAt(i + 1).A, listEllipsoid.ElementAt(i).A),
                    Average(listEllipsoid.ElementAt(i + 1).PowE, listEllipsoid.ElementAt(i).PowE)));
        }

        #region MathMethods

        /// <summary>
        /// Метод для нахождения среднего арифметического из двух чисел
        /// </summary>
        private static readonly Func<double, double, double> Average = (d1, d2) => (d1 + d2) / 2;

        /// <summary>
        /// Метод нахождения разности между двумя значениями
        /// </summary>
        private static readonly Func<double, double, double> Residual = (d1, d2) => d1 - d2;

        /// <summary>
        /// Метод для получения значения параметра M
        /// </summary>
        private static readonly Func<double, double, double, double> GetValueM = (a, powe, b) =>
            a * (1 - powe) / Math.Sqrt(Math.Pow(1 - powe * Math.Pow(Math.Sin(b), 2), 3));

        /// <summary>
        /// Метод для получения значения параметра N
        /// </summary>
        private static readonly Func<double, double, double, double> GetValueN = (a, powe, b) =>
            a / Math.Sqrt(1 - powe * Math.Pow(Math.Sin(b), 2));

        /// <summary>
        /// Метод для получения приращения широты
        /// </summary>
        /// <param name="m">параметр M</param>
        /// <param name="n">параметр N</param>
        /// <param name="h">геодзическая высота точки</param>
        /// <param name="b">широта</param>
        /// <param name="l">долгота</param>
        /// <param name="a">среднее значение экваториальных полуосей</param>
        /// <param name="powe">среднее значение квадратов экцентриситетов</param>
        /// <param name="deltaa">разность экваториальных полуосей</param>
        /// <param name="deltapowe">разность квадратов экцентриситетов</param>
        /// <param name="wx">параметр разворота системы по оси x</param>
        /// <param name="wy">параметр разворота системы по оси y</param>
        /// <param name="dm">масштабных коэффициент</param>
        /// <param name="dx">параметр смещения системы по оси x</param>
        /// <param name="dy">параметр смещения системы по оси y</param>
        /// <param name="dz">параметр смещения системы по оси z</param>
        /// <returns>значение прирощения широты</returns>
        private static double _getValueDeltaB(double m, double n, double h, double b, double l, double a, double powe,
            double deltaa, double deltapowe, double wx, double wy, double dm, double dx, double dy, double dz)
        {
            return Po /(m + h) * (n * powe * Math.Sin(b) * Math.Cos(b) * deltaa / a +
                            (Math.Pow(n, 2) / Math.Pow(a, 2) + 1) * n * Math.Sin(b) * Math.Cos(b) * deltapowe / 2 -
                            (dx * Math.Cos(l) + dy * Math.Sin(l)) * Math.Sin(b) + dz * Math.Cos(b)) -
                   wx * Math.Sin(l) * (1 + powe * Math.Cos(2 * b)) +
                   wy * Math.Cos(l) * (1 + powe * Math.Cos(2 * b)) -
                   Po * dm * powe * Math.Sin(b) * Math.Cos(b);
        }

        /// <summary>
        /// Метод для получения приращения долготы
        /// </summary>
        /// <param name="n">параметр N</param>
        /// <param name="h">геодзическая высота точки</param>
        /// <param name="b">широта</param>
        /// <param name="l">долгота</param>
        /// <param name="powe">среднее значение квадратов экцентриситетов</param>
        /// <param name="wx">параметр разворота системы по оси x</param>
        /// <param name="wy">параметр разворота системы по оси y</param>
        /// <param name="wz">параметр разворота системы по оси z</param>
        /// <param name="dx">параметр смещения системы по оси x</param>
        /// <param name="dy">параметр смещения системы по оси y</param>
        /// <returns>значение прирощения долготы</returns>
        private static double _getValueDeltaL(double n, double h, double b, double l, double powe, double wx,
            double wy, double wz, double dx, double dy)
        {
            return Po * (-dx * Math.Sin(l) + dy * Math.Cos(l)) / ((n + h) * Math.Cos(b)) +
                   Math.Tan(b) * (1 - powe) * (wx * Math.Cos(l) + wy * Math.Sin(l)) - wz;
        }

        /// <summary>
        /// Метод для получения приращения геодезической высоты
        /// </summary>
        /// <param name="n">параметр N</param>
        /// <param name="h">геодзическая высота точки</param>
        /// <param name="b">широта</param>
        /// <param name="l">долгота</param>
        /// <param name="a">среднее значение экваториальных полуосей</param>
        /// <param name="powe">среднее значение квадратов экцентриситетов</param>
        /// <param name="deltaa">разность экваториальных полуосей</param>
        /// <param name="deltapowe">разность квадратов экцентриситетов</param>
        /// <param name="wx">параметр разворота системы по оси x</param>
        /// <param name="wy">параметр разворота системы по оси y</param>
        /// <param name="dm">масштабных коэффициент</param>
        /// <param name="dx">параметр смещения системы по оси x</param>
        /// <param name="dy">параметр смещения системы по оси y</param>
        /// <param name="dz">параметр смещения системы по оси z</param>
        /// <returns>значение прирощения геодезичской высоты</returns>
        private static double _getValueDeltaH(double n, double h, double b, double l, double a, double powe,
            double deltaa, double deltapowe, double wx, double wy, double dm, double dx, double dy, double dz)
        {
            return -(a / n) * deltaa +
                   n * Math.Pow(Math.Sin(b), 2) * deltapowe / 2 +
                   (dx * Math.Cos(l) + dy * Math.Sin(l)) * Math.Cos(b) +
                   dz * Math.Sin(b) -
                   n * powe * Math.Sin(b) * Math.Cos(b) * (wx / Po * Math.Sin(l) - wy / Po * Math.Cos(l)) +
                   (Math.Pow(a, 2) / n + h) * dm;
        }

        #endregion
        #endregion

    }
}