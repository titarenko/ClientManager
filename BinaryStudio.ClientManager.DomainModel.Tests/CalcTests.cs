using System;
using NUnit.Framework;

namespace BinaryStudio.ClientManager.DomainModel.Tests
{
    [TestFixture]
    public class CalcTests
    {

        [Test, 
        TestCase(1, 2, 3), 
        TestCase(4.5, 5, 9.5), 
        TestCase(1.99999999999, 2, 4)]
        public void Should_ReturnZ_WhenAddingXAndY(double x, double y, double z)
        {
            var calc = new Calc();
            Assert.AreEqual(z, calc.Add(x, y), 1e-4);
        }
    }

    public class Calc
    {
        public double Add(double leftOperand, double rightOperand)
        {
            return leftOperand + rightOperand;
        }
    }
}