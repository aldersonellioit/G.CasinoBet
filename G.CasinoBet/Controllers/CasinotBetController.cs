using ELog;
using G.CasinoBet.AutherizationAtteributes;
using G.CasinoBet.Providers;
using Models.G.CasinoBet;
using Newtonsoft.Json;
using Services.G.CasinoBet;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace G.CasinoBet.Controllers
{
    [ModelValid]
    [RoutePrefix("api/Casino")]
    public class CasinotBetController : ApiController
    {
        #region Fields

        private readonly IDimBetService _betservice;
        #endregion

        #region Ctor

        public CasinotBetController(IDimBetService betservice)
        {
            _betservice = betservice;
        }

        #endregion

        #region Methods

        public HttpResponseMessage Return200(String Msg, Object Data = null)
        {
            if (Data == null)
                return Request.CreateResponse(HttpStatusCode.OK, new { status = 200, msg = Msg });
            return Request.CreateResponse(HttpStatusCode.OK, new { status = 200, msg = Msg, data = Data });
        }
        public HttpResponseMessage Return400(String Msg)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new { status = 300, msg = Msg });
        }
        public HttpResponseMessage Return401(String Msg)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new { status = 401, msg = Msg });
        }
        [HttpGet]
        [Route("getdate", Name = "getdate")]
        public HttpResponseMessage GetDate()
        {
            try
            {
                ErrorLog.WriteLogAll("getdate", "");
                var Response = _betservice.GetDate();
                if (Response != null && Response.Tables.Count > 0 && Response.Tables[Response.Tables.Count - 1].Columns.Contains("id") && Response.Tables[Response.Tables.Count - 1].Rows[0]["id"].ToString() == "-1")
                {
                    ErrorLog.WriteLog("getdate", "", Response.Tables[Response.Tables.Count - 1].Rows[0]["MSG"].ToString());
                    return Return400("Data Error");
                }
                if (Response == null || Response.Tables.Count <= 0)
                    return Return400("No Data Found.");
                if (Response.Tables[0].Rows.Count <= 0)
                    return Return400("No Record Found.");
                DateTime dt = Convert.ToDateTime(Response.Tables[0].Rows[0]["datetime"]);
                var tzone = Response.Tables[0].Rows[0]["timezone"] == null ? "" : Response.Tables[0].Rows[0]["timezone"];

                return Return200("success", new
                {
                    dt = dt.ToString("ddd/yyyy/MM/dd/HH/mm/ss/fff"),
                    tzone = tzone,
                });
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("GetDate : " + ex.ToString(), "");
                return Return400("Server Error");
            }
        }
        [HttpPost]
        [UserAuth, KeyFilter]
        [Route("placebetteen")]
        public HttpResponseMessage Placebetteen([FromBody] Placebetteen placebetteen)
        {
            try
            {
                ErrorLog.WriteLogAll("Placebetteen", JsonConvert.SerializeObject(placebetteen));

                placebetteen.btype = placebetteen.btype.ToLower();

                string u_id = string.Empty;
                var header = HttpContext.Current.Request.Headers["Authorization"].ToString();
                if (header != null)
                {
                    u_id = JwtTokenGenerator.GetSaGuid(header);
                }

                Guid uu_id;
                bool res = Guid.TryParse(u_id, out uu_id);

                placebetteen.uid = Convert.ToInt64(JwtTokenGenerator.GetUid(header));
                placebetteen.pid = u_id.ToString();
                if (!res)
                    return Return400("token not valid.");
                var Response = _betservice.Placebetteen(placebetteen);

                if (Response != null && Response.Tables.Count > 0 && Response.Tables[Response.Tables.Count - 1].Columns.Contains("id") && Response.Tables[Response.Tables.Count - 1].Rows[0]["id"].ToString() == "-1")
                {
                    ErrorLog.WriteLog("Placebetteen", JsonConvert.SerializeObject(placebetteen), Response.Tables[Response.Tables.Count - 1].Rows[0]["MSG"].ToString());
                    return Return400("Data Error");
                }
                if (Response == null || Response.Tables.Count <= 0)
                    return Return400("No Data Found.");
                if (Response.Tables[0].Rows.Count <= 0)
                    return Return400("No Record Found.");
                if (Response.Tables[0].Columns.Contains("id") && Response.Tables[0].Rows[0]["id"].ToString() == "0")
                    return Return400(Response.Tables[0].Rows[0]["MSG"].ToString());

                return Return200(Response.Tables[0].Rows[0]["MSG"].ToString());
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Placebetteen : " + ex.ToString(), " : Req" + JsonConvert.SerializeObject(placebetteen));
                return Return400("Server Error");
            }
        }
        [HttpPost]
        [UserAuth, KeyFilter]
        [Route("placebetbaccarat")]
        public HttpResponseMessage Placebetbaccarat([FromBody] Placebetteen placebetteen)
        {
            try
            {
                ErrorLog.WriteLogAll("Placebetbaccarat", JsonConvert.SerializeObject(placebetteen));

                placebetteen.btype = placebetteen.btype.ToLower();

                string u_id = string.Empty;
                var header = HttpContext.Current.Request.Headers["Authorization"].ToString();
                if (header != null)
                {
                    u_id = JwtTokenGenerator.GetSaGuid(header);
                }

                Guid uu_id;
                bool res = Guid.TryParse(u_id, out uu_id);

                placebetteen.uid = Convert.ToInt64(JwtTokenGenerator.GetUid(header));
                placebetteen.pid = u_id.ToString();
                if (!res)
                    return Return400("token not valid.");
                var Response = _betservice.Placebetbaccarat(placebetteen);

                if (Response != null && Response.Tables.Count > 0 && Response.Tables[Response.Tables.Count - 1].Columns.Contains("id") && Response.Tables[Response.Tables.Count - 1].Rows[0]["id"].ToString() == "-1")
                {
                    ErrorLog.WriteLog("Placebetbaccarat", JsonConvert.SerializeObject(placebetteen), Response.Tables[Response.Tables.Count - 1].Rows[0]["MSG"].ToString());
                    return Return400("Data Error");
                }
                if (Response == null || Response.Tables.Count <= 0)
                    return Return400("No Data Found.");
                if (Response.Tables[0].Rows.Count <= 0)
                    return Return400("No Record Found.");
                if (Response.Tables[0].Columns.Contains("id") && Response.Tables[0].Rows[0]["id"].ToString() == "0")
                    return Return400(Response.Tables[0].Rows[0]["MSG"].ToString());

                return Return200(Response.Tables[0].Rows[0]["MSG"].ToString());
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Placebetbaccarat : " + ex.ToString(), " : Req" + JsonConvert.SerializeObject(placebetteen));
                return Return400("Server Error");
            }
        }
        [HttpPost]
        [UserAuth, KeyFilter]
        [Route("placebetlucky7")]
        public HttpResponseMessage Placebetlucky7([FromBody] Placebetteen placebetteen)
        {
            try
            {
                ErrorLog.WriteLogAll("Placebetlucky7", JsonConvert.SerializeObject(placebetteen));

                placebetteen.btype = placebetteen.btype.ToLower();

                string u_id = string.Empty;
                var header = HttpContext.Current.Request.Headers["Authorization"].ToString();
                if (header != null)
                {
                    u_id = JwtTokenGenerator.GetSaGuid(header);
                }

                Guid uu_id;
                bool res = Guid.TryParse(u_id, out uu_id);

                placebetteen.uid = Convert.ToInt64(JwtTokenGenerator.GetUid(header));
                placebetteen.pid = u_id.ToString();
                if (!res)
                    return Return400("token not valid.");
                var Response = _betservice.Placebetlucky7(placebetteen);

                if (Response != null && Response.Tables.Count > 0 && Response.Tables[Response.Tables.Count - 1].Columns.Contains("id") && Response.Tables[Response.Tables.Count - 1].Rows[0]["id"].ToString() == "-1")
                {
                    ErrorLog.WriteLog("Placebetlucky7", JsonConvert.SerializeObject(placebetteen), Response.Tables[Response.Tables.Count - 1].Rows[0]["MSG"].ToString());
                    return Return400("Data Error");
                }
                if (Response == null || Response.Tables.Count <= 0)
                    return Return400("No Data Found.");
                if (Response.Tables[0].Rows.Count <= 0)
                    return Return400("No Record Found.");
                if (Response.Tables[0].Columns.Contains("id") && Response.Tables[0].Rows[0]["id"].ToString() == "0")
                    return Return400(Response.Tables[0].Rows[0]["MSG"].ToString());

                return Return200(Response.Tables[0].Rows[0]["MSG"].ToString());
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Placebetlucky7 : " + ex.ToString(), " : Req" + JsonConvert.SerializeObject(placebetteen));
                return Return400("Server Error");
            }
        }
        [HttpPost]
        [UserAuth, KeyFilter]
        [Route("placebetvlucky7")]
        public HttpResponseMessage Placebetvlucky7([FromBody] Placebetteen placebetteen)
        {
            try
            {
                ErrorLog.WriteLogAll("Placebetvlucky7", JsonConvert.SerializeObject(placebetteen));

                placebetteen.btype = placebetteen.btype.ToLower();

                string u_id = string.Empty;
                var header = HttpContext.Current.Request.Headers["Authorization"].ToString();
                if (header != null)
                {
                    u_id = JwtTokenGenerator.GetSaGuid(header);
                }

                Guid uu_id;
                bool res = Guid.TryParse(u_id, out uu_id);

                placebetteen.uid = Convert.ToInt64(JwtTokenGenerator.GetUid(header));
                placebetteen.pid = u_id.ToString();
                if (!res)
                    return Return400("token not valid.");
                var Response = _betservice.Placebetvlucky7(placebetteen);

                if (Response != null && Response.Tables.Count > 0 && Response.Tables[Response.Tables.Count - 1].Columns.Contains("id") && Response.Tables[Response.Tables.Count - 1].Rows[0]["id"].ToString() == "-1")
                {
                    ErrorLog.WriteLog("Placebetvlucky7", JsonConvert.SerializeObject(placebetteen), Response.Tables[Response.Tables.Count - 1].Rows[0]["MSG"].ToString());
                    return Return400("Data Error");
                }
                if (Response == null || Response.Tables.Count <= 0)
                    return Return400("No Data Found.");
                if (Response.Tables[0].Rows.Count <= 0)
                    return Return400("No Record Found.");
                if (Response.Tables[0].Columns.Contains("id") && Response.Tables[0].Rows[0]["id"].ToString() == "0")
                    return Return400(Response.Tables[0].Rows[0]["MSG"].ToString());

                return Return200(Response.Tables[0].Rows[0]["MSG"].ToString());
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Placebetvlucky7 : " + ex.ToString(), " : Req" + JsonConvert.SerializeObject(placebetteen));
                return Return400("Server Error");
            }
        }
        [HttpPost]
        [UserAuth, KeyFilter]
        [Route("placebetpoker")]
        public HttpResponseMessage Placebetpoker([FromBody] Placebetteen placebetteen)
        {
            try
            {
                ErrorLog.WriteLogAll("Placebetpoker", JsonConvert.SerializeObject(placebetteen));

                placebetteen.btype = placebetteen.btype.ToLower();

                string u_id = string.Empty;
                var header = HttpContext.Current.Request.Headers["Authorization"].ToString();
                if (header != null)
                {
                    u_id = JwtTokenGenerator.GetSaGuid(header);
                }

                Guid uu_id;
                bool res = Guid.TryParse(u_id, out uu_id);

                placebetteen.uid = Convert.ToInt64(JwtTokenGenerator.GetUid(header));
                placebetteen.pid = u_id.ToString();
                if (!res)
                    return Return400("token not valid.");
                var Response = _betservice.Placebetpoker(placebetteen);

                if (Response != null && Response.Tables.Count > 0 && Response.Tables[Response.Tables.Count - 1].Columns.Contains("id") && Response.Tables[Response.Tables.Count - 1].Rows[0]["id"].ToString() == "-1")
                {
                    ErrorLog.WriteLog("Placebetpoker", JsonConvert.SerializeObject(placebetteen), Response.Tables[Response.Tables.Count - 1].Rows[0]["MSG"].ToString());
                    return Return400("Data Error");
                }
                if (Response == null || Response.Tables.Count <= 0)
                    return Return400("No Data Found.");
                if (Response.Tables[0].Rows.Count <= 0)
                    return Return400("No Record Found.");
                if (Response.Tables[0].Columns.Contains("id") && Response.Tables[0].Rows[0]["id"].ToString() == "0")
                    return Return400(Response.Tables[0].Rows[0]["MSG"].ToString());

                return Return200(Response.Tables[0].Rows[0]["MSG"].ToString());
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Placebetpoker : " + ex.ToString(), " : Req" + JsonConvert.SerializeObject(placebetteen));
                return Return400("Server Error");
            }
        }
        [HttpPost]
        [UserAuth, KeyFilter]
        [Route("placebetdt")]
        public HttpResponseMessage Placebetdt([FromBody] Placebetteen placebetteen)
        {
            try
            {
                ErrorLog.WriteLogAll("Placebetdt", JsonConvert.SerializeObject(placebetteen));

                placebetteen.btype = placebetteen.btype.ToLower();

                string u_id = string.Empty;
                var header = HttpContext.Current.Request.Headers["Authorization"].ToString();
                if (header != null)
                {
                    u_id = JwtTokenGenerator.GetSaGuid(header);
                }

                Guid uu_id;
                bool res = Guid.TryParse(u_id, out uu_id);

                placebetteen.uid = Convert.ToInt64(JwtTokenGenerator.GetUid(header));
                placebetteen.pid = u_id.ToString();
                if (!res)
                    return Return400("token not valid.");
                var Response = _betservice.Placebetdt(placebetteen);

                if (Response != null && Response.Tables.Count > 0 && Response.Tables[Response.Tables.Count - 1].Columns.Contains("id") && Response.Tables[Response.Tables.Count - 1].Rows[0]["id"].ToString() == "-1")
                {
                    ErrorLog.WriteLog("Placebetdt", JsonConvert.SerializeObject(placebetteen), Response.Tables[Response.Tables.Count - 1].Rows[0]["MSG"].ToString());
                    return Return400("Data Error");
                }
                if (Response == null || Response.Tables.Count <= 0)
                    return Return400("No Data Found.");
                if (Response.Tables[0].Rows.Count <= 0)
                    return Return400("No Record Found.");
                if (Response.Tables[0].Columns.Contains("id") && Response.Tables[0].Rows[0]["id"].ToString() == "0")
                    return Return400(Response.Tables[0].Rows[0]["MSG"].ToString());

                return Return200(Response.Tables[0].Rows[0]["MSG"].ToString());
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Placebetdt : " + ex.ToString(), " : Req" + JsonConvert.SerializeObject(placebetteen));
                return Return400("Server Error");
            }
        }
        [HttpPost]
        [UserAuth, KeyFilter]
        [Route("placebetvdt")]
        public HttpResponseMessage Placebetvdt([FromBody] Placebetteen placebetteen)
        {
            try
            {
                ErrorLog.WriteLogAll("Placebetvdt", JsonConvert.SerializeObject(placebetteen));

                placebetteen.btype = placebetteen.btype.ToLower();

                string u_id = string.Empty;
                var header = HttpContext.Current.Request.Headers["Authorization"].ToString();
                if (header != null)
                {
                    u_id = JwtTokenGenerator.GetSaGuid(header);
                }

                Guid uu_id;
                bool res = Guid.TryParse(u_id, out uu_id);

                placebetteen.uid = Convert.ToInt64(JwtTokenGenerator.GetUid(header));
                placebetteen.pid = u_id.ToString();
                if (!res)
                    return Return400("token not valid.");
                var Response = _betservice.Placebetvdt(placebetteen);

                if (Response != null && Response.Tables.Count > 0 && Response.Tables[Response.Tables.Count - 1].Columns.Contains("id") && Response.Tables[Response.Tables.Count - 1].Rows[0]["id"].ToString() == "-1")
                {
                    ErrorLog.WriteLog("Placebetvdt", JsonConvert.SerializeObject(placebetteen), Response.Tables[Response.Tables.Count - 1].Rows[0]["MSG"].ToString());
                    return Return400("Data Error");
                }
                if (Response == null || Response.Tables.Count <= 0)
                    return Return400("No Data Found.");
                if (Response.Tables[0].Rows.Count <= 0)
                    return Return400("No Record Found.");
                if (Response.Tables[0].Columns.Contains("id") && Response.Tables[0].Rows[0]["id"].ToString() == "0")
                    return Return400(Response.Tables[0].Rows[0]["MSG"].ToString());

                return Return200(Response.Tables[0].Rows[0]["MSG"].ToString());
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Placebetvdt : " + ex.ToString(), " : Req" + JsonConvert.SerializeObject(placebetteen));
                return Return400("Server Error");
            }
        }
        [HttpPost]
        [UserAuth, KeyFilter]
        [Route("placebetbc")]
        public HttpResponseMessage Placebetbc([FromBody] Placebetteen placebetteen)
        {
            try
            {
                ErrorLog.WriteLogAll("Placebetbc", JsonConvert.SerializeObject(placebetteen));

                placebetteen.btype = placebetteen.btype.ToLower();

                string u_id = string.Empty;
                var header = HttpContext.Current.Request.Headers["Authorization"].ToString();
                if (header != null)
                {
                    u_id = JwtTokenGenerator.GetSaGuid(header);
                }

                Guid uu_id;
                bool res = Guid.TryParse(u_id, out uu_id);

                placebetteen.uid = Convert.ToInt64(JwtTokenGenerator.GetUid(header));
                placebetteen.pid = u_id.ToString();
                if (!res)
                    return Return400("token not valid.");
                var Response = _betservice.Placebetbc(placebetteen);

                if (Response != null && Response.Tables.Count > 0 && Response.Tables[Response.Tables.Count - 1].Columns.Contains("id") && Response.Tables[Response.Tables.Count - 1].Rows[0]["id"].ToString() == "-1")
                {
                    ErrorLog.WriteLog("Placebetbc", JsonConvert.SerializeObject(placebetteen), Response.Tables[Response.Tables.Count - 1].Rows[0]["MSG"].ToString());
                    return Return400("Data Error");
                }
                if (Response == null || Response.Tables.Count <= 0)
                    return Return400("No Data Found.");
                if (Response.Tables[0].Rows.Count <= 0)
                    return Return400("No Record Found.");
                if (Response.Tables[0].Columns.Contains("id") && Response.Tables[0].Rows[0]["id"].ToString() == "0")
                    return Return400(Response.Tables[0].Rows[0]["MSG"].ToString());

                return Return200(Response.Tables[0].Rows[0]["MSG"].ToString());
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Placebetbc : " + ex.ToString(), " : Req" + JsonConvert.SerializeObject(placebetteen));
                return Return400("Server Error");
            }
        }
        [HttpPost]
        [UserAuth, KeyFilter]
        [Route("placebetworli")]
        public HttpResponseMessage Placebetworli([FromBody] Placebetworli placebetteen)
        {
            try
            {
                ErrorLog.WriteLogAll("Placebetworli", JsonConvert.SerializeObject(placebetteen));

                placebetteen.btype = placebetteen.btype.ToLower();

                string u_id = string.Empty;
                var header = HttpContext.Current.Request.Headers["Authorization"].ToString();
                if (header != null)
                {
                    u_id = JwtTokenGenerator.GetSaGuid(header);
                }

                Guid uu_id;
                bool res = Guid.TryParse(u_id, out uu_id);

                placebetteen.uid = Convert.ToInt64(JwtTokenGenerator.GetUid(header));
                placebetteen.pid = u_id.ToString();
                if (!res)
                    return Return400("token not valid.");
                var Response = _betservice.Placebetworli(placebetteen);

                if (Response != null && Response.Tables.Count > 0 && Response.Tables[Response.Tables.Count - 1].Columns.Contains("id") && Response.Tables[Response.Tables.Count - 1].Rows[0]["id"].ToString() == "-1")
                {
                    ErrorLog.WriteLog("Placebetworli", JsonConvert.SerializeObject(placebetteen), Response.Tables[Response.Tables.Count - 1].Rows[0]["MSG"].ToString());
                    return Return400("Data Error");
                }
                if (Response == null || Response.Tables.Count <= 0)
                    return Return400("No Data Found.");
                if (Response.Tables[0].Rows.Count <= 0)
                    return Return400("No Record Found.");
                if (Response.Tables[0].Columns.Contains("id") && Response.Tables[0].Rows[0]["id"].ToString() == "0")
                    return Return400(Response.Tables[0].Rows[0]["MSG"].ToString());

                return Return200(Response.Tables[0].Rows[0]["MSG"].ToString());
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Placebetworli : " + ex.ToString(), " : Req" + JsonConvert.SerializeObject(placebetteen));
                return Return400("Server Error");
            }
        }
        [HttpPost]
        [UserAuth, KeyFilter]
        [Route("placebetother")]
        public HttpResponseMessage Placebetother([FromBody] Placebetteen placebetteen)
        {
            try
            {
                ErrorLog.WriteLogAll("Placebetother", JsonConvert.SerializeObject(placebetteen));

                placebetteen.btype = placebetteen.btype.ToLower();

                string u_id = string.Empty;
                var header = HttpContext.Current.Request.Headers["Authorization"].ToString();
                if (header != null)
                {
                    u_id = JwtTokenGenerator.GetSaGuid(header);
                }

                Guid uu_id;
                bool res = Guid.TryParse(u_id, out uu_id);

                placebetteen.uid = Convert.ToInt64(JwtTokenGenerator.GetUid(header));
                placebetteen.pid = u_id.ToString();
                if (!res)
                    return Return400("token not valid.");
                var Response = _betservice.Placebetother(placebetteen);

                if (Response != null && Response.Tables.Count > 0 && Response.Tables[Response.Tables.Count - 1].Columns.Contains("id") && Response.Tables[Response.Tables.Count - 1].Rows[0]["id"].ToString() == "-1")
                {
                    ErrorLog.WriteLog("Placebetother", JsonConvert.SerializeObject(placebetteen), Response.Tables[Response.Tables.Count - 1].Rows[0]["MSG"].ToString());
                    return Return400("Data Error");
                }
                if (Response == null || Response.Tables.Count <= 0)
                    return Return400("No Data Found.");
                if (Response.Tables[0].Rows.Count <= 0)
                    return Return400("No Record Found.");
                if (Response.Tables[0].Columns.Contains("id") && Response.Tables[0].Rows[0]["id"].ToString() == "0")
                    return Return400(Response.Tables[0].Rows[0]["MSG"].ToString());

                return Return200(Response.Tables[0].Rows[0]["MSG"].ToString());
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Placebetother : " + ex.ToString(), " : Req" + JsonConvert.SerializeObject(placebetteen));
                return Return400("Server Error");
            }
        }
        [HttpPost]
        [UserAuth, KeyFilter]
        [Route("placebet3cardj")]
        public HttpResponseMessage Placebet3cardj([FromBody] Placebet3cardj placebet3Cardj)
        {
            try
            {
                ErrorLog.WriteLogAll("Placebet3cardj", JsonConvert.SerializeObject(placebet3Cardj));

                placebet3Cardj.btype = placebet3Cardj.btype.ToLower();

                string u_id = string.Empty;
                var header = HttpContext.Current.Request.Headers["Authorization"].ToString();
                if (header != null)
                {
                    u_id = JwtTokenGenerator.GetSaGuid(header);
                }

                Guid uu_id;
                bool res = Guid.TryParse(u_id, out uu_id);

                placebet3Cardj.uid = Convert.ToInt64(JwtTokenGenerator.GetUid(header));
                placebet3Cardj.pid = u_id.ToString();
                if (!res)
                    return Return400("token not valid.");
                var Response = _betservice.Placebet3cardj(placebet3Cardj);

                if (Response != null && Response.Tables.Count > 0 && Response.Tables[Response.Tables.Count - 1].Columns.Contains("id") && Response.Tables[Response.Tables.Count - 1].Rows[0]["id"].ToString() == "-1")
                {
                    ErrorLog.WriteLog("Placebet3cardj", JsonConvert.SerializeObject(placebet3Cardj), Response.Tables[Response.Tables.Count - 1].Rows[0]["MSG"].ToString());
                    return Return400("Data Error");
                }
                if (Response == null || Response.Tables.Count <= 0)
                    return Return400("No Data Found.");
                if (Response.Tables[0].Rows.Count <= 0)
                    return Return400("No Record Found.");
                if (Response.Tables[0].Columns.Contains("id") && Response.Tables[0].Rows[0]["id"].ToString() == "0")
                    return Return400(Response.Tables[0].Rows[0]["MSG"].ToString());

                return Return200(Response.Tables[0].Rows[0]["MSG"].ToString());
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Placebet3cardj : " + ex.ToString(), " : Req" + JsonConvert.SerializeObject(placebet3Cardj));
                return Return400("Server Error");
            }
        }
        [HttpPost]
        [UserAuth, KeyFilter]
        [Route("placebetsport")]
        public HttpResponseMessage Placebetsport([FromBody] Placebetteen placebetteen)
        {
            try
            {
                ErrorLog.WriteLogAll("Placebetsport", JsonConvert.SerializeObject(placebetteen));

                placebetteen.btype = placebetteen.btype.ToLower();

                string u_id = string.Empty;
                var header = HttpContext.Current.Request.Headers["Authorization"].ToString();
                if (header != null)
                {
                    u_id = JwtTokenGenerator.GetSaGuid(header);
                }

                Guid uu_id;
                bool res = Guid.TryParse(u_id, out uu_id);

                placebetteen.uid = Convert.ToInt64(JwtTokenGenerator.GetUid(header));
                placebetteen.pid = u_id.ToString();
                if (!res)
                    return Return400("token not valid.");
                var Response = _betservice.Placebetsport(placebetteen);

                if (Response != null && Response.Tables.Count > 0 && Response.Tables[Response.Tables.Count - 1].Columns.Contains("id") && Response.Tables[Response.Tables.Count - 1].Rows[0]["id"].ToString() == "-1")
                {
                    ErrorLog.WriteLog("Placebetsport", JsonConvert.SerializeObject(placebetteen), Response.Tables[Response.Tables.Count - 1].Rows[0]["MSG"].ToString());
                    return Return400("Data Error");
                }
                if (Response == null || Response.Tables.Count <= 0)
                    return Return400("No Data Found.");
                if (Response.Tables[0].Rows.Count <= 0)
                    return Return400("No Record Found.");
                if (Response.Tables[0].Columns.Contains("id") && Response.Tables[0].Rows[0]["id"].ToString() == "0")
                    return Return400(Response.Tables[0].Rows[0]["MSG"].ToString());

                return Return200(Response.Tables[0].Rows[0]["MSG"].ToString());
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Placebetsport : " + ex.ToString(), " : Req" + JsonConvert.SerializeObject(placebetteen));
                return Return400("Server Error");
            }
        }
        [HttpPost]
        [UserAuth, KeyFilter]
        [Route("placebetcard32")]
        public HttpResponseMessage Placebetcard32([FromBody] Placebetteen placebetteen)
        {
            try
            {
                ErrorLog.WriteLogAll("Placebetcard32", JsonConvert.SerializeObject(placebetteen));

                placebetteen.btype = placebetteen.btype.ToLower();

                string u_id = string.Empty;
                var header = HttpContext.Current.Request.Headers["Authorization"].ToString();
                if (header != null)
                {
                    u_id = JwtTokenGenerator.GetSaGuid(header);
                }

                Guid uu_id;
                bool res = Guid.TryParse(u_id, out uu_id);

                placebetteen.uid = Convert.ToInt64(JwtTokenGenerator.GetUid(header));
                placebetteen.pid = u_id.ToString();
                if (!res)
                    return Return400("token not valid.");
                var Response = _betservice.Placebetcard32(placebetteen);

                if (Response != null && Response.Tables.Count > 0 && Response.Tables[Response.Tables.Count - 1].Columns.Contains("id") && Response.Tables[Response.Tables.Count - 1].Rows[0]["id"].ToString() == "-1")
                {
                    ErrorLog.WriteLog("Placebetcard32", JsonConvert.SerializeObject(placebetteen), Response.Tables[Response.Tables.Count - 1].Rows[0]["MSG"].ToString());
                    return Return400("Data Error");
                }
                if (Response == null || Response.Tables.Count <= 0)
                    return Return400("No Data Found.");
                if (Response.Tables[0].Rows.Count <= 0)
                    return Return400("No Record Found.");
                if (Response.Tables[0].Columns.Contains("id") && Response.Tables[0].Rows[0]["id"].ToString() == "0")
                    return Return400(Response.Tables[0].Rows[0]["MSG"].ToString());

                return Return200(Response.Tables[0].Rows[0]["MSG"].ToString());
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Placebetcard32 : " + ex.ToString(), " : Req" + JsonConvert.SerializeObject(placebetteen));
                return Return400("Server Error");
            }
        }
        [HttpPost]
        [UserAuth, KeyFilter]
        [Route("placebetab")]
        public HttpResponseMessage Placebetab([FromBody] Placebetteen placebetteen)
        {
            try
            {
                ErrorLog.WriteLogAll("Placebetab", JsonConvert.SerializeObject(placebetteen));

                placebetteen.btype = placebetteen.btype.ToLower();

                string u_id = string.Empty;
                var header = HttpContext.Current.Request.Headers["Authorization"].ToString();
                if (header != null)
                {
                    u_id = JwtTokenGenerator.GetSaGuid(header);
                }

                Guid uu_id;
                bool res = Guid.TryParse(u_id, out uu_id);

                placebetteen.uid = Convert.ToInt64(JwtTokenGenerator.GetUid(header));
                placebetteen.pid = u_id.ToString();
                if (!res)
                    return Return400("token not valid.");
                var Response = _betservice.Placebetab(placebetteen);

                if (Response != null && Response.Tables.Count > 0 && Response.Tables[Response.Tables.Count - 1].Columns.Contains("id") && Response.Tables[Response.Tables.Count - 1].Rows[0]["id"].ToString() == "-1")
                {
                    ErrorLog.WriteLog("Placebetab", JsonConvert.SerializeObject(placebetteen), Response.Tables[Response.Tables.Count - 1].Rows[0]["MSG"].ToString());
                    return Return400("Data Error");
                }
                if (Response == null || Response.Tables.Count <= 0)
                    return Return400("No Data Found.");
                if (Response.Tables[0].Rows.Count <= 0)
                    return Return400("No Record Found.");
                if (Response.Tables[0].Columns.Contains("id") && Response.Tables[0].Rows[0]["id"].ToString() == "0")
                    return Return400(Response.Tables[0].Rows[0]["MSG"].ToString());

                return Return200(Response.Tables[0].Rows[0]["MSG"].ToString());
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Placebetab : " + ex.ToString(), " : Req" + JsonConvert.SerializeObject(placebetteen));
                return Return400("Server Error");
            }
        }
        [HttpPost]
        [UserAuth, KeyFilter]
        [Route("placebetqueen")]
        public HttpResponseMessage Placebetqueen([FromBody] Placebetteen placebetteen)
        {
            try
            {
                ErrorLog.WriteLogAll("Placebetqueen", JsonConvert.SerializeObject(placebetteen));

                placebetteen.btype = placebetteen.btype.ToLower();

                string u_id = string.Empty;
                var header = HttpContext.Current.Request.Headers["Authorization"].ToString();
                if (header != null)
                {
                    u_id = JwtTokenGenerator.GetSaGuid(header);
                }

                Guid uu_id;
                bool res = Guid.TryParse(u_id, out uu_id);

                placebetteen.uid = Convert.ToInt64(JwtTokenGenerator.GetUid(header));
                placebetteen.pid = u_id.ToString();
                if (!res)
                    return Return400("token not valid.");
                var Response = _betservice.Placebetqueen(placebetteen);

                if (Response != null && Response.Tables.Count > 0 && Response.Tables[Response.Tables.Count - 1].Columns.Contains("id") && Response.Tables[Response.Tables.Count - 1].Rows[0]["id"].ToString() == "-1")
                {
                    ErrorLog.WriteLog("Placebetqueen", JsonConvert.SerializeObject(placebetteen), Response.Tables[Response.Tables.Count - 1].Rows[0]["MSG"].ToString());
                    return Return400("Data Error");
                }
                if (Response == null || Response.Tables.Count <= 0)
                    return Return400("No Data Found.");
                if (Response.Tables[0].Rows.Count <= 0)
                    return Return400("No Record Found.");
                if (Response.Tables[0].Columns.Contains("id") && Response.Tables[0].Rows[0]["id"].ToString() == "0")
                    return Return400(Response.Tables[0].Rows[0]["MSG"].ToString());

                return Return200(Response.Tables[0].Rows[0]["MSG"].ToString());
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Placebetqueen : " + ex.ToString(), " : Req" + JsonConvert.SerializeObject(placebetteen));
                return Return400("Server Error");
            }
        }
        [HttpPost]
        [UserAuth, KeyFilter]
        [Route("placebetrace")]
        public HttpResponseMessage Placebetrace([FromBody] Placebetteen placebetteen)
        {
            try
            {
                ErrorLog.WriteLogAll("Placebetrace", JsonConvert.SerializeObject(placebetteen));

                placebetteen.btype = placebetteen.btype.ToLower();

                string u_id = string.Empty;
                var header = HttpContext.Current.Request.Headers["Authorization"].ToString();
                if (header != null)
                {
                    u_id = JwtTokenGenerator.GetSaGuid(header);
                }

                Guid uu_id;
                bool res = Guid.TryParse(u_id, out uu_id);

                placebetteen.uid = Convert.ToInt64(JwtTokenGenerator.GetUid(header));
                placebetteen.pid = u_id.ToString();
                if (!res)
                    return Return400("token not valid.");
                var Response = _betservice.Placebetrace(placebetteen);

                if (Response != null && Response.Tables.Count > 0 && Response.Tables[Response.Tables.Count - 1].Columns.Contains("id") && Response.Tables[Response.Tables.Count - 1].Rows[0]["id"].ToString() == "-1")
                {
                    ErrorLog.WriteLog("Placebetrace", JsonConvert.SerializeObject(placebetteen), Response.Tables[Response.Tables.Count - 1].Rows[0]["MSG"].ToString());
                    return Return400("Data Error");
                }
                if (Response == null || Response.Tables.Count <= 0)
                    return Return400("No Data Found.");
                if (Response.Tables[0].Rows.Count <= 0)
                    return Return400("No Record Found.");
                if (Response.Tables[0].Columns.Contains("id") && Response.Tables[0].Rows[0]["id"].ToString() == "0")
                    return Return400(Response.Tables[0].Rows[0]["MSG"].ToString());

                return Return200(Response.Tables[0].Rows[0]["MSG"].ToString());
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Placebetrace : " + ex.ToString(), " : Req" + JsonConvert.SerializeObject(placebetteen));
                return Return400("Server Error");
            }
        }
        [HttpPost]
        [UserAuth, KeyFilter]
        [Route("placebetlottery")]
        public HttpResponseMessage Placebetlottery([FromBody] Placebet3cardj placebet3Cardj)
        {
            try
            {
                ErrorLog.WriteLogAll("Placebetlottery", JsonConvert.SerializeObject(placebet3Cardj));

                placebet3Cardj.btype = placebet3Cardj.btype.ToLower();

                string u_id = string.Empty;
                var header = HttpContext.Current.Request.Headers["Authorization"].ToString();
                if (header != null)
                {
                    u_id = JwtTokenGenerator.GetSaGuid(header);
                }

                Guid uu_id;
                bool res = Guid.TryParse(u_id, out uu_id);

                placebet3Cardj.uid = Convert.ToInt64(JwtTokenGenerator.GetUid(header));
                placebet3Cardj.pid = u_id.ToString();
                if (!res)
                    return Return400("token not valid.");
                var Response = _betservice.Placebetlottery(placebet3Cardj);

                if (Response != null && Response.Tables.Count > 0 && Response.Tables[Response.Tables.Count - 1].Columns.Contains("id") && Response.Tables[Response.Tables.Count - 1].Rows[0]["id"].ToString() == "-1")
                {
                    ErrorLog.WriteLog("Placebetlottery", JsonConvert.SerializeObject(placebet3Cardj), Response.Tables[Response.Tables.Count - 1].Rows[0]["MSG"].ToString());
                    return Return400("Data Error");
                }
                if (Response == null || Response.Tables.Count <= 0)
                    return Return400("No Data Found.");
                if (Response.Tables[0].Rows.Count <= 0)
                    return Return400("No Record Found.");
                if (Response.Tables[0].Columns.Contains("id") && Response.Tables[0].Rows[0]["id"].ToString() == "0")
                    return Return400(Response.Tables[0].Rows[0]["MSG"].ToString());

                return Return200(Response.Tables[0].Rows[0]["MSG"].ToString());
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Placebetlottery : " + ex.ToString(), " : Req" + JsonConvert.SerializeObject(placebet3Cardj));
                return Return400("Server Error");
            }
        }
        [HttpPost]
        [UserAuth, KeyFilter]
        [Route("placebetlotteryrep")]
        public HttpResponseMessage Placebetlotteryrep([FromBody] Placebetlotteryrep placebetlotteryrep)
        {
            try
            {
                ErrorLog.WriteLogAll("Placebetlotteryrep", JsonConvert.SerializeObject(placebetlotteryrep));


                string u_id = string.Empty;
                var header = HttpContext.Current.Request.Headers["Authorization"].ToString();
                if (header != null)
                {
                    u_id = JwtTokenGenerator.GetSaGuid(header);
                }

                Guid uu_id;
                bool res = Guid.TryParse(u_id, out uu_id);

                placebetlotteryrep.uid = Convert.ToInt64(JwtTokenGenerator.GetUid(header));
                if (!res)
                    return Return400("token not valid.");
                var Response = _betservice.Placebetlotteryrep(placebetlotteryrep);

                if (Response != null && Response.Tables.Count > 0 && Response.Tables[Response.Tables.Count - 1].Columns.Contains("id") && Response.Tables[Response.Tables.Count - 1].Rows[0]["id"].ToString() == "-1")
                {
                    ErrorLog.WriteLog("Placebetlotteryrep", JsonConvert.SerializeObject(placebetlotteryrep), Response.Tables[Response.Tables.Count - 1].Rows[0]["MSG"].ToString());
                    return Return400("Data Error");
                }
                if (Response == null || Response.Tables.Count <= 0)
                    return Return400("No Data Found.");
                if (Response.Tables[0].Rows.Count <= 0)
                    return Return400("No Record Found.");
                if (Response.Tables[0].Columns.Contains("id") && Response.Tables[0].Rows[0]["id"].ToString() == "0")
                    return Return400(Response.Tables[0].Rows[0]["MSG"].ToString());

                return Return200(Response.Tables[0].Rows[0]["MSG"].ToString());
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Placebetlotteryrep : " + ex.ToString(), " : Req" + JsonConvert.SerializeObject(placebetlotteryrep));
                return Return400("Server Error");
            }
        }
        [HttpPost]
        [UserAuth, KeyFilter]
        [Route("placebetlotterydtl")]
        public HttpResponseMessage Placebetlotterydtl([FromBody] Placebetlotterydtl placebetlotterydtl)
        {
            try
            {
                ErrorLog.WriteLogAll("Placebetlotterydtl", JsonConvert.SerializeObject(placebetlotterydtl));

                string u_id = string.Empty;
                var header = HttpContext.Current.Request.Headers["Authorization"].ToString();
                if (header != null)
                {
                    u_id = JwtTokenGenerator.GetSaGuid(header);
                }

                Guid uu_id;
                bool res = Guid.TryParse(u_id, out uu_id);

                placebetlotterydtl.uid = Convert.ToInt64(JwtTokenGenerator.GetUid(header));
                if (!res)
                    return Return400("token not valid.");
                var Response = _betservice.Placebetlotterydtl(placebetlotterydtl);

                if (Response != null && Response.Tables.Count > 0 && Response.Tables[Response.Tables.Count - 1].Columns.Contains("id") && Response.Tables[Response.Tables.Count - 1].Rows[0]["id"].ToString() == "-1")
                {
                    ErrorLog.WriteLog("Placebetlotterydtl", JsonConvert.SerializeObject(placebetlotterydtl), Response.Tables[Response.Tables.Count - 1].Rows[0]["MSG"].ToString());
                    return Return400("Data Error");
                }
                if (Response == null || Response.Tables.Count <= 0)
                    return Return400("No Data Found.");
                if (Response.Tables[0].Rows.Count <= 0)
                    return Return400("No Record Found.");
                if (Response.Tables[0].Columns.Contains("id") && Response.Tables[0].Rows[0]["id"].ToString() == "0")
                    return Return400(Response.Tables[0].Rows[0]["MSG"].ToString());

                return Return200(Response.Tables[0].Rows[0]["MSG"].ToString());
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Placebetlotterydtl : " + ex.ToString(), " : Req" + JsonConvert.SerializeObject(placebetlotterydtl));
                return Return400("Server Error");
            }
        }
        [HttpPost]
        [UserAuth, KeyFilter]
        [Route("placebettrap")]
        public HttpResponseMessage Placebettrap([FromBody] Placebetteen placebetteen)
        {
            try
            {
                ErrorLog.WriteLogAll("Placebettrap", JsonConvert.SerializeObject(placebetteen));

                placebetteen.btype = placebetteen.btype.ToLower();

                string u_id = string.Empty;
                var header = HttpContext.Current.Request.Headers["Authorization"].ToString();
                if (header != null)
                {
                    u_id = JwtTokenGenerator.GetSaGuid(header);
                }

                Guid uu_id;
                bool res = Guid.TryParse(u_id, out uu_id);

                placebetteen.uid = Convert.ToInt64(JwtTokenGenerator.GetUid(header));
                placebetteen.pid = u_id.ToString();
                if (!res)
                    return Return400("token not valid.");
                var Response = _betservice.Placebettrap(placebetteen);

                if (Response != null && Response.Tables.Count > 0 && Response.Tables[Response.Tables.Count - 1].Columns.Contains("id") && Response.Tables[Response.Tables.Count - 1].Rows[0]["id"].ToString() == "-1")
                {
                    ErrorLog.WriteLog("Placebettrap", JsonConvert.SerializeObject(placebetteen), Response.Tables[Response.Tables.Count - 1].Rows[0]["MSG"].ToString());
                    return Return400("Data Error");
                }
                if (Response == null || Response.Tables.Count <= 0)
                    return Return400("No Data Found.");
                if (Response.Tables[0].Rows.Count <= 0)
                    return Return400("No Record Found.");
                if (Response.Tables[0].Columns.Contains("id") && Response.Tables[0].Rows[0]["id"].ToString() == "0")
                    return Return400(Response.Tables[0].Rows[0]["MSG"].ToString());

                return Return200(Response.Tables[0].Rows[0]["MSG"].ToString());
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Placebettrap : " + ex.ToString(), " : Req" + JsonConvert.SerializeObject(placebetteen));
                return Return400("Server Error");
            }
        }
        [HttpPost]
        [UserAuth, KeyFilter]
        [Route("placebetpatti2")]
        public HttpResponseMessage Placebetpatti2([FromBody] Placebetpatti2 placebetteen)
        {
            try
            {
                ErrorLog.WriteLogAll("Placebetpatti2", JsonConvert.SerializeObject(placebetteen));

                placebetteen.btype = placebetteen.btype.ToLower();

                string u_id = string.Empty;
                var header = HttpContext.Current.Request.Headers["Authorization"].ToString();
                if (header != null)
                {
                    u_id = JwtTokenGenerator.GetSaGuid(header);
                }

                Guid uu_id;
                bool res = Guid.TryParse(u_id, out uu_id);

                placebetteen.uid = Convert.ToInt64(JwtTokenGenerator.GetUid(header));
                placebetteen.pid = u_id.ToString();
                if (!res)
                    return Return400("token not valid.");
                var Response = _betservice.Placebetpatti2(placebetteen);

                if (Response != null && Response.Tables.Count > 0 && Response.Tables[Response.Tables.Count - 1].Columns.Contains("id") && Response.Tables[Response.Tables.Count - 1].Rows[0]["id"].ToString() == "-1")
                {
                    ErrorLog.WriteLog("Placebetpatti2", JsonConvert.SerializeObject(placebetteen), Response.Tables[Response.Tables.Count - 1].Rows[0]["MSG"].ToString());
                    return Return400("Data Error");
                }
                if (Response == null || Response.Tables.Count <= 0)
                    return Return400("No Data Found.");
                if (Response.Tables[0].Rows.Count <= 0)
                    return Return400("No Record Found.");
                if (Response.Tables[0].Columns.Contains("id") && Response.Tables[0].Rows[0]["id"].ToString() == "0")
                    return Return400(Response.Tables[0].Rows[0]["MSG"].ToString());

                return Return200(Response.Tables[0].Rows[0]["MSG"].ToString());
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Placebetpatti2 : " + ex.ToString(), " : Req" + JsonConvert.SerializeObject(placebetteen));
                return Return400("Server Error");
            }
        }
        [HttpPost]
        [UserAuth, KeyFilter]
        [Route("placebetnotenum")]
        public HttpResponseMessage Placebetnotenum([FromBody] Placebetpatti2 placebetteen)
        {
            try
            {
                ErrorLog.WriteLogAll("Placebetnotenum", JsonConvert.SerializeObject(placebetteen));

                placebetteen.btype = placebetteen.btype.ToLower();

                string u_id = string.Empty;
                var header = HttpContext.Current.Request.Headers["Authorization"].ToString();
                if (header != null)
                {
                    u_id = JwtTokenGenerator.GetSaGuid(header);
                }

                Guid uu_id;
                bool res = Guid.TryParse(u_id, out uu_id);

                placebetteen.uid = Convert.ToInt64(JwtTokenGenerator.GetUid(header));
                placebetteen.pid = u_id.ToString();
                if (!res)
                    return Return400("token not valid.");
                var Response = _betservice.Placebetnotenum(placebetteen);

                if (Response != null && Response.Tables.Count > 0 && Response.Tables[Response.Tables.Count - 1].Columns.Contains("id") && Response.Tables[Response.Tables.Count - 1].Rows[0]["id"].ToString() == "-1")
                {
                    ErrorLog.WriteLog("Placebetnotenum", JsonConvert.SerializeObject(placebetteen), Response.Tables[Response.Tables.Count - 1].Rows[0]["MSG"].ToString());
                    return Return400("Data Error");
                }
                if (Response == null || Response.Tables.Count <= 0)
                    return Return400("No Data Found.");
                if (Response.Tables[0].Rows.Count <= 0)
                    return Return400("No Record Found.");
                if (Response.Tables[0].Columns.Contains("id") && Response.Tables[0].Rows[0]["id"].ToString() == "0")
                    return Return400(Response.Tables[0].Rows[0]["MSG"].ToString());

                return Return200(Response.Tables[0].Rows[0]["MSG"].ToString());
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Placebetnotenum : " + ex.ToString(), " : Req" + JsonConvert.SerializeObject(placebetteen));
                return Return400("Server Error");
            }
        }
        [HttpPost]
        [UserAuth, KeyFilter]
        [Route("placebetkbc")]
        public HttpResponseMessage Placebetkbc([FromBody] Placebetkbc placebetteen)
        {
            try
            {
                ErrorLog.WriteLogAll("Placebetkbc", JsonConvert.SerializeObject(placebetteen));

                string u_id = string.Empty;
                var header = HttpContext.Current.Request.Headers["Authorization"].ToString();
                if (header != null)
                {
                    u_id = JwtTokenGenerator.GetSaGuid(header);
                }

                Guid uu_id;
                bool res = Guid.TryParse(u_id, out uu_id);

                placebetteen.uid = Convert.ToInt64(JwtTokenGenerator.GetUid(header));
                placebetteen.pid = u_id.ToString();
                if (!res)
                    return Return400("token not valid.");


                //var sid1 = JToken.Parse(placebetteen.bjson);
                //if (sid1 != null && sid1.Count() == 5)
                //{
                //    //var ddd= sid.DeepEquals(i => i["sid"]).Values<string>, array1[])
                //    int[] array1 = new int[] { 1, 2, 3, 4, 5 };
                //    Object inner = sid1[0].Value<JObject>();

                //    //var unchangedKeys = sid.Properties().Where(c => JToken.Parse(c.Value, Model[c.Name])).Select(c => c.Name);
                //    //var d = array1[Convert.ToInt32(ja["sid"])];
                //}

                var Response = _betservice.Placebetkbc(placebetteen);


                if (Response != null && Response.Tables.Count > 0 && Response.Tables[Response.Tables.Count - 1].Columns.Contains("id") && Response.Tables[Response.Tables.Count - 1].Rows[0]["id"].ToString() == "-1")
                {
                    ErrorLog.WriteLog("Placebetkbc", JsonConvert.SerializeObject(placebetteen), Response.Tables[Response.Tables.Count - 1].Rows[0]["MSG"].ToString());
                    return Return400("Data Error");
                }
                if (Response == null || Response.Tables.Count <= 0)
                    return Return400("No Data Found.");
                if (Response.Tables[0].Rows.Count <= 0)
                    return Return400("No Record Found.");
                if (Response.Tables[0].Columns.Contains("id") && Response.Tables[0].Rows[0]["id"].ToString() == "0")
                    return Return400(Response.Tables[0].Rows[0]["MSG"].ToString());

                return Return200(Response.Tables[0].Rows[0]["MSG"].ToString());
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Placebetkbc : " + ex.ToString(), " : Req" + JsonConvert.SerializeObject(placebetteen));
                return Return400("Server Error");
            }
        }
        [HttpPost]
        [UserAuth, KeyFilter]
        [Route("placebetvteen")]
        public HttpResponseMessage Placebetvteen([FromBody] Placebetteen placebetteen)
        {
            try
            {
                ErrorLog.WriteLogAll("Placebetvteen", JsonConvert.SerializeObject(placebetteen));

                placebetteen.btype = placebetteen.btype.ToLower();

                string u_id = string.Empty;
                var header = HttpContext.Current.Request.Headers["Authorization"].ToString();
                if (header != null)
                {
                    u_id = JwtTokenGenerator.GetSaGuid(header);
                }

                Guid uu_id;
                bool res = Guid.TryParse(u_id, out uu_id);

                placebetteen.uid = Convert.ToInt64(JwtTokenGenerator.GetUid(header));
                placebetteen.pid = u_id.ToString();
                if (!res)
                    return Return400("token not valid.");
                var Response = _betservice.Placebetvteen(placebetteen);

                if (Response != null && Response.Tables.Count > 0 && Response.Tables[Response.Tables.Count - 1].Columns.Contains("id") && Response.Tables[Response.Tables.Count - 1].Rows[0]["id"].ToString() == "-1")
                {
                    ErrorLog.WriteLog("Placebetvteen", JsonConvert.SerializeObject(placebetteen), Response.Tables[Response.Tables.Count - 1].Rows[0]["MSG"].ToString());
                    return Return400("Data Error");
                }
                if (Response == null || Response.Tables.Count <= 0)
                    return Return400("No Data Found.");
                if (Response.Tables[0].Rows.Count <= 0)
                    return Return400("No Record Found.");
                if (Response.Tables[0].Columns.Contains("id") && Response.Tables[0].Rows[0]["id"].ToString() == "0")
                    return Return400(Response.Tables[0].Rows[0]["MSG"].ToString());

                return Return200(Response.Tables[0].Rows[0]["MSG"].ToString());
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Placebetvteen : " + ex.ToString(), " : Req" + JsonConvert.SerializeObject(placebetteen));
                return Return400("Server Error");
            }
        }
        [HttpPost]
        [UserAuth, KeyFilter]
        [Route("placebetvcard32")]
        public HttpResponseMessage Placebetvcard32([FromBody] Placebetteen placebetteen)
        {
            try
            {
                ErrorLog.WriteLogAll("Placebetvcard32", JsonConvert.SerializeObject(placebetteen));

                placebetteen.btype = placebetteen.btype.ToLower();

                string u_id = string.Empty;
                var header = HttpContext.Current.Request.Headers["Authorization"].ToString();
                if (header != null)
                {
                    u_id = JwtTokenGenerator.GetSaGuid(header);
                }

                Guid uu_id;
                bool res = Guid.TryParse(u_id, out uu_id);

                placebetteen.uid = Convert.ToInt64(JwtTokenGenerator.GetUid(header));
                placebetteen.pid = u_id.ToString();
                if (!res)
                    return Return400("token not valid.");
                var Response = _betservice.Placebetvcard32(placebetteen);

                if (Response != null && Response.Tables.Count > 0 && Response.Tables[Response.Tables.Count - 1].Columns.Contains("id") && Response.Tables[Response.Tables.Count - 1].Rows[0]["id"].ToString() == "-1")
                {
                    ErrorLog.WriteLog("Placebetvcard32", JsonConvert.SerializeObject(placebetteen), Response.Tables[Response.Tables.Count - 1].Rows[0]["MSG"].ToString());
                    return Return400("Data Error");
                }
                if (Response == null || Response.Tables.Count <= 0)
                    return Return400("No Data Found.");
                if (Response.Tables[0].Rows.Count <= 0)
                    return Return400("No Record Found.");
                if (Response.Tables[0].Columns.Contains("id") && Response.Tables[0].Rows[0]["id"].ToString() == "0")
                    return Return400(Response.Tables[0].Rows[0]["MSG"].ToString());

                return Return200(Response.Tables[0].Rows[0]["MSG"].ToString());
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Placebetvcard32 : " + ex.ToString(), " : Req" + JsonConvert.SerializeObject(placebetteen));
                return Return400("Server Error");
            }
        }
        [HttpPost]
        [UserAuth, KeyFilter]
        [Route("placebetvbc")]
        public HttpResponseMessage Placebetvbc([FromBody] Placebetteen placebetteen)
        {
            try
            {
                ErrorLog.WriteLogAll("Placebetvbc", JsonConvert.SerializeObject(placebetteen));

                placebetteen.btype = placebetteen.btype.ToLower();

                string u_id = string.Empty;
                var header = HttpContext.Current.Request.Headers["Authorization"].ToString();
                if (header != null)
                {
                    u_id = JwtTokenGenerator.GetSaGuid(header);
                }

                Guid uu_id;
                bool res = Guid.TryParse(u_id, out uu_id);

                placebetteen.uid = Convert.ToInt64(JwtTokenGenerator.GetUid(header));
                placebetteen.pid = u_id.ToString();
                if (!res)
                    return Return400("token not valid.");
                var Response = _betservice.Placebetvbc(placebetteen);

                if (Response != null && Response.Tables.Count > 0 && Response.Tables[Response.Tables.Count - 1].Columns.Contains("id") && Response.Tables[Response.Tables.Count - 1].Rows[0]["id"].ToString() == "-1")
                {
                    ErrorLog.WriteLog("Placebetvbc", JsonConvert.SerializeObject(placebetteen), Response.Tables[Response.Tables.Count - 1].Rows[0]["MSG"].ToString());
                    return Return400("Data Error");
                }
                if (Response == null || Response.Tables.Count <= 0)
                    return Return400("No Data Found.");
                if (Response.Tables[0].Rows.Count <= 0)
                    return Return400("No Record Found.");
                if (Response.Tables[0].Columns.Contains("id") && Response.Tables[0].Rows[0]["id"].ToString() == "0")
                    return Return400(Response.Tables[0].Rows[0]["MSG"].ToString());

                return Return200(Response.Tables[0].Rows[0]["MSG"].ToString());
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Placebetvbc : " + ex.ToString(), " : Req" + JsonConvert.SerializeObject(placebetteen));
                return Return400("Server Error");
            }
        }
        [HttpPost]
        [UserAuth, KeyFilter]
        [Route("placebetvbaccarat")]
        public HttpResponseMessage Placebetvbaccarat([FromBody] Placebetteen placebetteen)
        {
            try
            {
                ErrorLog.WriteLogAll("Placebetvbaccarat", JsonConvert.SerializeObject(placebetteen));

                placebetteen.btype = placebetteen.btype.ToLower();

                string u_id = string.Empty;
                var header = HttpContext.Current.Request.Headers["Authorization"].ToString();
                if (header != null)
                {
                    u_id = JwtTokenGenerator.GetSaGuid(header);
                }

                Guid uu_id;
                bool res = Guid.TryParse(u_id, out uu_id);

                placebetteen.uid = Convert.ToInt64(JwtTokenGenerator.GetUid(header));
                placebetteen.pid = u_id.ToString();
                if (!res)
                    return Return400("token not valid.");
                var Response = _betservice.Placebetvbaccarat(placebetteen);

                if (Response != null && Response.Tables.Count > 0 && Response.Tables[Response.Tables.Count - 1].Columns.Contains("id") && Response.Tables[Response.Tables.Count - 1].Rows[0]["id"].ToString() == "-1")
                {
                    ErrorLog.WriteLog("Placebetvbaccarat", JsonConvert.SerializeObject(placebetteen), Response.Tables[Response.Tables.Count - 1].Rows[0]["MSG"].ToString());
                    return Return400("Data Error");
                }
                if (Response == null || Response.Tables.Count <= 0)
                    return Return400("No Data Found.");
                if (Response.Tables[0].Rows.Count <= 0)
                    return Return400("No Record Found.");
                if (Response.Tables[0].Columns.Contains("id") && Response.Tables[0].Rows[0]["id"].ToString() == "0")
                    return Return400(Response.Tables[0].Rows[0]["MSG"].ToString());

                return Return200(Response.Tables[0].Rows[0]["MSG"].ToString());
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Placebetvbaccarat : " + ex.ToString(), " : Req" + JsonConvert.SerializeObject(placebetteen));
                return Return400("Server Error");
            }
        }
        [HttpPost]
        [UserAuth, KeyFilter]
        [Route("placebetvqueen")]
        public HttpResponseMessage Placebetvqueen([FromBody] Placebetteen placebetteen)
        {
            try
            {
                ErrorLog.WriteLogAll("Placebetvqueen", JsonConvert.SerializeObject(placebetteen));

                placebetteen.btype = placebetteen.btype.ToLower();

                string u_id = string.Empty;
                var header = HttpContext.Current.Request.Headers["Authorization"].ToString();
                if (header != null)
                {
                    u_id = JwtTokenGenerator.GetSaGuid(header);
                }

                Guid uu_id;
                bool res = Guid.TryParse(u_id, out uu_id);

                placebetteen.uid = Convert.ToInt64(JwtTokenGenerator.GetUid(header));
                placebetteen.pid = u_id.ToString();
                if (!res)
                    return Return400("token not valid.");
                var Response = _betservice.Placebetvqueen(placebetteen);

                if (Response != null && Response.Tables.Count > 0 && Response.Tables[Response.Tables.Count - 1].Columns.Contains("id") && Response.Tables[Response.Tables.Count - 1].Rows[0]["id"].ToString() == "-1")
                {
                    ErrorLog.WriteLog("Placebetvqueen", JsonConvert.SerializeObject(placebetteen), Response.Tables[Response.Tables.Count - 1].Rows[0]["MSG"].ToString());
                    return Return400("Data Error");
                }
                if (Response == null || Response.Tables.Count <= 0)
                    return Return400("No Data Found.");
                if (Response.Tables[0].Rows.Count <= 0)
                    return Return400("No Record Found.");
                if (Response.Tables[0].Columns.Contains("id") && Response.Tables[0].Rows[0]["id"].ToString() == "0")
                    return Return400(Response.Tables[0].Rows[0]["MSG"].ToString());

                return Return200(Response.Tables[0].Rows[0]["MSG"].ToString());
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Placebetvqueen : " + ex.ToString(), " : Req" + JsonConvert.SerializeObject(placebetteen));
                return Return400("Server Error");
            }
        }
        [HttpPost]
        [UserAuth, KeyFilter]
        [Route("placebetvrace")]
        public HttpResponseMessage Placebetvrace([FromBody] Placebetteen placebetteen)
        {
            try
            {
                ErrorLog.WriteLogAll("Placebetvrace", JsonConvert.SerializeObject(placebetteen));

                placebetteen.btype = placebetteen.btype.ToLower();

                string u_id = string.Empty;
                var header = HttpContext.Current.Request.Headers["Authorization"].ToString();
                if (header != null)
                {
                    u_id = JwtTokenGenerator.GetSaGuid(header);
                }

                Guid uu_id;
                bool res = Guid.TryParse(u_id, out uu_id);

                placebetteen.uid = Convert.ToInt64(JwtTokenGenerator.GetUid(header));
                placebetteen.pid = u_id.ToString();
                if (!res)
                    return Return400("token not valid.");
                var Response = _betservice.Placebetvrace(placebetteen);

                if (Response != null && Response.Tables.Count > 0 && Response.Tables[Response.Tables.Count - 1].Columns.Contains("id") && Response.Tables[Response.Tables.Count - 1].Rows[0]["id"].ToString() == "-1")
                {
                    ErrorLog.WriteLog("Placebetvrace", JsonConvert.SerializeObject(placebetteen), Response.Tables[Response.Tables.Count - 1].Rows[0]["MSG"].ToString());
                    return Return400("Data Error");
                }
                if (Response == null || Response.Tables.Count <= 0)
                    return Return400("No Data Found.");
                if (Response.Tables[0].Rows.Count <= 0)
                    return Return400("No Record Found.");
                if (Response.Tables[0].Columns.Contains("id") && Response.Tables[0].Rows[0]["id"].ToString() == "0")
                    return Return400(Response.Tables[0].Rows[0]["MSG"].ToString());

                return Return200(Response.Tables[0].Rows[0]["MSG"].ToString());
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Placebetvrace : " + ex.ToString(), " : Req" + JsonConvert.SerializeObject(placebetteen));
                return Return400("Server Error");
            }
        }
        [HttpPost]
        [UserAuth, KeyFilter]
        [Route("placebetvtrap")]
        public HttpResponseMessage Placebetvtrap([FromBody] Placebetteen placebetteen)
        {
            try
            {
                ErrorLog.WriteLogAll("Placebetvtrap", JsonConvert.SerializeObject(placebetteen));

                placebetteen.btype = placebetteen.btype.ToLower();

                string u_id = string.Empty;
                var header = HttpContext.Current.Request.Headers["Authorization"].ToString();
                if (header != null)
                {
                    u_id = JwtTokenGenerator.GetSaGuid(header);
                }

                Guid uu_id;
                bool res = Guid.TryParse(u_id, out uu_id);

                placebetteen.uid = Convert.ToInt64(JwtTokenGenerator.GetUid(header));
                placebetteen.pid = u_id.ToString();
                if (!res)
                    return Return400("token not valid.");
                var Response = _betservice.Placebetvtrap(placebetteen);

                if (Response != null && Response.Tables.Count > 0 && Response.Tables[Response.Tables.Count - 1].Columns.Contains("id") && Response.Tables[Response.Tables.Count - 1].Rows[0]["id"].ToString() == "-1")
                {
                    ErrorLog.WriteLog("Placebetvtrap", JsonConvert.SerializeObject(placebetteen), Response.Tables[Response.Tables.Count - 1].Rows[0]["MSG"].ToString());
                    return Return400("Data Error");
                }
                if (Response == null || Response.Tables.Count <= 0)
                    return Return400("No Data Found.");
                if (Response.Tables[0].Rows.Count <= 0)
                    return Return400("No Record Found.");
                if (Response.Tables[0].Columns.Contains("id") && Response.Tables[0].Rows[0]["id"].ToString() == "0")
                    return Return400(Response.Tables[0].Rows[0]["MSG"].ToString());

                return Return200(Response.Tables[0].Rows[0]["MSG"].ToString());
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Placebetvtrap : " + ex.ToString(), " : Req" + JsonConvert.SerializeObject(placebetteen));
                return Return400("Server Error");
            }
        }
        [HttpPost]
        [UserAuth, KeyFilter]
        [Route("placebetvpatti2")]
        public HttpResponseMessage Placebetvpatti2([FromBody] Placebetpatti2 placebetteen)
        {
            try
            {
                ErrorLog.WriteLogAll("Placebetvpatti2", JsonConvert.SerializeObject(placebetteen));

                placebetteen.btype = placebetteen.btype.ToLower();

                string u_id = string.Empty;
                var header = HttpContext.Current.Request.Headers["Authorization"].ToString();
                if (header != null)
                {
                    u_id = JwtTokenGenerator.GetSaGuid(header);
                }

                Guid uu_id;
                bool res = Guid.TryParse(u_id, out uu_id);

                placebetteen.uid = Convert.ToInt64(JwtTokenGenerator.GetUid(header));
                placebetteen.pid = u_id.ToString();
                if (!res)
                    return Return400("token not valid.");
                var Response = _betservice.Placebetvpatti2(placebetteen);

                if (Response != null && Response.Tables.Count > 0 && Response.Tables[Response.Tables.Count - 1].Columns.Contains("id") && Response.Tables[Response.Tables.Count - 1].Rows[0]["id"].ToString() == "-1")
                {
                    ErrorLog.WriteLog("Placebetpatti2", JsonConvert.SerializeObject(placebetteen), Response.Tables[Response.Tables.Count - 1].Rows[0]["MSG"].ToString());
                    return Return400("Data Error");
                }
                if (Response == null || Response.Tables.Count <= 0)
                    return Return400("No Data Found.");
                if (Response.Tables[0].Rows.Count <= 0)
                    return Return400("No Record Found.");
                if (Response.Tables[0].Columns.Contains("id") && Response.Tables[0].Rows[0]["id"].ToString() == "0")
                    return Return400(Response.Tables[0].Rows[0]["MSG"].ToString());

                return Return200(Response.Tables[0].Rows[0]["MSG"].ToString());
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Placebetvpatti2 : " + ex.ToString(), " : Req" + JsonConvert.SerializeObject(placebetteen));
                return Return400("Server Error");
            }
        }
        [HttpPost]
        [UserAuth, KeyFilter]
        [Route("placebetvnotenum")]
        public HttpResponseMessage Placebetvnotenum([FromBody] Placebetpatti2 placebetteen)
        {
            try
            {
                ErrorLog.WriteLogAll("Placebetvnotenum", JsonConvert.SerializeObject(placebetteen));

                placebetteen.btype = placebetteen.btype.ToLower();

                string u_id = string.Empty;
                var header = HttpContext.Current.Request.Headers["Authorization"].ToString();
                if (header != null)
                {
                    u_id = JwtTokenGenerator.GetSaGuid(header);
                }

                Guid uu_id;
                bool res = Guid.TryParse(u_id, out uu_id);

                placebetteen.uid = Convert.ToInt64(JwtTokenGenerator.GetUid(header));
                placebetteen.pid = u_id.ToString();
                if (!res)
                    return Return400("token not valid.");
                var Response = _betservice.Placebetvnotenum(placebetteen);

                if (Response != null && Response.Tables.Count > 0 && Response.Tables[Response.Tables.Count - 1].Columns.Contains("id") && Response.Tables[Response.Tables.Count - 1].Rows[0]["id"].ToString() == "-1")
                {
                    ErrorLog.WriteLog("Placebetvnotenum", JsonConvert.SerializeObject(placebetteen), Response.Tables[Response.Tables.Count - 1].Rows[0]["MSG"].ToString());
                    return Return400("Data Error");
                }
                if (Response == null || Response.Tables.Count <= 0)
                    return Return400("No Data Found.");
                if (Response.Tables[0].Rows.Count <= 0)
                    return Return400("No Record Found.");
                if (Response.Tables[0].Columns.Contains("id") && Response.Tables[0].Rows[0]["id"].ToString() == "0")
                    return Return400(Response.Tables[0].Rows[0]["MSG"].ToString());

                return Return200(Response.Tables[0].Rows[0]["MSG"].ToString());
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Placebetvnotenum : " + ex.ToString(), " : Req" + JsonConvert.SerializeObject(placebetteen));
                return Return400("Server Error");
            }
        }
        #endregion
    }
}
