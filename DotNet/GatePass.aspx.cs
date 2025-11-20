using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Web.UI.HtmlControls;

namespace DotNet
{
    public partial class GatePass : System.Web.UI.Page
    {
        bool exist;
        string gatepassno;
        string gatepassouttime;

        protected void Page_Load(object sender, EventArgs e)
        {
            //TimeOutRedirect();
            if (!IsPostBack)
            {
                clear_variable();
                //~/GatePass.aspx?gatepassno=
                checkGatepass();
                gatepassno = Request.QueryString["gatepassno"];
                //Label_gatepassno.Text = SFA_GET_GATEPASS.get_Gatepass(DynAx, gatepassno);
                if (gatepassno != null && exist == false)
                {
                    Label_gatepassno.Text = "Gatepass No: " + gatepassno;
                    //Label_gatepassno.Text = DateTime.Now.ToString("HH:mm:ss tt");
                    //Label_gatepassno.Text = DateTime.Now.ToString("d/M/yyyy");
                    Button_Checkout.Visible = true;
                }
                else if (gatepassno != null && exist == true)
                {
                    Label_gatepassno.Text = "Gatepass No: " + gatepassno + " already been scanned on " + gatepassouttime;
                }
                else
                {
                    Function_Method.MsgBox("No gatepass number detected.", this.Page, this);
                    Label_gatepassno.Text = "No gatepass number detected.";
                    Button_Checkout.Visible = false;
                    Button_Cancel.Visible = false;
                }
            }
        }

