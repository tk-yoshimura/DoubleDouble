﻿using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrecisionTestTools;
using System;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public class BumpFunctionTests {
        [TestMethod]
        public void BumpTest() {
            ddouble[] expecteds = {
                0,
                "1.805552954703391613238134495671682948026e-111",
                "7.047262358137851086435244530074944881288e-56",
                "2.396891800673170660433012370957835563032e-37",
                "4.429362283831479356403378283751257632891e-28",
                "1.610898929673808766289255176924779352116e-22",
                "8.219012764559409571907006520496547246998e-19",
                "3.662131541486833851699606065889478816422e-16",
                "3.555335263604078208237102717564543842306e-14",
                "1.249837969417730084768426619784725317399e-12",
                "2.157794055877442864043398336853507928780e-11",
                "2.221142306832748558820932381573049590950e-10",
                "1.551330314005074697789364207434496645382e-9",
                "8.040217049117136143852597400974148090819e-9",
                "3.296356161589367938703483099335460821504e-8",
                "1.120381818016302515653140401295653378740e-7",
                "3.269908460001808572834005798979611488689e-7",
                "8.418241738855284784039881071658437314718e-7",
                "1.952121316136281055369836590709746865108e-6",
                "4.145365563038587901645698590382827041555e-6",
                "8.168196619062895141201208805170396342211e-6",
                "0.00001509526282254338163938343926850189708246",
                "0.00002639402400933298281890091044619622449556",
                "0.00004398040704587353255623784830157045786523",
                "0.00007026135297475722057125072299197401882158",
                "0.0001081617182787280897907849776699047371532",
                "0.0001611344392028121107379090804915004268755",
                "0.0002331541505788943315508505108445273613687",
                "0.0003286954647877238857412569042542002231071",
                "0.0004526978271650920320432172415509969518162",
                "0.0006105192848911384915587102786910320838467",
                "0.0008078816794074636021820161466347736639092",
                "0.001050809752278488386732324599231099917753",
                "0.001345566496814909332494123892411651770872",
                "0.001698586842676094424686240758601407135933",
                "0.002116411469098437667076623200821297624636",
                "0.002605622235633743394253773361073715736162",
                "0.003172780419475339123869700645371270789153",
                "0.003824368669989649380163655060946814348066",
                "0.004566737342202122817322978755810443530780",
                "0.005406055655378539287925095635979612252276",
                "0.006348267940964000701900394774758399738551",
                "0.007399055094375690622018141379665189191061",
                "0.008563801224647811183316817440045537275610",
                "0.009847565401251553886555326407871606465111",
                "0.01125505832494224227640982670657946177278",
                "0.01279062369571157322204352889956826461499",
                "0.01445822401261205816952265136266103726882",
                "0.01626143051450034149068612037116625236340",
                "0.01820341695513262862021564160013663561781",
                "0.02028695689843876460092840837295982461825",
                "0.02251442421845682552995914085746009200992",
                "0.02488779649189933865268481639782503595647",
                "0.02740866097849094892860242266656201539630",
                "0.03007822289414177771235942121903009945514",
                "0.03289731569396793861884571194791691659843",
                "0.03586641309556284993185106228110814549602",
                "0.03898564258730653103896042800474153969179",
                "0.04225480018151970769778605623941265666018",
                "0.04567336618764692048512561467132150089335",
                "0.04924052179616925743577561355455218880758",
                "0.05295516627943015572077035127300888282199",
                "0.05681593463086965019581386877735296337685",
                "0.06082121547919342393829344296212937436248",
                "0.06496916912866406212754280996733993595945",
                "0.06925774559092060038539365893359862004723",
                "0.07368470248744961215952599346258099702439",
                "0.07824762271499839420805742674759521482557",
                "0.08294393177879918219842374606825038181827",
                "0.08777091471043167658056158122546054539563",
                "0.09272573249846558027963396517905213452637",
                "0.09780543797067801172462712357083762563598",
                "0.1030069910766213225271600407316345021127",
                "0.1083272735286194801898886029783982511912",
                "0.1137631027678955667912189453336882856217",
                "0.1193112452304838604336908293045967252943",
                "0.1249684288948667711059873278943750031612",
                "0.1307313550999131646373766749596362664136",
                "0.1365967096276976850937093993913086576412",
                "0.1425611730511712903611488024891252848505",
                "0.1486214303514549696486364414425792609310",
                "0.1547741798137675929423796393162333018901",
                "0.1610161412147031333018194973572218472296",
                "0.1673440633167717477451912911956143181071",
                "0.1737547306888441817238924170762977702676",
                "0.1802449698734211879970540994677532081549",
                "0.1868116549235209900192771462876812368116",
                "0.1934517123334701484687696866620588436329",
                "0.2001621253890280796627221582707755627167",
                "0.2069399379631039188188103054504818463413",
                "0.2137822577838665917147627589360602732590",
                "0.2206862592023340032139287649491561997038",
                "0.2276491854865831250798648251720665138830",
                "0.2346683506695760948912886004639855519668",
                "0.2417411409772734174833346818881487683784",
                "0.2488650158632276795861819290216218249425",
                "0.2560375086752419843541455995877759124592",
                "0.2632562269789571474925490562377503296436",
                "0.2705188525624195567183254653932686323363",
                "0.2778231411447949085358528433257519888708",
                "0.2851669218114477024571048497557449307005",
                "0.2925480961966168156534912579225888915251",
                "0.2999646374338967020694282845374791728567",
                "0.3074145888936934055746865026552905587123",
                "0.3148960627257750107054465557637663390180",
                "0.3224072382239865359326851063710534528983",
                "0.3299463600291576377817597724458784099226",
                "0.3375117361852048318062744257802624215989",
                "0.3451017360624242808809746578018578896812",
                "0.3527147881609917069575789435799898440632",
                "0.3603493778067370058499471063549583019781",
                "0.3680040447503463221998825268636151215274",
                "0.3756773806802666647110509654661803059289",
                "0.3833680266587500274834642997470344058339",
                "0.3910746704896773433462143910891612630953",
                "0.3987960440260488988755781481981225968351",
                "0.4065309204243181753410839809716377733512",
                "0.4142781113520812054041373548984207738721",
                "0.4220364641550139368875345303171455353609",
                "0.4298048589883760291208820953327586076997",
                "0.4375822059178710461403373403709056658891",
                "0.4453674419941700806589124781633448412940",
                "0.4531595283049682593454835055989763892642",
                "0.4609574470080510813082423369086233232366",
                "0.4687601983484998154305823811151072734541",
                "0.4765667976628618908360716102504918806370",
                "0.4843762723728530172761247327107891341673",
                "0.4921876589709423428246493881968504615440",
                0.5,
                "0.5078123410290576571753506118031495384560",
                "0.5156237276271469827238752672892108658327",
                "0.5234332023371381091639283897495081193630",
                "0.5312398016515001845694176188848927265459",
                "0.5390425529919489186917576630913766767634",
                "0.5468404716950317406545164944010236107358",
                "0.5546325580058299193410875218366551587060",
                "0.5624177940821289538596626596290943341109",
                "0.5701951410116239708791179046672413923003",
                "0.5779635358449860631124654696828544646391",
                "0.5857218886479187945958626451015792261279",
                "0.5934690795756818246589160190283622266488",
                "0.6012039559739511011244218518018774031649",
                "0.6089253295103226566537856089108387369047",
                "0.6166319733412499725165357002529655941661",
                "0.6243226193197333352889490345338196940711",
                "0.6319959552496536778001174731363848784726",
                "0.6396506221932629941500528936450416980219",
                "0.6472852118390082930424210564200101559368",
                "0.6548982639375757191190253421981421103188",
                "0.6624882638147951681937255742197375784011",
                "0.6700536399708423622182402275541215900774",
                "0.6775927617760134640673148936289465471017",
                "0.6851039372742249892945534442362336609820",
                "0.6925854111063065944253134973447094412877",
                "0.7000353625661032979305717154625208271433",
                "0.7074519038033831843465087420774111084749",
                "0.7148330781885522975428951502442550692995",
                "0.7221768588552050914641471566742480111292",
                "0.7294811474375804432816745346067313676637",
                "0.7367437730210428525074509437622496703564",
                "0.7439624913247580156458544004122240875408",
                "0.7511349841367723204138180709783781750575",
                "0.7582588590227265825166653181118512316216",
                "0.7653316493304239051087113995360144480332",
                "0.7723508145134168749201351748279334861170",
                "0.7793137407976659967860712350508438002962",
                "0.7862177422161334082852372410639397267410",
                "0.7930600620368960811811896945495181536587",
                "0.7998378746109719203372778417292244372833",
                "0.8065482876665298515312303133379411563671",
                "0.8131883450764790099807228537123187631884",
                "0.8197550301265788120029459005322467918451",
                "0.8262452693111558182761075829237022297324",
                "0.8326559366832282522548087088043856818929",
                "0.8389838587852968666981805026427781527704",
                "0.8452258201862324070576203606837666981099",
                "0.8513785696485450303513635585574207390690",
                "0.8574388269488287096388511975108747151495",
                "0.8634032903723023149062906006086913423588",
                "0.8692686449000868353626233250403637335864",
                "0.8750315711051332288940126721056249968388",
                "0.8806887547695161395663091706954032747057",
                "0.8862368972321044332087810546663117143783",
                "0.8916727264713805198101113970216017488088",
                "0.8969930089233786774728399592683654978873",
                "0.9021945620293219882753728764291623743640",
                "0.9072742675015344197203660348209478654736",
                "0.9122290852895683234194384187745394546044",
                "0.9170560682212008178015762539317496181817",
                "0.9217523772850016057919425732524047851744",
                "0.9263152975125503878404740065374190029756",
                "0.9307422544090793996146063410664013799528",
                "0.9350308308713359378724571900326600640405",
                "0.9391787845208065760617065570378706256375",
                "0.9431840653691303498041861312226470366231",
                "0.9470448337205698442792296487269911171780",
                "0.9507594782038307425642243864454478111924",
                "0.9543266338123530795148743853286784991066",
                "0.9577451998184802923022139437605873433398",
                "0.9610143574126934689610395719952584603082",
                "0.9641335869044371500681489377188918545040",
                "0.9671026843060320613811542880520830834016",
                "0.9699217771058582222876405787809699005449",
                "0.9725913390215090510713975773334379846037",
                "0.9751122035081006613473151836021749640435",
                "0.9774855757815431744700408591425399079901",
                "0.9797130431015612353990715916270401753817",
                "0.9817965830448673713797843583998633643822",
                "0.9837385694854996585093138796288337476366",
                "0.9855417759873879418304773486373389627312",
                "0.9872093763042884267779564711004317353850",
                "0.9887449416750577577235901732934205382272",
                "0.9901524345987484461134446735921283935349",
                "0.9914361987753521888166831825599544627244",
                "0.9926009449056243093779818586203348108089",
                "0.9936517320590359992980996052252416002614",
                "0.9945939443446214607120749043640203877477",
                "0.9954332626577978771826770212441895564692",
                "0.9961756313300103506198363449390531856519",
                "0.9968272195805246608761302993546287292108",
                "0.9973943777643662566057462266389262842638",
                "0.9978835885309015623329233767991787023754",
                "0.9983014131573239055753137592413985928641",
                "0.9986544335031850906675058761075883482291",
                "0.9989491902477215116132676754007689000822",
                "0.9991921183205925363978179838533652263361",
                "0.9993894807151088615084412897213089679162",
                "0.9995473021728349079679567827584490030482",
                "0.9996713045352122761142587430957457997769",
                "0.9997668458494211056684491494891554726386",
                "0.9998388655607971878892620909195084995731",
                "0.9998918382817212719102092150223300952628",
                "0.9999297386470252427794287492770080259812",
                "0.9999560195929541264674437621516984295421",
                "0.9999736059759906670171810990895538037755",
                "0.9999849047371774566183606165607314981029",
                "0.9999918318033809371048587987911948296037",
                "0.9999958546344369614120983543014096171730",
                "0.9999980478786838637189446301634092902531",
                "0.9999991581758261144715215960118928341563",
                "0.9999996730091539998191427165994201020389",
                "0.9999998879618181983697484346859598704347",
                "0.9999999670364383841063206129651690066454",
                "0.9999999919597829508828638561474025990259",
                "0.9999999984486696859949253022106357925655",
                "0.999999999777885769316725144117906761843",
                "0.999999999978422059441225571359566016631",
                "0.999999999998750162030582269915231573380",
                "0.999999999999964446647363959217917628973",
                "0.999999999999999633786845851316614830039",
                "0.999999999999999999178098723544059042809",
                "0.999999999999999999999838910107032619123",
                "0.999999999999999999999999999557063771617",
                "0.999999999999999999999999999999999999760",
                "1.000000000000000000000000000000000000000",
                "1.000000000000000000000000000000000000000",
                "1"
            };

            for ((int i, ddouble x) = (0, 0); i < expecteds.Length; i++, x += 1d / 256) {
                ddouble expected = expecteds[i];

                ddouble actual = ddouble.Bump(x);
                PrecisionAssert.AlmostEqual(expected, actual, 1e-31d, 1e-50, $"x = {x}");

                ddouble actual_dec = ddouble.Bump(ddouble.BitDecrement(x));
                PrecisionAssert.AlmostEqual(expected, actual_dec, 1e-31d, 1e-50, $"{x}-eps");

                ddouble actual_inc = ddouble.Bump(ddouble.BitIncrement(x));
                PrecisionAssert.AlmostEqual(expected, actual_inc, 1e-31d, 1e-50, $"{x}+eps");
            }
        }

        [TestMethod]
        public void BumpMonotoneTest() {
            for (ddouble x = 0; x <= 1; x += 1d / 16384) {
                ddouble yn = ddouble.Bump(x - Math.ScaleB(1, -32));
                ddouble yz = ddouble.Bump(x);
                ddouble yp = ddouble.Bump(x + Math.ScaleB(1, -32));

                Console.WriteLine($"x={x}\n{yz}");

                Assert.IsTrue(yn <= yz && yz <= yp, $"mlimit\n{yn},\n{yz},\n{yp}");
            }
        }

        [TestMethod]
        public void BumpAbnormalTest() {
            ddouble pg_pinf = ddouble.Bump(double.PositiveInfinity);
            ddouble pg_ninf = ddouble.Bump(double.NegativeInfinity);
            ddouble pg_nan = ddouble.Bump(double.NaN);

            PrecisionAssert.AreEqual(1, pg_pinf, nameof(pg_pinf));
            PrecisionAssert.IsPlusZero(pg_ninf, nameof(pg_ninf));
            PrecisionAssert.IsNaN(pg_nan, nameof(pg_nan));
        }
    }
}
