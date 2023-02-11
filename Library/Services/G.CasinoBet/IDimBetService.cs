using Models.G.CasinoBet;
using System;
using System.Data;

namespace Services.G.CasinoBet
{
    public interface IDimBetService
    {
        DataSet GetDate();
        DataSet CheckWords(String par, String ip, String pname, String uid, String uguid);
        DataSet Placebetteen(Placebetteen placebetteen);
        DataSet Placebetbaccarat(Placebetteen placebetteen);
        DataSet Placebetlucky7(Placebetteen placebetteen);
        DataSet Placebetvlucky7(Placebetteen placebetteen);
        DataSet Placebetpoker(Placebetteen placebetteen);
        DataSet Placebetdt(Placebetteen placebetteen);
        DataSet Placebetvdt(Placebetteen placebetteen);
        DataSet Placebetbc(Placebetteen placebetteen);
        DataSet Placebetworli(Placebetworli placebetteen);
        DataSet Placebetother(Placebetteen placebetteen);
        DataSet Placebet3cardj(Placebet3cardj placebet3Cardj);
        DataSet Placebetsport(Placebetteen placebetteen);
        DataSet Placebetcard32(Placebetteen placebetteen);
        DataSet Placebetab(Placebetteen placebetteen);
        DataSet Placebetqueen(Placebetteen placebetteen);
        DataSet Placebetrace(Placebetteen placebetteen);
        DataSet Placebetlottery(Placebet3cardj placebet3Cardj);
        DataSet Placebetlotteryrep(Placebetlotteryrep placebetlotteryrep);
        DataSet Placebetlotterydtl(Placebetlotterydtl placebetlotterydtl);
        DataSet Placebettrap(Placebetteen placebetteen);
        DataSet Placebetpatti2(Placebetpatti2 placebetteen);
        DataSet Placebetnotenum(Placebetpatti2 placebetteen);
        DataSet Placebetkbc(Placebetkbc placebetteen);
        DataSet Placebetvteen(Placebetteen placebetteen);
        DataSet Placebetvcard32(Placebetteen placebetteen);
        DataSet Placebetvbc(Placebetteen placebetteen);
        DataSet Placebetvbaccarat(Placebetteen placebetteen);
        DataSet Placebetvqueen(Placebetteen placebetteen);
        DataSet Placebetvrace(Placebetteen placebetteen);
        DataSet Placebetvtrap(Placebetteen placebetteen);
        DataSet Placebetvpatti2(Placebetpatti2 placebetteen);
        DataSet Placebetvnotenum(Placebetpatti2 placebetteen);
    }
}
