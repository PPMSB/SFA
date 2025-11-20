<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Invoice.aspx.cs" Inherits="DotNet.Invoice" %>

<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="shortcut icon" href="RESOURCES/LFIB_icon.ico" />

    <link id="lnkStyleSheet" href="STYLES/InvoiceDesign.css" rel="stylesheet" />

    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="mobile-web-app-capable" content="yes" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <link rel="stylesheet" type="text/css" href="main-styles.css" />
    <script src="scripts/BrowserReload_ThroughHistory.js"></script>
    <script src="scripts/PrintDiv.js"></script>
    <style type="text/css">
        .auto-style1 {
            width: 530px;
            height: 60px;
        }

        #TotalForCN {
            margin-left: 1294px;
        }

        @media print {
            .button_print {
                display: none !important;
            }
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <input name="b_print" id="b_print" type="button" class="button_print" onclick="PrintDiv('div_print');" value="Print" runat="server" />
        <div id="div_print" class="row" runat="server">
            <table class="table_style_HeaderCompanyInfo">

                <tr>
                    <td>
                        <asp:Label ID="Label_CompanyName" runat="server" Text="" Style="font-weight: bold;"></asp:Label><br />
                        <asp:Label ID="Label_CompanyAddress" runat="server" Text=""></asp:Label><br />
                        <asp:Label ID="Label_POAddress" runat="server" Text=""></asp:Label><br />
                        <asp:Label ID="Label_CompanyTel" runat="server" Text=""></asp:Label>
                        &nbsp
                <asp:Label ID="Label_CompanyTelFax" runat="server" Text=""></asp:Label><br />
                    </td>

                    <td style="width: 50%; text-align: right; border: none;">
                        <asp:Image ID="QRCodeImage" runat="server" Width="90px" Height="90px" />
                        <img class="Company_Icon" alt="POSIM PETROLEUM MARKETING" src="RESOURCES/PPM%20bg%20white.gif" /></td>
                </tr>
            </table>

            <div class="col-12 col-s-12 Form_Header">
                <asp:Label ID="Label_FormType" class="gettext_FormType" runat="server" Text="DO / INVOICE"></asp:Label>
            </div>
            <br />

            <table class="table_style" id="tblInvoiceNo" runat="server" style="width: 50%; float: right;">
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="INVOICE NO: "></asp:Label></td>
                    <td>
                        <asp:Label ID="Label_InvoiceNo" runat="server" Text=""></asp:Label></td>
                </tr>
            </table>

            <table class="table_style_HeaderCustomerInfo" id="tblHeaderCustomerInfo" runat="server">
                <tr>
                    <th>Customer Address: </th>
                    <th>Delivery Instruction & Address:</th>

                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label_CustomerName" runat="server" Text=""></asp:Label><br />
                        <asp:Label ID="Label_CustomerAddress" runat="server" Text=""></asp:Label><br />
                        <br />

                        <asp:Label ID="Label_CustomerHp" runat="server" Text=""></asp:Label><br />
                        <asp:Label ID="Label_CustomerTel" runat="server" Text=""></asp:Label>
                    </td>

                    <td style="vertical-align: text-top;" id="DeliveryAddress" runat="server">
                        <asp:Label ID="Label_DeliveryAddress" runat="server" Text=""></asp:Label></td>
                </tr>
            </table>
            <asp:Label ID="Label_TIN" runat="server" Text="TIN No. :"></asp:Label>
            <asp:Label ID="Label_getTinNo" runat="server" Text="" Width="320px"></asp:Label>

            
            <asp:Label ID="Label_SST" runat="server" Text="SST No. :"></asp:Label>
            <asp:Label ID="Label_getSstNo" runat="server" Text=""></asp:Label>
            <table class="table_style_HeaderCustomerInfo" id="tblCreditNote" runat="server">
                <%--                    <tr>
                        <th>Customer Address: </th>
                    </tr>--%>
                <tr>
                    <td class="auto-style1">
                        <asp:Label ID="lblCust" runat="server" Text=""></asp:Label><br />
                        <%--                            <div class="row">--%>
                        <asp:Label ID="lblCusAddress" runat="server" Width="82%"></asp:Label>
                        <asp:Label ID="lblSalesman" runat="server" Font-Bold="true" Width="10%"></asp:Label><br />
                        <%--                            </div>--%>

                        <asp:Label ID="lblCustHp" runat="server" Text=""></asp:Label><br />
                        <asp:Label ID="lblCustTel" runat="server" Text=""></asp:Label>
                    </td>
                    <td style="border-color: #FFFFFF" class="auto-style2">
                        <div class="col-12">
                            <div class="col-6">
                                <asp:Label ID="lblNo" runat="server" Text="No. :"></asp:Label>
                            </div>
                            <div class="col-6">
                                <asp:Label ID="lblInvoiceNo" runat="server"></asp:Label>
                            </div>
                        </div>

                        <div class="col-12">
                            <div class="col-6">
                                <asp:Label ID="lblDate" runat="server" Text="Date:"></asp:Label>
                            </div>
                            <div class="col-6">
                                <asp:Label ID="lblInvoiceDt" runat="server"></asp:Label>
                            </div>
                        </div>

                        <div class="col-12">
                            <div class="col-6">
                                <asp:Label ID="lblAccNo" runat="server" Text="Account No. :"></asp:Label>
                            </div>
                            <div class="col-6">
                                <asp:Label ID="lblInvoiceAccNo" runat="server"></asp:Label>
                            </div>
                        </div>

                        <style type="text/css" media="print">
                            .col-12::after {
                                content: "";
                                display: table;
                                clear: both;
                            }

                            .col-6 {
                                float: left;
                                width: 50%;
                            }
                        </style>
                    </td>

                </tr>

            </table>
            <br />
            <table class="table_style" id="tblInvoice" runat="server">
                <tr>
                    <th>Account No.</th>
                    <th>Your Order No.</th>
                    <th>Sales Order No.</th>
                    <th>Terms</th>
                    <th>Salesman No.</th>
                    <th>Date</th>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label_AccountNo" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="Label_OrderNo" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="Label_SalesOrderNo" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="Label_Terms" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="Label_SalesmanNo" runat="server" Text=""></asp:Label></td>
                    <td>
                        <asp:Label ID="Label_InvoiceDate" runat="server" Text=""></asp:Label></td>
                </tr>
            </table>

            <div class="col-12 col-s-12" id="InvoiceForDN" style="display: none" runat="server">
                <asp:GridView ID="GridView_FormList" runat="server"
                    CssClass="mydatagrid"
                    HtmlEncode="False"
                    Style="overflow: hidden;"
                    AutoGenerateColumns="false"
                    HeaderStyle-BorderColor="Black" HeaderStyle-BackColor="White"
                    HeaderStyle-BorderStyle="Solid" HeaderStyle-BorderWidth="1px">
                    <Columns>

                        <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-Width="57%" />
                        <asp:BoundField DataField="Location" HeaderText="Loc." ItemStyle-Width="5%" ItemStyle-HorizontalAlign="center" />
                        <asp:BoundField DataField="Qty./Vol." HeaderText="Qty./Vol." ItemStyle-Width="10%" ItemStyle-HorizontalAlign="center" />
                        <asp:BoundField DataField="Unit Price" HeaderText="Unit Price" ItemStyle-Width="14%" ItemStyle-Font-Bold="false" ItemStyle-HorizontalAlign="right" />
                        <asp:BoundField DataField="Amount RM" HeaderText="Amount (RM)" ItemStyle-Width="14%" ItemStyle-Font-Bold="false" ItemStyle-HorizontalAlign="right" />
                    </Columns>

                </asp:GridView>
            </div>

            <table class="table_style" runat="server" id="table_particular">
                <tr>
                    <td style="width: 70%">
                        <asp:Label ID="lblParticular" runat="server" Text="Particulars" ItemStyle-Width="70%" Font-Bold="true"></asp:Label>
                    </td>
                    <td style="width: 30%">
                        <asp:Label ID="lblAmount" Text="Amount (RM)" runat="server" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="right" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
            </table>

            <div id="InvoiceForCN" runat="server">
                <asp:Label ID="Label_CN_particulars" runat="server" Width="70%" class="gettextLabel"></asp:Label>
            </div>
            <br />
            <table id="Table1" class="table_style_total_CN" runat="server">
                <tr>
                    <td style="width: 50%">
                        <asp:Label ID="Label_CN_particulars_header" runat="server" class="gettextLabel"></asp:Label>
                    </td>
                    <td style="width: 15%; text-align: center;">
                        <asp:Label ID="Label_CN_Total" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>

            <div class="col-12 col-s-12" id="divCreditNote" style="display: none" runat="server">
                <asp:GridView ID="GridViewCN" runat="server"
                    HtmlEncode="False"
                    Style="overflow: hidden;" GridLines="None"
                    AutoGenerateColumns="false">
                    <Columns>
                        <asp:BoundField DataField="Invoice" HeaderText="INVOICE" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Font-Underline="true" ItemStyle-Width="1%" />
                        <asp:BoundField DataField="Dated" HeaderText="DATED" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Font-Underline="true" ItemStyle-Width="1%" />
                        <asp:BoundField DataField="Amount" HeaderText="AMOUNT (RM)" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Font-Underline="true" ItemStyle-Width="2%" />
                        <asp:BoundField DataField="Total Discount" HeaderText="TOTAL DISCOUNT" ItemStyle-HorizontalAlign="Left"
                            HeaderStyle-HorizontalAlign="Left" HeaderStyle-Font-Underline="true" ItemStyle-Width="7%" />
                    </Columns>
                </asp:GridView>

                <asp:GridView ID="GridViewCNReturn" runat="server"
                    HtmlEncode="False"
                    Style="overflow: hidden;" GridLines="None"
                    AutoGenerateColumns="false">
                    <Columns>
                        <asp:BoundField DataField="Name" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="1%" />
                        <asp:BoundField DataField="TotalUnit" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="1%" />
                        <asp:BoundField DataField="UnitId" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="2%" />
                        <asp:BoundField DataField="SalesPrice" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="2%" />
                        <asp:BoundField DataField="TotalPrice" ItemStyle-HorizontalAlign="Left"
                            HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="7%" />
                    </Columns>
                </asp:GridView>
                <div class="col-6 col-s-6" style="text-align: right">
                    <asp:Label ID="lblSubTotal" runat="server" Text="Sub-Total: " Visible="false"></asp:Label>
                </div>
                <div class="col-3 col-s-3" style="text-align: right; padding-right: 37px;">
                    <asp:Label ID="lblGetSubTotal" runat="server" Visible="false"></asp:Label>
                </div>
                <div class="col-5 col-s-5" style="text-align: right">
                    <asp:Label ID="lbldiscount" runat="server" Text="Total Discount: " Visible="false"></asp:Label>
                    <asp:Label ID="lblGetDiscount" runat="server" Visible="false"></asp:Label>
                </div>
                <div class="col-4 col-s-4" style="text-align: right; padding-right: 37px;">
                    <asp:Label ID="lblAfterDisc" runat="server" Visible="false"></asp:Label>
                </div>

                <div class="col-4 col-s-4">
                    <div class="col-8 col-s-8">
                        <asp:Label ID="lblName" runat="server" class="labeltext"></asp:Label>
                    </div>
                </div>

                <div class="col-6 col-s-6">
                    <div class="col-6 col-s-6">
                        <asp:Label ID="lblTotalUnit" runat="server" CssClass="labeltext"></asp:Label>

                        <asp:Label ID="lblUnitId" runat="server" CssClass="labeltext"></asp:Label>
                        <asp:Label ID="lblSalesPrice" runat="server" CssClass="labeltext"></asp:Label>
                    </div>

                    <div class="col-3 col-s-3">
                        <asp:Label ID="lblTotalPrice" runat="server" CssClass="labeltext"></asp:Label>
                    </div>
                </div>

            </div>

            <table class="col-12 col-s-12 table_style3_footer" id="TotalInvoiceAmount" runat="server">
                <tr>
                    <td style="width: 70%; text-align: left;">
                        <asp:Label ID="Label_DuePayment" runat="server" Text=""></asp:Label></td>
                    <td style="text-align: center; font-weight: bold;">
                        <asp:Label ID="Label_TotalAmount" runat="server" Text=""></asp:Label></td>
                </tr>
            </table>

            <%--            <div class="col-5 footer_textalign_left">
                <asp:Label ID="Label_rm" runat="server"></asp:Label>
            </div>--%>
            <table class="table_style footer1" runat="server" id="TotalCreditNoteAmount">
                <tr>
                    <td style="width: 70%">
                        <%--                        <asp:Label ID="Label3" runat="server" ItemStyle-Width="70%"></asp:Label>--%>
                        <asp:Label ID="Label_rm" runat="server" ItemStyle-Width="70%" Font-Size="Small"></asp:Label>

                    </td>
                    <td style="width: 30%">
                        <asp:Label ID="Label_CreditNoteAmount" Text="Amount (RM)" runat="server" ItemStyle-Width="30%" Font-Bold="true" CssClass="center"></asp:Label>
                    </td>
                </tr>
            </table>

            <table class="creditnote_companyname" id="table_companyname" runat="server">
                <tr>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text="POSIM PETROLEUM MARKETING SDN BHD" Font-Size="Small" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label9" runat="server" Text="Computer generated no signature is required." Font-Size="Small"></asp:Label>
                    </td>
                </tr>

            </table>

            <%--            <table class="invoice_companyname" id="table_invoicecompanyname" runat="server">
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="POSIM PETROLEUM MARKETING SDN BHD" Font-Size="Smaller" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label8" runat="server" Text="Computer generated no signature is required." Font-Size="Small"></asp:Label>
                    </td>
                </tr>
            </table>--%>
            <table class=" col-12 col-s-12 table_style4_footer_Author" id="tblCreditNoteAuthor" runat="server">
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="LabelCN_author" class="gettext_author" runat="server"></asp:Label></td>
                </tr>
            </table>

            <table class=" col-12 col-s-12 table_style3_footer_Author" id="tblinvoicefooter" runat="server">
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="Label_author" class="gettext_author" runat="server" Text=""></asp:Label></td>
                </tr>
                <%--                <tr>
                    <td>
                        <asp:Label ID="Label_rule" class="gettext_author" runat="server" Text=""></asp:Label></td>
                </tr>--%>
            </table>

            <table class="col-12 table_style3_footer_EOE">
                <tr>
                    <td style="text-align: center;">
                        <asp:Label ID="Label7" runat="server" Text="E. & O.E." Font-Size="Small"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <asp:Label ID="lblLatePayment" runat="server" Font-Size="Small"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <asp:Label ID="lblDiscrepancy" runat="server" Font-Size="Small"></asp:Label>
                    </td>
                </tr>
            </table>

        </div>

    </form>
</body>
</html>
