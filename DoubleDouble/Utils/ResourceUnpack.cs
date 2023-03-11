using System.Collections.ObjectModel;

namespace DoubleDouble.Utils {
    static class ResourceUnpack {
        const int MantissaBits = 116, UInt64Bits = 64, MantissaBias = 1023;
        const UInt64 SignMask = 0x8000000000000000uL;
        const UInt32 ExponentMask = 0x7FF00000u;
        const UInt64 MantissaMaskHi = 0x000FFFFFFFFFFFFFuL;
        const UInt64 MantissaMaskLo = ~MantissaMaskHi;

        public static Dictionary<string, ReadOnlyCollection<ddouble>> NumTable(byte[] resource, bool reverse = false) {
            Dictionary<string, ReadOnlyCollection<ddouble>> table = new();

            MemoryStream stream = new MemoryStream(resource);
            using (BinaryReader sr = new(stream)) {
                while (stream.Position < stream.Length) {
                    string key = sr.ReadString();
                    UInt32 n = sr.ReadUInt32();

                    List<ddouble> vals = new();

                    for (int i = 0; i < n; i++) {
                        (UInt64 hi, UInt64 lo) = (sr.ReadUInt64(), sr.ReadUInt64());

                        ddouble val = ToDDouble(hi, lo);

                        vals.Add(val);
                    }

                    UInt32 zeroset = sr.ReadUInt32();

                    if (zeroset != 0u) {
                        throw new IOException("The format of resource file is invalid.");
                    }

                    if (reverse) {
                        vals.Reverse();
                    }

                    table.Add(key, Array.AsReadOnly(vals.ToArray()));
                }
            }

            return table;
        }

        public static Dictionary<string, ReadOnlyCollection<(ddouble, ddouble)>> NumTableX2(byte[] resource, bool reverse = false) {
            Dictionary<string, ReadOnlyCollection<(ddouble, ddouble)>> table = new();

            MemoryStream stream = new MemoryStream(resource);
            using (BinaryReader sr = new(stream)) {
                while (stream.Position < stream.Length) {
                    string key = sr.ReadString();
                    UInt32 n = sr.ReadUInt32();

                    List<(ddouble, ddouble)> vals = new();

                    for (int i = 0; i < n; i++) {
                        (UInt64 hi0, UInt64 lo0) = (sr.ReadUInt64(), sr.ReadUInt64());
                        (UInt64 hi1, UInt64 lo1) = (sr.ReadUInt64(), sr.ReadUInt64());

                        ddouble val0 = ToDDouble(hi0, lo0);
                        ddouble val1 = ToDDouble(hi1, lo1);

                        vals.Add((val0, val1));
                    }

                    UInt32 zeroset = sr.ReadUInt32();

                    if (zeroset != 0u) {
                        throw new IOException("The format of resource file is invalid.");
                    }

                    if (reverse) {
                        vals.Reverse();
                    }

                    table.Add(key, Array.AsReadOnly(vals.ToArray()));
                }
            }

            return table;
        }

        public static Dictionary<string, ReadOnlyCollection<(ddouble, ddouble, ddouble, ddouble)>> NumTableX4(byte[] resource, bool reverse = false) {
            Dictionary<string, ReadOnlyCollection<(ddouble, ddouble, ddouble, ddouble)>> table = new();

            MemoryStream stream = new MemoryStream(resource);
            using (BinaryReader sr = new(stream)) {
                while (stream.Position < stream.Length) {
                    string key = sr.ReadString();
                    UInt32 n = sr.ReadUInt32();

                    List<(ddouble, ddouble, ddouble, ddouble)> vals = new();

                    for (int i = 0; i < n; i++) {
                        (UInt64 hi0, UInt64 lo0) = (sr.ReadUInt64(), sr.ReadUInt64());
                        (UInt64 hi1, UInt64 lo1) = (sr.ReadUInt64(), sr.ReadUInt64());
                        (UInt64 hi2, UInt64 lo2) = (sr.ReadUInt64(), sr.ReadUInt64());
                        (UInt64 hi3, UInt64 lo3) = (sr.ReadUInt64(), sr.ReadUInt64());

                        ddouble val0 = ToDDouble(hi0, lo0);
                        ddouble val1 = ToDDouble(hi1, lo1);
                        ddouble val2 = ToDDouble(hi2, lo2);
                        ddouble val3 = ToDDouble(hi3, lo3);

                        vals.Add((val0, val1, val2, val3));
                    }

                    UInt32 zeroset = sr.ReadUInt32();

                    if (zeroset != 0u) {
                        throw new IOException("The format of resource file is invalid.");
                    }

                    if (reverse) {
                        vals.Reverse();
                    }

                    table.Add(key, Array.AsReadOnly(vals.ToArray()));
                }
            }

            return table;
        }

        private static ddouble ToDDouble(UInt64 hi, UInt64 lo) {
            if (hi == 0uL && lo == 0uL) {
                return ddouble.Zero;
            }

            int sign = (hi < SignMask) ? 1 : -1;
            int exponent = (int)unchecked(((UIntUtil.Unpack(hi).high & ExponentMask) >> 20)) - MantissaBias;

            UInt64 hi64 = unchecked(
                ((hi & MantissaMaskHi) << (2 * UInt64Bits - MantissaBits)) |
                ((lo & MantissaMaskLo) >> (MantissaBits - UInt64Bits))
            );

            UInt64 lo64 = unchecked(((lo & MantissaMaskHi) << (2 * UInt64Bits - MantissaBits)));

            return (sign, exponent, hi64, lo64);
        }
    }
}
