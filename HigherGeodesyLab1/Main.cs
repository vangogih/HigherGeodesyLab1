using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using HigherGeodesyLab1.Helper;
using HigherGeodesyLab1.Transform;
using System.Linq;

namespace HigherGeodesyLab1
{
    enum Variant
    {
        V7,
        V21,
        V54
    }
    
    static class Program 
    {
        public static void Main() 
        {
            Console.WriteLine($"{Variant.V7.ToString()}");
            // 0. Получаем параметры элипсоида
            var listParamsEllipsoidsWGS84 = new List<Ellipsoid>();
            var listParamsEllipsoidsPZ90 = new List<Ellipsoid>();
            listParamsEllipsoidsWGS84.Read("ParamsEllipsoid(WGS-84).csv");
            listParamsEllipsoidsPZ90.Read("ParamsEllipsoid(PZ-90).csv");
            Ellipsoid.CalculateParamsEllipsoid(listParamsEllipsoidsWGS84);
            Ellipsoid.CalculateParamsEllipsoid(listParamsEllipsoidsPZ90);
            listParamsEllipsoidsWGS84.Save("ParamsEllipsoidWGS84.txt");
            listParamsEllipsoidsPZ90.Save("ParamsEllipsoidPZ90.txt");

            // 1. Трансформируем СК1 в СК2 матричным перемножением через параметры перехода
            var listSc1 = new List<SC1>();
            var listSc2 = new List<SC2>();
            listSc2.Read("TransParams V.7.csv");
            
            listSc1.ReadForXYZ($"SourceCoordinatesXYZ {Variant.V7.ToString()}.csv");
            SC2.Sc1ToSc2(listSc1, listSc2, listParamsEllipsoidsWGS84);
            listSc2.Save(listParamsEllipsoidsWGS84, $"TransformedCoordinatesSC2 {Variant.V7.ToString()}.txt");
            
            listSc1.ReadForXYZ($"SourceCoordinatesXYZ {Variant.V21.ToString()}.csv");
            SC2.Sc1ToSc2(listSc1, listSc2, listParamsEllipsoidsWGS84);
            listSc2.Save(listParamsEllipsoidsWGS84, $"TransformedCoordinatesSC2 {Variant.V21.ToString()}.txt");
            
            listSc1.ReadForXYZ($"SourceCoordinatesXYZ {Variant.V54.ToString()}.csv");
            SC2.Sc1ToSc2(listSc1, listSc2, listParamsEllipsoidsWGS84);
            listSc2.Save(listParamsEllipsoidsWGS84, $"TransformedCoordinatesSC2 {Variant.V54.ToString()}.txt");

            // 2. Переводим СК1 в СК2 через XYZ итеративным методом
            DifferentTransform.GetBlhFromXyz(TransformType.Iterarion, listParamsEllipsoidsWGS84, listSc2);
            listSc2.Save(listParamsEllipsoidsWGS84, "TransCoordBLHfromXYZIteration.txt");
            // 2. Переводим СК1 в СК2 через XYZ 1-ым методом Боуринга
            DifferentTransform.GetBlhFromXyz(TransformType.FirstBouring, listParamsEllipsoidsWGS84, listSc2);
            listSc2.Save(listParamsEllipsoidsWGS84, "TransCoordBLHfromXYZBouring1.txt");
            // 2. Переводим СК1 в СК2 через XYZ 2-ым методом Боуринга
            DifferentTransform.GetBlhFromXyz(TransformType.SecondBouring, listParamsEllipsoidsWGS84, listSc2);
            listSc2.Save(listParamsEllipsoidsWGS84, "TransCoordBLHfromXYZBouring2.txt");
            
            
        }

        [Obsolete]
        public static void Mode1()
        {
            var listParamsEllipsoids = new List<Ellipsoid>();
            listParamsEllipsoids.Read("ParamsEllipsoid.csv");
            Ellipsoid.CalculateParamsEllipsoid(listParamsEllipsoids);
            listParamsEllipsoids.Save("AllParamsEllipsoid.txt");

            var listSc1 = new List<SC1>();
            listSc1.ReadForBLH("SourceCoordinatesBLH.csv");
            SC1.GetBlhXyzFromCloseBlhSc1(listParamsEllipsoids, listSc1);
            listSc1.Save(listParamsEllipsoids, "TransformedCoordinatesSC1FromXYZ.txt");

            var listSc2 = new List<SC2>();
            listSc2.Read("TransParams.csv");
            SC2.Sc1ToSc2(listSc1, listSc2, listParamsEllipsoids);
            listSc2.Save(listParamsEllipsoids, "TransformedCoordinatesSC2.txt");

            var listSc2Expression = new List<SC2Expression>();
            SC2Expression.Blh1ToBlh2(listParamsEllipsoids, listSc1, listSc2, listSc2Expression);
            listSc2Expression.Save("TransformedBlhSC1toSC2.txt");
        }
    }
}