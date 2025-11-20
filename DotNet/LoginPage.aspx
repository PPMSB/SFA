<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginPage.aspx.cs" Inherits="DotNet.Login_Page" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="shortcut icon" href="RESOURCES/LFIB_icon.ico" />

    <link href="STYLES/Login.css" rel="stylesheet" />
    <title>Login Page</title>
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="mobile-web-app-capable" content="yes" />
    <script src="scripts/BrowserReload_ThroughHistory.js"></script>
    <link rel="stylesheet" href="STYLES/font-awesome.min.css" />
    <link rel="stylesheet" href="STYLES/bootstrap.min.css" media="screen" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>


    <%--    <style>
        body {
            background-image: url("RESOURCES/bgw1.jpg");
        }
    </style>--%>
    <script type="text/javascript">
        const form = document.getElementById('idLoginPAge');
        const username = document.getElementById('username');
        const password = document.getElementById('password');
        const loginBtn = document.getElementById('BtnLogin');
        if (loginBtn) {
            loginBtn.addEventListener('click', function (event) {
                event.preventDefault();
                if (username.value === '' || password.value === '') {
                    alert('Please fill in all fields!');
                } else {
                    alert(`Welcome ${username.value}!`);
                    form.reset();
                }
            });
        }

        document.addEventListener("DOMContentLoaded", function () {
            var passwordInput = document.getElementById("TextBox2");
            var togglePasswords = document.querySelectorAll(".toggle-password");
            for (var i = 0; i < togglePasswords.length; i++) {
                togglePasswords[i].addEventListener("click", function () {
                    if (passwordInput.type === "password") {
                        passwordInput.type = "text";
                        this.textContent = "Hide";
                    } else {
                        passwordInput.type = "password";
                        this.textContent = "Show";
                    }
                });
            }
        });

        $(document).ready(function () {
            $("#show_hide_password a").on('click', function (event) {
                event.preventDefault();
                if ($('#show_hide_password input').attr("type") == "text") {
                    $('#show_hide_password input').attr('type', 'password');
                    $('#show_hide_password i').addClass("fa-eye-slash");
                    $('#show_hide_password i').removeClass("fa-eye");
                } else if ($('#show_hide_password input').attr("type") == "password") {
                    $('#show_hide_password input').attr('type', 'text');
                    $('#show_hide_password i').removeClass("fa-eye-slash");
                    $('#show_hide_password i').addClass("fa-eye");
                }
            });
        });

        //    const forgotPasswordLink = document.getElementById('forgot-password');
        //    forgotPasswordLink.addEventListener('click', (event) => {
        //        event.preventDefault();
        //        alert('Password reset instructions sent to your email!');
        //    });
    </script>
</head>

<body>
    <div class="login-box">
        <img src="RESOURCES/user.png" class="avatar" />
        <div class="login-container">
            <%--  <div class="avatar">
    <img src="RESOURCES/user.png" alt="User Avatar"/>
  </div>--%>
            <h1>Sales Force Automation</h1>
            <form id="idLoginPage" method="post" action="#" runat="server">
                <div class="input-group">
                    <asp:TextBox ID="TextBox1" type="text" runat="server" CssClass="form-control" style="border-end-end-radius: 15px; border-start-end-radius: 15px;" name="username" required></asp:TextBox>
                    <label for="username">Username</label>
                </div>

                <div class="input-group">
                    <div class="input-group" id="show_hide_password">
                        <asp:TextBox ID="TextBox2" type="password" runat="server" name="password" CssClass="form-control password-input" Width="180px" required></asp:TextBox>
                        <label for="password">Password</label>
                        <div class="input-group-addon" style="border-end-end-radius: 15px; border-start-end-radius: 15px;">
                            <a href=""><i class="fa fa-eye-slash" aria-hidden="true"></i></a>
                        </div>
                    </div>
                </div>
                <asp:Button ID="BtnLogin" type="submit" OnClick="BtnLogin_Click" runat="server" Text="LOGIN" CssClass="button" />
            </form>
            <a href="ChangePassword.aspx" id="forgot-password">🔓 Reset Password</a>
            <asp:Label ID="Label1" class="label_version" runat="server" Text=""></asp:Label>

        </div>
    </div>
</body>
</html>
