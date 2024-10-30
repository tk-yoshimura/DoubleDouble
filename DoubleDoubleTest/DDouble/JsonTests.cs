using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public partial class JsonTests {
        [TestMethod]
        public void JsonTest() {
            ddouble pi = ddouble.Pi;

            string str = JsonSerializer.Serialize<ddouble>(pi);

            ddouble pi2 = JsonSerializer.Deserialize<ddouble>(str);

            Assert.AreEqual(pi.ToString(), pi2.ToString());
        }
    }
}
