using System.Collections.Generic;
using HigherGeodesyLab1.Helper;

namespace HigherGeodesyLab1.Transform
{
    public class Topocentric : GeneralFunctionality
    {
        public double Dq { get; private set; }
        public double A12 { get; private set; }
        public double V12 { get; private set; }
        public double Z12 { get; private set; }

        public static void Transform(List<SC1> listsSc1)
        {
            List<Topocentric> topocentrics = new List<Topocentric>();
            Matrix sc2Matrix = new Matrix(3,1);
            //|Xr|
            //|Yr|
            //|Zr|2
            Matrix sc1Matrix = new Matrix(3, 1);
            //|Xr|
            //|Yr|
            //|Zr|1
            Matrix paramsTransformMatrix = new Matrix(3, 3);
            //|-sin(B)cos(L),-sin(L),cos(B)cos(L)|
            //|-sin(B)cos(L),cos(L),cos(B)cos(L)|
            //|cos(B),0,sin(B)|


        }
    }
}