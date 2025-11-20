<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Visitor_ChangePassword.aspx.cs" Inherits="DotNet.Visitor_ChangePassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="shortcut icon" href="RESOURCES/LFIB_icon.ico" />

    <script src="scripts/GoToTab.js"></script>
    <link href="STYLES/ChangePassword.css" rel="stylesheet" />


    <title>Change Password</title>
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="mobile-web-app-capable" content="yes" />

    <script src="scripts/BrowserReload_ThroughHistory.js"></script>

</head>
<body>
    <div class="container">
        <img src="RESOURCES/user.png" class="avatar" />
        <form id="idChangePassword" runat="server">
            <div class="row">
                <h2 style="text-align: center">Change Password</h2>
                <div class="vl">
                </div>

                <div class="col">
                    <h3>New password must:</h3>
                    <p>&#10003 At least 12 characters</p>
                    <p>&#x2713 At least 1 upper case letter (A-Z)</p>
                    <p>&#x2713 At least 1 lower case letter (a-z)</p>
                    <p>&#x2713 At least 1 number (0-9)</p>
                    <p>&#x2713 Not same as Old password</p>
                    <p>&#x2713 Not contain User Id as password combination</p>
                </div>

                <div class="col">

                    <asp:TextBox ID="TextBox_UserId" autocomplete="off" class="inputtext_3" placeholder="Domain User ID" required="required" runat="server" MaxLength="13"></asp:TextBox>
                    <asp:TextBox ID="TextBox_CurrentPassword" type="password" class="inputtext_3" placeholder="Current Password" required="required" runat="server"></asp:TextBox>
                    <asp:TextBox ID="TextBox_NewPassword" type="password" class="inputtext_3" placeholder="New Password" required="required" runat="server" Minlength="12"></asp:TextBox>
                    <asp:TextBox ID="TextBox_ConfirmNewPassword" type="password" class="inputtext_3" placeholder="Re-Enter New Password" required="required" runat="server" Minlength="12"></asp:TextBox>

                    <asp:Button ID="Button_Confirm" runat="server" OnClick="Button_Confirm_Click" Text="Reset Password" class="ConfirmButton" />

                    <asp:Button ID="Button_Cancel" runat="server" OnClick="Button_Cancel_Click" UseSubmitBehavior="false" CausesValidation="false" Text="Cancel" class="CancelButton" />

                </div>


            </div>
        </form>
    </div>
<%--                <div class="footer">
            <p>
                &copy;
                <script>document.write(new Date().getFullYear())</script>
                LION POSIM BERHAD 198201002310 (82056-X) All Rights Reserved.
            </p>
        </div>--%>

</body>
</html>
