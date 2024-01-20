using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public partial class JsonTests {
        [TestMethod]
        public void JsonTest() {
            ddouble pi = ddouble.PI;

            string str = JsonSerializer.Serialize<ddouble>(pi);

            ddouble pi2 = JsonSerializer.Deserialize<ddouble>(str);

            Assert.AreEqual(pi.ToString(), pi2.ToString());
        }
    }
}
