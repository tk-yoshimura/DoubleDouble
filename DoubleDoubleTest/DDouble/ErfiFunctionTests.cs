﻿using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public class ErfiFunctionTests {
        [TestMethod]
        public void ErfiTest() {
            ddouble[] expecteds = {
                "0",
                "0.1417854741302653892695611291822166870284",
                "0.2880836197949719840347000657269359169307",
                "0.4438425591867228373564889759936520181721",
                "0.6149520946965109808396811856236413930513",
                "0.8089077755380093303885887470587797490134",
                "1.035757284411962967860153121657800186386",
                "1.309523726702842603015781291779763179117",
                "1.650425758797542876025337729561362443896",
                "2.088437702338401695728226737046338979760",
                "2.669134288682375042514337772244237758134",
                "3.463498225740043425281759743250548693805",
                "4.584733257284426942221381032382701256662",
                "6.217712450945903254655722618265149391260",
                "8.671694929603424624528939507121413419276",
                "12.47682084592790352464947394351765707004",
                "18.56480241457555259870429191324101719886",
                "28.61522436520737244744332486045401626177",
                "45.73515014065393064927848135636707229226",
                "75.82541550886600169642515031763956362548",
                "130.3957550132469268137315308322899098275",
                "232.5117391345682176846703110275196699506",
                "429.6800722766928978578778044318818072404",
                "822.4940190230732011724368356696668099761",
                "1629.994622601565651061647952076274162779",
                "3342.787935462847224996360611022191695145",
                "7091.422701590346430101943651771310553441",
                "15556.81878713589463421438945152589103223",
                "35282.28771517168531015799721641242794918",
                "82707.92771358109749528158170304120145478",
                "200361.4093850143371089487534959998007051",
                "501524.6711497978952024299036225421327059",
                "1.296959730717639231527940950621767303819e6",
                "3.464728197355206858354300773046620693854e6",
                "9.560467515846173411849930213529016033425e6",
                "2.724709786908200771364536860472748983920e7",
                "8.019745890121747817731640016987353558785e7",
                "2.437652373007814313535178203436747675698e8",
                "7.651177545300527295138451248561439737702e8",
                "2.479745982849625756843051716430970295112e9",
                "8.298273880676803516146223190746919951873e9",
                "2.867154709878626520365807198593538421336e10",
                "1.022774953560307600098360615507619079314e11",
                "3.766681425457432213309033530000986513178e11",
                "1.432099172039832821476869289855699288226e12",
                "5.620945664839590278321216261625806464340e12",
                "2.277484801591185607629307448387235151916e13",
                "9.525768603136860589618850840502401151812e13",
                "4.112751455828238709717312025942799233644e14",
                "1.832917847079552227654629616851439675212e15",
                "8.431842912883763041256213110105322360486e15",
                "4.003708686454918374948492012393667002611e16",
                "1.962252677547840500494187831613913812130e17",
                "9.926415149376551078396654648488920269627e17",
                "5.182842294781432575668638051059822916749e18",
                "2.793026429564073956499945477845059779040e19",
                "1.553486253460503993888981032171520673074e20",
                "8.917832028976550006540993072832273922579e20",
                "5.283542226056481870811380549302851291625e21",
                "3.230724476212227671123160195053665752814e22",
                "2.038818719178621130770728335863434632816e23",
                "1.327873252489381657248863145295437078399e24",
                "8.925438593955495394023975960513442191479e24",
                "6.191463003122407100661534855274493016762e25",
                "4.432449746002334631994113424773495106566e26",
                "3.274744550292794632317064159760064669120e27",
                "2.496838980184112950478365016240851939447e28",
                "1.964625360595488568478078423251833569445e29",
                "1.595298001394745672777335107074809316532e30",
                "1.336822279418773539231906272427237764376e31",
                "1.156038670294215100256231281952416310868e32",
                "1.031656381603770745621722552876151675745e33",
                "9.500776643665995567475170723057540497704e33",
                "9.029046896448313715433850653226425246018e34",
                "8.854845947450609382144514687257982862406e35",
                "8.961362707502366102142829890981686454027e36",
                "9.358770288532333942259873338938990360164e37",
                "1.008586244811629679999651510799928131000e39",
                "1.121643226437356743948022741857864480831e40",
                "1.287187709888264452600440457175337204413e41",
                "1.524307422708669699360546614726544062464e42",
                "1.862708672933508651085318276279031862456e43",
                "2.348860531614195412303120275154311967790e44",
                "3.056382764843839584484085785033269595772e45",
                "4.103881683437385279812462144050221637606e46",
                "5.686131760951135267397487166760213205320e47",
                "8.129661876188907009110128157369964765839e48",
                "1.199388985109730109936239337121197339319e50",
                "1.825905189505990453134325055831927126573e51",
                "2.868306903949720961275564993316783605925e52",
                "4.649442957066557003675688758375134516219e53",
                "7.776834677879269925770015784524694860995e54",
                "1.342240123529734283157260432388274423574e56",
                "2.390461679497141255953531324911964786301e57",
                "4.392951662360862611002117623346384211109e58",
                "8.330153814764798090146289503977445342707e59",
                "1.629935799524349403718387287287144079589e61",
                "3.290847952926369739956045930860538895730e62",
                "6.855892000175581561404692663668412843384e63",
                "1.473798035929366933445588586507733206001e65",
                "3.269105136308551513762560177708468131025e66",
                "7.482311656940663881506171901314391803638e67",
                "1.767086466870209959215601006982860018245e69",
                "4.306198511836670968476666359994575100585e70",
                "1.082788646363468292955626191481238249930e72",
                "2.809350516067728205070231005343165648130e73",
                "7.521075754232289409547490603683715614205e74",
                "2.077614801397481170707438453358820309508e76",
                "5.921890693949829805445544033375818114151e77",
                "1.741667743392499124247748224636093557400e79",
                "5.285414994027353843073163488337111380136e80",
                "1.655011648899468537480957711109605330225e82",
                "5.347250519427755278536516340097538677841e83",
                "1.782653248590051119877820680154399520206e85",
                "6.132103218675128916328915441005481224964e86",
                "2.176494949087272093767574755042520423768e88",
                "7.970964962998145257481559934230536744504e89",
                "3.012094447586337863306483368658679756361e91",
                "1.174438161053115559515685725373962672779e93",
                "4.724926485538754981735508200949955411272e94",
                "1.961384563867380603481670593910821125947e96",
                "8.401033627808546860058804566017277159274e97",
                "3.712824796618345074352439193862459130761e99",
                "1.693079901363171311126311444152400168208e101",
                "7.966200502251799718775899163329540297717e102",
                "3.867455446239488418204086414570114487696e104",
                "1.937310584833768725784343218291128571614e106",
                "1.001319280784399565932671921928394223701e108",
                "5.340044324608436774368315950790589762687e109",
                "2.938432179387028769659086177320984424317e111",
                "1.668340098691960871998028382480895942378e113",
                "9.773524431577581356274224326353628887689e114",
                "5.907654871613051446889908921460261427579e116",
                "3.684477728592848603126541915152640037415e118",
                "2.371009430098814058032893626556049685877e120",
                "1.574297403776351342658163638331852731415e122",
                "1.078539524015566115398387305586567110875e124",
                "7.623963025504083563611491967711782555001e125",
                "5.560586234282908278706201182345844010325e127",
                "4.184611268667357312198471548258344016840e129",
                "3.249257033685136393299568748049558857192e131",
                "2.603197366983444565622587565735148582514e133",
                "2.151908924008520761307770595077980230813e135",
                "1.835414362869259722717619822257975630366e137",
                "1.615241663722112060799220605738214809007e139",
                "1.466674476240745340578832886117013681724e141",
                "1.374113196393031445549043516143074014766e143",
                "1.328322562721229207781207970472104821144e145",
                "1.324880080196447228801110155439159003657e147",
                "1.363456533870625495827622967716100593210e149",
                "1.447762957843802197415853584937487952334e151",
                "1.586151972180111044061483013212022355462e153",
                "1.793011367133720311055691262742684434414e155",
                "2.091278865907692655727381751952552412020e157",
                "2.516698925170155445629267452925997156336e159",
                "3.124933241713478582154538285080367928459e161",
                "4.003502923397732203029270396568006000649e163",
                "5.292114684008526964490062053959255475276e165",
                "7.217849213224589824879555746160068876462e167",
                "1.015723728035816944475229682660638518954e170",
                "1.474797539628786202447733153131835124599e172",
                "2.209418605737531666106484212382793629151e174",
                "3.415168941898107477751215228817570356889e176",
                "5.446716251448208376634163544234197773411e178",
                "8.962836583078825769514694048014651064030e180",
                "1.521753017438591133112527663794492913645e183",
                "2.665818843968872317668778972011110450821e185",
                "4.818420187342000630912914470146738961005e187",
                "8.985993273815336050823738256805070801525e189",
                "1.729078475905429300104969914512863498030e192",
                "3.432814467824885356165166661703116680352e194",
                "7.031901687509445929897236215484805932954e196",
                "1.486215896306601841404445991823618664248e199",
                "3.240988522155027674875043131693685386163e201",
                "7.292213769715724188405206653480419538225e203",
                "1.692884891040183470511419817540795898321e206",
                "4.054912060004321045887146399625535974801e208",
                "1.002123535283447724334639273838079978336e211",
                "2.555328475250948423148949522853563013936e213",
                "6.722916245911214115220686978159881514399e215",
                "1.824962757731019016076350110939637182246e218",
                "5.111348566514492131004542211225185016342e220",
                "1.477073234086377927028158434523778469313e223",
                "4.404062248765562871133605327509826120260e225",
                "1.354845007234303197128655536825322131023e228",
                "4.300415172356987198946855359422772380793e230",
                "1.408366283684841099902667926264908454874e233",
                "4.758884668960726788766653067753384296974e235",
                "1.659124326326630041978659297454789025629e238",
                "5.968108911122791818717823282030715213113e240",
                "2.215024199367692930700190985951074173746e243",
                "8.482112107268429711172806514206835973920e245",
                "3.351299267458319719997141350275026414124e248",
                "1.366173825803640383677337020891681818182e251",
                "5.746218446047810089876567857052808654032e253",
                "2.493685067556528260990666494396512479172e256",
                "1.116565640809231125774975086295409754446e259",
                "5.158340175046622860621168466206651310371e261",
                "2.458774200442449217680427270722337388538e264",
                "1.209233446994071456034972114966773773753e267",
                "6.135986249821951253809528562741857222457e269",
                "3.212485801604752815347176931284600040412e272",
                "1.735323946034025986601319682599059743698e275",
                "9.671688467491275706074705577944222128724e277",
                "5.561684251899650969550771914456922561934e280",
                "3.299837447133424343357235256372618344891e283",
                "2.020044088756976729155787388078641394747e286",
                "1.275883829725434103456820050349007030316e289",
                "8.314637164730987655299566436793997899881e291"
            };

            for ((int i, ddouble x) = (0, 0); i < expecteds.Length; i++, x += 1d / 8) {
                ddouble expected = expecteds[i];

                ddouble y = ddouble.Erfi(x);
                ddouble y_neg = ddouble.Erfi(-x);
                ddouble y_dec = ddouble.Erfi(ddouble.BitDecrement(x));
                ddouble y_inc = ddouble.Erfi(ddouble.BitIncrement(x));

                Console.WriteLine(x);
                Console.WriteLine(y);

                HPAssert.AreEqual(expected, y, expected * 4e-29);
                HPAssert.AreEqual(-expected, y_neg, expected * 4e-29);
                
                if (x > 0) {
                    HPAssert.AreEqual(expected, y_dec, expected * 4e-29);
                    HPAssert.AreEqual(expected, y_inc, expected * 4e-29);
                }
            }

            Assert.AreEqual(0, ddouble.Erfi(0));

            Assert.IsTrue(ddouble.IsFinite(ddouble.Erfi(ddouble.Epsilon)));

            Assert.IsTrue(ddouble.IsNaN(ddouble.Erfi(ddouble.NaN)));
            Assert.IsTrue(ddouble.IsPositiveInfinity(ddouble.Erfi(ddouble.PositiveInfinity)));
            Assert.IsTrue(ddouble.IsNegativeInfinity(ddouble.Erfi(ddouble.NegativeInfinity)));
        }

        [TestMethod]
        public void DawsonFTest() {
            ddouble[] expecteds = {
                "0",
                "0.1237060184828397339382226579409357066401",
                "0.2398391635628982123649799419758239353577",
                "0.3417442551906100786393159664730912377974",
                "0.4244363835020222959340423524896695710964",
                "0.4850624642080813978383048154011944653373",
                "0.5230127677445182531387957140644953377149",
                "0.5396989828965289080131434546334200571308",
                "0.5380795069127684191363874204075567547920",
                "0.5220504950180077130499359098685507412152",
                "0.4958270739643261192168277558727994692211",
                "0.4634169401539564156975299224492729745848",
                "0.4282490710853986254771901051517453449916",
                "0.3929766153972906516503245570218368616968",
                "0.3594364206717429204772700614187838118550",
                "0.3287247031462869636921177501684790747777",
                "0.3013403889237919660346644392864226952119",
                "0.2773518558940046797364455024861410205697",
                "0.2565542628448491546227011475422271961832",
                "0.2385983453344650041699287347171296111014",
                "0.2230837221674354811269173182510305487675",
                "0.2096184044329277918326198808004674772364",
                "0.1978509471741545235739788837028056679347",
                "0.1874832020359482915442473486118359725321",
                "0.1782710306105582873425994922405126302292",
                "0.1700187100915766753560416686549074268358",
                "0.1625709145606869966055043200689920343181",
                "0.1558045551308537852741017090461253497987",
                "0.1496215930807564847530417782986806947846",
                "0.1439432002236586544341772301344805525468",
                "0.1387052395935911982958257376916847124558",
                "0.1338548657059378644515457313033326585537",
                "0.1293480012360051155914705262576659452783",
                "0.1251474680755086699788285806807532113867",
                "0.1212215942943236569679344287487943600686",
                "0.1175431634373978411593063064635874857083",
                "0.1140886102268249801604070363433653507119",
                "0.1108373952067854342940928126936227438858",
                "0.1077715111802444954250203502502823768907",
                "0.1048750883222575523356639441162157473359",
                "0.1021340744242768354385510070492717462886",
                "0.09953597324946795241559529343240254288533",
                "0.09706962847320189143605512295145217746673",
                "0.09472504382758852273238604135655567826521",
                "0.09249323231075475996731114854567288188380",
                "0.09036608895026993045953833483423111700251",
                "0.08833628281447531186215896493312010004489",
                "0.08639716487021182404885520263650018727199",
                "0.08454268897454385223907092939884307194007",
                "0.08276734381929029930479573610537449606409",
                "0.08106609406101172223516728088135635481332",
                "0.07943432919452531668330693374794418728671",
                "0.07786781898606987138888501076332405008944",
                "0.07636267448842898075217470844870882069489",
                "0.07491531382621561386788591056997460330004",
                "0.07352243207385584014735203706930370581716",
                "0.07218097465823629202776351330973987109780",
                "0.07088811380761200370878346454068654417986",
                "0.06964122764217069666599154160004790932715",
                "0.06843788156270939177197803431305688295376",
                "0.06727581164463061598693271840806256838768",
                "0.06615290978683171033385786963719354899646",
                "0.06506721040057242590859055698282485163396",
                "0.06401687845328806184522180250243055927428",
                "0.06300019870755338791924572951696726884111",
                "0.06201556601679339029048261036518049867985",
                "0.06106147655752788294162368880063771844802",
                "0.06013651989345656246277733152932426800721",
                "0.05923937177997213955134969180255205362169",
                "0.05836878762908806006310265201586483867372",
                "0.05752359656457836860725195855088732004030",
                "0.05670269600559451804589342802841425456026",
                "0.05590504672435046070372688265785159621899",
                "0.05512966832982272080444373587843959617639",
                "0.05437563513493836466501248544492988549428",
                "0.05364207236954019214047335580476707927593",
                "0.05292815270562564644066849075522537579314",
                "0.05223309306503889121478938283378261702174",
                "0.05155615168302628935271068153652803673249",
                "0.05089662540390537586989615438529080852007",
                "0.05025384718759852803274841986071548588791",
                "0.04962718380798947325037856374733350309529",
                "0.04901603372601170726056173162149879601622",
                "0.04841982512210558129328618477434037083108",
                "0.04783801407421343834833799734669208591721",
                "0.04727008286884403132721257473206783835460",
                "0.04671553843394953029957728502140398659097",
                "0.04617391088343889538916194195225133926258",
                "0.04564475216411602011957178103699485464648",
                "0.04512763479669353951405182188894297210392",
                "0.04462215070330547952011572103558540362734",
                "0.04412791011463440868308597220900444177083",
                "0.04364454055039053664978178367939054489430",
                "0.04317168586743925139982958017339385493854",
                "0.04270900537037688239392050207766573014646",
                "0.04225617297980815502534670340737482474906",
                "0.04181287645398826031792911758880895599112",
                "0.04137881665986245875720242880875243919849",
                "0.04095370688987086566064016265811310778947",
                "0.04053727222118923708260089776832945742051",
                "0.04012924891435147311439648806276737848537",
                "0.03972938384844908957360194257844482962649",
                "0.03933743399032966223360979027116987047447",
                "0.03895316589542251531513668499974565337278",
                "0.03857635523800775062039763311407541710975",
                "0.03820678636891591554802534820251445336373",
                "0.03784425189880181110836243967925678906224",
                "0.03748855230527859544662481044165521452012",
                "0.03713949556232874146895375801068494235360",
                "0.03679689679052772041569823966943497616257",
                "0.03646057792672554776146972370678685822546",
                "0.03613036741193147794264820988417827796675",
                "0.03580609989623900947661882599630764713772",
                "0.03548761595971271691428800335314481974740",
                "0.03517476184823594036237162560570383574902",
                "0.03486738922338964949677328018877964817430",
                "0.03456535492549840959226484256778507031415",
                "0.03426852074903981106283376303143905551323",
                "0.03397675322966943233819410091922445571897",
                "0.03368992344216479663817664603394662940932",
                "0.03340790680863922587293065065641302375896",
                "0.03313058291642032444209726517023673673843",
                "0.03285783534502834601130205930125969471775",
                "0.03258955150172718334698524953121431718396",
                "0.03232562246515542577108703059094627150867",
                "0.03206594283657707886567604376279727500858",
                "0.03181041059832134437670076199443220863078",
                "0.03155892697900850403801411846896075928009",
                "0.03131139632518461178355335195230901508146",
                "0.03106772597901153197330741559595292531059",
                "0.03082782616168101060872927846454534676945",
                "0.03059160986224206346630532149094098798399",
                "0.03035899273155012982869245661229643663809",
                "0.03012989298106428305798255828053738586736",
                "0.02990423128623541040905101126893499824364",
                "0.02968193069424376659569501593076775765445",
                "0.02946291653585875343040662885442578929652",
                "0.02924711634120725913676625012832159902231",
                "0.02903445975924947712212875781643114700235",
                "0.02882487848077288076193143623148717888061",
                "0.02861830616472601848052144570565503077244",
                "0.02841467836772406770109967852985672841066",
                "0.02821393247656769826843520519013993446234",
                "0.02801600764362579289368091188343898632214",
                "0.02782084472494099752638696754046453158524",
                "0.02762838622092496845512418988826583783443",
                "0.02743857621951758242291076834446474742834",
                "0.02725136034169131533917514648806961248186",
                "0.02706668568918850590679754909382619252630",
                "0.02688450079438533191257643834312605784779",
                "0.02670475557218206611932854349390927173701",
                "0.02652740127382457070721398065398171308296",
                "0.02635239044256705725423368073945191734527",
                "0.02617967687109090483619332829342320445581",
                "0.02600921556059881191621218228036281733958",
                "0.02584096268150777679427027970816128027801",
                "0.02567487553566837368211293994206849672404",
                "0.02551091252004153291774168985201849072948",
                "0.02534903309176755926692898751573443446392",
                "0.02518919773456544546420894258717421061847",
                "0.02503136792640367194699495234782353186858",
                "0.02487550610838664006813190544113393040448",
                "0.02472157565480367604553548809473687148432",
                "0.02456954084429017687240362675627624838377",
                "0.02441936683205295701281529698882949641939",
                "0.02427101962311420493767974423886656669166",
                "0.02412446604653067980254185441324967567191",
                "0.02397967373054687865867188220866548168822",
                "0.02383661107864289082923794192627980234792",
                "0.02369524724643953529782219202005486565142",
                "0.02355555211942515552389631321676667218354",
                "0.02341749629147012997998011985248434932728",
                "0.02328105104409675147244757999646292029988",
                "0.02314618832647363917741949626483698692413",
                "0.02301288073610527917585920705165202617460",
                "0.02288110150018864667794427249468246835969",
                "0.02275082445761015036671129909971534728484",
                "0.02262202404155736037605920220431094616216",
                "0.02249467526272114010857529798537301359235",
                "0.02236875369306490192044062921480958372561",
                "0.02224423545013875096180650191364419651627",
                "0.02212109718191727326591392691539540355755",
                "0.02199931605214066644332093961446311747941",
                "0.02187886972613980679606690214206098828517",
                "0.02175973635712669789199790393952072293010",
                "0.02164189457293255504864619351196651050038",
                "0.02152532346317655004123422062792162265061",
                "0.02141000256684897280758429127176399543793",
                "0.02129591186029326398450916728735265428526",
                "0.02118303174557203566784467594450959865662",
                "0.02107134303920282962309202071628152763661",
                "0.02096082696124996396336043996405226183621",
                "0.02085146512475939263646238294275593339145",
                "0.02074323952552404841307578035733269647154",
                "0.02063613253216766084695211957629931776816",
                "0.02053012687653553721026804523249302414986",
                "0.02042520564438126794135454002419330273526",
                "0.02032135226633876985668899829977087065656",
                "0.02021855050916951138655290193397698738749",
                "0.02011678446727517544437914019781729956187",
                "0.02001603855446640822543036969174556711519",
                "0.01991629749597867718813072867380093606988",
                "0.01981754632072661958665261792238526123997",
                "0.01971977035378860503331910725486575422610",
                "0.01962295520911356246552810102424471945094",
                "0.01952708678244243432286582811503565790683",
                "0.01943215124443691941412903376588496929374",
                "0.01933813503400845154140065790177554452280",
                "0.01924502485184063408362867873144382451673",
                "0.01915280765409861202614552490051058347034",
                "0.01906147064631911292428177194370980038599",
                "0.01897100127747512754778476705918802478119",
                "0.01888138723420942997903950898975050082750",
                "0.01879261643523135621637713182761013666778",
                "0.01870467702587147032320654302710800489515",
                "0.01861755737278894829978201161937193123925",
                "0.01853124605882670255022144176459687579576",
                "0.01844573187800945446489549196932463050856",
                "0.01836100383068013960954972438947222113428",
                "0.01827705111877019966069087628487616706769",
                "0.01819386314119947788724864177716419115653",
                "0.01811142948940159096987021881428996323651",
                "0.01802973994297079957405796303740133651914",
                "0.01794878446542654363931369337772635578611",
                "0.01786855320009194608686383632137665172220",
                "0.01778903646608272084329518484718318220982",
                "0.01771022475440304797367058261572782402583",
                "0.01763210872414510055048743319520732443772",
                "0.01755467919878902487783131696241923500791",
                "0.01747792716260028805607937577766904750245",
                "0.01740184375712141481408555981713150113522",
                "0.01732642027775523924577631864636264121340",
                "0.01725164817043689675013350665853725883539",
                "0.01717751902839187726254065842846489369047",
                "0.01710402458897755294803712496123243773698",
                "0.01703115673060568206192407735234128084119",
                "0.01695890746974347582170633351918041930578",
                "0.01688726895799089702077090427235961818762",
                "0.01681623347923193788602028672534206913117",
                "0.01674579344685770047004532885929844786576",
                "0.01667594140105917579843578041580641347884",
                "0.01660667000618768818383424765723489212130",
                "0.01653797204818103868422716707622497555104",
                "0.01646984043205344673243045776917940433563",
                "0.01640226817944745160053053308280715491244",
                "0.01633524842624599568625651632017549383783",
                "0.01626877442024296971250071829866219757137",
                "0.01620283951887055590684452181361173958480",
                "0.01613743718698175916132418237323095457684",
                "0.01607256099468656814628546288821481079681",
                "0.01600820461524023844487413319258735027884",
                "0.01594436182298223806186058749914353411604",
                "0.01588102649132444221416002265576064113014",
                "0.01581819259078720919949117258582807577666",
                "0.01575585418708201243002037448984347093254",
                "0.01569400543923934547260731935687198749934",
                "0.01563264059778065721672244655715194493276"
            };

            for ((int i, ddouble x) = (0, 0); i < expecteds.Length; i++, x += 1d / 8) {
                ddouble expected = expecteds[i];

                ddouble y = ddouble.DawsonF(x);
                ddouble y_neg = ddouble.DawsonF(-x);
                ddouble y_dec = ddouble.DawsonF(ddouble.BitDecrement(x));
                ddouble y_inc = ddouble.DawsonF(ddouble.BitIncrement(x));

                Console.WriteLine(x);
                Console.WriteLine(y);

                HPAssert.AreEqual(expected, y, expected * 4e-31);
                HPAssert.AreEqual(-expected, y_neg, expected * 4e-31);

                if (x > 0) {
                    HPAssert.AreEqual(expected, y_dec, expected * 4e-31);
                    HPAssert.AreEqual(expected, y_inc, expected * 4e-31);
                }
            }

            Assert.AreEqual(0, ddouble.DawsonF(0));

            Assert.IsTrue(ddouble.IsFinite(ddouble.DawsonF(ddouble.Epsilon)));

            Assert.IsTrue(ddouble.IsNaN(ddouble.DawsonF(ddouble.NaN)));
            Assert.AreEqual(0, ddouble.DawsonF(ddouble.PositiveInfinity));
            Assert.AreEqual(0, ddouble.DawsonF(ddouble.NegativeInfinity));
        }
    }
}
