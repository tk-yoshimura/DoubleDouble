namespace DoubleDoubleNumTablePacking {
    public static class Program {
        static void Main() {
            const string dirpath_root = "../../../../DoubleDouble/Resource/";

            Directory.CreateDirectory(dirpath_root);

            using (BinaryWriter sw = new(File.OpenWrite(dirpath_root + nameof(BernoulliTable) + ".bin"))) {
                BernoulliTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.OpenWrite(dirpath_root + nameof(CiSiTable) + ".bin"))) {
                CiSiTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.OpenWrite(dirpath_root + nameof(EiTable) + ".bin"))) {
                EiTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.OpenWrite(dirpath_root + nameof(ErfTable) + ".bin"))) {
                ErfTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.OpenWrite(dirpath_root + nameof(ErfcTable) + ".bin"))) {
                ErfcTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.OpenWrite(dirpath_root + nameof(ErfiTable) + ".bin"))) {
                ErfiTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.OpenWrite(dirpath_root + nameof(FresnelTable) + ".bin"))) {
                FresnelTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.OpenWrite(dirpath_root + nameof(GammaTable) + ".bin"))) {
                GammaTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.OpenWrite(dirpath_root + nameof(LogGammaTable) + ".bin"))) {
                LogGammaTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.OpenWrite(dirpath_root + nameof(DigammaTable) + ".bin"))) {
                DigammaTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.OpenWrite(dirpath_root + nameof(IncompGammaTable) + ".bin"))) {
                IncompGammaTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.OpenWrite(dirpath_root + nameof(InverseErfTable) + ".bin"))) {
                InverseErfTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.OpenWrite(dirpath_root + nameof(InverseErfcTable) + ".bin"))) {
                InverseErfcTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.OpenWrite(dirpath_root + nameof(LambertWTable) + ".bin"))) {
                LambertWTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.OpenWrite(dirpath_root + nameof(RiemannZetaTable) + ".bin"))) {
                RiemannZetaTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.OpenWrite(dirpath_root + nameof(StieltjesGammaTable) + ".bin"))) {
                StieltjesGammaTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.OpenWrite(dirpath_root + nameof(GaussIntegralTable) + ".bin"))) {
                GaussIntegralTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.OpenWrite(dirpath_root + nameof(StruveIntegralTable) + ".bin"))) {
                StruveIntegralTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.OpenWrite(dirpath_root + nameof(MathieuMTable) + ".bin"))) {
                MathieuMTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.OpenWrite(dirpath_root + nameof(MathieuDTable) + ".bin"))) {
                MathieuDTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.OpenWrite(dirpath_root + nameof(MathieuATable) + ".bin"))) {
                MathieuATable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.OpenWrite(dirpath_root + nameof(MathieuBTable) + ".bin"))) {
                MathieuBTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.OpenWrite(dirpath_root + nameof(BesselKTable) + ".bin"))) {
                BesselKTable.Pack(sw);
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}