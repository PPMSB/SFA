using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using MySql.Data.MySqlClient;
using System;
using System.Web.UI.HtmlControls;

namespace DotNet
{
    public partial class CheckIn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TimeOutRedirect();
            if (!IsPostBack)
            {
                clear_variable();
                //~/CheckIn.aspx?location=
                string location = Request.QueryString["location"];
                if (location != null)
                {
                    Label_Location.Text = location;
                    Button_Confirm.Visible = true;
                }
                else
                {
                    Function_Method.MsgBox("Please Rescan QR code.", this.Page, this);
                    Button_Confirm.Visible = false;
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
            TextBox_Name.Text = "";
            TextBox_Phone.Text = "";
            Label_Location.Text = "";
        }
        protected void Button_Confirm_click(object sender, EventArgs e)
        {
            string Error_MySQL = SaveToMySQL();
            if (Error_MySQL == "")//no error
            {
                Function_Method.MsgBox("Thank you for using Lion POSIM Check-In!", this.Page, this);
                clear_variable();
                Button_Confirm.Visible = false;
            }
            else
            {
                Function_Method.MsgBox(Error_MySQL, this.Page, this);
            }
        }

        private string SaveToMySQL()
        {
            string error = "";
            try
            {
                MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
                string Query;

                Query = "insert into check_in_tbl(Name,PhoneNumber,QRlocation,DateIn,TimeIn,Reserve1,Reserve2) values(@D1,@D2,@D3,@D4,@D5,@D6,@D7)";
                MySqlCommand cmd = new MySqlCommand(Query, conn);

                //1:name
                MySqlParameter _D1 = new MySqlParameter("@D1", MySqlDbType.VarChar, 0);
                _D1.Value = TextBox_Name.Text.Trim();
                cmd.Parameters.Add(_D1);
                //2:PhoneNumber,   
                MySqlParameter _D2 = new MySqlParameter("@D2", MySqlDbType.VarChar, 0);
                _D2.Value = TextBox_Phone.Text.Trim();
                cmd.Parameters.Add(_D2);
                //3:QRlocation,
                MySqlParameter _D3 = new MySqlParameter("@D3", MySqlDbType.VarChar, 0);
                _D3.Value = Label_Location.Text;
                cmd.Parameters.Add(_D3);
                //4:Date in
                MySqlParameter _D4 = new MySqlParameter("@D4", MySqlDbType.VarChar, 0);
                _D4.Value = DateTime.Now.ToString("dd/MM/yyyy");
                cmd.Parameters.Add(_D4);
                //5:Time in
                MySqlParameter _D5 = new MySqlParameter("@D5", MySqlDbType.VarChar, 0);
                _D5.Value = DateTime.Now.ToString("hh:mm:ss");
                cmd.Parameters.Add(_D5);
                //Reserve1
                MySqlParameter _D6 = new MySqlParameter("@D6", MySqlDbType.VarChar, 0);
                _D6.Value = "";
                cmd.Parameters.Add(_D6);

                //Reserve2
                MySqlParameter _D7 = new MySqlParameter("@D7", MySqlDbType.VarChar, 0);
                _D7.Value = "";
                cmd.Parameters.Add(_D7);

                conn.Open(); cmd.ExecuteNonQuery();
                conn.Close();
                return error;
            }
            catch (Exception ER_CI_00)
            {
                error = "ER_CI_00: " + ER_CI_00.ToString();
                return error;
            }
        }
    }
}