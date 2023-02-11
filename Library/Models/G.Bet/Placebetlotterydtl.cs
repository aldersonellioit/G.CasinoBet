using System;
using System.ComponentModel.DataAnnotations;

namespace Models.G.CasinoBet
{
    public class Placebetlotterydtl
    {
        public Int64 uid { get; set; }
        [Range(1, Int64.MaxValue, ErrorMessage = "marketid is empty.")]
        public Int64 mid { get; set; }
        [Required(ErrorMessage = "gametype is empty.")]
        public String gtype { get; set; }
        [Required(ErrorMessage = "subgametype is empty.")]
        public String subtype { get; set; }
        [Required(ErrorMessage = "statement is empty.")]
        public String ste { get; set; }
    }
}
