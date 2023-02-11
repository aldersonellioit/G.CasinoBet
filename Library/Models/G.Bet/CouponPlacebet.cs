using System;

namespace Models.G.CasinoBet
{
    public class CouponPlacebet
    {
        public Double Amount { get; set; }
        public String Product { get; set; }
        public String PlayerId { get; set; }
        public String PlayerName { get; set; }
        public Double Balance { get; set; }
    }
    public class CouponPlacebetRes
    {
        public string data { get; set; }
        public string error { get; set; }
        public int errorcode { get; set; }
    }
}
