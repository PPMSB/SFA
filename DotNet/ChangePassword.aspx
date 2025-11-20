<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="DotNet.ChangePassword" %>

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
    <style>
        .password-container {
      position: relative;
      display: inline-block;
      width: 100%; /* Or your desired width */
  }
  .password-container .inputtext_3 {
      width: 100%;
      padding-right: 40px; /* Space for the button */
  }
  .password-container .toggle-password {
      position: absolute;
      right: 10px;
      top: 50%;
      transform: translateY(-50%);
      background: none;
      border: none;
      cursor: pointer;
      color: #999;
      padding: 5px;
  }
  .password-container .toggle-password:hover {
      color: #333;
  }
  .password-container .toggle-password i {
      font-size: 16px;
  }
    </style>
<script type="text/javascript">
    function togglePassword(hiddenClientId) {

        var textBoxClientId = document.getElementById(hiddenClientId).value;
        var textBox = document.getElementById(textBoxClientId);
        if (!textBox) {
            console.error('TextBox not found: ' + textBoxClientId); // Debug in F12 Console
            return;
        }

        var button = event.target.closest('button');
        if (!button) return;

        var icon = button.querySelector('i');
        if (!icon) {
            console.error('Icon not found in button'); // Debug
            return;
        }

        // Toggle input type and icon classes (safer: doesn't overwrite other classes)
        if (textBox.type === 'password') {
            textBox.type = 'text';
            icon.classList.remove('fa-eye-slash');
            icon.classList.add('fa-eye');
            console.log('Password visible'); // Debug: Remove after testing
        } else {
            textBox.type = 'password';
            icon.classList.remove('fa-eye');
            icon.classList.add('fa-eye-slash');
            console.log('Password hidden'); // Debug
        }

        // Force icon visibility (in case of CSS conflicts)
        icon.style.visibility = 'visible';
        icon.style.opacity = '1';
        icon.style.display = 'inline-block';
    }

</script>

</head>
<body>
    <div class="container">
        <img src="RESOURCES/user.png" class="avatar" />
        <form id="idChangePassword" runat="server">
            <!-- Hidden Fields for ClientIDs (Fixes the Exception) -->
            <asp:HiddenField ID="Hidden_CurrentPasswordId" runat="server" />
            <asp:HiddenField ID="Hidden_NewPasswordId" runat="server" />
            <asp:HiddenField ID="Hidden_ConfirmPasswordId" runat="server" />
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
                    <p>&#x2713 Not contain full name or any part of (first/last) name</p>
                </div>

                <div class="col">

                    <asp:TextBox ID="TextBox_UserId" autocomplete="off" class="inputtext_3" placeholder="Domain User ID  (exp: abuhm)" required="required" runat="server" MaxLength="13" oninput="this.value = this.value.toLowerCase();"></asp:TextBox>
<div class="password-container">
    <asp:TextBox ID="TextBox_CurrentPassword" type="password" class="inputtext_3" placeholder="Current Password" required="required" runat="server"></asp:TextBox>
    <button type="button" class="toggle-password" onclick="togglePassword('Hidden_CurrentPasswordId')" aria-label="Toggle current password visibility">
        <i class="fa fa-eye-slash" aria-hidden="true"></i>
    </button>
</div>

<div class="password-container">
    <asp:TextBox ID="TextBox_NewPassword" type="password" class="inputtext_3" placeholder="New Password" required="required" runat="server" minlength="12"></asp:TextBox>
    <button type="button" class="toggle-password" onclick="togglePassword('Hidden_NewPasswordId')" aria-label="Toggle new password visibility">
        <i class="fa fa-eye-slash" aria-hidden="true"></i>
    </button>
</div>

<div class="password-container">
    <asp:TextBox ID="TextBox_ConfirmNewPassword" type="password" class="inputtext_3" placeholder="Re-Enter New Password" required="required" runat="server" minlength="12"></asp:TextBox>
    <button type="button" class="toggle-password" onclick="togglePassword('Hidden_ConfirmPasswordId')" aria-label="Toggle confirm password visibility">
        <i class="fa fa-eye-slash" aria-hidden="true"></i>
    </button>
</div>
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
