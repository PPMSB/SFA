<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintingLayout.aspx.cs" Inherits="DotNet.PrintingLayout" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="shortcut icon" href="RESOURCES/LFIB_icon.ico" />
    <link id="lnkStyleSheet" href="STYLES/InvoiceDesign.css" rel="stylesheet" />
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="mobile-web-app-capable" content="yes" />
    <script src="scripts/BrowserReload_ThroughHistory.js"></script>
    <script src="scripts/PrintDiv.js"></script>

    <style>
        /* General layout styles */
        body {
            font-family: Arial, sans-serif;
            font-size: 12px;
            margin: 0;
            padding: 0;
            background-color: white;
        }

        /* A4 size layout */
        .container {
            width: 210mm;
            margin: 0 auto;
            padding: 20mm;
            background-color: white;
            box-sizing: border-box;
        }

        h2, h8 {
            text-align: center;
            margin: 0;
            padding: 5mm 0;
        }

        table {
            width: 100%;
            border-collapse: collapse;
            margin-bottom: 10mm;
        }

        td {
            padding: 3mm;
            border: 1px solid #000;
        }

        .table_style1 th {
            border: 1px solid black;
            padding: 8px;
            text-align: center;
            background-color: #f2f2f2; /* Optional for background color */
        }


        .table_style1 td {
            padding: 8px; /* Adds some padding inside the cells */
            text-align: left; /* Align text to the left */
            border: 1px solid black; /* Adds borders to the cells */
            white-space: normal; /* Allow text to wrap */
            word-break: break-all; /* Forces breaking of long words if necessary */
        }

        .label-container {
            display: grid;
            grid-template-columns: 1fr 1fr; /* Create two equal-width columns */
        }

        .label-column {
            padding: 10px;
        }

        .label-container {
            display: grid;
            grid-template-columns: 1fr 1fr; /* Two equal columns */
            column-gap: 20px; /* Space between columns */
            align-items: center; /* Vertically align content */
        }

        .label-row {
            display: grid;
            grid-template-columns: 150px 1fr; /* Fixed label width, flexible content width */
            align-items: center; /* Vertically align content */
        }


        /* Printing styles */
        @media print {
            body {
                margin: 0;
                padding: 0;
                background-color: white;
            }

            .container {
                width: 100%;
                padding: 0;
                margin: 0;
                box-sizing: border-box;
            }

            table {
                margin-bottom: 5mm;
            }

            td {
                padding: 2mm;
            }

            .quotationfooter {
                font-size: 9px;
            }

            .button_print {
                display: none;
            }
        }
    </style>
</head>
<body class="excludePage">
    <form id="form1" runat="server">
        <div class="container">
            <input name="b_print" id="b_print" type="button" class="button_print" onclick="PrintDiv('div_print', this);" value=" Print " runat="server" />
            <h8>POSIM PETROLEUM MARKETING SDN BHD</h8>
            <h2>POINTS REDEMPTION FORM</h2>

            <div id="div_print" class="row" runat="server">
                <div class="label-container">
                    <div class="label-row">
                        Customer Name:<asp:Label ID="Label_CustName" runat="server"></asp:Label><br />
                    </div>
                    <div class="label-row">
                        Account No:<asp:Label ID="Label_AccNo" runat="server"></asp:Label><br />
                    </div>
                    <div class="label-row">
                        HP No:<asp:Label ID="Label_HpNo" runat="server"></asp:Label><br />
                    </div>                    
                    <div class="label-row">
                        Contact Person:<asp:Label ID="Label_ContactNo" runat="server"></asp:Label><br />
                    </div>
                    <div class="label-row">
                        Application No:<asp:Label ID="Label_AppNo" runat="server"></asp:Label><br />
                    </div>
                    <div class="label-row">
                        Submission Date:<asp:Label ID="Label_SubmissionDt" runat="server"></asp:Label><br />
                    </div>
                    <div class="label-row">
                        Salesman:<asp:Label ID="Label_Salesman" runat="server"></asp:Label><br />
                    </div>
                    <div class="label-row">
                        HQ/BR:<asp:Label ID="Label_HqBr" runat="server"></asp:Label><br />
                    </div>
                    <div class="label-row">
                        Class:<asp:Label ID="Label_Class" runat="server"></asp:Label><br />
                    </div>
                    <div class="label-row">
                        Loyalty Point:<asp:Label ID="Label_Loyalty" runat="server"></asp:Label><br />
                    </div>
                    <div class="label-row">
                        Redemption Type:<asp:Label ID="Label_RedempType" runat="server"></asp:Label><br />
                    </div>
                </div>

                <asp:Table ID="table_particular" runat="server" CssClass="table_style1" Width="99%">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell>No.</asp:TableHeaderCell>
                        <asp:TableHeaderCell>Items/Others</asp:TableHeaderCell>
                        <asp:TableHeaderCell>Quantity</asp:TableHeaderCell>
                        <asp:TableHeaderCell>Amount(RM)</asp:TableHeaderCell>
                        <asp:TableHeaderCell>Points Value</asp:TableHeaderCell>
                        <asp:TableHeaderCell>Invoice No</asp:TableHeaderCell>
                        <asp:TableHeaderCell>Invoice Date</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>

                <%--                  <asp:Label ID="lblNote" runat="server"  CssClass="label_style"
                      Text="Note: Any offer items not available during redemption due to unforeseen circumstances shall be exchangeable for other items of equivalent values">
                  </asp:Label>--%>

                <div id="InvoiceForCN" runat="server">
                    <asp:Label ID="Label_CN_particulars" runat="server" class="gettextLabel"></asp:Label>
                </div>


                <div id="divCreditNote" style="display: none" runat="server">
                    <asp:GridView ID="GridViewCN" runat="server" CssClass="mydatagrid" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="Invoice" HeaderText="Invoice" />
                            <asp:BoundField DataField="Dated" HeaderText="Dated" />
                            <asp:BoundField DataField="Amount" HeaderText="Amount (RM)" />
                        </Columns>
                    </asp:GridView>
                </div>

                <div class="label-container">
                    <div class="label-row">
                        Deliver To:<asp:Label ID="Lbl_deliverTo" runat="server"></asp:Label><br />
                    </div>
                    <div class="label-row">
                        Beneficiary Name:<asp:Label ID="Lbl_BeneName" runat="server"></asp:Label><br />
                    </div>
                    <div class="label-row">
                        Deliver Address:<asp:Label ID="Lbl_Address" runat="server"></asp:Label><br />
                    </div>
                    <div class="label-row">
                        Beneficiary I/C:<asp:Label ID="Lbl_BeneIC" runat="server"></asp:Label><br />
                    </div>
