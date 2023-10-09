using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public class SequenceTests {
        [TestMethod]
        public void TaylorTest() {
            for (int n = 0; n < ddouble.TaylorSequence.Count; n++) {
                Console.WriteLine($"1/{n}! = {ddouble.TaylorSequence[n]}");
            }

            Assert.AreEqual(1, ddouble.TaylorSequence[0]);
            Assert.AreEqual(1, ddouble.TaylorSequence[1]);
            Assert.AreEqual(ddouble.Rcp(2), ddouble.TaylorSequence[2]);
            Assert.AreEqual(ddouble.Rcp(6), ddouble.TaylorSequence[3]);
            Assert.AreEqual(ddouble.Rcp(24), ddouble.TaylorSequence[4]);
            Assert.AreEqual(ddouble.Rcp(120), ddouble.TaylorSequence[5]);
            Assert.AreEqual(ddouble.Rcp(720), ddouble.TaylorSequence[6]);
        }

        [TestMethod]
        public void BernoulliTest() {
            for (int n = 0; n <= 32; n += 4) {
                Console.WriteLine($"B({2 * n}) = {ddouble.BernoulliSequence[n]}");
            }

            for (int n = 0; n <= 32; n++) {
                Console.WriteLine($"B({2 * n}) = {ddouble.BernoulliSequence[n]}");
            }

            Assert.AreEqual(1, ddouble.BernoulliSequence[0]);
            HPAssert.NeighborBits(ddouble.Rcp(6), ddouble.BernoulliSequence[1]);
            HPAssert.NeighborBits((ddouble)(-1) / 30, ddouble.BernoulliSequence[2]);
            HPAssert.NeighborBits((ddouble)(1) / 42, ddouble.BernoulliSequence[3]);
            HPAssert.NeighborBits((ddouble)(-1) / 30, ddouble.BernoulliSequence[4]);
            HPAssert.NeighborBits((ddouble)(5) / 66, ddouble.BernoulliSequence[5]);
            HPAssert.NeighborBits((ddouble)(-691) / 2730, ddouble.BernoulliSequence[6]);
            HPAssert.NeighborBits((ddouble)(7) / 6, ddouble.BernoulliSequence[7]);
        }

        [TestMethod]
        public void HarmonicTest() {
            for (int n = 0; n <= 1024; n += 4) {
                Console.WriteLine($"H({n}) = {ddouble.HarmonicNumber(n)}");
            }

            for (int n = 0; n <= 1024; n++) {
                Console.WriteLine($"H({n}) = {ddouble.HarmonicNumber(n)}");
            }

            Assert.AreEqual(0, ddouble.HarmonicNumber(0));
            Assert.AreEqual(1, ddouble.HarmonicNumber(1));
            HPAssert.NeighborBits((ddouble)(3) / 2, ddouble.HarmonicNumber(2));
            HPAssert.NeighborBits((ddouble)(11) / 6, ddouble.HarmonicNumber(3));
            HPAssert.NeighborBits((ddouble)(25) / 12, ddouble.HarmonicNumber(4));
        }

        [TestMethod]
        public void StieltjesGammaTest() {
            ddouble[] gs = {
                ddouble.EulerGamma,
                "-0.07281584548367672486058637587490131913773633833433795259900655974",
                "-0.009690363192872318484530386035212529359065806101340749880701365452",
                "0.002053834420303345866160046542753384285715804445410618245481483337",
                "0.002325370065467300057468170177526068000904469413784850990758040907",
                "0.0007933238173010627017533348774444448307315394045848870757342562698",
                "-0.0002387693454301996098724218419080042777837151563580786314764253074",
                "-0.0005272895670577510460740975054788582819962534729698953310134042269",
                "-0.0003521233538030395096020521650012087417291805337923503566573315074",
                "-0.00003439477441808804817791462379822739062078953859444162975929190484",
                "0.0002053328149090647946837222892370653029598537741667643038402087144",
                "0.0002701844395439035266729020820679556738278420586884025039737358031",
                "0.0001672729121051401933535015433411834466078066328055658280477909377",
                "-0.00002746380660376015886000760369335518152678533767039553609283308917",
                "-0.0002092092620592999458371396973445849578315442115060695624342083257",
                "-0.0002834686553202414466429344749971269770687029807176752539699432930",
                "-0.0001996968583089697747077845632032403919157649740340612798596671626",
                "0.00002627703710991833669946659763051012281607869292911406079711751835",
                "0.0003073684081492528265927547519486256455238112907314616910811036523",
                "0.0005036054530473556290555964377171600353212698076494978373237909270",
                "0.0004663435615115594494005948244335505251131434739256889976707266281",
                "0.0001044377697560001158107956743677204910444282507055467478343714867",
                "-0.0005415995822039977016551961731741055845438609287007488018391913164",
                "-0.001243962090408245779299741599537165809147028113964637716532971108",
                "-0.001588511278903561561906196611521115857318722822144129067478194125",
                "-0.001074591952738488824724291987353173089273979331453170361409902582",
                "0.0006568035186371544315047730033562152488860650604775373760992825251",
                "0.003477836913618538209007359574258811547662915663885919292269369089",
                "0.006400068531700629458107228221945863666637198144588475220590330626",
                "0.007371151770472239134412402423559402157841327488512840153180311593",
                "0.003557728855573160947913537748908402610809650649522125076131381743",
                "-0.007513325997815228933135160081576145616636587418058437829820653898",
                "-0.02570372910842040179348788378034991655408420213709272996040553488"
            };

            for (int n = 0; n <= 32; n++) {
                Console.WriteLine($"Gs({n}) = {ddouble.StieltjesGamma[n]}");

                HPAssert.AreEqual(gs[n], ddouble.StieltjesGamma[n], ddouble.Abs(gs[n]) * 1e-31);
            }
        }
    }
}
