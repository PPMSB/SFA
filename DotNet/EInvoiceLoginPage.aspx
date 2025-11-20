<%@ Page Title="" Language="C#" MasterPageFile="~/lhdn/EInvoiceMaster.Master" AutoEventWireup="true" CodeBehind="EInvoiceLoginPage.aspx.cs" Inherits="DotNetSync.lhdn.EInvoiceLoginPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="page_title" runat="server">
    <title>e-Invoice Completed List</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="css_section" runat="server">
    <link rel="stylesheet" type="text/css" href="STYLES/custom.css"/>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">    
    <div class="col-12 col-sm-10 col-md-8 col-lg-6 mx-auto my-5 text-center py-md-5">
        <form class="form-signin col-sm-8 col-md-6 col-lg-6 mx-auto my-5">
          <img class="mb-4" src="IMG/LOGO.png" alt="" height="72">
          <h1 class="h3 mb-3 font-weight-normal text-gold">User Authentication</h1>
          <label for="login_id" class="visually-hidden">User Id</label>
          <input type="text" id="login_id" class="form-control my-2 font-biggest" placeholder="Domain User Id" required autofocus>
          <label for="password" class="visually-hidden">Password</label>
          <input type="password" id="password" class="form-control my-2 font-biggest" placeholder="Password" required>
          <p id="error_msg" class="col-12 bg-danger text-light p-2 radius-5px d-none"></p>
          <!--
          <div class="checkbox mb-3">
            <label>
              <input type="checkbox" value="remember-me"> Remember me
            </label>
          </div>
          -->
          <button class="btn btn-lg px-5 text-uppercase btn-success" type="submit" onclick="login()">Login</button>
          <p class="my-3"><a href="member_forgot_password.php" class="">Forgot Password?</a></p>
          <!--<p class="my-3"><a href="member_registration.php" class="">Sign Up</a></p>-->
        </form>
      </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="javascript_section" runat="server">
    <script src="scripts/jquery-3.5.1.js"></script>
    <script>
        function login() {
            event.preventDefault();

            var login_id = $("#login_id").val();
            var password = $('#password').val();
            var web = 1;

            if (login_id.length == 0 || password.length == 0) {
                alert("ID / Password Required");
                return;
            }

            $.ajax({
                type: "POST",
                url: "EInvoiceLoginPage.aspx/login", 
                data: JSON.stringify({ user_id: login_id, password: password }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    console.log(response);

                    var json_response = JSON.parse(response.d);

                    // Access properties from the JSON object                    
                    //var start_time = json_response.StartTime;
                    var status = json_response.status;
                    var status_msg = json_response.status_msg;

                    if (status == "success") {
                        location.assign('EInvoiceDashboard.aspx');
                    } else {
                        $("#error_msg").html(status_msg);
                        $("#error_msg").addClass("d-block");
                        $("#error_msg").removeClass("d-none");
                    }
                },
                error: function (jqXHR, textStatus) {
                    alert(textStatus);
                    console.log(textStatus.toString());
                }
            });
        }
    </script>
    </script>
</asp:Content>
