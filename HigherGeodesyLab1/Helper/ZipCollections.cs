using System;
using System.Collections.Generic;

namespace HigherGeodesyLab1.Helper
{
    public static class ZipCollections
    {
        /// <summary>
        ///   Применяет указанную функцию к соответствующим элементам двух последовательностей, создания последовательности результатов.
        /// </summary>
        /// <param name="first">
        ///   Первая последовательность для объединения.
        /// </param>
        /// <param name="second">
        ///   Вторая последовательность для объединения.
        /// </param>
        /// <param name="resultSelector">
        ///   Функция, которая указывает, как объединить элементы двух последовательностей.
        /// </param>
        /// <typeparam name="TFirst">
        ///   Тип элементов первой входной последовательностью.
        /// </typeparam>
        /// <typeparam name="TSecond">
        ///   Тип элементов второй входной последовательности.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///   Тип элементов полученной последовательности.
        /// </typeparam>
        /// <returns>
        ///   <see cref="T:System.Collections.Generic.IEnumerable`1" /> Содержащий объединенные элементы двух входных последовательностей.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   Параметр <paramref name="first" /> или <paramref name="second" /> имеет значение <see langword="null" />.
        /// </exception>
        public static IEnumerable<TResult> MyZip<TFirst, TSecond, TResult>(IEnumerable<TFirst> first,
            IEnumerable<TSecond> second,
            Func<TFirst, TSecond, TResult> resultSelector)
        {
            if (first == null)
                throw new ArgumentException(first.ToString());
            if (second == null)
                throw new ArgumentException(second.ToString());
            if (resultSelector == null)
                throw new ArgumentException(resultSelector.ToString());
            return ZipIterator(first, second, resultSelector);
        }

        /// <summary>
        /// Применяет указанную функцию к соответствующим элементам трех последовательностей, создания последовательности результатов.
        /// </summary>
        /// <param name="first">Первая последовательность для объединения.</param>
        /// <param name="second">Вторая последовательность для объединения.</param>
        /// <param name="third">Третья последовательность для объединения.</param>
        /// <param name="resultSelector">Функция, которая указывает, как объединить элементы трех последовательностей.</param>
        /// <typeparam name="TFirst">Тип элементов первой входной последовательностью.</typeparam>
        /// <typeparam name="TSecond">Тип элементов второй входной последовательности.</typeparam>
        /// <typeparam name="TThird">Тип элементов третий входной последовательности.</typeparam>
        /// <typeparam name="TResult">Тип элементов полученной последовательности.</typeparam>
        /// <returns><see cref="T:System.Collections.Generic.IEnumerable`1" /> Содержащий объединенные элементы трех входных последовательностей.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// Параметр <paramref name="first" /> или <paramref name="second" /> или <paramref name="third" />  имеет значение <see langword="null" />.
        /// </exception>
        public static IEnumerable<TResult> MyZip<TFirst, TSecond, TThird, TResult>(IEnumerable<TFirst> first,
            IEnumerable<TSecond> second, IEnumerable<TThird> third,
            Func<TFirst, TSecond, TThird, TResult> resultSelector)
        {
            if (first == null)
                throw new ArgumentException(first.ToString());
            if (second == null)
                throw new ArgumentException(second.ToString());
            if (third == null)
                throw new ArgumentException(third.ToString());
            if (resultSelector == null)
                throw new ArgumentException(resultSelector.ToString());
            return ZipIterator(first, second, third, resultSelector);
        }

