using System;
using System.Collections.Generic;
using System.Linq;
using HigherGeodesyLab1.Helper;


namespace HigherGeodesyLab1.Transform
{
    /// <inheritdoc />
    /// <summary>
    /// Класс для преобразования элипсоидальных и геоцентрических координат
    /// </summary>
    public class SC1 : GeneralFunctionality
    {
        #region Encapsulation

        /// <summary>
        /// Имя элипсоида
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// широта 
        /// </summary>
        public double B { get; private set; }

        /// <summary>
        /// долгота
        /// </summary>
        public double L { get; private set; }

        /// <summary>
        /// координата по оси Х в системе координат Sc1
        /// </summary>
        public double X { get; private set; }

        /// <summary>
        /// координата по оси Y в системе координат Sc1
        /// </summary>
        public double Y { get; private set; }

        /// <summary>
        /// координата по оси Z в системе координат Sc1
        /// </summary>
        public double Z { get; private set; }

        /// <summary>
        /// высота геодезическая
        /// </summary>
        public double H { get; private set; }

        #endregion

        /// <summary>
        /// радиус кривизны первого вертикала
        /// </summary>
        private static double _n;

        private double _d;
        private double _r;

        #region Ctor

        /// <summary>
        /// Конструктор для преобразования элипсоидальных и геоцентрических координат
        /// </summary>
        /// <param name="name">Имя элипсоида</param>
        /// <param name="bdegrees">широта в градусах</param>
        /// <param name="bseconds">широта в секундах</param>
        /// <param name="ldegrees">долгота в градусах</param>
        /// <param name="lseconds">долгота в секундах</param>
        /// <param name="h">высота</param>
        public SC1(string name, string bdegrees, string bseconds, string ldegrees, string lseconds, string h)
        {
            Name = name;
            H = TryParse(h);
            try
            {
                B = DegToRad(double.Parse(bdegrees), double.Parse(bseconds));
                L = DegToRad(double.Parse(ldegrees), double.Parse(lseconds));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Пользщовательский конструктор прямоугольных координат
        /// </summary>
        public SC1(string name, string _x, string _y, string _z)
        {
            Name = name;
            X = TryParse(_x);
            Y = TryParse(_y);
            Z = TryParse(_z);
        }

        #endregion

        #region TransformMethod

        /// <summary>
        /// Основной метод в котором осуществляется преобразование элипсоидальных и геоцентрических координат
        /// </summary>
        /// <param name="listEllipsoid">список с параметрами элипсоида</param>
        /// <param name="listBlHtoXyz">список геодезических координат точек</param>
        public static void GetBlhXyzFromCloseBlhSc1(List<Ellipsoid> listEllipsoid, List<SC1> listBlHtoXyz)
        {
            var seq = ZipCollections.MyZip(listBlHtoXyz, listEllipsoid,
                (e, b) => new Tuple<SC1, Ellipsoid>(e, b)); //Объединяем две коллекции

            foreach (var tuple in seq)
            {
                tuple.Item1.Name = tuple.Item1.Name + "=>SC1";
                _n = GetValueN(tuple.Item2.A, tuple.Item2.PowE, tuple.Item1.B);
                tuple.Item1.X = GetValueX(_n, tuple.Item1.H, tuple.Item1.B, tuple.Item1.L);
                tuple.Item1.Y = GetValueY(_n, tuple.Item1.H, tuple.Item1.B, tuple.Item1.L);
                tuple.Item1.Z = GetValueZ(tuple.Item2.PowE, _n, tuple.Item1.H, tuple.Item1.B);

                tuple.Item1._d = GetValueD(tuple.Item1.X, tuple.Item1.Y);

                tuple.Item1.B = Iteration(tuple.Item1.X, tuple.Item1.Y, tuple.Item1.Z, tuple.Item2.PowE, tuple.Item2.A);
                tuple.Item1.L = GetValueL(tuple.Item1.Y, tuple.Item1._d);
                tuple.Item1.H = GetValueH(tuple.Item1._d, tuple.Item1.B, tuple.Item1.Z, tuple.Item2.A,
                    tuple.Item2.PowE);
            }
        }

        public static void GetBlhFromXyzIteration(List<Ellipsoid> ellipsoids, List<SC1> sc1s)
        {
            var seq = ZipCollections.MyZip(sc1s, ellipsoids,
                (e, b) => new Tuple<SC1, Ellipsoid>(e, b));

            foreach (Tuple<SC1, Ellipsoid> tuple in seq)
            {
                tuple.Item1.Name = tuple.Item1.Name + "XYZ=>BLH";

                tuple.Item1._d = GetValueD(tuple.Item1.X, tuple.Item1.Y);

                tuple.Item1.B = Iteration(tuple.Item1.X, tuple.Item1.Y, tuple.Item1.Z, tuple.Item2.PowE, tuple.Item2.A);
                tuple.Item1.L = GetValueL(tuple.Item1.Y, tuple.Item1._d);
                tuple.Item1.H = GetValueH(tuple.Item1._d, tuple.Item1.B, tuple.Item1.Z, tuple.Item2.A,
                    tuple.Item2.PowE);
            }
        }

        public static void GEtBlhFromXyzBouring1(List<Ellipsoid> ellipsoids, List<SC1> sc1s)
        {
            var seq = ZipCollections.MyZip(sc1s, ellipsoids,
                (e, b) => new Tuple<SC1, Ellipsoid>(e, b));

            foreach (Tuple<SC1, Ellipsoid> tuple in seq)
            {
                tuple.Item1._d = GetValueD(tuple.Item1.X, tuple.Item1.Y); //Q
                tuple.Item1._r = GetValueRBouring(tuple.Item2.PowE, tuple.Item1.X, tuple.Item1.Y, tuple.Item1.Z);

                tuple.Item1.B =
                    GetValueBBouring1(tuple.Item2.PowE, tuple.Item2.PowEAmp, tuple.Item1.Z, tuple.Item1._d,
                        tuple.Item2.B, tuple.Item1._r);
                tuple.Item1.L = GetValueL(tuple.Item1.Y, tuple.Item1._d);
                tuple.Item1.H = GetValueHBouring(tuple.Item1._d, tuple.Item1.B, tuple.Item1.Z, tuple.Item2.A,
                    tuple.Item2.PowE);
            }
        }

        public static void GEtBlhFromXyzBouring2(List<Ellipsoid> ellipsoids, List<SC1> sc1s)
        {
            var seq = ZipCollections.MyZip(sc1s, ellipsoids,
                (e, b) => new Tuple<SC1, Ellipsoid>(e, b));

            foreach (Tuple<SC1, Ellipsoid> tuple in seq)
            {
                tuple.Item1._d = GetValueD(tuple.Item1.X, tuple.Item1.Y); //Q

                tuple.Item1.L = GetVAlueUBouring2(tuple.Item1.Z, tuple.Item1._d, tuple.Item2.PowE);
                tuple.Item1.B = GetValueBBouring2(tuple.Item1.Z, tuple.Item1._d, tuple.Item2.PowE, tuple.Item2.A,
                    tuple.Item1.L);
                tuple.Item1.H = GetValueHBouring(tuple.Item1._d, tuple.Item1.B, tuple.Item1.Z, tuple.Item2.A,
                    tuple.Item2.PowE);
            }
        }

        #endregion

        #region MathMethods

        /// <summary>
        /// Метод для получения значения кривизны первого вертикала
        /// </summary>
        /// <param name="a">экваториальная полуось</param>
        /// <param name="powe">Квадрат первого эксцентриситета</param>
        /// <param name="b">Значение широты</param>
        /// <returns>Значение в градусах</returns>
        public static readonly Func<double, double, double, double> GetValueN = (a, powe, b) =>
            a / Math.Sqrt(1 - powe * Math.Pow(Math.Sin(b), 2));

        /// <summary>
        /// Метод для получения значения геоцентрической координаты X
        /// </summary>
        /// <param name="n">радиус кривизны первого вертикала</param>
        /// <param name="h">геодезическая высота точки</param>
        /// <param name="b">Значение широты</param>
        /// <param name="l">Значение долготы</param>
        /// <returns>Значение в градусах</returns>
        public static readonly Func<double, double, double, double, double> GetValueX = (n, h, b, l) =>
            (n + h) * Math.Cos(b) * Math.Cos(l);

        /// <summary>
        /// Метод для получения значения геоцентрической координаты Y
        /// </summary>
        /// <param name="n">радиус кривизны первого вертикала</param>
        /// <param name="h">геодезическая высота точки</param>
        /// <param name="b">Значение широты</param>
        /// <param name="l">Значение долготы</param>
        /// <returns>Значение в градусах</returns>
        public static readonly Func<double, double, double, double, double> GetValueY = (n, h, b, l) =>
            (n + h) * Math.Cos(b) * Math.Sin(l);

        /// <summary>
        /// Метод для получения значения геоцентрической координаты Z
        /// </summary>
        /// <param name="powe">Квадрат первого эксцентриситета</param>
        /// <param name="n">радиус кривизны первого вертикала</param>
        /// <param name="h">Значение широты</param>
        /// <param name="b">Значение долготы</param>
        /// <returns>Значение в градусах</returns>
        public static readonly Func<double, double, double, double, double> GetValueZ = (powe, n, h, b) =>
            ((1 - powe) * n + h) * Math.Sin(b);

        private static readonly Func<double, double, double, double, double, double, double> GetValueBBouring1 =
            (powe, poweAmp, z, q, b, r) => Math.Atan(z / q *
                                                     ((Math.Pow(r, 3) + b * poweAmp * Math.Pow(z, 2)) /
                                                     (Math.Pow(r, 3) - b * powe * (1 - powe) * Math.Pow(q, 2))));

        private static readonly Func<double, double, double, double, double, double> GetValueBBouring2 =
            (z, q, powe, a, u) => Math.Atan((z + powe * a * Math.Pow(Math.Sin(u), 3) / Math.Sqrt(1 - powe))
                                            / (q - powe * a * Math.Pow(Math.Cos(u), 3)));

        private static readonly Func<double, double, double, double, double> GetValueRBouring =
            (powe, x, y, z) => Math.Sqrt(Math.Pow(z, 2) + (Math.Pow(x, 2) + Math.Pow(y, 2)) * (1 - powe));

        private static readonly Func<double, double, double, double, double, double> GetValueHBouring =
            (q, b, z, a, powe) =>
                q * Math.Cos(b) + z * Math.Sin(b) - a * Math.Sqrt(1 - powe * Math.Pow(Math.Sin(b), 2));

        private static readonly Func<double, double, double, double> GetVAlueUBouring2 =
            (z, q, powe) => Math.Atan(z / q * Math.Sqrt(1 - powe));

        #endregion
    }
}