namespace DoubleDouble {

    public static class DoubleDoubleIOExpand {

        public static void Write(this BinaryWriter writer, ddouble n) {
            writer.Write(n.Hi);
            writer.Write(n.Lo);
        }

        public static ddouble ReadDDouble(this BinaryReader reader) {
            double hi = reader.ReadDouble();
            double lo = reader.ReadDouble();

            return new ddouble((hi, lo));
        }
    }
}
