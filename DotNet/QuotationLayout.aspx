<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuotationLayout.aspx.cs" Inherits="DotNet.QuotationLayout" %>

<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="shortcut icon" href="RESOURCES/LFIB_icon.ico" />

    <link id="lnkStyleSheet" href="STYLES/InvoiceDesign.css" rel="stylesheet" />

    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <%--    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />--%>
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="mobile-web-app-capable" content="yes" />
    <script src="scripts/BrowserReload_ThroughHistory.js"></script>
    <script src="scripts/PrintDiv.js"></script>
</head>
<body class="excludePage">
    <form id="form1" runat="server">
        <input name="b_print" id="b_print" type="button" class="button_print" visible="false" onclick="PrintDiv('div_print', this);" value=" Print " runat="server" />
        <div id="div_print" class="row" runat="server">
            <div class="col-12 col-s-12">
                <table id="tblInvoiceNo" runat="server" style="float: left;">
                    <tr>
                        <td style="width: 10%">Ref :
                            <asp:Label ID="Label_InvoiceNo" runat="server"></asp:Label><br />
                            Date:
                            <asp:Label ID="Label_Date" runat="server"></asp:Label>
                        </td>
                        <td style="width: 30%; text-align: right; border: none;">
                            <img class="Company_Icon" alt="POSIM PETROLEUM MARKETING" src="RESOURCES/PPM%20bg%20white.gif" />
                        </td>
                    </tr>
                </table>
            </div>

            <table id="tblHeaderCustomerInfo" style="width: 50%" runat="server">
                <tr>
                    <td>
                        <asp:Label ID="Label_CustomerName" runat="server"></asp:Label><br />
                        <asp:Label ID="Label_CustomerAddress" runat="server"></asp:Label><br />
                        <br />

                        <asp:Label ID="Label_CustomerHp" runat="server"></asp:Label><br />
                        <asp:Label ID="Label_CustomerTel" runat="server"></asp:Label>
                    </td>

                    <%--                    <td style="vertical-align: text-top;" id="DeliveryAddress" runat="server">
                        <asp:Label ID="Label_DeliveryAddress" runat="server" Text=""></asp:Label></td>--%>
                </tr>
            </table>
            <div>
                <asp:Label ID="Label_Re" runat="server" Text="RE: SUPPLY OF HI-REV LUBRICANTS" Font-Bold="true" Font-Underline="true"></asp:Label>
            </div>
            <br />
            <asp:Label ID="Label_Enquiry" runat="server"></asp:Label>
            <table class="table_style" runat="server" id="table_particular">
                <tr>
                    <td style="width: 10%">
                        <asp:Label ID="lblNo" runat="server" Text="No."></asp:Label>
                    </td>
                    <td style="width: 50%">
                        <asp:Label ID="lblProduct" runat="server" Text="Product Name"></asp:Label>
                    </td>
                    <td style="width: 30%">
                        <asp:Label ID="lblUnit" Text="Unit" runat="server"></asp:Label>
                    </td>
                    <td style="width: 30%">
                        <asp:Label ID="lblAmount" Text="Net Delivery Price RM" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>

            <div class="col-12 col-s-12" id="InvoiceForDN" style="display: none" runat="server">
                <asp:GridView ID="GridView_FormList" runat="server"
                    CssClass="mydatagrid" HtmlEncode="False" ShowHeader="false"
                    Style="overflow: hidden;" AutoGenerateColumns="false"
                    HeaderStyle-BorderColor="Black" HeaderStyle-BackColor="White"
                    HeaderStyle-BorderStyle="Solid" HeaderStyle-BorderWidth="1px">
                    <Columns>
                        <asp:TemplateField HeaderText="No." ItemStyle-Width="10%">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Description" HeaderText="Description" />
                        <asp:BoundField DataField="Unit" HeaderText="Unit" ItemStyle-Width="30%" />
                        <asp:BoundField DataField="Amount RM" HeaderText="Amount (RM)" ItemStyle-Width="13%" />
                    </Columns>
                </asp:GridView>
            </div>

            <div id="InvoiceForCN" runat="server">
                <asp:Label ID="Label_CN_particulars" runat="server" class="gettextLabel"></asp:Label>
            </div>
            <br />
            <table id="Table1" class="table_style_total_CN" runat="server">
                <tr>
                    <td style="width: 70%">
                        <asp:Label ID="Label_CN_particulars_header" runat="server" class="gettextLabel"></asp:Label>
                    </td>
                    <td style="width: 15%">
                        <asp:Label ID="Label_CN_Total" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>

            <div class="col-12 col-s-12" id="divCreditNote" style="display: none" runat="server">
                <asp:GridView ID="GridViewCN" runat="server"
                    CssClass="mydatagrid"
                    HtmlEncode="False"
                    Style="overflow: hidden;"
                    AutoGenerateColumns="false">
                    <Columns>
                        <asp:BoundField DataField="Invoice" HeaderText="Invoice" ItemStyle-Width="57%" />
                        <asp:BoundField DataField="Dated" HeaderText="Dated" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="center" />
                        <asp:BoundField DataField="Amount" HeaderText="Amount (RM)" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="center" />
                    </Columns>
                </asp:GridView>
            </div>


            <table class="col-12 quotationfooter">
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="Label9" runat="server" Text="Computer generated no signature is required." Font-Size="Small"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="Label_author" class="gettext_author" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label_rule" class="gettext_author" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        Terms and conditions: <br />
                        <asp:Label ID="lblPayment30" runat="server" Font-Size="Smaller" Font-Bold="true"></asp:Label><br />
                        <asp:Label ID="lblPayment60" runat="server" Font-Size="Smaller" Font-Bold="true"></asp:Label><br />
                        <asp:Label ID="lblTnc" runat="server" Font-Size="Smaller"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <asp:Label ID="Label7" runat="server" Text="POSIM PETROLEUM MARKETING SDN BHD 198501007134 (139576-M)" Font-Size="Small" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        <asp:Label ID="Label8" runat="server"
                            Text="No. 3 Jalan Keluli 15/16, Seksyen 15, 40200 Shah Alam, P.O. Box 7695, 40724 Shah Alam, Selangor Darul Ehsan, Malaysia"
                            Font-Size="Smaller" CssClass="text-center"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        <asp:Label ID="Label10" runat="server" Text="Tel: 03-5102 2999" Font-Size="Smaller"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>

    </form>
</body>
</html>
