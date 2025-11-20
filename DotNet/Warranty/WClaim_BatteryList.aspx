<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WClaim_BatteryList.aspx.cs" Inherits="DotNet.WClaim_BatteryList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="shortcut icon" href="RESOURCES/LFIB_icon.ico" />

    <script src="scripts/GoToTab.js"></script>
    <link href="STYLES/xxtra_global_Content_Adjuster.css" rel="stylesheet" />
    <link href="STYLES/xtra_Grid_View.css" rel="stylesheet" />
    <link href="STYLES/xtra_Glowing.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />

    <title>Inventory</title>
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="mobile-web-app-capable" content="yes" />

    <script src="scripts/BrowserReload_ThroughHistory.js"></script>
    <style type="text/css">
        /* General Page Styling */
body {
    font-family: Arial, sans-serif;
    background-color: #f9fafb;
    margin: 20px;
    color: #333;
}

/* Section Title */
.image_title p {
    font-size: 20px;
    font-weight: bold;
    color: #f97316; /* orange */
    margin-bottom: 15px;
}

/* Input Styling */
input[type="text"], input[type="number"] {
    width: 70px;
    padding: 6px 8px;
    border: 1px solid #ccc;
    border-radius: 6px;
    outline: none;
    transition: border 0.3s ease;
}
input[type="text"]:focus, input[type="number"]:focus {
    border-color: #f97316;
    box-shadow: 0 0 4px rgba(249, 115, 22, 0.5);
}

/* Buttons */
.btn {
    background-color: #6b7280; /* grey */
    color: #fff; /* white text */
    border: none;
    padding: 8px 16px;
    border-radius: 8px;
    font-weight: bold;
    cursor: pointer;
    transition: background 0.3s ease;
}
.btn:hover {
    background-color: #f97316; /* orange hover */
}

