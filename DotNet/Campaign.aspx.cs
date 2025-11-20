using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static NLog.LayoutRenderers.Wrappers.ReplaceLayoutRendererWrapper;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DotNet
{
    public partial class Campaign : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                checkDataRequest();
                LoadSalesman();
            }
        }

        private bool LoadSalesman()
        {
            Axapta DynAx = Function_Method.GlobalAxapta();
            DropDownList_Salesman.Items.Clear();
            List<ListItem> items = new List<ListItem>();

            items = SFA_GET_Enquiries_SalesmanTotal.getSalesman(DynAx);
            if (items.Count > 1)
            {
                DropDownList_Salesman.Items.AddRange(items.ToArray());
                return true;
            }
            else
            {
                Function_Method.MsgBox("There is no Salesman available.", this.Page, this);
                return false;
            }
        }

        private void checkDataRequest()
        {
            GLOBAL.data_passing = Session["data_passing"].ToString();
            if (GLOBAL.data_passing != "")
            {
                var split = GLOBAL.data_passing.Split('_');
                TextboxCustAcc.Text = split[1];
            }
            else
            {
                Button_NewCampaign_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
                lblDoc.Visible = false;
                btnDisplay.Visible = false;
            }
        }

        protected void Button_FindList_Click(object sender, EventArgs e)
        {
            Session["data_passing"] = "_CACM@";//Campaign > CustomerMaster
            Response.Redirect("CustomerMaster.aspx");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();
            try
            {
                UploadPic();

                using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LF_CampaignWebNewCust"))
                {
                    DynAx.TTSBegin();
                    DynRec.set_Field("CustAccount", TextboxCustAcc.Text);
                    DynRec.set_Field("StartDate", Convert.ToDateTime(start_date.Value));
                    DynRec.set_Field("FormStatus", 1);
                    if (fuDocument.HasFile)
                    {
                        DynRec.set_Field("Attachment", 1);
                    }
                    else
                    {
                        DynRec.set_Field("Attachment", 0);
                    }
                    var split = hdEmplId.Value.Split('(', ')');
                    DynRec.set_Field("EmplID", DropDownList_Salesman.SelectedValue);
                    DynRec.Call("insert");
                    DynAx.TTSCommit();
                    DynAx.TTSAbort();
                    Function_Method.MsgBox("New campaign saved.", this.Page, this);
                }

                DropDownList_Salesman.SelectedItem.Text = "";
            }
            catch (Exception ER_C_01)
            {
                Function_Method.MsgBox("ER_C_01: " + ER_C_01.Message, this.Page, this);
            }

        }

        protected void UploadPic()
        {
            HttpPostedFile file = Request.Files["fuDocument"];

            try
            {
                if (file.ContentLength > 0)
                {
                    string path = @"e:/Campaign/" + DropDownList_Salesman.SelectedValue + "/" + TextboxCustAcc.Text;
                    if (!Directory.Exists(@"e:/Redemption/" + DropDownList_Salesman.SelectedValue))
                    {
                        Directory.CreateDirectory(@"e:/Redemption/" + DropDownList_Salesman.SelectedValue);
                        Directory.CreateDirectory(path);
                    }
                    else
                    {
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                    }

                    MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
                    string query = "insert into lf_campaign(CustAccount,StartDate, EmplID, ImagePath) values (@c1,@c2,@c3,@c4)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFile currentFile = Request.Files[i];
                        string filename = currentFile.FileName;

                        System.Drawing.Image img = System.Drawing.Image.FromStream(currentFile.InputStream);
                        img.Save(path + "/" + filename);

                        MySqlParameter _C1 = new MySqlParameter("@c1", MySqlDbType.VarChar, 0);
                        _C1.Value = TextboxCustAcc.Text;
                        cmd.Parameters.Add(_C1);

                        MySqlParameter _C2 = new MySqlParameter("c2", MySqlDbType.VarChar, 0);
                        _C2.Value = start_date.Value;
                        cmd.Parameters.Add(_C2);

                        MySqlParameter _C3 = new MySqlParameter("@c3", MySqlDbType.VarChar, 0);
                        _C3.Value = DropDownList_Salesman.SelectedValue;
                        cmd.Parameters.Add(_C3);

                        MySqlParameter _C4 = new MySqlParameter("@c4", MySqlDbType.VarChar, 0);
                        _C4.Value = path + "/" + filename;
                        cmd.Parameters.Add(_C4);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        conn.Close();
                        cmd.Parameters.Clear();
                    }
                }
            }
            catch (Exception ER_C_02)
            {
                Function_Method.MsgBox("ER_C_02: " + ER_C_02.ToString(), this.Page, this);

                throw;
            }
        }

        private void getList()
        {
            Axapta DynAx = Function_Method.GlobalAxapta();

            try
            {
                int LF_CampaignWebNewCust = DynAx.GetTableId("LF_CampaignWebNewCust");
                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", LF_CampaignWebNewCust);

                AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                dt.Columns.Add("Customer Account");
                dt.Columns.Add("Employee ID");
                dt.Columns.Add("Start Date");
                dt.Columns.Add("Status");
                dt.Columns.Add("Attachment");

                DataRow row;

                while ((bool)axQueryRun.Call("next"))
                {
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", LF_CampaignWebNewCust);

                    row = dt.NewRow();

                    row["Customer Account"] = DynRec.get_Field("CustAccount").ToString();
                    row["Employee ID"] = DynRec.get_Field("EmplId").ToString();
                    row["Start Date"] = DynRec.get_Field("StartDate").ToString();
                    row["Status"] = DynRec.get_Field("FormStatus").ToString() == "1" ? "Submitted" :
                                    DynRec.get_Field("FormStatus").ToString() == "2" ? "Approved" : "Rejected";
                    row["Attachment"] = DynRec.get_Field("Attachment").ToString() == "1" ? "Yes" : "No";

                    dt.Rows.Add(row);
                    DynRec.Dispose();
                }

                ds.Tables.Add(dt);
                gvCampaign.DataSource = dt;
                gvCampaign.DataBind();

                gvCampaign.UseAccessibleHeader = true;
                if (gvCampaign.HeaderRow != null)
                {
                    gvCampaign.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
            catch (Exception ER_C_03)
            {
                DynAx.TTSAbort();
                Function_Method.MsgBox("ER_C_03: " + ER_C_03.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("MainMenu.aspx");

        }

        protected void Button_NewCampaign_section_Click(object sender, EventArgs e)
        {
            Button_NewCampaign_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
            Button_CampaignList_section.Attributes.Add("style", "background-color:transparent");
            dvList.Visible = false;
            dvNew.Visible = true;
            lblDoc.Visible = false;
            btnDisplay.Visible = false;
        }

        protected void Button_CampaignList_section_Click(object sender, EventArgs e)
        {
            Button_NewCampaign_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
            Button_NewCampaign_section.Attributes.Add("style", "background-color:transparent");
            dvNew.Visible = false;
            dvList.Visible = true;
            gvCampaign.Visible = true;
            getList();
        }

        protected void gvCampaign_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SelectAccount")
            {
                gvCampaign.Visible = false;
                dvNew.Visible = true;

                string[] arguments = e.CommandArgument.ToString().Split('|');
                TextboxCustAcc.Text = arguments[0];
                DropDownList_Salesman.SelectedValue = arguments[1];
                start_date.Value = Convert.ToDateTime(arguments[2]).ToString("yyyy-MM-dd");
                lblStatus.Text = arguments[3];

                if (arguments[4].ToString() == "Yes")
                {
                    GetImage(arguments[0]);
                }
            }
        }

        private void GetImage(string CustAccount)
        {
            int count = 0;
            MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
            string query = "select CustAccount, StartDate, EmplID, ImagePath from lf_campaign where CustAccount='" + CustAccount + "'";

            MySqlCommand cmd = new MySqlCommand(query, conn);

            MySqlParameter _C1 = new MySqlParameter("@c1", MySqlDbType.VarChar, 0);
            _C1.Value = CustAccount;
            cmd.Parameters.Add(_C1);

            conn.Open();

            List<ListItem> files = new List<ListItem>();
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (reader.GetValue(0).ToString() == CustAccount)
                    {
                        string img1 = reader.GetValue(3).ToString();
                        string[] arr_pathSplit = img1.Split('/');

                        var filePath = GLOBAL.externalServerIP + "/Images/" + arr_pathSplit[1] + "/" + arr_pathSplit[2] + "/"
                            + arr_pathSplit[3] + "/" + arr_pathSplit[4]; //for external live
                        files.Add(new ListItem(arr_pathSplit[4], filePath));

                        repeater.DataSource = files;
                        repeater.DataBind();
                        count++;
                    }
                }
            }

            if (count > 0)
            {
                lblDisplay.Visible = false;
                fuDocument.Visible = false;
                display_section.Visible = true;
                dvUpload.Visible = false;
                lblDoc.Visible = true;
                btnDisplay.Visible = true;
            }
            else
            {
                lblDisplay.Visible = true;
                fuDocument.Visible = true;
                lblDoc.Visible = false;
                btnDisplay.Visible = false;
            }
        }

    }
}