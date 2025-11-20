<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Visitor_CheckInOutQRCode.aspx.cs" Inherits="DotNet.Visitor_CheckInOutQRCode" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="display: flex;justify-content: center;
    align-items: center;
    flex-direction: column;">
            <asp:Image ID="QRCodeImage" runat="server" Width="50%" Height="50%" />
            <img class="Company_Icon" alt="POSIM PETROLEUM MARKETING" src="RESOURCES/PPM%20bg%20white.gif" style="width: 46%;"/>
        </div>
               <asp:Button ID="BtnRedirect" runat="server" OnClick="RedirectSecurityToSystem" Text="Redirect" class="glow_green button-margin" Enabled="true" />

    </form>
</body>
</html>

