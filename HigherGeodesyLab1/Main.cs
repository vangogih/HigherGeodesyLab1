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
            // 1. Получаем параметры элипсоида
            var listParamsEllipsoids = new List<Ellipsoid>();
            listParamsEllipsoids.Read("ParamsEllipsoid.csv");
            Ellipsoid.CalculateParamsEllipsoid(listParamsEllipsoids);
            listParamsEllipsoids.Save("AllParamsEllipsoid.txt");

            // 2. Трансформируем СК1 в СК2 матричным перемножением через параметры перехода
            var listSc1 = new List<SC1>();
            var listSc2 = new List<SC2>();
            listSc1.ReadForXYZ("SourceCoordinatesXYZ.csv");
            listSc2.Read("TransParams.csv");
            SC2.Sc1ToSc2(listSc1, listSc2, listParamsEllipsoids);
            listSc2.Save(listParamsEllipsoids, "TransformedCoordinatesSC2.txt");

            // 3. Переводим СК1 в СК2 через XYZ итеративным методом
            SC1.GetBlhFromXyzIteration(listParamsEllipsoids, listSc1);
            listSc1.Save(listParamsEllipsoids, "TransCoordBLHfromXYZIteration.txt");
            // 3. Переводим СК1 в СК2 через XYZ 1-ым методом Боуринга
            SC1.GEtBlhFromXyzBouring1(listParamsEllipsoids, listSc1);
            listSc1.Save(listParamsEllipsoids, "TransCoordBLHfromXYZBouring1.txt");
            // 3. Переводим СК1 в СК2 через XYZ 2-ым методом Боуринга
            SC1.GEtBlhFromXyzBouring2(listParamsEllipsoids, listSc1);
            listSc1.Save(listParamsEllipsoids, "TransCoordBLHfromXYZBouring2.txt");
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