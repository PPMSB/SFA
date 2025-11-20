using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using MySql.Data.MySqlClient;
using System;
using System.DirectoryServices;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


namespace DotNet
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //check_session();
            TimeOutRedirect();
            //if (!IsPostBack)
            //{
            //    clear_variable();
            //}

            if (!IsPostBack)
            {
                if (Request.QueryString["expired"] == "true")
                {
                    Function_Method.MsgBox("Password expired soon, please change your password now.", this.Page, this);
                }

                // Set hidden fields with ClientIDs
                Hidden_CurrentPasswordId.Value = TextBox_CurrentPassword.ClientID;
                Hidden_NewPasswordId.Value = TextBox_NewPassword.ClientID;
                Hidden_ConfirmPasswordId.Value = TextBox_ConfirmNewPassword.ClientID;

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
            Response.Redirect("LoginPage.aspx");
        }

        protected void Button_Confirm_Click(object sender, EventArgs e)
        {
            string userId = TextBox_UserId.Text.Trim().ToLower();
            string currentPw = TextBox_CurrentPassword.Text.Trim(); // Trim for safety
            string newPw = TextBox_NewPassword.Text.Trim();
            string confirmPw = TextBox_ConfirmNewPassword.Text.Trim();

            // Null/empty checks (basic required validation)
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(currentPw) || string.IsNullOrEmpty(newPw) || string.IsNullOrEmpty(confirmPw))
            {
                Function_Method.MsgBox("All fields are required. Please fill in your User ID and passwords.", this.Page, this);
                clear_variable(); // Clear on error to reset form
                return;
            }

            // Check: New password not the same as current
            if (newPw.Equals(currentPw, StringComparison.OrdinalIgnoreCase))
            {
                Function_Method.MsgBox("New password cannot be the same as your current password.", this.Page, this);
                clear_variable();
                return;
            }

            // Check: New vs Confirm match
            if (newPw != confirmPw)
            {
                Function_Method.MsgBox("New password entered is not the same when reenter. Please confirm your new password again.", this.Page, this);
                clear_variable();
                return;
            }

            // Check: Minimum length (server-side enforcement)
            if (newPw.Length < 12)
            {
                Function_Method.MsgBox("New password must be at least 12 characters long.", this.Page, this);
                clear_variable();
                return;
            }

            // Check: Complexity (1 uppercase, 1 lowercase, 1 number) using regex
            if (!System.Text.RegularExpressions.Regex.IsMatch(newPw, @"[A-Z]") ||
                !System.Text.RegularExpressions.Regex.IsMatch(newPw, @"[a-z]") ||
                !System.Text.RegularExpressions.Regex.IsMatch(newPw, @"\d"))
            {
                Function_Method.MsgBox("New password must contain at least one uppercase letter (A-Z), one lowercase letter (a-z), and one number (0-9).", this.Page, this);
                clear_variable();
                return;
            }

            // Check: Not containing User ID (improved: substring + your intersect logic)
            if (newPw.ToLower().Contains(userId) || // Direct substring check (stricter)
                (newPw.ToCharArray().Intersect(userId.ToCharArray()).Count() > 4)) // Your original overlap check as fallback
            {
                Function_Method.MsgBox("New password entered should not contain any user ID or combination of it.", this.Page, this);
                clear_variable();
                return;
            }

            // Check: Not containing full name or parts (your custom method)
            string fullUserName = GetLoggedInUser_NameWithPasswordValidation(userId, newPw); // Pass userId and newPw as params (adjust if needed)
            if (string.IsNullOrEmpty(fullUserName)) // Assuming empty = invalid (contains name parts)
            {
                Function_Method.MsgBox("New password cannot contain your full name or any part of it (e.g., first/last name). Please choose a different password.", this.Page, this);
                clear_variable();
                return;
            }

            // All validations passed - Proceed with AD password change
            try
            {
                using (DirectoryEntry de = GetUser())
                {
                    if (de != null)
                    {
                        // AD ChangePassword: Validates currentPw implicitly; throws if invalid
                        de.Invoke("ChangePassword", new object[] { currentPw, newPw });
                        de.CommitChanges();

                        clear_variable();
                        Function_Method.MsgBox("Password has been reset successfully.", this.Page, this);
                        Response.Redirect("LoginPage.aspx", false); // false to prevent double postback
                    }
                    else
                    {
                        Function_Method.MsgBox("Unable to access user account. Please contact support.", this.Page, this);
                        clear_variable();
                    }
                }
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                // Handle specific AD errors (e.g., "old password incorrect" or policy violation)
                string errorMsg = "Failed to change password: " + ex.Message;
                if (ex.Message.Contains("incorrect") || ex.Message.Contains("wrong")) // Common AD error patterns
                {
                    errorMsg = "Current password is incorrect. Please try again.";
                }
                else if (ex.Message.Contains("policy") || ex.Message.Contains("complexity"))
                {
                    errorMsg = "Password does not meet domain policy requirements. Please choose a stronger password.";
                }
                Function_Method.MsgBox("ER_CP_00: " + errorMsg, this.Page, this);
                clear_variable(); // Clear on AD error too
            }
            catch (Exception ex) // Catch other unexpected errors (e.g., network/AD connection)
            {
                Function_Method.MsgBox("An unexpected error occurred: " + ex.Message, this.Page, this);
                clear_variable();
            }
        }


        public static string GetCurrentDomainPath()
        {

            // First, try the fallback using Environment.UserDomainName
            string primaryDnsSuffix = Environment.UserDomainName;
            string username = Environment.UserName;
            //string password = Environment.Password;

            if (!string.IsNullOrEmpty(primaryDnsSuffix) && !string.Equals(primaryDnsSuffix, Environment.MachineName, StringComparison.OrdinalIgnoreCase))
            {
                Function_Method.AddLog("Using fallback for domain path.");
                return "LDAP://" + primaryDnsSuffix;  // e.g., "LDAP://lionpb.com.my"
            }
            // If fallback fails, proceed to try the LDAP query
            try
            {
                using (DirectoryEntry de = new DirectoryEntry("LDAP://RootDSE"))
                {
                    if (de.Properties["defaultNamingContext"].Count > 0)
                    {
                        Function_Method.AddLog("Successfully retrieved domain path via LDAP.");
                        return "LDAP://" + de.Properties["defaultNamingContext"][0].ToString();
                    }
                    else
                    {
                        Function_Method.AddLog("defaultNamingContext property not found.");
                        return null; // Or another appropriate value indicating failure
                    }
                }
            }
            catch (System.Runtime.InteropServices.COMException comEx)
            {
                Function_Method.AddLog($"COMException: {comEx.Message} (Error Code: {comEx.ErrorCode})");
                return null; // Or another appropriate value indicating failure
            }
            catch (Exception ex)
            {
                Function_Method.AddLog($"Exception: {ex.Message}\nStack Trace: {ex.StackTrace}");
                return null; // Or another appropriate value indicating failure
            }
            // If everything fails, return null
            Function_Method.AddLog("Failed to get domain path via both fallback and LDAP.");
            return null;
        }

        private DirectoryEntry GetUser()
        {
            string UserId = TextBox_UserId.Text.Trim();
            DirectoryEntry de = new DirectoryEntry(GetCurrentDomainPath());
            DirectorySearcher deSearch = new DirectorySearcher();
            deSearch.SearchRoot = de;

            deSearch.Filter = "(&(objectClass=user)(SAMAccountName=" + UserId + "))";
            deSearch.SearchScope = SearchScope.Subtree;
            SearchResult results = deSearch.FindOne();

            if (!(results == null))
            {
                de = new DirectoryEntry(results.Path, UserId, TextBox_CurrentPassword.Text, AuthenticationTypes.Secure);
                //de = new DirectoryEntry(results.Path);
                return de;
            }
            else
            {
                return null;
            }
        }

        #region Neil - Check password not contain any user name
        public static string GetLoggedInUser_NameWithPasswordValidation(string UserId, string Password)
        {
            if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(Password))
                return string.Empty; // Early exit for invalid input

            string fullUser_Name = string.Empty;
            MySqlConnection conn = null;
            try
            {
                conn = new MySqlConnection(GLOBAL.connStr);
                string Query = "SELECT * FROM user_tbl WHERE user_id = @D1 LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(Query, conn);

                MySqlParameter _D1 = new MySqlParameter("@D1", MySqlDbType.VarChar, 255);
                _D1.Value = UserId.Trim().ToLowerInvariant();
                cmd.Parameters.Add(_D1);

                conn.Open();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (reader.GetValue(0) != DBNull.Value && // Basic null check
                            reader.GetValue(2) != DBNull.Value &&
                            reader.GetValue(2).ToString().Trim() == UserId.Trim()) // Security check
                        {
                            if (reader.GetValue(1) != DBNull.Value)
                            {
                                fullUser_Name = reader.GetValue(1).ToString().Trim();

                                // Now validate password against full name and its parts
                                if (IsPasswordValid(fullUser_Name, Password))
                                {
                                    return fullUser_Name; // Valid: Return the full user name
                                }
                                else
                                {
                                    // Invalid: Log for security/audit (adjust logger as needed)
                                    System.Diagnostics.Debug.WriteLine($"Password validation failed for UserId '{UserId}': Password contains name parts from '{fullUser_Name}'.");
                                    // Or use Console.WriteLine, or a proper logger like NLog
                                    return string.Empty; // Invalid password rule
                                }
                            }
                        }
                    }
                }

                return string.Empty; // No user found
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetLoggedInUser NameWithPasswordValidation: {ex.Message}");
                return string.Empty; // Graceful fallback
            }
            finally
            {
                conn?.Close();
                conn?.Dispose();
            }
        }

        /// <summary>
        /// Helper method: Checks if password does NOT contain the full name or any of its parts.
        /// </summary>
        /// <param name="fullName">e.g., "Allen Choo Foo Kin"</param>
        /// <param name="password">The password to validate</param>
        /// <returns>True if valid (no name parts found), false otherwise</returns>
        private static bool IsPasswordValid(string fullName, string password)
        {
            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(password))
            {
                System.Diagnostics.Debug.WriteLine("IsPasswordValid: Early return true due to empty input.");
                return true; // Edge case - but shouldn't happen
            }


            // Normalize: Trim and lowercase both for case-insensitive check
            string normalizedPassword = password.Trim().ToLowerInvariant();
            string normalizedFullName = fullName.Trim().ToLowerInvariant();

            System.Diagnostics.Debug.WriteLine($"IsPasswordValid Debug - FullName: '{fullName}' -> Normalized: '{normalizedFullName}'");
            System.Diagnostics.Debug.WriteLine($"IsPasswordValid Debug - Password: '{password}' -> Normalized: '{normalizedPassword}'");

            // Check full name as substring
            bool fullMatch = normalizedPassword.Contains(normalizedFullName);
            System.Diagnostics.Debug.WriteLine($"Full name check: Contains '{normalizedFullName}'? {fullMatch}");
            if (fullMatch)
            {
                System.Diagnostics.Debug.WriteLine("Invalid: Full name match found.");
                return false; // Invalid: Contains full name
            }

            // Split full name into parts (words separated by spaces)
            string[] nameParts = normalizedFullName.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            System.Diagnostics.Debug.WriteLine($"Name parts: [{string.Join(", ", nameParts)}]");



            // Check full name as substring
            if (normalizedPassword.Contains(normalizedFullName))
            {
                return false; // Invalid: Contains full name
            }


            // Check each part as substring in password
            foreach (string part in nameParts)
            {
                bool partMatch = !string.IsNullOrEmpty(part) && normalizedPassword.Contains(part);
                System.Diagnostics.Debug.WriteLine($"Part '{part}' in password? {partMatch} (at index: {normalizedPassword.IndexOf(part)})");
                if (partMatch)
                {
                    System.Diagnostics.Debug.WriteLine($"Invalid: Name part '{part}' found in password.");
                    return false; // Invalid: Contains a name part (e.g., "allen" or "choo")
                }
            }
            System.Diagnostics.Debug.WriteLine("Valid: No matches found.");
            return true; // Valid: No matches found
        }

        #endregion
    }
}