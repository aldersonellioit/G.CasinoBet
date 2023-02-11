using System;
using System.ComponentModel.DataAnnotations;

namespace Models.G.CasinoBet
{
    public class Placebetfancy
    {
        [Range(1, Int64.MaxValue, ErrorMessage = "eventtypeid is empty.")]
        public Int64 etid { get; set; }
        [Range(1, Int64.MaxValue, ErrorMessage = "competitionid is empty.")]
        public Int64 cid { get; set; }
        [Range(1, Int64.MaxValue, ErrorMessage = "gameid is empty.")]
        public Int64 gmid { get; set; }
        //[Required(ErrorMessage = "usertype is empty.")]
        //public String utype { get; set; }
        public Int64 uid { get; set; }
        [Range(1, Int64.MaxValue, ErrorMessage = "marketid is empty.")]
        public Int64 mid { get; set; }
        [Range(1, Int64.MaxValue, ErrorMessage = "sectionid is empty.")]
        public Int64 sid { get; set; }
        [Range(1, Double.MaxValue, ErrorMessage = "bhav is empty.")]
        public Double bhav { get; set; }
        [Range(1, Double.MaxValue, ErrorMessage = "userrate is empty.")]
        public Double urate { get; set; }
        [Range(1, Double.MaxValue, ErrorMessage = "amount is empty.")]
        public Double amt { get; set; }
        //[Required(ErrorMessage = "subgametype is empty.")]
        public String sgtype { get; set; } = "";
        [Required(ErrorMessage = "bettype is empty.")]
        public String btype { get; set; }
        [Required(ErrorMessage = "gametype is empty.")]
        public String gtype { get; set; }
        [Required(ErrorMessage = "ipadress is empty.")]
        public String ip { get; set; }
        [Required(ErrorMessage = "browserdetail is empty.")]
        public String bdetail { get; set; }
        public String pid { get; set; }
        public String spcnt { get; set; }
    }
}
