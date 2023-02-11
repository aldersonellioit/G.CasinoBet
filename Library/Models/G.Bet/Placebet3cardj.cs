using System;
using System.ComponentModel.DataAnnotations;

namespace Models.G.CasinoBet
{
    public class Placebet3cardj
    {
        [Required(ErrorMessage = "subtype is empty.")]
        public String subtype { get; set; }
        public Int64 uid { get; set; }
        [Range(1, Int64.MaxValue, ErrorMessage = "marketid is empty.")]
        public Int64 mid { get; set; }
        [Range(1, Int64.MaxValue, ErrorMessage = "sectionid is empty.")]
        public Int64 sid { get; set; }
        [Range(0, Int32.MaxValue, ErrorMessage = "subsectionid is empty.")]
        public Int32 subid { get; set; }
        [Range(0, Double.MaxValue, ErrorMessage = "userrate is empty.")]
        public Double urate { get; set; }
        [Range(1, Double.MaxValue, ErrorMessage = "amount is empty.")]
        public Double amt { get; set; }
        [Required(ErrorMessage = "bettype is empty.")]
        public String btype { get; set; }
        [Required(ErrorMessage = "gametype is empty.")]
        public String gtype { get; set; }
        [Required(ErrorMessage = "ipadress is empty.")]
        public String ip { get; set; }
        [Required(ErrorMessage = "browserdetail is empty.")]
        public String bdetail { get; set; }
        [Required(ErrorMessage = "usercard is empty.")]
        public String ucard { get; set; }
        public String pid { get; set; }
    }
}
