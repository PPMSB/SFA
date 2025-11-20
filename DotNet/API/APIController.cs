using DotNet.Visitor_Model;
using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using static DotNet.CampaignModel.CampaignModel;

namespace DotNet
{
    public class APIController : ApiController
    {
        [Route("api/API/UploadFile")]
        public object UploadFile()
        {
            Function_Method.isWarranty = true;
            Function_Method.isPBM = false;
            Function_Method.isPoonshReport = false;
            Function_Method.isVPPPCampaign = true;

            string Msg = "";
            var req = System.Web.HttpContext.Current.Request;
            HttpPostedFile File = req.Files["File"];

            string FileNo = System.Web.HttpContext.Current.Request.Form["FileNo"];
            string DocID = System.Web.HttpContext.Current.Request.Form["DocID"];
            string UserName = HttpContext.Current.Request.Form["UserName"];
            string UserDisplayName = GLOBAL.logined_user_name;
            string CampaignID = HttpContext.Current.Request.Form["CampaignID"];
            string CustAcc = HttpContext.Current.Request.Form["CustAcc"];
            string FullName = HttpContext.Current.Request.Form["FullName"];
            string CustName = GlobalHelper.GET_CustomerName_From_CustID(CustAcc, UserName);
            
            UserMainMenuModel userModel = GlobalHelper.UserMainMenuModelData(UserName);//6/6/2025-Fix email Dispaly salesman name issue.(Neil)
            if (userModel != null)
            {
                UserDisplayName = userModel.logined_user_name;
            }
            
            int intFileNo = Int32.Parse(FileNo);
            try
            {
                int TempFileNo = GlobalHelper.SaveFile(UserName, intFileNo, File, CampaignID, CustAcc, "VPPPCampaign");

                if (TempFileNo == 0)
                {
                    Msg = "Upload Document failed, please resubmit again.";
                    return Json(Msg);
                }

                MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
                conn.Open();
                string SQL = "update campaign_document set FileID = @p0, UpdatedDateTime = @p2, UpdatedBy = @p3, Status = @p4 where ID = @p1";

                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                Dictionary<string, object> ParamDict = new Dictionary<string, object> {
                    { "@p0", TempFileNo },
                    { "@p1", DocID },
                    { "@p2", DateTime.Now.ToString(GLOBAL.gDisplayDateTimeFormat) },
                    { "@p3", UserName },
                    { "@p4", DocumentStatus.Uploaded },
                };
                GlobalHelper.PumpParamQuery(cmd, ParamDict);

                string MailTo = "ppm_vppp@posim.com.my";
                string Sendmsg = UserDisplayName + " submitted VPPP Campaign offer letter [" + CustName + "] on " + DateTime.Now.ToString(GLOBAL.gDisplayDateTimeFormat);
                string Subject = "VPPP Campaign submitted document" + " [" + CustName + "] ";

                API.APIProcessor.GetEmailInfo(MailTo, "", "", "" + ",", "", Sendmsg, Subject);
                //cmd.Parameters.AddWithValue("@p0", TempFileNo);
                //cmd.Parameters.AddWithValue("@p1", DocID);
                //cmd.Parameters.AddWithValue("@p2", DateTime.Now.ToString(GLOBAL.gDisplayDateTimeFormat));
                //cmd.Parameters.AddWithValue("@p3", GLOBAL.user_id);
                //cmd.Parameters.AddWithValue("@p4", DocumentStatus.Awaiting);

                cmd.ExecuteNonQuery();
                //GlobalHelper.LogToDatabase("APIController.UploadFile", $"{SQL}", null, UserName);
                conn.Close();
            }
            catch (Exception ex)
            {
                Function_Method.AddLog("> Error: " + ex.Message);//Check PDF Unable Submit 30.5.2025
                Msg = ex.ToString();
            }

            return Json(Msg);
        }

        [Route("api/API/UpdateOriReceive")]
        public object UpdateOriReceive()
        {
            string Msg = "";
            string DocID = System.Web.HttpContext.Current.Request.Form["DocID"];
            string strIsOriReceive = System.Web.HttpContext.Current.Request.Form["IsOriReceive"];
            bool IsOriReceive = (strIsOriReceive == "true" ? true : false);
            try
            {
                MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
                conn.Open();
                string SQL = "update campaign_document set IsOriReceive = @p0 where ID = @p1";

                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                Dictionary<string, object> ParamDict = new Dictionary<string, object> {
                    { "@p0", IsOriReceive },
                    { "@p1", DocID },
                };
                GlobalHelper.PumpParamQuery(cmd, ParamDict);

                cmd.ExecuteNonQuery();
                //GlobalHelper.LogToDatabase("APIController.UpdateOriReceive", $"{SQL}", null, "system");
                conn.Close();
            }
            catch (Exception ex)
            {
                Msg = ex.ToString();
            }

            return Json(Msg);
        }

        [Route("api/API/SendCampaignEmailNotification")]
        public string SendCampaignEmailNotification()
        {
            Function_Method.isWarranty = true;
            Function_Method.isPBM = false;
            Function_Method.isPoonshReport = false;
            string FileName = DateTime.Now.ToString(GLOBAL.gDisplayDateFormat) + " - CampaignEmailNotification";
            try
            {
                API.APIProcessor.NotifySalesmanCustomerReturnForm();

                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        [Route("api/API/RemoveUnusableIncentiveCampaignFile")]
        public string RemoveUnusableIncentiveCampaignFile()
        {
            try
            {
                HashSet<string> AvailableCompanyList = API.APIProcessor.GetAvailableCompanyList();
                HashSet<string> FileList = API.APIProcessor.GetRemoveFileID(AvailableCompanyList);
                API.APIProcessor.RemoveFile(FileList);

                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        [Route("api/API/FetchCampaignDataFromAxaptaDocument")]
        public string FetchCampaignDataFromAxaptaDocument()
        {
            try
            {
                API.APIProcessor.FetchCampaignDataFromAxaptaDocument();

                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        


        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}