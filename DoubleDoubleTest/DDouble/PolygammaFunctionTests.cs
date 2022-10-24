﻿using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public class PolygammaFunctionTests {
        [TestMethod]
        public void PolygammaPlusXTest() {
            ddouble[] xrcp3_expecteds = {
                "-3.132033780020806322996419074287268854155",
                "10.09559712542709408179200409989251636052",
                "-55.12212239940160755245575595481501856908",
                "488.1838165438142417557707623221055926354",
                "-5838.126959146053001493682874735019847641",
                "87502.21523619660237952856384716927388504",
                "-1.574738211752141481663773785579786349638e6",
                "3.306795068702937175946047259644659259909e7",
                "-7.936216079646194203152889520289552806421e8",
                "2.142772163322746832760079812938631784176e10",
                "-6.428311871947932483212095239687401366203e11",
                "2.121342537477343882450615364997793046033e13",
                "-7.636832793045285993447868244580677377437e14",
                "2.978364755975215298594956987047392435651e16",
                "-1.250913194012986857323823545292239602394e18",
                "5.629109369125531912481739900015947763932e19",
                "-2.701972496708358885085601101527123338304e21"
            };
            ddouble[] xrcp2_expecteds = {
                "-1.963510026021423479440976332998755567193",
                "4.934802200544679309417245499938075567657",
                "-16.82879664423431999559633426116029987071",
                "97.40909103400243723644033268870511124973",
                "-771.4742498266672251905359219240334210345",
                "7691.113548602435496241755549219359190938",
                "-92203.45792380302328623108795826541569781",
                "1.290440218185598064969486268531320148351e6",
                "-2.064489996176004142640251788793538653646e7",
                "3.715954523850974272237078266537599634836e8",
                "-7.431824508858768975491796771277214753871e9",
                "1.634995211347588000460293649197329010065e11",
                "-3.923983571677609426828547578011830231983e12",
                "1.020235301346519504127715173987855148545e14",
                "-2.856658445221248147083380715909708904369e15",
                "8.569974937266651725203269743703669500965e16",
                "-2.742391937439321116043117742696464301184e18"
            };
            ddouble[] x1_expecteds = {
                "-0.5772156649015328606065120900824024310422",
                "1.644934066848226436472415166646025189219",
                "-2.404113806319188570799476323022899981530",
                "6.493939402266829149096022179247007416649",
                "-24.88626612344087823195277167496882003337",
                "122.0811674381338967657421515749104633482",
                "-726.0114797149844353246542358918536669119",
                "5060.549875237639470468573602083608424905",
                "-40400.97839874763488532782365545085427878",
                "363240.9114223826268071435255657477648911",
                "-3.630593311606628712990618842832054105457e6",
                "3.992662298773108670232707324047201489779e7",
                "-4.790603798898314524268767644990636347190e8",
                "6.227402193410971764192853408947415910061e9",
                "-8.718095783017206784519122031036435756611e10",
                "1.307694352218913820890099907485110170285e12",
                "-2.092294967948151090663165568811151437911e13"
            };
            ddouble[] x2_expecteds = {
                "0.4227843350984671393934879099175975689578",
                "0.6449340668482264364724151666460251892189",
                "-0.4041138063191885707994763230228999815300",
                "0.4939394022668291490960221792470074166485",
                "-0.8862661234408782319527716749688200333699",
                "2.081167438133896765742151574910463348218",
                "-6.011479714984435324654235891853666911902",
                "20.54987523763947046857360208360842490516",
                "-80.97839874763488532782365545085427877963",
                "360.9114223826268071435255657477648911444",
                "-1793.311606628712990618842832054105457279",
                "9822.987731086702327073240472014897792737",
                "-58779.88983145242687676449906363471897092",
                "381393.4109717641928534089474159100605105",
                "-2.666630172067845191220310364357566106455e6",
                "1.998421891382089009990748511017028532863e7",
                "-1.597914815109066316556881115143791102514e8"
            };
            ddouble[] x4_expecteds = {
                "1.256117668431800472726821243250930902291",
                "0.2838229557371153253613040555349140781078",
                "-0.08003973224511449672540224894882590745590",
                "0.04486532819275507502194810517293334257443",
                "-0.03750069134211279985400624287005460127118",
                "0.04155838463595437891087585474585429472016",
                "-0.05726160798855055099168445152444880490588",
                "0.09419965464907266336098205617358265550532",
                "-0.1799305263271578472566687110280327805424",
                "0.3910177187036247018246055282861531471111",
                "-0.9519244156357158131729646558429972881790",
                "2.564729638752319452450958221268772770451",
                "-7.572825660626846281341008460202891052690",
                "24.30811333305938798192917682049083105088",
                "-84.25456183323504831761257514811461754195",
                "313.5450408303190475213512581953694475237",
                "-1246.627412979491101638133514630469087646"
            };
            ddouble[] x8_expecteds = {
                "2.015641477955609996536345052774740426101",
                "0.1331370146940314251345466859204016064525",
                "-0.01769956919576777390929167773621387851734",
                "0.004699239795945103867953627563295383082245",
                "-0.001868795150637613515568481414106278739486",
                "0.0009895100047713386985290704019523477031052",
                "-0.0006540077388357522676388244971654193968485",
                "0.0005180006142069730045633152917039339194725",
                "-0.0004780108952053932442640934521395077704591",
                "0.0005034554975983348296247922650996956792240",
                "-0.0005957605173775548817484629642698087345763",
                "0.0007823229837159865891478731891977727595619",
                "-0.001128629008105434198106630669041710946361",
                "0.001774087660046464245653132614477738620237",
                "-0.003017474522571974436547095297100336392782",
                "0.005520672272289554398032103812741344904691",
                "-0.01080964393628448601388857409787917322632"
            };
            ddouble[] x16_expecteds = {
                "2.741013328327460368386716903146590797951",
                "0.06449378340323936178171077231192722541736",
                "-0.004158010123958962154186529784226526244033",
                "0.0005359612597642188298988185704989867792821",
                "-0.0001035912536035878274790793747406089387431",
                "0.00002668717152575119590426327252602384874487",
                "-8.590996985078728985906033109107969200654e-6",
                "3.317553637347401343223176953967889240717e-6",
                "-1.494142012984606381722220857486704258367e-6",
                "7.687961123259225217523864568465140914140e-7",
                "-4.448741433186511758418584242141487774379e-7",
                "2.859404448218163897617877236117786674069e-7",
                "-2.020982149832662974266803836148340508023e-7",
                "1.557738626195685805239030465264537198255e-7",
                "-1.300308478817425780043256276511081879562e-7",
                "1.168534239116929675743499754036877173003e-7",
                "-1.124758503924543168135305541400637999682e-7"
            };
            ddouble[] x32_expecteds = {
                "3.450029530534987242153326090171007139697",
                "0.03174336652030209012658168043874142714133",
                "-0.001007556760214090739218511059311726463463",
                "0.00006395575477797629957667367357464242328092",
                "-6.088980637002713770220713267415298042568e-6",
                "7.728797333132754942484837555987203722842e-7",
                "-1.226176291916820561801477598238605600115e-7",
                "2.334204206034863721175125699843824402234e-8",
                "-5.183645561462205065385421414521072201695e-9",
                "1.315489746185330454219039113014002456311e-9",
                "-3.755398032743789724333272011523566241274e-10",
                "1.191093451036965618204708130903928327020e-10",
                "-4.155203528200057851143961784431633397367e-11",
                "1.581216358036530429048522500146844246934e-11",
                "-6.518031181112207377403580110781324997879e-12",
                "2.893279207611498363441524803114507756322e-12",
                "-1.375915113737355028302415137498478506818e-12"
            };
            ddouble[] x48_expecteds = {
                "3.860748176829252741171646777712298633718",
                "0.02105185413233829383780923084017887952869",
                "-0.0004431641995829658457838294154589902164850",
                "0.00001865747854532293143995108994412476203879",
                "-1.178193073055497523051664567452483225950e-6",
                "9.919794941018281346162878733459871445174e-8",
                "-1.043957748455881216875216290954690778585e-8",
                "1.318342907394489557060993786350587583099e-9",
                "-1.942247811041130797714833261929800401954e-10",
                "3.270068486203752068005861199302053859126e-11",
                "-6.193634803478570163194289166805775303265e-12",
                "1.303394417098582386469679401867597228458e-12",
                "-3.017052304343782816632499069356326757505e-13",
                "7.618376462814150986459656419740309225021e-14",
                "-2.083954268111221939586702659626595540471e-14",
                "6.138788808689230941664675269242565419835e-15",
                "-1.937423253319950685661238379332593064592e-15"
            };
            ddouble[] x64_expecteds = {
                "4.151050238804236165399371143320644619234",
                "0.01574770606433893015574400307135046450730",
                "-0.0002479851221632853494989320234167558112602",
                "7.810070883425701632576133345339534652575e-6",
                "-3.689492338414187686482413631164641018981e-7",
                "2.323849601800061492917481853073060059535e-8",
                "-1.829576053437621093672493384661133091127e-9",
                "1.728483340054565351731983126380470000845e-10",
                "-1.905099903968899502462620299260825422883e-11",
                "2.399679816695422106499555447854683914230e-12",
                "-3.400418787728331873673995184966330860439e-13",
                "5.353774207763982500523422531359516980504e-14",
                "-9.271957425863330710550938035928326160339e-15",
                "1.751710847041137688227389640447046076173e-15",
                "-3.585143730331338503391914729425325279663e-16",
                "7.901807882258539544134156091131326879364e-17",
                "-1.865952775565173349145401214727765490814e-17"
            };
            ddouble[] x96_expecteds = {
                "4.559130815987242107379822339693930323591",
                "0.01047110851491297833872701120486528438729",
                "-0.0001096431117813377753475156294620138792386",
                "2.296127886868890674393214062072384826468e-6",
                "-7.212703595389226437741296646896764426863e-8",
                "3.020889690075578554418648950914308536688e-9",
                "-1.581530948832298627086204381658843480085e-10",
                "9.935684738652617912199785924280285690591e-12",
                "-7.282168679128589008549448772857516364901e-13",
                "6.099744550524821677006777477237708240354e-14",
                "-5.747925343019764297136120882815485701729e-15",
                "6.018165351834886079713192552592255110948e-16",
                "-6.931158574399678766851373892167979200895e-17",
                "8.708275334553508858347534807853946688219e-18",
                "-1.185268146171350033270116558185093436575e-18",
                "1.737327907636861495585325270041936019860e-19",
                "-2.728388697798364463678384886582386927346e-20"
            };
            ddouble[] x128_expecteds = {
                "4.848118927687639516189135313339097477476",
                "0.007843097050014615129539165768044658428073",
                "-0.00006151385601545905609372706359740314608764",
                "9.649083931863369106358439630988775581543e-7",
                "-2.270326139587236917397007816891745496370e-8",
                "7.122409268278285928863059119615885578716e-10",
                "-2.793015775941484012197248191399747801053e-11",
                "1.314313208228301545938272855825298075155e-12",
                "-7.215541354190934736087978786012496865532e-14",
                "4.527189664115852772131733793137093354746e-15",
                "-3.195499514943489540910415640861622563994e-16",
                "2.506132843404904310640402610514296562594e-17",
                "-2.162020863059602398377529754786926136723e-18",
                "2.034707609113506249478076555948767764641e-19",
                "-2.074455006822807692698078380160191002979e-20",
                "2.277657874737240126146565635078571246010e-21",
                "-2.679377825857669764777666956557713351309e-22"
            };
            ddouble[] x256_expecteds = {
                "5.543223047915747508270461600455737078603",
                "0.003913889328608396405461529929293372114800",
                "-0.00001531851012200510764811766334972350324331",
                "1.199096004572688992700028449563324360723e-7",
                "-1.407933325101820082682704586494998610186e-9",
                "2.204186831868870309295658367348873220753e-11",
                "-4.313444128956131425718988628881913994080e-13",
                "1.012932818675093864322494506928648753033e-14",
                "-2.775130067625208728411634221876838635487e-16",
                "8.689152832964879223307625784202351593359e-18",
                "-3.060719266798138928183673057336779508688e-19",
                "1.197916356589447287612145126457110234589e-20",
                "-5.157290835341679239628781988469233943002e-22",
                "2.422170766515157987870843207386285066420e-23",
                "-1.232393631042222539738141349441126908711e-24",
                "6.752712523184131663673849291174322418118e-26",
                "-3.964329520887161038159968168240484888762e-27"
            };
            ddouble[] x512_expecteds = {
                "6.237347744648190248411710686386059896684",
                "0.001955033590395297932905093990874591268672",
                "-3.822155122170286188305157482569267883059e-6",
                "1.494487378284319794763524971831203796118e-8",
                "-8.765310699339597354305819165374307125178e-11",
                "6.854582005934456932569034182205148149279e-13",
                "-6.700458364012755213464246495317018968830e-15",
                "7.859757683120399495589322325883429862600e-17",
                "-1.075623921621582133529879507261799703857e-18",
                "1.682300431292731301627164653214851578050e-20",
                "-2.960049981860700698630772181622186052178e-22",
                "5.786978711776749361275871501156404844264e-24",
                "-1.244506794292009675063692114110447924405e-25",
                "2.919651944424195581232693439490813714857e-27",
                "-7.420392185957051587211510895745559518433e-29",
                "2.030987180515143763827165091906300394714e-30",
                "-5.955943784413634634487547232122558080979e-32"
            };
            ddouble[] x1024_expecteds = {
                "6.930983444876600972770560747438310741121",
                "0.0009770394923786026216525966908576305552979",
                "-9.546060937280718048279424223628568970946e-7",
                "1.865375409692268184344011272812268950534e-9",
                "-5.467635025285560559492208141216701224905e-12",
                "2.136837459901390869968178255717668446342e-14",
                "-1.043886878350796611552260551099689114762e-16",
                "6.119509313492321503613852309644181923871e-19",
                "-4.185299260688962824479404984947529150367e-21",
                "3.271360050191606962888860039806419780197e-23",
                "-2.876621107020556671064597056281016443139e-25",
                "2.810570190354119068034685077011786476345e-27",
                "-3.020639236069436426954205382088164480819e-29",
                "3.541537210284510597476784951776081034254e-31",
                "-4.498283581723818174057345724701530682435e-33",
                "6.152994137574006769003416613812065520319e-35",
                "-9.017566642965158350288830343316550815037e-37"
            };
            ddouble[] x2048_expecteds = {
                "7.624374825666183952154448396250529110156",
                "0.0004884004786921034938867727793830404775366",
                "-2.385350228450966064644730771909192913900e-7",
                "2.330012294215966667658892947365188681265e-10",
                "-3.413937155974845786576909496909642681901e-13",
                "6.669473634510637256567227229662171996221e-16",
                "-1.628686895985213573291494961918474777381e-18",
                "4.772708188603645522764734467302824279155e-21",
                "-1.631694847737466814223328435139784478738e-23",
                "6.375363343884391607652259960290697319127e-26",
                "-2.802356956745483966476665628605243985056e-28",
                "1.368672207077338359333505463193506116980e-30",
                "-7.353060164403454406969044011905756751443e-33",
                "4.309484697013943893778263170751692184779e-35",
                "-2.736179998555876556303961518211648592006e-37",
                "1.8708917488641126672561964265400254474e-39",
                "-1.370616229915201441354955374701171751556e-41"
            };
            ddouble[] x4096_expecteds = {
                "8.317644091439790011330179857191882272991",
                "0.0002441704297477068711282519324167471284141",
                "-5.961919846697579595901974000248476891870e-8",
                "2.911449033249352939409223536170548703955e-11",
                "-2.132669253124114620203869028685069136313e-14",
                "2.082939030785762414858192280040588888992e-17",
                "-2.542960528255635721253500367909191322507e-20",
                "3.725494479038321188935967615435646304738e-23",
                "-6.367588894335352872727226551806643849971e-26",
                "1.243821483992185977996301810565077284031e-28",
                "-2.733339714262604602685889058859159889332e-31",
                "6.674006995197362814553660995276651955381e-34",
                "-1.792554573807443221004905112632403133410e-36",
                "5.252265534457600911712554080759693180280e-39",
                "-1.667182204827819025633181423744745198589e-41",
                "5.69907193948455874166416786820535e-44",
                "-2.087317111385409338093846396136052378015e-46"
            };
            ddouble[] x8192_expecteds = {
                "9.010852310881275591453751252701705498215",
                "0.0001220777633837618235155992785170158745703",
                "-1.490298029427350401753775063577059254183e-8",
                "3.638644995116596265962856680923282729070e-12",
                "-1.332592923289157656833030014420198067598e-15",
                "6.507198510721220583981939750617658658501e-19",
                "-3.971921175751020126051034959296184644479e-22",
                "2.909299499155100653694969383247210683378e-25",
                "-2.486125409479577381673071568619605191965e-28",
                "2.428005011723440349768407829055318112193e-31",
                "-2.667648762024921782513503198461730527219e-34",
                "3.256605903170359818082959882685311570523e-37",
                "-4.373150753953430642269931969933396820105e-40",
                "6.406373460317707809611443765133348141e-43",
                "-1.016698450053190870310939853791702986237e-45",
                "1.7376277892434294582880840499e-48",
                "-3.181885652290160104667734352050046439341e-51"
            };
            ddouble[] x16384_expecteds = {
                "9.704030009950668473751738426732771838389",
                "0.00006103701893304484350266882841311289259777",
                "-3.725517679076251189850242455418116836872e-9",
                "4.547889859439534536809424903634090650662e-13",
                "-8.327689175924167363333253930996269115603e-17",
                "2.033189285072689858618650989210969727286e-20",
                "-6.204990634740283121197001313968772748722e-24",
                "2.272404780879109549945103095067340251408e-27",
                "-9.709056933284284622491696342102245697495e-31",
                "4.740895123096206550960401138247680434921e-34",
                "-2.604330941215167474383755439014045022053e-37",
                "1.589605964733559210425466904044895573459e-40",
                "-1.067272899375957281875166482312402217612e-43",
                "7.8171787107892792909525622978884869087587e-47",
                "-6.202784679620425369156751351644806629626e-50",
                "5.300392779948469240608170232405204644611e-53",
                "-4.852802594335331866593740245183554047436e-56"
            };

            foreach ((ddouble x, ddouble[] expecteds) in new (ddouble, ddouble[])[] {
                (ddouble.Rcp(3), xrcp3_expecteds), (ddouble.Rcp(2), xrcp2_expecteds),
                (1, x1_expecteds), (2, x2_expecteds), (4, x4_expecteds), (8, x8_expecteds), (16, x16_expecteds),
                (32, x32_expecteds), (48, x48_expecteds), (64, x64_expecteds), (96, x96_expecteds),
                (128, x128_expecteds), (256, x256_expecteds), (512, x512_expecteds), (1024, x1024_expecteds),
                (2048, x2048_expecteds), (4096, x4096_expecteds), (8192, x8192_expecteds), (16384, x16384_expecteds)}) {

                for (int n = 0; n <= 16; n++) {
                    ddouble expected = expecteds[n];

                    ddouble actual = ddouble.Polygamma(n, x);
                    HPAssert.AreEqual(expected, actual, ddouble.Abs(expected) * 2e-29d, $"{x},{n}");

                    if (x > 0) {
                        ddouble actual_dec = ddouble.Polygamma(n, ddouble.BitDecrement(x));
                        HPAssert.AreEqual(expected, actual_dec, ddouble.Abs(expected) * 2e-29d, $"{x}-eps,{n}");

                        ddouble actual_inc = ddouble.Polygamma(n, ddouble.BitIncrement(x));
                        HPAssert.AreEqual(expected, actual_inc, ddouble.Abs(expected) * 2e-29d, $"{x}+eps,{n}");
                    }
                }
            }
        }

        [TestMethod]
        public void PolygammaMinusXTest() {
            ddouble[] x1d3_expecteds = {
                "1.681765584213411527597659183354886878129",
                "12.06387540935871740998731723327568515323",
                "46.61516343510270471166937155621961904930",
                "517.3313356375320901719110120176550006965",
                "5647.650557273360469361112129392565399565",
                "88852.87465876487446593172249936554343246",
                "1.562317117095185506044079625920194233769e6",
                "3.319669649452954316681337023322187866829e7",
                "7.920681111204684705022667268423715667272e8",
                "2.144862882444138102010741276821995617954e10",
                "6.425171380169253983297733584364130800550e11",
                "2.121860325462636989257854615293851713696e13",
                "7.635900442346127384486638075283916055113e14",
                "2.978546530321753581108839381321247962400e16",
                "1.2508750179656022919875521730335306099278e18",
                "5.629195261244097082716888248937796403010e19",
                "2.701951882133340234598422211636180897320e21"
            };
            ddouble[] x1d2_expecteds = {
                "0.03648997397857652055902366700124443280684",
                "8.934802200544679309417245499938075567657",
                "-0.8287966442343199955963342611602998707098",
                "193.4090910340024372364403326887051112497",
                "-3.474249826667225190535921924033421034468",
                "15371.11354860243549624175554921935919094",
                "-43.45792380302328623108795826541569781152",
                "2.580680218185598064969486268531320148351e6",
                "-1059.961760041426402517887935386536456390",
                "7.431845723850974272237078266537599634836e8",
                "-42108.85876897549179677127721475387105018",
                "3.269987339347588000460293649197329010065e11",
                "-2.464477609426828547578011830231983090794e6",
                "2.040470389218519504127715173987855148545e14",
                "-1.991796481470833807159097089043686102054e8",
                "1.713994967539145172520326974370366950096e17",
                "-2.123938511604311774269646430118435976326e10"
            };
            ddouble[] x2d3_expecteds = {
                "-1.632033780020806322996419074287268854155",
                "12.34559712542709408179200409989251636052",
                "-48.37212239940160755245575595481501856908",
                "518.5588165438142417557707623221055926354",
                "-5655.876959146053001493682874735019847641",
                "88869.09023619660237952856384716927388504",
                "-1.562436336752141481663773785579786349638e6",
                "3.319712037452937175946047259644659259909e7",
                "-7.920715717146194203152889520289552806421e8",
                "2.144864712260246832760079812938631784176e10",
                "-6.425173048541682483212095239687401366203e11",
                "2.121860443339375132450615364997793046033e13",
                "-7.635900562493629743447868244580677377437e14",
                "2.978546540932788267344956987047392435651e16",
                "-1.2508750191718965338863235452922396023942e18",
                "5.629195262517985140216114900015947763932e19",
                "-2.701951882294170110429351101527123338304e21"
            };
            ddouble[] x5d4_expecteds = {
                "3.714139120213527830373113237182819306830",
                "19.18187964767160649839766288041707824912",
                "123.7213667836623603685672930895615909216",
                "1558.220912534850599760254079190246723227",
                "24481.06371418458252746107765095288952266",
                "492230.2106228745797136083679504951324611",
                "1.179122206844090462546894144514763905124e7",
                "3.303526877181860568545099041301637323442e8",
                "1.056911423128156675907974227838030714957e10",
                "3.805137431302343928841395526064050909152e11",
                "1.522020474017433759405734706549276009224e13",
                "6.696940385655708609446716678018223479349e14",
                "3.214523309386136240230372976862206313259e16",
                "1.671553517726064052896692802981466948579e18",
                "9.360697088570752169369999668244629347632e19",
                "5.616418774887069835115315195787627773045e21",
                "3.594507904572116257448775950096200762357e23"
            };
            ddouble[] x7d4_expecteds = {
                "-2.322691628614360646184768241334778815462",
                "19.30163754452978647623277036650075690133",
                "-124.2138213542305819249587424730886718871",
                "1558.384840416549526415991607874880152176",
                "-24481.77733829588783410569157614909260485",
                "492230.9303018743214822774633319201818376",
                "-1.179122545421787517596356750403840548003e7",
                "3.303526941602634899042860587078476069395e8",
                "-1.056911426446482539229753110733244102088e10",
                "3.805137432269359301728542287402452010951e11",
                "-1.522020474072359711191414903085163368196e13",
                "6.696940385677531599531628004386315775486e14",
                "-3.214523309387506682510327712514700452635e16",
                "1.671553517726133094327844578543663135136e18",
                "-9.360697088570800082881558519738580139782e19",
                "5.616418774887072751524048960825562313683e21",
                "-3.594507904572116480551980719480631296056e23"
            };
            ddouble[] x11d5_expecteds = {
                "5.322870221172649419396581154721952459154",
                "28.20053015219407856724602058735391396582",
                "246.9151203992111672378471783027953264680",
                "3768.520540634763159031848585081690525984",
                "74935.41188648129804447345001357401548631",
                "1.875502842143779489407287948867502464156e6",
                "5.624675822337832084632327674832986253115e7",
                "1.968781269276941104250938360613313029943e9",
                "7.874970723291393658851088837785074554278e10",
                "3.543753439356365147436628151883563076214e12",
                "1.771874582385301996076689554287669661330e14",
                "9.745313085380611586301363583529988432211e15",
                "5.847187413315671514044993778437737802643e17",
                "3.800671889207282682029934085392889616240e19",
                "2.660470310027889984850563733342418749041e21",
                "1.995352734840287676132162248698553677293e23",
                "1.596282187407178360898367838770890633180e25"
            };
            ddouble[] x13d5_expecteds = {
                "0.1148975066969351363206065692345228262249",
                "10.59168836239021545200119585933211310995",
                "-22.37779658040922058612863087467587535105",
                "283.5383805977355566692328828138817829422",
                "-2037.458483014215997063245882565899371798",
                "31893.09994293285298860145493777492972158",
                "-413775.2818640757124093318186137200220040",
                "7.990964453422899060881158625412112476932e6",
                "-1.498090525994159026882971452209144899682e8",
                "3.520723007369340303715590584257414699884e9",
                "-8.551717448808715131566355939370512703347e10",
                "2.397565056755283055779695247280572433079e12",
                "-7.101005501669021786298744006215359733859e13",
                "2.327692342908550748849139023008699246202e15",
                "-8.100569834542753805601464483867440576490e16",
                "3.049301973386180205947610017553758401403e18",
                "-1.2166305747769302854722558030336973275083e20"
            };
            ddouble[] x23d7_expecteds = {
                "3.839444163525235544672230492114225145110",
                "15.88366495939162051163634219971836079122",
                "80.83530412504712643163557010895113886426",
                "926.7544034506826764514366271571742318340",
                "12481.61457953809302150297713692824311034",
                "221528.0121870901620097651899195919200964",
                "4.624948652216547959212071741573857018416e6",
                "1.135696509952567175448176830495744204066e8",
                "3.177017401423574324093155803533490601339e9",
                "1.001126940040651743126063535726952342383e11",
                "3.503429091478214346755634466166754967282e12",
                "1.3488993423600700033936354954284927728569e14",
                "5.665244104160915940537898738047670097212e15",
                "2.577710279302681578667686369579397695442e17",
                "1.2630732893793968217577817951151300525581e19",
                "6.631144736625442839560967403421204307074e20",
                "3.713438819532407792182748768079698595002e22"
            };
            ddouble[] x26d7_expecteds = {
                "-1.064532825090133822152046283964256624911",
                "15.91011979783724439652563259420228781587",
                "-80.95944644708972860938809600870285296143",
                "926.7640481641756155874446583597773855595",
                "-12481.66040710967332880044167727234805145",
                "221528.0237380031121103036390785290651486",
                "-4.624948708142907662269056263182867135820e6",
                "1.135696510239252436839134600515614015913e8",
                "-3.177017401565809331339029929896834872775e9",
                "1.001126940041857126740214546109963560554e11",
                "-3.503429091478830165540112393021773384447e12",
                "1.3488993423600776655941604869496786004852e14",
                "-5.665244104160919988366880156008118323597e15",
                "2.577710279302681647111806981913491644920e17",
                "-1.2630732893793968255097524376831650670278e19",
                "6.631144736625442840377493653680950994472e20",
                "-3.713438819532407792229326426509516232009e22"
            };

            foreach ((ddouble x, ddouble[] expecteds) in new (ddouble, ddouble[])[] {
                (-ddouble.Rcp(3), x1d3_expecteds), (-ddouble.Rcp(2), x1d2_expecteds), (-2 * ddouble.Rcp(3), x2d3_expecteds),
                (-5 * ddouble.Rcp(4), x5d4_expecteds), (-7 * ddouble.Rcp(4), x7d4_expecteds),
                (-11 * ddouble.Rcp(5), x11d5_expecteds), (-13 * ddouble.Rcp(5), x13d5_expecteds),
                (-23 * ddouble.Rcp(7), x23d7_expecteds), (-26 * ddouble.Rcp(7), x26d7_expecteds)}) {

                for (int n = 0; n <= 16; n++) {
                    ddouble expected = expecteds[n];

                    ddouble actual = ddouble.Polygamma(n, x);
                    HPAssert.AreEqual(expected, actual, ddouble.Abs(expected) * 4e-30d, $"{x},{n}");

                    if (x > 0) {
                        ddouble actual_dec = ddouble.Polygamma(n, ddouble.BitDecrement(x));
                        HPAssert.AreEqual(expected, actual_dec, ddouble.Abs(expected) * 4e-30d, $"{x}-eps,{n}");

                        ddouble actual_inc = ddouble.Polygamma(n, ddouble.BitIncrement(x));
                        HPAssert.AreEqual(expected, actual_inc, ddouble.Abs(expected) * 4e-30d, $"{x}+eps,{n}");
                    }
                }
            }
        }

        [TestMethod]
        public void PolygammaLimitTest() {
            for (int n = 1; n <= 16; n++) {
                for (ddouble x = 16384; ddouble.IsFinite(x); x *= 2) {
                    ddouble yn = ddouble.Polygamma(n, x - 1);
                    ddouble yz = ddouble.Polygamma(n, x);
                    ddouble yp = ddouble.Polygamma(n, x + 1);

                    Console.WriteLine($"n={n} x={x}\n{yz}");

                    if ((n & 1) == 1) {
                        Assert.IsTrue(yn >= yz && yz >= yp && yp >= 0, $"{yn},\n{yz},\n{yp}");
                    }
                    else {
                        Assert.IsTrue(yn <= yz && yz <= yp && yp <= 0, $"{yn},\n{yz},\n{yp}");
                    }
                }
            }
        }

        [TestMethod]
        public void PolygammaAbnormalTest() {
            for (int n = 1; n <= 16; n++) {
                Console.WriteLine(n);

                ddouble pg_pzero = ddouble.Polygamma(n, 0d);
                ddouble pg_mzero = ddouble.Polygamma(n, -0d);
                ddouble pg_mone = ddouble.Polygamma(n, -1d);
                ddouble pg_mtwo = ddouble.Polygamma(n, -2d);
                ddouble pg_pinf = ddouble.Polygamma(n, double.PositiveInfinity);
                ddouble pg_ninf = ddouble.Polygamma(n, double.NegativeInfinity);
                ddouble pg_nan = ddouble.Polygamma(n, double.NaN);

                Assert.IsTrue(((n & 1) == 1) ? ddouble.IsPositiveInfinity(pg_pzero) : ddouble.IsNaN(pg_pzero), nameof(pg_pzero));
                Assert.IsTrue(((n & 1) == 1) ? ddouble.IsPositiveInfinity(pg_mzero) : ddouble.IsNaN(pg_mzero), nameof(pg_mzero));
                Assert.IsTrue(((n & 1) == 1) ? ddouble.IsPositiveInfinity(pg_mone) : ddouble.IsNaN(pg_mone), nameof(pg_mone));
                Assert.IsTrue(((n & 1) == 1) ? ddouble.IsPositiveInfinity(pg_mtwo) : ddouble.IsNaN(pg_mtwo), nameof(pg_mtwo));
                Assert.IsTrue(ddouble.IsZero(pg_pinf), nameof(pg_pinf));
                Assert.IsTrue(ddouble.IsNaN(pg_ninf), nameof(pg_ninf));
                Assert.IsTrue(ddouble.IsNaN(pg_nan), nameof(pg_nan));
            }
        }
    }
}
