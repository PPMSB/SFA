using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DotNet
{
    public partial class CheckInRecord : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            check_session();
            TimeOutRedirect();

            if (!IsPostBack)
            {
                clear_variable();
                Function_Method.LoadSelectionMenu(GLOBAL.module_access_authority,
                    null, CustomerMasterTag2,
                    null, SFATag2,
                    null, SalesQuotation2,
                    null, PaymentTag2,
                    null, RedemptionTag2,
                    null, InventoryMasterTag2,

                    null, EORTag2,
                    null, null,
                    null, null,
                    null, null,
                    null, null,
                    null, null, null, null, null
                    );
                SetFocus(TextBox_SearchItem);
                TextBox_SearchItem.Attributes.Add("autocomplete", "off");

                //first time, reload gridview with Search ALL approach
                GridView1.PageIndex = 0;
                f_call_MySQL();
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
                //
            }
            catch
            {
                Response.Redirect("LoginPage.aspx");
            }
        }
        private void clear_variable()
        {
            GridView1.DataSource = null;
            GridView1.DataBind();

        }

        protected void CheckItem(object sender, EventArgs e)
        {
            clear_variable();
            GridView1.PageIndex = 0;
            f_call_MySQL();
        }

        //=====================================================================================

        private void f_call_MySQL()
        {
            //Button_Delete.Enabled = false;
            MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);

            string search_field = "%" + TextBox_SearchItem.Text.Trim() + "%";
            string search_criteria = DropDownList1.SelectedItem.Text;


            try
            {
                string Query;
                if (search_field != "%%")
                {
                    Query = "select check_in_key,Name,PhoneNumber,QRlocation,DateIn,TimeIn from check_in_tbl where " + search_criteria + " like'" + search_field + "' " + "order by check_in_key Desc";

                }
                else
                {
                    Query = "select check_in_key,Name,PhoneNumber,QRlocation,DateIn,TimeIn from check_in_tbl order by check_in_key Desc";

                }
                MySqlCommand cmd = new MySqlCommand(Query, conn);
                conn.Open();
                DataSet ds = new DataSet();

                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    da.Fill(ds);
                    GridView1.DataSource = ds;
                    GridView1.DataBind();
                    GridView_RowDataBound_();
                }

                conn.Close();

            }
            catch (Exception ER_CR_00)
            {
                Function_Method.MsgBox("ER_CR_00: " + ER_CR_00.ToString(), this.Page, this);
            }
        }
        protected void GridView_RowDataBound_Header(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.Header)
            {

                e.Row.Cells[0].Text = "No";
                e.Row.Cells[1].Text = "Name";
                e.Row.Cells[2].Text = "Phone No.";
                e.Row.Cells[3].Text = "Location";
                e.Row.Cells[4].Text = "Date In";
                e.Row.Cells[5].Text = "Time In";
            }

        }
        private void GridView_RowDataBound_()
        {
            int row_count = GridView1.Rows.Count;


            for (int i = 0; i < row_count; i++)
            {
                if (GridView1.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    GridView1.Rows[i].Cells[0].Text = (i + 1).ToString();
                }
            }
        }

    }
}