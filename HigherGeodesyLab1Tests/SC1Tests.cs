using HigherGeodesyLab1.Helper;
using HigherGeodesyLab1.Transform;
using NUnit.Framework;

namespace HigherGeodesyLab1Tests
{
    [TestFixture]
    public class SC1Tests: GeneralFunctionality
    {
        [Test]
        public void GetValueN_AandBandPowE_returnN()
        {
            //arrange
            double a = 6378382.46;
            double powe = 0.006398838513067;
            double grad =51;
            double min =31.84;
            double delta = 0.0000001;
            double expected =6390928.884511180;
            //act
            double actual = SC1.GetValueN(a, powe, DegToRad(grad,min));
            //assert
            Assert.AreEqual(expected,actual,delta);
        }

        [Test]
        public void GetValueX_NandHandBandL_returnX()
        {
            //arrange
            double N = 6390928.884511180;
            double B = DegToRad(51,31.84);
            double L = DegToRad(35,21.51);
            double H = 15;
            double expected =3242434.880305990000;
            double delta = 0.0000001;
            //act
            double actual = SC1.GetValueX(N, H, B, L);
            //assert
            Assert.AreEqual(expected,actual,delta);
        }

        [Test]
        public void GetValueY_NandHandBandL_returnY()
        {
            //arrange
            double N = 6390928.884511180;
            double B = DegToRad(51,31.84);
            double L = DegToRad(35,21.51);
            double H = 15;
            double expected =2300745.7065465100;
            double delta = 0.0000001;
            //act
            double actual = SC1.GetValueY(N, H, B, L);
            //assert
            Assert.AreEqual(expected,actual,delta);
        }

        [Test]
        public void GetValueZ_PoweandNandHandB_returnY()
        {
            //arrange
            double N = 6390928.884511180;
            double B = DegToRad(51,31.84);
            double H = 15;
            double powe = 0.006398838513067;
            double expected =4971715.4961667200;
            double delta = 0.0000001;
            //act
            double actual = SC1.GetValueZ(powe, N, H, B);
            //assert
            Assert.AreEqual(expected,actual,delta);
        }
    }
}