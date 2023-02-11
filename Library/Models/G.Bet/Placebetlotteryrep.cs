using System;
using System.ComponentModel.DataAnnotations;

namespace Models.G.CasinoBet
{
    public class Placebetlotteryrep
    {
        public Int64 uid { get; set; }
        [Range(1, Int64.MaxValue, ErrorMessage = "marketid is empty.")]
        public Int64 mid { get; set; }
        [Required(ErrorMessage = "gametype is empty.")]
        public String gtype { get; set; }
        [Required(ErrorMessage = "ipadress is empty.")]
        public String ip { get; set; }
        [Required(ErrorMessage = "browserdetail is empty.")]
        public String bdetail { get; set; }
    }
}
