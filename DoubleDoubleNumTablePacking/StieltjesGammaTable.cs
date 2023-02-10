using DoubleDoubleHexcode;
using System.Collections.ObjectModel;

namespace DoubleDoubleNumTablePacking {
    public static class StieltjesGammaTable {
        public static void Pack(BinaryWriter stream) {
            stream.Write(nameof(StieltjesGammaTable));
            stream.Write((UInt32)table.Count);
            foreach (Hexcode v in table) {
                stream.Write((UInt64)v.Hi);
                stream.Write((UInt64)v.Lo);
            }
            stream.Write((UInt32)0u);
        }

        static ReadOnlyCollection<Hexcode> table = Array.AsReadOnly(new Hexcode[] {
            (+1, -1, 0x93C467E37DB0C7A4uL, 0xD1BE3F810152CB56uL),
            (-1, -4, 0x95207957DD250E5BuL, 0x3A35A1180FCAF769uL),
            (-1, -7, 0x9EC4543FFBE22C6DuL, 0xA87D05E086D50993uL),
            (+1, -9, 0x86999FAAA66D8DC0uL, 0x25FB45527AF3AC23uL),
            (+1, -9, 0x98653C61DD109D65uL, 0xEE115A8D9691365BuL),
            (+1, -11, 0xCFF70F66DB4D25C9uL, 0x78089F5BAECE2E19uL),
            (-1, -13, 0xFA5E287A82F1B76AuL, 0x9653FAD711A228A3uL),
            (-1, -11, 0x8A39CDC8BD68D14AuL, 0x047840C338214A33uL),
            (-1, -12, 0xB89D324F57E8B639uL, 0x0E04A6FC7E6EC6A0uL),
            (-1, -15, 0x90431B9A132CA60CuL, 0x190D746FC52D216BuL),
            (+1, -13, 0xD74E9B98E7663441uL, 0x4BD845146DF3293BuL),
            (+1, -12, 0x8DA78AA7619D7813uL, 0xBAC5E22AA6B2669AuL),
            (+1, -13, 0xAF65FAFDF1E31946uL, 0x594CC67B9065D4DEuL),
            (-1, -16, 0xE662135A16F4FAAAuL, 0x482EE8056970AD31uL),
            (-1, -13, 0xDB5F2F045CB83D20uL, 0x43722BCCB44DFECAuL),
            (-1, -12, 0x949E84D513BA3BB8uL, 0xD8D4A3A099164FBDuL),
            (-1, -13, 0xD165B79BDB55C390uL, 0xCB652873170BE799uL),
            (+1, -16, 0xDC6D81EC45080545uL, 0x0E796EF68F7B0D91uL),
            (+1, -12, 0xA1264A162B3801D5uL, 0x11E614DD7DC1981BuL),
            (+1, -11, 0x840463CDBF748534uL, 0xE14AEF106F9A750EuL),
            (+1, -12, 0xF47F92C35D0B682CuL, 0xD988BB8934CDD555uL),
            (+1, -14, 0xDB0599C7390E38FCuL, 0x7E3D595AB9F3CFB0uL),
            (-1, -11, 0x8DFA21F8ED7C2EF4uL, 0x6B839B73A65D26C8uL),
            (-1, -10, 0xA30C70FDD5332A95uL, 0x0D3EF47AE649CFC4uL),
            (-1, -10, 0xD03597FC03FD1DD9uL, 0xF0A1DE4B895317CBuL),
            (-1, -10, 0x8CD952964D0C8A9BuL, 0x2B6F6ABB42F5F8DCuL),
            (+1, -11, 0xAC2D56879F7AD62EuL, 0x11D0B674C3C1FD8DuL),
            (+1, -9, 0xE3EC6BCE083CDF7DuL, 0xFF0D42D6E7CD463CuL),
            (+1, -8, 0xD1B7AA849584BD04uL, 0x97F75E360A8A8A1EuL),
            (+1, -8, 0xF189B3E4DE312F28uL, 0x3A57B0E130AE50C4uL),
            (+1, -9, 0xE928C9152D48C7FEuL, 0x18784BCC464BB3DCuL),
            (-1, -8, 0xF63258B8EF4DCCEFuL, 0xF74669C5F1B18305uL),
            (-1, -6, 0xD290A07CFD3495F6uL, 0x7A1E8205555B7A6EuL),
        });
    }
}
