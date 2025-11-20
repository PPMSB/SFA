<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Campaign_DocumentPDF.aspx.cs" Inherits="DotNet.Campaign_DocumentPDF" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="shortcut icon" href="RESOURCES/LFIB_icon.ico" />
    <script src="scripts/GoToTab.js"></script>
    <link href="STYLES/xxtra_global_Content_Adjuster.css" rel="stylesheet" />
    <link href="STYLES/xtra_ButtonUp.css" rel="stylesheet" />
    <link href="STYLES/xtra_Grid_View.css" rel="stylesheet" />
    <link href="STYLES/xtra_Glowing.css" rel="stylesheet" />
    <link href="Content/form_custom.css" rel="stylesheet" />
    <link href="Content/custom.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css" media="screen" />
    <script src="../scripts/jquery/jquery.min.js" type="text/javascript"></script>

    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap.bundle.min.js"></script>

    <link href="../Content/bootstrap-datepicker.min.css" rel="stylesheet" />
    <script src="../scripts/jquery/bootstrap-datepicker.js" type="text/javascript"></script>
    <link href="../Content/bootstrap-datetimepicker.min.css" rel="stylesheet" />

    <script src="../scripts/moment.min.js"></script>
    <script src="../scripts/bootstrap-datetimepicker.js"></script>


    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="mobile-web-app-capable" content="yes" />

    <script src="scripts/BrowserReload_ThroughHistory.js"></script>
    <link href="STYLES/xtra_top_navigation.css" rel="stylesheet" />
    <script src="scripts/top_navigation.js"></script>

    <style>
        @media print {
            body {
                width: 210mm;
                height: 297mm;
                margin: 0;
                padding: 0;
            }

            .print-hide {
                display: none;
            }

            .print-show {
                display: block;
            }

            .container {
                display: block; /* Change to a block layout for print */
            }

            .col-6, .col-7, .col-8, .col-3, .col-4 {
                width: 50%;
            }

            .column {
                float: left;
                width: 50%;
                padding: 10px;
                height: 300px;
            }

            .col-p-6 {
                width: 100%;
            }


            /* Additional styles for proper printing layout */
        }

        table, td, th {
            border: 1px solid;
        }

        table {
            width: 50%;
            border-collapse: collapse;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <asp:UpdateProgress runat="server" ID="UpdateProgress" class="gettext" AssociatedUpdatePanelID="UpdatePanel1">
            <ProgressTemplate>
                <div class="loading">
                    <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlContent" runat="server">
                    <div style="display: flex; justify-content: center">
                        <div style="width: 90%">
                            <div style="display: flex; justify-content: center;">
                                <div class="mt-3 p-2" style="display: flex; justify-content: space-between; align-items: center; border-bottom: 1px solid black; width: 100%">

                                    <div style="line-height: 0.3;">

                                        <p style="font-weight: bold; font-size: 14px">POSIM PETROLEUM MARKETING SDN BHD 198501007134 (139576-M)</p>
                                        <p style="font-size: 12px">Wisma Posim, Lot 72, Persiaran Jubli Perak, 40000 Shah Alam, Selangor D.E</p>
                                        <p style="font-size: 12px">P.O.Box 7695, 40724 Shah Alam, Selangor Darul Ehsan, Malaysia</p>
                                        <p style="font-size: 12px">Tel: +603 5102 2999 (Hunting Line)</p>
                                    </div>
                                    <img class="Company_Icon" alt="POSIM PETROLEUM MARKETING" src="<%= GLOBAL_VAR.GLOBAL.BaseDirectory + "RESOURCES/PosimNewLogo.jpeg"%>" style="width: 25%;" />
                                </div>
                            </div>

                            <div class="mb-1" style="text-align: center; margin: 10px 0 10px 0; font-weight: 500; font-size: 14px">
                                <u>OFFER LETTER OF VOLUME INCENTIVE CAMPAIGN (Phase 1)</u>
                            </div>

                            <div>
                                <div style="line-height: 1;">
                                    <div class="row">
                                        <div>
                                            <label class="" style="font-size: 12px">Campaign ID<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>:&nbsp;&nbsp;<asp:Label ID="lblCampaignID" class="gettext" runat="server"></asp:Label>&nbsp;&nbsp;<span>(with effective from <span runat="server" id="lblCampaignStartDate">01 July 2024 </span>until further notice)</span></label>
                                        </div>

                                    </div>

                                    <div class="row">
                                        <div>
                                            <label class="" style="font-size: 12px">Sequence No<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>:&nbsp;&nbsp;<asp:Label ID="lblSequenceNo" class="gettext" runat="server"></asp:Label></label>
                                        </div>

                                    </div>

                                    <div class="row">
                                        <div>
                                            <label class="" style="font-size: 12px">Customer Group<span>&nbsp;&nbsp;&nbsp;&nbsp;</span>:&nbsp;&nbsp;<u>2W / 4W&nbsp;&nbsp;</u><span style="font-size: 9px">(Please circle where appropriate)</span></label>
                                        </div>

                                    </div>

                                    <div class="row">
                                        <div>
                                            <label class="" style="font-size: 12px">Customer Account<span>&nbsp;&nbsp;&nbsp;</span>:&nbsp;&nbsp;<asp:Label ID="lblCustomerAccount" class="gettext" runat="server" Style="text-decoration: underline"></asp:Label>&nbsp;<span style="font-size: 9px">(Tyre & Battery account will be consolidated into principal account, if applicable)</span></label>
                                        </div>

                                    </div>

                                    <div class="row">
                                        <div>
                                            <label class="" style="font-size: 12px">Workshop Name<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>:&nbsp;&nbsp;<asp:Label ID="lblWorkshopName" class="gettext" runat="server" Style="text-decoration: underline"></asp:Label></label>
                                        </div>

                                    </div>

                                    <div class="row">
                                        <div>
                                            <label class="" style="font-size: 12px">PPM Salesman<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>:&nbsp;&nbsp;<asp:Label ID="lblSalesman" class="gettext" runat="server"></asp:Label></label>
                                        </div>

                                    </div>
                                </div>
                                <p style="font-size: 12px; font-weight: 500">Dear Sir / Madam,</p>
                                <u style="font-size: 15px; font-weight: 500">Target:</u>

                                <div style="margin: 0 10px 0 10px">
                                    <div class="row">
                                        <span style="font-size: 12px; font-weight: 500">1. No of Month: 12 Months, Total Sales Target with <span runat="server" id="lblTerms">M60</span>&nbsp;Prompt Payment : <span runat="server" id="lblFirstTarget1"></span></span>
                                    </div>
                                    <div class="row">
                                        <span style="font-size: 12px">2. Incentive Rate:</span>
                                    </div>
                                </div>

                                <asp:Repeater ID="tbProductPercentage" runat="server">
                                    <HeaderTemplate>
                                        <table style="border: 1px solid; text-align: center; font-size: 12px">
                                            <tr>
                                                <th style="border: 1px solid; width: 130px">Target</th>
                                                <th style="border: 1px solid; width: 100px">Product
                                                    <br />
                                                    Group A</th>
                                                <th style="border: 1px solid; width: 100px">Product
                                                    <br />
                                                    Group B</th>
                                                <th style="border: 1px solid; width: 100px">Product<br />
                                                    Group C</th>
                                                <th style="border: 1px solid; width: 100px">Product<br />
                                                    Group D</th>
                                                <th style="border: 1px solid; width: 100px">Product<br />
                                                    Group E</th>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td style="border: 1px solid;"><%# Eval("TargetAmount") %></td>
                                            <td style="border: 1px solid;"><%# Eval("ProductA") %></td>
                                            <td style="border: 1px solid;"><%# Eval("ProductB") %></td>
                                            <td style="border: 1px solid;"><%# Eval("ProductC") %></td>
                                            <td style="border: 1px solid;"><%# Eval("ProductD") %></td>
                                            <td style="border: 1px solid;"><%# Eval("ProductE") %></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                                <div runat="server" id="SalesRecordWrapper">
                                    <p style="font-weight: bold; font-size: 15px">Past 12 months Sales: RM <span style="text-decoration: underline" runat="server" id="lblSalesRecord"></span></p>
                                </div>

                                <u style="font-weight: bold; font-size: 15px">Terms and Condition :</u>
                                <div style="margin-left: 10px; font-size: 12px; line-height: 1; margin-bottom: 5px">
                                    <p>1.  Payment Terms to qualify for incentive calculation: Maximum <span runat="server" id="lblTerms1"></span>&nbsp;and achieve <span runat="server" id="lblTerms2"></span>&nbsp;target.</p>
                                    <p>2.  Volume Incentive Target is subject to Management Approval. </p>
                                    <p>3.  Committed target & duration must be fully compiled with to qualify for incentive. </p>
                                    <p>4.  Incentive will be in the form of Credit Note after campaign end. All invoices generated within the campaign period must be fully paid to entitle for incentive. </p>
                                    <p>5.  Customers who are unable to achieve <span runat="server" id="lblTerms3"></span>&nbsp;target must at least achieve <span runat="server" id="lblTerms4"></span>&nbsp;target of one-level-down to entitle for one-level-down incentive rate as shown above. </p>
                                    <p>6.  No stock return allowed after campaign end. </p>
                                    <p>7.  The account of "Tire (-T)" and "Battery (-B)", will be consolidated into ONE account calculation with your principal account. (Only the first eight (8) identical account numbers will be consolidated into one account calculation).  </p>
                                    <p>8.  Consolidation of different account for incentive calculation is NOT ALLOWED. </p>
                                    <p>9.  The Product Group & Incentive Table as per Appendix I. </p>
                                    <p>10  The management reserves the right to extend the whole campaign period. </p>
                                    <p>11. Customer classified as "A Distributor" class are not entitled to participate in this campaign. </p>
                                    <b>12. GST (Goods & Services Tax) payable amount is NOT calculated into sales/target achievement. </b>
                                </div>

                                <div style="border: 1px solid black; padding: 0px 10px 0 10px; display: flex; align-items: center; font-size: 12px; line-height: 1;">
                                    <div style="border-right: 1px solid black; padding-right: 40px">
                                        <p>I accept/decline to participate this campaign</p>
                                        <p>(Please circle where appropriate).</p>
                                        <br />
                                        <br />
                                        <br />
                                        <br />
                                        <br />
                                        <div style="width: 100%; border-bottom: 1px solid black"></div>
                                        <p>Customer Authorized Chop & Signature</p>
                                        <p>Authorized Personnel:________________________________</p>
                                        <p>Date Sign:__________________________________________</p>
                                        <p>Campaign Start Date:_________________________________</p>
                                        <p>Campaign End Date:__________________________________</p>
                                        <p>Email to receive monthly update: </p>
                                        <br />
                                        <div style="width: 100%; border-bottom: 1px solid black"></div>
                                        <br />
                                    </div>
                                    <div style="margin-left: 10px; position: relative; bottom: 5px;">
                                        <u style="font-weight: bold;">For Office Use Only:</u>
                                        <div runat="server" id="ExistedCampaignWrapper">
                                            <p>Campaign Start Date: <span runat="server" id="spanCampaignStartDate"></span></p>
                                            <p>Campaign End Date: <span runat="server" id="spanCampaignEndDate"></span></p>
                                        </div>
                                        <div runat="server">
                                            <p>Annual Sales:RM_____________________</p>
                                            <p>(_________________ to _________________)</p>
                                        </div>

                                        <br />
                                        <br />
                                        <br />
                                        <br />
                                        <br />
                                        <div style="display: flex; justify-content: space-between; align-items: center;">
                                            <p>Verify By:_____________________&nbsp;&nbsp;&nbsp;</p>
                                            <p>Date:_____________________</p>
                                        </div>
                                        <br />
                                        <div style="display: flex; justify-content: space-between; align-items: center;">
                                            <p>Approved By:________________&nbsp;&nbsp;&nbsp;</p>
                                            <p>Date:_____________________</p>
                                        </div>
                                    </div>
                                </div>

                                <div style="display: flex; justify-content: flex-end; margin-top: 15px">
                                    <img class="Company_Icon" alt="POSIM PETROLEUM MARKETING" src="<%= GLOBAL_VAR.GLOBAL.BaseDirectory + "RESOURCES/HirevLogo.jpeg"%>" style="width: 7%;" />
                                </div>

                            </div>
                        </div>
                    </div>


                    <%--                    <asp:GridView ID="gvProductPercentage" runat="server" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="TargetAmount" HeaderText="Target Amount" />
                            <asp:BoundField DataField="ProductA" HeaderText="Product A" />
                            <asp:BoundField DataField="ProductB" HeaderText="Product B" />
                            <asp:BoundField DataField="ProductC" HeaderText="Product C" />
                            <asp:BoundField DataField="ProductD" HeaderText="Product D" />
                            <asp:BoundField DataField="ProductE" HeaderText="Product E" />
                        </Columns>
                    </asp:GridView>--%>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Button ID="BtnRedirect" runat="server" OnClick="btnGeneratePDF_Click" Text="Redirect" class="glow_green button-margin" Enabled="true" />

    </form>
</body>
</html>
