using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HigherGeodesyLab1.Transform;

namespace HigherGeodesyLab1.Helper
{
    /// <summary>
    /// Класс для чтения файло и записи результатов
    /// </summary>
    internal static class SaveRead
    {
        #region ReadSaveEllipsoid

        /// <summary>
        /// Метод для чтения данных из файла 
        /// </summary>
        /// <param name="dataList">Список с параметрами</param>
        /// <param name="fileName">Имя файла загрузки данных</param>
        public static void Read(this List<Ellipsoid> dataList, string fileName)
        {
            try
            {
                using (var sr = new StreamReader(fileName))
                {
                    while (!sr.EndOfStream)
                    {
                        var readLine = sr.ReadLine();
                        if (readLine != null)
                        {
                            var read = readLine.Split(';'); //name,a,b,alf,e,e',e^2,e'^2
                            dataList.Add(new Ellipsoid(read[0], read[1], read[2], read[3], read[4], read[5], read[6],
                                read[7]));
                        }
                    }
                    sr.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка ввода/вывода: {e}");
                throw;
            }
        }

        /// <summary>
        /// Метод для записи данных в файл
        /// </summary>
        /// <param name="dataList">Список с параметрами</param>
        /// <param name="fileName">Имя файла загрузки данных</param>
        public static void Save(this List<Ellipsoid> dataList, string fileName)
        {
            using (var fs = new StreamWriter(fileName))
            {
                dataList.ForEach(item =>
                    fs.WriteLine(
                        $"{item.Name}|a={item.A:F2}|b={item.B:F2}|alf={item.Alf:F5}|e={item.E:F5}|e'={item.EAmp:F5}|e^2={item.PowE:F5}|e'^2={item.PowEAmp:F5}"));
                fs.Close();
            }
        }

        #endregion

        #region ReadSaveSC1

        public static void Read(this List<SC1> dataList, string fileName)
        {
            try
            {
                using (var sr = new StreamReader(fileName))
                {
                    while (!sr.EndOfStream)
                    {
                        var readLine = sr.ReadLine();
                        if (readLine != null)
                        {
                            var read = readLine.Split(';'); //name,bdegrees,bseconds,ldegrees,lseconds,H
                            dataList.Add(new SC1(read[0], read[1], read[2], read[3], read[4], read[5]));
                        }
                    }
                    sr.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка ввода/вывода: {e}");
                throw;
            }
        }

        public static void Save(this List<SC1> dataList, List<Ellipsoid> listEllipdoid, string fileName)
        {
            IEnumerable<Tuple<SC1, Ellipsoid>> seq =
                dataList.Zip(listEllipdoid, (sc1, sc2) => new Tuple<SC1, Ellipsoid>(sc1, sc2));

            using (var fs = new StreamWriter(fileName))
            {
                foreach (var tuple in seq)
                {
                    fs.WriteLine(
                        $"{tuple.Item1.Name}|B={tuple.Item1.B:F15}|L={tuple.Item1.L:F15}|H={tuple.Item1.H:F15}|X={tuple.Item1.X:F5}|Y={tuple.Item1.Y:F5}|Z={tuple.Item1.Z:F5}|A={tuple.Item2.A}|PowE={tuple.Item2.PowE}");
                }
//                seq.ForEach(item =>
//                    fs.WriteLine(
//                        $"{item.Name}|B={item.B:F5}|L={item.L:F5}|H={item.H:F5}|X={item.X:F5}|Y={item.Y:F5}|Z={item.Z:F5}"));
                fs.Close();
            }
        }

        #endregion

        #region ReadSaveSC2

        public static void Read(this List<SC2> dataList, string fileName)
        {
            try
            {
                using (var sr = new StreamReader(fileName))
                {
                    while (!sr.EndOfStream)
                    {
                        var readLine = sr.ReadLine();
                        if (readLine != null)
                        {
                            var read = readLine.Split(';'); //name,dx,dy,dz,wx,wy,wz,dm
                            dataList.Add(new SC2(read[0], read[1], read[2], read[3], read[4], read[5], read[6],
                                read[7]));
                        }
                    }
                    sr.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка ввода/вывода: {e}");
                throw;
            }
        }

        public static void Save(this List<SC2> dataList,List<Ellipsoid> listEllipdoid, string fileName)
        {
            List<Ellipsoid> newlistEllipsoid = new List<Ellipsoid>();
            for (int i = 0; i < listEllipdoid.Count; i++)
                if (i % 2 != 0) newlistEllipsoid.Add(listEllipdoid[i]);

            IEnumerable<Tuple<SC2, Ellipsoid>> seq =
                dataList.Zip(newlistEllipsoid, (sc1, sc2) => new Tuple<SC2, Ellipsoid>(sc1, sc2));
            using (var fs = new StreamWriter(fileName))
            {
                foreach (var tuple in seq)
                {
                    fs.WriteLine(
                        $"{tuple.Item1.Name}|B={tuple.Item1.B:F15}|L={tuple.Item1.L:F8}|H={tuple.Item1.H:F8}|X={tuple.Item1.X:F5}|Y={tuple.Item1.Y:F5}|Z={tuple.Item1.Z:F5}|A={tuple.Item2.A}|PowE={tuple.Item2.PowE}");
                }
                fs.Close();
            }
        }

        public static void Save(this List<SC2Expression> dataList, string fileName)
        {
            using (var fs = new StreamWriter(fileName))
            {
                dataList.ForEach(item =>
                    fs.WriteLine(
                        $"{item.Name}|Bsc2={item.BSc2:F15}|Lsc2={item.LSc2:F8}|Hsc2={item.HSc2:F5}"));
                fs.Close();
            }
        }
        #endregion
    }
}