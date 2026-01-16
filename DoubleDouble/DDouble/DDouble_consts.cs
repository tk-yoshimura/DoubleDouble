using System.Diagnostics;

namespace DoubleDouble {
    public partial struct ddouble {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static readonly ddouble
            Pi = (+1, +1, 0xC90FDAA22168C234uL, 0xC4C6628B80DC1CD1uL),
            RcpPi = (+1, -2, 0xA2F9836E4E441529uL, 0xFC2757D1F534DDC0uL),
            E = (+1, +1, 0xADF85458A2BB4A9AuL, 0xAFDC5620273D3CF1uL),
            RcpE = (+1, -2, 0xBC5AB1B16779BE35uL, 0x75BD8F0520A9F21BuL),
            DigammaZero = (+1, 0, 0xBB16C31AB5F1FB70uL, 0xD4E4432F05278358uL),
            EulerGamma = (+1, -1, 0x93C467E37DB0C7A4uL, 0xD1BE3F810152CB56uL),
            Zeta3 = (+1, 0, 0x99DD0027803109C1uL, 0xB8B8AE2CF3483F8DuL),
            Zeta5 = (+1, 0, 0x84BA0C7653E66DBDuL, 0x118271E8CCFFA309uL),
            Zeta7 = (+1, 0, 0x811196D0A679C46DuL, 0x4DFACD442D6216FBuL),
            Zeta9 = (+1, 0, 0x8041CF9EC0B5AB81uL, 0x7FD07DD8E6B4333CuL),
            Ln2 = (+1, -1, 0xB17217F7D1CF79ABuL, 0xC9E3B39803F2F6AFuL),
            LbE = (+1, 0, 0xB8AA3B295C17F0BBuL, 0xBE87FED0691D3E88uL),
            Lg2 = (+1, -2, 0x9A209A84FBCFF798uL, 0x8F8959AC0B7C9178uL),
            Lb10 = (+1, +1, 0xD49A784BCD1B8AFEuL, 0x492BF6FF4DAFDB4CuL),
            Sqrt2 = (+1, 0, 0xB504F333F9DE6484uL, 0x597D89B3754ABE9FuL),
            Point5 = (+1, -1, 0x8000000000000000uL, 0x0000000000000000uL),
            ErdosBorwein = (+1, 0, 0xCDA82FCF21F9121BuL, 0xF9B9629409231DE0uL),
            FeigenbaumDelta = (+1, 2, 0x956A197E30E4BA3AuL, 0x3809DA8B5C4C87EDuL),
            FeigenbaumAlpha = (+1, 1, 0xA02FA4831C037B70uL, 0x65437EAC7F404AD3uL),
            LemniscatePi = (+1, 1, 0xA7CFCA7CFA858373uL, 0xCC3BB5D6046CF0BDuL),
            GlaisherA = (+1, 0, 0xA42692797EC39898uL, 0x9F6AB71A1ED3BABCuL),
            CatalanG = (+1, -1, 0xEA7CB89F409AE845uL, 0x215822E37D32D0C6uL),
            FransenRobinson = (+1, 1, 0xB3B281F50C729CB8uL, 0x7D0615AD23F4DC47uL),
            KhinchinK = (+1, 1, 0xABDE7211E3686BBCuL, 0xBF7CF8E5199FE70CuL),
            MeisselMertens = (+1, -2, 0x85E2F67259FF6A3EuL, 0x6F326DC1668A2F41uL),
            LambertOmega = (+1, -1, 0x91304D7C74B2BA5EuL, 0xAFDDAA6286DC28E1uL),
            LandauRamanujan = (+1, -1, 0xC3A4294EFECE5322uL, 0x1A3AAFEFD88366A3uL),
            MillsTheta = (+1, 0, 0xA73763F7B46E5A60uL, 0x831E98883E852FFEuL),
            SoldnerMu = (+1, 0, 0xB9C677931F512642uL, 0xC676B1774EF3FFCBuL),
            SierpinskiK = (+1, -1, 0xD2A4ACF1B1952D4CuL, 0xF9B6C08D9048AEA1uL),
            RcpFibonacci = (+1, 1, 0xD7085DE3B412547AuL, 0x6E1D157303AB3415uL),
            Niven = (+1, 0, 0xDA445BCFC382C748uL, 0xBAA1D4C214E3C4ADuL),
            GolombDickman = (+1, -1, 0x9FD41712B4E41BD4uL, 0x26B9F2F254BA047AuL),
            GoldenRatio = (+1, 0, 0xCF1BBCDCBFA53E0AuL, 0xF9CE60302E76E41AuL),
            MalardiTheta = (+1, 0, 0xF48FA1402B8AE691uL, 0x2F3DFFAF80B92CFAuL);
    }
}