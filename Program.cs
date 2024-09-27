﻿using System.Numerics;
using System.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Tools;
using Mpir.NET;

class Program
{
    static void Main(string[] args)
    {
        gmp_randstate_t tmp = new gmp_randstate_t();

        tmp.

        RSA rsa = new RSA();

        //byte[] data = new byte[tmp.Length];

        //BigInteger qwe = BigInteger.Parse(tmp);
        //Console.WriteLine(IsTheRealPrime(qwe));

        //var p = rsa.GetPrimeBigInteger();
        //Console.WriteLine(Encoding.Default.GetString(p.ToByteArray()));

        //var q = rsa.GetPrimeBigInteger();
        //Console.WriteLine(Encoding.Default.GetString(q.ToByteArray()));

        //StreebogTest();
    }

    static bool IsTheRealPrime(BigInteger number)
    {
        for (UInt64 i = 2; i < number; i++)
            if (number % i == 0 && i != number)
                return false;
        
        return true;
    }

    static void StreebogTest()
    {
        string tmp = "";
        byte[] message =
        {
            0x32,0x31,0x30,0x39,0x38,0x37,0x36,0x35,0x34,0x33,0x32,0x31,0x30,0x39,0x38,0x37,
            0x36,0x35,0x34,0x33,0x32,0x31,0x30,0x39,0x38,0x37,0x36,0x35,0x34,0x33,0x32,0x31,
            0x30,0x39,0x38,0x37,0x36,0x35,0x34,0x33,0x32,0x31,0x30,0x39,0x38,0x37,0x36,0x35,
            0x34,0x33,0x32,0x31,0x30,0x39,0x38,0x37,0x36,0x35,0x34,0x33,0x32,0x31,0x30
        };
        tmp = Encoding.Default.GetString(message);

        Console.WriteLine(new Streebog().GetHash(message));
        Console.WriteLine(new Streebog().GetHash(tmp));
    }

    static void TestGovna() 
    {
        List<string> list = new List<string>();
        list.Add("323223591540165516656364027973222668247561493358688270406215698588306139646170858367778563002750919483843145264269687484273072289026754993162141580220768068384368539480993741814402995359298797644613369230283016244802409612665324121004529205809843143219243575585831061629923046102492491184519957521223360446616511005030483077009522873926126862089147084430528638453949155150941825122575632426927906049514886455860705413646862895736301032346614773404438790311278838513826403998829127101733829021017445898491939903655388164756976796731877575404695014573327856595391959940288158264903318776887495176773089134521014027462561743202497899770662413583310025863712059507411789507198462399908150154735212818996502855717718722825469664471465402218049276947952475172080403441763189000171088971345504393954005726011531945322009315174765944436707154383800111412627816522417910091258617502804173358066568954259253111283874352058818472320849065488312125120232956151302998930367457552294931586839876879148951537778688807361888631906662307924938357540214771084430370691041746856724735680918225443070496835360506241844015425466302227840254093139923547114705218435453498379315418161236695966361691178781808708008576806877909950356903174776391603905533407");
        list.Add("320798535914298485254405651644251795426342938376796856243224850941653944619764069550694861570839214256745546562692152235702363942783932119767215816579515062462018028351477041643047169971718038051884882360586180721728925927202029135525282137933938605354151504150120750856707945202188272156711861235814216803850500661035125659488803458479381346017376738080149314363478301671384980553161345629789702199365908420986637438437336464839489335758993161694313058844816767396331378451017481866660547453099021529801593188007862560149857445411748133286568635575864167462097302230578974302716085771652377404696232763052340936149654808599046155427902651709999495792201232642130888607233085571431932767999327748330027598798218907085066388330934663154923748439435658407659533822711916968685790436010381717334542357775627485565974093556344450035625223465237178976424794931536875146090860226880270930012750748600894064758918401224392383887182620888818893763911353418795396198153159703509921660237960703710651864273858611423687000303510064614835020458719670605528447925575071911580491421478077772090999431937306739596198199843137809215908116293377598423850732133177874434875319385693038101784119918253265216148604645331157252340222162559812482569545177");
        list.Add("4724801837867943945306124224462818616208049684807436313591477108145365826709061580333994636397053842711236923061633456539187544219309068184695669013009909158503869455806188303362193326854415890272867548307290622990834925810708768578844012985413044964057903293796546017748923990224938572292830921238268259587379987502147268813034972056654382770003200086031063387619355524422838879910628519472544649942062860636157331316172001692230499689082393217084482065947171015225571289988552771025450796155481491066559100058950587609771395105766786966530445414935367039897762191453319366795596998568517737068094271730079845829316739663416380593524758313975530061131281008564314714373105980960098368702211460085717051318232194773118576075591003915043619220122842427419727658821825930844549016038858499050240568650722323335454181546326421993473594671400035055090248193671199041952444208643267459891851510435930679821936057856831109926957444191352578135003410420376072024033862799815273434494233334192563355870379819984763812977460477798469977915720783150755784174686123609539564901933513204049595845092307708878397828912358787949458922725577021310687584515208580936170032728173956599526520611980839451661212927539915042960207556846610809485089353");
        list.Add("368468983608249904065871905083459526990296448336454619598770747354953855209855201570161358794075595947955363632263031251586059252140424488278695767542057284318161906351532241461813250270379606648770128767306028932919928238144357805965632720162483726933140511824323473615642454082792440835883784088149638444084491930287416737195365409217166749637457265188330983139290891098418634540892951783908783060154432912910923963067309161673599742023910960196652661566637243266992118604334238411092566813861070253690095987387974287776081295179286683507740545158708618721280796462441655958818526916243696665044457063264553136577275994176745865307743389163218112501746103751681739708021380260096415050766800048938261228734815826462182014109382194533314621953820639913023975544969017099209568034351761589841199896985307005643303827444895045553761811756359955018786995856879351311357129120988523405426887730033499772857590351531816086985290293970650895093114451441019647691765200187709648557461978806271744909134654779873360891880508825293917768608180084635270523008572985381920981033341768153983235084920422238395969189595223530023395761622879768132251557528582714419488644539313889995987081093807703045285904862118668944201954146131089517496867471");

        foreach (string s in list)
        {
            Console.WriteLine("IsPrime? --> " + IsTheRealPrime(BigInteger.Parse(s)));
        }
    
    }
}
