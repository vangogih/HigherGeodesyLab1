using System;
using System.Collections.Generic;
using System.IO;
using HigherGeodesyLab1.Helper;
using HigherGeodesyLab1.Transform;
using System.Linq;

namespace HigherGeodesyLab1
{
    static class Program
    {
        public static void Main()
        {
            DateTime start = DateTime.Now;
            var listParamsEllipsoids = new List<Ellipsoid>();
            listParamsEllipsoids.Read("ParamsEllipsoid.csv");
            Ellipsoid.Transform(listParamsEllipsoids);
            listParamsEllipsoids.Save("AllParamsEllipsoid.txt");
//            ===================Преобразование в СК1============================
            var listSc1 = new List<SC1>();
            listSc1.Read("SourceCoordinates.csv");
            SC1.GetValueSc1(listParamsEllipsoids, listSc1);
            listSc1.Save(listParamsEllipsoids, "TransformedCoordinatesSC1.txt");
//            ===================Преобразование в СК2============================
            var listSc2 = new List<SC2>();
            listSc2.Read("TransParams.csv");
            SC2.Sc1ToSc2(listSc1, listSc2, listParamsEllipsoids);
            listSc2.Save(listParamsEllipsoids, "TransformedCoordinatesSC2.txt");
//            ===================Преобразование из СК1 в СК2 BLH=====================
            var listSc2Expression = new List<SC2Expression>();
            SC2Expression.Blh1ToBlh2(listParamsEllipsoids, listSc1, listSc2, listSc2Expression);
            listSc2Expression.Save("TransformedBlhSC1toSC2.txt");
            Console.WriteLine($"{DateTime.Now - start}");
        }
    }
}