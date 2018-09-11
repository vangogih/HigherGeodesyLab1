using System;
using System.Linq;

namespace HigherGeodesyLab1.Helper
{
    /// <summary>
    /// класс по работе с матрицами
    /// </summary>
    internal sealed class Matrix
    {
        private int _col;

        private int _row;

        private double[,] _matrix;

        #region Encapsulation

        /// <summary>
        /// кол-во столбцов
        /// </summary>
        public int Col
        {
            get { return _col;}
            
            set
            {
                if (value > 0) _col = value;
            }
        }

        /// <summary>
        /// кол-во строк
        /// </summary>
        public int Row
        {
            get { return _row;}
            set
            {
                if (value > 0) _row = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        ///  Контруктор определяющий матрицу размерностью [Rows, Collumns]
        /// </summary>
        /// <param name="col">Кол-во строк</param>
        /// <param name="row">Кол-во столбцов</param>
        public Matrix(int row, int col)
        {
            Col = col;
            Row = row;
            _matrix = new double[_row, _col]; //myArr[2,3] (2-строка(row) 3-столбец(col))
        }

        /// <summary>
        /// индексатор матрицы
        /// </summary>
        /// <param name="i">Кол-во строк</param>
        /// <param name="j">Кол-во столбцов</param>
        public double this[int i, int j]
        {
            get { return _matrix[i, j];}
            set { _matrix[i, j] = value;}
        }

        #endregion

        #region PrivateMethods

        /// <summary>
        /// Умножение матрицы на число
        /// </summary>
        /// <param name="a">исходная матрица</param>
        /// <param name="ch">число</param>
        /// <returns>матрица того же порядка, умноженная на заданное число</returns>
        private static Matrix Multiply(Matrix a, double ch)
        {
            Matrix resMass = new Matrix(a.Row, a.Col);
            for (int rows = 0; rows < a.Row; rows++)
            {
                for (int col = 0; col < a.Col; col++)
                {
                    resMass[rows, col] = a[rows, col] * ch;
                }
            }
            return resMass;
        }

        /// <summary>
        /// Умножение матрицы А на матрицу Б
        /// </summary>
        /// <param name="a">матрица А</param>
        /// <param name="b">матрица Б</param>
        /// <returns>результат умножения матрицы А на Б</returns>
        private static Matrix Multiply(Matrix a, Matrix b)
        {
            if (a.Col != b.Row)
                throw new Exception("Несоблюдено условие перемножения матриц");
            Matrix resMass = new Matrix(a.Col, b.Col);
            for (int bCol = 0; bCol < b.Col; bCol++)
            for (int aRow = 0; aRow < a.Row; aRow++)
            for (int bRow = 0; bRow < b.Row; bRow++)
                resMass[aRow, bCol] += a[aRow, bRow] * b[bRow, bCol];
            //ГОСПОДИ ЭТО ЕБАНАЯ МАГИЯ!!! НЕ СПРАШИВАЙТЕ МЕНЯ КАК ЭТО РАБОТАЕТ! Я CАМ НЕ ЗНАЮ.
            return resMass;
        }

        /// <summary>
        /// Вычитание матрицы А из матрицы Б
        /// </summary>
        /// <param name="a">матрица А</param>
        /// <param name="b">матрица Б</param>
        /// <returns>матрица С - результат разности матрицы А и Б</returns>
        /// <exception cref="Exception">несовпадение размерностей матриц</exception>
        private static Matrix Subtraction(Matrix a, Matrix b)
        {
            if (a.Col != b.Row)
                throw new Exception("Размерности матриц не совпадают");
            Matrix resMass = new Matrix(a.Row, a.Col);
            for (int row = 0; row < a.Row; row++)
            {
                for (int col = 0; col < a.Col; col++)
                {
                    resMass[row, col] = a[row, col] - b[row, col];
                }
            }
            return resMass;
        }

        /// <summary>
        /// Сложение матрицы А и матрицы Б
        /// </summary>
        /// <param name="a">матрица Б</param>
        /// <param name="b">матрица А</param>
        /// <returns>сумма матриц А и Б</returns>
        /// <exception cref="Exception">несовпадение размерностей матриц</exception>
        private static Matrix Sum(Matrix a, Matrix b)
        {
            if (a.Col != b.Col && a.Row != b.Row)
                throw new Exception("Размерности матриц не совпадают");
            Matrix resMass = new Matrix(a.Row, a.Col);
            for (int row = 0; row < a.Row; row++)
            {
                for (int col = 0; col < a.Col; col++)
                {
                    resMass[row, col] = a[row, col] + b[row, col];
                }
            }
            return resMass;
        }

        #endregion

        #region OverrideOpperands

        /// <summary>
        /// Перегрузка оператора вычитания
        /// </summary>
        /// <param name="a">матрица А</param>
        /// <param name="b">матрица Б</param>
        /// <returns>разность матрицы А и Б</returns>
        public static Matrix operator -(Matrix a, Matrix b)
        {
            return Subtraction(a, b);
        }

        /// <summary>
        /// Перегрузка оператора сложения
        /// </summary>
        /// <param name="a">матрица А</param>
        /// <param name="b">матрица Б</param>
        /// <returns>сумму матриц А и Б</returns>
        public static Matrix operator +(Matrix a, Matrix b)
        {
            return Sum(a, b);
        }

        /// <summary>
        /// перегрузка оператора умножения матрицы А на матрицу Б
        /// </summary>
        /// <param name="a">матрица А</param>
        /// <param name="b">матрица Б</param>
        /// <returns>произведение матриц А и Б</returns>
        public static Matrix operator *(Matrix a, Matrix b)
        {
            return Multiply(a, b);
        }

        /// <summary>
        /// перегрузка оператора умножения матрицы А на число
        /// </summary>
        /// <param name="a">матрица А</param>
        /// <param name="num">число на которое нужно умножить</param>
        /// <returns>произвдение матрицы А на число</returns>
        public static Matrix operator *(Matrix a, double num)
        {
            return Multiply(a, num);
        }

        #endregion
    }
}