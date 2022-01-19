using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public class ErrorFunctionTests {
        [TestMethod]
        public void ErfTest() {
            foreach ((ddouble x, ddouble expected) in new (ddouble, ddouble)[] {
                (0.125d, "1.4031620480133381739302944652162339818698e-1"),
                (0.250d, "2.7632639016823693298506826776481571206535e-1"),
                (0.375d, "4.0411690943482229832382508591912176753043e-1"),
                (0.500d, "5.2049987781304653768274665389196452873645e-1"),
                (0.625d, "6.2324088218841797244864050587679027470027e-1"),
                (0.750d, "7.1115563365351513159893783459141077737421e-1"),
                (0.875d, "7.8407506105985965831453571789884936277597e-1"),
                (1.000d, "8.4270079294971486934122063508260925929607e-1"),
                (1.125d, "8.8838823170170776406957844692716347678384e-1"),
                (1.250d, "9.2290012825645823013652348119728114042360e-1"),
                (1.375d, "9.4817007278209032256392949870347440467647e-1"),
                (1.500d, "9.6610514647531072706697626164594785868141e-1"),
                (1.625d, "9.7844373323998366472018392963442100150619e-1"),
                (1.750d, "9.8667167121918244377221110012868797660730e-1"),
                (1.875d, "9.9199005767011997029646305969122440247114e-1"),
                (2.000d, "9.9532226501895273416206925636725292861089e-1"),
                (2.125d, "9.9734597064051765846932681181490670098944e-1"),
                (2.250d, "9.9853728341331884830208920362701704608848e-1"),
                (2.375d, "9.9921706178210888080244889764152161139388e-1"),
                (2.500d, "9.9959304798255504106043578426002508727965e-1"),
                (2.625d, "9.9979462426385878255951809708384422280449e-1"),
                (2.750d, "9.9989937807788036316309560802491304323494e-1"),
                (2.875d, "9.9995214516025622658700266030314443207505e-1"),
                (3.000d, "9.9997790950300141455862722387041767962015e-1"),
                (3.125d, "9.9999010326537475438053136662214623049528e-1"),
                (3.250d, "9.9999569722053632487816952410486976340243e-1"),
                (3.375d, "9.9999818471857255964423817783465025044307e-1"),
                (3.500d, "9.9999925690162765858725447631624390436428e-1"),
                (3.625d, "9.9999970485980748843014140727957140026145e-1"),
                (3.750d, "9.9999988627274343020334674092341699339114e-1"),
                (3.875d, "9.9999995748605591750887797100076200060312e-1"),
                (4.000d, "9.9999998458274209971998114784032651311595e-1"),
                (4.125d, "9.9999999457659920043493399468776591424469e-1"),
                (4.250d, "9.9999999814942586261325747994416192847127e-1"),
                (4.375d, "9.9999999938751670464306398419136118112067e-1"),
                (4.500d, "9.9999999980338395584571125237208396323357e-1"),
                (4.625d, "9.9999999993878389486965773746246111511346e-1"),
                (4.750d, "9.9999999998151495227851468911257058389902e-1"),
                (4.875d, "9.9999999999458659353370205852243976157834e-1"),
                (5.000d, "9.9999999999846254020557196514981165651462e-1"),
                (5.125d, "9.9999999999957654366236637763259198058357e-1"),
                (5.250d, "9.9999999999988689686733112846117213195201e-1"),
                (5.375d, "9.9999999999997070511445512845651133139358e-1"),
                (5.500d, "9.9999999999999264215208202560193693163760e-1"),
                (5.625d, "9.9999999999999820797999434899358459381754e-1"),
                (5.750d, "9.9999999999999957678633825742623740532801e-1"),
                (5.875d, "9.9999999999999990308444354722824831199546e-1"),
                (6.000d, "9.9999999999999997848026328750108688340665e-1"),
                (6.125d, "9.9999999999999999536677784470073422299867e-1"),
                (6.250d, "9.9999999999999999903277958681237460085180e-1"),
                (6.375d, "9.9999999999999999980422476957193682451707e-1"),
                (6.500d, "9.9999999999999999996157851672879352530124e-1"),
                (6.625d, "9.9999999999999999999268913031446967710921e-1"),
                (6.750d, "9.9999999999999999999865123211063886994878e-1"),
                (6.875d, "9.9999999999999999999975874648656241776567e-1"),
                (7.000d, "9.9999999999999999999995816174392220585601e-1"),
                (7.125d, "9.9999999999999999999999296551225254003788e-1"),
                (7.250d, "9.9999999999999999999999885330991851849884e-1"),
                (7.375d, "9.9999999999999999999999981877829475603796e-1"),
                (7.500d, "9.9999999999999999999999997223350613969431e-1"),
                (7.625d, "9.9999999999999999999999999587546657979082e-1"),
                (7.750d, "9.9999999999999999999999999940602521404829e-1"),
                (7.875d, "9.9999999999999999999999999991707276217070e-1"),
                (8.000d, "9.9999999999999999999999999998877570282702e-1"),
                (8.125d, "9.9999999999999999999999999999981264335294e-1"),
            }) {
                ddouble x_dec = ddouble.BitDecrement(x);
                ddouble x_inc = ddouble.BitIncrement(x);

                Console.WriteLine(ddouble.Erf(x_dec));
                Console.WriteLine(ddouble.Erf(x));
                Console.WriteLine(ddouble.Erf(x_inc));

                Assert.IsTrue(ddouble.Abs(expected / ddouble.Erf(x_dec) - 1) < 1e-29);
                Assert.IsTrue(ddouble.Abs(expected / ddouble.Erf(x) - 1) < 1e-29);
                Assert.IsTrue(ddouble.Abs(expected / ddouble.Erf(x_inc) - 1) < 1e-29);

                Assert.IsTrue(ddouble.Abs(-expected / ddouble.Erf(-x_dec) - 1) < 1e-29);
                Assert.IsTrue(ddouble.Abs(-expected / ddouble.Erf(-x) - 1) < 1e-29);
                Assert.IsTrue(ddouble.Abs(-expected / ddouble.Erf(-x_inc) - 1) < 1e-29);

                HPAssert.NeighborBits(expected, ddouble.Erf(x_dec), 32);
                HPAssert.NeighborBits(expected, ddouble.Erf(x), 32);
                HPAssert.NeighborBits(expected, ddouble.Erf(x_inc), 32);
            }

            ddouble erf_pzero = ddouble.Erf(0d);
            ddouble erf_mzero = ddouble.Erf(-0d);
            ddouble erf_pinf = ddouble.Erf(double.PositiveInfinity);
            ddouble erf_ninf = ddouble.Erf(double.NegativeInfinity);
            ddouble erf_nan = ddouble.Erf(double.NaN);

            Assert.IsTrue(ddouble.IsPlusZero(erf_pzero), nameof(erf_pzero));
            Assert.IsTrue(ddouble.IsMinusZero(erf_mzero), nameof(erf_mzero));
            Assert.IsTrue(erf_pinf == 1, nameof(erf_pinf));
            Assert.IsTrue(erf_ninf == -1, nameof(erf_ninf));
            Assert.IsTrue(ddouble.IsNaN(erf_nan), nameof(erf_nan));
        }

        [TestMethod]
        public void ErfcTest() {
            foreach ((ddouble x, ddouble expected) in new (ddouble, ddouble)[] {
                (0.125d, "8.5968379519866618260697055347837660181302e-1"),
                (0.250d, "7.2367360983176306701493173223518428793465e-1"),
                (0.375d, "5.9588309056517770167617491408087823246957e-1"),
                (0.500d, "4.7950012218695346231725334610803547126355e-1"),
                (0.625d, "3.7675911781158202755135949412320972529973e-1"),
                (0.750d, "2.8884436634648486840106216540858922262579e-1"),
                (0.875d, "2.1592493894014034168546428210115063722403e-1"),
                (1.000d, "1.5729920705028513065877936491739074070393e-1"),
                (1.125d, "1.1161176829829223593042155307283652321616e-1"),
                (1.250d, "7.7099871743541769863476518802718859576399e-2"),
                (1.375d, "5.1829927217909677436070501296525595323527e-2"),
                (1.500d, "3.3894853524689272933023738354052141318590e-2"),
                (1.625d, "2.1556266760016335279816070365578998493810e-2"),
                (1.750d, "1.3328328780817556227788899871312023392698e-2"),
                (1.875d, "8.0099423298800297035369403087755975288643e-3"),
                (2.000d, "4.6777349810472658379307436327470713891082e-3"),
                (2.125d, "2.6540293594823415306731881850932990105594e-3"),
                (2.250d, "1.4627165866811516979107963729829539115167e-3"),
                (2.375d, "7.8293821789111919755110235847838860612012e-4"),
                (2.500d, "4.0695201744495893956421573997491272034868e-4"),
                (2.625d, "2.0537573614121744048190291615577719550787e-4"),
                (2.750d, "1.0062192211963683690439197508695676506474e-4"),
                (2.875d, "4.7854839743773412997339696855567924953532e-5"),
                (3.000d, "2.2090496998585441372776129582320379847707e-5"),
                (3.125d, "9.8967346252456194686333778537695047224152e-6"),
                (3.250d, "4.3027794636751218304758951302365975671351e-6"),
                (3.375d, "1.8152814274403557618221653497495569332462e-6"),
                (3.500d, "7.4309837234141274552368375609563572066009e-7"),
                (3.625d, "2.9514019251156985859272042859973854684078e-7"),
                (3.750d, "1.1372725656979665325907658300660885722833e-7"),
                (3.875d, "4.2513944082491122028999237999396876247710e-8"),
                (4.000d, "1.5417257900280018852159673486884048572145e-8"),
                (4.125d, "5.4234007995650660053122340857553106174577e-9"),
                (4.250d, "1.8505741373867425200558380715287267361247e-9"),
                (4.375d, "6.1248329535693601580863881887932854053455e-10"),
                (4.500d, "1.9661604415428874762791603676643326605777e-10"),
                (4.625d, "6.1216105130342262537538884886542045933838e-11"),
                (4.750d, "1.8485047721485310887429416100978423243114e-11"),
                (4.875d, "5.4134064662979414775602384216560604263947e-12"),
                (5.000d, "1.5374597944280348501883434853833788901181e-12"),
                (5.125d, "4.2345633763362236740801941642802610729622e-13"),
                (5.250d, "1.1310313266887153882786804799111859257998e-13"),
                (5.375d, "2.9294885544871543488668606421188109198891e-14"),
                (5.500d, "7.3578479179743980630683623985700902082231e-15"),
                (5.625d, "1.7920200056510064154061824581558795923425e-15"),
                (5.750d, "4.2321366174257376259467198527189716324398e-16"),
                (5.875d, "9.6915556452771751688004539529941131108198e-17"),
                (6.000d, "2.1519736712498913116593350399187384630478e-17"),
                (6.125d, "4.6332221552992657770013341280629024441645e-18"),
                (6.250d, "9.6722041318762539914820331840630214548519e-19"),
                (6.375d, "1.9577523042806317548292570344235153086643e-19"),
                (6.500d, "3.8421483271206474698758045437687766214493e-20"),
                (6.625d, "7.3108696855303228907941357549942016820028e-21"),
                (6.750d, "1.3487678893611300512198696745911492419410e-21"),
                (6.875d, "2.4125351343758223432661414648132812791541e-22"),
                (7.000d, "4.1838256077794143986140102238999322500296e-23"),
                (7.125d, "7.0344877474599621171176288003682647641631e-24"),
                (7.250d, "1.1466900814815011616694721910609817044788e-24"),
                (7.375d, "1.8122170524396203532055023289389068416934e-25"),
                (7.500d, "2.7766493860305691006639662093224125867397e-26"),
                (7.625d, "4.1245334202091771956331271931487145480757e-27"),
                (7.750d, "5.9397478595171462152753239533061421673945e-28"),
                (7.875d, "8.2927237829304430630765952338570312623548e-29"),
                (8.000d, "1.1224297172982927079967888443170279093432e-29"),
                (8.125d, "1.4728237544682700263363490101606859851069e-30"),
                (8.250d, "1.873566470550499707861653117702631450777e-31"), 
                (8.500d, "2.762324071333771446134502930057782220140e-33"), 
                (8.750d, "3.597115728647072015655353606339209916159e-35"), 
                (9.000d, "4.137031746513810238053903467362524595710e-37"), 
                (9.250d, "4.202037214919711134532494234453493753163e-39"), 
                (9.500d, "3.769214485654879941677087321047321969621e-41"), 
                (9.750d, "2.985700832800578322056651761043123070674e-43"), 
                (10.00d, "2.088487583762544757000786294957788611561e-45"), 
                (10.25d, "1.290013890504290403797209253385948962351e-47"), 
                (10.50d, "7.035928090177522686735314989983875191751e-50"), 
                (10.75d, "3.388452427064942942660894089219606831681e-52"), 
                (11.00d, "1.440866137943694680339809702856082753964e-54"), 
                (11.25d, "5.409760870689921360947655026259550905907e-57"), 
                (11.50d, "1.793309643576782058109116063975786076405e-59"), 
                (11.75d, "5.248636097008541671157983183971734749304e-62"), 
                (12.00d, "1.356261169205904212780306156590417572667e-64"), 
                (12.25d, "3.094126740540209854237029614468312650811e-67"), 
                (12.50d, "6.231942781979911006139549129089687366380e-70"), 
                (12.75d, "1.108134576852057960907593116377449735918e-72"), 
                (13.00d, "1.739557315466724521804198548243026985089e-75"), 
                (13.25d, "2.410771372637134615588252740201400742679e-78"), 
                (13.50d, "2.949433113257988264759187435938438926110e-81"), 
                (13.75d, "3.185523620640338001790800447697931519182e-84"), 
                (14.00d, "3.037229847750311665115172806783328447912e-87"), 
                (14.25d, "2.556372343088648868939699771470534558477e-90"), 
                (14.50d, "1.899395941979503049574200023929227908760e-93"), 
                (14.75d, "1.245797352185758615141408086658100171681e-96"), 
                (15.00d, "7.212994172451206666565066558692927109934e-100"), 
                (15.25d, "3.686515149230813465764332211389301971018e-103"), 
                (15.50d, "1.663201640048872334149102741177324070423e-106"), 
                (15.75d, "6.623668412559063808762121636728292844925e-110"), 
                (16.00d, "2.328485751571530693364872854573442597534e-113"), 
                (16.25d, "7.225478196226944360486775000452028512357e-117"), 
                (16.50d, "1.979130575553267972057334325050809148358e-120"), 
                (16.75d, "4.785134183467091950453594998528076725618e-124"), 
                (17.00d, "1.021228015094260881145599235077652994402e-127"), 
                (17.25d, "1.923789189586878804528003707702041062035e-131"), 
                (17.50d, "3.198863812343480988193469195296936871692e-135"), 
                (17.75d, "4.694993499922695090818200892276059029545e-139"), 
                (18.00d, "6.082369231816399307668466715702274949588e-143"), 
                (18.25d, "6.955154367397821601893728142483383500675e-147"), 
                (18.50d, "7.019961574985679324600020297239088904740e-151"), 
                (18.75d, "6.253951588878104938080162591555563553455e-155"), 
                (19.00d, "4.917722839256475446413297625239608170931e-159"), 
                (19.25d, "3.413196955966576099558098510532677691189e-163"), 
                (19.50d, "2.090954147922729460496243175316188288743e-167"), 
                (19.75d, "1.130607053466201413823813027190139137812e-171"), 
                (20.00d, "5.395865611607900928934999167905345604088e-176"), 
                (20.25d, "2.272956417880783670852495026489847565112e-180"), 
                (20.50d, "8.450842369572458394324668950605102484119e-185"), 
                (20.75d, "2.773232110564736291495298137161389961725e-189"), 
                (21.00d, "8.032453871022455669021356947138268888968e-194"), 
                (21.25d, "2.053452158444984112647947761116551036295e-198"), 
                (21.50d, "4.633336539658445649129685587506432913376e-203"), 
                (21.75d, "9.227299967997719828242606115069933902625e-208"), 
                (22.00d, "1.621905860933472513052034647026123265178e-212"), 
                (22.25d, "2.516202428760510733789701065888253148603e-217"), 
                (22.50d, "3.445348860464601762032717331402956086707e-222"), 
                (22.75d, "4.163775674054040907034328506200476820593e-227"), 
                (23.00d, "4.441265948088057244074884428946738565069e-232"), 
                (23.25d, "4.181099003178296282154476262499709475942e-237"), 
                (23.50d, "3.474059495649971502741911545224997354591e-242"), 
                (23.75d, "2.547687348154478056649046693146926437782e-247"), 
                (24.00d, "1.648982583151933514218512437543746903943e-252"), 
                (24.25d, "9.419895378132563858808978102213852790506e-258"), 
                (24.50d, "4.749361264067378997552878283177710144873e-263"), 
                (24.75d, "2.113403975655410135839019349039793114209e-268"), 
                (25.00d, "8.300172571196522752044012769513722768714e-274"),
            }) {
                ddouble x_dec = ddouble.BitDecrement(x);
                ddouble x_inc = ddouble.BitIncrement(x);

                Console.WriteLine(ddouble.Erfc(x_dec));
                Console.WriteLine(ddouble.Erfc(x));
                Console.WriteLine(ddouble.Erfc(x_inc));

                HPAssert.AreEqual(expected, ddouble.Erfc(x_dec), expected * 2e-29);
                HPAssert.AreEqual(expected, ddouble.Erfc(x), expected * 2e-29);
                HPAssert.AreEqual(expected, ddouble.Erfc(x_inc), expected * 2e-29);

                Assert.IsTrue(ddouble.Abs((2 - expected) / ddouble.Erfc(-x_dec) - 1) < 1e-28);
                Assert.IsTrue(ddouble.Abs((2 - expected) / ddouble.Erfc(-x) - 1) < 1e-29);
                Assert.IsTrue(ddouble.Abs((2 - expected) / ddouble.Erfc(-x_inc) - 1) < 1e-28);
            }

            ddouble erfc_pzero = ddouble.Erfc(0d);
            ddouble erfc_mzero = ddouble.Erfc(-0d);
            ddouble erfc_pinf = ddouble.Erfc(double.PositiveInfinity);
            ddouble erfc_ninf = ddouble.Erfc(double.NegativeInfinity);
            ddouble erfc_nan = ddouble.Erfc(double.NaN);

            Assert.IsTrue(erfc_pzero == 1, nameof(erfc_pzero));
            Assert.IsTrue(erfc_mzero == 1, nameof(erfc_mzero));
            Assert.IsTrue(ddouble.IsPlusZero(erfc_pinf), nameof(erfc_pinf));
            Assert.IsTrue(erfc_ninf == 2, nameof(erfc_ninf));
            Assert.IsTrue(ddouble.IsNaN(erfc_nan), nameof(erfc_nan));
        }

        [TestMethod]
        public void InverseErfTest() {
            for (ddouble v = -1; v <= 1; v += 0.0625d) {
                ddouble y = ddouble.Erf(v);
                ddouble z = ddouble.InverseErf(y);

                Assert.AreEqual(v, z);
            }

            for (ddouble v = Math.ScaleB(1, -12); v > Math.ScaleB(1, -985); v /= 2) {
                ddouble y = ddouble.Erf(v);
                ddouble z = ddouble.InverseErf(y);

                Assert.AreEqual(v, z, $"exponent: {ddouble.Frexp(v).exp}");

                ddouble y_dec = ddouble.BitDecrement(y);
                ddouble z_dec = ddouble.InverseErf(y_dec);

                ddouble y_inc = ddouble.BitDecrement(y);
                ddouble z_inc = ddouble.InverseErf(y_inc);

                Assert.AreNotEqual(y, y_dec);
                Assert.AreNotEqual(y, y_inc);

                Assert.AreEqual(z, z_dec, $"exponent: {ddouble.Frexp(v).exp}");
                Assert.AreEqual(z, z_inc, $"exponent: {ddouble.Frexp(v).exp}");
            }

            for (ddouble y = Math.ScaleB(1, -12); y > Math.ScaleB(1, -20); y /= 2) {
                ddouble z = ddouble.InverseErf(y);

                ddouble y_dec = ddouble.BitDecrement(y);
                ddouble z_dec = ddouble.InverseErf(y_dec);

                ddouble y_inc = ddouble.BitDecrement(y);
                ddouble z_inc = ddouble.InverseErf(y_inc);

                Assert.AreNotEqual(y, y_dec);
                Assert.AreNotEqual(y, y_inc);

                Assert.AreEqual(z, z_dec, $"exponent: {ddouble.Frexp(y).exp}");
                Assert.AreEqual(z, z_inc, $"exponent: {ddouble.Frexp(y).exp}");
            }
        }

        [TestMethod]
        public void InverseErfcTest() {
            for (ddouble v = 0; v <= 10; v += 0.0625d) {
                ddouble y = ddouble.Erfc(v);
                ddouble z = ddouble.InverseErfc(y);

                Assert.AreEqual(v, z);
            }
        }
    }
}
