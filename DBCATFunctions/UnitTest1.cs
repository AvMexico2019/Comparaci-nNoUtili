using Microsoft.VisualStudio.TestTools.UnitTesting;
using ComparaciónNoUtili;

namespace DBCATFunctions
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GetPais()
        {
            Assert.AreEqual("México", Program.GetPais(165));
        }
    }
}