        /// <summary>
        /// Применяет указанную функцию к соответствующим элементам четырех последовательностей, создания последовательности результатов.
        /// </summary>
        /// <param name="first">Первая последовательность для объединения.</param>
        /// <param name="second">Вторая последовательность для объединения.</param>
        /// <param name="third">Третья последовательность для объединения.</param>
        /// <param name="fourth">Четвертая последовательность для объединения.</param>
        /// <param name="resultSelector">Функция, которая указывает, как объединить элементы четырех последовательностей.</param>
        /// <typeparam name="TFirst">Тип элементов первой входной последовательности.</typeparam>
        /// <typeparam name="TSecond">Тип элементов второй входной последовательности.</typeparam>
        /// <typeparam name="TThird">Тип элементов третий входной последовательности.</typeparam>
        /// <typeparam name="TFourth">Тип элементов четвертой входной последовательности.</typeparam>
        /// <typeparam name="TResult">Тип элементов полученной последовательности.</typeparam>
        /// <returns><see cref="T:System.Collections.Generic.IEnumerable`1" /> Содержащий объединенные элементы четырех входных последовательностей.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// Параметр <paramref name="first" /> или <paramref name="second" /> или <paramref name="third" /> или <paramref name="fourth" /> имеет значение <see langword="null" />.
        /// </exception>
        public static IEnumerable<TResult> MyZip<TFirst, TSecond, TThird, TFourth, TResult>(
            IEnumerable<TFirst> first,
            IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth,
            Func<TFirst, TSecond, TThird, TFourth, TResult> resultSelector)
        {
            if (first == null)
                throw new ArgumentException(first.ToString());
            if (second == null)
                throw new ArgumentException(second.ToString());
            if (third == null)
                throw new ArgumentException(third.ToString());
            if (fourth == null)
                throw new ArgumentException(fourth.ToString());
            if (resultSelector == null)
                throw new ArgumentException(resultSelector.ToString());
            return ZipIterator(first, second, third, fourth, resultSelector);
        }

        private static IEnumerable<TResult> ZipIterator<TFirst, TSecond, TResult>(IEnumerable<TFirst> first,
            IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            IEnumerator<TFirst> e1 = first.GetEnumerator();
            try
            {
                IEnumerator<TSecond> e2 = second.GetEnumerator();
                try
                {
                    while (e1.MoveNext() && e2.MoveNext())
                        yield return resultSelector(e1.Current, e2.Current);
                }
                finally
                {
                    if (e2 != null)
                        e2.Dispose();
                }
            }
            finally
            {
                if (e1 != null)
                    e1.Dispose();
            }
        }

        private static IEnumerable<TResult> ZipIterator<TFirst, TSecond, TThird, TResult>(IEnumerable<TFirst> first,
            IEnumerable<TSecond> second, IEnumerable<TThird> third,
            Func<TFirst, TSecond, TThird, TResult> resultSelector)
        {
            IEnumerator<TFirst> e1 = first.GetEnumerator();
            try
            {
                IEnumerator<TSecond> e2 = second.GetEnumerator();
                try
                {
                    IEnumerator<TThird> e3 = third.GetEnumerator();
                    try
                    {
                        while (e1.MoveNext() && e2.MoveNext() && e3.MoveNext())
                            yield return resultSelector(e1.Current, e2.Current, e3.Current);
                    }
                    finally
                    {
                        if (e3 != null)
                        {
                            e3.Dispose();
                        }
                    }
                }
                finally
                {
                    if (e2 != null)
                        e2.Dispose();
                }
            }
            finally
            {
                if (e1 != null)
                    e1.Dispose();
            }
        }

        private static IEnumerable<TResult> ZipIterator<TFirst, TSecond, TThird, TFourth, TResult>(
            IEnumerable<TFirst> first,
            IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth,
            Func<TFirst, TSecond, TThird, TFourth, TResult> resultSelector)
        {
            IEnumerator<TFirst> e1 = first.GetEnumerator();
            try
            {
                IEnumerator<TSecond> e2 = second.GetEnumerator();
                try
                {
                    IEnumerator<TThird> e3 = third.GetEnumerator();
                    try
                    {
                        IEnumerator<TFourth> e4 = fourth.GetEnumerator();
                        try
                        {
                            while (e1.MoveNext() && e2.MoveNext() && e3.MoveNext() && e4.MoveNext())
                                yield return resultSelector(e1.Current, e2.Current, e3.Current, e4.Current);
                        }
                        finally
                        {
                            if (e4 != null)
                            {
                                e4.Dispose();
                            }
                        }
                    }
                    finally
                    {
                        if (e3 != null)
                        {
                            e3.Dispose();
                        }
                    }
                }
                finally
                {
                    if (e2 != null)
                        e2.Dispose();
                }
            }
            finally
            {
                if (e1 != null)
                    e1.Dispose();
            }
        }
    }
}