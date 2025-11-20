using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DotNet
{
    public partial class Visitor_ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //check_session();
            TimeOutRedirect();
            if (!IsPostBack)
            {
                clear_variable();
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
            TextBox_UserId.Text = "";
            TextBox_CurrentPassword.Text = "";
            TextBox_NewPassword.Text = "";
            TextBox_ConfirmNewPassword.Text = "";
        }

        protected void Button_Cancel_Click(object sender, EventArgs e)
        {
            clear_variable();
            Response.Redirect("Visitor_Login.aspx");
        }

        protected void Button_Confirm_Click(object sender, EventArgs e)
        {
            if (TextBox_NewPassword.Text != TextBox_ConfirmNewPassword.Text)
            {
                Function_Method.MsgBox("New password entered is not the same when reenter. Please confirm your new password again.", this.Page, this);
                return;
            }

            string String1 = TextBox_NewPassword.Text;
            string String2 = TextBox_UserId.Text;

            var result = String1.ToCharArray().Intersect(String2.ToCharArray()).ToList();
            if (result.Count > 4)
            {
                Function_Method.MsgBox("New password entered should not contain any user id.", this.Page, this);
                return;
            }

            try
            {
                bool IsUpdated = UpdatePassword();

                if (IsUpdated)
                {
                    clear_variable();
                    Function_Method.MsgBox("Password have been reset.", this.Page, this);

                    Response.Redirect("Visitor_Login.aspx?changedpassword=true");
                }
                else
                {
                    Function_Method.MsgBox("Incorrect user or password.", this.Page, this);
                }

            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                Function_Method.MsgBox("ER_CP_00:  {ex.Message}", this.Page, this);
                throw;
            }
        }

        private bool UpdatePassword()
        {
            string UserName = TextBox_UserId.Text.Trim();
            string Password = TextBox_CurrentPassword.Text;
            string UpdatedPassword = TextBox_NewPassword.Text;

            using (MySqlConnection connection = new MySqlConnection(GLOBAL.connStr))
            {
                connection.Open();

                string GetUser = "select UserName from visitor_userlogin where UserName = @p1 and UserPassword = @p0";

                MySqlCommand cmd = new MySqlCommand(GetUser, connection);
                cmd.Parameters.AddWithValue("@p0", Password);
                cmd.Parameters.AddWithValue("@p1", UserName);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.GetValue(0) != DBNull.Value)
                        {
                            connection.Close();
                            connection.Open();

                            string Query = "update visitor_userlogin set UserPassword = @p0, IsUpdatedPassword = @p2 where UserName = @p1";
                            MySqlCommand cmdUpdate = new MySqlCommand(Query, connection);
                            cmdUpdate.Parameters.AddWithValue("@p0", UpdatedPassword);
                            cmdUpdate.Parameters.AddWithValue("@p1", UserName);
                            cmdUpdate.Parameters.AddWithValue("@p2", true);

                            cmdUpdate.ExecuteNonQuery();
                            connection.Close();

                            return true;
                        }
                    }
                    connection.Close();

                    return false;
                }
            }
        }
    }
}