/* GridView */
.mydatagrid {
    border-collapse: collapse;
    width: 100%;
    margin-top: 15px;
}
.mydatagrid th {
    background-color: #f97316;
    color: white;
    padding: 8px;
    text-align: left;
}
.mydatagrid td {
    border-bottom: 1px solid #ddd;
    padding: 8px;
}
.mydatagrid tr:hover {
    background-color: #f1f1f1;
}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="row">
            <div class="mobile_hidden">
               
                <div class="col-9 col-s-9 image_title">
                    <p>Warrantly Battery Info</p>
                </div>
            </div>
           
            <asp:ScriptManager ID="ScriptManager1" runat="server" />

            
            <div class="col-12 col-s-12">
                <asp:UpdateProgress runat="server" ID="UpdateProgress2" class="gettext">
                    <ProgressTemplate>
                        <div class="loading">
                            <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>

                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <!-- Insert Section -->
        <div id="insertPanel" runat="server" style="margin-bottom: 20px;">
            <table>
                <tr style="display:none">
                    <td><asp:Label runat="server" Text="Invoice Id:" AssociatedControlID="txtInvoiceId" /></td>
                    <td><asp:TextBox ID="txtInvoiceId" runat="server" Width="70px" /></td>

                    <td><asp:Label runat="server" Text="Serial No.Id:" AssociatedControlID="txtSerialNumberId" /></td>
                    <td><asp:TextBox ID="txtSerialNumberId" runat="server" Width="70px" /></td>
                </tr>

                <tr style="display:none">

                <td><asp:Label runat="server" Text="Batch No.:" /></td>
    <td><asp:TextBox ID="txtBatch_No" runat="server" Width="70px" />
        <!-- No validator here, accepts any text --></td>
    

                </tr>
                <tr>
                    <td style="width: 25%;"><asp:Label runat="server" Text="Vol:" AssociatedControlID="txtLF_Bat_Vol" /></td>
                    <td style="width: 25%;">
                        <asp:TextBox ID="txtLF_Bat_Vol" runat="server" Width="70px" />
                        <asp:RegularExpressionValidator ID="revLF_Bat_Vol" runat="server" ControlToValidate="txtLF_Bat_Vol"
        ErrorMessage="Please enter a valid decimal number." ValidationExpression="^\d+(\.\d+)?$" ForeColor="Red" Display="Dynamic" />
            </td>
                <td style="width: 25%;"><asp:Label runat="server" Text="CCA:" AssociatedControlID="txtLF_Bat_CCA" /></td>
                <td style="width: 25%;">
        <asp:TextBox ID="txtLF_Bat_CCA" runat="server" Width="70px" />
        <asp:RegularExpressionValidator ID="revLF_Bat_CCA" runat="server"
            ControlToValidate="txtLF_Bat_CCA"
            ErrorMessage="Please enter a valid decimal number."
            ValidationExpression="^\d+(\.\d+)?$"
            ForeColor="Red" Display="Dynamic" />
                </td>

                </tr>
                 
                <tr>

    <td><asp:Label runat="server" Text="SOH(%):" AssociatedControlID="txtLF_Bat_SOH" /></td>
    <td>
        <asp:TextBox ID="txtLF_Bat_SOH" runat="server" Width="70px" />
        <asp:RegularExpressionValidator ID="revLF_Bat_SOH" runat="server"
            ControlToValidate="txtLF_Bat_SOH"
            ErrorMessage="Please enter a valid decimal number."
            ValidationExpression="^\d+(\.\d+)?$"
            ForeColor="Red" Display="Dynamic" />
                </td>
                <td ><asp:Label runat="server" Text="SOC(%):" AssociatedControlID="txtLF_Bat_SOC" /></td>
                 <td>
        <asp:TextBox ID="txtLF_Bat_SOC" runat="server" Width="70px" />
        <asp:RegularExpressionValidator ID="revLF_Bat_SOC" runat="server"
            ControlToValidate="txtLF_Bat_SOC"
            ErrorMessage="Please enter a valid decimal number."
            ValidationExpression="^\d+(\.\d+)?$"
            ForeColor="Red" Display="Dynamic" />
                </td>

                </tr>
                <tr>

                <td ><asp:Label runat="server" Text="IR:" AssociatedControlID="txtLF_Bat_IR" /></td>
                <td >
        <asp:TextBox ID="txtLF_Bat_IR" runat="server" Width="70px" />
        <asp:RegularExpressionValidator ID="revLF_Bat_IR" runat="server"
            ControlToValidate="txtLF_Bat_IR"
            ErrorMessage="Please enter a valid decimal number."
            ValidationExpression="^\d+(\.\d+)?$"
            ForeColor="Red" Display="Dynamic" />
                 </td>
                    <td></td><td></td>
                </tr>

                <tr>
                    <td colspan="2" style="text-align:center;">
                        <asp:Button ID="btnInsert" runat="server" Text="Insert" CssClass="btn" OnClick="btnInsert_Click" />
                    </td>
                    <td colspan="2" style="text-align:center;">
                        <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn" OnClientClick="window.close(); return false;" />
                        <asp:TextBox ID="txtHiddenRef" runat="server" Width="70px" Visible="false" />
                        <asp:TextBox ID="txtHiddenStatus" runat="server" Visible="false" />
                    </td>
                </tr>
            </table>
        </div>

        <!-- GridView with Edit and Paging -->
        <div id="grdCharges_WarrantyBattery" runat="server" style="max-width: 110%; overflow: auto; max-height: 100%;">
            <asp:GridView ID="GridView1" runat="server"
                PageSize="20" HorizontalAlign="Left"
                CssClass="mydatagrid" PagerStyle-CssClass="pager"
                HeaderStyle-CssClass="header" RowStyle-CssClass="rows"
                AllowPaging="True" AllowCustomPaging="True"
                AutoGenerateColumns="False"
                OnPageIndexChanging="datagrid_PageIndexChanging"
                DataKeyNames="RecId"
                OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
                >
                <Columns>
                  <asp:CommandField ShowSelectButton="False" Visible="false" />
                    <asp:BoundField DataField="No." HeaderText="No." ReadOnly="True" />
                    <asp:BoundField DataField="InvoiceId" HeaderText="Invoice Id" ReadOnly="True" />
                    <asp:BoundField DataField="SerialNumberId" HeaderText="Serial No.Id" ReadOnly="True"  />
                    <asp:BoundField DataField="BatchNumber" HeaderText="Batch No." />
                    <asp:BoundField DataField="LF_Bat_Vol" HeaderText="Vol" />
                    <asp:BoundField DataField="LF_Bat_CCA" HeaderText="CCA" />
                    <asp:BoundField DataField="LF_Bat_SOH" HeaderText="SOH(%)" />
                    <asp:BoundField DataField="LF_Bat_SOC" HeaderText="SOC(%)" />
                    <asp:BoundField DataField="LF_Bat_IR" HeaderText="IR" />
                    
                    <asp:BoundField DataField="modifiedDateTime" HeaderText="Updated Date & Time" />
                    <asp:BoundField DataField="RecId" HeaderText="RecId" Visible="false" />
                    <asp:BoundField DataField="Remarks" HeaderText="Current Status" />
                </Columns>
                <HeaderStyle CssClass="header" />
                <PagerSettings PageButtonCount="2" />
                <PagerStyle CssClass="pager" />
                <RowStyle CssClass="rows" />
            </asp:GridView>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>
