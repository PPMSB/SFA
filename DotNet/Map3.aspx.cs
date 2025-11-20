using ActiveDs;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Web.Services;
using Newtonsoft.Json;
using System.Web.Script.Services;
using MySqlX.XDevAPI.Relational;
using GLOBAL_FUNCTION;
using static ZXing.QrCode.Internal.Mode;
using System.IO;


namespace DotNet
{
    public partial class Map3 : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            check_session();
            TimeOutRedirect();

            // Ensure that the Page_Load event is not triggered during a postback
            if (!IsPostBack)
            {
                // Register the initMap JavaScript function to be called on page load
                ClientScript.RegisterStartupScript(this.GetType(), "initializeMap", "initMap();", true);

                // Populate dropdowns and other initializations
                PopulateStateIdDropDown();
                PopulateCustomerMainGroupDropDown();
            }
        }

        private void TimeOutRedirect()
        {
            HtmlMeta meta = new HtmlMeta();

            meta.HttpEquiv = "Refresh";

            meta.Content = Convert.ToString(Session.Timeout * 60) + ";url=LoginPage.aspx";

            this.Page.Header.Controls.Add(meta);
        }

        private void check_session()
        {
            try
            {
                //load session user
                GLOBAL.user_id = Session["user_id"].ToString();
                GLOBAL.axPWD = Session["axPWD"].ToString();
                GLOBAL.logined_user_name = Session["logined_user_name"].ToString();
                GLOBAL.user_authority_lvl = Convert.ToInt32(Session["user_authority_lvl"]);
                GLOBAL.page_access_authority = Convert.ToInt32(Session["page_access_authority"]);
                GLOBAL.user_company = Session["user_company"].ToString();
                GLOBAL.module_access_authority = Convert.ToInt32(Session["module_access_authority"]);
                GLOBAL.switch_Company = Session["switch_Company"].ToString();
                GLOBAL.system_checking = Convert.ToInt32(Session["system_checking"]);
                GLOBAL.data_passing = Session["data_passing"].ToString();
            }
            catch
            {
                Session.Clear();
                Response.Redirect("LoginPage.aspx");
            }
        }

        private void PopulateStateIdDropDown()
        {
            Axapta DynAx = new Axapta();

            try
            {
                // Connect to Axapta using your existing connection code
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName,
                    new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                    GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                // Call the method to get state IDs
                List<ListItem> stateIdList = GetAxStateIds(DynAx);

                // Bind the state IDs to the dropdown list
                ddlState.DataSource = stateIdList;
                ddlState.DataBind();
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                //System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        public List<ListItem> GetAxStateIds(Axapta DynAx)
        {
            List<ListItem> stateIdList = new List<ListItem>();

            int AddressState = 418;

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", AddressState);

            var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", 3);//countryRegionId
            qbr.Call("value", "MY");
            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

            stateIdList.Add(new ListItem("-- SELECT --", ""));
            while ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", AddressState);
                string temp_StateId = DynRec.get_Field("StateId").ToString();

                System.Diagnostics.Debug.WriteLine($"State ID: {temp_StateId}");

                stateIdList.Add(new ListItem(temp_StateId));

                DynRec.Dispose();
            }

            return stateIdList;
        }

        private void PopulateCustomerMainGroupDropDown()
        {
            Axapta DynAx = new Axapta();

            try
            {
                // Connect to Axapta using your existing connection code
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName,
                    new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                    GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                // Call the method to get customer main groups
                List<ListItem> customerMainGroupList = GetAxCustomerMainGroups(DynAx);

                // Bind the customer main groups to the DropDownList
                ddlCustomerMainGroup.DataSource = customerMainGroupList;
                ddlCustomerMainGroup.DataBind();
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                //System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        public List<ListItem> GetAxCustomerMainGroups(Axapta DynAx)
        {
            List<ListItem> customerMainGroupList = new List<ListItem>();

            int MSBCustomerMainGroup = 30004;
            string customerMainGroupField = "MainGroupDesc";

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", MSBCustomerMainGroup);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

            customerMainGroupList.Add(new ListItem("-- SELECT --", ""));
            while ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord dynRec = (AxaptaRecord)axQueryRun.Call("Get", MSBCustomerMainGroup);
                string tempCustomerMainGroupDesc = dynRec.get_Field(customerMainGroupField).ToString();

                // Output to debug for verification
                System.Diagnostics.Debug.WriteLine($"Customer Main Group Description: {tempCustomerMainGroupDesc}");

                customerMainGroupList.Add(new ListItem(tempCustomerMainGroupDesc));

                dynRec.Dispose();
            }

            return customerMainGroupList;
        }

        public List<ListItem> get_custCoordinate(Axapta DynAx, string state)
        {
            string gps = "";
            string address = ""; string name = "";
            List<ListItem> stateCoordinateList = new List<ListItem>();

            int CustTable = 77;
            AxaptaObject axQuery3 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource3 = (AxaptaObject)axQuery3.Call("addDataSource", CustTable);

            //var temp_EmpId = RocTin.GetEmpID(DynAx, Session["user_id"].ToString() + "@posim.com.my");
            //string emplid = temp_EmpId.Item3.ToString();
            //var qbr1 = (AxaptaObject)axQueryDataSource3.Call("addRange", 30033);//EmplId
            //qbr1.Call("value", emplid);

            var qbr3 = (AxaptaObject)axQueryDataSource3.Call("addRange", 33);//state
            qbr3.Call("value", state);

            AxaptaObject axQueryRun3 = DynAx.CreateAxaptaObject("QueryRun", axQuery3);

            while ((bool)axQueryRun3.Call("next"))
            {
                AxaptaRecord DynRec3 = (AxaptaRecord)axQueryRun3.Call("Get", CustTable);
                gps = DynRec3.get_Field("GPS").ToString();
                address = DynRec3.get_Field("Address").ToString();
                name = DynRec3.get_Field("Name").ToString();
                if (gps != "" && gps != "0000")
                {
                    if (stateCoordinateList.Count <= 500)
                    {
                        stateCoordinateList.Add(new ListItem(gps, name));
                    }
                }

                DynRec3.Dispose();
            }
            return stateCoordinateList;
        }

        public List<ListItem> get_GroupCoordinate(Axapta DynAx, string CustomerMainGroup)
        {
            string gps = "";
            string address = ""; string name = "";
            List<ListItem> stateCooridinateList = new List<ListItem>();

            int CustTable = 77;
            AxaptaObject axQuery3 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource3 = (AxaptaObject)axQuery3.Call("addDataSource", CustTable);

            string tempCustGroup = GetAxCustomerMainGroupDesc(DynAx, CustomerMainGroup);

            var qbr4 = (AxaptaObject)axQueryDataSource3.Call("addRange", 30003);//CustomerMainGroup
            qbr4.Call("value", tempCustGroup);

            AxaptaObject axQueryRun3 = DynAx.CreateAxaptaObject("QueryRun", axQuery3);

            while ((bool)axQueryRun3.Call("next"))
            {
                AxaptaRecord DynRec3 = (AxaptaRecord)axQueryRun3.Call("Get", CustTable);
                gps = DynRec3.get_Field("GPS").ToString();
                address = DynRec3.get_Field("Address").ToString();
                name = DynRec3.get_Field("Name").ToString();
                if (gps != "" && gps != "0000")
                {
                    stateCooridinateList.Add(new ListItem(gps, address));
                }

                DynRec3.Dispose();
            }
            return stateCooridinateList;
        }

        public string GetAxCustomerMainGroupDesc(Axapta DynAx, string MainGroupDesc)
        {
            int MSBCustomerMainGroup = 30004;
            string customerMainGroupField = "CustomerMainGroup";
            string tempCustomerMainGroupDesc = "";

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", MSBCustomerMainGroup);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

            if ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord dynRec = (AxaptaRecord)axQueryRun.Call("Get", MSBCustomerMainGroup);
                tempCustomerMainGroupDesc = dynRec.get_Field(customerMainGroupField).ToString();

                dynRec.Dispose();
            }

            return tempCustomerMainGroupDesc;
        }

        protected void btnCust_Click(object sender, EventArgs e)
        {
            Response.Redirect("CustomerMaster.aspx");
        }

        public class CustomData
        {
            public List<string> coordinates { get; set; }
            public List<string> names { get; set; }
        }

        [WebMethod]
        public static string GetCoordinates(string selectedState)
        {
            Map3 mapPage = new Map3();

            Axapta DynAx = Function_Method.GlobalAxapta();
            List<ListItem> coordinatesList = new List<ListItem>();

            // Populate coordinatesList and nameList with data
            coordinatesList = mapPage.get_custCoordinate(DynAx, selectedState);

            List<string> coordinates = coordinatesList.Select(item => item.Text).ToList();

            // Convert coordinatesList to a list of strings (assuming Value is the name)
            List<string> names = coordinatesList.Select(item => item.Value).ToList();

            CustomData data = new CustomData
            {
                coordinates = coordinates,
                names = names,
            };

            string serializedCoordinates = JsonConvert.SerializeObject(data);

            return serializedCoordinates;
        }


        [WebMethod]
        public static string GetGroupCoordinates(string customerGroup)
        {
            Map3 mapPage = new Map3();

            Axapta DynAx = Function_Method.GlobalAxapta();
            List<ListItem> coordinatesList = new List<ListItem>();

            coordinatesList = mapPage.get_GroupCoordinate(DynAx, customerGroup);

            List<string> coordinates = coordinatesList.Select(item => item.Text).ToList();

            // Convert coordinatesList to a list of strings (assuming Value is the name)
            List<string> names = coordinatesList.Select(item => item.Value).ToList();

            CustomData data = new CustomData
            {
                coordinates = coordinates,
                names = names,
            };

            string serializedCoordinates = JsonConvert.SerializeObject(coordinates);

            return serializedCoordinates;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Session["data_passing"] = ddlState.SelectedItem.Text;
            Response.Redirect("MapCustList.aspx");

        }
    }
}
