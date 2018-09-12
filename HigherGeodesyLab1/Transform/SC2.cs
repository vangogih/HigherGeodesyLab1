using System;
using System.Collections.Generic;
using HigherGeodesyLab1.Helper;
using System.Linq;


namespace HigherGeodesyLab1.Transform
{
    /// <inheritdoc />
    /// <summary>
    ///  Класс для преобразования эллипсоидальных и геоцентрических координат
    /// </summary>
    public class SC2 : GeneralFunctionality
    {
        #region Encapsulation

        /// <summary>
        /// имя эллипсоида
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// параметр смещения системы по оси X
        /// </summary>
        public double Dx { get; private set; }

        /// <summary>
        /// параметр смещения системы по оси Y
        /// </summary>
        public double Dy { get; private set; }

        /// <summary>
        /// параметр смещения системы по оси Z
        /// </summary>
        public double Dz { get; private set; }

        /// <summary>
        /// параметр разворота системы по оси X
        /// </summary>
        public double Wx { get; private set; }

        /// <summary>
        /// параметр разворота системы по оси Y
        /// </summary>
        public double Wy { get; private set; }

        /// <summary>
        /// параметр разворота системы по оси Z
        /// </summary>
        public double Wz { get; private set; }

        /// <summary>
        /// масштабных коэффициент
        /// </summary>
        public double Dm { get; private set; }

        /// <summary>
        /// Координата X в системе SC2
        /// </summary>
        public double X { get; private set; }

        /// <summary>
        /// Координата Y в системе SC2
        /// </summary>
        public double Y { get; private set; }

        /// <summary>
        /// Координата Z в системе SC2
        /// </summary>
        public double Z { get; private set; }

        /// <summary>
        /// Долгота в системе координат Sc2
        /// </summary>
        public double B { get; set; }

        /// <summary>
        /// Широта в системе координат Sc2
        /// </summary>
        public double L { get; set; }

        /// <summary>
        /// Геодезическая высота в системе координат Sc2
        /// </summary>
        public double H { get; set; }

        private const double Scaledenominator = 10000000;

        public double _d;
        public double _r;

        #endregion

        #region Ctor

        /// <summary>
        /// Конструктор по умолчанию для параметров трансформирования из СК1 в СК2 и обратно
        /// </summary>
        public SC2(string name, string dx, string dy, string dz, string wx, string wy, string wz, string dm)
        {
            Name = name;
            Dx = TryParse(dx);
            Dy = TryParse(dy);
            Dz = TryParse(dz);
            Dm = TryParse(dm) / Scaledenominator; //масштабный коэффициент
            try
            {
                Wx = SecToRad(TryParse(wx));
                Wz = SecToRad(TryParse(wz));
                Wy = SecToRad(TryParse(wy));
            }
            catch (Exception e)
            {
//                Console.WriteLine($"Параметр Wx или Wy или Wz: {e}");
                throw;
            }
        }

        #endregion

        #region TransformMethods

        /// <summary>
        /// Преборазование прямоугольной системы координат СК1 в СК2 
        /// </summary>
        /// <param name="listSc1">коллекция с параметрами системы SC1</param>
        /// <param name="listParamsSc2">коллекция с параметрами системы SC2</param>
        public static void Sc1ToSc2(List<SC1> listSc1, List<SC2> listParamsSc2, List<Ellipsoid> listEllipsoid)
        {
//            List<SC1> newlistSc1 = SelectSecond(listSc1);
//            List<Ellipsoid> newlistEllipsoid = SelectSecond(listEllipsoid);

            MathMod.Matrix sc2Matrix = new MathMod.Matrix(3, 1);
            //|Xp|
            //|Yp|
            //|Zp|
            MathMod.Matrix sc1Matrix = new MathMod.Matrix(3, 1);
            //|Xp|
            //|Yp|
            //|Zp|
            MathMod.Matrix paramsTransformMatrix = new MathMod.Matrix(3, 3);
            //|1,Wz(rad),-Wy(rad)|
            //|-Wz(rad),1,-Wx(rad)|
            //|-Wy(rad),-Wx(rad),1|
            MathMod.Matrix deltaMatrix = new MathMod.Matrix(3, 1);
            //|deltaXo|
            //|deltaYo|
            //|deltaZo|

            IEnumerable<Tuple<SC1, SC2, Ellipsoid>> seq =
                ZipCollections.MyZip(listSc1, listParamsSc2, listEllipsoid,
                    (first, second, third) => new Tuple<SC1, SC2, Ellipsoid>(first, second, third));

            foreach (var t in seq)
            {
                t.Item2.Name = t.Item1.Name + "=>SC2";

                //Инициализируем матрицу SC1
                sc1Matrix.Matr[0, 0] = t.Item1.X;
                sc1Matrix.Matr[1, 0] = t.Item1.Y;
                sc1Matrix.Matr[2, 0] = t.Item1.Z;

                //Инициализируем матрицу с дельтами
                deltaMatrix.Matr[0, 0] = t.Item2.Dx;
                deltaMatrix.Matr[1, 0] = t.Item2.Dy;
                deltaMatrix.Matr[2, 0] = t.Item2.Dz;

                //Инициализируем матрицу параметров трансформирования
                paramsTransformMatrix.Matr[0, 0] = 1; //строка, столбец
                paramsTransformMatrix.Matr[0, 1] = t.Item2.Wz;
                paramsTransformMatrix.Matr[0, 2] = t.Item2.Wy * -1;
                //=
                paramsTransformMatrix.Matr[1, 0] = t.Item2.Wz * -1;
                paramsTransformMatrix.Matr[1, 1] = 1;
                paramsTransformMatrix.Matr[1, 2] = t.Item2.Wx;
                //=
                paramsTransformMatrix.Matr[2, 0] = t.Item2.Wy;
                paramsTransformMatrix.Matr[2, 1] = t.Item2.Wx * -1;
                paramsTransformMatrix.Matr[2, 2] = 1;

                sc2Matrix = (1 + t.Item2.Dm) * paramsTransformMatrix * sc1Matrix + deltaMatrix;

                t.Item2.X = sc2Matrix.Matr[0, 0];
                t.Item2.Y = sc2Matrix.Matr[1, 0];
                t.Item2.Z = sc2Matrix.Matr[2, 0];

                t.Item2._d = GetValueD(t.Item2.X, t.Item2.Y);
//
//                t.Item2.B = Iteration(t.Item2.X, t.Item2.Y, t.Item2.Z, t.Item3.PowE, t.Item3.A);
//                t.Item2.L = GetValueL(t.Item2.Y, t.Item2._d);
//                t.Item2.H = GetValueH(t.Item2._d, t.Item2.B, t.Item2.Z, t.Item3.A,
//                    t.Item3.PowE);

                DifferentTransform.GetBlhFromXyz(TransformType.Iterarion, listEllipsoid, listParamsSc2);
            }
        }

        #endregion
    }
}