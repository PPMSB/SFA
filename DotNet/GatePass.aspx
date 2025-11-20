<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GatePass.aspx.cs" Inherits="DotNet.GatePass" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>Gatepass Check</title>

    <meta http-equiv="X-UA-Compatible" content="IE=Edge"/>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>    
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0"/>  
    <meta name="apple-mobile-web-app-capable" content="yes"/>
    <meta name="mobile-web-app-capable" content="yes"/>

    <script src="scripts/BrowserReload_ThroughHistory.js"></script>

	<link href="https://fonts.googleapis.com/css?family=Open+Sans:300,400,700" rel="stylesheet"/>

	<style type="text/css">

		.div_class{

			margin-top: 0;

			background-color: #000;

		}

		.btn_class{

			padding-top: 15px;

			padding-bottom: 15px;

			padding-left: 20px;

			padding-right: 20px;

			font-family: Open Sans;

			border-radius: 2em;

			font-weight: 600;

		}

	</style>

</head>
<body style="margin: 0; font-family: Open Sans;">
	<div class="div_class">

	<img src="RESOURCES/hirev_logo.png" style="display: inline-block; width: 10%; padding: 5px;"/>


</div>

<br/>

    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"/>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
    <div>
	<center>
        <h2> 
            <asp:Label ID="Label_gatepassno" class="gettext" runat="server"></asp:Label> 
            
            <asp:Label ID="previous_scan_datetime" class="gettext" runat="server"></asp:Label> 
        </h2>

        <form method="post" id="checkout">    
            <asp:Button ID="Button_Checkout" runat="server"  OnClick="Button_Checkout_click" Text="Check Out" class="glow_green"/>
          <%--<button 
            style="background-color: white;
            padding-top: 15px;
            padding-bottom: 15px;
            padding-left: 20px;
            padding-right: 20px;
            font-family: Open Sans;
            border-radius: 2em;
            font-weight: 600;
            font-size: 18px;
            width: 200px;"
            name="confirm"
            >Check Out
          </button>--%>
          <asp:Button ID="Button_Cancel" runat="server"  OnClick="Button_Cancel_click" Text="Cancel" class="glow_green"/>
        <%--<button
            style="background-color: white;
            margin-top: 10px;    
            padding-left: 15px;
            padding-right: 15px;
            font-family: Open Sans;
            border-radius: 2em;
            font-weight: 600;
            font-size: 18px;
            width: 120px;
            height: 50px;"
            name="cancel"
            >Cancel
          </button>--%>
            </form>
    </center>
    </div>
      </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>

