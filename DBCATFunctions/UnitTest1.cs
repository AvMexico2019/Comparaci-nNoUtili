using Microsoft.VisualStudio.TestTools.UnitTesting;
using Comparaci�nNoUtili;

namespace DBCATFunctions
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GetPais()
        {
            Assert.AreEqual("M�xico", Program.GetPais(165));
        }
    }
}
