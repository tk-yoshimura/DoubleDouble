namespace DoubleDoubleNumTablePacking {
    public static class Program {
        static void Main() {
            const string dirpath_root = "../../../../DoubleDouble/Resource/";

            Directory.CreateDirectory(dirpath_root);

            using (BinaryWriter sw = new(File.Open(dirpath_root + nameof(BarnesGTable) + ".bin", FileMode.Create))) {
                BarnesGTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.Open(dirpath_root + nameof(BernoulliTable) + ".bin", FileMode.Create))) {
                BernoulliTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.Open(dirpath_root + nameof(CiSiTable) + ".bin", FileMode.Create))) {
                CiSiTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.Open(dirpath_root + nameof(ClausenTable) + "_NearZero.bin", FileMode.Create))) {
                ClausenTable.PackNearZero(sw);
            }
            using (BinaryWriter sw = new(File.Open(dirpath_root + nameof(ClausenTable) + "_Pade.bin", FileMode.Create))) {
                ClausenTable.PackPade(sw);
            }

            using (BinaryWriter sw = new(File.Open(dirpath_root + nameof(EiTable) + ".bin", FileMode.Create))) {
                EiTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.Open(dirpath_root + nameof(ErfTable) + ".bin", FileMode.Create))) {
                ErfTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.Open(dirpath_root + nameof(ErfcTable) + ".bin", FileMode.Create))) {
                ErfcTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.Open(dirpath_root + nameof(ErfiTable) + ".bin", FileMode.Create))) {
                ErfiTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.Open(dirpath_root + nameof(EulerQTable) + ".bin", FileMode.Create))) {
                EulerQTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.Open(dirpath_root + nameof(FresnelTable) + ".bin", FileMode.Create))) {
                FresnelTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.Open(dirpath_root + nameof(GammaTable) + ".bin", FileMode.Create))) {
                GammaTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.Open(dirpath_root + nameof(LogGammaTable) + ".bin", FileMode.Create))) {
                LogGammaTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.Open(dirpath_root + nameof(InverseGammaTable) + ".bin", FileMode.Create))) {
                InverseGammaTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.Open(dirpath_root + nameof(DigammaTable) + ".bin", FileMode.Create))) {
                DigammaTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.Open(dirpath_root + nameof(IncompGammaTable) + ".bin", FileMode.Create))) {
                IncompGammaTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.Open(dirpath_root + nameof(InverseErfTable) + ".bin", FileMode.Create))) {
                InverseErfTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.Open(dirpath_root + nameof(InverseErfcTable) + ".bin", FileMode.Create))) {
                InverseErfcTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.Open(dirpath_root + nameof(LambertWTable) + ".bin", FileMode.Create))) {
                LambertWTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.Open(dirpath_root + nameof(RiemannZetaTable) + ".bin", FileMode.Create))) {
                RiemannZetaTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.Open(dirpath_root + nameof(StieltjesGammaTable) + ".bin", FileMode.Create))) {
                StieltjesGammaTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.Open(dirpath_root + nameof(GaussIntegralTable) + ".bin", FileMode.Create))) {
                GaussIntegralTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.Open(dirpath_root + nameof(StruveIntegralTable) + ".bin", FileMode.Create))) {
                StruveIntegralTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.Open(dirpath_root + nameof(MathieuMTable) + ".bin", FileMode.Create))) {
                MathieuMTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.Open(dirpath_root + nameof(MathieuDTable) + ".bin", FileMode.Create))) {
                MathieuDTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.Open(dirpath_root + nameof(MathieuATable) + ".bin", FileMode.Create))) {
                MathieuATable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.Open(dirpath_root + nameof(MathieuBTable) + ".bin", FileMode.Create))) {
                MathieuBTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.Open(dirpath_root + nameof(MathieuNZTable) + ".bin", FileMode.Create))) {
                MathieuNZTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.Open(dirpath_root + nameof(BesselKTable) + ".bin", FileMode.Create))) {
                BesselKTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.Open(dirpath_root + nameof(ScorerTable) + ".bin", FileMode.Create))) {
                ScorerTable.Pack(sw);
            }

            using (BinaryWriter sw = new(File.Open(dirpath_root + nameof(TiTable) + ".bin", FileMode.Create))) {
                TiTable.Pack(sw);
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}