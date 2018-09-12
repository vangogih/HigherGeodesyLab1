using HigherGeodesyLab1.Helper;
using HigherGeodesyLab1.Transform;
using NUnit.Framework;

namespace HigherGeodesyLab1Tests
{
    [TestFixture]
    public class TestGaussProjection : GeneralFunctionality
    {
        [Test]
        public void NumberSixDegreesSoneTest()
        {
            //arrange
            double L1 = DegToRad(15, 20);
            double L2 = DegToRad(25, 45);
            double L3 = DegToRad(54, 55);

            //act
            double actual1 = new GaussProjection().NumberSixDegreesZone(L1);
            double actual2 = new GaussProjection().NumberSixDegreesZone(L2);
            double actual3 = new GaussProjection().NumberSixDegreesZone(L3);

            //assert
            Assert.AreEqual(3, actual1, $"actual={actual1}");
            Assert.AreEqual(5, actual2, $"actual={actual2}");
            Assert.AreEqual(10, actual3, $"actual={actual3}");
        }

        [Test]
        public void GetValueRangeLTest()
        {
            //arrange
            double L1 = DegToRad(15, 20);
            double L2 = DegToRad(25, 45);
            double L3 = DegToRad(54, 55);
            double delta = 0.000000000005;

            //act
            double num1 = new GaussProjection().NumberSixDegreesZone(L1);
            double num2 = new GaussProjection().NumberSixDegreesZone(L2);
            double num3 = new GaussProjection().NumberSixDegreesZone(L3);

            double actual1 = new GaussProjection().GetValueRangeL(L1, num1);
            double actual2 = new GaussProjection().GetValueRangeL(L2, num2);
            double actual3 = new GaussProjection().GetValueRangeL(L3, num3);

            double expected1 = L1 - DegToRad((num1 - 1) * 6 + 3, 0);
            double expected2 = L2 - DegToRad((num2 - 1) * 6 + 3, 0);
            double expected3 = L3 - DegToRad((num3 - 1) * 6 + 3, 0);

            //assert
            Assert.AreEqual(expected1, actual1, delta);
            Assert.AreEqual(expected2, actual2, delta);
            Assert.AreEqual(expected3, actual3, delta);
        }
    }
}