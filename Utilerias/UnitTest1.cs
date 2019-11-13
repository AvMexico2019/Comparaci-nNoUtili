using Microsoft.VisualStudio.TestTools.UnitTesting;
using ComparaciÛnNoUtili;

namespace Utilerias
{
    [TestClass]
    public class PruebaUtilerias
    {
        [TestMethod]
        public void EliminateDoubleSpace()
        {
            Assert.AreEqual(1, Program.EDS("   ").Length);
            Assert.AreEqual(1, Program.EDS("       ").Length);
            Assert.AreEqual("AVENIDA CONSTITUYENTES DE 1975", Program.EDS(" AVENIDA CONSTITUYENTES DE 1975"));
        }
        /*
        [TestMethod]
        public void HexaString()
        {
            string a1 = "abcdefghijklmnopqrstuvwxyz";
            string a2 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string a3 = "¡…Õ”⁄‹—";
            string a4 = "·ÈÌÛ˙¸Ò";
            string a5 = "0123456789|∞¨\"#$%&/()=?'\\°ø¥®*+~[{^]}`<>,;.:-_";
            string h1 = "6162636465666768696A6B6C6D6E6F707172737475767778797A";
            string h2 = "4142434445464748494A4B4C4D4E4F505152535455565758595A";
            string h3 = "C1C9CDD3DADCD1";
            string h4 = "E1E9EDF3FAFCF1";
            string h5 = "303132333435363738397CB0AC22232425262F28293D3F275CA1BFB4A82A2B7E5B7B5E5D7D603C3E2C3B2E3A2D5F";
            string r1 = Program.HEXAString(a1);
            string r2 = Program.HEXAString(a2);
            string r3 = Program.HEXAString(a3);
            string r4 = Program.HEXAString(a4);
            string r5 = Program.HEXAString(a5);
            Assert.AreEqual(h1, r1, false, "error en A1");
            Assert.AreEqual(h2, r2, false, "error en A2");
            //Assert.AreEqual(h3, r3, false, "error en A3");
            //Assert.AreEqual(h4, r4, false, "error en A4");
            Assert.AreEqual(h5, r5, false, "error en A5");

        }
        */
    }
}
 