        private void TimeOutRedirect()
        {
            HtmlMeta meta = new HtmlMeta();

            meta.HttpEquiv = "Refresh";

            meta.Content = Convert.ToString(Session.Timeout * 60) + ";url=LoginPage.aspx";

            this.Page.Header.Controls.Add(meta);
        }
        private void clear_variable()
        {
            //Label_gatepassno.Text = "";
        }
        protected void Button_Checkout_click(object sender, EventArgs e)
        {
            //string Error_MySQL = SaveToMySQL();
            //if (Error_MySQL == "")//no error
            //{
            //    Function_Method.MsgBox("Thank you for using Lion POSIM Check-In!", this.Page, this);
            //    clear_variable();
            Axapta DynAx = new Axapta();

            //add new group
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.AdminID2, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);
            try
            {
                using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LF_Conso_Gatepass"))
                {
                    //Debug.WriteLine("exist", exist.ToString());
                    if (exist == false)
                    {
                        DynRec.set_Field("Gatepass", Request.QueryString["gatepassno"]);
                        //DynRec.set_Field("GatepassDate", Label_gatepassno.Text);
                        var today_datetime = DateTime.Now.ToString("d/M/yyyy hh:mm:ss tt"); //var temp_Today = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        //var today_time = DateTime.Now.ToString("hh:mm:ss tt");
                        //var today_date1 = Function_Method.get_correct_date(GLOBAL.system_checking, today_date, false);
                        //var today_time1 = Function_Method.get_correct_date(GLOBAL.system_checking, today_time, false);
                        var temp_today_date = DateTime.ParseExact(today_datetime, "d/M/yyyy hh:mm:ss tt", null);
                        //var temp_today_time = DateTime.ParseExact(today_time, "hh:mm:ss tt", CultureInfo.InvariantCulture);
                        //string s = temp_today_time.ToString().Split(' ')[1] + ' ' + temp_today_time.ToString().Split(' ')[2];
                        //Debug.WriteLine("temp_today_date", temp_today_date);
                        //Debug.WriteLine("temp_today_time", temp_today_time.TimeOfDay.ToString());
                        DynRec.set_Field("GatepassOutDate", temp_today_date);
                        //double timestamp = Stopwatch.GetTimestamp();
                        //long seconds = DateTimeOffset.Now.ToUnixTimeSeconds();//Now.ToUnixTimeSeconds(); //timestamp / Stopwatch.Frequency;

                        // time in seconds
                        TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1));
                        //Debug.WriteLine("secsec1", seconds.TotalSeconds.ToString());
                        double totalseconds = ts.TotalSeconds;
                        TimeSpan time = TimeSpan.FromSeconds(totalseconds); //  
                        string temp_today_time = new DateTime(time.Ticks).ToLocalTime().ToString("HH:mm:ss");
                        //Debug.WriteLine("secsec", temp_today_time);
                        //Debug.WriteLine("dayday", temp_today_date.ToShortDateString());
                        //var time = DateTime.ParseExact(s, "hh:mm:ss tt", null);
                        //var a = new DateTime(ts.Ticks).ToLocalTime();
                        //var b = a.ToLongTimeString();
                        //Debug.WriteLine("secsec1", temp_today_time.ToShortTimeString());                       
                        DynRec.set_Field("GatepassOutTime", temp_today_time); //temp_today_date

                        DynRec.Insert();
                        exist = true;
                        Label_gatepassno.Text = "Gatepass No: " + Request.QueryString["gatepassno"] + " successfully checked out.";
                    }
                    else
                    {
                        Label_gatepassno.Text = "Gatepass No: " + gatepassno + " already been scanned on ";
                    }
                }
            }
            catch (Exception ex)
            {
                Function_Method.MsgBox("Error: " + ex.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
            Button_Checkout.Visible = false;
            Button_Cancel.Visible = false;
            //}
            //else
            //{
            //    Function_Method.MsgBox(Error_MySQL, this.Page, this);
            //}
        }
        protected void Button_Cancel_click(object sender, EventArgs e)
        {
            //string Error_MySQL = SaveToMySQL();
            //if (Error_MySQL == "")//no error
            //{
            Function_Method.MsgBox("User clicked Cancel.", this.Page, this);
            //    clear_variable();
            Button_Checkout.Visible = false;
            Button_Cancel.Visible = false;
            //}
            //else
            //{
            //    Function_Method.MsgBox(Error_MySQL, this.Page, this);
            //}
        }

        private bool checkGatepass()
        {
            Axapta DynAx = new Axapta();
            AxaptaRecord DynRec;
            try
            {
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.AdminID2, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);
                //DynAx.LogonAs(GLOBAL.security_user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                //string gatepassno = Request.QueryString["gatepassno"];
                DynRec = DynAx.CreateAxaptaRecord("LF_Conso_Gatepass");
                //DynRec.ExecuteStmt("select * from %1 ");
                string stmt = string.Format("select * from %1 where %1.{0} == '{1}'", "Gatepass", Request.QueryString["gatepassno"]);
                DynRec.ExecuteStmt(stmt);
                //Function_Method.MsgBox("ST: ", this.Page, this);
                while (DynRec.Found)
                {
                    gatepassno = DynRec.get_Field("Gatepass").ToString();
                    string gatepassOutDate = DynRec.get_Field("GatepassOutDate").ToString();
                    if (gatepassOutDate != "")
                    {
                        string[] arr_getDate = gatepassOutDate.Trim().Split(' ');//date + " " + time;
                        gatepassOutDate = arr_getDate[0];
                    }
                    // check by int
                    //int gatepassOutTime = (int) DynRec.get_Field("GatepassOutTime"); 
                    //TimeSpan ts = TimeSpan.FromSeconds(gatepassOutTime); //  1686630150.39465
                    //gatepassouttime = gatepassOutDate + " " + new DateTime(ts.Ticks).ToString("hh:mm:ss tt").ToString();

                    // check by string
                    string gatepassOutTime = DynRec.get_Field("GatepassOutTime").ToString();
                    gatepassouttime = gatepassOutDate + " " + gatepassOutTime;
                    exist = true;
                    Button_Checkout.Visible = false;
                    Button_Cancel.Visible = false;
                    DynRec.Next();
                }
                DynRec.Dispose();
                //SFA_GET_GATEPASS.get_Gatepass(DynAx, gatepassno);

                return true;

            }
            catch (Exception ex)
            {
                Function_Method.MsgBox("Error: " + ex.ToString(), this.Page, this);
                Button_Checkout.Visible = false;
                Button_Cancel.Visible = false;
                return false;
            }
            finally
            {
                DynAx.Logoff();
            }
        }
    }
}