<%--                    <div class="label-row">
                        Payment Send To:<asp:Label ID="Lbl_PaymentTo" runat="server"></asp:Label><br />
                    </div>--%>
                    <div class="label-row">
                        Beneficiary Bank Account:<asp:Label ID="Lbl_BeneBankAcc" runat="server"></asp:Label><br />
                    </div>
                    <div class="label-row">
                        Remarks:<asp:Label ID="Lbl_Remarks" runat="server"></asp:Label><br />
                    </div>
                    <div class="label-row">
                        LAST INVOICE:<asp:Label ID="Lbl_LastInvoice" runat="server"></asp:Label><br />
                    </div>
                    <div class="label-row">
                        LAST OUTSTANDING:<asp:Label ID="Lbl_LastOutstanding" runat="server"></asp:Label><br />
                    </div>
                    <div class="label-row">
                        Journal Remarks:<asp:Label ID="Lbl_JournalRemark" runat="server"></asp:Label><br />
                    </div>
                    <div class="label-row">
                        HOD Remarks:<asp:Label ID="Lbl_HODRemark" runat="server"></asp:Label><br />
                    </div>
                </div>
                <div class="label-row">
                    Process Date:<asp:Label ID="Lbl_ProcessDt" runat="server"></asp:Label><br />
                </div>
                <div class="label-row">
                    Process Status:
                        <asp:Label ID="Lbl_ProcessStat" runat="server" CssClass="col-10"></asp:Label>
                </div>
                <div class="label-row">
                    JOURNAL NO.:
                        <asp:Label ID="Lbl_Journal" runat="server" CssClass="col-10" Font-Bold="true"></asp:Label>
                </div>

                <table border="1" cellpadding="6" cellspacing="0" style="width:99%">
                    <!-- First Row: Headers -->
                    <tr>
                        <th>Pre-Posted</th>
                        <td><asp:Literal ID="LiteralPreposted" runat="server"></asp:Literal></td>
                        <th colspan="2">Deduction Point</th>
                    </tr>

                    <!-- Second Row: Records -->
                    <tr>
                        <th>Loyalty Point</th>
                        <td><asp:Literal ID="LiteralLp" runat="server"></asp:Literal></td>
                        <th>Loyalty Point</th>
                        <td><asp:Literal ID="LiteraldeductLp" runat="server"></asp:Literal></td>
                    </tr>

                    <!-- Third Row: Deduction Points -->
                    <tr>
                        <th>A&P Point</th>
                        <td><asp:Literal ID="LiteralAp" runat="server"></asp:Literal></td>
                        <th>A&P Point</th>
                        <td><asp:Literal ID="LiteralDeductAp" runat="server"></asp:Literal></td>
                    </tr>
                </table>
                <div class="label-row">
                    Printed By:
                        <asp:Label ID="Lbl_Printedby" runat="server" CssClass="col-10"></asp:Label>
                </div>

            </div>
        </div>
    </form>
</body>
</html>
