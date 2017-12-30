using HigherGeodesyLab1.Helper;
using NUnit.Framework;

namespace HigherGeodesyLab1Tests
{
    [TestFixture]
    public class GeneralFunctionalityTests : GeneralFunctionality
    {
        private const double delta = 0.000000000000001;
        [Test]
        public void TryParse_6378382com46_return654883dot25()
        {
            //arrange
            string a = "6378382,46";
            double expected = 6378382.46;

            //act
            double actual = TryParse(a);

            //assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SecToRad_1dot8_return0dot00000872664626()
        {
            //arrange
            double a = 1.8;
            double expected = 0.00000872664626;

            //act
            double actual = SecToRad(a);

            //assert
            Assert.AreEqual(expected, actual,delta);
        }

        [Test]
        public void DegToRad_51and31dot84_return0dot899379799()
        {
            //arrange
            double grad = 51;
            double min = 31.84;
            double expected = 0.8993797990810250;

            //act
            double actual = DegToRad(grad,min);

            //assert
            Assert.AreEqual(expected, actual,delta);
        }

        [Test]
        public void Iteration_XandYandZandPowEandA_returnB()
        {
            //arrange
            double X= 3242434.880305990000;
            double Y= 2300745.7065465100;
            double Z= 4971715.4961667200;
            double powe= 0.006398838513067;
            double A= 6378382.46;
            double expected = 0.8993797990810250;

            //act
            double actual = Iteration(X,Y,Z,powe,A);

            //assert
            Assert.AreEqual(expected, actual,delta);
        }

        [Test]
        public void GetValueD_XandY_returnD()
        {
            //arrange
            double X= 3242434.880305990000;
            double Y= 2300745.7065465100;
            double expected = 3975778.50982888;
            double delta = 0.00000001;

            //act
            double actual = GetValueD(X,Y);

            //assert
            Assert.AreEqual(expected, actual,delta);
        }

        [Test]
        public void GetValueL_XandD_returnL()
        {
            //arrange
            double Y= 2300745.7065465100;
            double D= 3975778.50982888;
            double expected = 0.6171222435664150;
//            double delta = 0.00000001;

            //act
            double actual = GetValueL(Y,D);

            //assert
            Assert.AreEqual(expected, actual,delta);
        }

        [Test]
        public void GetValueH_DandBandZandAandPowe_returnH()
        {
            //arrange
            double D = 3975778.50982888;
            double B = DegToRad(51,31.84);
            double powe = 0.006398838513067;
            double Z = 4971715.4961667200;
            double A = 6378382.46;
            double expected =15;
            double delta = 0.0000001;

            //act
            double actual = GetValueH(D, B, Z, A, powe);
            //assert
            Assert.AreEqual(expected,actual,delta);
        }

    }
}