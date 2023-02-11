using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.G.CasinoBet
{
    public class Placebetkbc
    {
        public Int64 uid { get; set; }
        [Range(1, Int64.MaxValue, ErrorMessage = "marketid is empty.")]
        public Int64 mid { get; set; }
        [Range(1, Double.MaxValue, ErrorMessage = "amount is empty.")]
        public Double amt { get; set; }

        [Required(ErrorMessage = "betjson is empty.")]
        public String bjson { get; set; }
        [Required(ErrorMessage = "gametype is empty.")]
        public String gtype { get; set; }
        [Required(ErrorMessage = "ipadress is empty.")]
        public String ip { get; set; }
        [Required(ErrorMessage = "browserdetail is empty.")]
        public String bdetail { get; set; }
        [Range(0, Int64.MaxValue, ErrorMessage = "marketid is empty.")]
        public Int64 bt { get; set; }
        public String pid { get; set; }
        //[Range(0, Double.MaxValue, ErrorMessage = "bhav is empty.")]
    }
    public class MyArray
    {
        public int sid { get; set; }
        public int ssid { get; set; }
    }

    public class Root
    {
        public List<MyArray> MyArray { get; set; }
    }
}
