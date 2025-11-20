<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MapCustList.aspx.cs" Inherits="DotNet.MapCustList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="col-12">
                <asp:Button ID="button" runat="server" Text="Export" OnClick="button_Click" CssClass="glow_yellow" />
            </div>

            <asp:GridView ID="gv_Customer" runat="server" HtmlEncode="False">
                <Columns>
                    <asp:TemplateField HeaderText="No.">
                        <ItemTemplate>
                            <%#Container.DataItemIndex+1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
