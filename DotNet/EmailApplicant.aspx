<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmailApplicant.aspx.cs" Inherits="DotNet.EmailApplicant" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="STYLES/xtra_top_navigation.css" rel="stylesheet" />

    <title>Email</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="col-12 col-s-12">
                <div class="col-3 col-s-3">
                    <asp:Label ID="lblto" Text="TO :" runat="server" CssClass="labeltext"></asp:Label>
                </div>

                <div class="col-6 col-s-6">
                    <asp:Label ID="lblCc" Text="CC :" runat="server" CssClass="labeltext"></asp:Label>
                    <asp:DropDownList ID="ddlApplicant" runat="server" CssClass="dropdownlist"></asp:DropDownList>
                    <asp:DropDownList ID="ddlApplicant2" runat="server" CssClass="dropdownlist"></asp:DropDownList>
                </div>
                <br />

                <div class="col-6 col-s-6">
                    <asp:Label ID="lblSubject" runat="server" Text="Subject" CssClass="labeltext"></asp:Label>
                    <asp:TextBox ID="txtSubject" runat="server" Text="Application For Redemption ()" Width="35%"></asp:TextBox>
                </div>
            </div>
            <br />

            <div class="col-12 col-s-12">
                <div class="col-6 col-s-6">
                    <asp:Label ID="lblContent" runat="server" Text="Content" CssClass="labeltext" Style="vertical-align: top;"></asp:Label>
                    <asp:TextBox ID="txtContent" runat="server" TextMode="MultiLine" Rows="7" Columns="50"></asp:TextBox>
                </div>
            </div>
            <br />

            <div class="col-12 col-s-12">
                <div class="col-6 col-s-6">
                    <asp:Label ID="lblFormNo" runat="server" Text="Form No." CssClass="labeltext"></asp:Label>
                    <asp:TextBox ID="lblGetFormNo" runat="server" CssClass="gettextLabel"></asp:TextBox>
                </div>
            </div>
            <br />

            <asp:Button ID="buttonSend" runat="server" Text="Send" OnClick="buttonSend_Click" class="glow_green" />
            <asp:Button ID="buttonCancel" runat="server" Text="Cancel" OnClientClick="javascript:window.close();" class="glow_red" />
        </div>
    </form>
</body>
</html>
