using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace DoubleDoubleTest {
    [TestClass]
    public partial class DDoubleIOTests {
        [TestMethod]
        public void IOTest() {
            const string filename_bin = "dd_iotest.bin";

            ddouble[] vs = {
                ddouble.Zero,
                ddouble.MinusZero,
                1, 2, 3, 4, 5, 7, 10, 11, 13, 100, 1000,
                -1, -2, -3, -4, -5, -7, -10, -11, -13, -100, -1000,
                (ddouble)(1d) / 2,
                (ddouble)(1d) / 3,
                (ddouble)(1d) / 4,
                (ddouble)(1d) / 5,
                (ddouble)(1d) / 7,
                (ddouble)(1d) / 10,
                (ddouble)(1d) / 11,
                (ddouble)(1d) / 13,
                (ddouble)(1d) / 100,
                (ddouble)(1d) / 1000,
                (ddouble)(-1d) / 2,
                (ddouble)(-1d) / 3,
                (ddouble)(-1d) / 4,
                (ddouble)(-1d) / 5,
                (ddouble)(-1d) / 7,
                (ddouble)(-1d) / 10,
                (ddouble)(-1d) / 11,
                (ddouble)(-1d) / 13,
                (ddouble)(-1d) / 100,
                (ddouble)(-1d) / 1000,
                ddouble.PositiveInfinity,
                ddouble.NegativeInfinity,
                ddouble.NaN,
            };

            List<ddouble> us = new List<ddouble>();

            using (BinaryWriter stream = new BinaryWriter(File.OpenWrite(filename_bin))) {
                foreach (ddouble v in vs) {
                    stream.Write(v);
                }
            }

            using (BinaryReader stream = new BinaryReader(File.OpenRead(filename_bin))) {
                for (int i = 0; i < vs.Length; i++) {
                    ddouble u = stream.ReadDDouble();

                    us.Add(u);
                }
            }

            for (int i = 0; i < vs.Length; i++) {
                if (!ddouble.IsNaN(vs[i]) && !ddouble.IsNaN(us[i])) {
                    Assert.AreEqual(vs[i], us[i]);
                }
                else {
                    Assert.IsTrue(ddouble.IsNaN(vs[i]) && ddouble.IsNaN(us[i]));
                }
            }

            File.Delete(filename_bin);
        }
    }
}
