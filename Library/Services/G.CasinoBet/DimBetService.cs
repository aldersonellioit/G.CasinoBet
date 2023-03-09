using Common;
using Data;
using Models.G.CasinoBet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.G.CasinoBet
{
    public class DimBetService : IDimBetService
    {
        #region Fields

        private readonly ISqlClientService _sqlClientService;

        #endregion

        #region Ctor

        public DimBetService(ISqlClientService sqlClientService)
        {
            _sqlClientService = sqlClientService;
        }

        #endregion

        #region Methods
        public DataSet GetDate()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                DataSet ds = _sqlClientService.Execute("gettimezone", ConfigItems.Conn_LogDgroup, parameters);
                return ds;
            }
            catch (Exception ex)
            {
                WriteLogAll("GetDate" + ex.ToString(), "");
                throw ex;
            }

        }
        public DataSet CheckWords(String par, String ip, String pname, String uid, String uguid)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { ParameterName = "Userid", Value = uid });
                parameters.Add(new SqlParameter() { ParameterName = "UGuid", Value = uguid });
                parameters.Add(new SqlParameter() { ParameterName = "ProcedreName", Value = pname });
                parameters.Add(new SqlParameter() { ParameterName = "Perameters", Value = par });
                parameters.Add(new SqlParameter() { ParameterName = "Ip", Value = ip });
                return _sqlClientService.Execute("InsertSqlInjection", ConfigItems.Conn_LogDgroup, parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet Placebetteen(Placebetteen placebetteen)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet dscom = new DataSet();
                DataSet dscasino = new DataSet();
                placebetteen.gtype.ToLower();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                parameters.Add(new SqlParameter() { ParameterName = "MarketId", Value = placebetteen.mid });
                parameters.Add(new SqlParameter() { ParameterName = "GameType", Value = placebetteen.gtype });
                ds = _sqlClientService.Execute("Casino_PlacebetCheck", ConfigItems.Conn_AccDgroup, parameters);
                WriteLogAll("Placebetteen+Casino_PlacebetCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds));
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("id") && ds.Tables[0].Rows[0]["id"].ToString() == "1")
                {
                    var parameterscas = new List<SqlParameter>();
                    parameterscas.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                    parameterscas.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                    parameterscas.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                    parameterscas.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                    parameterscas.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                    dscasino = _sqlClientService.Execute("teen_placeExpoCheck", ConfigItems.Conn_Casinogroup, parameterscas);
                    WriteLogAll("Placebetteen1+teen_placeExpoCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscasino));
                    //1 as Id,'ok' as msg,username,u.PartnershipType,general,ExposerLimit,oldexposer,AutoConfirm
                    if (dscasino != null && dscasino.Tables != null && dscasino.Tables.Count > 0 && dscasino.Tables[0].Rows.Count > 0 && dscasino.Tables[0].Columns.Contains("id") && dscasino.Tables[0].Rows[0]["id"].ToString() == "1")
                    {
                        var parameters3 = new List<SqlParameter>();
                        parameters3.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                        parameters3.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                        parameters3.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                        parameters3.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                        parameters3.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                        parameters3.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                        parameters3.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                        dscom = _sqlClientService.Execute("group2_teen_placebetconform", ConfigItems.Conn_Casino, parameters3);
                        WriteLogAll("Placebetteen2+group2_teen_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscom));
                        if (dscom != null && dscom.Tables != null && dscom.Tables.Count > 0 && dscom.Tables[0].Rows.Count > 0 && dscom.Tables[0].Columns.Contains("id") && dscom.Tables[0].Rows[0]["id"].ToString() == "1")
                        {
                            var parameters1 = new List<SqlParameter>();
                            parameters1.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                            parameters1.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                            parameters1.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                            parameters1.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                            parameters1.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                            parameters1.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                            parameters1.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                            parameters1.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                            parameters1.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newUserrate", Value = dscom.Tables[0].Rows[0]["NewUserrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newAmount", Value = dscom.Tables[0].Rows[0]["Amount"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Gameid", Value = dscom.Tables[0].Rows[0]["Gameid"] });
                            ds1 = _sqlClientService.Execute("teen_placebetconform", ConfigItems.Conn_Casinogroup, parameters1);
                            WriteLogAll("Placebetteen3+teen_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds1));

                            //select 1 as ID,'Ok' as Msg,	@userexposer as userexposer, @JsonGameSub as masterexposer
                            if (ds1 != null && ds1.Tables != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0 && ds1.Tables[0].Columns.Contains("id") && ds1.Tables[0].Rows[0]["id"].ToString() == "1")
                            {
                                try
                                {
                                    var param = "marketid=" + placebetteen.mid + "&userid=" + placebetteen.uid + "&subsectionid=" + placebetteen.subid + "&gametype=" + placebetteen.gtype + "&subgametype="
                                                                     + placebetteen.subtype + "&username=" + ds.Tables[0].Rows[0]["username"] + "&sectionid=" + placebetteen.sid + "&bettype=" + placebetteen.btype
                                                                    + "&nation=" + ds1.Tables[0].Rows[0]["nation"].ToString() + "&userrate=" + placebetteen.urate + "&Masteramount=" + ds1.Tables[0].Rows[0]["conformamount"]
                                                                    + "&ipaddress=" + placebetteen.ip + "&browserdetail=" + placebetteen.bdetail + "&BookvalType=" + ds1.Tables[0].Rows[0]["exposertype"] +
                                                                    "&Bookvalmatch=" + ds1.Tables[0].Rows[0]["matchbook"] + "&BookvalFancy=" + ds1.Tables[0].Rows[0]["fancybook"] + "&amount=" + placebetteen.amt + "&frate=" + ds.Tables[0].Rows[0]["balrate"] + "&Currency=" + ds.Tables[0].Rows[0]["ccode"];

                                    Task.Factory.StartNew(() => Post(ConfigItems.AdminBookUrl + ApiEndpoint.teenbook, param, "application/x-www-form-urlencoded", "POST"));
                                }
                                catch (System.Exception ex)
                                {
                                    WriteLogAll("Placebetteen+bk" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                var parameters2 = new List<SqlParameter>();
                                parameters2.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                                parameters2.Add(new SqlParameter() { ParameterName = "Amount", Value = Convert.ToDouble(ds1.Tables[0].Rows[0]["userexposer"]) });
                                parameters2.Add(new SqlParameter() { ParameterName = "ExposJson", Value = ds1.Tables[0].Rows[0]["masterexposer"].ToString() });
                                parameters2.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                                parameters2.Add(new SqlParameter() { ParameterName = "gameType", Value = placebetteen.gtype });
                                parameters2.Add(new SqlParameter() { ParameterName = "Userrate", Value = ds1.Tables[0].Rows[0]["newuserrate"] });
                                parameters2.Add(new SqlParameter() { ParameterName = "BetAmount", Value = ds1.Tables[0].Rows[0]["conformamount"] });
                                ds2 = _sqlClientService.Execute("Casino_ExposerUpdate", ConfigItems.Conn_AccDgroup, parameters2);
                                WriteLogAll("Placebetteen4+Casino_ExposerUpdate", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds2));
                                try
                                {
                                    if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 0 && ds2.Tables[1].Rows.Count > 0 && ds2.Tables[1].Columns.Contains("SendCoupon") && ds2.Tables[1].Rows[0]["SendCoupon"].ToString() == "1")
                                    {//Select 0 as SendCoupon,0 as BetAmount,'' as couponcode
                                        CouponPlacebet couponPlacebet = new CouponPlacebet();
                                        couponPlacebet.Amount = Convert.ToDouble(ds1.Tables[0].Rows[0]["conformamount"]);
                                        couponPlacebet.PlayerName = ds.Tables[0].Rows[0]["username"].ToString();
                                        couponPlacebet.PlayerId = placebetteen.pid;
                                        couponPlacebet.Product = ds2.Tables[1].Rows[0]["couponcode"].ToString();
                                        couponPlacebet.Balance = Convert.ToDouble(ds.Tables[0].Rows[0]["general"]);
                                        if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 1 && ds2.Tables[2].Rows.Count > 0 && ds2.Tables[2].Columns.Contains("clientid") && ds2.Tables[2].Columns.Contains("secretkey"))
                                        {
                                            var par = ConfigItems.Encrypt ? Encryptionbonus.EncryptString(JsonConvert.SerializeObject(couponPlacebet), ds2.Tables[2].Rows[0]["secretkey"].ToString()) : JsonConvert.SerializeObject(couponPlacebet);
                                            var resp = Postbonus(ConfigItems.CopUrl + "transaction/event", par, "application/json", "POST", ds2.Tables[2].Rows[0]["clientid"].ToString());
                                            var res = ConfigItems.Encrypt ? Encryptionbonus.DecryptString(resp, ds2.Tables[2].Rows[0]["secretkey"].ToString()) : resp;
                                            WriteLogAll("Placebetteen3", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(res));
                                        }
                                        //var resp = Post(ConfigItems.CopUrl + ApiEndpoint.placebet, JsonConvert.SerializeObject(couponPlacebet), "application/json", "POST");
                                        //var data = JsonConvert.DeserializeObject<CouponPlacebetRes>(resp);

                                        //Returnds("0", data.error);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteLogAll("Placebetteen+cop" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                Placebetfinal(placebetteen.uid);
                                return ds2;
                            }
                            else
                            {
                                Placebetfinal(placebetteen.uid);
                                return ds1;
                            }
                        }
                        Placebetfinal(placebetteen.uid);
                        return dscom;
                    }
                    Placebetfinal(placebetteen.uid);
                    return dscasino;
                }
                return ds;
            }
            catch (Exception ex)
            {
                Placebetfinal(placebetteen.uid);
                WriteLogAll("Placebetteen" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                throw ex;
            }

        }
        public DataSet Placebetvteen(Placebetteen placebetteen)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet dscom = new DataSet();
                DataSet dscasino = new DataSet();
                placebetteen.gtype.ToLower();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                parameters.Add(new SqlParameter() { ParameterName = "MarketId", Value = placebetteen.mid });
                parameters.Add(new SqlParameter() { ParameterName = "GameType", Value = placebetteen.gtype });
                ds = _sqlClientService.Execute("Casino_PlacebetCheck", ConfigItems.Conn_AccDgroup, parameters);
                WriteLogAll("Placebetvteen+Casino_PlacebetCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds));

                //1 as Id,'ok' as msg,username,u.PartnershipType,general,ExposerLimit,oldexposer,AutoConfirm
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("id") && ds.Tables[0].Rows[0]["id"].ToString() == "1")
                {
                    var parameterscas = new List<SqlParameter>();
                    parameterscas.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                    parameterscas.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                    parameterscas.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                    parameterscas.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                    parameterscas.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                    dscasino = _sqlClientService.Execute("z_teen_placeExpoCheck", ConfigItems.Conn_Casinogroup, parameterscas);
                    WriteLogAll("Placebetvteen1+z_teen_placeExpoCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscasino));
                    if (dscasino != null && dscasino.Tables != null && dscasino.Tables.Count > 0 && dscasino.Tables[0].Rows.Count > 0 && dscasino.Tables[0].Columns.Contains("id") && dscasino.Tables[0].Rows[0]["id"].ToString() == "1")
                    {
                        var parameters3 = new List<SqlParameter>();
                        parameters3.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                        parameters3.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                        parameters3.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                        parameters3.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                        parameters3.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                        parameters3.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                        parameters3.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                        dscom = _sqlClientService.Execute("group2_z_teen_placebetconform", ConfigItems.Conn_Casino, parameters3);
                        WriteLogAll("Placebetvteen2+group2_z_teen_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscom));
                        if (dscom != null && dscom.Tables != null && dscom.Tables.Count > 0 && dscom.Tables[0].Rows.Count > 0 && dscom.Tables[0].Columns.Contains("id") && dscom.Tables[0].Rows[0]["id"].ToString() == "1")
                        {
                            var parameters1 = new List<SqlParameter>();
                            parameters1.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                            parameters1.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                            parameters1.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                            parameters1.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                            parameters1.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                            parameters1.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                            parameters1.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                            parameters1.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                            parameters1.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newUserrate", Value = dscom.Tables[0].Rows[0]["NewUserrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newAmount", Value = dscom.Tables[0].Rows[0]["Amount"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Gameid", Value = dscom.Tables[0].Rows[0]["Gameid"] });
                            ds1 = _sqlClientService.Execute("z_teen_placebetconform", ConfigItems.Conn_Casinogroup, parameters1);
                            WriteLogAll("Placebetvteen3+z_teen_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds1));

                            //select 1 as ID,'Ok' as Msg,	@userexposer as userexposer, @JsonGameSub as masterexposer
                            if (ds1 != null && ds1.Tables != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0 && ds1.Tables[0].Columns.Contains("id") && ds1.Tables[0].Rows[0]["id"].ToString() == "1")
                            {
                                try
                                {
                                    var param = "marketid=" + placebetteen.mid + "&userid=" + placebetteen.uid + "&subsectionid=" + placebetteen.subid + "&gametype=" + placebetteen.gtype + "&subgametype="
                                                                     + placebetteen.subtype + "&username=" + ds.Tables[0].Rows[0]["username"] + "&sectionid=" + placebetteen.sid + "&bettype=" + placebetteen.btype
                                                                    + "&nation=" + ds1.Tables[0].Rows[0]["nation"].ToString() + "&userrate=" + placebetteen.urate + "&Masteramount=" + ds1.Tables[0].Rows[0]["conformamount"]
                                                                    + "&ipaddress=" + placebetteen.ip + "&browserdetail=" + placebetteen.bdetail + "&BookvalType=" + ds1.Tables[0].Rows[0]["exposertype"] +
                                                                    "&Bookvalmatch=" + ds1.Tables[0].Rows[0]["matchbook"] + "&BookvalFancy=" + ds1.Tables[0].Rows[0]["fancybook"] + "&amount=" + placebetteen.amt + "&frate=" + ds.Tables[0].Rows[0]["balrate"] + "&Currency=" + ds.Tables[0].Rows[0]["ccode"];

                                    Task.Factory.StartNew(() => Post(ConfigItems.AdminBookUrl + ApiEndpoint.vteenbook, param, "application/x-www-form-urlencoded", "POST"));
                                }
                                catch (System.Exception ex)
                                {
                                    WriteLogAll("Placebetvteen+bk" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                var parameters2 = new List<SqlParameter>();
                                parameters2.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                                parameters2.Add(new SqlParameter() { ParameterName = "Amount", Value = Convert.ToDouble(ds1.Tables[0].Rows[0]["userexposer"]) });
                                parameters2.Add(new SqlParameter() { ParameterName = "ExposJson", Value = ds1.Tables[0].Rows[0]["masterexposer"].ToString() });
                                parameters2.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                                parameters2.Add(new SqlParameter() { ParameterName = "gameType", Value = placebetteen.gtype });
                                parameters2.Add(new SqlParameter() { ParameterName = "Userrate", Value = ds1.Tables[0].Rows[0]["newuserrate"] });
                                parameters2.Add(new SqlParameter() { ParameterName = "BetAmount", Value = ds1.Tables[0].Rows[0]["conformamount"] });
                                ds2 = _sqlClientService.Execute("Casino_ExposerUpdate", ConfigItems.Conn_AccDgroup, parameters2);
                                WriteLogAll("Placebetvteen2+Casino_ExposerUpdate", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds2));
                                try
                                {
                                    if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 0 && ds2.Tables[1].Rows.Count > 0 && ds2.Tables[1].Columns.Contains("SendCoupon") && ds2.Tables[1].Rows[0]["SendCoupon"].ToString() == "1")
                                    {//Select 0 as SendCoupon,0 as BetAmount,'' as couponcode
                                        CouponPlacebet couponPlacebet = new CouponPlacebet();
                                        couponPlacebet.Amount = Convert.ToDouble(ds1.Tables[0].Rows[0]["conformamount"]);
                                        couponPlacebet.PlayerName = ds.Tables[0].Rows[0]["username"].ToString();
                                        couponPlacebet.PlayerId = placebetteen.pid;
                                        couponPlacebet.Product = ds2.Tables[1].Rows[0]["couponcode"].ToString();
                                        couponPlacebet.Balance = Convert.ToDouble(ds.Tables[0].Rows[0]["general"]);
                                        if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 1 && ds2.Tables[2].Rows.Count > 0 && ds2.Tables[2].Columns.Contains("clientid") && ds2.Tables[2].Columns.Contains("secretkey"))
                                        {
                                            var par = ConfigItems.Encrypt ? Encryptionbonus.EncryptString(JsonConvert.SerializeObject(couponPlacebet), ds2.Tables[2].Rows[0]["secretkey"].ToString()) : JsonConvert.SerializeObject(couponPlacebet);
                                            var resp = Postbonus(ConfigItems.CopUrl + "transaction/event", par, "application/json", "POST", ds2.Tables[2].Rows[0]["clientid"].ToString());
                                            var res = ConfigItems.Encrypt ? Encryptionbonus.DecryptString(resp, ds2.Tables[2].Rows[0]["secretkey"].ToString()) : resp;
                                            WriteLogAll("Placebetvteen3", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(res));
                                        }
                                        //var resp = Post(ConfigItems.CopUrl + ApiEndpoint.placebet, JsonConvert.SerializeObject(couponPlacebet), "application/json", "POST");
                                        //var data = JsonConvert.DeserializeObject<CouponPlacebetRes>(resp);

                                        //Returnds("0", data.error);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteLogAll("Placebetvteen+cop" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                Placebetfinal(placebetteen.uid);
                                return ds2;
                            }
                            else
                            {
                                Placebetfinal(placebetteen.uid);
                                return ds1;
                            }
                        }
                        Placebetfinal(placebetteen.uid);
                        return dscom;
                    }
                    Placebetfinal(placebetteen.uid);
                    return dscasino;
                }
                //Placebetfinal(placebetteen.uid);
                return ds;
            }
            catch (Exception ex)
            {
                Placebetfinal(placebetteen.uid);
                WriteLogAll("Placebetvteen" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                throw ex;
            }

        }
        public DataSet Placebetbaccarat(Placebetteen placebetteen)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet dscom = new DataSet();
                DataSet dscasino = new DataSet();
                placebetteen.gtype.ToLower();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                parameters.Add(new SqlParameter() { ParameterName = "MarketId", Value = placebetteen.mid });
                parameters.Add(new SqlParameter() { ParameterName = "GameType", Value = placebetteen.gtype });
                ds = _sqlClientService.Execute("Casino_PlacebetCheck", ConfigItems.Conn_AccDgroup, parameters);
                WriteLogAll("Placebetbaccarat+Casino_PlacebetCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds));

                //1 as Id,'ok' as msg,username,u.PartnershipType,general,ExposerLimit,oldexposer,AutoConfirm
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("id") && ds.Tables[0].Rows[0]["id"].ToString() == "1")
                {
                    var parameterscas = new List<SqlParameter>();
                    parameterscas.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                    parameterscas.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                    parameterscas.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                    parameterscas.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                    parameterscas.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                    dscasino = _sqlClientService.Execute("baccarat_placeExpoCheck", ConfigItems.Conn_Casinogroup, parameterscas);
                    WriteLogAll("Placebetbaccarat1+baccarat_placeExpoCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscasino));
                    if (dscasino != null && dscasino.Tables != null && dscasino.Tables.Count > 0 && dscasino.Tables[0].Rows.Count > 0 && dscasino.Tables[0].Columns.Contains("id") && dscasino.Tables[0].Rows[0]["id"].ToString() == "1")
                    {
                        var parameters3 = new List<SqlParameter>();
                        parameters3.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                        parameters3.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                        parameters3.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                        parameters3.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                        parameters3.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                        parameters3.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                        parameters3.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                        dscom = _sqlClientService.Execute("group2_baccarat_placebetconform", ConfigItems.Conn_Casino, parameters3);
                        WriteLogAll("Placebetbaccarat2+group2_baccarat_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscom));
                        if (dscom != null && dscom.Tables != null && dscom.Tables.Count > 0 && dscom.Tables[0].Rows.Count > 0 && dscom.Tables[0].Columns.Contains("id") && dscom.Tables[0].Rows[0]["id"].ToString() == "1")
                        {
                            var parameters1 = new List<SqlParameter>();
                            parameters1.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                            parameters1.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                            parameters1.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                            parameters1.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                            parameters1.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                            parameters1.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                            parameters1.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                            parameters1.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                            parameters1.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newUserrate", Value = dscom.Tables[0].Rows[0]["NewUserrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newAmount", Value = dscom.Tables[0].Rows[0]["Amount"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Gameid", Value = dscom.Tables[0].Rows[0]["Gameid"] });
                            ds1 = _sqlClientService.Execute("baccarat_placebetconform", ConfigItems.Conn_Casinogroup, parameters1);
                            WriteLogAll("Placebetbaccarat3+baccarat_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds1));

                            //select 1 as ID,'Ok' as Msg,	@userexposer as userexposer, @JsonGameSub as masterexposer
                            if (ds1 != null && ds1.Tables != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0 && ds1.Tables[0].Columns.Contains("id") && ds1.Tables[0].Rows[0]["id"].ToString() == "1")
                            {
                                try
                                {
                                    var param = "marketid=" + placebetteen.mid + "&userid=" + placebetteen.uid + "&subsectionid=" + placebetteen.subid + "&gametype=" + placebetteen.gtype + "&subgametype="
                                                                     + placebetteen.subtype + "&username=" + ds.Tables[0].Rows[0]["username"] + "&sectionid=" + placebetteen.sid + "&bettype=" + placebetteen.btype
                                                                    + "&nation=" + ds1.Tables[0].Rows[0]["nation"].ToString() + "&userrate=" + placebetteen.urate + "&Masteramount=" + ds1.Tables[0].Rows[0]["conformamount"]
                                                                    + "&ipaddress=" + placebetteen.ip + "&browserdetail=" + placebetteen.bdetail + "&BookvalType=" + ds1.Tables[0].Rows[0]["exposertype"] +
                                                                    "&Bookvalmatch=" + ds1.Tables[0].Rows[0]["matchbook"] + "&BookvalFancy=" + ds1.Tables[0].Rows[0]["fancybook"] + "&amount=" + placebetteen.amt + "&frate=" + ds.Tables[0].Rows[0]["balrate"] + "&Currency=" + ds.Tables[0].Rows[0]["ccode"];

                                    Task.Factory.StartNew(() => Post(ConfigItems.AdminBookUrl + ApiEndpoint.baccaratbook, param, "application/x-www-form-urlencoded", "POST"));
                                }
                                catch (System.Exception ex)
                                {
                                    WriteLogAll("Placebetbaccarat+bk" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                var parameters2 = new List<SqlParameter>();
                                parameters2.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                                parameters2.Add(new SqlParameter() { ParameterName = "Amount", Value = Convert.ToDouble(ds1.Tables[0].Rows[0]["userexposer"]) });
                                parameters2.Add(new SqlParameter() { ParameterName = "ExposJson", Value = ds1.Tables[0].Rows[0]["masterexposer"].ToString() });
                                parameters2.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                                parameters2.Add(new SqlParameter() { ParameterName = "gameType", Value = placebetteen.gtype });
                                parameters2.Add(new SqlParameter() { ParameterName = "Userrate", Value = ds1.Tables[0].Rows[0]["newuserrate"] });
                                parameters2.Add(new SqlParameter() { ParameterName = "BetAmount", Value = ds1.Tables[0].Rows[0]["conformamount"] });
                                ds2 = _sqlClientService.Execute("Casino_ExposerUpdate", ConfigItems.Conn_AccDgroup, parameters2);
                                WriteLogAll("Placebetbaccarat4+Casino_ExposerUpdate", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds2));
                                try
                                {
                                    if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 0 && ds2.Tables[1].Rows.Count > 0 && ds2.Tables[1].Columns.Contains("SendCoupon") && ds2.Tables[1].Rows[0]["SendCoupon"].ToString() == "1")
                                    {//Select 0 as SendCoupon,0 as BetAmount,'' as couponcode
                                        CouponPlacebet couponPlacebet = new CouponPlacebet();
                                        couponPlacebet.Amount = Convert.ToDouble(ds1.Tables[0].Rows[0]["conformamount"]);
                                        couponPlacebet.PlayerName = ds.Tables[0].Rows[0]["username"].ToString();
                                        couponPlacebet.PlayerId = placebetteen.pid;
                                        couponPlacebet.Product = ds2.Tables[1].Rows[0]["couponcode"].ToString();
                                        couponPlacebet.Balance = Convert.ToDouble(ds.Tables[0].Rows[0]["general"]);
                                        if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 1 && ds2.Tables[2].Rows.Count > 0 && ds2.Tables[2].Columns.Contains("clientid") && ds2.Tables[2].Columns.Contains("secretkey"))
                                        {
                                            var par = ConfigItems.Encrypt ? Encryptionbonus.EncryptString(JsonConvert.SerializeObject(couponPlacebet), ds2.Tables[2].Rows[0]["secretkey"].ToString()) : JsonConvert.SerializeObject(couponPlacebet);
                                            var resp = Postbonus(ConfigItems.CopUrl + "transaction/event", par, "application/json", "POST", ds2.Tables[2].Rows[0]["clientid"].ToString());
                                            var res = ConfigItems.Encrypt ? Encryptionbonus.DecryptString(resp, ds2.Tables[2].Rows[0]["secretkey"].ToString()) : resp;
                                            WriteLogAll("Placebetbaccarat3", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(res));
                                        }
                                        //var resp = Post(ConfigItems.CopUrl + ApiEndpoint.placebet, JsonConvert.SerializeObject(couponPlacebet), "application/json", "POST");
                                        //var data = JsonConvert.DeserializeObject<CouponPlacebetRes>(resp);

                                        //Returnds("0", data.error);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteLogAll("Placebetbaccarat+cop" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                Placebetfinal(placebetteen.uid);
                                return ds2;
                            }
                            else
                            {
                                Placebetfinal(placebetteen.uid);
                                return ds1;
                            }
                        }
                        Placebetfinal(placebetteen.uid);
                        return dscom;
                    }
                    Placebetfinal(placebetteen.uid);
                    return dscasino;
                }
                //Placebetfinal(placebetteen.uid);
                return ds;
            }
            catch (Exception ex)
            {
                Placebetfinal(placebetteen.uid);
                WriteLogAll("Placebetbaccarat" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                throw ex;
            }

        }
        public DataSet Placebetlucky7(Placebetteen placebetteen)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet dscom = new DataSet();
                DataSet dscasino = new DataSet();
                placebetteen.gtype.ToLower();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                parameters.Add(new SqlParameter() { ParameterName = "MarketId", Value = placebetteen.mid });
                parameters.Add(new SqlParameter() { ParameterName = "GameType", Value = placebetteen.gtype });
                ds = _sqlClientService.Execute("Casino_PlacebetCheck", ConfigItems.Conn_AccDgroup, parameters);
                WriteLogAll("Placebetlucky7+Casino_PlacebetCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds));

                //1 as Id,'ok' as msg,username,u.PartnershipType,general,ExposerLimit,oldexposer,AutoConfirm
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("id") && ds.Tables[0].Rows[0]["id"].ToString() == "1")
                {
                    var parameterscas = new List<SqlParameter>();
                    parameterscas.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                    parameterscas.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                    parameterscas.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                    parameterscas.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                    parameterscas.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                    dscasino = _sqlClientService.Execute("lucky7_placeExpoCheck", ConfigItems.Conn_Casinogroup, parameterscas);
                    WriteLogAll("Placebetlucky71+lucky7_placeExpoCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscasino));
                    if (dscasino != null && dscasino.Tables != null && dscasino.Tables.Count > 0 && dscasino.Tables[0].Rows.Count > 0 && dscasino.Tables[0].Columns.Contains("id") && dscasino.Tables[0].Rows[0]["id"].ToString() == "1")
                    {
                        var parameters3 = new List<SqlParameter>();
                        parameters3.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                        parameters3.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                        parameters3.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                        parameters3.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                        parameters3.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                        parameters3.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                        parameters3.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                        dscom = _sqlClientService.Execute("group2_lucky7_placebetconform", ConfigItems.Conn_Casino, parameters3);
                        WriteLogAll("Placebetlucky72+group2_lucky7_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscom));
                        if (dscom != null && dscom.Tables != null && dscom.Tables.Count > 0 && dscom.Tables[0].Rows.Count > 0 && dscom.Tables[0].Columns.Contains("id") && dscom.Tables[0].Rows[0]["id"].ToString() == "1")
                        {
                            var parameters1 = new List<SqlParameter>();
                            parameters1.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                            parameters1.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                            parameters1.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                            parameters1.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                            parameters1.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                            parameters1.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                            parameters1.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                            parameters1.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                            parameters1.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newUserrate", Value = dscom.Tables[0].Rows[0]["NewUserrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newAmount", Value = dscom.Tables[0].Rows[0]["Amount"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Gameid", Value = dscom.Tables[0].Rows[0]["Gameid"] });
                            ds1 = _sqlClientService.Execute("lucky7_placebetconform", ConfigItems.Conn_Casinogroup, parameters1);
                            WriteLogAll("Placebetlucky73+lucky7_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds1));

                            //select 1 as ID,'Ok' as Msg,	@userexposer as userexposer, @JsonGameSub as masterexposer
                            if (ds1 != null && ds1.Tables != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0 && ds1.Tables[0].Columns.Contains("id") && ds1.Tables[0].Rows[0]["id"].ToString() == "1")
                            {
                                try
                                {
                                    var param = "marketid=" + placebetteen.mid + "&userid=" + placebetteen.uid + "&subsectionid=" + placebetteen.subid + "&gametype=" + placebetteen.gtype + "&subgametype="
                                                                     + placebetteen.subtype + "&username=" + ds.Tables[0].Rows[0]["username"] + "&sectionid=" + placebetteen.sid + "&bettype=" + placebetteen.btype
                                                                    + "&nation=" + ds1.Tables[0].Rows[0]["nation"].ToString() + "&userrate=" + placebetteen.urate + "&Masteramount=" + ds1.Tables[0].Rows[0]["conformamount"]
                                                                    + "&ipaddress=" + placebetteen.ip + "&browserdetail=" + placebetteen.bdetail + "&BookvalType=" + ds1.Tables[0].Rows[0]["exposertype"] +
                                                                    "&Bookvalmatch=" + ds1.Tables[0].Rows[0]["matchbook"] + "&BookvalFancy=" + ds1.Tables[0].Rows[0]["fancybook"] + "&amount=" + placebetteen.amt + "&frate=" + ds.Tables[0].Rows[0]["balrate"] + "&Currency=" + ds.Tables[0].Rows[0]["ccode"];

                                    Task.Factory.StartNew(() => Post(ConfigItems.AdminBookUrl + ApiEndpoint.lucky7book, param, "application/x-www-form-urlencoded", "POST"));
                                }
                                catch (System.Exception ex)
                                {
                                    WriteLogAll("Placebetlucky+bk" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                var parameters2 = new List<SqlParameter>();
                                parameters2.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                                parameters2.Add(new SqlParameter() { ParameterName = "Amount", Value = Convert.ToDouble(ds1.Tables[0].Rows[0]["userexposer"]) });
                                parameters2.Add(new SqlParameter() { ParameterName = "ExposJson", Value = ds1.Tables[0].Rows[0]["masterexposer"].ToString() });
                                parameters2.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                                parameters2.Add(new SqlParameter() { ParameterName = "gameType", Value = placebetteen.gtype });
                                parameters2.Add(new SqlParameter() { ParameterName = "Userrate", Value = ds1.Tables[0].Rows[0]["newuserrate"] });
                                parameters2.Add(new SqlParameter() { ParameterName = "BetAmount", Value = ds1.Tables[0].Rows[0]["conformamount"] });
                                ds2 = _sqlClientService.Execute("Casino_ExposerUpdate", ConfigItems.Conn_AccDgroup, parameters2);
                                WriteLogAll("Placebetlucky74+Casino_ExposerUpdate", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds2));
                                try
                                {
                                    if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 0 && ds2.Tables[1].Rows.Count > 0 && ds2.Tables[1].Columns.Contains("SendCoupon") && ds2.Tables[1].Rows[0]["SendCoupon"].ToString() == "1")
                                    {//Select 0 as SendCoupon,0 as BetAmount,'' as couponcode
                                        CouponPlacebet couponPlacebet = new CouponPlacebet();
                                        couponPlacebet.Amount = Convert.ToDouble(ds1.Tables[0].Rows[0]["conformamount"]);
                                        couponPlacebet.PlayerName = ds.Tables[0].Rows[0]["username"].ToString();
                                        couponPlacebet.PlayerId = placebetteen.pid;
                                        couponPlacebet.Product = ds2.Tables[1].Rows[0]["couponcode"].ToString();
                                        couponPlacebet.Balance = Convert.ToDouble(ds.Tables[0].Rows[0]["general"]);
                                        if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 1 && ds2.Tables[2].Rows.Count > 0 && ds2.Tables[2].Columns.Contains("clientid") && ds2.Tables[2].Columns.Contains("secretkey"))
                                        {
                                            var par = ConfigItems.Encrypt ? Encryptionbonus.EncryptString(JsonConvert.SerializeObject(couponPlacebet), ds2.Tables[2].Rows[0]["secretkey"].ToString()) : JsonConvert.SerializeObject(couponPlacebet);
                                            var resp = Postbonus(ConfigItems.CopUrl + "transaction/event", par, "application/json", "POST", ds2.Tables[2].Rows[0]["clientid"].ToString());
                                            var res = ConfigItems.Encrypt ? Encryptionbonus.DecryptString(resp, ds2.Tables[2].Rows[0]["secretkey"].ToString()) : resp;
                                            WriteLogAll("Placebetlucky73", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(res));
                                        }
                                        //var resp = Post(ConfigItems.CopUrl + ApiEndpoint.placebet, JsonConvert.SerializeObject(couponPlacebet), "application/json", "POST");
                                        //var data = JsonConvert.DeserializeObject<CouponPlacebetRes>(resp);

                                        //Returnds("0", data.error);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteLogAll("Placebetlucky7+cop" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                Placebetfinal(placebetteen.uid);
                                return ds2;
                            }
                            else
                            {
                                Placebetfinal(placebetteen.uid);
                                return ds1;
                            }
                        }
                        Placebetfinal(placebetteen.uid);
                        return dscom;
                    }
                    Placebetfinal(placebetteen.uid);
                    return dscasino;
                }
                //Placebetfinal(placebetteen.uid);
                return ds;
            }
            catch (Exception ex)
            {
                Placebetfinal(placebetteen.uid);
                WriteLogAll("Placebetlucky7" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                throw ex;
            }

        }
        public DataSet Placebetvlucky7(Placebetteen placebetteen)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet dscom = new DataSet();
                DataSet dscasino = new DataSet();
                placebetteen.gtype.ToLower();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                parameters.Add(new SqlParameter() { ParameterName = "MarketId", Value = placebetteen.mid });
                parameters.Add(new SqlParameter() { ParameterName = "GameType", Value = placebetteen.gtype });
                ds = _sqlClientService.Execute("Casino_PlacebetCheck", ConfigItems.Conn_AccDgroup, parameters);
                WriteLogAll("Placebetvlucky7+Casino_PlacebetCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds));

                //1 as Id,'ok' as msg,username,u.PartnershipType,general,ExposerLimit,oldexposer,AutoConfirm
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("id") && ds.Tables[0].Rows[0]["id"].ToString() == "1")
                {
                    var parameterscas = new List<SqlParameter>();
                    parameterscas.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                    parameterscas.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                    parameterscas.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                    parameterscas.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                    parameterscas.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                    dscasino = _sqlClientService.Execute("Z_lucky7_placeExpoCheck", ConfigItems.Conn_Casinogroup, parameterscas);
                    WriteLogAll("Placebetvlucky71+Z_lucky7_placeExpoCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscasino));
                    if (dscasino != null && dscasino.Tables != null && dscasino.Tables.Count > 0 && dscasino.Tables[0].Rows.Count > 0 && dscasino.Tables[0].Columns.Contains("id") && dscasino.Tables[0].Rows[0]["id"].ToString() == "1")
                    {
                        var parameters3 = new List<SqlParameter>();
                        parameters3.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                        parameters3.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                        parameters3.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                        parameters3.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                        parameters3.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                        parameters3.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                        parameters3.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                        dscom = _sqlClientService.Execute("group2_Z_lucky7_placebetconform", ConfigItems.Conn_Casino, parameters3);
                        WriteLogAll("Placebetvlucky72+group2_Z_lucky7_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscom));
                        if (dscom != null && dscom.Tables != null && dscom.Tables.Count > 0 && dscom.Tables[0].Rows.Count > 0 && dscom.Tables[0].Columns.Contains("id") && dscom.Tables[0].Rows[0]["id"].ToString() == "1")
                        {
                            var parameters1 = new List<SqlParameter>();
                            parameters1.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                            parameters1.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                            parameters1.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                            parameters1.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                            parameters1.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                            parameters1.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                            parameters1.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                            parameters1.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                            parameters1.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newUserrate", Value = dscom.Tables[0].Rows[0]["NewUserrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newAmount", Value = dscom.Tables[0].Rows[0]["Amount"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Gameid", Value = dscom.Tables[0].Rows[0]["Gameid"] });
                            ds1 = _sqlClientService.Execute("Z_lucky7_placebetconform", ConfigItems.Conn_Casinogroup, parameters1);
                            WriteLogAll("Placebetvlucky73+Z_lucky7_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds1));

                            //select 1 as ID,'Ok' as Msg,	@userexposer as userexposer, @JsonGameSub as masterexposer
                            if (ds1 != null && ds1.Tables != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0 && ds1.Tables[0].Columns.Contains("id") && ds1.Tables[0].Rows[0]["id"].ToString() == "1")
                            {
                                try
                                {
                                    var param = "marketid=" + placebetteen.mid + "&userid=" + placebetteen.uid + "&subsectionid=" + placebetteen.subid + "&gametype=" + placebetteen.gtype + "&subgametype="
                                                                     + placebetteen.subtype + "&username=" + ds.Tables[0].Rows[0]["username"] + "&sectionid=" + placebetteen.sid + "&bettype=" + placebetteen.btype
                                                                    + "&nation=" + ds1.Tables[0].Rows[0]["nation"].ToString() + "&userrate=" + placebetteen.urate + "&Masteramount=" + ds1.Tables[0].Rows[0]["conformamount"]
                                                                    + "&ipaddress=" + placebetteen.ip + "&browserdetail=" + placebetteen.bdetail + "&BookvalType=" + ds1.Tables[0].Rows[0]["exposertype"] +
                                                                    "&Bookvalmatch=" + ds1.Tables[0].Rows[0]["matchbook"] + "&BookvalFancy=" + ds1.Tables[0].Rows[0]["fancybook"] + "&amount=" + placebetteen.amt + "&frate=" + ds.Tables[0].Rows[0]["balrate"] + "&Currency=" + ds.Tables[0].Rows[0]["ccode"];

                                    Task.Factory.StartNew(() => Post(ConfigItems.AdminBookUrl + ApiEndpoint.vlucky7book, param, "application/x-www-form-urlencoded", "POST"));
                                }
                                catch (System.Exception ex)
                                {
                                    WriteLogAll("Placebetvlucky+bk" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                var parameters2 = new List<SqlParameter>();
                                parameters2.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                                parameters2.Add(new SqlParameter() { ParameterName = "Amount", Value = Convert.ToDouble(ds1.Tables[0].Rows[0]["userexposer"]) });
                                parameters2.Add(new SqlParameter() { ParameterName = "ExposJson", Value = ds1.Tables[0].Rows[0]["masterexposer"].ToString() });
                                parameters2.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                                parameters2.Add(new SqlParameter() { ParameterName = "gameType", Value = placebetteen.gtype });
                                parameters2.Add(new SqlParameter() { ParameterName = "Userrate", Value = ds1.Tables[0].Rows[0]["newuserrate"] });
                                parameters2.Add(new SqlParameter() { ParameterName = "BetAmount", Value = ds1.Tables[0].Rows[0]["conformamount"] });
                                ds2 = _sqlClientService.Execute("Casino_ExposerUpdate", ConfigItems.Conn_AccDgroup, parameters2);
                                WriteLogAll("Placebetvlucky74+Casino_ExposerUpdate", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds2));
                                try
                                {
                                    if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 0 && ds2.Tables[1].Rows.Count > 0 && ds2.Tables[1].Columns.Contains("SendCoupon") && ds2.Tables[1].Rows[0]["SendCoupon"].ToString() == "1")
                                    {//Select 0 as SendCoupon,0 as BetAmount,'' as couponcode
                                        CouponPlacebet couponPlacebet = new CouponPlacebet();
                                        couponPlacebet.Amount = Convert.ToDouble(ds1.Tables[0].Rows[0]["conformamount"]);
                                        couponPlacebet.PlayerName = ds.Tables[0].Rows[0]["username"].ToString();
                                        couponPlacebet.PlayerId = placebetteen.pid;
                                        couponPlacebet.Product = ds2.Tables[1].Rows[0]["couponcode"].ToString();
                                        couponPlacebet.Balance = Convert.ToDouble(ds.Tables[0].Rows[0]["general"]);
                                        if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 1 && ds2.Tables[2].Rows.Count > 0 && ds2.Tables[2].Columns.Contains("clientid") && ds2.Tables[2].Columns.Contains("secretkey"))
                                        {
                                            var par = ConfigItems.Encrypt ? Encryptionbonus.EncryptString(JsonConvert.SerializeObject(couponPlacebet), ds2.Tables[2].Rows[0]["secretkey"].ToString()) : JsonConvert.SerializeObject(couponPlacebet);
                                            var resp = Postbonus(ConfigItems.CopUrl + "transaction/event", par, "application/json", "POST", ds2.Tables[2].Rows[0]["clientid"].ToString());
                                            var res = ConfigItems.Encrypt ? Encryptionbonus.DecryptString(resp, ds2.Tables[2].Rows[0]["secretkey"].ToString()) : resp;
                                            WriteLogAll("Placebetvlucky73", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(res));
                                        }
                                        //var resp = Post(ConfigItems.CopUrl + ApiEndpoint.placebet, JsonConvert.SerializeObject(couponPlacebet), "application/json", "POST");
                                        //var data = JsonConvert.DeserializeObject<CouponPlacebetRes>(resp);

                                        //Returnds("0", data.error);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteLogAll("Placebetvlucky7+cop" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                Placebetfinal(placebetteen.uid);
                                return ds2;
                            }
                            else
                            {
                                Placebetfinal(placebetteen.uid);
                                return ds1;
                            }
                        }
                        Placebetfinal(placebetteen.uid);
                        return dscom;
                    }
                    Placebetfinal(placebetteen.uid);
                    return dscasino;
                }
                //Placebetfinal(placebetteen.uid);
                return ds;
            }
            catch (Exception ex)
            {
                Placebetfinal(placebetteen.uid);
                WriteLogAll("Placebetvlucky7" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                throw ex;
            }

        }
        public DataSet Placebetpoker(Placebetteen placebetteen)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet dscom = new DataSet();
                DataSet dscasino = new DataSet();
                placebetteen.gtype.ToLower();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                parameters.Add(new SqlParameter() { ParameterName = "MarketId", Value = placebetteen.mid });
                parameters.Add(new SqlParameter() { ParameterName = "GameType", Value = placebetteen.gtype });
                ds = _sqlClientService.Execute("Casino_PlacebetCheck", ConfigItems.Conn_AccDgroup, parameters);
                WriteLogAll("Placebetpoker+Casino_PlacebetCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds));

                //1 as Id,'ok' as msg,username,u.PartnershipType,general,ExposerLimit,oldexposer,AutoConfirm
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("id") && ds.Tables[0].Rows[0]["id"].ToString() == "1")
                {
                    var parameterscas = new List<SqlParameter>();
                    parameterscas.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                    parameterscas.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                    parameterscas.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                    parameterscas.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                    parameterscas.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                    dscasino = _sqlClientService.Execute("poker_placeExpoCheck", ConfigItems.Conn_Casinogroup, parameterscas);
                    WriteLogAll("Placebetpoker1+poker_placeExpoCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscasino));
                    if (dscasino != null && dscasino.Tables != null && dscasino.Tables.Count > 0 && dscasino.Tables[0].Rows.Count > 0 && dscasino.Tables[0].Columns.Contains("id") && dscasino.Tables[0].Rows[0]["id"].ToString() == "1")
                    {
                        var parameters3 = new List<SqlParameter>();
                        parameters3.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                        parameters3.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                        parameters3.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                        parameters3.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                        parameters3.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                        parameters3.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                        parameters3.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                        dscom = _sqlClientService.Execute("group2_poker_placebetconform", ConfigItems.Conn_Casino, parameters3);
                        WriteLogAll("Placebetpoker2+group2_poker_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscom));
                        if (dscom != null && dscom.Tables != null && dscom.Tables.Count > 0 && dscom.Tables[0].Rows.Count > 0 && dscom.Tables[0].Columns.Contains("id") && dscom.Tables[0].Rows[0]["id"].ToString() == "1")
                        {
                            var parameters1 = new List<SqlParameter>();
                            parameters1.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                            parameters1.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                            parameters1.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                            parameters1.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                            parameters1.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                            parameters1.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                            parameters1.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                            parameters1.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                            parameters1.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newUserrate", Value = dscom.Tables[0].Rows[0]["NewUserrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newAmount", Value = dscom.Tables[0].Rows[0]["Amount"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Gameid", Value = dscom.Tables[0].Rows[0]["Gameid"] });
                            ds1 = _sqlClientService.Execute("poker_placebetconform", ConfigItems.Conn_Casinogroup, parameters1);
                            WriteLogAll("Placebetpoker3+poker_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds1));

                            //select 1 as ID,'Ok' as Msg,	@userexposer as userexposer, @JsonGameSub as masterexposer
                            if (ds1 != null && ds1.Tables != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0 && ds1.Tables[0].Columns.Contains("id") && ds1.Tables[0].Rows[0]["id"].ToString() == "1")
                            {
                                try
                                {
                                    var param = "marketid=" + placebetteen.mid + "&userid=" + placebetteen.uid + "&subsectionid=" + placebetteen.subid + "&gametype=" + placebetteen.gtype + "&subgametype="
                                                                     + placebetteen.subtype + "&username=" + ds.Tables[0].Rows[0]["username"] + "&sectionid=" + placebetteen.sid + "&bettype=" + placebetteen.btype
                                                                    + "&nation=" + ds1.Tables[0].Rows[0]["nation"].ToString() + "&userrate=" + placebetteen.urate + "&Masteramount=" + ds1.Tables[0].Rows[0]["conformamount"]
                                                                    + "&ipaddress=" + placebetteen.ip + "&browserdetail=" + placebetteen.bdetail + "&BookvalType=" + ds1.Tables[0].Rows[0]["exposertype"] +
                                                                    "&Bookvalmatch=" + ds1.Tables[0].Rows[0]["matchbook"] + "&BookvalFancy=" + ds1.Tables[0].Rows[0]["fancybook"] + "&amount=" + placebetteen.amt + "&frate=" + ds.Tables[0].Rows[0]["balrate"] + "&Currency=" + ds.Tables[0].Rows[0]["ccode"];

                                    Task.Factory.StartNew(() => Post(ConfigItems.AdminBookUrl + ApiEndpoint.pokerbook, param, "application/x-www-form-urlencoded", "POST"));
                                }
                                catch (System.Exception ex)
                                {
                                    WriteLogAll("Placebetpoker+bk" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                var parameters2 = new List<SqlParameter>();
                                parameters2.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                                parameters2.Add(new SqlParameter() { ParameterName = "Amount", Value = Convert.ToDouble(ds1.Tables[0].Rows[0]["userexposer"]) });
                                parameters2.Add(new SqlParameter() { ParameterName = "ExposJson", Value = ds1.Tables[0].Rows[0]["masterexposer"].ToString() });
                                parameters2.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                                parameters2.Add(new SqlParameter() { ParameterName = "gameType", Value = placebetteen.gtype });
                                parameters2.Add(new SqlParameter() { ParameterName = "Userrate", Value = ds1.Tables[0].Rows[0]["newuserrate"] });
                                parameters2.Add(new SqlParameter() { ParameterName = "BetAmount", Value = ds1.Tables[0].Rows[0]["conformamount"] });
                                ds2 = _sqlClientService.Execute("Casino_ExposerUpdate", ConfigItems.Conn_AccDgroup, parameters2);
                                WriteLogAll("Placebetpoker4+Casino_ExposerUpdate", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds2));
                                try
                                {
                                    if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 0 && ds2.Tables[1].Rows.Count > 0 && ds2.Tables[1].Columns.Contains("SendCoupon") && ds2.Tables[1].Rows[0]["SendCoupon"].ToString() == "1")
                                    {//Select 0 as SendCoupon,0 as BetAmount,'' as couponcode
                                        CouponPlacebet couponPlacebet = new CouponPlacebet();
                                        couponPlacebet.Amount = Convert.ToDouble(ds1.Tables[0].Rows[0]["conformamount"]);
                                        couponPlacebet.PlayerName = ds.Tables[0].Rows[0]["username"].ToString();
                                        couponPlacebet.PlayerId = placebetteen.pid;
                                        couponPlacebet.Product = ds2.Tables[1].Rows[0]["couponcode"].ToString();
                                        couponPlacebet.Balance = Convert.ToDouble(ds.Tables[0].Rows[0]["general"]);
                                        if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 1 && ds2.Tables[2].Rows.Count > 0 && ds2.Tables[2].Columns.Contains("clientid") && ds2.Tables[2].Columns.Contains("secretkey"))
                                        {
                                            var par = ConfigItems.Encrypt ? Encryptionbonus.EncryptString(JsonConvert.SerializeObject(couponPlacebet), ds2.Tables[2].Rows[0]["secretkey"].ToString()) : JsonConvert.SerializeObject(couponPlacebet);
                                            var resp = Postbonus(ConfigItems.CopUrl + "transaction/event", par, "application/json", "POST", ds2.Tables[2].Rows[0]["clientid"].ToString());
                                            var res = ConfigItems.Encrypt ? Encryptionbonus.DecryptString(resp, ds2.Tables[2].Rows[0]["secretkey"].ToString()) : resp;
                                            WriteLogAll("Placebetpoker3", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(res));
                                        }
                                        //var resp = Post(ConfigItems.CopUrl + ApiEndpoint.placebet, JsonConvert.SerializeObject(couponPlacebet), "application/json", "POST");
                                        //var data = JsonConvert.DeserializeObject<CouponPlacebetRes>(resp);

                                        //Returnds("0", data.error);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteLogAll("Placebetpoker+cop" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                Placebetfinal(placebetteen.uid);
                                return ds2;
                            }
                            else
                            {
                                Placebetfinal(placebetteen.uid);
                                return ds1;
                            }
                        }
                        Placebetfinal(placebetteen.uid);
                        return dscom;
                    }
                    Placebetfinal(placebetteen.uid);
                    return dscasino;
                }
                //Placebetfinal(placebetteen.uid);
                return ds;
            }
            catch (Exception ex)
            {
                Placebetfinal(placebetteen.uid);
                WriteLogAll("Placebetpoker" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                throw ex;
            }

        }
        public DataSet Placebetdt(Placebetteen placebetteen)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet dscom = new DataSet();
                DataSet dscasino = new DataSet();
                placebetteen.gtype.ToLower();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                parameters.Add(new SqlParameter() { ParameterName = "MarketId", Value = placebetteen.mid });
                parameters.Add(new SqlParameter() { ParameterName = "GameType", Value = placebetteen.gtype });
                ds = _sqlClientService.Execute("Casino_PlacebetCheck", ConfigItems.Conn_AccDgroup, parameters);
                WriteLogAll("Placebetdt+Casino_PlacebetCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds));

                //1 as Id,'ok' as msg,username,u.PartnershipType,general,ExposerLimit,oldexposer,AutoConfirm
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("id") && ds.Tables[0].Rows[0]["id"].ToString() == "1")
                {
                    var parameterscas = new List<SqlParameter>();
                    parameterscas.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                    parameterscas.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                    parameterscas.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                    parameterscas.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                    parameterscas.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                    dscasino = _sqlClientService.Execute("dt_placeExpoCheck", ConfigItems.Conn_Casinogroup, parameterscas);
                    WriteLogAll("Placebetdt1+dt_placeExpoCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscasino));
                    if (dscasino != null && dscasino.Tables != null && dscasino.Tables.Count > 0 && dscasino.Tables[0].Rows.Count > 0 && dscasino.Tables[0].Columns.Contains("id") && dscasino.Tables[0].Rows[0]["id"].ToString() == "1")
                    {
                        var parameters3 = new List<SqlParameter>();
                        parameters3.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                        parameters3.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                        parameters3.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                        parameters3.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                        parameters3.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                        parameters3.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                        parameters3.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                        dscom = _sqlClientService.Execute("group2_dt_placebetconform", ConfigItems.Conn_Casino, parameters3);
                        WriteLogAll("Placebetdt2+group2_dt_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscom));
                        if (dscom != null && dscom.Tables != null && dscom.Tables.Count > 0 && dscom.Tables[0].Rows.Count > 0 && dscom.Tables[0].Columns.Contains("id") && dscom.Tables[0].Rows[0]["id"].ToString() == "1")
                        {
                            var parameters1 = new List<SqlParameter>();
                            parameters1.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                            parameters1.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                            parameters1.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                            parameters1.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                            parameters1.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                            parameters1.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                            parameters1.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                            parameters1.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                            parameters1.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newUserrate", Value = dscom.Tables[0].Rows[0]["NewUserrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newAmount", Value = dscom.Tables[0].Rows[0]["Amount"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Gameid", Value = dscom.Tables[0].Rows[0]["Gameid"] });
                            ds1 = _sqlClientService.Execute("dt_placebetconform", ConfigItems.Conn_Casinogroup, parameters1);
                            WriteLogAll("Placebetdt3+dt_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds1));
                            //select 1 as ID,'Ok' as Msg,	@userexposer as userexposer, @JsonGameSub as masterexposer
                            if (ds1 != null && ds1.Tables != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0 && ds1.Tables[0].Columns.Contains("id") && ds1.Tables[0].Rows[0]["id"].ToString() == "1")
                            {
                                try
                                {
                                    var param = "marketid=" + placebetteen.mid + "&userid=" + placebetteen.uid + "&subsectionid=" + placebetteen.subid + "&gametype=" + placebetteen.gtype + "&subgametype="
                                                                     + placebetteen.subtype + "&username=" + ds.Tables[0].Rows[0]["username"] + "&sectionid=" + placebetteen.sid + "&bettype=" + placebetteen.btype
                                                                    + "&nation=" + ds1.Tables[0].Rows[0]["nation"].ToString() + "&userrate=" + placebetteen.urate + "&Masteramount=" + ds1.Tables[0].Rows[0]["conformamount"]
                                                                    + "&ipaddress=" + placebetteen.ip + "&browserdetail=" + placebetteen.bdetail + "&BookvalType=" + ds1.Tables[0].Rows[0]["exposertype"] +
                                                                    "&Bookvalmatch=" + ds1.Tables[0].Rows[0]["matchbook"] + "&BookvalFancy=" + ds1.Tables[0].Rows[0]["fancybook"] + "&amount=" + placebetteen.amt + "&frate=" + ds.Tables[0].Rows[0]["balrate"] + "&Currency=" + ds.Tables[0].Rows[0]["ccode"];

                                    Task.Factory.StartNew(() => Post(ConfigItems.AdminBookUrl + ApiEndpoint.dtbook, param, "application/x-www-form-urlencoded", "POST"));
                                }
                                catch (System.Exception ex)
                                {
                                    WriteLogAll("Placebetdt+bk" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                var parameters2 = new List<SqlParameter>();
                                parameters2.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                                parameters2.Add(new SqlParameter() { ParameterName = "Amount", Value = Convert.ToDouble(ds1.Tables[0].Rows[0]["userexposer"]) });
                                parameters2.Add(new SqlParameter() { ParameterName = "ExposJson", Value = ds1.Tables[0].Rows[0]["masterexposer"].ToString() });
                                parameters2.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                                parameters2.Add(new SqlParameter() { ParameterName = "gameType", Value = placebetteen.gtype });
                                parameters2.Add(new SqlParameter() { ParameterName = "Userrate", Value = ds1.Tables[0].Rows[0]["newuserrate"] });
                                parameters2.Add(new SqlParameter() { ParameterName = "BetAmount", Value = ds1.Tables[0].Rows[0]["conformamount"] });
                                ds2 = _sqlClientService.Execute("Casino_ExposerUpdate", ConfigItems.Conn_AccDgroup, parameters2);
                                WriteLogAll("Placebetdt4+Casino_ExposerUpdate", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds2));
                                try
                                {
                                    if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 0 && ds2.Tables[1].Rows.Count > 0 && ds2.Tables[1].Columns.Contains("SendCoupon") && ds2.Tables[1].Rows[0]["SendCoupon"].ToString() == "1")
                                    {//Select 0 as SendCoupon,0 as BetAmount,'' as couponcode
                                        CouponPlacebet couponPlacebet = new CouponPlacebet();
                                        couponPlacebet.Amount = Convert.ToDouble(ds1.Tables[0].Rows[0]["conformamount"]);
                                        couponPlacebet.PlayerName = ds.Tables[0].Rows[0]["username"].ToString();
                                        couponPlacebet.PlayerId = placebetteen.pid;
                                        couponPlacebet.Product = ds2.Tables[1].Rows[0]["couponcode"].ToString();
                                        couponPlacebet.Balance = Convert.ToDouble(ds.Tables[0].Rows[0]["general"]);
                                        if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 1 && ds2.Tables[2].Rows.Count > 0 && ds2.Tables[2].Columns.Contains("clientid") && ds2.Tables[2].Columns.Contains("secretkey"))
                                        {
                                            var par = ConfigItems.Encrypt ? Encryptionbonus.EncryptString(JsonConvert.SerializeObject(couponPlacebet), ds2.Tables[2].Rows[0]["secretkey"].ToString()) : JsonConvert.SerializeObject(couponPlacebet);
                                            var resp = Postbonus(ConfigItems.CopUrl + "transaction/event", par, "application/json", "POST", ds2.Tables[2].Rows[0]["clientid"].ToString());
                                            var res = ConfigItems.Encrypt ? Encryptionbonus.DecryptString(resp, ds2.Tables[2].Rows[0]["secretkey"].ToString()) : resp;
                                            WriteLogAll("Placebetdt3", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(res));
                                        }
                                        //var resp = Post(ConfigItems.CopUrl + ApiEndpoint.placebet, JsonConvert.SerializeObject(couponPlacebet), "application/json", "POST");
                                        //var data = JsonConvert.DeserializeObject<CouponPlacebetRes>(resp);

                                        //Returnds("0", data.error);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteLogAll("Placebetdt+cop" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                Placebetfinal(placebetteen.uid);
                                return ds2;
                            }
                            else
                            {
                                Placebetfinal(placebetteen.uid);
                                return ds1;
                            }
                        }
                        Placebetfinal(placebetteen.uid);
                        return dscom;
                    }
                    Placebetfinal(placebetteen.uid);
                    return dscasino;
                }
                //Placebetfinal(placebetteen.uid);
                return ds;
            }
            catch (Exception ex)
            {
                Placebetfinal(placebetteen.uid);
                WriteLogAll("Placebetdt" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                throw ex;
            }

        }
        public DataSet Placebetvdt(Placebetteen placebetteen)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet dscom = new DataSet();
                DataSet dscasino = new DataSet();
                placebetteen.gtype.ToLower();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                parameters.Add(new SqlParameter() { ParameterName = "MarketId", Value = placebetteen.mid });
                parameters.Add(new SqlParameter() { ParameterName = "GameType", Value = placebetteen.gtype });
                ds = _sqlClientService.Execute("Casino_PlacebetCheck", ConfigItems.Conn_AccDgroup, parameters);
                WriteLogAll("Placebetvdt+Casino_PlacebetCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds));
                //1 as Id,'ok' as msg,username,u.PartnershipType,general,ExposerLimit,oldexposer,AutoConfirm
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("id") && ds.Tables[0].Rows[0]["id"].ToString() == "1")
                {
                    var parameterscas = new List<SqlParameter>();
                    parameterscas.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                    parameterscas.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                    parameterscas.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                    parameterscas.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                    parameterscas.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                    dscasino = _sqlClientService.Execute("Z_dt_placeExpoCheck", ConfigItems.Conn_Casinogroup, parameterscas);
                    WriteLogAll("Placebetvdt1+Z_dt_placeExpoCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscasino));
                    if (dscasino != null && dscasino.Tables != null && dscasino.Tables.Count > 0 && dscasino.Tables[0].Rows.Count > 0 && dscasino.Tables[0].Columns.Contains("id") && dscasino.Tables[0].Rows[0]["id"].ToString() == "1")
                    {
                        var parameters3 = new List<SqlParameter>();
                        parameters3.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                        parameters3.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                        parameters3.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                        parameters3.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                        parameters3.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                        parameters3.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                        parameters3.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                        dscom = _sqlClientService.Execute("group2_Z_dt_placebetconform", ConfigItems.Conn_Casino, parameters3);
                        WriteLogAll("Placebetvdt2+group2_Z_dt_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscom));
                        if (dscom != null && dscom.Tables != null && dscom.Tables.Count > 0 && dscom.Tables[0].Rows.Count > 0 && dscom.Tables[0].Columns.Contains("id") && dscom.Tables[0].Rows[0]["id"].ToString() == "1")
                        {
                            var parameters1 = new List<SqlParameter>();
                            parameters1.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                            parameters1.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                            parameters1.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                            parameters1.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                            parameters1.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                            parameters1.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                            parameters1.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                            parameters1.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                            parameters1.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newUserrate", Value = dscom.Tables[0].Rows[0]["NewUserrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newAmount", Value = dscom.Tables[0].Rows[0]["Amount"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Gameid", Value = dscom.Tables[0].Rows[0]["Gameid"] });
                            ds1 = _sqlClientService.Execute("Z_dt_placebetconform", ConfigItems.Conn_Casinogroup, parameters1);
                            WriteLogAll("Placebetvdt3+Z_dt_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds1));

                            //select 1 as ID,'Ok' as Msg,	@userexposer as userexposer, @JsonGameSub as masterexposer
                            if (ds1 != null && ds1.Tables != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0 && ds1.Tables[0].Columns.Contains("id") && ds1.Tables[0].Rows[0]["id"].ToString() == "1")
                            {
                                try
                                {
                                    var param = "marketid=" + placebetteen.mid + "&userid=" + placebetteen.uid + "&subsectionid=" + placebetteen.subid + "&gametype=" + placebetteen.gtype + "&subgametype="
                                                                     + placebetteen.subtype + "&username=" + ds.Tables[0].Rows[0]["username"] + "&sectionid=" + placebetteen.sid + "&bettype=" + placebetteen.btype
                                                                    + "&nation=" + ds1.Tables[0].Rows[0]["nation"].ToString() + "&userrate=" + placebetteen.urate + "&Masteramount=" + ds1.Tables[0].Rows[0]["conformamount"]
                                                                    + "&ipaddress=" + placebetteen.ip + "&browserdetail=" + placebetteen.bdetail + "&BookvalType=" + ds1.Tables[0].Rows[0]["exposertype"] +
                                                                    "&Bookvalmatch=" + ds1.Tables[0].Rows[0]["matchbook"] + "&BookvalFancy=" + ds1.Tables[0].Rows[0]["fancybook"] + "&amount=" + placebetteen.amt + "&frate=" + ds.Tables[0].Rows[0]["balrate"] + "&Currency=" + ds.Tables[0].Rows[0]["ccode"];

                                    Task.Factory.StartNew(() => Post(ConfigItems.AdminBookUrl + ApiEndpoint.vdtbook, param, "application/x-www-form-urlencoded", "POST"));
                                }
                                catch (System.Exception ex)
                                {
                                    WriteLogAll("Placebetvdt+bk" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                var parameters2 = new List<SqlParameter>();
                                parameters2.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                                parameters2.Add(new SqlParameter() { ParameterName = "Amount", Value = Convert.ToDouble(ds1.Tables[0].Rows[0]["userexposer"]) });
                                parameters2.Add(new SqlParameter() { ParameterName = "ExposJson", Value = ds1.Tables[0].Rows[0]["masterexposer"].ToString() });
                                parameters2.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                                parameters2.Add(new SqlParameter() { ParameterName = "gameType", Value = placebetteen.gtype });
                                parameters2.Add(new SqlParameter() { ParameterName = "Userrate", Value = ds1.Tables[0].Rows[0]["newuserrate"] });
                                parameters2.Add(new SqlParameter() { ParameterName = "BetAmount", Value = ds1.Tables[0].Rows[0]["conformamount"] });
                                ds2 = _sqlClientService.Execute("Casino_ExposerUpdate", ConfigItems.Conn_AccDgroup, parameters2);
                                WriteLogAll("Placebetvdt4+Casino_ExposerUpdate", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds2));
                                try
                                {
                                    if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 0 && ds2.Tables[1].Rows.Count > 0 && ds2.Tables[1].Columns.Contains("SendCoupon") && ds2.Tables[1].Rows[0]["SendCoupon"].ToString() == "1")
                                    {//Select 0 as SendCoupon,0 as BetAmount,'' as couponcode
                                        CouponPlacebet couponPlacebet = new CouponPlacebet();
                                        couponPlacebet.Amount = Convert.ToDouble(ds1.Tables[0].Rows[0]["conformamount"]);
                                        couponPlacebet.PlayerName = ds.Tables[0].Rows[0]["username"].ToString();
                                        couponPlacebet.PlayerId = placebetteen.pid;
                                        couponPlacebet.Product = ds2.Tables[1].Rows[0]["couponcode"].ToString();
                                        couponPlacebet.Balance = Convert.ToDouble(ds.Tables[0].Rows[0]["general"]);
                                        if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 1 && ds2.Tables[2].Rows.Count > 0 && ds2.Tables[2].Columns.Contains("clientid") && ds2.Tables[2].Columns.Contains("secretkey"))
                                        {
                                            var par = ConfigItems.Encrypt ? Encryptionbonus.EncryptString(JsonConvert.SerializeObject(couponPlacebet), ds2.Tables[2].Rows[0]["secretkey"].ToString()) : JsonConvert.SerializeObject(couponPlacebet);
                                            var resp = Postbonus(ConfigItems.CopUrl + "transaction/event", par, "application/json", "POST", ds2.Tables[2].Rows[0]["clientid"].ToString());
                                            var res = ConfigItems.Encrypt ? Encryptionbonus.DecryptString(resp, ds2.Tables[2].Rows[0]["secretkey"].ToString()) : resp;
                                            WriteLogAll("Placebetvdt3", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(res));
                                        }
                                        //var resp = Post(ConfigItems.CopUrl + ApiEndpoint.placebet, JsonConvert.SerializeObject(couponPlacebet), "application/json", "POST");
                                        //var data = JsonConvert.DeserializeObject<CouponPlacebetRes>(resp);

                                        //Returnds("0", data.error);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteLogAll("Placebetvdt+cop" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                Placebetfinal(placebetteen.uid);
                                return ds2;
                            }
                            else
                            {
                                Placebetfinal(placebetteen.uid);
                                return ds1;
                            }
                        }
                        Placebetfinal(placebetteen.uid);
                        return dscom;
                    }
                    Placebetfinal(placebetteen.uid);
                    return dscasino;
                }
                //Placebetfinal(placebetteen.uid);
                return ds;
            }
            catch (Exception ex)
            {
                Placebetfinal(placebetteen.uid);
                WriteLogAll("Placebetvdt" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                throw ex;
            }

        }
        public DataSet Placebetbc(Placebetteen placebetteen)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet dscom = new DataSet();
                DataSet dscasino = new DataSet();
                placebetteen.gtype.ToLower();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                parameters.Add(new SqlParameter() { ParameterName = "MarketId", Value = placebetteen.mid });
                parameters.Add(new SqlParameter() { ParameterName = "GameType", Value = placebetteen.gtype });
                ds = _sqlClientService.Execute("Casino_PlacebetCheck", ConfigItems.Conn_AccDgroup, parameters);
                WriteLogAll("Placebetbc+Casino_PlacebetCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds));

                //1 as Id,'ok' as msg,username,u.PartnershipType,general,ExposerLimit,oldexposer,AutoConfirm
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("id") && ds.Tables[0].Rows[0]["id"].ToString() == "1")
                {
                    var parameterscas = new List<SqlParameter>();
                    parameterscas.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                    parameterscas.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                    parameterscas.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                    parameterscas.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                    parameterscas.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                    dscasino = _sqlClientService.Execute("bc_placeExpoCheck", ConfigItems.Conn_Casinogroup, parameterscas);
                    WriteLogAll("Placebetbc1+bc_placeExpoCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscasino));
                    if (dscasino != null && dscasino.Tables != null && dscasino.Tables.Count > 0 && dscasino.Tables[0].Rows.Count > 0 && dscasino.Tables[0].Columns.Contains("id") && dscasino.Tables[0].Rows[0]["id"].ToString() == "1")
                    {
                        var parameters3 = new List<SqlParameter>();
                        parameters3.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                        parameters3.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                        parameters3.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                        parameters3.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                        parameters3.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                        parameters3.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                        parameters3.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                        dscom = _sqlClientService.Execute("group2_bc_placebetconform", ConfigItems.Conn_Casino, parameters3);
                        WriteLogAll("Placebetbc2+group2_bc_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscom));
                        if (dscom != null && dscom.Tables != null && dscom.Tables.Count > 0 && dscom.Tables[0].Rows.Count > 0 && dscom.Tables[0].Columns.Contains("id") && dscom.Tables[0].Rows[0]["id"].ToString() == "1")
                        {
                            var parameters1 = new List<SqlParameter>();
                            parameters1.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                            parameters1.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                            parameters1.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                            parameters1.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                            parameters1.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                            parameters1.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                            parameters1.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                            parameters1.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                            parameters1.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newUserrate", Value = dscom.Tables[0].Rows[0]["NewUserrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newAmount", Value = dscom.Tables[0].Rows[0]["Amount"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Gameid", Value = dscom.Tables[0].Rows[0]["Gameid"] });
                            ds1 = _sqlClientService.Execute("bc_placebetconform", ConfigItems.Conn_Casinogroup, parameters1);
                            WriteLogAll("Placebetbc3+bc_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds1));
                            //select 1 as ID,'Ok' as Msg,	@userexposer as userexposer, @JsonGameSub as masterexposer
                            if (ds1 != null && ds1.Tables != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0 && ds1.Tables[0].Columns.Contains("id") && ds1.Tables[0].Rows[0]["id"].ToString() == "1")
                            {
                                try
                                {
                                    var param = "marketid=" + placebetteen.mid + "&userid=" + placebetteen.uid + "&subsectionid=" + placebetteen.subid + "&gametype=" + placebetteen.gtype + "&subgametype="
                                                                     + placebetteen.subtype + "&username=" + ds.Tables[0].Rows[0]["username"] + "&sectionid=" + placebetteen.sid + "&bettype=" + placebetteen.btype
                                                                    + "&nation=" + ds1.Tables[0].Rows[0]["nation"].ToString() + "&userrate=" + placebetteen.urate + "&Masteramount=" + ds1.Tables[0].Rows[0]["conformamount"]
                                                                    + "&ipaddress=" + placebetteen.ip + "&browserdetail=" + placebetteen.bdetail + "&BookvalType=" + ds1.Tables[0].Rows[0]["exposertype"] +
                                                                    "&Bookvalmatch=" + ds1.Tables[0].Rows[0]["matchbook"] + "&BookvalFancy=" + ds1.Tables[0].Rows[0]["fancybook"] + "&amount=" + placebetteen.amt + "&frate=" + ds.Tables[0].Rows[0]["balrate"] + "&Currency=" + ds.Tables[0].Rows[0]["ccode"];

                                    Task.Factory.StartNew(() => Post(ConfigItems.AdminBookUrl + ApiEndpoint.bcbook, param, "application/x-www-form-urlencoded", "POST"));
                                }
                                catch (System.Exception ex)
                                {
                                    WriteLogAll("Placebetbc+bk" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                var parameters2 = new List<SqlParameter>();
                                parameters2.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                                parameters2.Add(new SqlParameter() { ParameterName = "Amount", Value = Convert.ToDouble(ds1.Tables[0].Rows[0]["userexposer"]) });
                                parameters2.Add(new SqlParameter() { ParameterName = "ExposJson", Value = ds1.Tables[0].Rows[0]["masterexposer"].ToString() });
                                parameters2.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                                parameters2.Add(new SqlParameter() { ParameterName = "gameType", Value = placebetteen.gtype });
                                parameters2.Add(new SqlParameter() { ParameterName = "Userrate", Value = ds1.Tables[0].Rows[0]["newuserrate"] });
                                parameters2.Add(new SqlParameter() { ParameterName = "BetAmount", Value = ds1.Tables[0].Rows[0]["conformamount"] });
                                ds2 = _sqlClientService.Execute("Casino_ExposerUpdate", ConfigItems.Conn_AccDgroup, parameters2);
                                WriteLogAll("Placebetbc4+Casino_ExposerUpdate", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds2));
                                try
                                {
                                    if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 0 && ds2.Tables[1].Rows.Count > 0 && ds2.Tables[1].Columns.Contains("SendCoupon") && ds2.Tables[1].Rows[0]["SendCoupon"].ToString() == "1")
                                    {//Select 0 as SendCoupon,0 as BetAmount,'' as couponcode
                                        CouponPlacebet couponPlacebet = new CouponPlacebet();
                                        couponPlacebet.Amount = Convert.ToDouble(ds1.Tables[0].Rows[0]["conformamount"]);
                                        couponPlacebet.PlayerName = ds.Tables[0].Rows[0]["username"].ToString();
                                        couponPlacebet.PlayerId = placebetteen.pid;
                                        couponPlacebet.Product = ds2.Tables[1].Rows[0]["couponcode"].ToString();
                                        couponPlacebet.Balance = Convert.ToDouble(ds.Tables[0].Rows[0]["general"]);
                                        if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 1 && ds2.Tables[2].Rows.Count > 0 && ds2.Tables[2].Columns.Contains("clientid") && ds2.Tables[2].Columns.Contains("secretkey"))
                                        {
                                            var par = ConfigItems.Encrypt ? Encryptionbonus.EncryptString(JsonConvert.SerializeObject(couponPlacebet), ds2.Tables[2].Rows[0]["secretkey"].ToString()) : JsonConvert.SerializeObject(couponPlacebet);
                                            var resp = Postbonus(ConfigItems.CopUrl + "transaction/event", par, "application/json", "POST", ds2.Tables[2].Rows[0]["clientid"].ToString());
                                            var res = ConfigItems.Encrypt ? Encryptionbonus.DecryptString(resp, ds2.Tables[2].Rows[0]["secretkey"].ToString()) : resp;
                                            WriteLogAll("Placebetbc3", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(res));
                                        }
                                        //var resp = Post(ConfigItems.CopUrl + ApiEndpoint.placebet, JsonConvert.SerializeObject(couponPlacebet), "application/json", "POST");
                                        //var data = JsonConvert.DeserializeObject<CouponPlacebetRes>(resp);

                                        //Returnds("0", data.error);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteLogAll("Placebetbc+cop" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                Placebetfinal(placebetteen.uid);
                                return ds2;
                            }
                            else
                            {
                                Placebetfinal(placebetteen.uid);
                                return ds1;
                            }
                        }
                        Placebetfinal(placebetteen.uid);
                        return dscom;
                    }
                    Placebetfinal(placebetteen.uid);
                    return dscasino;
                }
                //Placebetfinal(placebetteen.uid);
                return ds;
            }
            catch (Exception ex)
            {
                Placebetfinal(placebetteen.uid);
                WriteLogAll("Placebetbc" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                throw ex;
            }

        }
        public DataSet Placebetworli(Placebetworli placebetteen)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet dscom = new DataSet();
                DataSet dscasino = new DataSet();
                placebetteen.gtype.ToLower();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                parameters.Add(new SqlParameter() { ParameterName = "MarketId", Value = placebetteen.mid });
                parameters.Add(new SqlParameter() { ParameterName = "GameType", Value = placebetteen.gtype });
                ds = _sqlClientService.Execute("Casino_PlacebetCheck", ConfigItems.Conn_AccDgroup, parameters);
                WriteLogAll("Placebetworli+Casino_PlacebetCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds));

                //1 as Id,'ok' as msg,username,u.PartnershipType,general,ExposerLimit,oldexposer,AutoConfirm
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("id") && ds.Tables[0].Rows[0]["id"].ToString() == "1")
                {
                    var parameterscas = new List<SqlParameter>();
                    parameterscas.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                    parameterscas.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                    parameterscas.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                    parameterscas.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                    parameterscas.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                    dscasino = _sqlClientService.Execute("worli_placeExpoCheck", ConfigItems.Conn_Casinogroup, parameterscas);
                    WriteLogAll("Placebetworli1+worli_placeExpoCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscasino));
                    if (dscasino != null && dscasino.Tables != null && dscasino.Tables.Count > 0 && dscasino.Tables[0].Rows.Count > 0 && dscasino.Tables[0].Columns.Contains("id") && dscasino.Tables[0].Rows[0]["id"].ToString() == "1")
                    {
                        var parameters3 = new List<SqlParameter>();
                        parameters3.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                        parameters3.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                        parameters3.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                        parameters3.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                        parameters3.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                        parameters3.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                        parameters3.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "exposeramount", Value = dscasino.Tables[0].Rows[0]["exposeramount"] });
                        dscom = _sqlClientService.Execute("group2_worli_placebetconform", ConfigItems.Conn_Casino, parameters3);
                        WriteLogAll("Placebetworli2+group2_worli_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscom));
                        if (dscom != null && dscom.Tables != null && dscom.Tables.Count > 0 && dscom.Tables[0].Rows.Count > 0 && dscom.Tables[0].Columns.Contains("id") && dscom.Tables[0].Rows[0]["id"].ToString() == "1")
                        {
                            var parameters1 = new List<SqlParameter>();
                            parameters1.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                            parameters1.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                            parameters1.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                            parameters1.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                            parameters1.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                            parameters1.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                            parameters1.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                            parameters1.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                            parameters1.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newUserrate", Value = dscom.Tables[0].Rows[0]["NewUserrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newAmount", Value = dscom.Tables[0].Rows[0]["Amount"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Gameid", Value = dscom.Tables[0].Rows[0]["Gameid"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "exposeramount", Value = dscasino.Tables[0].Rows[0]["exposeramount"] });
                            ds1 = _sqlClientService.Execute("worli_placebetconform", ConfigItems.Conn_Casinogroup, parameters1);
                            WriteLogAll("Placebetworli3+worli_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds1));

                            //select 1 as ID,'Ok' as Msg,	@userexposer as userexposer, @JsonGameSub as masterexposer
                            if (ds1 != null && ds1.Tables != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0 && ds1.Tables[0].Columns.Contains("id") && ds1.Tables[0].Rows[0]["id"].ToString() == "1")
                            {
                                try
                                {
                                    var param = "marketid=" + placebetteen.mid + "&userid=" + placebetteen.uid + "&subsectionid=" + placebetteen.subid + "&gametype=" + placebetteen.gtype + "&subgametype="
                                                                     + placebetteen.subtype + "&username=" + ds.Tables[0].Rows[0]["username"] + "&sectionid=" + placebetteen.sid + "&bettype=" + placebetteen.btype
                                                                    + "&nation=" + ds1.Tables[0].Rows[0]["nation"].ToString() + "&userrate=" + ds1.Tables[0].Rows[0]["newuserrate"] + "&Masteramount=" + ds1.Tables[0].Rows[0]["MasterAmount"]
                                                                    + "&ipaddress=" + placebetteen.ip + "&browserdetail=" + placebetteen.bdetail + "&BookvalType=" + ds1.Tables[0].Rows[0]["exposertype"] +
                                                                    "&Bookvalmatch=" + ds1.Tables[0].Rows[0]["matchbook"] + "&BookvalFancy=" + ds1.Tables[0].Rows[0]["fancybook"] + "&amount=" + ds1.Tables[0].Rows[0]["conformamount"] +
                                                                    "&Userratefront=" + placebetteen.urate + "&frate=" + ds.Tables[0].Rows[0]["balrate"] + "&Currency=" + ds.Tables[0].Rows[0]["ccode"];

                                    Task.Factory.StartNew(() => Post(ConfigItems.AdminBookUrl + ApiEndpoint.worlibook, param, "application/x-www-form-urlencoded", "POST"));
                                }
                                catch (System.Exception ex)
                                {
                                    WriteLogAll("Placebetworli+bk" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                var parameters2 = new List<SqlParameter>();
                                parameters2.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                                parameters2.Add(new SqlParameter() { ParameterName = "Amount", Value = Convert.ToDouble(ds1.Tables[0].Rows[0]["userexposer"]) });
                                parameters2.Add(new SqlParameter() { ParameterName = "ExposJson", Value = ds1.Tables[0].Rows[0]["masterexposer"].ToString() });
                                parameters2.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                                parameters2.Add(new SqlParameter() { ParameterName = "gameType", Value = placebetteen.gtype });
                                parameters2.Add(new SqlParameter() { ParameterName = "Userrate", Value = ds1.Tables[0].Rows[0]["newuserrate"] });
                                parameters2.Add(new SqlParameter() { ParameterName = "BetAmount", Value = ds1.Tables[0].Rows[0]["conformamount"] });
                                ds2 = _sqlClientService.Execute("Casino_ExposerUpdate", ConfigItems.Conn_AccDgroup, parameters2);
                                WriteLogAll("Placebetworli4+Casino_ExposerUpdate", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds2));

                                try
                                {
                                    if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 0 && ds2.Tables[1].Rows.Count > 0 && ds2.Tables[1].Columns.Contains("SendCoupon") && ds2.Tables[1].Rows[0]["SendCoupon"].ToString() == "1")
                                    {//Select 0 as SendCoupon,0 as BetAmount,'' as couponcode
                                        CouponPlacebet couponPlacebet = new CouponPlacebet();
                                        couponPlacebet.Amount = Convert.ToDouble(ds1.Tables[0].Rows[0]["conformamount"]);
                                        couponPlacebet.PlayerName = ds.Tables[0].Rows[0]["username"].ToString();
                                        couponPlacebet.PlayerId = placebetteen.pid;
                                        couponPlacebet.Product = ds2.Tables[1].Rows[0]["couponcode"].ToString();
                                        couponPlacebet.Balance = Convert.ToDouble(ds.Tables[0].Rows[0]["general"]);
                                        if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 1 && ds2.Tables[2].Rows.Count > 0 && ds2.Tables[2].Columns.Contains("clientid") && ds2.Tables[2].Columns.Contains("secretkey"))
                                        {
                                            var par = ConfigItems.Encrypt ? Encryptionbonus.EncryptString(JsonConvert.SerializeObject(couponPlacebet), ds2.Tables[2].Rows[0]["secretkey"].ToString()) : JsonConvert.SerializeObject(couponPlacebet);
                                            var resp = Postbonus(ConfigItems.CopUrl + "transaction/event", par, "application/json", "POST", ds2.Tables[2].Rows[0]["clientid"].ToString());
                                            var res = ConfigItems.Encrypt ? Encryptionbonus.DecryptString(resp, ds2.Tables[2].Rows[0]["secretkey"].ToString()) : resp;
                                            WriteLogAll("Placebetworli4", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(res));
                                        }
                                        //var resp = Post(ConfigItems.CopUrl + ApiEndpoint.placebet, JsonConvert.SerializeObject(couponPlacebet), "application/json", "POST");
                                        //var data = JsonConvert.DeserializeObject<CouponPlacebetRes>(resp);

                                        //Returnds("0", data.error);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteLogAll("Placebetworli+cop" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                Placebetfinal(placebetteen.uid);
                                return ds2;
                            }
                            else
                            {
                                Placebetfinal(placebetteen.uid);
                                return ds1;
                            }
                        }
                        Placebetfinal(placebetteen.uid);
                        return dscom;
                    }
                    Placebetfinal(placebetteen.uid);
                    return dscasino;
                }
                //Placebetfinal(placebetteen.uid);
                return ds;
            }
            catch (Exception ex)
            {
                Placebetfinal(placebetteen.uid);
                WriteLogAll("Placebetworli" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                throw ex;
            }

        }
        public DataSet Placebetother(Placebetteen placebetteen)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet dscom = new DataSet();
                DataSet dscasino = new DataSet();
                placebetteen.gtype.ToLower();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                parameters.Add(new SqlParameter() { ParameterName = "MarketId", Value = placebetteen.mid });
                parameters.Add(new SqlParameter() { ParameterName = "GameType", Value = placebetteen.gtype });
                ds = _sqlClientService.Execute("Casino_PlacebetCheck", ConfigItems.Conn_AccDgroup, parameters);
                WriteLogAll("Placebetother+Casino_PlacebetCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds));

                //1 as Id,'ok' as msg,username,u.PartnershipType,general,ExposerLimit,oldexposer,AutoConfirm
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("id") && ds.Tables[0].Rows[0]["id"].ToString() == "1")
                {
                    var parameterscas = new List<SqlParameter>();
                    parameterscas.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                    parameterscas.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                    parameterscas.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                    parameterscas.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                    parameterscas.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                    dscasino = _sqlClientService.Execute("other_placeExpoCheck", ConfigItems.Conn_Casinogroup, parameterscas);
                    WriteLogAll("Placebetother1+other_placeExpoCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscasino));
                    if (dscasino != null && dscasino.Tables != null && dscasino.Tables.Count > 0 && dscasino.Tables[0].Rows.Count > 0 && dscasino.Tables[0].Columns.Contains("id") && dscasino.Tables[0].Rows[0]["id"].ToString() == "1")
                    {
                        var parameters3 = new List<SqlParameter>();
                        parameters3.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                        parameters3.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                        parameters3.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                        parameters3.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                        parameters3.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                        parameters3.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                        parameters3.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                        dscom = _sqlClientService.Execute("group2_other_placebetconform", ConfigItems.Conn_Casino, parameters3);
                        WriteLogAll("Placebetother2+group2_other_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscom));
                        if (dscom != null && dscom.Tables != null && dscom.Tables.Count > 0 && dscom.Tables[0].Rows.Count > 0 && dscom.Tables[0].Columns.Contains("id") && dscom.Tables[0].Rows[0]["id"].ToString() == "1")
                        {
                            var parameters1 = new List<SqlParameter>();
                            parameters1.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                            parameters1.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                            parameters1.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                            parameters1.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                            parameters1.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                            parameters1.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                            parameters1.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                            parameters1.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                            parameters1.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newUserrate", Value = dscom.Tables[0].Rows[0]["NewUserrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newAmount", Value = dscom.Tables[0].Rows[0]["Amount"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Gameid", Value = dscom.Tables[0].Rows[0]["Gameid"] });
                            ds1 = _sqlClientService.Execute("other_placebetconform", ConfigItems.Conn_Casinogroup, parameters1);
                            WriteLogAll("Placebetother3+other_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds1));

                            //select 1 as ID,'Ok' as Msg,	@userexposer as userexposer, @JsonGameSub as masterexposer
                            if (ds1 != null && ds1.Tables != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0 && ds1.Tables[0].Columns.Contains("id") && ds1.Tables[0].Rows[0]["id"].ToString() == "1")
                            {
                                try
                                {
                                    var param = "marketid=" + placebetteen.mid + "&userid=" + placebetteen.uid + "&subsectionid=" + placebetteen.subid + "&gametype=" + placebetteen.gtype + "&subgametype="
                                                                     + placebetteen.subtype + "&username=" + ds.Tables[0].Rows[0]["username"] + "&sectionid=" + placebetteen.sid + "&bettype=" + placebetteen.btype
                                                                    + "&nation=" + ds1.Tables[0].Rows[0]["nation"].ToString() + "&userrate=" + placebetteen.urate + "&Masteramount=" + ds1.Tables[0].Rows[0]["conformamount"]
                                                                    + "&ipaddress=" + placebetteen.ip + "&browserdetail=" + placebetteen.bdetail + "&BookvalType=" + ds1.Tables[0].Rows[0]["exposertype"] +
                                                                    "&Bookvalmatch=" + ds1.Tables[0].Rows[0]["matchbook"] + "&BookvalFancy=" + ds1.Tables[0].Rows[0]["fancybook"] + "&amount=" + placebetteen.amt + "&frate=" + ds.Tables[0].Rows[0]["balrate"] + "&Currency=" + ds.Tables[0].Rows[0]["ccode"];

                                    Task.Factory.StartNew(() => Post(ConfigItems.AdminBookUrl + ApiEndpoint.otherbook, param, "application/x-www-form-urlencoded", "POST"));
                                }
                                catch (System.Exception ex)
                                {
                                    WriteLogAll("Placebetother+bk" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                var parameters2 = new List<SqlParameter>();
                                parameters2.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                                parameters2.Add(new SqlParameter() { ParameterName = "Amount", Value = Convert.ToDouble(ds1.Tables[0].Rows[0]["userexposer"]) });
                                parameters2.Add(new SqlParameter() { ParameterName = "ExposJson", Value = ds1.Tables[0].Rows[0]["masterexposer"].ToString() });
                                parameters2.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                                parameters2.Add(new SqlParameter() { ParameterName = "gameType", Value = placebetteen.gtype });
                                parameters2.Add(new SqlParameter() { ParameterName = "Userrate", Value = ds1.Tables[0].Rows[0]["newuserrate"] });
                                parameters2.Add(new SqlParameter() { ParameterName = "BetAmount", Value = ds1.Tables[0].Rows[0]["conformamount"] });
                                ds2 = _sqlClientService.Execute("Casino_ExposerUpdate", ConfigItems.Conn_AccDgroup, parameters2);
                                WriteLogAll("Placebetother4+Casino_ExposerUpdate", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds2));
                                try
                                {
                                    if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 0 && ds2.Tables[1].Rows.Count > 0 && ds2.Tables[1].Columns.Contains("SendCoupon") && ds2.Tables[1].Rows[0]["SendCoupon"].ToString() == "1")
                                    {//Select 0 as SendCoupon,0 as BetAmount,'' as couponcode
                                        CouponPlacebet couponPlacebet = new CouponPlacebet();
                                        couponPlacebet.Amount = Convert.ToDouble(ds1.Tables[0].Rows[0]["conformamount"]);
                                        couponPlacebet.PlayerName = ds.Tables[0].Rows[0]["username"].ToString();
                                        couponPlacebet.PlayerId = placebetteen.pid;
                                        couponPlacebet.Product = ds2.Tables[1].Rows[0]["couponcode"].ToString();
                                        couponPlacebet.Balance = Convert.ToDouble(ds.Tables[0].Rows[0]["general"]);
                                        if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 1 && ds2.Tables[2].Rows.Count > 0 && ds2.Tables[2].Columns.Contains("clientid") && ds2.Tables[2].Columns.Contains("secretkey"))
                                        {
                                            var par = ConfigItems.Encrypt ? Encryptionbonus.EncryptString(JsonConvert.SerializeObject(couponPlacebet), ds2.Tables[2].Rows[0]["secretkey"].ToString()) : JsonConvert.SerializeObject(couponPlacebet);
                                            var resp = Postbonus(ConfigItems.CopUrl + "transaction/event", par, "application/json", "POST", ds2.Tables[2].Rows[0]["clientid"].ToString());
                                            var res = ConfigItems.Encrypt ? Encryptionbonus.DecryptString(resp, ds2.Tables[2].Rows[0]["secretkey"].ToString()) : resp;
                                            WriteLogAll("Placebetfancy4", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(res));
                                        }
                                        //var resp = Post(ConfigItems.CopUrl + ApiEndpoint.placebet, JsonConvert.SerializeObject(couponPlacebet), "application/json", "POST");
                                        //var data = JsonConvert.DeserializeObject<CouponPlacebetRes>(resp);

                                        //Returnds("0", data.error);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteLogAll("Placebetteen+cop" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                Placebetfinal(placebetteen.uid);
                                return ds2;
                            }
                            else
                            {
                                Placebetfinal(placebetteen.uid);
                                return ds1;
                            }
                        }
                        Placebetfinal(placebetteen.uid);
                        return dscom;
                    }
                    Placebetfinal(placebetteen.uid);
                    return dscasino;
                }
                //Placebetfinal(placebetteen.uid);
                return ds;
            }
            catch (Exception ex)
            {
                Placebetfinal(placebetteen.uid);
                WriteLogAll("Placebetother" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                throw ex;
            }

        }
        public DataSet Placebet3cardj(Placebet3cardj placebet3Cardj)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet dscom = new DataSet();
                DataSet dscasino = new DataSet();
                placebet3Cardj.gtype.ToLower();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { ParameterName = "Userid", Value = placebet3Cardj.uid });
                parameters.Add(new SqlParameter() { ParameterName = "MarketId", Value = placebet3Cardj.mid });
                parameters.Add(new SqlParameter() { ParameterName = "GameType", Value = placebet3Cardj.gtype });
                ds = _sqlClientService.Execute("Casino_PlacebetCheck", ConfigItems.Conn_AccDgroup, parameters);
                WriteLogAll("Placebet3cardj+Casino_PlacebetCheck", "Req:" + JsonConvert.SerializeObject(placebet3Cardj) + "Res" + JsonConvert.SerializeObject(ds));

                //1 as Id,'ok' as msg,username,u.PartnershipType,general,ExposerLimit,oldexposer,AutoConfirm
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("id") && ds.Tables[0].Rows[0]["id"].ToString() == "1")
                {
                    var parameterscas = new List<SqlParameter>();
                    parameterscas.Add(new SqlParameter() { ParameterName = "marketid", Value = placebet3Cardj.mid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebet3Cardj.sid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebet3Cardj.subid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "bettype", Value = placebet3Cardj.btype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userrate", Value = placebet3Cardj.urate });
                    parameterscas.Add(new SqlParameter() { ParameterName = "amount", Value = placebet3Cardj.amt });
                    parameterscas.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "gametype", Value = placebet3Cardj.gtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebet3Cardj.subtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userid", Value = placebet3Cardj.uid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebet3Cardj.ip });
                    parameterscas.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebet3Cardj.bdetail });
                    parameterscas.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "usercard", Value = placebet3Cardj.ucard });
                    dscasino = _sqlClientService.Execute("cardj_placeExpoCheck", ConfigItems.Conn_Casinogroup, parameterscas);
                    WriteLogAll("Placebet3cardj1+cardj_placeExpoCheck", "Req:" + JsonConvert.SerializeObject(placebet3Cardj) + "Res" + JsonConvert.SerializeObject(dscasino));
                    if (dscasino != null && dscasino.Tables != null && dscasino.Tables.Count > 0 && dscasino.Tables[0].Rows.Count > 0 && dscasino.Tables[0].Columns.Contains("id") && dscasino.Tables[0].Rows[0]["id"].ToString() == "1")
                    {
                        var parameters3 = new List<SqlParameter>();
                        parameters3.Add(new SqlParameter() { ParameterName = "marketid", Value = placebet3Cardj.mid });
                        parameters3.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebet3Cardj.sid });
                        parameters3.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebet3Cardj.subid });
                        parameters3.Add(new SqlParameter() { ParameterName = "bettype", Value = placebet3Cardj.btype });
                        parameters3.Add(new SqlParameter() { ParameterName = "userrate", Value = placebet3Cardj.urate });
                        parameters3.Add(new SqlParameter() { ParameterName = "amount", Value = placebet3Cardj.amt });
                        parameters3.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "gametype", Value = placebet3Cardj.gtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebet3Cardj.subtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "usercard", Value = placebet3Cardj.ucard });
                        dscom = _sqlClientService.Execute("group2_cardj_placebetconform", ConfigItems.Conn_Casino, parameters3);
                        WriteLogAll("Placebet3cardj2+group2_cardjplacebetconform", "Req:" + JsonConvert.SerializeObject(placebet3Cardj) + "Res" + JsonConvert.SerializeObject(dscom));
                        if (dscom != null && dscom.Tables != null && dscom.Tables.Count > 0 && dscom.Tables[0].Rows.Count > 0 && dscom.Tables[0].Columns.Contains("id") && dscom.Tables[0].Rows[0]["id"].ToString() == "1")
                        {
                            var parameters1 = new List<SqlParameter>();
                            parameters1.Add(new SqlParameter() { ParameterName = "marketid", Value = placebet3Cardj.mid });
                            parameters1.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebet3Cardj.sid });
                            parameters1.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebet3Cardj.subid });
                            parameters1.Add(new SqlParameter() { ParameterName = "bettype", Value = placebet3Cardj.btype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userrate", Value = placebet3Cardj.urate });
                            parameters1.Add(new SqlParameter() { ParameterName = "amount", Value = placebet3Cardj.amt });
                            parameters1.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "gametype", Value = placebet3Cardj.gtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebet3Cardj.subtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userid", Value = placebet3Cardj.uid });
                            parameters1.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebet3Cardj.ip });
                            parameters1.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebet3Cardj.bdetail });
                            parameters1.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "usercard", Value = placebet3Cardj.ucard });
                            parameters1.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newUserrate", Value = dscom.Tables[0].Rows[0]["NewUserrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newAmount", Value = dscom.Tables[0].Rows[0]["Amount"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Gameid", Value = dscom.Tables[0].Rows[0]["Gameid"] });
                            ds1 = _sqlClientService.Execute("cardj_placebetconform", ConfigItems.Conn_Casinogroup, parameters1);
                            WriteLogAll("Placebet3cardj3+cardj_placebetconform", "Req:" + JsonConvert.SerializeObject(placebet3Cardj) + "Res" + JsonConvert.SerializeObject(ds1));

                            //select 1 as ID,'Ok' as Msg,	@userexposer as userexposer, @JsonGameSub as masterexposer
                            if (ds1 != null && ds1.Tables != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0 && ds1.Tables[0].Columns.Contains("id") && ds1.Tables[0].Rows[0]["id"].ToString() == "1")
                            {
                                try
                                {
                                    var param = "marketid=" + placebet3Cardj.mid + "&userid=" + placebet3Cardj.uid + "&subsectionid=" + placebet3Cardj.subid + "&gametype=" + placebet3Cardj.gtype + "&subgametype="
                                                                     + placebet3Cardj.subtype + "&username=" + ds.Tables[0].Rows[0]["username"] + "&sectionid=" + placebet3Cardj.sid + "&bettype=" + placebet3Cardj.btype
                                                                    + "&nation=" + ds1.Tables[0].Rows[0]["nation"].ToString() + "&userrate=" + placebet3Cardj.urate + "&Masteramount=" + ds1.Tables[0].Rows[0]["conformamount"]
                                                                    + "&ipaddress=" + placebet3Cardj.ip + "&browserdetail=" + placebet3Cardj.bdetail + "&BookvalType=" + ds1.Tables[0].Rows[0]["exposertype"] +
                                                                    "&Bookvalmatch=" + ds1.Tables[0].Rows[0]["matchbook"] + "&BookvalFancy=" + ds1.Tables[0].Rows[0]["fancybook"] + "&amount=" + placebet3Cardj.amt + "&frate=" + ds.Tables[0].Rows[0]["balrate"] + "&Currency=" + ds.Tables[0].Rows[0]["ccode"];

                                    Task.Factory.StartNew(() => Post(ConfigItems.AdminBookUrl + ApiEndpoint.p3cardjbook, param, "application/x-www-form-urlencoded", "POST"));
                                }
                                catch (System.Exception ex)
                                {
                                    WriteLogAll("Placebet3cardj+bk" + ex.ToString(), JsonConvert.SerializeObject(placebet3Cardj));
                                }
                                var parameters2 = new List<SqlParameter>();
                                parameters2.Add(new SqlParameter() { ParameterName = "Userid", Value = placebet3Cardj.uid });
                                parameters2.Add(new SqlParameter() { ParameterName = "Amount", Value = Convert.ToDouble(ds1.Tables[0].Rows[0]["userexposer"]) });
                                parameters2.Add(new SqlParameter() { ParameterName = "ExposJson", Value = ds1.Tables[0].Rows[0]["masterexposer"].ToString() });
                                parameters2.Add(new SqlParameter() { ParameterName = "marketid", Value = placebet3Cardj.mid });
                                parameters2.Add(new SqlParameter() { ParameterName = "gameType", Value = placebet3Cardj.gtype });
                                parameters2.Add(new SqlParameter() { ParameterName = "Userrate", Value = ds1.Tables[0].Rows[0]["newuserrate"] });
                                parameters2.Add(new SqlParameter() { ParameterName = "BetAmount", Value = ds1.Tables[0].Rows[0]["conformamount"] });
                                ds2 = _sqlClientService.Execute("Casino_ExposerUpdate", ConfigItems.Conn_AccDgroup, parameters2);
                                WriteLogAll("Placebet3cardj4+Casino_ExposerUpdate", "Req:" + JsonConvert.SerializeObject(placebet3Cardj) + "Res" + JsonConvert.SerializeObject(ds2));
                                try
                                {
                                    if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 0 && ds2.Tables[1].Rows.Count > 0 && ds2.Tables[1].Columns.Contains("SendCoupon") && ds2.Tables[1].Rows[0]["SendCoupon"].ToString() == "1")
                                    {//Select 0 as SendCoupon,0 as BetAmount,'' as couponcode
                                        CouponPlacebet couponPlacebet = new CouponPlacebet();
                                        couponPlacebet.Amount = Convert.ToDouble(ds1.Tables[0].Rows[0]["conformamount"]);
                                        couponPlacebet.PlayerName = ds.Tables[0].Rows[0]["username"].ToString();
                                        couponPlacebet.PlayerId = placebet3Cardj.pid;
                                        couponPlacebet.Product = ds2.Tables[1].Rows[0]["couponcode"].ToString();
                                        couponPlacebet.Balance = Convert.ToDouble(ds.Tables[0].Rows[0]["general"]);
                                        if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 1 && ds2.Tables[2].Rows.Count > 0 && ds2.Tables[2].Columns.Contains("clientid") && ds2.Tables[2].Columns.Contains("secretkey"))
                                        {
                                            var par = ConfigItems.Encrypt ? Encryptionbonus.EncryptString(JsonConvert.SerializeObject(couponPlacebet), ds2.Tables[2].Rows[0]["secretkey"].ToString()) : JsonConvert.SerializeObject(couponPlacebet);
                                            var resp = Postbonus(ConfigItems.CopUrl + "transaction/event", par, "application/json", "POST", ds2.Tables[2].Rows[0]["clientid"].ToString());
                                            var res = ConfigItems.Encrypt ? Encryptionbonus.DecryptString(resp, ds2.Tables[2].Rows[0]["secretkey"].ToString()) : resp;
                                            WriteLogAll("Placebet3cardj3", "Req:" + JsonConvert.SerializeObject(placebet3Cardj) + "Res" + JsonConvert.SerializeObject(res));
                                        }
                                        //var resp = Post(ConfigItems.CopUrl + ApiEndpoint.placebet, JsonConvert.SerializeObject(couponPlacebet), "application/json", "POST");
                                        //var data = JsonConvert.DeserializeObject<CouponPlacebetRes>(resp);

                                        //Returnds("0", data.error);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteLogAll("Placebet3cardj+cop" + ex.ToString(), JsonConvert.SerializeObject(placebet3Cardj));
                                }
                                Placebetfinal(placebet3Cardj.uid);
                                return ds2;
                            }
                            else
                            {
                                Placebetfinal(placebet3Cardj.uid);
                                return ds1;
                            }
                        }
                        Placebetfinal(placebet3Cardj.uid);
                        return dscom;
                    }
                    Placebetfinal(placebet3Cardj.uid);
                    return dscasino;
                }
                //Placebetfinal(placebet3Cardj.uid);
                return ds;
            }
            catch (Exception ex)
            {
                Placebetfinal(placebet3Cardj.uid);
                WriteLogAll("Placebet3cardj" + ex.ToString(), JsonConvert.SerializeObject(placebet3Cardj));
                throw ex;
            }

        }
        public DataSet Placebetsport(Placebetteen placebetteen)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet dscom = new DataSet();
                DataSet dscasino = new DataSet();
                placebetteen.gtype.ToLower();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                parameters.Add(new SqlParameter() { ParameterName = "MarketId", Value = placebetteen.mid });
                parameters.Add(new SqlParameter() { ParameterName = "GameType", Value = placebetteen.gtype });
                ds = _sqlClientService.Execute("Casino_PlacebetCheck", ConfigItems.Conn_AccDgroup, parameters);
                WriteLogAll("Placebetsport+Casino_PlacebetCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds));

                //1 as Id,'ok' as msg,username,u.PartnershipType,general,ExposerLimit,oldexposer,AutoConfirm
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("id") && ds.Tables[0].Rows[0]["id"].ToString() == "1")
                {
                    var parameterscas = new List<SqlParameter>();
                    parameterscas.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                    parameterscas.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                    parameterscas.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                    parameterscas.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                    parameterscas.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                    dscasino = _sqlClientService.Execute("sports_placeExpoCheck", ConfigItems.Conn_Casinogroup, parameterscas);
                    WriteLogAll("Placebetsport1+sports_placeExpoCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscasino));
                    if (dscasino != null && dscasino.Tables != null && dscasino.Tables.Count > 0 && dscasino.Tables[0].Rows.Count > 0 && dscasino.Tables[0].Columns.Contains("id") && dscasino.Tables[0].Rows[0]["id"].ToString() == "1")
                    {
                        var parameters3 = new List<SqlParameter>();
                        parameters3.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                        parameters3.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                        parameters3.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                        parameters3.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                        parameters3.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                        parameters3.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                        parameters3.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                        dscom = _sqlClientService.Execute("group2_sports_placebetconform", ConfigItems.Conn_Casino, parameters3);
                        WriteLogAll("Placebetsport2+group2_sports_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscom));
                        if (dscom != null && dscom.Tables != null && dscom.Tables.Count > 0 && dscom.Tables[0].Rows.Count > 0 && dscom.Tables[0].Columns.Contains("id") && dscom.Tables[0].Rows[0]["id"].ToString() == "1")
                        {
                            var parameters1 = new List<SqlParameter>();
                            parameters1.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                            parameters1.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                            parameters1.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                            parameters1.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                            parameters1.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                            parameters1.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                            parameters1.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                            parameters1.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                            parameters1.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newUserrate", Value = dscom.Tables[0].Rows[0]["NewUserrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newAmount", Value = dscom.Tables[0].Rows[0]["Amount"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Gameid", Value = dscom.Tables[0].Rows[0]["Gameid"] });
                            ds1 = _sqlClientService.Execute("sports_placebetconform", ConfigItems.Conn_Casinogroup, parameters1);
                            WriteLogAll("Placebetsport3+sports_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds1));

                            //select 1 as ID,'Ok' as Msg,	@userexposer as userexposer, @JsonGameSub as masterexposer
                            if (ds1 != null && ds1.Tables != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0 && ds1.Tables[0].Columns.Contains("id") && ds1.Tables[0].Rows[0]["id"].ToString() == "1")
                            {
                                try
                                {
                                    var param = "marketid=" + placebetteen.mid + "&userid=" + placebetteen.uid + "&subsectionid=" + placebetteen.subid + "&gametype=" + placebetteen.gtype + "&subgametype="
                                                                     + placebetteen.subtype + "&username=" + ds.Tables[0].Rows[0]["username"] + "&sectionid=" + placebetteen.sid + "&bettype=" + placebetteen.btype
                                                                    + "&nation=" + ds1.Tables[0].Rows[0]["nation"].ToString() + "&userrate=" + placebetteen.urate + "&Masteramount=" + ds1.Tables[0].Rows[0]["conformamount"]
                                                                    + "&ipaddress=" + placebetteen.ip + "&browserdetail=" + placebetteen.bdetail + "&BookvalType=" + ds1.Tables[0].Rows[0]["exposertype"] +
                                                                    "&Bookvalmatch=" + ds1.Tables[0].Rows[0]["matchbook"] + "&BookvalFancy=" + ds1.Tables[0].Rows[0]["fancybook"] + "&amount=" + placebetteen.amt + "&frate=" + ds.Tables[0].Rows[0]["balrate"] + "&Currency=" + ds.Tables[0].Rows[0]["ccode"];

                                    Task.Factory.StartNew(() => Post(ConfigItems.AdminBookUrl + ApiEndpoint.sportbook, param, "application/x-www-form-urlencoded", "POST"));
                                }
                                catch (System.Exception ex)
                                {
                                    WriteLogAll("Placebetsport+bk" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                var parameters2 = new List<SqlParameter>();
                                parameters2.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                                parameters2.Add(new SqlParameter() { ParameterName = "Amount", Value = Convert.ToDouble(ds1.Tables[0].Rows[0]["userexposer"]) });
                                parameters2.Add(new SqlParameter() { ParameterName = "ExposJson", Value = ds1.Tables[0].Rows[0]["masterexposer"].ToString() });
                                parameters2.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                                parameters2.Add(new SqlParameter() { ParameterName = "gameType", Value = placebetteen.gtype });
                                parameters2.Add(new SqlParameter() { ParameterName = "Userrate", Value = ds1.Tables[0].Rows[0]["newuserrate"] });
                                parameters2.Add(new SqlParameter() { ParameterName = "BetAmount", Value = ds1.Tables[0].Rows[0]["conformamount"] });
                                ds2 = _sqlClientService.Execute("Casino_ExposerUpdate", ConfigItems.Conn_AccDgroup, parameters2);
                                WriteLogAll("Placebetsport4+Casino_ExposerUpdate", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds2));
                                try
                                {
                                    if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 0 && ds2.Tables[1].Rows.Count > 0 && ds2.Tables[1].Columns.Contains("SendCoupon") && ds2.Tables[1].Rows[0]["SendCoupon"].ToString() == "1")
                                    {//Select 0 as SendCoupon,0 as BetAmount,'' as couponcode
                                        CouponPlacebet couponPlacebet = new CouponPlacebet();
                                        couponPlacebet.Amount = Convert.ToDouble(ds1.Tables[0].Rows[0]["conformamount"]);
                                        couponPlacebet.PlayerName = ds.Tables[0].Rows[0]["username"].ToString();
                                        couponPlacebet.PlayerId = placebetteen.pid;
                                        couponPlacebet.Product = ds2.Tables[1].Rows[0]["couponcode"].ToString();
                                        couponPlacebet.Balance = Convert.ToDouble(ds.Tables[0].Rows[0]["general"]);
                                        if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 1 && ds2.Tables[2].Rows.Count > 0 && ds2.Tables[2].Columns.Contains("clientid") && ds2.Tables[2].Columns.Contains("secretkey"))
                                        {
                                            var par = ConfigItems.Encrypt ? Encryptionbonus.EncryptString(JsonConvert.SerializeObject(couponPlacebet), ds2.Tables[2].Rows[0]["secretkey"].ToString()) : JsonConvert.SerializeObject(couponPlacebet);
                                            var resp = Postbonus(ConfigItems.CopUrl + "transaction/event", par, "application/json", "POST", ds2.Tables[2].Rows[0]["clientid"].ToString());
                                            var res = ConfigItems.Encrypt ? Encryptionbonus.DecryptString(resp, ds2.Tables[2].Rows[0]["secretkey"].ToString()) : resp;
                                            WriteLogAll("Placebetsport3", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(res));
                                        }
                                        //var resp = Post(ConfigItems.CopUrl + ApiEndpoint.placebet, JsonConvert.SerializeObject(couponPlacebet), "application/json", "POST");
                                        //var data = JsonConvert.DeserializeObject<CouponPlacebetRes>(resp);

                                        //Returnds("0", data.error);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteLogAll("Placebetsport+cop" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                Placebetfinal(placebetteen.uid);
                                return ds2;
                            }
                            else
                            {
                                Placebetfinal(placebetteen.uid);
                                return ds1;
                            }
                        }
                        Placebetfinal(placebetteen.uid);
                        return dscom;
                    }
                    Placebetfinal(placebetteen.uid);
                    return dscasino;
                }
                //Placebetfinal(placebetteen.uid);
                return ds;
            }
            catch (Exception ex)
            {
                Placebetfinal(placebetteen.uid);
                WriteLogAll("Placebetsport" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                throw ex;
            }

        }
        public DataSet Placebetcard32(Placebetteen placebetteen)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet dscom = new DataSet();
                DataSet dscasino = new DataSet();
                placebetteen.gtype.ToLower();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                parameters.Add(new SqlParameter() { ParameterName = "MarketId", Value = placebetteen.mid });
                parameters.Add(new SqlParameter() { ParameterName = "GameType", Value = placebetteen.gtype });
                ds = _sqlClientService.Execute("Casino_PlacebetCheck", ConfigItems.Conn_AccDgroup, parameters);
                WriteLogAll("Placebetcard32+Casino_PlacebetCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds));

                //1 as Id,'ok' as msg,username,u.PartnershipType,general,ExposerLimit,oldexposer,AutoConfirm
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("id") && ds.Tables[0].Rows[0]["id"].ToString() == "1")
                {
                    var parameterscas = new List<SqlParameter>();
                    parameterscas.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                    parameterscas.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                    parameterscas.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                    parameterscas.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                    parameterscas.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                    dscasino = _sqlClientService.Execute("card32_placeExpoCheck", ConfigItems.Conn_Casinogroup, parameterscas);
                    WriteLogAll("Placebetcard321+card32_placeExpoCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscasino));
                    if (dscasino != null && dscasino.Tables != null && dscasino.Tables.Count > 0 && dscasino.Tables[0].Rows.Count > 0 && dscasino.Tables[0].Columns.Contains("id") && dscasino.Tables[0].Rows[0]["id"].ToString() == "1")
                    {
                        var parameters3 = new List<SqlParameter>();
                        parameters3.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                        parameters3.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                        parameters3.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                        parameters3.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                        parameters3.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                        parameters3.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                        parameters3.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                        dscom = _sqlClientService.Execute("group2_card32_placebetconform", ConfigItems.Conn_Casino, parameters3);
                        WriteLogAll("Placebetcard322+group2_card32_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscom));
                        if (dscom != null && dscom.Tables != null && dscom.Tables.Count > 0 && dscom.Tables[0].Rows.Count > 0 && dscom.Tables[0].Columns.Contains("id") && dscom.Tables[0].Rows[0]["id"].ToString() == "1")
                        {
                            var parameters1 = new List<SqlParameter>();
                            parameters1.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                            parameters1.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                            parameters1.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                            parameters1.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                            parameters1.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                            parameters1.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                            parameters1.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                            parameters1.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                            parameters1.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newUserrate", Value = dscom.Tables[0].Rows[0]["NewUserrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newAmount", Value = dscom.Tables[0].Rows[0]["Amount"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Gameid", Value = dscom.Tables[0].Rows[0]["Gameid"] });
                            ds1 = _sqlClientService.Execute("card32_placebetconform", ConfigItems.Conn_Casinogroup, parameters1);
                            WriteLogAll("Placebetcard323+card32_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds1));

                            //select 1 as ID,'Ok' as Msg,	@userexposer as userexposer, @JsonGameSub as masterexposer
                            if (ds1 != null && ds1.Tables != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0 && ds1.Tables[0].Columns.Contains("id") && ds1.Tables[0].Rows[0]["id"].ToString() == "1")
                            {
                                try
                                {
                                    var param = "marketid=" + placebetteen.mid + "&userid=" + placebetteen.uid + "&subsectionid=" + placebetteen.subid + "&gametype=" + placebetteen.gtype + "&subgametype="
                                                                     + placebetteen.subtype + "&username=" + ds.Tables[0].Rows[0]["username"] + "&sectionid=" + placebetteen.sid + "&bettype=" + placebetteen.btype
                                                                    + "&nation=" + ds1.Tables[0].Rows[0]["nation"].ToString().Replace("&", "*") + "&userrate=" + placebetteen.urate + "&Masteramount=" + ds1.Tables[0].Rows[0]["conformamount"]
                                                                    + "&ipaddress=" + placebetteen.ip + "&browserdetail=" + placebetteen.bdetail + "&BookvalType=" + ds1.Tables[0].Rows[0]["exposertype"] +
                                                                    "&Bookvalmatch=" + ds1.Tables[0].Rows[0]["matchbook"] + "&BookvalFancy=" + ds1.Tables[0].Rows[0]["fancybook"] + "&amount=" + placebetteen.amt + "&frate=" + ds.Tables[0].Rows[0]["balrate"] + "&Currency=" + ds.Tables[0].Rows[0]["ccode"];
                                    WriteLogAll("Placebetcard32+bk", param);
                                    Task.Factory.StartNew(() => Post(ConfigItems.AdminBookUrl + ApiEndpoint.card32book, param, "application/x-www-form-urlencoded", "POST"));
                                }
                                catch (System.Exception ex)
                                {
                                    WriteLogAll("Placebetcard32+bk" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                var parameters2 = new List<SqlParameter>();
                                parameters2.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                                parameters2.Add(new SqlParameter() { ParameterName = "Amount", Value = Convert.ToDouble(ds1.Tables[0].Rows[0]["userexposer"]) });
                                parameters2.Add(new SqlParameter() { ParameterName = "ExposJson", Value = ds1.Tables[0].Rows[0]["masterexposer"].ToString() });
                                parameters2.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                                parameters2.Add(new SqlParameter() { ParameterName = "gameType", Value = placebetteen.gtype });
                                parameters2.Add(new SqlParameter() { ParameterName = "Userrate", Value = ds1.Tables[0].Rows[0]["newuserrate"] });
                                parameters2.Add(new SqlParameter() { ParameterName = "BetAmount", Value = ds1.Tables[0].Rows[0]["conformamount"] });
                                ds2 = _sqlClientService.Execute("Casino_ExposerUpdate", ConfigItems.Conn_AccDgroup, parameters2);
                                WriteLogAll("Placebetcard324+Casino_ExposerUpdate", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds2));
                                try
                                {
                                    if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 0 && ds2.Tables[1].Rows.Count > 0 && ds2.Tables[1].Columns.Contains("SendCoupon") && ds2.Tables[1].Rows[0]["SendCoupon"].ToString() == "1")
                                    {//Select 0 as SendCoupon,0 as BetAmount,'' as couponcode
                                        CouponPlacebet couponPlacebet = new CouponPlacebet();
                                        couponPlacebet.Amount = Convert.ToDouble(ds1.Tables[0].Rows[0]["conformamount"]);
                                        couponPlacebet.PlayerName = ds.Tables[0].Rows[0]["username"].ToString();
                                        couponPlacebet.PlayerId = placebetteen.pid;
                                        couponPlacebet.Product = ds2.Tables[1].Rows[0]["couponcode"].ToString();
                                        couponPlacebet.Balance = Convert.ToDouble(ds.Tables[0].Rows[0]["general"]);
                                        if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 1 && ds2.Tables[2].Rows.Count > 0 && ds2.Tables[2].Columns.Contains("clientid") && ds2.Tables[2].Columns.Contains("secretkey"))
                                        {
                                            var par = ConfigItems.Encrypt ? Encryptionbonus.EncryptString(JsonConvert.SerializeObject(couponPlacebet), ds2.Tables[2].Rows[0]["secretkey"].ToString()) : JsonConvert.SerializeObject(couponPlacebet);
                                            var resp = Postbonus(ConfigItems.CopUrl + "transaction/event", par, "application/json", "POST", ds2.Tables[2].Rows[0]["clientid"].ToString());
                                            var res = ConfigItems.Encrypt ? Encryptionbonus.DecryptString(resp, ds2.Tables[2].Rows[0]["secretkey"].ToString()) : resp;
                                            WriteLogAll("Placebetcard323", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(res));
                                        }
                                        //var resp = Post(ConfigItems.CopUrl + ApiEndpoint.placebet, JsonConvert.SerializeObject(couponPlacebet), "application/json", "POST");
                                        //var data = JsonConvert.DeserializeObject<CouponPlacebetRes>(resp);

                                        //Returnds("0", data.error);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteLogAll("Placebetcard32+cop" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                Placebetfinal(placebetteen.uid);
                                return ds2;
                            }
                            else
                            {
                                Placebetfinal(placebetteen.uid);
                                return ds1;
                            }
                        }
                        Placebetfinal(placebetteen.uid);
                        return dscom;
                    }
                    Placebetfinal(placebetteen.uid);
                    return dscasino;
                }
                //Placebetfinal(placebetteen.uid);
                return ds;
            }
            catch (Exception ex)
            {
                Placebetfinal(placebetteen.uid);
                WriteLogAll("Placebetcard32" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                throw ex;
            }

        }
        public DataSet Placebetab(Placebetteen placebetteen)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet dscom = new DataSet();
                DataSet dscasino = new DataSet();
                placebetteen.gtype.ToLower();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                parameters.Add(new SqlParameter() { ParameterName = "MarketId", Value = placebetteen.mid });
                parameters.Add(new SqlParameter() { ParameterName = "GameType", Value = placebetteen.gtype });
                ds = _sqlClientService.Execute("Casino_PlacebetCheck", ConfigItems.Conn_AccDgroup, parameters);
                WriteLogAll("Placebetab+Casino_PlacebetCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds));
                //1 as Id,'ok' as msg,username,u.PartnershipType,general,ExposerLimit,oldexposer,AutoConfirm
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("id") && ds.Tables[0].Rows[0]["id"].ToString() == "1")
                {
                    var parameterscas = new List<SqlParameter>();
                    parameterscas.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                    parameterscas.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                    parameterscas.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                    parameterscas.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                    parameterscas.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                    dscasino = _sqlClientService.Execute("ab_placeExpoCheck", ConfigItems.Conn_Casinogroup, parameterscas);
                    WriteLogAll("Placebetab1+ab_placeExpoCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds1));
                    if (dscasino != null && dscasino.Tables != null && dscasino.Tables.Count > 0 && dscasino.Tables[0].Rows.Count > 0 && dscasino.Tables[0].Columns.Contains("id") && dscasino.Tables[0].Rows[0]["id"].ToString() == "1")
                    {
                        var parameters3 = new List<SqlParameter>();
                        parameters3.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                        parameters3.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                        parameters3.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                        parameters3.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                        parameters3.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                        parameters3.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                        parameters3.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                        dscom = _sqlClientService.Execute("group2_ab_placebetconform", ConfigItems.Conn_Casino, parameters3);
                        WriteLogAll("Placebetab2+group2_ab_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscom));
                        if (dscom != null && dscom.Tables != null && dscom.Tables.Count > 0 && dscom.Tables[0].Rows.Count > 0 && dscom.Tables[0].Columns.Contains("id") && dscom.Tables[0].Rows[0]["id"].ToString() == "1")
                        {//Amount,NewUserrate Gameid
                            var parameters1 = new List<SqlParameter>();
                            parameters1.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                            parameters1.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                            parameters1.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                            parameters1.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                            parameters1.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                            parameters1.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                            parameters1.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                            parameters1.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                            parameters1.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newUserrate", Value = dscom.Tables[0].Rows[0]["NewUserrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newAmount", Value = dscom.Tables[0].Rows[0]["Amount"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Gameid", Value = dscom.Tables[0].Rows[0]["Gameid"] });
                            ds1 = _sqlClientService.Execute("ab_placebetconform", ConfigItems.Conn_Casinogroup, parameters1);
                            WriteLogAll("Placebetab3+ab_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds1));

                            //select 1 as ID,'Ok' as Msg,	@userexposer as userexposer, @JsonGameSub as masterexposer
                            if (ds1 != null && ds1.Tables != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0 && ds1.Tables[0].Columns.Contains("id") && ds1.Tables[0].Rows[0]["id"].ToString() == "1")
                            {
                                try
                                {
                                    var param = "marketid=" + placebetteen.mid + "&userid=" + placebetteen.uid + "&subsectionid=" + placebetteen.subid + "&gametype=" + placebetteen.gtype + "&subgametype="
                                                                     + placebetteen.subtype + "&username=" + ds.Tables[0].Rows[0]["username"] + "&sectionid=" + placebetteen.sid + "&bettype=" + placebetteen.btype
                                                                    + "&nation=" + ds1.Tables[0].Rows[0]["nation"].ToString() + "&userrate=" + placebetteen.urate + "&Masteramount=" + ds1.Tables[0].Rows[0]["conformamount"]
                                                                    + "&ipaddress=" + placebetteen.ip + "&browserdetail=" + placebetteen.bdetail + "&BookvalType=" + ds1.Tables[0].Rows[0]["exposertype"] +
                                                                    "&Bookvalmatch=" + ds1.Tables[0].Rows[0]["matchbook"] + "&BookvalFancy=" + ds1.Tables[0].Rows[0]["fancybook"] + "&amount=" + placebetteen.amt + "&frate=" + ds.Tables[0].Rows[0]["balrate"] + "&Currency=" + ds.Tables[0].Rows[0]["ccode"];

                                    Task.Factory.StartNew(() => Post(ConfigItems.AdminBookUrl + ApiEndpoint.abbook, param, "application/x-www-form-urlencoded", "POST"));
                                }
                                catch (System.Exception ex)
                                {
                                    WriteLogAll("Placebetab+bk" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                var parameters2 = new List<SqlParameter>();
                                parameters2.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                                parameters2.Add(new SqlParameter() { ParameterName = "Amount", Value = Convert.ToDouble(ds1.Tables[0].Rows[0]["userexposer"]) });
                                parameters2.Add(new SqlParameter() { ParameterName = "ExposJson", Value = ds1.Tables[0].Rows[0]["masterexposer"].ToString() });
                                parameters2.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                                parameters2.Add(new SqlParameter() { ParameterName = "gameType", Value = placebetteen.gtype });
                                parameters2.Add(new SqlParameter() { ParameterName = "Userrate", Value = ds1.Tables[0].Rows[0]["newuserrate"] });
                                parameters2.Add(new SqlParameter() { ParameterName = "BetAmount", Value = ds1.Tables[0].Rows[0]["conformamount"] });
                                ds2 = _sqlClientService.Execute("Casino_ExposerUpdate", ConfigItems.Conn_AccDgroup, parameters2);
                                WriteLogAll("Placebetab4+Casino_ExposerUpdate", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds2));
                                try
                                {
                                    if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 0 && ds2.Tables[1].Rows.Count > 0 && ds2.Tables[1].Columns.Contains("SendCoupon") && ds2.Tables[1].Rows[0]["SendCoupon"].ToString() == "1")
                                    {//Select 0 as SendCoupon,0 as BetAmount,'' as couponcode
                                        CouponPlacebet couponPlacebet = new CouponPlacebet();
                                        couponPlacebet.Amount = Convert.ToDouble(ds1.Tables[0].Rows[0]["conformamount"]);
                                        couponPlacebet.PlayerName = ds.Tables[0].Rows[0]["username"].ToString();
                                        couponPlacebet.PlayerId = placebetteen.pid;
                                        couponPlacebet.Product = ds2.Tables[1].Rows[0]["couponcode"].ToString();
                                        couponPlacebet.Balance = Convert.ToDouble(ds.Tables[0].Rows[0]["general"]);
                                        if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 1 && ds2.Tables[2].Rows.Count > 0 && ds2.Tables[2].Columns.Contains("clientid") && ds2.Tables[2].Columns.Contains("secretkey"))
                                        {
                                            var par = ConfigItems.Encrypt ? Encryptionbonus.EncryptString(JsonConvert.SerializeObject(couponPlacebet), ds2.Tables[2].Rows[0]["secretkey"].ToString()) : JsonConvert.SerializeObject(couponPlacebet);
                                            var resp = Postbonus(ConfigItems.CopUrl + "transaction/event", par, "application/json", "POST", ds2.Tables[2].Rows[0]["clientid"].ToString());
                                            var res = ConfigItems.Encrypt ? Encryptionbonus.DecryptString(resp, ds2.Tables[2].Rows[0]["secretkey"].ToString()) : resp;
                                            WriteLogAll("Placebetab3", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(res));
                                        }
                                        //var resp = Post(ConfigItems.CopUrl + ApiEndpoint.placebet, JsonConvert.SerializeObject(couponPlacebet), "application/json", "POST");
                                        //var data = JsonConvert.DeserializeObject<CouponPlacebetRes>(resp);

                                        //Returnds("0", data.error);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteLogAll("Placebetab+cop" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                Placebetfinal(placebetteen.uid);
                                return ds2;
                            }
                            else
                            {
                                Placebetfinal(placebetteen.uid);
                                return ds1;
                            }
                        }
                        Placebetfinal(placebetteen.uid);
                        return dscom;
                    }
                    Placebetfinal(placebetteen.uid);
                    return dscasino;
                }
                return ds;
            }
            catch (Exception ex)
            {
                Placebetfinal(placebetteen.uid);
                WriteLogAll("Placebetab" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                throw ex;
            }

        }
        public DataSet Placebetqueen(Placebetteen placebetteen)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet dscom = new DataSet();
                DataSet dscasino = new DataSet();
                placebetteen.gtype.ToLower();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                parameters.Add(new SqlParameter() { ParameterName = "MarketId", Value = placebetteen.mid });
                parameters.Add(new SqlParameter() { ParameterName = "GameType", Value = placebetteen.gtype });
                ds = _sqlClientService.Execute("Casino_PlacebetCheck", ConfigItems.Conn_AccDgroup, parameters);
                WriteLogAll("Placebetqueen+Casino_PlacebetCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds));
                //1 as Id,'ok' as msg,username,u.PartnershipType,general,ExposerLimit,oldexposer,AutoConfirm
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("id") && ds.Tables[0].Rows[0]["id"].ToString() == "1")
                {
                    var parameterscas = new List<SqlParameter>();
                    parameterscas.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                    parameterscas.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                    parameterscas.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                    parameterscas.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                    parameterscas.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                    dscasino = _sqlClientService.Execute("queen_placeExpoCheck", ConfigItems.Conn_Casinogroup, parameterscas);
                    WriteLogAll("Placebetqueen1+queen_placeExpoCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscasino));
                    if (dscasino != null && dscasino.Tables != null && dscasino.Tables.Count > 0 && dscasino.Tables[0].Rows.Count > 0 && dscasino.Tables[0].Columns.Contains("id") && dscasino.Tables[0].Rows[0]["id"].ToString() == "1")
                    {
                        var parameters3 = new List<SqlParameter>();
                        parameters3.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                        parameters3.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                        parameters3.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                        parameters3.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                        parameters3.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                        parameters3.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                        parameters3.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                        dscom = _sqlClientService.Execute("group2_queen_placebetconform", ConfigItems.Conn_Casino, parameters3);
                        WriteLogAll("Placebetqueen2+group2_queen_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscom));
                        if (dscom != null && dscom.Tables != null && dscom.Tables.Count > 0 && dscom.Tables[0].Rows.Count > 0 && dscom.Tables[0].Columns.Contains("id") && dscom.Tables[0].Rows[0]["id"].ToString() == "1")
                        {
                            var parameters1 = new List<SqlParameter>();
                            parameters1.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                            parameters1.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                            parameters1.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                            parameters1.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                            parameters1.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                            parameters1.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                            parameters1.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                            parameters1.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                            parameters1.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newUserrate", Value = dscom.Tables[0].Rows[0]["NewUserrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newAmount", Value = dscom.Tables[0].Rows[0]["Amount"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Gameid", Value = dscom.Tables[0].Rows[0]["Gameid"] });
                            ds1 = _sqlClientService.Execute("queen_placebetconform", ConfigItems.Conn_Casinogroup, parameters1);
                            WriteLogAll("Placebetqueen3+queen_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds1));

                            //select 1 as ID,'Ok' as Msg,	@userexposer as userexposer, @JsonGameSub as masterexposer
                            if (ds1 != null && ds1.Tables != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0 && ds1.Tables[0].Columns.Contains("id") && ds1.Tables[0].Rows[0]["id"].ToString() == "1")
                            {
                                try
                                {
                                    var param = "marketid=" + placebetteen.mid + "&userid=" + placebetteen.uid + "&subsectionid=" + placebetteen.subid + "&gametype=" + placebetteen.gtype + "&subgametype="
                                                                     + placebetteen.subtype + "&username=" + ds.Tables[0].Rows[0]["username"] + "&sectionid=" + placebetteen.sid + "&bettype=" + placebetteen.btype
                                                                    + "&nation=" + ds1.Tables[0].Rows[0]["nation"].ToString() + "&userrate=" + placebetteen.urate + "&Masteramount=" + ds1.Tables[0].Rows[0]["conformamount"]
                                                                    + "&ipaddress=" + placebetteen.ip + "&browserdetail=" + placebetteen.bdetail + "&BookvalType=" + ds1.Tables[0].Rows[0]["exposertype"] +
                                                                    "&Bookvalmatch=" + ds1.Tables[0].Rows[0]["matchbook"] + "&BookvalFancy=" + ds1.Tables[0].Rows[0]["fancybook"] + "&amount=" + placebetteen.amt + "&frate=" + ds.Tables[0].Rows[0]["balrate"] + "&Currency=" + ds.Tables[0].Rows[0]["ccode"];

                                    Task.Factory.StartNew(() => Post(ConfigItems.AdminBookUrl + ApiEndpoint.queenbook, param, "application/x-www-form-urlencoded", "POST"));
                                }
                                catch (System.Exception ex)
                                {
                                    WriteLogAll("Placebetqueen+bk" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                var parameters2 = new List<SqlParameter>();
                                parameters2.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                                parameters2.Add(new SqlParameter() { ParameterName = "Amount", Value = Convert.ToDouble(ds1.Tables[0].Rows[0]["userexposer"]) });
                                parameters2.Add(new SqlParameter() { ParameterName = "ExposJson", Value = ds1.Tables[0].Rows[0]["masterexposer"].ToString() });
                                parameters2.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                                parameters2.Add(new SqlParameter() { ParameterName = "gameType", Value = placebetteen.gtype });
                                parameters2.Add(new SqlParameter() { ParameterName = "Userrate", Value = ds1.Tables[0].Rows[0]["newuserrate"] });
                                parameters2.Add(new SqlParameter() { ParameterName = "BetAmount", Value = ds1.Tables[0].Rows[0]["conformamount"] });
                                ds2 = _sqlClientService.Execute("Casino_ExposerUpdate", ConfigItems.Conn_AccDgroup, parameters2);
                                WriteLogAll("Placebetqueen4+Casino_ExposerUpdate", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds2));
                                try
                                {
                                    if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 0 && ds2.Tables[1].Rows.Count > 0 && ds2.Tables[1].Columns.Contains("SendCoupon") && ds2.Tables[1].Rows[0]["SendCoupon"].ToString() == "1")
                                    {//Select 0 as SendCoupon,0 as BetAmount,'' as couponcode
                                        CouponPlacebet couponPlacebet = new CouponPlacebet();
                                        couponPlacebet.Amount = Convert.ToDouble(ds1.Tables[0].Rows[0]["conformamount"]);
                                        couponPlacebet.PlayerName = ds.Tables[0].Rows[0]["username"].ToString();
                                        couponPlacebet.PlayerId = placebetteen.pid;
                                        couponPlacebet.Product = ds2.Tables[1].Rows[0]["couponcode"].ToString();
                                        couponPlacebet.Balance = Convert.ToDouble(ds.Tables[0].Rows[0]["general"]);
                                        if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 1 && ds2.Tables[2].Rows.Count > 0 && ds2.Tables[2].Columns.Contains("clientid") && ds2.Tables[2].Columns.Contains("secretkey"))
                                        {
                                            var par = ConfigItems.Encrypt ? Encryptionbonus.EncryptString(JsonConvert.SerializeObject(couponPlacebet), ds2.Tables[2].Rows[0]["secretkey"].ToString()) : JsonConvert.SerializeObject(couponPlacebet);
                                            var resp = Postbonus(ConfigItems.CopUrl + "transaction/event", par, "application/json", "POST", ds2.Tables[2].Rows[0]["clientid"].ToString());
                                            var res = ConfigItems.Encrypt ? Encryptionbonus.DecryptString(resp, ds2.Tables[2].Rows[0]["secretkey"].ToString()) : resp;
                                            WriteLogAll("Placebetqueen3", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(res));
                                        }
                                        //var resp = Post(ConfigItems.CopUrl + ApiEndpoint.placebet, JsonConvert.SerializeObject(couponPlacebet), "application/json", "POST");
                                        //var data = JsonConvert.DeserializeObject<CouponPlacebetRes>(resp);

                                        //Returnds("0", data.error);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteLogAll("Placebetqueen+cop" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                Placebetfinal(placebetteen.uid);
                                return ds2;
                            }
                            else
                            {
                                Placebetfinal(placebetteen.uid);
                                return ds1;
                            }
                        }
                        Placebetfinal(placebetteen.uid);
                        return dscom;
                    }
                    Placebetfinal(placebetteen.uid);
                    return dscasino;
                }
                //Placebetfinal(placebetteen.uid);
                return ds;
            }
            catch (Exception ex)
            {
                Placebetfinal(placebetteen.uid);
                WriteLogAll("Placebetqueen" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                throw ex;
            }

        }
        public DataSet Placebetrace(Placebetteen placebetteen)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet dscom = new DataSet();
                DataSet dscasino = new DataSet();
                placebetteen.gtype.ToLower();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                parameters.Add(new SqlParameter() { ParameterName = "MarketId", Value = placebetteen.mid });
                parameters.Add(new SqlParameter() { ParameterName = "GameType", Value = placebetteen.gtype });
                ds = _sqlClientService.Execute("Casino_PlacebetCheck", ConfigItems.Conn_AccDgroup, parameters);
                WriteLogAll("Placebetrace+Casino_PlacebetCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds));

                //1 as Id,'ok' as msg,username,u.PartnershipType,general,ExposerLimit,oldexposer,AutoConfirm
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("id") && ds.Tables[0].Rows[0]["id"].ToString() == "1")
                {
                    var parameterscas = new List<SqlParameter>();
                    parameterscas.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                    parameterscas.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                    parameterscas.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                    parameterscas.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                    parameterscas.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                    dscasino = _sqlClientService.Execute("race_placeExpoCheck", ConfigItems.Conn_Casinogroup, parameterscas);
                    WriteLogAll("Placebetrace1+race_placeExpoCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscasino));
                    if (dscasino != null && dscasino.Tables != null && dscasino.Tables.Count > 0 && dscasino.Tables[0].Rows.Count > 0 && dscasino.Tables[0].Columns.Contains("id") && dscasino.Tables[0].Rows[0]["id"].ToString() == "1")
                    {
                        var parameters3 = new List<SqlParameter>();
                        parameters3.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                        parameters3.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                        parameters3.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                        parameters3.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                        parameters3.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                        parameters3.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                        parameters3.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                        dscom = _sqlClientService.Execute("group2_race_placebetconform", ConfigItems.Conn_Casino, parameters3);
                        WriteLogAll("Placebetrace2+group2_race_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscom));
                        if (dscom != null && dscom.Tables != null && dscom.Tables.Count > 0 && dscom.Tables[0].Rows.Count > 0 && dscom.Tables[0].Columns.Contains("id") && dscom.Tables[0].Rows[0]["id"].ToString() == "1")
                        {
                            var parameters1 = new List<SqlParameter>();
                            parameters1.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                            parameters1.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                            parameters1.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                            parameters1.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                            parameters1.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                            parameters1.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                            parameters1.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                            parameters1.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                            parameters1.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newUserrate", Value = dscom.Tables[0].Rows[0]["NewUserrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newAmount", Value = dscom.Tables[0].Rows[0]["Amount"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Gameid", Value = dscom.Tables[0].Rows[0]["Gameid"] });
                            ds1 = _sqlClientService.Execute("race_placebetconform", ConfigItems.Conn_Casinogroup, parameters1);
                            WriteLogAll("Placebetrace3+race_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds1));

                            //select 1 as ID,'Ok' as Msg,	@userexposer as userexposer, @JsonGameSub as masterexposer
                            if (ds1 != null && ds1.Tables != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0 && ds1.Tables[0].Columns.Contains("id") && ds1.Tables[0].Rows[0]["id"].ToString() == "1")
                            {
                                try
                                {
                                    var param = "marketid=" + placebetteen.mid + "&userid=" + placebetteen.uid + "&subsectionid=" + placebetteen.subid + "&gametype=" + placebetteen.gtype + "&subgametype="
                                                                     + placebetteen.subtype + "&username=" + ds.Tables[0].Rows[0]["username"] + "&sectionid=" + placebetteen.sid + "&bettype=" + placebetteen.btype
                                                                    + "&nation=" + ds1.Tables[0].Rows[0]["nation"].ToString() + "&userrate=" + placebetteen.urate + "&Masteramount=" + ds1.Tables[0].Rows[0]["conformamount"]
                                                                    + "&ipaddress=" + placebetteen.ip + "&browserdetail=" + placebetteen.bdetail + "&BookvalType=" + ds1.Tables[0].Rows[0]["exposertype"] +
                                                                    "&Bookvalmatch=" + ds1.Tables[0].Rows[0]["matchbook"] + "&BookvalFancy=" + ds1.Tables[0].Rows[0]["fancybook"] + "&amount=" + placebetteen.amt + "&frate=" + ds.Tables[0].Rows[0]["balrate"] + "&Currency=" + ds.Tables[0].Rows[0]["ccode"];

                                    Task.Factory.StartNew(() => Post(ConfigItems.AdminBookUrl + ApiEndpoint.racebook, param, "application/x-www-form-urlencoded", "POST"));
                                }
                                catch (System.Exception ex)
                                {
                                    WriteLogAll("Placebetrace+bk" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                var parameters2 = new List<SqlParameter>();
                                parameters2.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                                parameters2.Add(new SqlParameter() { ParameterName = "Amount", Value = Convert.ToDouble(ds1.Tables[0].Rows[0]["userexposer"]) });
                                parameters2.Add(new SqlParameter() { ParameterName = "ExposJson", Value = ds1.Tables[0].Rows[0]["masterexposer"].ToString() });
                                parameters2.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                                parameters2.Add(new SqlParameter() { ParameterName = "gameType", Value = placebetteen.gtype });
                                parameters2.Add(new SqlParameter() { ParameterName = "Userrate", Value = ds1.Tables[0].Rows[0]["newuserrate"] });
                                parameters2.Add(new SqlParameter() { ParameterName = "BetAmount", Value = ds1.Tables[0].Rows[0]["conformamount"] });
                                ds2 = _sqlClientService.Execute("Casino_ExposerUpdate", ConfigItems.Conn_AccDgroup, parameters2);
                                WriteLogAll("Placebetrace4+Casino_ExposerUpdate", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds2));
                                try
                                {
                                    if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 0 && ds2.Tables[1].Rows.Count > 0 && ds2.Tables[1].Columns.Contains("SendCoupon") && ds2.Tables[1].Rows[0]["SendCoupon"].ToString() == "1")
                                    {//Select 0 as SendCoupon,0 as BetAmount,'' as couponcode
                                        CouponPlacebet couponPlacebet = new CouponPlacebet();
                                        couponPlacebet.Amount = Convert.ToDouble(ds1.Tables[0].Rows[0]["conformamount"]);
                                        couponPlacebet.PlayerName = ds.Tables[0].Rows[0]["username"].ToString();
                                        couponPlacebet.PlayerId = placebetteen.pid;
                                        couponPlacebet.Product = ds2.Tables[1].Rows[0]["couponcode"].ToString();
                                        couponPlacebet.Balance = Convert.ToDouble(ds.Tables[0].Rows[0]["general"]);
                                        if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 1 && ds2.Tables[2].Rows.Count > 0 && ds2.Tables[2].Columns.Contains("clientid") && ds2.Tables[2].Columns.Contains("secretkey"))
                                        {
                                            var par = ConfigItems.Encrypt ? Encryptionbonus.EncryptString(JsonConvert.SerializeObject(couponPlacebet), ds2.Tables[2].Rows[0]["secretkey"].ToString()) : JsonConvert.SerializeObject(couponPlacebet);
                                            var resp = Postbonus(ConfigItems.CopUrl + "transaction/event", par, "application/json", "POST", ds2.Tables[2].Rows[0]["clientid"].ToString());
                                            var res = ConfigItems.Encrypt ? Encryptionbonus.DecryptString(resp, ds2.Tables[2].Rows[0]["secretkey"].ToString()) : resp;
                                            WriteLogAll("Placebetrace3", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(res));
                                        }
                                        //var resp = Post(ConfigItems.CopUrl + ApiEndpoint.placebet, JsonConvert.SerializeObject(couponPlacebet), "application/json", "POST");
                                        //var data = JsonConvert.DeserializeObject<CouponPlacebetRes>(resp);

                                        //Returnds("0", data.error);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteLogAll("Placebetrace+cop" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                Placebetfinal(placebetteen.uid);
                                return ds2;
                            }
                            else
                            {
                                Placebetfinal(placebetteen.uid);
                                return ds1;
                            }
                        }
                        Placebetfinal(placebetteen.uid);
                        return dscom;
                    }
                    Placebetfinal(placebetteen.uid);
                    return dscasino;
                }
                //Placebetfinal(placebetteen.uid);
                return ds;
            }
            catch (Exception ex)
            {
                Placebetfinal(placebetteen.uid);
                WriteLogAll("Placebetrace" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                throw ex;
            }

        }
        public DataSet Placebetlottery(Placebet3cardj placebet3Cardj)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet dscom = new DataSet();
                DataSet dscasino = new DataSet();
                placebet3Cardj.gtype.ToLower();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { ParameterName = "Userid", Value = placebet3Cardj.uid });
                parameters.Add(new SqlParameter() { ParameterName = "MarketId", Value = placebet3Cardj.mid });
                parameters.Add(new SqlParameter() { ParameterName = "GameType", Value = placebet3Cardj.gtype });
                ds = _sqlClientService.Execute("Casino_PlacebetCheck", ConfigItems.Conn_AccDgroup, parameters);
                WriteLogAll("Placebetlottery+Casino_PlacebetCheck", "Req:" + JsonConvert.SerializeObject(placebet3Cardj) + "Res" + JsonConvert.SerializeObject(ds));

                //1 as Id,'ok' as msg,username,u.PartnershipType,general,ExposerLimit,oldexposer,AutoConfirm
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("id") && ds.Tables[0].Rows[0]["id"].ToString() == "1")
                {
                    var parameterscas = new List<SqlParameter>();
                    parameterscas.Add(new SqlParameter() { ParameterName = "marketid", Value = placebet3Cardj.mid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebet3Cardj.sid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebet3Cardj.subid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "bettype", Value = placebet3Cardj.btype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userrate", Value = placebet3Cardj.urate });
                    parameterscas.Add(new SqlParameter() { ParameterName = "amount", Value = placebet3Cardj.amt });
                    parameterscas.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "gametype", Value = placebet3Cardj.gtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebet3Cardj.subtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userid", Value = placebet3Cardj.uid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebet3Cardj.ip });
                    parameterscas.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebet3Cardj.bdetail });
                    parameterscas.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "usercard", Value = placebet3Cardj.ucard });
                    dscasino = _sqlClientService.Execute("lottery_placeExpoCheck", ConfigItems.Conn_Casinogroup, parameterscas);
                    WriteLogAll("Placebetlottery1+lottery_placeExpoCheck", "Req:" + JsonConvert.SerializeObject(placebet3Cardj) + "Res" + JsonConvert.SerializeObject(dscasino));
                    if (dscasino != null && dscasino.Tables != null && dscasino.Tables.Count > 0 && dscasino.Tables[0].Rows.Count > 0 && dscasino.Tables[0].Columns.Contains("id") && dscasino.Tables[0].Rows[0]["id"].ToString() == "1")
                    {
                        var parameters3 = new List<SqlParameter>();
                        parameters3.Add(new SqlParameter() { ParameterName = "marketid", Value = placebet3Cardj.mid });
                        parameters3.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebet3Cardj.sid });
                        parameters3.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebet3Cardj.subid });
                        parameters3.Add(new SqlParameter() { ParameterName = "bettype", Value = placebet3Cardj.btype });
                        parameters3.Add(new SqlParameter() { ParameterName = "userrate", Value = placebet3Cardj.urate });
                        parameters3.Add(new SqlParameter() { ParameterName = "amount", Value = placebet3Cardj.amt });
                        parameters3.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "gametype", Value = placebet3Cardj.gtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebet3Cardj.subtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "userid", Value = placebet3Cardj.uid });
                        parameters3.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebet3Cardj.ip });
                        parameters3.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebet3Cardj.bdetail });
                        parameters3.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "usercard", Value = placebet3Cardj.ucard });
                        dscom = _sqlClientService.Execute("group2_lottery_placebetconform", ConfigItems.Conn_Casino, parameters3);
                        WriteLogAll("Placebetlottery2+group2_lottery_placebetconform", "Req:" + JsonConvert.SerializeObject(placebet3Cardj) + "Res" + JsonConvert.SerializeObject(dscom));
                        if (dscom != null && dscom.Tables != null && dscom.Tables.Count > 0 && dscom.Tables[0].Rows.Count > 0 && dscom.Tables[0].Columns.Contains("id") && dscom.Tables[0].Rows[0]["id"].ToString() == "1")
                        {
                            var parameters1 = new List<SqlParameter>();
                            parameters1.Add(new SqlParameter() { ParameterName = "marketid", Value = placebet3Cardj.mid });
                            parameters1.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebet3Cardj.sid });
                            parameters1.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebet3Cardj.subid });
                            parameters1.Add(new SqlParameter() { ParameterName = "bettype", Value = placebet3Cardj.btype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userrate", Value = placebet3Cardj.urate });
                            parameters1.Add(new SqlParameter() { ParameterName = "amount", Value = placebet3Cardj.amt });
                            parameters1.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "gametype", Value = placebet3Cardj.gtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebet3Cardj.subtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userid", Value = placebet3Cardj.uid });
                            parameters1.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebet3Cardj.ip });
                            parameters1.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebet3Cardj.bdetail });
                            parameters1.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "usercard", Value = placebet3Cardj.ucard });
                            parameters1.Add(new SqlParameter() { ParameterName = "newUserrate", Value = dscom.Tables[0].Rows[0]["NewUserrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newAmount", Value = dscom.Tables[0].Rows[0]["Amount"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Gameid", Value = dscom.Tables[0].Rows[0]["Gameid"] });
                            ds1 = _sqlClientService.Execute("lottery_placebetconform", ConfigItems.Conn_Casinogroup, parameters1);
                            WriteLogAll("Placebetlottery3+lottery_placebetconform", "Req:" + JsonConvert.SerializeObject(placebet3Cardj) + "Res" + JsonConvert.SerializeObject(ds1));

                            //select 1 as ID,'Ok' as Msg,	@userexposer as userexposer, @JsonGameSub as masterexposer
                            if (ds1 != null && ds1.Tables != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0 && ds1.Tables[0].Columns.Contains("id") && ds1.Tables[0].Rows[0]["id"].ToString() == "1")
                            {
                                try
                                {
                                    var param = "marketid=" + placebet3Cardj.mid + "&userid=" + placebet3Cardj.uid + "&subsectionid=" + placebet3Cardj.subid + "&gametype=" + placebet3Cardj.gtype + "&subgametype="
                                                                     + placebet3Cardj.subtype + "&username=" + ds.Tables[0].Rows[0]["username"] + "&sectionid=" + placebet3Cardj.sid + "&bettype=" + placebet3Cardj.btype
                                                                    + "&nation=" + ds1.Tables[0].Rows[0]["nation"].ToString() + "&userrate=" + placebet3Cardj.urate + "&Masteramount=" + ds1.Tables[0].Rows[0]["conformamount"]
                                                                    + "&ipaddress=" + placebet3Cardj.ip + "&browserdetail=" + placebet3Cardj.bdetail + "&BookvalType=" + ds1.Tables[0].Rows[0]["exposertype"] +
                                                                    "&Bookvalmatch=" + ds1.Tables[0].Rows[0]["matchbook"] + "&BookvalFancy=" + ds1.Tables[0].Rows[0]["fancybook"] + "&amount=" + placebet3Cardj.amt + "&frate=" + ds.Tables[0].Rows[0]["balrate"] + "&Currency=" + ds.Tables[0].Rows[0]["ccode"];

                                    Task.Factory.StartNew(() => Post(ConfigItems.AdminBookUrl + ApiEndpoint.lotterybook, param, "application/x-www-form-urlencoded", "POST"));
                                }
                                catch (System.Exception ex)
                                {
                                    WriteLogAll("Placebetlottery+bk" + ex.ToString(), JsonConvert.SerializeObject(placebet3Cardj));
                                }
                                var parameters2 = new List<SqlParameter>();
                                parameters2.Add(new SqlParameter() { ParameterName = "Userid", Value = placebet3Cardj.uid });
                                parameters2.Add(new SqlParameter() { ParameterName = "Amount", Value = Convert.ToDouble(ds1.Tables[0].Rows[0]["userexposer"]) });
                                parameters2.Add(new SqlParameter() { ParameterName = "ExposJson", Value = ds1.Tables[0].Rows[0]["masterexposer"].ToString() });
                                parameters2.Add(new SqlParameter() { ParameterName = "marketid", Value = placebet3Cardj.mid });
                                parameters2.Add(new SqlParameter() { ParameterName = "gameType", Value = placebet3Cardj.gtype });
                                parameters2.Add(new SqlParameter() { ParameterName = "Userrate", Value = ds1.Tables[0].Rows[0]["newuserrate"] });
                                parameters2.Add(new SqlParameter() { ParameterName = "BetAmount", Value = ds1.Tables[0].Rows[0]["conformamount"] });
                                ds2 = _sqlClientService.Execute("Casino_ExposerUpdate", ConfigItems.Conn_AccDgroup, parameters2);
                                WriteLogAll("Placebetlottery4+Casino_ExposerUpdate", "Req:" + JsonConvert.SerializeObject(placebet3Cardj) + "Res" + JsonConvert.SerializeObject(ds2));
                                try
                                {
                                    if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 0 && ds2.Tables[1].Rows.Count > 0 && ds2.Tables[1].Columns.Contains("SendCoupon") && ds2.Tables[1].Rows[0]["SendCoupon"].ToString() == "1")
                                    {//Select 0 as SendCoupon,0 as BetAmount,'' as couponcode
                                        CouponPlacebet couponPlacebet = new CouponPlacebet();
                                        couponPlacebet.Amount = Convert.ToDouble(ds1.Tables[0].Rows[0]["conformamount"]);
                                        couponPlacebet.PlayerName = ds.Tables[0].Rows[0]["username"].ToString();
                                        couponPlacebet.PlayerId = placebet3Cardj.pid;
                                        couponPlacebet.Product = ds2.Tables[1].Rows[0]["couponcode"].ToString();
                                        couponPlacebet.Balance = Convert.ToDouble(ds.Tables[0].Rows[0]["general"]);
                                        if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 1 && ds2.Tables[2].Rows.Count > 0 && ds2.Tables[2].Columns.Contains("clientid") && ds2.Tables[2].Columns.Contains("secretkey"))
                                        {
                                            var par = ConfigItems.Encrypt ? Encryptionbonus.EncryptString(JsonConvert.SerializeObject(couponPlacebet), ds2.Tables[2].Rows[0]["secretkey"].ToString()) : JsonConvert.SerializeObject(couponPlacebet);
                                            var resp = Postbonus(ConfigItems.CopUrl + "transaction/event", par, "application/json", "POST", ds2.Tables[2].Rows[0]["clientid"].ToString());
                                            var res = ConfigItems.Encrypt ? Encryptionbonus.DecryptString(resp, ds2.Tables[2].Rows[0]["secretkey"].ToString()) : resp;
                                            WriteLogAll("Placebetlottery3", "Req:" + JsonConvert.SerializeObject(placebet3Cardj) + "Res" + JsonConvert.SerializeObject(res));
                                        }
                                        //var resp = Post(ConfigItems.CopUrl + ApiEndpoint.placebet, JsonConvert.SerializeObject(couponPlacebet), "application/json", "POST");
                                        //var data = JsonConvert.DeserializeObject<CouponPlacebetRes>(resp);

                                        //Returnds("0", data.error);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteLogAll("Placebetlottery+cop" + ex.ToString(), JsonConvert.SerializeObject(placebet3Cardj));
                                }
                                Placebetfinal(placebet3Cardj.uid);
                                return ds2;
                            }
                            else
                            {
                                Placebetfinal(placebet3Cardj.uid);
                                return ds1;
                            }
                        }
                        Placebetfinal(placebet3Cardj.uid);
                        return dscom;
                    }
                    Placebetfinal(placebet3Cardj.uid);
                    return dscasino;
                }
                //Placebetfinal(placebet3Cardj.uid);
                return ds;
            }
            catch (Exception ex)
            {
                Placebetfinal(placebet3Cardj.uid);
                WriteLogAll("Placebetlottery" + ex.ToString(), JsonConvert.SerializeObject(placebet3Cardj));
                throw ex;
            }

        }
        public DataSet Placebetlotteryrep(Placebetlotteryrep placebetlotteryrep)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet dscom = new DataSet();
                DataSet dscasino = new DataSet();
                placebetlotteryrep.gtype.ToLower();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetlotteryrep.uid });
                parameters.Add(new SqlParameter() { ParameterName = "MarketId", Value = placebetlotteryrep.mid });
                parameters.Add(new SqlParameter() { ParameterName = "GameType", Value = placebetlotteryrep.gtype });
                ds = _sqlClientService.Execute("Casino_PlacebetCheck", ConfigItems.Conn_AccDgroup, parameters);
                WriteLogAll("Placebetlotteryrep+Casino_PlacebetCheck", "Req:" + JsonConvert.SerializeObject(placebetlotteryrep) + "Res" + JsonConvert.SerializeObject(ds));

                //1 as Id,'ok' as msg,username,u.PartnershipType,general,ExposerLimit,oldexposer,AutoConfirm
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("id") && ds.Tables[0].Rows[0]["id"].ToString() == "1")
                {
                    var parameterscas = new List<SqlParameter>();
                    parameterscas.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetlotteryrep.mid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetlotteryrep.gtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userid", Value = placebetlotteryrep.uid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetlotteryrep.ip });
                    parameterscas.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetlotteryrep.bdetail });
                    parameterscas.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                    dscasino = _sqlClientService.Execute("lottery_placeRepeteExpocheck", ConfigItems.Conn_Casinogroup, parameterscas);
                    WriteLogAll("Placebetlotteryrep1+lottery_placeRepeteExpocheck", "Req:" + JsonConvert.SerializeObject(placebetlotteryrep) + "Res" + JsonConvert.SerializeObject(dscasino));
                    if (dscasino != null && dscasino.Tables != null && dscasino.Tables.Count > 0 && dscasino.Tables[0].Rows.Count > 0 && dscasino.Tables[0].Columns.Contains("id") && dscasino.Tables[0].Rows[0]["id"].ToString() == "1")
                    {
                        var parameters3 = new List<SqlParameter>();
                        parameters3.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetlotteryrep.mid });
                        parameters3.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetlotteryrep.gtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "userid", Value = placebetlotteryrep.uid });
                        parameters3.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetlotteryrep.ip });
                        parameters3.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetlotteryrep.bdetail });
                        parameters3.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "SectionId", Value = dscasino.Tables[0].Rows[0]["SectionId"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "Amount", Value = dscasino.Tables[0].Rows[0]["Amount"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "totalbetamount", Value = dscasino.Tables[0].Rows[0]["totalbetamount"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "totalbet", Value = dscasino.Tables[0].Rows[0]["totalbet"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "randomno", Value = dscasino.Tables[0].Rows[0]["randomno"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "usercard", Value = dscasino.Tables[0].Rows[0]["usercard"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                        dscom = _sqlClientService.Execute("Group2_lottery_placebetconformreapeat", ConfigItems.Conn_Casino, parameters3);
                        WriteLogAll("Placebetlotteryrep2+Group2_lottery_placebetconformreapeat", "Req:" + JsonConvert.SerializeObject(placebetlotteryrep) + "Res" + JsonConvert.SerializeObject(dscom));
                        if (dscom != null && dscom.Tables != null && dscom.Tables.Count > 0 && dscom.Tables[0].Rows.Count > 0 && dscom.Tables[0].Columns.Contains("id") && dscom.Tables[0].Rows[0]["id"].ToString() == "1")
                        {
                            var parameters1 = new List<SqlParameter>();
                            parameters1.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetlotteryrep.mid });
                            parameters1.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetlotteryrep.gtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userid", Value = placebetlotteryrep.uid });
                            parameters1.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetlotteryrep.ip });
                            parameters1.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetlotteryrep.bdetail });
                            parameters1.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                            ds1 = _sqlClientService.Execute("lottery_placebetconformreapeat", ConfigItems.Conn_Casinogroup, parameters1);
                            WriteLogAll("Placebetlotteryrep3+lottery_placebetconformreapeat", "Req:" + JsonConvert.SerializeObject(placebetlotteryrep) + "Res" + JsonConvert.SerializeObject(ds1));

                            //select 1 as ID,'Ok' as Msg,	@userexposer as userexposer, @JsonGameSub as masterexposer
                            if (ds1 != null && ds1.Tables != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0 && ds1.Tables[0].Columns.Contains("id") && ds1.Tables[0].Rows[0]["id"].ToString() == "1")
                            {
                                //try
                                //{
                                //    var param = "marketid=" + placebetlotteryrep.mid + "&userid=" + placebetlotteryrep.uid + "&gametype=" + placebetlotteryrep.gtype + "&username=" + ds.Tables[0].Rows[0]["username"]
                                //                                    + "&nation=" + ds1.Tables[0].Rows[0]["nation"].ToString() + "&Masteramount=" + ds1.Tables[0].Rows[0]["conformamount"]
                                //                                    + "&ipaddress=" + placebetlotteryrep.ip + "&browserdetail=" + placebetlotteryrep.bdetail + "&BookvalType=" + ds1.Tables[0].Rows[0]["exposertype"] +
                                //                                    "&Bookvalmatch=" + ds1.Tables[0].Rows[0]["matchbook"] + "&BookvalFancy=" + ds1.Tables[0].Rows[0]["fancybook"];

                                //    Task.Factory.StartNew(() => Post(ConfigItems.AdminBookUrl + ApiEndpoint.lotteryrepbook, param, "application/x-www-form-urlencoded", "POST"));
                                //}
                                //catch (System.Exception ex)
                                //{
                                //    WriteLogAll("Placebetlottery+bk" + ex.ToString(), JsonConvert.SerializeObject(placebetlotteryrep));
                                //}
                                var parameters2 = new List<SqlParameter>();
                                parameters2.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetlotteryrep.uid });
                                parameters2.Add(new SqlParameter() { ParameterName = "Amount", Value = Convert.ToDouble(ds1.Tables[0].Rows[0]["userexposer"]) });
                                parameters2.Add(new SqlParameter() { ParameterName = "ExposJson", Value = ds1.Tables[0].Rows[0]["masterexposer"].ToString() });
                                parameters2.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetlotteryrep.mid });
                                parameters2.Add(new SqlParameter() { ParameterName = "gameType", Value = placebetlotteryrep.gtype });
                                ds2 = _sqlClientService.Execute("Casino_ExposerUpdate", ConfigItems.Conn_AccDgroup, parameters2);
                                WriteLogAll("Placebetlotteryrep4+Casino_ExposerUpdate", "Req:" + JsonConvert.SerializeObject(placebetlotteryrep) + "Res" + JsonConvert.SerializeObject(ds2));
                                return ds2;
                            }
                            else
                            {
                                return ds1;
                            }
                        }
                        Placebetfinal(placebetlotteryrep.uid);
                        return dscom;
                    }
                    Placebetfinal(placebetlotteryrep.uid);
                    return dscasino;
                }
                return ds;
            }
            catch (Exception ex)
            {
                WriteLogAll("Placebetlotteryrep" + ex.ToString(), JsonConvert.SerializeObject(placebetlotteryrep));
                throw ex;
            }

        }
        public DataSet Placebetlotterydtl(Placebetlotterydtl placebetlotterydtl)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                placebetlotterydtl.gtype.ToLower();
                var parameters = new List<SqlParameter>();
                //parameters.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetlotterydtl.uid });
                //parameters.Add(new SqlParameter() { ParameterName = "MarketId", Value = placebetlotterydtl.mid });
                //parameters.Add(new SqlParameter() { ParameterName = "GameType", Value = placebetlotterydtl.gtype });
                //ds = _sqlClientService.Execute("Casino_PlacebetCheck", ConfigItems.Conn_AccDgroup, parameters);
                //WriteLogAll("Placebetlotterydtl+Casino_PlacebetCheck", "Req:" + JsonConvert.SerializeObject(placebetlotterydtl) + "Res" + JsonConvert.SerializeObject(ds));
                //1 as Id,'ok' as msg,username,u.PartnershipType,general,ExposerLimit,oldexposer,AutoConfirm
                //if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("id") && ds.Tables[0].Rows[0]["id"].ToString() == "1")
                //{
                var parameters1 = new List<SqlParameter>();
                parameters1.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetlotterydtl.subtype });
                parameters1.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetlotterydtl.mid });
                parameters1.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetlotterydtl.gtype });
                parameters1.Add(new SqlParameter() { ParameterName = "userid", Value = placebetlotterydtl.uid });
                parameters1.Add(new SqlParameter() { ParameterName = "statement", Value = placebetlotterydtl.ste });
                ds1 = _sqlClientService.Execute("lottery_placebetdelete", ConfigItems.Conn_Casinogroup, parameters1);
                WriteLogAll("Placebetlotterydtl1+lottery_placebetdelete", "Req:" + JsonConvert.SerializeObject(placebetlotterydtl) + "Res" + JsonConvert.SerializeObject(ds1));
                //select 1 as ID,'Ok' as Msg,	@userexposer as userexposer, @JsonGameSub as masterexposer
                if (ds1 != null && ds1.Tables != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0 && ds1.Tables[0].Columns.Contains("id") && ds1.Tables[0].Rows[0]["id"].ToString() == "1")
                {
                    var parameters2 = new List<SqlParameter>();
                    parameters2.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetlotterydtl.uid });
                    parameters2.Add(new SqlParameter() { ParameterName = "Amount", Value = Convert.ToDouble(ds1.Tables[0].Rows[0]["userexposer"]) });
                    parameters2.Add(new SqlParameter() { ParameterName = "ExposJson", Value = ds1.Tables[0].Rows[0]["masterexposer"].ToString() });
                    parameters2.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetlotterydtl.mid });
                    parameters2.Add(new SqlParameter() { ParameterName = "gameType", Value = placebetlotterydtl.gtype });
                    ds2 = _sqlClientService.Execute("Casino_ExposerUpdate", ConfigItems.Conn_AccDgroup, parameters2);
                    WriteLogAll("Placebetlotterydtl2+Casino_ExposerUpdate", "Req:" + JsonConvert.SerializeObject(placebetlotterydtl) + "Res" + JsonConvert.SerializeObject(ds2));
                    return ds2;
                }
                else
                {
                    return ds1;
                }
                //}
                //return ds;
            }
            catch (Exception ex)
            {
                WriteLogAll("Placebetlotterydtl" + ex.ToString(), JsonConvert.SerializeObject(placebetlotterydtl));
                throw ex;
            }

        }
        public DataSet Placebettrap(Placebetteen placebetteen)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet dscom = new DataSet();
                DataSet dscasino = new DataSet();
                placebetteen.gtype.ToLower();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                parameters.Add(new SqlParameter() { ParameterName = "MarketId", Value = placebetteen.mid });
                parameters.Add(new SqlParameter() { ParameterName = "GameType", Value = placebetteen.gtype });
                ds = _sqlClientService.Execute("Casino_PlacebetCheck", ConfigItems.Conn_AccDgroup, parameters);
                WriteLogAll("Placebettrap+Casino_PlacebetCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds));

                //1 as Id,'ok' as msg,username,u.PartnershipType,general,ExposerLimit,oldexposer,AutoConfirm
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("id") && ds.Tables[0].Rows[0]["id"].ToString() == "1")
                {
                    var parameterscas = new List<SqlParameter>();
                    parameterscas.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                    parameterscas.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                    parameterscas.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                    parameterscas.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                    parameterscas.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                    dscasino = _sqlClientService.Execute("other1_placeExpoCheck", ConfigItems.Conn_Casinogroup, parameterscas);
                    WriteLogAll("Placebettrap1+other1_placeExpoCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscasino));
                    if (dscasino != null && dscasino.Tables != null && dscasino.Tables.Count > 0 && dscasino.Tables[0].Rows.Count > 0 && dscasino.Tables[0].Columns.Contains("id") && dscasino.Tables[0].Rows[0]["id"].ToString() == "1")
                    {
                        var parameters3 = new List<SqlParameter>();
                        parameters3.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                        parameters3.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                        parameters3.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                        parameters3.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                        parameters3.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                        parameters3.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                        parameters3.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                        dscom = _sqlClientService.Execute("group2_other1_placebetconform", ConfigItems.Conn_Casino, parameters3);
                        WriteLogAll("Placebettrap2+group2_other1_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscom));
                        if (dscom != null && dscom.Tables != null && dscom.Tables.Count > 0 && dscom.Tables[0].Rows.Count > 0 && dscom.Tables[0].Columns.Contains("id") && dscom.Tables[0].Rows[0]["id"].ToString() == "1")
                        {
                            var parameters1 = new List<SqlParameter>();
                            parameters1.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                            parameters1.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                            parameters1.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                            parameters1.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                            parameters1.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                            parameters1.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                            parameters1.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                            parameters1.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                            parameters1.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newUserrate", Value = dscom.Tables[0].Rows[0]["NewUserrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newAmount", Value = dscom.Tables[0].Rows[0]["Amount"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Gameid", Value = dscom.Tables[0].Rows[0]["Gameid"] });
                            ds1 = _sqlClientService.Execute("other1_placebetconform", ConfigItems.Conn_Casinogroup, parameters1);
                            WriteLogAll("Placebettrap3+other1_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds1));

                            //select 1 as ID,'Ok' as Msg,	@userexposer as userexposer, @JsonGameSub as masterexposer
                            if (ds1 != null && ds1.Tables != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0 && ds1.Tables[0].Columns.Contains("id") && ds1.Tables[0].Rows[0]["id"].ToString() == "1")
                            {
                                try
                                {
                                    var param = "marketid=" + placebetteen.mid + "&userid=" + placebetteen.uid + "&subsectionid=" + placebetteen.subid + "&gametype=" + placebetteen.gtype + "&subgametype="
                                                                     + placebetteen.subtype + "&username=" + ds.Tables[0].Rows[0]["username"] + "&sectionid=" + placebetteen.sid + "&bettype=" + placebetteen.btype
                                                                    + "&nation=" + ds1.Tables[0].Rows[0]["nation"].ToString() + "&userrate=" + placebetteen.urate + "&Masteramount=" + ds1.Tables[0].Rows[0]["conformamount"]
                                                                    + "&ipaddress=" + placebetteen.ip + "&browserdetail=" + placebetteen.bdetail + "&BookvalType=" + ds1.Tables[0].Rows[0]["exposertype"] +
                                                                    "&Bookvalmatch=" + ds1.Tables[0].Rows[0]["matchbook"] + "&BookvalFancy=" + ds1.Tables[0].Rows[0]["fancybook"] + "&amount=" + placebetteen.amt + "&frate=" + ds.Tables[0].Rows[0]["balrate"] + "&Currency=" + ds.Tables[0].Rows[0]["ccode"];

                                    Task.Factory.StartNew(() => Post(ConfigItems.AdminBookUrl + ApiEndpoint.trapbook, param, "application/x-www-form-urlencoded", "POST"));
                                }
                                catch (System.Exception ex)
                                {
                                    WriteLogAll("Placebettrap+bk" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                var parameters2 = new List<SqlParameter>();
                                parameters2.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                                parameters2.Add(new SqlParameter() { ParameterName = "Amount", Value = Convert.ToDouble(ds1.Tables[0].Rows[0]["userexposer"]) });
                                parameters2.Add(new SqlParameter() { ParameterName = "ExposJson", Value = ds1.Tables[0].Rows[0]["masterexposer"].ToString() });
                                parameters2.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                                parameters2.Add(new SqlParameter() { ParameterName = "gameType", Value = placebetteen.gtype });
                                parameters2.Add(new SqlParameter() { ParameterName = "Userrate", Value = ds1.Tables[0].Rows[0]["newuserrate"] });
                                parameters2.Add(new SqlParameter() { ParameterName = "BetAmount", Value = ds1.Tables[0].Rows[0]["conformamount"] });
                                ds2 = _sqlClientService.Execute("Casino_ExposerUpdate", ConfigItems.Conn_AccDgroup, parameters2);
                                WriteLogAll("Placebettrap4+Casino_ExposerUpdate", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds2));
                                try
                                {
                                    if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 0 && ds2.Tables[1].Rows.Count > 0 && ds2.Tables[1].Columns.Contains("SendCoupon") && ds2.Tables[1].Rows[0]["SendCoupon"].ToString() == "1")
                                    {//Select 0 as SendCoupon,0 as BetAmount,'' as couponcode
                                        CouponPlacebet couponPlacebet = new CouponPlacebet();
                                        couponPlacebet.Amount = Convert.ToDouble(ds1.Tables[0].Rows[0]["conformamount"]);
                                        couponPlacebet.PlayerName = ds.Tables[0].Rows[0]["username"].ToString();
                                        couponPlacebet.PlayerId = placebetteen.pid;
                                        couponPlacebet.Product = ds2.Tables[1].Rows[0]["couponcode"].ToString();
                                        couponPlacebet.Balance = Convert.ToDouble(ds.Tables[0].Rows[0]["general"]);
                                        if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 1 && ds2.Tables[2].Rows.Count > 0 && ds2.Tables[2].Columns.Contains("clientid") && ds2.Tables[2].Columns.Contains("secretkey"))
                                        {
                                            var par = ConfigItems.Encrypt ? Encryptionbonus.EncryptString(JsonConvert.SerializeObject(couponPlacebet), ds2.Tables[2].Rows[0]["secretkey"].ToString()) : JsonConvert.SerializeObject(couponPlacebet);
                                            var resp = Postbonus(ConfigItems.CopUrl + "transaction/event", par, "application/json", "POST", ds2.Tables[2].Rows[0]["clientid"].ToString());
                                            var res = ConfigItems.Encrypt ? Encryptionbonus.DecryptString(resp, ds2.Tables[2].Rows[0]["secretkey"].ToString()) : resp;
                                            WriteLogAll("Placebettrap3", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(res));
                                        }
                                        //var resp = Post(ConfigItems.CopUrl + ApiEndpoint.placebet, JsonConvert.SerializeObject(couponPlacebet), "application/json", "POST");
                                        //var data = JsonConvert.DeserializeObject<CouponPlacebetRes>(resp);

                                        //Returnds("0", data.error);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteLogAll("Placebettrap+cop" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                Placebetfinal(placebetteen.uid);
                                return ds2;
                            }
                            else
                            {
                                Placebetfinal(placebetteen.uid);
                                return ds1;
                            }
                        }
                        Placebetfinal(placebetteen.uid);
                        return dscom;
                    }
                    Placebetfinal(placebetteen.uid);
                    return dscasino;
                }
                //Placebetfinal(placebetteen.uid);
                return ds;
            }
            catch (Exception ex)
            {
                Placebetfinal(placebetteen.uid);
                WriteLogAll("Placebettrap" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                throw ex;
            }

        }
        public DataSet Placebetpatti2(Placebetpatti2 placebetteen)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet dscom = new DataSet();
                DataSet dscasino = new DataSet();
                placebetteen.gtype.ToLower();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                parameters.Add(new SqlParameter() { ParameterName = "MarketId", Value = placebetteen.mid });
                parameters.Add(new SqlParameter() { ParameterName = "GameType", Value = placebetteen.gtype });
                ds = _sqlClientService.Execute("Casino_PlacebetCheck", ConfigItems.Conn_AccDgroup, parameters);
                WriteLogAll("Placebetpatti2+Casino_PlacebetCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds));

                //1 as Id,'ok' as msg,username,u.PartnershipType,general,ExposerLimit,oldexposer,AutoConfirm
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("id") && ds.Tables[0].Rows[0]["id"].ToString() == "1")
                {
                    var parameterscas = new List<SqlParameter>();
                    parameterscas.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                    parameterscas.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                    parameterscas.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                    parameterscas.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                    parameterscas.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "bhav", Value = placebetteen.bhav });
                    dscasino = _sqlClientService.Execute("other1_placeExpoCheck", ConfigItems.Conn_Casinogroup, parameterscas);
                    WriteLogAll("Placebetpatti21+other1_placeExpoCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscasino));
                    if (dscasino != null && dscasino.Tables != null && dscasino.Tables.Count > 0 && dscasino.Tables[0].Rows.Count > 0 && dscasino.Tables[0].Columns.Contains("id") && dscasino.Tables[0].Rows[0]["id"].ToString() == "1")
                    {
                        var parameters3 = new List<SqlParameter>();
                        parameters3.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                        parameters3.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                        parameters3.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                        parameters3.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                        parameters3.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                        parameters3.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                        parameters3.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "bhav", Value = placebetteen.bhav });
                        dscom = _sqlClientService.Execute("group2_other1_placebetconform", ConfigItems.Conn_Casino, parameters3);
                        WriteLogAll("Placebetpatti22+group2_other1_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscom));
                        if (dscom != null && dscom.Tables != null && dscom.Tables.Count > 0 && dscom.Tables[0].Rows.Count > 0 && dscom.Tables[0].Columns.Contains("id") && dscom.Tables[0].Rows[0]["id"].ToString() == "1")
                        {
                            var parameters1 = new List<SqlParameter>();
                            parameters1.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                            parameters1.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                            parameters1.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                            parameters1.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                            parameters1.Add(new SqlParameter() { ParameterName = "bhav", Value = placebetteen.bhav });
                            parameters1.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                            parameters1.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                            parameters1.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                            parameters1.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                            parameters1.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newUserrate", Value = dscom.Tables[0].Rows[0]["NewUserrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newAmount", Value = dscom.Tables[0].Rows[0]["Amount"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Gameid", Value = dscom.Tables[0].Rows[0]["Gameid"] });
                            ds1 = _sqlClientService.Execute("other1_placebetconform", ConfigItems.Conn_Casinogroup, parameters1);
                            WriteLogAll("Placebetpatti23+other1_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds1));

                            //select 1 as ID,'Ok' as Msg,	@userexposer as userexposer, @JsonGameSub as masterexposer
                            if (ds1 != null && ds1.Tables != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0 && ds1.Tables[0].Columns.Contains("id") && ds1.Tables[0].Rows[0]["id"].ToString() == "1")
                            {
                                try
                                {
                                    var param = "marketid=" + placebetteen.mid + "&userid=" + placebetteen.uid + "&subsectionid=" + placebetteen.subid + "&gametype=" + placebetteen.gtype + "&subgametype="
                                                                     + placebetteen.subtype + "&username=" + ds.Tables[0].Rows[0]["username"] + "&sectionid=" + placebetteen.sid + "&bettype=" + placebetteen.btype
                                                                    + "&nation=" + ds1.Tables[0].Rows[0]["nation"].ToString() + "&userrate=" + placebetteen.urate + "&Masteramount=" + ds1.Tables[0].Rows[0]["conformamount"]
                                                                    + "&ipaddress=" + placebetteen.ip + "&browserdetail=" + placebetteen.bdetail + "&BookvalType=" + ds1.Tables[0].Rows[0]["exposertype"] +
                                                                    "&Bookvalmatch=" + ds1.Tables[0].Rows[0]["matchbook"] + "&BookvalFancy=" + ds1.Tables[0].Rows[0]["fancybook"] + "&amount=" + placebetteen.amt + "&frate=" + ds.Tables[0].Rows[0]["balrate"] + "&Currency=" + ds.Tables[0].Rows[0]["ccode"];

                                    Task.Factory.StartNew(() => Post(ConfigItems.AdminBookUrl + ApiEndpoint.trapbook, param, "application/x-www-form-urlencoded", "POST"));
                                }
                                catch (System.Exception ex)
                                {
                                    WriteLogAll("Placebetpatti2+bk" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                var parameters2 = new List<SqlParameter>();
                                parameters2.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                                parameters2.Add(new SqlParameter() { ParameterName = "Amount", Value = Convert.ToDouble(ds1.Tables[0].Rows[0]["userexposer"]) });
                                parameters2.Add(new SqlParameter() { ParameterName = "ExposJson", Value = ds1.Tables[0].Rows[0]["masterexposer"].ToString() });
                                parameters2.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                                parameters2.Add(new SqlParameter() { ParameterName = "gameType", Value = placebetteen.gtype });
                                parameters2.Add(new SqlParameter() { ParameterName = "Userrate", Value = ds1.Tables[0].Rows[0]["newuserrate"] });
                                parameters2.Add(new SqlParameter() { ParameterName = "BetAmount", Value = ds1.Tables[0].Rows[0]["conformamount"] });
                                ds2 = _sqlClientService.Execute("Casino_ExposerUpdate", ConfigItems.Conn_AccDgroup, parameters2);
                                WriteLogAll("Placebetpatti24+Casino_ExposerUpdate", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds2));
                                try
                                {
                                    if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 0 && ds2.Tables[1].Rows.Count > 0 && ds2.Tables[1].Columns.Contains("SendCoupon") && ds2.Tables[1].Rows[0]["SendCoupon"].ToString() == "1")
                                    {//Select 0 as SendCoupon,0 as BetAmount,'' as couponcode
                                        CouponPlacebet couponPlacebet = new CouponPlacebet();
                                        couponPlacebet.Amount = Convert.ToDouble(ds1.Tables[0].Rows[0]["conformamount"]);
                                        couponPlacebet.PlayerName = ds.Tables[0].Rows[0]["username"].ToString();
                                        couponPlacebet.PlayerId = placebetteen.pid;
                                        couponPlacebet.Product = ds2.Tables[1].Rows[0]["couponcode"].ToString();
                                        couponPlacebet.Balance = Convert.ToDouble(ds.Tables[0].Rows[0]["general"]);
                                        if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 1 && ds2.Tables[2].Rows.Count > 0 && ds2.Tables[2].Columns.Contains("clientid") && ds2.Tables[2].Columns.Contains("secretkey"))
                                        {
                                            var par = ConfigItems.Encrypt ? Encryptionbonus.EncryptString(JsonConvert.SerializeObject(couponPlacebet), ds2.Tables[2].Rows[0]["secretkey"].ToString()) : JsonConvert.SerializeObject(couponPlacebet);
                                            var resp = Postbonus(ConfigItems.CopUrl + "transaction/event", par, "application/json", "POST", ds2.Tables[2].Rows[0]["clientid"].ToString());
                                            var res = ConfigItems.Encrypt ? Encryptionbonus.DecryptString(resp, ds2.Tables[2].Rows[0]["secretkey"].ToString()) : resp;
                                            WriteLogAll("Placebetpatti23", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(res));
                                        }
                                        //var resp = Post(ConfigItems.CopUrl + ApiEndpoint.placebet, JsonConvert.SerializeObject(couponPlacebet), "application/json", "POST");
                                        //var data = JsonConvert.DeserializeObject<CouponPlacebetRes>(resp);

                                        //Returnds("0", data.error);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteLogAll("Placebetpatti2+cop" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                Placebetfinal(placebetteen.uid);
                                return ds2;
                            }
                            else
                            {
                                Placebetfinal(placebetteen.uid);
                                return ds1;
                            }
                        }
                        Placebetfinal(placebetteen.uid);
                        return dscom;
                    }
                    Placebetfinal(placebetteen.uid);
                    return dscasino;
                }
                //Placebetfinal(placebetteen.uid);
                return ds;
            }
            catch (Exception ex)
            {
                Placebetfinal(placebetteen.uid);
                WriteLogAll("Placebetpatti2" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                throw ex;
            }

        }
        public DataSet Placebetnotenum(Placebetpatti2 placebetteen)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet dscom = new DataSet();
                DataSet dscasino = new DataSet();
                placebetteen.gtype.ToLower();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                parameters.Add(new SqlParameter() { ParameterName = "MarketId", Value = placebetteen.mid });
                parameters.Add(new SqlParameter() { ParameterName = "GameType", Value = placebetteen.gtype });
                ds = _sqlClientService.Execute("Casino_PlacebetCheck", ConfigItems.Conn_AccDgroup, parameters);
                WriteLogAll("Placebetnotenum+Casino_PlacebetCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds));

                //1 as Id,'ok' as msg,username,u.PartnershipType,general,ExposerLimit,oldexposer,AutoConfirm
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("id") && ds.Tables[0].Rows[0]["id"].ToString() == "1")
                {
                    var parameterscas = new List<SqlParameter>();
                    parameterscas.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                    parameterscas.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                    parameterscas.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                    parameterscas.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                    parameterscas.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                    dscasino = _sqlClientService.Execute("other2_placeExpoCheck", ConfigItems.Conn_Casinogroup, parameterscas);
                    WriteLogAll("Placebetnotenum1+other2_placeExpoCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscasino));
                    if (dscasino != null && dscasino.Tables != null && dscasino.Tables.Count > 0 && dscasino.Tables[0].Rows.Count > 0 && dscasino.Tables[0].Columns.Contains("id") && dscasino.Tables[0].Rows[0]["id"].ToString() == "1")
                    {
                        var parameters3 = new List<SqlParameter>();
                        parameters3.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                        parameters3.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                        parameters3.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                        parameters3.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                        parameters3.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                        parameters3.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                        parameters3.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                        dscom = _sqlClientService.Execute("group2_other2_placebetconform", ConfigItems.Conn_Casino, parameters3);
                        WriteLogAll("Placebetnotenum2+group2_other2_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscom));
                        if (dscom != null && dscom.Tables != null && dscom.Tables.Count > 0 && dscom.Tables[0].Rows.Count > 0 && dscom.Tables[0].Columns.Contains("id") && dscom.Tables[0].Rows[0]["id"].ToString() == "1")
                        {
                            var parameters1 = new List<SqlParameter>();
                            parameters1.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                            parameters1.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                            parameters1.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                            parameters1.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                            parameters1.Add(new SqlParameter() { ParameterName = "bhav", Value = placebetteen.bhav });
                            parameters1.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                            parameters1.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                            parameters1.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                            parameters1.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                            parameters1.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newUserrate", Value = dscom.Tables[0].Rows[0]["NewUserrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newAmount", Value = dscom.Tables[0].Rows[0]["Amount"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Gameid", Value = dscom.Tables[0].Rows[0]["Gameid"] });
                            ds1 = _sqlClientService.Execute("other2_placebetconform", ConfigItems.Conn_Casinogroup, parameters1);
                            WriteLogAll("Placebetnotenum3+other2_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds1));

                            //select 1 as ID,'Ok' as Msg,	@userexposer as userexposer, @JsonGameSub as masterexposer
                            if (ds1 != null && ds1.Tables != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0 && ds1.Tables[0].Columns.Contains("id") && ds1.Tables[0].Rows[0]["id"].ToString() == "1")
                            {
                                try
                                {
                                    var param = "marketid=" + placebetteen.mid + "&userid=" + placebetteen.uid + "&subsectionid=" + placebetteen.subid + "&gametype=" + placebetteen.gtype + "&subgametype="
                                                                     + placebetteen.subtype + "&username=" + ds.Tables[0].Rows[0]["username"] + "&sectionid=" + placebetteen.sid + "&bettype=" + placebetteen.btype
                                                                    + "&nation=" + ds1.Tables[0].Rows[0]["nation"].ToString() + "&userrate=" + placebetteen.urate + "&Masteramount=" + ds1.Tables[0].Rows[0]["conformamount"]
                                                                    + "&ipaddress=" + placebetteen.ip + "&browserdetail=" + placebetteen.bdetail + "&BookvalType=" + ds1.Tables[0].Rows[0]["exposertype"] +
                                                                    "&Bookvalmatch=" + ds1.Tables[0].Rows[0]["matchbook"] + "&BookvalFancy=" + ds1.Tables[0].Rows[0]["fancybook"] + "&amount=" + placebetteen.amt + "&frate=" + ds.Tables[0].Rows[0]["balrate"] + "&Currency=" + ds.Tables[0].Rows[0]["ccode"];

                                    Task.Factory.StartNew(() => Post(ConfigItems.AdminBookUrl + ApiEndpoint.notenumbook, param, "application/x-www-form-urlencoded", "POST"));
                                }
                                catch (System.Exception ex)
                                {
                                    WriteLogAll("Placebetnotenum+bk" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                var parameters2 = new List<SqlParameter>();
                                parameters2.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                                parameters2.Add(new SqlParameter() { ParameterName = "Amount", Value = Convert.ToDouble(ds1.Tables[0].Rows[0]["userexposer"]) });
                                parameters2.Add(new SqlParameter() { ParameterName = "ExposJson", Value = ds1.Tables[0].Rows[0]["masterexposer"].ToString() });
                                parameters2.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                                parameters2.Add(new SqlParameter() { ParameterName = "gameType", Value = placebetteen.gtype });
                                parameters2.Add(new SqlParameter() { ParameterName = "Userrate", Value = ds1.Tables[0].Rows[0]["newuserrate"] });
                                parameters2.Add(new SqlParameter() { ParameterName = "BetAmount", Value = ds1.Tables[0].Rows[0]["conformamount"] });
                                ds2 = _sqlClientService.Execute("Casino_ExposerUpdate", ConfigItems.Conn_AccDgroup, parameters2);
                                WriteLogAll("Placebetnotenum4+Casino_ExposerUpdate", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds2));
                                try
                                {
                                    if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 0 && ds2.Tables[1].Rows.Count > 0 && ds2.Tables[1].Columns.Contains("SendCoupon") && ds2.Tables[1].Rows[0]["SendCoupon"].ToString() == "1")
                                    {//Select 0 as SendCoupon,0 as BetAmount,'' as couponcode
                                        CouponPlacebet couponPlacebet = new CouponPlacebet();
                                        couponPlacebet.Amount = Convert.ToDouble(ds1.Tables[0].Rows[0]["conformamount"]);
                                        couponPlacebet.PlayerName = ds.Tables[0].Rows[0]["username"].ToString();
                                        couponPlacebet.PlayerId = placebetteen.pid;
                                        couponPlacebet.Product = ds2.Tables[1].Rows[0]["couponcode"].ToString();
                                        couponPlacebet.Balance = Convert.ToDouble(ds.Tables[0].Rows[0]["general"]);
                                        if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 1 && ds2.Tables[2].Rows.Count > 0 && ds2.Tables[2].Columns.Contains("clientid") && ds2.Tables[2].Columns.Contains("secretkey"))
                                        {
                                            var par = ConfigItems.Encrypt ? Encryptionbonus.EncryptString(JsonConvert.SerializeObject(couponPlacebet), ds2.Tables[2].Rows[0]["secretkey"].ToString()) : JsonConvert.SerializeObject(couponPlacebet);
                                            var resp = Postbonus(ConfigItems.CopUrl + "transaction/event", par, "application/json", "POST", ds2.Tables[2].Rows[0]["clientid"].ToString());
                                            var res = ConfigItems.Encrypt ? Encryptionbonus.DecryptString(resp, ds2.Tables[2].Rows[0]["secretkey"].ToString()) : resp;
                                            WriteLogAll("Placebetnotenum3", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(res));
                                        }
                                        //var resp = Post(ConfigItems.CopUrl + ApiEndpoint.placebet, JsonConvert.SerializeObject(couponPlacebet), "application/json", "POST");
                                        //var data = JsonConvert.DeserializeObject<CouponPlacebetRes>(resp);

                                        //Returnds("0", data.error);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteLogAll("Placebetnotenum+cop" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                Placebetfinal(placebetteen.uid);
                                return ds2;
                            }
                            else
                            {
                                Placebetfinal(placebetteen.uid);
                                return ds1;
                            }
                        }
                        Placebetfinal(placebetteen.uid);
                        return dscom;
                    }
                    Placebetfinal(placebetteen.uid);
                    return dscasino;
                }
                //Placebetfinal(placebetteen.uid);
                return ds;
            }
            catch (Exception ex)
            {
                Placebetfinal(placebetteen.uid);
                WriteLogAll("Placebetnotenum" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                throw ex;
            }
        }
        public DataSet Placebetkbc(Placebetkbc placebetteen)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet dscom = new DataSet();
                DataSet dscasino = new DataSet();
                placebetteen.gtype.ToLower();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                parameters.Add(new SqlParameter() { ParameterName = "MarketId", Value = placebetteen.mid });
                parameters.Add(new SqlParameter() { ParameterName = "GameType", Value = placebetteen.gtype });
                ds = _sqlClientService.Execute("Casino_PlacebetCheck", ConfigItems.Conn_AccDgroup, parameters);
                WriteLogAll("Placebetkbc+Casino_PlacebetCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds));

                //1 as Id,'ok' as msg,username,u.PartnershipType,general,ExposerLimit,oldexposer,AutoConfirm
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("id") && ds.Tables[0].Rows[0]["id"].ToString() == "1")
                {
                    var parameterscas = new List<SqlParameter>();
                    parameterscas.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                    parameterscas.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                    parameterscas.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                    parameterscas.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                    dscasino = _sqlClientService.Execute("otherkbc_placeExpoCheck", ConfigItems.Conn_Casinogroup, parameterscas);
                    WriteLogAll("Placebetkbc1+otherkbc_placeExpoCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscasino));
                    if (dscasino != null && dscasino.Tables != null && dscasino.Tables.Count > 0 && dscasino.Tables[0].Rows.Count > 0 && dscasino.Tables[0].Columns.Contains("id") && dscasino.Tables[0].Rows[0]["id"].ToString() == "1")
                    {
                        var parameters3 = new List<SqlParameter>();
                        parameters3.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                        parameters3.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                        parameters3.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "betjson", Value = placebetteen.bjson });
                        dscom = _sqlClientService.Execute("group2_otherkbc_placebetconform", ConfigItems.Conn_Casino, parameters3);
                        WriteLogAll("Placebetkbc2+group2_otherkbc_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscom));
                        if (dscom != null && dscom.Tables != null && dscom.Tables.Count > 0 && dscom.Tables[0].Rows.Count > 0 && dscom.Tables[0].Columns.Contains("id") && dscom.Tables[0].Rows[0]["id"].ToString() == "1")
                        {
                            var parameters1 = new List<SqlParameter>();
                            parameters1.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                            parameters1.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                            parameters1.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                            parameters1.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                            parameters1.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                            parameters1.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "betjson", Value = placebetteen.bjson });
                            parameters1.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.bt });
                            parameters1.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newUserrate", Value = dscom.Tables[0].Rows[0]["NewUserrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newAmount", Value = dscom.Tables[0].Rows[0]["Amount"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Gameid", Value = dscom.Tables[0].Rows[0]["Gameid"] });
                            ds1 = _sqlClientService.Execute("otherkbc_placebetconform", ConfigItems.Conn_Casinogroup, parameters1);
                            WriteLogAll("Placebetkbc3+otherkbc_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds1));

                            //select 1 as ID,'Ok' as Msg,	@userexposer as userexposer, @JsonGameSub as masterexposer
                            if (ds1 != null && ds1.Tables != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0 && ds1.Tables[0].Columns.Contains("id") && ds1.Tables[0].Rows[0]["id"].ToString() == "1")
                            {
                                try
                                {
                                    var param = "marketid=" + placebetteen.mid + "&userid=" + placebetteen.uid + "&gametype=" + placebetteen.gtype + "&username=" + ds.Tables[0].Rows[0]["username"]
                                                                    + "&nation=" + ds1.Tables[0].Rows[0]["nation"].ToString() + "&Masteramount=" + ds1.Tables[0].Rows[0]["conformamount"]
                                                                    + "&ipaddress=" + placebetteen.ip + "&browserdetail=" + placebetteen.bdetail + "&BookvalType=" + ds1.Tables[0].Rows[0]["exposertype"] +
                                                                    "&Bookvalmatch=" + ds1.Tables[0].Rows[0]["matchbook"] + "&BookvalFancy=" + ds1.Tables[0].Rows[0]["fancybook"] + "&amount=" + placebetteen.amt + "&nationjson=" + ds1.Tables[0].Rows[0]["nationjson"] + "&frate=" + ds.Tables[0].Rows[0]["balrate"] + "&Currency=" + ds.Tables[0].Rows[0]["ccode"];

                                    Task.Factory.StartNew(() => Post(ConfigItems.AdminBookUrl + ApiEndpoint.otherkbcbook, param, "application/x-www-form-urlencoded", "POST"));
                                }
                                catch (System.Exception ex)
                                {
                                    WriteLogAll("Placebetkbc4+bk" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                var parameters2 = new List<SqlParameter>();
                                parameters2.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                                parameters2.Add(new SqlParameter() { ParameterName = "Amount", Value = Convert.ToDouble(ds1.Tables[0].Rows[0]["userexposer"]) });
                                parameters2.Add(new SqlParameter() { ParameterName = "ExposJson", Value = ds1.Tables[0].Rows[0]["masterexposer"].ToString() });
                                parameters2.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                                parameters2.Add(new SqlParameter() { ParameterName = "gameType", Value = placebetteen.gtype });
                                parameters2.Add(new SqlParameter() { ParameterName = "Userrate", Value = "2.0" });
                                parameters2.Add(new SqlParameter() { ParameterName = "BetAmount", Value = ds1.Tables[0].Rows[0]["conformamount"] });
                                ds2 = _sqlClientService.Execute("Casino_ExposerUpdate", ConfigItems.Conn_AccDgroup, parameters2);
                                WriteLogAll("Placebetkbc2+Casino_ExposerUpdate", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds2));
                                try
                                {
                                    if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 0 && ds2.Tables[1].Rows.Count > 0 && ds2.Tables[1].Columns.Contains("SendCoupon") && ds2.Tables[1].Rows[0]["SendCoupon"].ToString() == "1")
                                    {//Select 0 as SendCoupon,0 as BetAmount,'' as couponcode
                                        CouponPlacebet couponPlacebet = new CouponPlacebet();
                                        couponPlacebet.Amount = Convert.ToDouble(ds1.Tables[0].Rows[0]["conformamount"]);
                                        couponPlacebet.PlayerName = ds.Tables[0].Rows[0]["username"].ToString();
                                        couponPlacebet.PlayerId = placebetteen.pid;
                                        couponPlacebet.Product = ds2.Tables[1].Rows[0]["couponcode"].ToString();
                                        couponPlacebet.Balance = Convert.ToDouble(ds.Tables[0].Rows[0]["general"]);
                                        if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 1 && ds2.Tables[2].Rows.Count > 0 && ds2.Tables[2].Columns.Contains("clientid") && ds2.Tables[2].Columns.Contains("secretkey"))
                                        {
                                            var par = ConfigItems.Encrypt ? Encryptionbonus.EncryptString(JsonConvert.SerializeObject(couponPlacebet), ds2.Tables[2].Rows[0]["secretkey"].ToString()) : JsonConvert.SerializeObject(couponPlacebet);
                                            var resp = Postbonus(ConfigItems.CopUrl + "transaction/event", par, "application/json", "POST", ds2.Tables[2].Rows[0]["clientid"].ToString());
                                            var res = ConfigItems.Encrypt ? Encryptionbonus.DecryptString(resp, ds2.Tables[2].Rows[0]["secretkey"].ToString()) : resp;
                                            WriteLogAll("Placebetnotenum3", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(res));
                                        }
                                        //var resp = Post(ConfigItems.CopUrl + ApiEndpoint.placebet, JsonConvert.SerializeObject(couponPlacebet), "application/json", "POST");
                                        //var data = JsonConvert.DeserializeObject<CouponPlacebetRes>(resp);

                                        //Returnds("0", data.error);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteLogAll("Placebetnotenum+cop" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                Placebetfinal(placebetteen.uid);
                                return ds2;
                            }
                            else
                            {
                                Placebetfinal(placebetteen.uid);
                                return ds1;
                            }
                        }
                        Placebetfinal(placebetteen.uid);
                        return dscom;
                    }
                    Placebetfinal(placebetteen.uid);
                    return dscasino;
                }
                //Placebetfinal(placebetteen.uid);
                return ds;
            }
            catch (Exception ex)
            {
                Placebetfinal(placebetteen.uid);
                WriteLogAll("Placebetnotenum" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                throw ex;
            }
        }
        public DataSet Placebetvcard32(Placebetteen placebetteen)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet dscom = new DataSet();
                DataSet dscasino = new DataSet();
                placebetteen.gtype.ToLower();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                parameters.Add(new SqlParameter() { ParameterName = "MarketId", Value = placebetteen.mid });
                parameters.Add(new SqlParameter() { ParameterName = "GameType", Value = placebetteen.gtype });
                ds = _sqlClientService.Execute("Casino_PlacebetCheck", ConfigItems.Conn_AccDgroup, parameters);
                WriteLogAll("Placebetvcard32+Casino_PlacebetCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds));

                //1 as Id,'ok' as msg,username,u.PartnershipType,general,ExposerLimit,oldexposer,AutoConfirm
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("id") && ds.Tables[0].Rows[0]["id"].ToString() == "1")
                {
                    var parameterscas = new List<SqlParameter>();
                    parameterscas.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                    parameterscas.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                    parameterscas.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                    parameterscas.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                    parameterscas.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                    dscasino = _sqlClientService.Execute("z_card32_placeExpoCheck", ConfigItems.Conn_Casinogroup, parameterscas);
                    WriteLogAll("Placebetvcard321+z_card32_placeExpoCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscasino));
                    if (dscasino != null && dscasino.Tables != null && dscasino.Tables.Count > 0 && dscasino.Tables[0].Rows.Count > 0 && dscasino.Tables[0].Columns.Contains("id") && dscasino.Tables[0].Rows[0]["id"].ToString() == "1")
                    {
                        var parameters3 = new List<SqlParameter>();
                        parameters3.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                        parameters3.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                        parameters3.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                        parameters3.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                        parameters3.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                        parameters3.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                        parameters3.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                        dscom = _sqlClientService.Execute("group2_z_card32_placebetconform", ConfigItems.Conn_Casino, parameters3);
                        WriteLogAll("Placebetvcard322+group2_z_card32_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscom));
                        if (dscom != null && dscom.Tables != null && dscom.Tables.Count > 0 && dscom.Tables[0].Rows.Count > 0 && dscom.Tables[0].Columns.Contains("id") && dscom.Tables[0].Rows[0]["id"].ToString() == "1")
                        {
                            var parameters1 = new List<SqlParameter>();
                            parameters1.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                            parameters1.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                            parameters1.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                            parameters1.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                            parameters1.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                            parameters1.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                            parameters1.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                            parameters1.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                            parameters1.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newUserrate", Value = dscom.Tables[0].Rows[0]["NewUserrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newAmount", Value = dscom.Tables[0].Rows[0]["Amount"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Gameid", Value = dscom.Tables[0].Rows[0]["Gameid"] });
                            ds1 = _sqlClientService.Execute("z_card32_placebetconform", ConfigItems.Conn_Casinogroup, parameters1);
                            WriteLogAll("Placebetvcard323+z_card32_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds1));

                            //select 1 as ID,'Ok' as Msg,	@userexposer as userexposer, @JsonGameSub as masterexposer
                            if (ds1 != null && ds1.Tables != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0 && ds1.Tables[0].Columns.Contains("id") && ds1.Tables[0].Rows[0]["id"].ToString() == "1")
                            {
                                try
                                {
                                    var param = "marketid=" + placebetteen.mid + "&userid=" + placebetteen.uid + "&subsectionid=" + placebetteen.subid + "&gametype=" + placebetteen.gtype + "&subgametype="
                                                                     + placebetteen.subtype + "&username=" + ds.Tables[0].Rows[0]["username"] + "&sectionid=" + placebetteen.sid + "&bettype=" + placebetteen.btype
                                                                    + "&nation=" + ds1.Tables[0].Rows[0]["nation"].ToString() + "&userrate=" + placebetteen.urate + "&Masteramount=" + ds1.Tables[0].Rows[0]["conformamount"]
                                                                    + "&ipaddress=" + placebetteen.ip + "&browserdetail=" + placebetteen.bdetail + "&BookvalType=" + ds1.Tables[0].Rows[0]["exposertype"] +
                                                                    "&Bookvalmatch=" + ds1.Tables[0].Rows[0]["matchbook"] + "&BookvalFancy=" + ds1.Tables[0].Rows[0]["fancybook"] + "&amount=" + placebetteen.amt + "&frate=" + ds.Tables[0].Rows[0]["balrate"] + "&Currency=" + ds.Tables[0].Rows[0]["ccode"];

                                    Task.Factory.StartNew(() => Post(ConfigItems.AdminBookUrl + ApiEndpoint.vcard32book, param, "application/x-www-form-urlencoded", "POST"));
                                }
                                catch (System.Exception ex)
                                {
                                    WriteLogAll("Placebetvcard32+bk" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                var parameters2 = new List<SqlParameter>();
                                parameters2.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                                parameters2.Add(new SqlParameter() { ParameterName = "Amount", Value = Convert.ToDouble(ds1.Tables[0].Rows[0]["userexposer"]) });
                                parameters2.Add(new SqlParameter() { ParameterName = "ExposJson", Value = ds1.Tables[0].Rows[0]["masterexposer"].ToString() });
                                parameters2.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                                parameters2.Add(new SqlParameter() { ParameterName = "gameType", Value = placebetteen.gtype });
                                parameters2.Add(new SqlParameter() { ParameterName = "Userrate", Value = ds1.Tables[0].Rows[0]["newuserrate"] });
                                parameters2.Add(new SqlParameter() { ParameterName = "BetAmount", Value = ds1.Tables[0].Rows[0]["conformamount"] });
                                ds2 = _sqlClientService.Execute("Casino_ExposerUpdate", ConfigItems.Conn_AccDgroup, parameters2);
                                WriteLogAll("Placebetvcard324+Casino_ExposerUpdate", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds2));
                                try
                                {
                                    if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 0 && ds2.Tables[1].Rows.Count > 0 && ds2.Tables[1].Columns.Contains("SendCoupon") && ds2.Tables[1].Rows[0]["SendCoupon"].ToString() == "1")
                                    {//Select 0 as SendCoupon,0 as BetAmount,'' as couponcode
                                        CouponPlacebet couponPlacebet = new CouponPlacebet();
                                        couponPlacebet.Amount = Convert.ToDouble(ds1.Tables[0].Rows[0]["conformamount"]);
                                        couponPlacebet.PlayerName = ds.Tables[0].Rows[0]["username"].ToString();
                                        couponPlacebet.PlayerId = placebetteen.pid;
                                        couponPlacebet.Product = ds2.Tables[1].Rows[0]["couponcode"].ToString();
                                        couponPlacebet.Balance = Convert.ToDouble(ds.Tables[0].Rows[0]["general"]);
                                        if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 1 && ds2.Tables[2].Rows.Count > 0 && ds2.Tables[2].Columns.Contains("clientid") && ds2.Tables[2].Columns.Contains("secretkey"))
                                        {
                                            var par = ConfigItems.Encrypt ? Encryptionbonus.EncryptString(JsonConvert.SerializeObject(couponPlacebet), ds2.Tables[2].Rows[0]["secretkey"].ToString()) : JsonConvert.SerializeObject(couponPlacebet);
                                            var resp = Postbonus(ConfigItems.CopUrl + "transaction/event", par, "application/json", "POST", ds2.Tables[2].Rows[0]["clientid"].ToString());
                                            var res = ConfigItems.Encrypt ? Encryptionbonus.DecryptString(resp, ds2.Tables[2].Rows[0]["secretkey"].ToString()) : resp;
                                            WriteLogAll("Placebetvcard323", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(res));
                                        }
                                        //var resp = Post(ConfigItems.CopUrl + ApiEndpoint.placebet, JsonConvert.SerializeObject(couponPlacebet), "application/json", "POST");
                                        //var data = JsonConvert.DeserializeObject<CouponPlacebetRes>(resp);

                                        //Returnds("0", data.error);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteLogAll("Placebetvbc+cop" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                Placebetfinal(placebetteen.uid);
                                return ds2;
                            }
                            else
                            {
                                Placebetfinal(placebetteen.uid);
                                return ds1;
                            }
                        }
                        Placebetfinal(placebetteen.uid);
                        return dscom;
                    }
                    Placebetfinal(placebetteen.uid);
                    return dscasino;
                }
                //Placebetfinal(placebetteen.uid);
                return ds;
            }
            catch (Exception ex)
            {
                Placebetfinal(placebetteen.uid);
                WriteLogAll("Placebetvcard32" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                throw ex;
            }
        }
        public DataSet Placebetvbc(Placebetteen placebetteen)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet dscom = new DataSet();
                DataSet dscasino = new DataSet();
                placebetteen.gtype.ToLower();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                parameters.Add(new SqlParameter() { ParameterName = "MarketId", Value = placebetteen.mid });
                parameters.Add(new SqlParameter() { ParameterName = "GameType", Value = placebetteen.gtype });
                ds = _sqlClientService.Execute("Casino_PlacebetCheck", ConfigItems.Conn_AccDgroup, parameters);
                WriteLogAll("Placebetvbc+Casino_PlacebetCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds));

                //1 as Id,'ok' as msg,username,u.PartnershipType,general,ExposerLimit,oldexposer,AutoConfirm
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("id") && ds.Tables[0].Rows[0]["id"].ToString() == "1")
                {
                    var parameterscas = new List<SqlParameter>();
                    parameterscas.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                    parameterscas.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                    parameterscas.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                    parameterscas.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                    parameterscas.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                    dscasino = _sqlClientService.Execute("z_bc_placeExpoCheck", ConfigItems.Conn_Casinogroup, parameterscas);
                    WriteLogAll("Placebetvbc1+z_bc_placeExpoCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscasino));
                    if (dscasino != null && dscasino.Tables != null && dscasino.Tables.Count > 0 && dscasino.Tables[0].Rows.Count > 0 && dscasino.Tables[0].Columns.Contains("id") && dscasino.Tables[0].Rows[0]["id"].ToString() == "1")
                    {
                        var parameters3 = new List<SqlParameter>();
                        parameters3.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                        parameters3.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                        parameters3.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                        parameters3.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                        parameters3.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                        parameters3.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                        parameters3.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                        dscom = _sqlClientService.Execute("group2_z_bc_placebetconform", ConfigItems.Conn_Casino, parameters3);
                        WriteLogAll("Placebetvbc2+group2_z_bc_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscom));
                        if (dscom != null && dscom.Tables != null && dscom.Tables.Count > 0 && dscom.Tables[0].Rows.Count > 0 && dscom.Tables[0].Columns.Contains("id") && dscom.Tables[0].Rows[0]["id"].ToString() == "1")
                        {
                            var parameters1 = new List<SqlParameter>();
                            parameters1.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                            parameters1.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                            parameters1.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                            parameters1.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                            parameters1.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                            parameters1.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                            parameters1.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                            parameters1.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                            parameters1.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newUserrate", Value = dscom.Tables[0].Rows[0]["NewUserrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newAmount", Value = dscom.Tables[0].Rows[0]["Amount"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Gameid", Value = dscom.Tables[0].Rows[0]["Gameid"] });
                            ds1 = _sqlClientService.Execute("z_bc_placebetconform", ConfigItems.Conn_Casinogroup, parameters1);
                            WriteLogAll("Placebetvbc3+z_bc_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds1));

                            //select 1 as ID,'Ok' as Msg,	@userexposer as userexposer, @JsonGameSub as masterexposer
                            if (ds1 != null && ds1.Tables != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0 && ds1.Tables[0].Columns.Contains("id") && ds1.Tables[0].Rows[0]["id"].ToString() == "1")
                            {
                                try
                                {
                                    var param = "marketid=" + placebetteen.mid + "&userid=" + placebetteen.uid + "&subsectionid=" + placebetteen.subid + "&gametype=" + placebetteen.gtype + "&subgametype="
                                                                     + placebetteen.subtype + "&username=" + ds.Tables[0].Rows[0]["username"] + "&sectionid=" + placebetteen.sid + "&bettype=" + placebetteen.btype
                                                                    + "&nation=" + ds1.Tables[0].Rows[0]["nation"].ToString() + "&userrate=" + placebetteen.urate + "&Masteramount=" + ds1.Tables[0].Rows[0]["conformamount"]
                                                                    + "&ipaddress=" + placebetteen.ip + "&browserdetail=" + placebetteen.bdetail + "&BookvalType=" + ds1.Tables[0].Rows[0]["exposertype"] +
                                                                    "&Bookvalmatch=" + ds1.Tables[0].Rows[0]["matchbook"] + "&BookvalFancy=" + ds1.Tables[0].Rows[0]["fancybook"] + "&amount=" + placebetteen.amt + "&frate=" + ds.Tables[0].Rows[0]["balrate"] + "&Currency=" + ds.Tables[0].Rows[0]["ccode"];

                                    Task.Factory.StartNew(() => Post(ConfigItems.AdminBookUrl + ApiEndpoint.vbcbook, param, "application/x-www-form-urlencoded", "POST"));
                                }
                                catch (System.Exception ex)
                                {
                                    WriteLogAll("Placebetvbc+bk" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                var parameters2 = new List<SqlParameter>();
                                parameters2.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                                parameters2.Add(new SqlParameter() { ParameterName = "Amount", Value = Convert.ToDouble(ds1.Tables[0].Rows[0]["userexposer"]) });
                                parameters2.Add(new SqlParameter() { ParameterName = "ExposJson", Value = ds1.Tables[0].Rows[0]["masterexposer"].ToString() });
                                parameters2.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                                parameters2.Add(new SqlParameter() { ParameterName = "gameType", Value = placebetteen.gtype });
                                ds2 = _sqlClientService.Execute("Casino_ExposerUpdate", ConfigItems.Conn_AccDgroup, parameters2);
                                WriteLogAll("Placebetvbc4+Casino_ExposerUpdate", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds2));
                                try
                                {
                                    if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 0 && ds2.Tables[1].Rows.Count > 0 && ds2.Tables[1].Columns.Contains("SendCoupon") && ds2.Tables[1].Rows[0]["SendCoupon"].ToString() == "1")
                                    {//Select 0 as SendCoupon,0 as BetAmount,'' as couponcode
                                        CouponPlacebet couponPlacebet = new CouponPlacebet();
                                        couponPlacebet.Amount = Convert.ToDouble(ds1.Tables[0].Rows[0]["conformamount"]);
                                        couponPlacebet.PlayerName = ds.Tables[0].Rows[0]["username"].ToString();
                                        couponPlacebet.PlayerId = placebetteen.pid;
                                        couponPlacebet.Product = ds2.Tables[1].Rows[0]["couponcode"].ToString();
                                        couponPlacebet.Balance = Convert.ToDouble(ds.Tables[0].Rows[0]["general"]);
                                        if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 1 && ds2.Tables[2].Rows.Count > 0 && ds2.Tables[2].Columns.Contains("clientid") && ds2.Tables[2].Columns.Contains("secretkey"))
                                        {
                                            var par = ConfigItems.Encrypt ? Encryptionbonus.EncryptString(JsonConvert.SerializeObject(couponPlacebet), ds2.Tables[2].Rows[0]["secretkey"].ToString()) : JsonConvert.SerializeObject(couponPlacebet);
                                            var resp = Postbonus(ConfigItems.CopUrl + "transaction/event", par, "application/json", "POST", ds2.Tables[2].Rows[0]["clientid"].ToString());
                                            var res = ConfigItems.Encrypt ? Encryptionbonus.DecryptString(resp, ds2.Tables[2].Rows[0]["secretkey"].ToString()) : resp;
                                            WriteLogAll("Placebetfancy4", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(res));
                                        }
                                        //var resp = Post(ConfigItems.CopUrl + ApiEndpoint.placebet, JsonConvert.SerializeObject(couponPlacebet), "application/json", "POST");
                                        //var data = JsonConvert.DeserializeObject<CouponPlacebetRes>(resp);

                                        //Returnds("0", data.error);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteLogAll("Placebetvbc+cop" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                Placebetfinal(placebetteen.uid);
                                return ds2;
                            }
                            else
                            {
                                Placebetfinal(placebetteen.uid);
                                return ds1;
                            }
                        }
                        Placebetfinal(placebetteen.uid);
                        return dscom;
                    }
                    Placebetfinal(placebetteen.uid);
                    return dscasino;
                }
                //Placebetfinal(placebetteen.uid);
                return ds;
            }
            catch (Exception ex)
            {
                Placebetfinal(placebetteen.uid);
                WriteLogAll("Placebetvbc" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                throw ex;
            }

        }
        public DataSet Placebetvbaccarat(Placebetteen placebetteen)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet dscom = new DataSet();
                DataSet dscasino = new DataSet();
                placebetteen.gtype.ToLower();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                parameters.Add(new SqlParameter() { ParameterName = "MarketId", Value = placebetteen.mid });
                parameters.Add(new SqlParameter() { ParameterName = "GameType", Value = placebetteen.gtype });
                ds = _sqlClientService.Execute("Casino_PlacebetCheck", ConfigItems.Conn_AccDgroup, parameters);
                WriteLogAll("Placebetvbaccarat+Casino_PlacebetCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds));

                //1 as Id,'ok' as msg,username,u.PartnershipType,general,ExposerLimit,oldexposer,AutoConfirm
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("id") && ds.Tables[0].Rows[0]["id"].ToString() == "1")
                {
                    var parameterscas = new List<SqlParameter>();
                    parameterscas.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                    parameterscas.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                    parameterscas.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                    parameterscas.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                    parameterscas.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                    dscasino = _sqlClientService.Execute("Z_baccarat_placeExpoCheck", ConfigItems.Conn_Casinogroup, parameterscas);
                    WriteLogAll("Placebetvbaccarat1+Z_baccarat_placeExpoCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscasino));
                    if (dscasino != null && dscasino.Tables != null && dscasino.Tables.Count > 0 && dscasino.Tables[0].Rows.Count > 0 && dscasino.Tables[0].Columns.Contains("id") && dscasino.Tables[0].Rows[0]["id"].ToString() == "1")
                    {
                        var parameters3 = new List<SqlParameter>();
                        parameters3.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                        parameters3.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                        parameters3.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                        parameters3.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                        parameters3.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                        parameters3.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                        parameters3.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                        dscom = _sqlClientService.Execute("group2_Z_baccarat_placebetconform", ConfigItems.Conn_Casino, parameters3);
                        WriteLogAll("Placebetvbaccarat2+group2_Z_baccarat_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscom));
                        if (dscom != null && dscom.Tables != null && dscom.Tables.Count > 0 && dscom.Tables[0].Rows.Count > 0 && dscom.Tables[0].Columns.Contains("id") && dscom.Tables[0].Rows[0]["id"].ToString() == "1")
                        {
                            var parameters1 = new List<SqlParameter>();
                            parameters1.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                            parameters1.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                            parameters1.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                            parameters1.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                            parameters1.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                            parameters1.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                            parameters1.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                            parameters1.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                            parameters1.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newUserrate", Value = dscom.Tables[0].Rows[0]["NewUserrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newAmount", Value = dscom.Tables[0].Rows[0]["Amount"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Gameid", Value = dscom.Tables[0].Rows[0]["Gameid"] });
                            ds1 = _sqlClientService.Execute("Z_baccarat_placebetconform", ConfigItems.Conn_Casinogroup, parameters1);
                            WriteLogAll("Placebetvbaccarat3+baccarat_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds1));

                            //select 1 as ID,'Ok' as Msg,	@userexposer as userexposer, @JsonGameSub as masterexposer
                            if (ds1 != null && ds1.Tables != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0 && ds1.Tables[0].Columns.Contains("id") && ds1.Tables[0].Rows[0]["id"].ToString() == "1")
                            {
                                try
                                {
                                    var param = "marketid=" + placebetteen.mid + "&userid=" + placebetteen.uid + "&subsectionid=" + placebetteen.subid + "&gametype=" + placebetteen.gtype + "&subgametype="
                                                                     + placebetteen.subtype + "&username=" + ds.Tables[0].Rows[0]["username"] + "&sectionid=" + placebetteen.sid + "&bettype=" + placebetteen.btype
                                                                    + "&nation=" + ds1.Tables[0].Rows[0]["nation"].ToString() + "&userrate=" + placebetteen.urate + "&Masteramount=" + ds1.Tables[0].Rows[0]["conformamount"]
                                                                    + "&ipaddress=" + placebetteen.ip + "&browserdetail=" + placebetteen.bdetail + "&BookvalType=" + ds1.Tables[0].Rows[0]["exposertype"] +
                                                                    "&Bookvalmatch=" + ds1.Tables[0].Rows[0]["matchbook"] + "&BookvalFancy=" + ds1.Tables[0].Rows[0]["fancybook"] + "&amount=" + placebetteen.amt + "&frate=" + ds.Tables[0].Rows[0]["balrate"] + "&Currency=" + ds.Tables[0].Rows[0]["ccode"];

                                    Task.Factory.StartNew(() => Post(ConfigItems.AdminBookUrl + ApiEndpoint.vbaccaratbook, param, "application/x-www-form-urlencoded", "POST"));
                                }
                                catch (System.Exception ex)
                                {
                                    WriteLogAll("Placebetvbaccarat+bk" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                var parameters2 = new List<SqlParameter>();
                                parameters2.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                                parameters2.Add(new SqlParameter() { ParameterName = "Amount", Value = Convert.ToDouble(ds1.Tables[0].Rows[0]["userexposer"]) });
                                parameters2.Add(new SqlParameter() { ParameterName = "ExposJson", Value = ds1.Tables[0].Rows[0]["masterexposer"].ToString() });
                                parameters2.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                                parameters2.Add(new SqlParameter() { ParameterName = "gameType", Value = placebetteen.gtype });
                                parameters2.Add(new SqlParameter() { ParameterName = "Userrate", Value = ds1.Tables[0].Rows[0]["newuserrate"] });
                                parameters2.Add(new SqlParameter() { ParameterName = "BetAmount", Value = ds1.Tables[0].Rows[0]["conformamount"] });
                                ds2 = _sqlClientService.Execute("Casino_ExposerUpdate", ConfigItems.Conn_AccDgroup, parameters2);
                                WriteLogAll("Placebetvbaccarat4+Casino_ExposerUpdate", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds2));
                                try
                                {
                                    if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 0 && ds2.Tables[1].Rows.Count > 0 && ds2.Tables[1].Columns.Contains("SendCoupon") && ds2.Tables[1].Rows[0]["SendCoupon"].ToString() == "1")
                                    {//Select 0 as SendCoupon,0 as BetAmount,'' as couponcode
                                        CouponPlacebet couponPlacebet = new CouponPlacebet();
                                        couponPlacebet.Amount = Convert.ToDouble(ds1.Tables[0].Rows[0]["conformamount"]);
                                        couponPlacebet.PlayerName = ds.Tables[0].Rows[0]["username"].ToString();
                                        couponPlacebet.PlayerId = placebetteen.pid;
                                        couponPlacebet.Product = ds2.Tables[1].Rows[0]["couponcode"].ToString();
                                        couponPlacebet.Balance = Convert.ToDouble(ds.Tables[0].Rows[0]["general"]);
                                        if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 1 && ds2.Tables[2].Rows.Count > 0 && ds2.Tables[2].Columns.Contains("clientid") && ds2.Tables[2].Columns.Contains("secretkey"))
                                        {
                                            var par = ConfigItems.Encrypt ? Encryptionbonus.EncryptString(JsonConvert.SerializeObject(couponPlacebet), ds2.Tables[2].Rows[0]["secretkey"].ToString()) : JsonConvert.SerializeObject(couponPlacebet);
                                            var resp = Postbonus(ConfigItems.CopUrl + "transaction/event", par, "application/json", "POST", ds2.Tables[2].Rows[0]["clientid"].ToString());
                                            var res = ConfigItems.Encrypt ? Encryptionbonus.DecryptString(resp, ds2.Tables[2].Rows[0]["secretkey"].ToString()) : resp;
                                            WriteLogAll("Placebetvbaccarat3", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(res));
                                        }
                                        //var resp = Post(ConfigItems.CopUrl + ApiEndpoint.placebet, JsonConvert.SerializeObject(couponPlacebet), "application/json", "POST");
                                        //var data = JsonConvert.DeserializeObject<CouponPlacebetRes>(resp);

                                        //Returnds("0", data.error);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteLogAll("Placebetvbaccarat+cop" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                Placebetfinal(placebetteen.uid);
                                return ds2;
                            }
                            else
                            {
                                Placebetfinal(placebetteen.uid);
                                return ds1;
                            }
                        }
                        Placebetfinal(placebetteen.uid);
                        return dscom;
                    }
                    Placebetfinal(placebetteen.uid);
                    return dscasino;
                }
                //Placebetfinal(placebetteen.uid);
                return ds;
            }
            catch (Exception ex)
            {
                Placebetfinal(placebetteen.uid);
                WriteLogAll("Placebetvbaccarat" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                throw ex;
            }

        }
        public DataSet Placebetvqueen(Placebetteen placebetteen)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet dscom = new DataSet();
                DataSet dscasino = new DataSet();
                placebetteen.gtype.ToLower();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                parameters.Add(new SqlParameter() { ParameterName = "MarketId", Value = placebetteen.mid });
                parameters.Add(new SqlParameter() { ParameterName = "GameType", Value = placebetteen.gtype });
                ds = _sqlClientService.Execute("Casino_PlacebetCheck", ConfigItems.Conn_AccDgroup, parameters);
                WriteLogAll("Placebetvqueen+Casino_PlacebetCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds));

                //1 as Id,'ok' as msg,username,u.PartnershipType,general,ExposerLimit,oldexposer,AutoConfirm
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("id") && ds.Tables[0].Rows[0]["id"].ToString() == "1")
                {
                    var parameterscas = new List<SqlParameter>();
                    parameterscas.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                    parameterscas.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                    parameterscas.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                    parameterscas.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                    parameterscas.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                    dscasino = _sqlClientService.Execute("Z_queen_placeExpoCheck", ConfigItems.Conn_Casinogroup, parameterscas);
                    WriteLogAll("Placebetvqueen1+Z_queen_placeExpoCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscasino));
                    if (dscasino != null && dscasino.Tables != null && dscasino.Tables.Count > 0 && dscasino.Tables[0].Rows.Count > 0 && dscasino.Tables[0].Columns.Contains("id") && dscasino.Tables[0].Rows[0]["id"].ToString() == "1")
                    {
                        var parameters3 = new List<SqlParameter>();
                        parameters3.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                        parameters3.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                        parameters3.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                        parameters3.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                        parameters3.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                        parameters3.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                        parameters3.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                        dscom = _sqlClientService.Execute("group2_Z_queen_placebetconform", ConfigItems.Conn_Casino, parameters3);
                        WriteLogAll("Placebetvqueen2+group2_Z_queen_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscom));
                        if (dscom != null && dscom.Tables != null && dscom.Tables.Count > 0 && dscom.Tables[0].Rows.Count > 0 && dscom.Tables[0].Columns.Contains("id") && dscom.Tables[0].Rows[0]["id"].ToString() == "1")
                        {
                            var parameters1 = new List<SqlParameter>();
                            parameters1.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                            parameters1.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                            parameters1.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                            parameters1.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                            parameters1.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                            parameters1.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                            parameters1.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                            parameters1.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                            parameters1.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newUserrate", Value = dscom.Tables[0].Rows[0]["NewUserrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newAmount", Value = dscom.Tables[0].Rows[0]["Amount"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Gameid", Value = dscom.Tables[0].Rows[0]["Gameid"] });
                            ds1 = _sqlClientService.Execute("Z_queen_placebetconform", ConfigItems.Conn_Casinogroup, parameters1);
                            WriteLogAll("Placebetvqueen3+Z_queen_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds1));

                            //select 1 as ID,'Ok' as Msg,	@userexposer as userexposer, @JsonGameSub as masterexposer
                            if (ds1 != null && ds1.Tables != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0 && ds1.Tables[0].Columns.Contains("id") && ds1.Tables[0].Rows[0]["id"].ToString() == "1")
                            {
                                try
                                {
                                    var param = "marketid=" + placebetteen.mid + "&userid=" + placebetteen.uid + "&subsectionid=" + placebetteen.subid + "&gametype=" + placebetteen.gtype + "&subgametype="
                                                                     + placebetteen.subtype + "&username=" + ds.Tables[0].Rows[0]["username"] + "&sectionid=" + placebetteen.sid + "&bettype=" + placebetteen.btype
                                                                    + "&nation=" + ds1.Tables[0].Rows[0]["nation"].ToString() + "&userrate=" + placebetteen.urate + "&Masteramount=" + ds1.Tables[0].Rows[0]["conformamount"]
                                                                    + "&ipaddress=" + placebetteen.ip + "&browserdetail=" + placebetteen.bdetail + "&BookvalType=" + ds1.Tables[0].Rows[0]["exposertype"] +
                                                                    "&Bookvalmatch=" + ds1.Tables[0].Rows[0]["matchbook"] + "&BookvalFancy=" + ds1.Tables[0].Rows[0]["fancybook"] + "&amount=" + placebetteen.amt + "&frate=" + ds.Tables[0].Rows[0]["balrate"] + "&Currency=" + ds.Tables[0].Rows[0]["ccode"];

                                    Task.Factory.StartNew(() => Post(ConfigItems.AdminBookUrl + ApiEndpoint.vqueenbook, param, "application/x-www-form-urlencoded", "POST"));
                                }
                                catch (System.Exception ex)
                                {
                                    WriteLogAll("Placebetvqueen+bk" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                var parameters2 = new List<SqlParameter>();
                                parameters2.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                                parameters2.Add(new SqlParameter() { ParameterName = "Amount", Value = Convert.ToDouble(ds1.Tables[0].Rows[0]["userexposer"]) });
                                parameters2.Add(new SqlParameter() { ParameterName = "ExposJson", Value = ds1.Tables[0].Rows[0]["masterexposer"].ToString() });
                                parameters2.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                                parameters2.Add(new SqlParameter() { ParameterName = "gameType", Value = placebetteen.gtype });
                                parameters2.Add(new SqlParameter() { ParameterName = "Userrate", Value = ds1.Tables[0].Rows[0]["newuserrate"] });
                                parameters2.Add(new SqlParameter() { ParameterName = "BetAmount", Value = ds1.Tables[0].Rows[0]["conformamount"] });
                                ds2 = _sqlClientService.Execute("Casino_ExposerUpdate", ConfigItems.Conn_AccDgroup, parameters2);
                                WriteLogAll("Placebetvqueen4+Casino_ExposerUpdate", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds2));
                                try
                                {
                                    if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 0 && ds2.Tables[1].Rows.Count > 0 && ds2.Tables[1].Columns.Contains("SendCoupon") && ds2.Tables[1].Rows[0]["SendCoupon"].ToString() == "1")
                                    {//Select 0 as SendCoupon,0 as BetAmount,'' as couponcode
                                        CouponPlacebet couponPlacebet = new CouponPlacebet();
                                        couponPlacebet.Amount = Convert.ToDouble(ds1.Tables[0].Rows[0]["conformamount"]);
                                        couponPlacebet.PlayerName = ds.Tables[0].Rows[0]["username"].ToString();
                                        couponPlacebet.PlayerId = placebetteen.pid;
                                        couponPlacebet.Product = ds2.Tables[1].Rows[0]["couponcode"].ToString();
                                        couponPlacebet.Balance = Convert.ToDouble(ds.Tables[0].Rows[0]["general"]);
                                        if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 1 && ds2.Tables[2].Rows.Count > 0 && ds2.Tables[2].Columns.Contains("clientid") && ds2.Tables[2].Columns.Contains("secretkey"))
                                        {
                                            var par = ConfigItems.Encrypt ? Encryptionbonus.EncryptString(JsonConvert.SerializeObject(couponPlacebet), ds2.Tables[2].Rows[0]["secretkey"].ToString()) : JsonConvert.SerializeObject(couponPlacebet);
                                            var resp = Postbonus(ConfigItems.CopUrl + "transaction/event", par, "application/json", "POST", ds2.Tables[2].Rows[0]["clientid"].ToString());
                                            var res = ConfigItems.Encrypt ? Encryptionbonus.DecryptString(resp, ds2.Tables[2].Rows[0]["secretkey"].ToString()) : resp;
                                            WriteLogAll("Placebetvqueen3", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(res));
                                        }
                                        //var resp = Post(ConfigItems.CopUrl + ApiEndpoint.placebet, JsonConvert.SerializeObject(couponPlacebet), "application/json", "POST");
                                        //var data = JsonConvert.DeserializeObject<CouponPlacebetRes>(resp);

                                        //Returnds("0", data.error);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteLogAll("Placebetvqueen+cop" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                Placebetfinal(placebetteen.uid);
                                return ds2;
                            }
                            else
                            {
                                Placebetfinal(placebetteen.uid);
                                return ds1;
                            }
                        }
                        Placebetfinal(placebetteen.uid);
                        return dscom;
                    }
                    Placebetfinal(placebetteen.uid);
                    return dscasino;
                }
                //Placebetfinal(placebetteen.uid);
                return ds;
            }
            catch (Exception ex)
            {
                Placebetfinal(placebetteen.uid);
                WriteLogAll("Placebetvqueen" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                throw ex;
            }

        }
        public DataSet Placebetvrace(Placebetteen placebetteen)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet dscom = new DataSet();
                DataSet dscasino = new DataSet();
                placebetteen.gtype.ToLower();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                parameters.Add(new SqlParameter() { ParameterName = "MarketId", Value = placebetteen.mid });
                parameters.Add(new SqlParameter() { ParameterName = "GameType", Value = placebetteen.gtype });
                ds = _sqlClientService.Execute("Casino_PlacebetCheck", ConfigItems.Conn_AccDgroup, parameters);
                WriteLogAll("Placebetvrace+Casino_PlacebetCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds));

                //1 as Id,'ok' as msg,username,u.PartnershipType,general,ExposerLimit,oldexposer,AutoConfirm
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("id") && ds.Tables[0].Rows[0]["id"].ToString() == "1")
                {
                    var parameterscas = new List<SqlParameter>();
                    parameterscas.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                    parameterscas.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                    parameterscas.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                    parameterscas.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                    parameterscas.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                    dscasino = _sqlClientService.Execute("Z_race_placeExpoCheck", ConfigItems.Conn_Casinogroup, parameterscas);
                    WriteLogAll("Placebetvrace1+Z_race_placeExpoCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscasino));
                    if (dscasino != null && dscasino.Tables != null && dscasino.Tables.Count > 0 && dscasino.Tables[0].Rows.Count > 0 && dscasino.Tables[0].Columns.Contains("id") && dscasino.Tables[0].Rows[0]["id"].ToString() == "1")
                    {
                        var parameters3 = new List<SqlParameter>();
                        parameters3.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                        parameters3.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                        parameters3.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                        parameters3.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                        parameters3.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                        parameters3.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                        parameters3.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                        dscom = _sqlClientService.Execute("group2_Z_race_placebetconform", ConfigItems.Conn_Casino, parameters3);
                        WriteLogAll("Placebetvrace2+group2_Z_race_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscom));
                        if (dscom != null && dscom.Tables != null && dscom.Tables.Count > 0 && dscom.Tables[0].Rows.Count > 0 && dscom.Tables[0].Columns.Contains("id") && dscom.Tables[0].Rows[0]["id"].ToString() == "1")
                        {
                            var parameters1 = new List<SqlParameter>();
                            parameters1.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                            parameters1.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                            parameters1.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                            parameters1.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                            parameters1.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                            parameters1.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                            parameters1.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                            parameters1.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                            parameters1.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newUserrate", Value = dscom.Tables[0].Rows[0]["NewUserrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newAmount", Value = dscom.Tables[0].Rows[0]["Amount"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Gameid", Value = dscom.Tables[0].Rows[0]["Gameid"] });
                            ds1 = _sqlClientService.Execute("Z_race_placebetconform", ConfigItems.Conn_Casinogroup, parameters1);
                            WriteLogAll("Placebetvrace3+Z_race_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds1));

                            //select 1 as ID,'Ok' as Msg,	@userexposer as userexposer, @JsonGameSub as masterexposer
                            if (ds1 != null && ds1.Tables != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0 && ds1.Tables[0].Columns.Contains("id") && ds1.Tables[0].Rows[0]["id"].ToString() == "1")
                            {
                                try
                                {
                                    var param = "marketid=" + placebetteen.mid + "&userid=" + placebetteen.uid + "&subsectionid=" + placebetteen.subid + "&gametype=" + placebetteen.gtype + "&subgametype="
                                                                     + placebetteen.subtype + "&username=" + ds.Tables[0].Rows[0]["username"] + "&sectionid=" + placebetteen.sid + "&bettype=" + placebetteen.btype
                                                                    + "&nation=" + ds1.Tables[0].Rows[0]["nation"].ToString() + "&userrate=" + placebetteen.urate + "&Masteramount=" + ds1.Tables[0].Rows[0]["conformamount"]
                                                                    + "&ipaddress=" + placebetteen.ip + "&browserdetail=" + placebetteen.bdetail + "&BookvalType=" + ds1.Tables[0].Rows[0]["exposertype"] +
                                                                    "&Bookvalmatch=" + ds1.Tables[0].Rows[0]["matchbook"] + "&BookvalFancy=" + ds1.Tables[0].Rows[0]["fancybook"] + "&amount=" + placebetteen.amt + "&frate=" + ds.Tables[0].Rows[0]["balrate"] + "&Currency=" + ds.Tables[0].Rows[0]["ccode"];

                                    Task.Factory.StartNew(() => Post(ConfigItems.AdminBookUrl + ApiEndpoint.vracebook, param, "application/x-www-form-urlencoded", "POST"));
                                }
                                catch (System.Exception ex)
                                {
                                    WriteLogAll("Placebetvrace+bk" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                var parameters2 = new List<SqlParameter>();
                                parameters2.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                                parameters2.Add(new SqlParameter() { ParameterName = "Amount", Value = Convert.ToDouble(ds1.Tables[0].Rows[0]["userexposer"]) });
                                parameters2.Add(new SqlParameter() { ParameterName = "ExposJson", Value = ds1.Tables[0].Rows[0]["masterexposer"].ToString() });
                                parameters2.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                                parameters2.Add(new SqlParameter() { ParameterName = "gameType", Value = placebetteen.gtype });
                                parameters2.Add(new SqlParameter() { ParameterName = "Userrate", Value = ds1.Tables[0].Rows[0]["newuserrate"] });
                                parameters2.Add(new SqlParameter() { ParameterName = "BetAmount", Value = ds1.Tables[0].Rows[0]["conformamount"] });
                                ds2 = _sqlClientService.Execute("Casino_ExposerUpdate", ConfigItems.Conn_AccDgroup, parameters2);
                                WriteLogAll("Placebetvrace4+Casino_ExposerUpdate", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds2));
                                try
                                {
                                    if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 0 && ds2.Tables[1].Rows.Count > 0 && ds2.Tables[1].Columns.Contains("SendCoupon") && ds2.Tables[1].Rows[0]["SendCoupon"].ToString() == "1")
                                    {//Select 0 as SendCoupon,0 as BetAmount,'' as couponcode
                                        CouponPlacebet couponPlacebet = new CouponPlacebet();
                                        couponPlacebet.Amount = Convert.ToDouble(ds1.Tables[0].Rows[0]["conformamount"]);
                                        couponPlacebet.PlayerName = ds.Tables[0].Rows[0]["username"].ToString();
                                        couponPlacebet.PlayerId = placebetteen.pid;
                                        couponPlacebet.Product = ds2.Tables[1].Rows[0]["couponcode"].ToString();
                                        couponPlacebet.Balance = Convert.ToDouble(ds.Tables[0].Rows[0]["general"]);
                                        if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 1 && ds2.Tables[2].Rows.Count > 0 && ds2.Tables[2].Columns.Contains("clientid") && ds2.Tables[2].Columns.Contains("secretkey"))
                                        {
                                            var par = ConfigItems.Encrypt ? Encryptionbonus.EncryptString(JsonConvert.SerializeObject(couponPlacebet), ds2.Tables[2].Rows[0]["secretkey"].ToString()) : JsonConvert.SerializeObject(couponPlacebet);
                                            var resp = Postbonus(ConfigItems.CopUrl + "transaction/event", par, "application/json", "POST", ds2.Tables[2].Rows[0]["clientid"].ToString());
                                            var res = ConfigItems.Encrypt ? Encryptionbonus.DecryptString(resp, ds2.Tables[2].Rows[0]["secretkey"].ToString()) : resp;
                                            WriteLogAll("Placebetvrace3", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(res));
                                        }
                                        //var resp = Post(ConfigItems.CopUrl + ApiEndpoint.placebet, JsonConvert.SerializeObject(couponPlacebet), "application/json", "POST");
                                        //var data = JsonConvert.DeserializeObject<CouponPlacebetRes>(resp);

                                        //Returnds("0", data.error);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteLogAll("Placebetvrace+cop" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                Placebetfinal(placebetteen.uid);
                                return ds2;
                            }
                            else
                            {
                                Placebetfinal(placebetteen.uid);
                                return ds1;
                            }
                        }
                        Placebetfinal(placebetteen.uid);
                        return dscom;
                    }
                    Placebetfinal(placebetteen.uid);
                    return dscasino;
                }
                //Placebetfinal(placebetteen.uid);
                return ds;
            }
            catch (Exception ex)
            {
                Placebetfinal(placebetteen.uid);
                WriteLogAll("Placebetvrace" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                throw ex;
            }

        }
        public DataSet Placebetvtrap(Placebetteen placebetteen)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet dscom = new DataSet();
                DataSet dscasino = new DataSet();
                placebetteen.gtype.ToLower();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                parameters.Add(new SqlParameter() { ParameterName = "MarketId", Value = placebetteen.mid });
                parameters.Add(new SqlParameter() { ParameterName = "GameType", Value = placebetteen.gtype });
                ds = _sqlClientService.Execute("Casino_PlacebetCheck", ConfigItems.Conn_AccDgroup, parameters);
                WriteLogAll("Placebetvtrap+Casino_PlacebetCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds));

                //1 as Id,'ok' as msg,username,u.PartnershipType,general,ExposerLimit,oldexposer,AutoConfirm
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("id") && ds.Tables[0].Rows[0]["id"].ToString() == "1")
                {
                    var parameterscas = new List<SqlParameter>();
                    parameterscas.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                    parameterscas.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                    parameterscas.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                    parameterscas.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                    parameterscas.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                    dscasino = _sqlClientService.Execute("Z_other1_placeExpoCheck", ConfigItems.Conn_Casinogroup, parameterscas);
                    WriteLogAll("Placebetvtrap1+Z_other1_placeExpoCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscasino));
                    if (dscasino != null && dscasino.Tables != null && dscasino.Tables.Count > 0 && dscasino.Tables[0].Rows.Count > 0 && dscasino.Tables[0].Columns.Contains("id") && dscasino.Tables[0].Rows[0]["id"].ToString() == "1")
                    {
                        var parameters3 = new List<SqlParameter>();
                        parameters3.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                        parameters3.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                        parameters3.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                        parameters3.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                        parameters3.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                        parameters3.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                        parameters3.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                        dscom = _sqlClientService.Execute("group2_Z_other1_placebetconform", ConfigItems.Conn_Casino, parameters3);
                        WriteLogAll("Placebetvtrap2+group2_Z_other1_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscom));
                        if (dscom != null && dscom.Tables != null && dscom.Tables.Count > 0 && dscom.Tables[0].Rows.Count > 0 && dscom.Tables[0].Columns.Contains("id") && dscom.Tables[0].Rows[0]["id"].ToString() == "1")
                        {
                            var parameters1 = new List<SqlParameter>();
                            parameters1.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                            parameters1.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                            parameters1.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                            parameters1.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                            parameters1.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                            parameters1.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                            parameters1.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                            parameters1.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                            parameters1.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newUserrate", Value = dscom.Tables[0].Rows[0]["NewUserrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newAmount", Value = dscom.Tables[0].Rows[0]["Amount"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Gameid", Value = dscom.Tables[0].Rows[0]["Gameid"] });
                            ds1 = _sqlClientService.Execute("Z_other1_placebetconform", ConfigItems.Conn_Casinogroup, parameters1);
                            WriteLogAll("Placebetvtrap3+Z_other1_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds1));

                            //select 1 as ID,'Ok' as Msg,	@userexposer as userexposer, @JsonGameSub as masterexposer
                            if (ds1 != null && ds1.Tables != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0 && ds1.Tables[0].Columns.Contains("id") && ds1.Tables[0].Rows[0]["id"].ToString() == "1")
                            {
                                try
                                {
                                    var param = "marketid=" + placebetteen.mid + "&userid=" + placebetteen.uid + "&subsectionid=" + placebetteen.subid + "&gametype=" + placebetteen.gtype + "&subgametype="
                                                                     + placebetteen.subtype + "&username=" + ds.Tables[0].Rows[0]["username"] + "&sectionid=" + placebetteen.sid + "&bettype=" + placebetteen.btype
                                                                    + "&nation=" + ds1.Tables[0].Rows[0]["nation"].ToString() + "&userrate=" + placebetteen.urate + "&Masteramount=" + ds1.Tables[0].Rows[0]["conformamount"]
                                                                    + "&ipaddress=" + placebetteen.ip + "&browserdetail=" + placebetteen.bdetail + "&BookvalType=" + ds1.Tables[0].Rows[0]["exposertype"] +
                                                                    "&Bookvalmatch=" + ds1.Tables[0].Rows[0]["matchbook"] + "&BookvalFancy=" + ds1.Tables[0].Rows[0]["fancybook"] + "&amount=" + placebetteen.amt + "&frate=" + ds.Tables[0].Rows[0]["balrate"] + "&Currency=" + ds.Tables[0].Rows[0]["ccode"];

                                    Task.Factory.StartNew(() => Post(ConfigItems.AdminBookUrl + ApiEndpoint.vtrapbook, param, "application/x-www-form-urlencoded", "POST"));
                                }
                                catch (System.Exception ex)
                                {
                                    WriteLogAll("Placebetvtrap+bk" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                var parameters2 = new List<SqlParameter>();
                                parameters2.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                                parameters2.Add(new SqlParameter() { ParameterName = "Amount", Value = Convert.ToDouble(ds1.Tables[0].Rows[0]["userexposer"]) });
                                parameters2.Add(new SqlParameter() { ParameterName = "ExposJson", Value = ds1.Tables[0].Rows[0]["masterexposer"].ToString() });
                                parameters2.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                                parameters2.Add(new SqlParameter() { ParameterName = "gameType", Value = placebetteen.gtype });
                                parameters2.Add(new SqlParameter() { ParameterName = "Userrate", Value = ds1.Tables[0].Rows[0]["newuserrate"] });
                                parameters2.Add(new SqlParameter() { ParameterName = "BetAmount", Value = ds1.Tables[0].Rows[0]["conformamount"] });
                                ds2 = _sqlClientService.Execute("Casino_ExposerUpdate", ConfigItems.Conn_AccDgroup, parameters2);
                                WriteLogAll("Placebetvtrap4+Casino_ExposerUpdate", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds2));
                                try
                                {
                                    if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 0 && ds2.Tables[1].Rows.Count > 0 && ds2.Tables[1].Columns.Contains("SendCoupon") && ds2.Tables[1].Rows[0]["SendCoupon"].ToString() == "1")
                                    {//Select 0 as SendCoupon,0 as BetAmount,'' as couponcode
                                        CouponPlacebet couponPlacebet = new CouponPlacebet();
                                        couponPlacebet.Amount = Convert.ToDouble(ds1.Tables[0].Rows[0]["conformamount"]);
                                        couponPlacebet.PlayerName = ds.Tables[0].Rows[0]["username"].ToString();
                                        couponPlacebet.PlayerId = placebetteen.pid;
                                        couponPlacebet.Product = ds2.Tables[1].Rows[0]["couponcode"].ToString();
                                        couponPlacebet.Balance = Convert.ToDouble(ds.Tables[0].Rows[0]["general"]);
                                        if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 1 && ds2.Tables[2].Rows.Count > 0 && ds2.Tables[2].Columns.Contains("clientid") && ds2.Tables[2].Columns.Contains("secretkey"))
                                        {
                                            var par = ConfigItems.Encrypt ? Encryptionbonus.EncryptString(JsonConvert.SerializeObject(couponPlacebet), ds2.Tables[2].Rows[0]["secretkey"].ToString()) : JsonConvert.SerializeObject(couponPlacebet);
                                            var resp = Postbonus(ConfigItems.CopUrl + "transaction/event", par, "application/json", "POST", ds2.Tables[2].Rows[0]["clientid"].ToString());
                                            var res = ConfigItems.Encrypt ? Encryptionbonus.DecryptString(resp, ds2.Tables[2].Rows[0]["secretkey"].ToString()) : resp;
                                            WriteLogAll("Placebetvtrap3", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(res));
                                        }
                                        //var resp = Post(ConfigItems.CopUrl + ApiEndpoint.placebet, JsonConvert.SerializeObject(couponPlacebet), "application/json", "POST");
                                        //var data = JsonConvert.DeserializeObject<CouponPlacebetRes>(resp);

                                        //Returnds("0", data.error);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteLogAll("Placebetvtrap+cop" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                Placebetfinal(placebetteen.uid);
                                return ds2;
                            }
                            else
                            {
                                Placebetfinal(placebetteen.uid);
                                return ds1;
                            }
                        }
                        Placebetfinal(placebetteen.uid);
                        return dscom;
                    }
                    Placebetfinal(placebetteen.uid);
                    return dscasino;
                }
                //Placebetfinal(placebetteen.uid);
                return ds;
            }
            catch (Exception ex)
            {
                Placebetfinal(placebetteen.uid);
                WriteLogAll("Placebetvtrap" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                throw ex;
            }

        }
        public DataSet Placebetvpatti2(Placebetpatti2 placebetteen)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet dscom = new DataSet();
                DataSet dscasino = new DataSet();
                placebetteen.gtype.ToLower();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                parameters.Add(new SqlParameter() { ParameterName = "MarketId", Value = placebetteen.mid });
                parameters.Add(new SqlParameter() { ParameterName = "GameType", Value = placebetteen.gtype });
                ds = _sqlClientService.Execute("Casino_PlacebetCheck", ConfigItems.Conn_AccDgroup, parameters);
                WriteLogAll("Placebetvpatti2+Casino_PlacebetCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds));

                //1 as Id,'ok' as msg,username,u.PartnershipType,general,ExposerLimit,oldexposer,AutoConfirm
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("id") && ds.Tables[0].Rows[0]["id"].ToString() == "1")
                {
                    var parameterscas = new List<SqlParameter>();
                    parameterscas.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                    parameterscas.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                    parameterscas.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                    parameterscas.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                    parameterscas.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                    dscasino = _sqlClientService.Execute("Z_other1_placeExpoCheck", ConfigItems.Conn_Casinogroup, parameterscas);
                    WriteLogAll("Placebetvpatti21+Z_other1_placeExpoCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscasino));
                    if (dscasino != null && dscasino.Tables != null && dscasino.Tables.Count > 0 && dscasino.Tables[0].Rows.Count > 0 && dscasino.Tables[0].Columns.Contains("id") && dscasino.Tables[0].Rows[0]["id"].ToString() == "1")
                    {
                        var parameters3 = new List<SqlParameter>();
                        parameters3.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                        parameters3.Add(new SqlParameter() { ParameterName = "bhav", Value = placebetteen.bhav });
                        parameters3.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                        parameters3.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                        parameters3.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                        parameters3.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                        parameters3.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                        parameters3.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                        dscom = _sqlClientService.Execute("group2_Z_other1_placebetconform", ConfigItems.Conn_Casino, parameters3);
                        WriteLogAll("Placebetvpatti22+group2_Z_other1_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscom));
                        if (dscom != null && dscom.Tables != null && dscom.Tables.Count > 0 && dscom.Tables[0].Rows.Count > 0 && dscom.Tables[0].Columns.Contains("id") && dscom.Tables[0].Rows[0]["id"].ToString() == "1")
                        {
                            var parameters1 = new List<SqlParameter>();
                            parameters1.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                            parameters1.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                            parameters1.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                            parameters1.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                            parameters1.Add(new SqlParameter() { ParameterName = "bhav", Value = placebetteen.bhav });
                            parameters1.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                            parameters1.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                            parameters1.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                            parameters1.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                            parameters1.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newUserrate", Value = dscom.Tables[0].Rows[0]["NewUserrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newAmount", Value = dscom.Tables[0].Rows[0]["Amount"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Gameid", Value = dscom.Tables[0].Rows[0]["Gameid"] });
                            ds1 = _sqlClientService.Execute("Z_other1_placebetconform", ConfigItems.Conn_Casinogroup, parameters1);
                            WriteLogAll("Placebetvpatti23+Z_other1_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds1));

                            //select 1 as ID,'Ok' as Msg,	@userexposer as userexposer, @JsonGameSub as masterexposer
                            if (ds1 != null && ds1.Tables != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0 && ds1.Tables[0].Columns.Contains("id") && ds1.Tables[0].Rows[0]["id"].ToString() == "1")
                            {
                                try
                                {
                                    var param = "marketid=" + placebetteen.mid + "&userid=" + placebetteen.uid + "&subsectionid=" + placebetteen.subid + "&gametype=" + placebetteen.gtype + "&subgametype="
                                                                     + placebetteen.subtype + "&username=" + ds.Tables[0].Rows[0]["username"] + "&sectionid=" + placebetteen.sid + "&bettype=" + placebetteen.btype
                                                                    + "&nation=" + ds1.Tables[0].Rows[0]["nation"].ToString() + "&userrate=" + placebetteen.urate + "&Masteramount=" + ds1.Tables[0].Rows[0]["conformamount"]
                                                                    + "&ipaddress=" + placebetteen.ip + "&browserdetail=" + placebetteen.bdetail + "&BookvalType=" + ds1.Tables[0].Rows[0]["exposertype"] +
                                                                    "&Bookvalmatch=" + ds1.Tables[0].Rows[0]["matchbook"] + "&BookvalFancy=" + ds1.Tables[0].Rows[0]["fancybook"] + "&amount=" + placebetteen.amt + "&frate=" + ds.Tables[0].Rows[0]["balrate"] + "&Currency=" + ds.Tables[0].Rows[0]["ccode"];

                                    Task.Factory.StartNew(() => Post(ConfigItems.AdminBookUrl + ApiEndpoint.vtrapbook, param, "application/x-www-form-urlencoded", "POST"));
                                }
                                catch (System.Exception ex)
                                {
                                    WriteLogAll("Placebetvpatti2+bk" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                var parameters2 = new List<SqlParameter>();
                                parameters2.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                                parameters2.Add(new SqlParameter() { ParameterName = "Amount", Value = Convert.ToDouble(ds1.Tables[0].Rows[0]["userexposer"]) });
                                parameters2.Add(new SqlParameter() { ParameterName = "ExposJson", Value = ds1.Tables[0].Rows[0]["masterexposer"].ToString() });
                                parameters2.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                                parameters2.Add(new SqlParameter() { ParameterName = "gameType", Value = placebetteen.gtype });
                                parameters2.Add(new SqlParameter() { ParameterName = "Userrate", Value = ds1.Tables[0].Rows[0]["newuserrate"] });
                                parameters2.Add(new SqlParameter() { ParameterName = "BetAmount", Value = ds1.Tables[0].Rows[0]["conformamount"] });
                                ds2 = _sqlClientService.Execute("Casino_ExposerUpdate", ConfigItems.Conn_AccDgroup, parameters2);
                                WriteLogAll("Placebetvpatti24+Casino_ExposerUpdate", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds2));
                                try
                                {
                                    if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 0 && ds2.Tables[1].Rows.Count > 0 && ds2.Tables[1].Columns.Contains("SendCoupon") && ds2.Tables[1].Rows[0]["SendCoupon"].ToString() == "1")
                                    {//Select 0 as SendCoupon,0 as BetAmount,'' as couponcode
                                        CouponPlacebet couponPlacebet = new CouponPlacebet();
                                        couponPlacebet.Amount = Convert.ToDouble(ds1.Tables[0].Rows[0]["conformamount"]);
                                        couponPlacebet.PlayerName = ds.Tables[0].Rows[0]["username"].ToString();
                                        couponPlacebet.PlayerId = placebetteen.pid;
                                        couponPlacebet.Product = ds2.Tables[1].Rows[0]["couponcode"].ToString();
                                        couponPlacebet.Balance = Convert.ToDouble(ds.Tables[0].Rows[0]["general"]);
                                        if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 1 && ds2.Tables[2].Rows.Count > 0 && ds2.Tables[2].Columns.Contains("clientid") && ds2.Tables[2].Columns.Contains("secretkey"))
                                        {
                                            var par = ConfigItems.Encrypt ? Encryptionbonus.EncryptString(JsonConvert.SerializeObject(couponPlacebet), ds2.Tables[2].Rows[0]["secretkey"].ToString()) : JsonConvert.SerializeObject(couponPlacebet);
                                            var resp = Postbonus(ConfigItems.CopUrl + "transaction/event", par, "application/json", "POST", ds2.Tables[2].Rows[0]["clientid"].ToString());
                                            var res = ConfigItems.Encrypt ? Encryptionbonus.DecryptString(resp, ds2.Tables[2].Rows[0]["secretkey"].ToString()) : resp;
                                            WriteLogAll("Placebetvpatti23", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(res));
                                        }
                                        //var resp = Post(ConfigItems.CopUrl + ApiEndpoint.placebet, JsonConvert.SerializeObject(couponPlacebet), "application/json", "POST");
                                        //var data = JsonConvert.DeserializeObject<CouponPlacebetRes>(resp);

                                        //Returnds("0", data.error);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteLogAll("Placebetvpatti2+cop" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                Placebetfinal(placebetteen.uid);
                                return ds2;
                            }
                            else
                            {
                                Placebetfinal(placebetteen.uid);
                                return ds1;
                            }
                        }
                        Placebetfinal(placebetteen.uid);
                        return dscom;
                    }
                    Placebetfinal(placebetteen.uid);
                    return dscasino;
                }
                //Placebetfinal(placebetteen.uid);
                return ds;
            }
            catch (Exception ex)
            {
                Placebetfinal(placebetteen.uid);
                WriteLogAll("Placebetvpatti2" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                throw ex;
            }

        }
        public DataSet Placebetvnotenum(Placebetpatti2 placebetteen)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet dscom = new DataSet();
                DataSet dscasino = new DataSet();
                placebetteen.gtype.ToLower();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                parameters.Add(new SqlParameter() { ParameterName = "MarketId", Value = placebetteen.mid });
                parameters.Add(new SqlParameter() { ParameterName = "GameType", Value = placebetteen.gtype });
                ds = _sqlClientService.Execute("Casino_PlacebetCheck", ConfigItems.Conn_AccDgroup, parameters);
                WriteLogAll("Placebetvnotenum+Casino_PlacebetCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds));

                //1 as Id,'ok' as msg,username,u.PartnershipType,general,ExposerLimit,oldexposer,AutoConfirm
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("id") && ds.Tables[0].Rows[0]["id"].ToString() == "1")
                {
                    var parameterscas = new List<SqlParameter>();
                    parameterscas.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                    parameterscas.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                    parameterscas.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                    parameterscas.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                    parameterscas.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                    parameterscas.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                    parameterscas.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                    parameterscas.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                    dscasino = _sqlClientService.Execute("Z_other2_placeExpoCheck", ConfigItems.Conn_Casinogroup, parameterscas);
                    WriteLogAll("Placebetvnotenum1+Z_other2_placeExpoCheck", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscasino));
                    if (dscasino != null && dscasino.Tables != null && dscasino.Tables.Count > 0 && dscasino.Tables[0].Rows.Count > 0 && dscasino.Tables[0].Columns.Contains("id") && dscasino.Tables[0].Rows[0]["id"].ToString() == "1")
                    {
                        var parameters3 = new List<SqlParameter>();
                        parameters3.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                        parameters3.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                        parameters3.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                        parameters3.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                        parameters3.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                        parameters3.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                        parameters3.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                        parameters3.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                        parameters3.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                        dscom = _sqlClientService.Execute("group2_Z_other2_placebetconform", ConfigItems.Conn_Casino, parameters3);
                        WriteLogAll("Placebetvnotenum2+group2_Z_other2_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(dscom));
                        if (dscom != null && dscom.Tables != null && dscom.Tables.Count > 0 && dscom.Tables[0].Rows.Count > 0 && dscom.Tables[0].Columns.Contains("id") && dscom.Tables[0].Rows[0]["id"].ToString() == "1")
                        {
                            var parameters1 = new List<SqlParameter>();
                            parameters1.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                            parameters1.Add(new SqlParameter() { ParameterName = "sectionid", Value = placebetteen.sid });
                            parameters1.Add(new SqlParameter() { ParameterName = "subsectionid", Value = placebetteen.subid });
                            parameters1.Add(new SqlParameter() { ParameterName = "bettype", Value = placebetteen.btype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userrate", Value = placebetteen.urate });
                            parameters1.Add(new SqlParameter() { ParameterName = "bhav", Value = placebetteen.bhav });
                            parameters1.Add(new SqlParameter() { ParameterName = "amount", Value = placebetteen.amt });
                            parameters1.Add(new SqlParameter() { ParameterName = "general", Value = ds.Tables[0].Rows[0]["general"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "oldexposer", Value = ds.Tables[0].Rows[0]["exposer"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "exposerlimit", Value = ds.Tables[0].Rows[0]["ExposerLimit"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "gametype", Value = placebetteen.gtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "subgametype", Value = placebetteen.subtype });
                            parameters1.Add(new SqlParameter() { ParameterName = "userid", Value = placebetteen.uid });
                            parameters1.Add(new SqlParameter() { ParameterName = "username", Value = ds.Tables[0].Rows[0]["username"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "ipaddress", Value = placebetteen.ip });
                            parameters1.Add(new SqlParameter() { ParameterName = "browserdetail", Value = placebetteen.bdetail });
                            parameters1.Add(new SqlParameter() { ParameterName = "Usertype", Value = ds.Tables[0].Rows[0]["PartnershipType"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "frate", Value = ds.Tables[0].Rows[0]["balrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemin", Value = ds.Tables[0].Rows[0]["balratemin"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "fratemul", Value = ds.Tables[0].Rows[0]["balratemul"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Spart", Value = ds.Tables[0].Rows[0]["apart"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "currname", Value = ds.Tables[0].Rows[0]["ccode"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newUserrate", Value = dscom.Tables[0].Rows[0]["NewUserrate"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "newAmount", Value = dscom.Tables[0].Rows[0]["Amount"] });
                            parameters1.Add(new SqlParameter() { ParameterName = "Gameid", Value = dscom.Tables[0].Rows[0]["Gameid"] });
                            ds1 = _sqlClientService.Execute("Z_other2_placebetconform", ConfigItems.Conn_Casinogroup, parameters1);
                            WriteLogAll("Placebetvnotenum3+Z_other2_placebetconform", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds1));

                            //select 1 as ID,'Ok' as Msg,	@userexposer as userexposer, @JsonGameSub as masterexposer
                            if (ds1 != null && ds1.Tables != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0 && ds1.Tables[0].Columns.Contains("id") && ds1.Tables[0].Rows[0]["id"].ToString() == "1")
                            {
                                try
                                {
                                    var param = "marketid=" + placebetteen.mid + "&userid=" + placebetteen.uid + "&subsectionid=" + placebetteen.subid + "&gametype=" + placebetteen.gtype + "&subgametype="
                                                                     + placebetteen.subtype + "&username=" + ds.Tables[0].Rows[0]["username"] + "&sectionid=" + placebetteen.sid + "&bettype=" + placebetteen.btype
                                                                    + "&nation=" + ds1.Tables[0].Rows[0]["nation"].ToString() + "&userrate=" + placebetteen.urate + "&Masteramount=" + ds1.Tables[0].Rows[0]["conformamount"]
                                                                    + "&ipaddress=" + placebetteen.ip + "&browserdetail=" + placebetteen.bdetail + "&BookvalType=" + ds1.Tables[0].Rows[0]["exposertype"] +
                                                                    "&Bookvalmatch=" + ds1.Tables[0].Rows[0]["matchbook"] + "&BookvalFancy=" + ds1.Tables[0].Rows[0]["fancybook"] + "&amount=" + placebetteen.amt + "&frate=" + ds.Tables[0].Rows[0]["balrate"] + "&Currency=" + ds.Tables[0].Rows[0]["ccode"];

                                    Task.Factory.StartNew(() => Post(ConfigItems.AdminBookUrl + ApiEndpoint.vnotenumbook, param, "application/x-www-form-urlencoded", "POST"));
                                }
                                catch (System.Exception ex)
                                {
                                    WriteLogAll("Placebetvnotenum+bk" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                var parameters2 = new List<SqlParameter>();
                                parameters2.Add(new SqlParameter() { ParameterName = "Userid", Value = placebetteen.uid });
                                parameters2.Add(new SqlParameter() { ParameterName = "Amount", Value = Convert.ToDouble(ds1.Tables[0].Rows[0]["userexposer"]) });
                                parameters2.Add(new SqlParameter() { ParameterName = "ExposJson", Value = ds1.Tables[0].Rows[0]["masterexposer"].ToString() });
                                parameters2.Add(new SqlParameter() { ParameterName = "marketid", Value = placebetteen.mid });
                                parameters2.Add(new SqlParameter() { ParameterName = "gameType", Value = placebetteen.gtype });
                                parameters2.Add(new SqlParameter() { ParameterName = "Userrate", Value = ds1.Tables[0].Rows[0]["newuserrate"] });
                                parameters2.Add(new SqlParameter() { ParameterName = "BetAmount", Value = ds1.Tables[0].Rows[0]["conformamount"] });
                                ds2 = _sqlClientService.Execute("Casino_ExposerUpdate", ConfigItems.Conn_AccDgroup, parameters2);
                                WriteLogAll("Placebetvnotenum4+Casino_ExposerUpdate", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(ds2));
                                try
                                {
                                    if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 0 && ds2.Tables[1].Rows.Count > 0 && ds2.Tables[1].Columns.Contains("SendCoupon") && ds2.Tables[1].Rows[0]["SendCoupon"].ToString() == "1")
                                    {//Select 0 as SendCoupon,0 as BetAmount,'' as couponcode
                                        CouponPlacebet couponPlacebet = new CouponPlacebet();
                                        couponPlacebet.Amount = Convert.ToDouble(ds1.Tables[0].Rows[0]["conformamount"]);
                                        couponPlacebet.PlayerName = ds.Tables[0].Rows[0]["username"].ToString();
                                        couponPlacebet.PlayerId = placebetteen.pid;
                                        couponPlacebet.Product = ds2.Tables[1].Rows[0]["couponcode"].ToString();
                                        couponPlacebet.Balance = Convert.ToDouble(ds.Tables[0].Rows[0]["general"]);
                                        if (ds2 != null && ds2.Tables != null && ds2.Tables.Count > 1 && ds2.Tables[2].Rows.Count > 0 && ds2.Tables[2].Columns.Contains("clientid") && ds2.Tables[2].Columns.Contains("secretkey"))
                                        {
                                            var par = ConfigItems.Encrypt ? Encryptionbonus.EncryptString(JsonConvert.SerializeObject(couponPlacebet), ds2.Tables[2].Rows[0]["secretkey"].ToString()) : JsonConvert.SerializeObject(couponPlacebet);
                                            var resp = Postbonus(ConfigItems.CopUrl + "transaction/event", par, "application/json", "POST", ds2.Tables[2].Rows[0]["clientid"].ToString());
                                            var res = ConfigItems.Encrypt ? Encryptionbonus.DecryptString(resp, ds2.Tables[2].Rows[0]["secretkey"].ToString()) : resp;
                                            WriteLogAll("Placebetteen3", "Req:" + JsonConvert.SerializeObject(placebetteen) + "Res" + JsonConvert.SerializeObject(res));
                                        }
                                        //var resp = Post(ConfigItems.CopUrl + ApiEndpoint.placebet, JsonConvert.SerializeObject(couponPlacebet), "application/json", "POST");
                                        //var data = JsonConvert.DeserializeObject<CouponPlacebetRes>(resp);

                                        //Returnds("0", data.error);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteLogAll("Placebetteen+cop" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                                }
                                Placebetfinal(placebetteen.uid);
                                return ds2;
                            }
                            else
                            {
                                Placebetfinal(placebetteen.uid);
                                return ds1;
                            }
                        }
                        Placebetfinal(placebetteen.uid);
                        return dscom;
                    }
                    Placebetfinal(placebetteen.uid);
                    return dscasino;
                }
                //Placebetfinal(placebetteen.uid);
                return ds;
            }
            catch (Exception ex)
            {
                Placebetfinal(placebetteen.uid);
                WriteLogAll("Placebetvnotenum" + ex.ToString(), JsonConvert.SerializeObject(placebetteen));
                throw ex;
            }
        }
        public void Placebetfinal(Int64 uid)
        {
            try
            {
                DataSet ds = new DataSet();
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { ParameterName = "Userid", Value = uid });
                _sqlClientService.Execute("BetStatusUpdate", ConfigItems.Conn_AccDgroup, parameters);
                WriteLogAll("Placebetfinal+BetStatusUpdate", "Req:" + JsonConvert.SerializeObject(uid) + "Res" + JsonConvert.SerializeObject(ds));
            }
            catch (Exception ex)
            {
                WriteLogAll("Placebetfinal" + ex.ToString(), JsonConvert.SerializeObject(uid));
            }

        }
        #endregion
        public static string Postbonus(string uri, string data, string contentType, string method = "POST", String cid = "")
        {
            try
            {
                byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

                request.ContentLength = dataBytes.Length;
                request.ContentType = contentType;
                request.Method = method;
                request.Accept = "text/plain";

                request.Headers.Add("X-Client", cid);
                if (!ConfigItems.Encrypt)
                {
                    request.Headers.Add("Encrypt", "false");
                }

                using (Stream requestBody = request.GetRequestStream())
                {
                    requestBody.Write(dataBytes, 0, dataBytes.Length);
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                return "{\"status\":\"400\",\"msg\":\"Unauthorized Access\"}";
            }
            catch (WebException ex)
            {
                //_Elog.WriteLog(ex.Response.ToString());
                //return ex.Response.ToString();
                using (Stream stream = ex.Response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                return "{\"status\":\"400\",\"msg\":\"" + ex.Message.ToString() + "\"}";
            }
        }
        public static string Post(string uri, string data, string contentType, string method = "POST", string authHeader = "")
        {
            try
            {
                byte[] dataBytes = Encoding.UTF8.GetBytes(data);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                request.ContentLength = dataBytes.Length;
                request.ContentType = contentType;
                request.Method = method;

                if (!string.IsNullOrEmpty(authHeader))
                    request.Headers.Add("Authorization", "bearer " + authHeader);

                using (Stream requestBody = request.GetRequestStream())
                {
                    requestBody.Write(dataBytes, 0, dataBytes.Length);
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                return "{'status':'400','message':'Unauthorized Access'}";
            }
            catch (WebException ex)
            {
                //if (ex.Response != null)
                //{
                var response = (HttpWebResponse)ex.Response;
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
                //}
            }
            catch (Exception ex)
            {
                WriteLogAll("Post : " + ex.ToString(), " : URI : " + uri + " : data : " + data);
                return "{'status':'400','message':'" + ex.ToString() + "'}";
            }
        }

        public static void WriteLogAll(String str, String request = null)
        {
            try
            {
                if (ConfigItems.AllLog)
                {
                    string path = System.Web.Hosting.HostingEnvironment.MapPath("~/Log/Bet/Log_" + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt");
                    if (!File.Exists(path))
                    {
                        var myFile = File.Create(path);
                        myFile.Close();
                    }
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine("---------------------------------------------------------------------------------------------");
                        //sw.Write(DateTime.Now);.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                        sw.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture));
                        sw.WriteLine(Environment.NewLine + str + Environment.NewLine + request);
                    }
                }
            }
            catch (Exception)
            { }
        }
    }
}
