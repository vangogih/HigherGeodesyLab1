using System;
using HigherGeodesyLab1.Transform;
using NUnit.Framework;

namespace HigherGeodesyLab1Tests
{
    [TestFixture]
    public class EllipsoidTests
    {
        [Test]
        public void GetValuepowE_6379222and6358192_return0dot006582396()
        {
            //arrange
            double a = 6378382.46;
            double b = 6357942.59;
            double expected = 0.006398838513067;
            double delta = 0.0000000000000005;

            //act
            double actual = Ellipsoid.GetValuePowE(a, b);

            //assert
            Assert.AreEqual(expected,actual,delta);
        }

        [Test]
        public void GetValueAlf__6379222and6358192_return0dot003204554()
        {
            //arrange
            double a = 6378382.46;
            double b = 6357942.59;
            double expected = 0.003204553839188;
            double delta = 0.000000000000001;

            //act
            double actual = Ellipsoid.GetValueAlf(a, b);

            //assert
            Assert.AreEqual(expected,actual,delta);
        }

        [Test]
        public void GetValuePowEAlf__6379222and6358192_return0dot006582396()
        {
            //arrange
            double alf = 0.003204553839188;
            double expected = 0.006398838513067;
            double delta = 0.000000000000001;

            //act
            double actual = Ellipsoid.GetValuePowE(alf);

            //assert
            Assert.AreEqual(expected,actual,delta);
        }



    }
}