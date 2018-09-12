using System;
using System.Collections.Generic;
using HigherGeodesyLab1.Helper;

namespace HigherGeodesyLab1.Transform
{
    public class DifferentTransform : GeneralFunctionality
    {
        public static void GetBlhFromXyz(TransformType type, List<Ellipsoid> ellipsoids,
            List<SC1> sc1s)
        {
            var seq = ZipCollections.MyZip(sc1s, ellipsoids,
                (e, b) => new Tuple<SC1, Ellipsoid>(e, b));
            switch (type)
            {
                case TransformType.Iterarion:
                    foreach (var tuple in seq)
                    {
                        tuple.Item1.Name = tuple.Item1.Name + "XYZ=>BLH";

                        tuple.Item1._d = GetValueD(tuple.Item1.X, tuple.Item1.Y);

                        tuple.Item1.B = Iteration(tuple.Item1.X, tuple.Item1.Y, tuple.Item1.Z, tuple.Item2.PowE,
                            tuple.Item2.A);
                        tuple.Item1.L = GetValueL(tuple.Item1.Y, tuple.Item1._d);
                        tuple.Item1.H = GetValueH(tuple.Item1._d, tuple.Item1.B, tuple.Item1.Z, tuple.Item2.A,
                            tuple.Item2.PowE);
                    }

                    break;
                case TransformType.FirstBouring:
                    foreach (Tuple<SC1, Ellipsoid> tuple in seq)
                    {
                        tuple.Item1._d = GetValueD(tuple.Item1.X, tuple.Item1.Y); //Q
                        tuple.Item1._r =
                            GetValueRBouring(tuple.Item2.PowE, tuple.Item1.X, tuple.Item1.Y, tuple.Item1.Z);

                        tuple.Item1.B =
                            GetValueBBouring1(tuple.Item2.PowE, tuple.Item2.PowEAmp, tuple.Item1.Z, tuple.Item1._d,
                                tuple.Item2.B, tuple.Item1._r);
                        tuple.Item1.L = GetValueL(tuple.Item1.Y, tuple.Item1._d);
                        tuple.Item1.H = GetValueHBouring(tuple.Item1._d, tuple.Item1.B, tuple.Item1.Z, tuple.Item2.A,
                            tuple.Item2.PowE);
                    }

                    break;
                case TransformType.SecondBouring:
                    foreach (Tuple<SC1, Ellipsoid> tuple in seq)
                    {
                        tuple.Item1._d = GetValueD(tuple.Item1.X, tuple.Item1.Y); //Q

                        tuple.Item1.L = GetVAlueUBouring2(tuple.Item1.Z, tuple.Item1._d, tuple.Item2.PowE);
                        tuple.Item1.B = GetValueBBouring2(tuple.Item1.Z, tuple.Item1._d, tuple.Item2.PowE,
                            tuple.Item2.A,
                            tuple.Item1.L);
                        tuple.Item1.H = GetValueHBouring(tuple.Item1._d, tuple.Item1.B, tuple.Item1.Z, tuple.Item2.A,
                            tuple.Item2.PowE);
                    }

                    break;
            }
        }

        public static void GetBlhFromXyz(TransformType type, List<Ellipsoid> ellipsoids,
            List<SC2> sc1s)
        {
            var seq = ZipCollections.MyZip(sc1s, ellipsoids,
                (e, b) => new Tuple<SC2, Ellipsoid>(e, b));
            switch (type)
            {
                case TransformType.Iterarion:
                    foreach (var tuple in seq)
                    {
                        tuple.Item1.Name = tuple.Item1.Name + "XYZ=>BLH";

                        tuple.Item1._d = GetValueD(tuple.Item1.X, tuple.Item1.Y);

                        tuple.Item1.B = Iteration(tuple.Item1.X, tuple.Item1.Y, tuple.Item1.Z, tuple.Item2.PowE,
                            tuple.Item2.A);
                        tuple.Item1.L = GetValueL(tuple.Item1.Y, tuple.Item1._d);
                        tuple.Item1.H = GetValueH(tuple.Item1._d, tuple.Item1.B, tuple.Item1.Z, tuple.Item2.A,
                            tuple.Item2.PowE);
                    }

                    break;
                case TransformType.FirstBouring:
                    foreach (Tuple<SC2, Ellipsoid> tuple in seq)
                    {
                        tuple.Item1._d = GetValueD(tuple.Item1.X, tuple.Item1.Y); //Q
                        tuple.Item1._r =
                            GetValueRBouring(tuple.Item2.PowE, tuple.Item1.X, tuple.Item1.Y, tuple.Item1.Z);

                        tuple.Item1.B =
                            GetValueBBouring1(tuple.Item2.PowE, tuple.Item2.PowEAmp, tuple.Item1.Z, tuple.Item1._d,
                                tuple.Item2.B, tuple.Item1._r);
                        tuple.Item1.L = GetValueL(tuple.Item1.Y, tuple.Item1._d);
                        tuple.Item1.H = GetValueHBouring(tuple.Item1._d, tuple.Item1.B, tuple.Item1.Z, tuple.Item2.A,
                            tuple.Item2.PowE);
                    }

                    break;
                case TransformType.SecondBouring:
                    foreach (Tuple<SC2, Ellipsoid> tuple in seq)
                    {
                        tuple.Item1._d = GetValueD(tuple.Item1.X, tuple.Item1.Y); //Q

                        tuple.Item1.L = GetVAlueUBouring2(tuple.Item1.Z, tuple.Item1._d, tuple.Item2.PowE);
                        tuple.Item1.B = GetValueBBouring2(tuple.Item1.Z, tuple.Item1._d, tuple.Item2.PowE,
                            tuple.Item2.A,
                            tuple.Item1.L);
                        tuple.Item1.H = GetValueHBouring(tuple.Item1._d, tuple.Item1.B, tuple.Item1.Z, tuple.Item2.A,
                            tuple.Item2.PowE);
                    }

                    break;
            }
        }

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
    }
}