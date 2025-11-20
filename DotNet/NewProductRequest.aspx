<%@ Page Language="C#" autoeventwireup="true" codebehind="NewProductRequest.aspx.cs" inherits="DotNet.NewProductRequest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <style>
        * {
            padding: 0;
            margin: 2px;
        }

        #productDetail, .detailth, .detailtd, #coolanttable, .coolantth, .coolanttd, #besideCoolant, .besideth, .besidetd, #tblReview, #tblReview th, #tblReview td {
            border: 2px solid black;
            border-collapse: collapse;
        }

        #tblReview, #productDetail, #tblinfo, #divPack, tablechild {
            margin: auto;
        }

        #tableparent {
            position: center;
        }

        .detailtd, .coolanttd, .besidetd {
            text-align: center;
        }

        .detailth {
            text-align: center;
        }

        .lblreview, .colQC, .colProduct, .colPurchase {
            border: none;
        }

        .infoth {
            text-align: left;
        }

        #tableparent {
            display: flex;
        }

        .tablechildleft {
           flex: 1;
           padding: 5px;
           display: flex;
           justify-content: center;
        }
/*
        .tablechildleft table {
            float: right;
        }*/

        .tablechildright {
           flex: 1;
           padding: 5px;
        }

        .tablechildright table {
            float: left;
        }

        .textboxsizeshort {
            width: 100px;
            border: none;
            text-align: center;
        }

        .textboxnoline {
            border: none;
            justify-content: center;
            text-align: center;
        }

        .textboxline {
            border: none;
            border-bottom: 2px solid #000;
            text-align: center;
            width: 120px;
        }

        .textboxlinelong {
            border: none;
            border-bottom: 2px solid #000;
            text-align: center;
            width: 550px;
        }

        .fixSize {
            width: 600px;
            resize: none;
        }

        #caplidText, #inductabText, #drumpailText, .th {
            font-size: 16px;
            font-weight: bold;
            font-family: auto;
        } 

/*        .pth {
            width: 50px;
        }
*/
        .thh {
            width: 200px;
        }

        #divPack {
            text-align: left;
        }

        .flex-container {
            display: flex;

        }

        .flex-containerTop {
            display: flex;
            justify-content: center;
            align-items: center;
        }

        .flex-item {
            margin-right: 10px;
        }


        .btnClass {
            position: center;
            display: flex;
           justify-content: center;
           align-items: center;
        }

    </style>

    <style type="text/css" media="print">
        .no-print {
            display: none;
        }
    </style>

    <script type="text/javascript">
        function printPage() {
            window.print();
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <br />
        <div>
            <center>
                <h3>
                    Lion Petroleum Products Sdn Bhd
                </h3>
                <h2>
                    NEW PRODUCT REQUEST FORM
                </h2>
            </center>

            <table id="tblinfo">
                <tr >
                    <td colspan="4">
                        <div class="flex-containerTop">
                            <div class="flex-item">
                                <p class="th"> serial No: </p>
                            </div>
                            <div class="flex-item"> 
                                <asp:Label ID="serialNo" runat="server" />
                            </div>
                            <div class="flex-item">
                                <p class="th"> Status: </p>
                            </div>
                            <div class="flex-item">
                                <asp:Label ID="status" runat="server" />
                            </div>
                        </div>                                                 
                    </td>
                </tr>

                <tr>
                    <td colspan="4">
                        <div class="flex-containerTop">
                            <div class="flex-item">
                                <p class="th"> Request by: </p>
                            </div>
                            <div class="flex-item"> 
                                <asp:Label ID="salesmanName" runat="server" />
                            </div>
                            <div class="flex-item">
                                <p class="th"> Date: </p>
                            </div>
                            <div class="flex-item">
                                <asp:Label ID="requestDate" runat="server" />
                            </div>
                        </div>                                                 
                    </td>
                </tr>

                <tr>
                    <th class="infoth" >
                        New Product Name:
                    </th>
                    <td class="infotd" >
                        <asp:TextBox ID="productName" runat="server"  CssClass="textboxlinelong" />
                        <br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="SubmitCheck" ControlToValidate="productName" Display="Dynamic" ErrorMessage="Product Name should not be empty"></asp:RequiredFieldValidator>
                    </td>
                </tr>

                <tr>
                    <th class="infoth" >
                        Product Application (Please indicate in details):
                    </th>
                    <td class="infotd" >
                        <asp:TextBox ID="productApp" runat="server"  CssClass="textboxlinelong" />
                        <br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="SubmitCheck" ControlToValidate="productApp" Display="Dynamic" ErrorMessage="Product Application should not be emoty"></asp:RequiredFieldValidator>
                    </td>
                </tr>

                <tr>
                    <th class="infoth" >
                        Product Performance (i.e. API/JASO/API GL/DEXRON):
                    </th>
                    <td class="infotd" >
                        <asp:TextBox ID="productPerform" runat="server"  CssClass="textboxlinelong" />
                        <br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="SubmitCheck" ControlToValidate="productPerform" Display="Dynamic" ErrorMessage="Product Performance should not be empty"></asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>

            <br />

            <table id="productDetail">
                <tr class="detailtr" >
                    <th class="detailth" ></th>
                    <th class="detailth" > Bottle </th>
                    <th class="detailth" > 
                        <asp:RadioButton ID="radCap" runat="server" Text="Cap /" GroupName="capLid" Value="Cap" />                        
                        <asp:RadioButton ID="radLid" runat="server" Text="Lid" GroupName="capLid" Value="Lip" />
                        <asp:TextBox ID="caplidText" runat="server" Visible="false" readOnly="true" CssClass="textboxsizeshort" />
                    </th>
                    <th class="detailth" >                         
                        <asp:RadioButton ID="radInduc" runat="server" Text="Induction /" GroupName="inducTab" Value="Induction" />
                        <asp:RadioButton ID="radTab" runat="server" Text="Tab Seal" GroupName="inducTab" Value="Tab Seal" />
                        <asp:TextBox ID="inductabText" runat="server" Visible="false" readOnly="true" CssClass="textboxsizeshort" />
                    </th>
                    <th class="detailth" >
                        <asp:RadioButton ID="radDrum" runat="server" Text="Drum /" GroupName="drumPail" Value="Drum" />
                        <asp:RadioButton ID="radPail" runat="server" Text="Pail" GroupName="drumPail" Value="Pail" />
                        <asp:TextBox ID="drumpailText" runat="server" Visible="false" readOnly="true" CssClass="textboxsizeshort" />
                    </th>
                    <th class="detailth" > Label </th>
                    <th class="detailth" > Sticker </th>
                    <th class="detailth" > Opp Tape </th>
                    <th class="detailth" > Others Please Specify </th>
                </tr>

                <tr class="detailtr" >
                    <td class="detailtd" > Colour </td>
                    <td class="detailtd" >
                        <asp:TextBox ID="colorBottle" runat="server" CssClass="textboxsizeshort" />
                    </td>
                    <td class="detailtd" >
                        <asp:TextBox ID="colourCapLip" runat="server" CssClass="textboxsizeshort" />
                    </td>
                    <td class="detailtd" >
                        <asp:TextBox ID="colorInduction" runat="server" CssClass="textboxsizeshort" />
                    </td>
                    <td class="detailtd" >
                        <asp:TextBox ID="colorDrumPail" runat="server" CssClass="textboxsizeshort" />
                    </td>
                    <td class="detailtd" >
                        <asp:TextBox ID="colorLabel" runat="server" CssClass="textboxsizeshort" />
                    </td>
                    <td class="detailtd" >
                        <asp:TextBox ID="colorSticker" runat="server" CssClass="textboxsizeshort" />
                    </td>
                    <td class="detailtd" >
                        <asp:TextBox ID="colorOpp" runat="server" CssClass="textboxsizeshort" />
                    </td>
                    <td class="detailtd" >
                        <asp:TextBox ID="colorOthers" runat="server" CssClass="textboxsizeshort" />
                    </td>
                </tr>

                <tr class="detailtr" >
                    <td> Original Mould </td>
                    <td class="detailtd" >
                        <asp:TextBox ID="oriBottle" runat="server" CssClass="textboxsizeshort" />
                    </td>
                    <td class="detailtd" >
                        <asp:TextBox ID="oriCapLip" runat="server" CssClass="textboxsizeshort" />
                    </td>
                    <td class="detailtd" >
                        <asp:TextBox ID="oriInduction" runat="server" CssClass="textboxsizeshort" />
                    </td>
                    <td class="detailtd" >
                        <asp:TextBox ID="oriDrumPail" runat="server" CssClass="textboxsizeshort" />
                    </td>
                    <td class="detailtd" >
                        <asp:TextBox ID="oriLabel" runat="server" CssClass="textboxsizeshort" />
                    </td>
                    <td class="detailtd" >
                        <asp:TextBox ID="oriSticker" runat="server" CssClass="textboxsizeshort" />
                    </td>
                    <td class="detailtd" >
                        <asp:TextBox ID="oriOpp" runat="server" CssClass="textboxsizeshort" />
                    </td>
                    <td class="detailtd" >
                        <asp:TextBox ID="oriOthers" runat="server" CssClass="textboxsizeshort" />
                    </td>
                </tr>

                <%--Logo--%>
                <tr class="detailtr" >
                    <td class="detailtd" > Logo </td>
                    <td class="detailtd" >
                        <asp:TextBox ID="logoBottle" runat="server" CssClass="textboxsizeshort" />
                    </td>
                    <td class="detailtd" >
                        <asp:TextBox ID="logoCapLip" runat="server" CssClass="textboxsizeshort" />
                    </td>
                    <td class="detailtd" >
                        <asp:TextBox ID="logoInduction" runat="server" CssClass="textboxsizeshort" />
                    </td>
                    <td class="detailtd" >
                        <asp:TextBox ID="logoDrumPail" runat="server" CssClass="textboxsizeshort" />
                    </td>
                    <td class="detailtd" >
                        <asp:TextBox ID="logoLabel" runat="server" CssClass="textboxsizeshort" />
                    </td>
                    <td class="detailtd" >
                        <asp:TextBox ID="logoSticker" runat="server" CssClass="textboxsizeshort" />
                    </td>
                    <td class="detailtd" >
                        <asp:TextBox ID="logoOpp" runat="server" CssClass="textboxsizeshort" />
                    </td>
                    <td class="detailtd" >
                        <asp:TextBox ID="logoOthers" runat="server" CssClass="textboxsizeshort" />
                    </td>
                </tr>
            </table>
            
        </div>


        <%--coolant--%>
        <div id="tableparent" >
            <div class="tablechildleft" >
                
                <table id="coolanttable" >
                    <tr class="coolanttr" >
                        <th class="coolantth" >
                            Coolant Colour:
                        </th>
                        <td class="coolanttd" >
                            <asp:TextBox ID="coolantColour" runat="server" CssClass="textboxsizeshort" />
                        </td>
                    </tr>

                    <tr class="coolanttr" >
                        <th class="coolantth" >
                            Fragance:
                        </th>
                        <td class="coolanttd" >
                            <asp:RadioButton ID="radCoolYes" runat="server" Text="Yes /" GroupName="CoolYesNo" Value="Yes" />
                            <asp:RadioButton ID="radCoolNo" runat="server" Text="No" GroupName="CoolYesNo" Value="No" />
                            <asp:TextBox ID="coolText" runat="server" Visible="false" readOnly="true" CssClass="textboxsizeshort" />
                        </td>
                    </tr>
                </table>
            </div>

            <div class="tablechildright" >
                <table id="besideCoolant" >
                    <tr class="besidetr" >
                        <th class="besideth" ></th>
                        <th class="besideth" > Registration </th>
                        <th class="besideth" > Date </th>
                        <th class="besideth" > Others Please Specify </th>
                    </tr>

                    <tr class="besidetr" >
                        <td class="besidetd" > API/OEM </td>
                        <td class="besidetd" >
                            <asp:RadioButton ID="apiYes" runat="server" Text="Yes /" GroupName="BesideYesNo" Value="Yes" />
                            <asp:RadioButton ID="apiNo" runat="server" Text="No" GroupName="BesideYesNo" Value="No" />
                            <asp:TextBox ID="apiText" runat="server" Visible="false" readOnly="true" CssClass="textboxsizeshort" />
                        </td>
                        <td class="besidetd" >
                            <asp:TextBox ID="apiDate" runat="server" TextMode="Date" />
                        </td>
                        <td class="besidetd" >
                            <asp:TextBox ID="apiOthers" runat="server" CssClass="textboxsizeshort" />
                        </td>
                    </tr>

                    <tr class="besidetr" >
                        <td class="besidetd" > Form E/D </td>
                        <td class="besidetd" >
                            <asp:RadioButton ID="EDYes" runat="server" Text="Yes /" GroupName="EDYesNo" Value="Yes" />
                            <asp:RadioButton ID="EDNo" runat="server" Text="No" GroupName="EDYesNo" Value="No" />
                            <asp:TextBox ID="edText" runat="server" Visible="false" readOnly="true" CssClass="textboxsizeshort" />
                        </td>
                        <td class="besidetd" >
                            <asp:TextBox ID="EDDate" runat="server" TextMode="Date" />
                        </td>
                        <td class="besidetd" >
                            <asp:TextBox ID="EDOthers" runat="server" CssClass="textboxsizeshort" />
                        </td>
                    </tr>

                    <tr class="besidetr" >
                        <td class="besidetd" > Web & APP </td>
                        <td class="besidetd" >
                            <asp:RadioButton ID="APPYes" runat="server" Text="Yes /" GroupName="APPYesNo" Value="Yes" />
                            <asp:RadioButton ID="APPNo" runat="server" Text="No" GroupName="APPYesNo" Value="No" />
                            <asp:TextBox ID="appText" runat="server" Visible="false" readOnly="true" CssClass="textboxsizeshort" />
                        </td>
                        <td class="besidetd" >
                            <asp:TextBox ID="APPDate" runat="server" TextMode="Date" />
                        </td>
                        <td class="besidetd" >
                            <asp:TextBox ID="APPOthers" runat="server" CssClass="textboxsizeshort" />
                        </td>
                    </tr>
                </table>
            </div>

        </div>

        <div>
            <table id="divPack">
                <tr >
                    <td colspan="4">
                        <div class="flex-container">
                            <div class="flex-item">
                                <p class="th"> Packing Size: </p>
                            </div>
                            <div class="flex-item">
                                <asp:TextBox ID="packSize" runat="server" CssClass="textboxline" />   
                            </div>
                            <div class="flex-item">
                                <p class="th"> Litre * </p>
                            </div>
                            <div class="flex-item">
                                <asp:TextBox ID="litreX" runat="server" CssClass="textboxline" /> (Quantity)
                            </div>
                        </div>
                    </td>
                    <%--<td class="pth" >
                        <p class="th"> Litre * </p>
                    </td>
                    <td>
                        <asp:TextBox ID="litreX" runat="server" CssClass="textboxline" /> (Quantity)
                    </td>--%>
                   <%-- <th> Litre X </th>
                    <td>
                         <asp:TextBox ID="litreX" runat="server" CssClass="textboxline" /> (Quantity)
                    </td>--%>

                </tr>
                    
                <tr>
                    <th class="question"> Product Launching Date: </th>
                    <td>
                        <asp:TextBox ID="launchDate" runat="server" CssClass="textboxline" TextMode="Date" />
                        <%--<p class="th">   1st Production's Quantity: </p>--%>
                        
                    </td>
                    <th class="thh" >  1st Production's Quantity: </th>
                    <td>
                         <asp:TextBox ID="qty1Product" runat="server" CssClass="textboxline" /> 
                    </td>
                </tr>

                <tr>
                    <th class="question"> Stock Code: </th>
                    <td>
                         <asp:TextBox ID="stockCode" runat="server" CssClass="textboxline" />
                    </td>
                    <th> PPM customer's </th>
                    <td>
                         Yes <asp:CheckBox runat="server" ID="PPMYes" />
                    </td>
                </tr>

                <tr>
                    <th class="question"> Remarks: </th>
                    <td colspan="4" >
                        <asp:Textbox ID="remarks" runat="server" TextMode="Multiline" Rows="4" CssClass="fixSize" />
                    </td>
                </tr>
            </table>

        </div>

        <asp:Table ID="tblReview" runat="server" Visible="false" >

            <asp:TableRow>
                <asp:TableHeaderCell> &nbsp; </asp:TableHeaderCell>
                <asp:TableHeaderCell> Signature / Acknowledgement </asp:TableHeaderCell>
                <asp:TableHeaderCell> Name </asp:TableHeaderCell>
                <asp:TableHeaderCell> Expected Completion Date </asp:TableHeaderCell>
                <asp:TableHeaderCell> Comment / Remarks </asp:TableHeaderCell>
            </asp:TableRow>
          
            <asp:TableRow ID="Check" Visible="false" >
                <asp:TableCell> Checked by </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="checkSign" runat="server" CssClass="textboxnoline" />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList ID="ddlCheck" runat="server" Visible="false" />
                    <asp:TextBox ID="checkUser" runat="server" CssClass="textboxnoline" ReadOnly="true" Visible="false" />
                    <asp:TextBox ID="addCheckUser" runat="server"  Visible="false" />
                    <asp:TextBox ID="checkerText" runat="server" Visible="false" />
                    <asp:Button ID="btnCheck" runat="server" Text="Add" Onclick="btnAddCheck_Click"  Visible="false" />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="checkDate" runat="server" CssClass="textboxnoline" TextMode="Date" />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="checkRemark" runat="server" CssClass="textboxnoline" />
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow ID="reviewTag" CssClass="reviewfield" Visible="false" >
                <asp:TableCell CssClass="lblreview" BorderStyle="None"> Review By: </asp:TableCell>
                <asp:TableCell ColumnSpan="4"> &nbsp; </asp:TableCell>
            </asp:TableRow>        

            <asp:TableRow ID="reviewQC" CssClass="reviewQC" Visible="false" >
                <asp:TableCell CssClass="colQC" BorderStyle="None"> QC </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="qcSign" runat="server" CssClass="textboxnoline" />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList ID="ddlQC" runat="server"  Visible="false" />
                    <asp:TextBox ID="qcUser" runat="server" CssClass="textboxnoline" ReadOnly="true" Visible="false" />
                    <asp:TextBox ID="addQCUser" runat="server"  Visible="false" />
                    <asp:TextBox ID="qcText" runat="server" Visible="false" />
                    <asp:Button ID="btnQC" runat="server" Text="Add" Onclick="btnAddQC_Click"  Visible="false" />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="qcDate" runat="server" CssClass="textboxnoline" TextMode="Date" />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="qcRemark" runat="server" CssClass="textboxnoline" />
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow ID="reviewProduct" CssClass="reviewProduct" Visible="false" >
                 <asp:TableCell CssClass="colProduct" BorderStyle="None"> Production </asp:TableCell>
                 <asp:TableCell>
                     <asp:TextBox ID="productSign" runat="server" CssClass="textboxnoline" />
                 </asp:TableCell>
                 <asp:TableCell>
                     <asp:DropDownList ID="ddlProduct" runat="server"  Visible="false" />
                     <asp:TextBox ID="productUser" runat="server" CssClass="textboxnoline" ReadOnly="true" Visible="false" />
                     <asp:TextBox ID="addProductUser" runat="server"  Visible="false" />
                     <asp:TextBox ID="productText" runat="server" Visible="false" />
                     <asp:Button ID="btnProduct" runat="server" Text="Add" Onclick="btnAddProduct_Click"  Visible="false" />
                 </asp:TableCell>
                 <asp:TableCell>
                     <asp:TextBox ID="productDate" runat="server" CssClass="textboxnoline" TextMode="Date" />
                 </asp:TableCell>
                 <asp:TableCell>
                     <asp:TextBox ID="productRemark" runat="server" CssClass="textboxnoline" />
                 </asp:TableCell>
             </asp:TableRow>

            <asp:TableRow ID="reviewPurchase" CssClass="reviewPurchase" Visible="false" >
                <asp:TableCell CssClass="colPurchase" BorderStyle="None"> Purchasing </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="purchaseSign" runat="server" CssClass="textboxnoline" />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList ID="ddlPurchase" runat="server" Visible="false" />
                    <asp:TextBox ID="purchaseUser" runat="server" CssClass="textboxnoline" ReadOnly="true" Visible="false" />
                    <asp:TextBox ID="addPurchaseUser" runat="server"  Visible="false" />
                    <asp:TextBox ID="purchaseText" runat="server" Visible="false" />
                    <asp:Button ID="btnPurchase" runat="server" Text="Add" Onclick="btnAddPurchase_Click"  Visible="false" />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="purchaseDate" runat="server" CssClass="textboxnoline" TextMode="Date" />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="purchaseRemark" runat="server" CssClass="textboxnoline" />
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow ID="rowWare" Visible="false" >
                <asp:TableCell> Warehouse </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="wareSign" runat="server" CssClass="textboxnoline" />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList ID="ddlWarehouse" runat="server" Visible="false" />
                    <asp:TextBox ID="wareUser" runat="server" CssClass="textboxnoline" ReadOnly="true" Visible="false" />
                    <asp:TextBox ID="addWarehouseUser" runat="server" Visible="false" />
                    <asp:TextBox ID="wareText" runat="server" Visible="false" />
                    <asp:Button ID="btnWarehouse" runat="server" Text="Add" Onclick="btnAddWarehouse_Click" Visible="false" />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="wareDate" runat="server" CssClass="textboxnoline" TextMode="Date" />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="wareRemark" runat="server" CssClass="textboxnoline" />
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow ID="rowAP" Visible="false" >
                <asp:TableCell> A&P (Marketing) </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="anpSign" runat="server" CssClass="textboxnoline" />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList ID="ddlAnP" runat="server" Visible="false" />
                    <asp:TextBox ID="anpUser" runat="server" CssClass="textboxnoline" ReadOnly="true" Visible="false" />
                    <asp:TextBox ID="addAnPUser" runat="server"  Visible="false" />
                    <asp:TextBox ID="marketText" runat="server" Visible="false" />
                    <asp:Button ID="btnAnP" runat="server" Text="Add" Onclick="btnAddAnP_Click" Visible="false" />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="anpDate" runat="server" CssClass="textboxnoline" TextMode="Date" />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="anpRemark" runat="server" CssClass="textboxnoline" />
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow ID="rowIT" Visible="false" >
                <asp:TableCell> IT </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="itSign" runat="server" CssClass="textboxnoline" />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList ID="ddlIT" runat="server" Visible="false" />
                    <asp:TextBox ID="itUser" runat="server" CssClass="textboxnoline" ReadOnly="true" Visible="false" />
                    <asp:TextBox ID="addITUser" runat="server" Visible="false" />
                    <asp:TextBox ID="itText" runat="server" Visible="false" />
                    <asp:Button ID="btnIT" runat="server" Text="Add" Onclick="btnAddIT_Click" Visible="false" />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="itDate" runat="server" CssClass="textboxnoline" TextMode="Date" />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="itRemark" runat="server" CssClass="textboxnoline" />
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow ID="rowApprove" Visible="false" >
                <asp:TableCell> Approved by </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="approveSign" runat="server" CssClass="textboxnoline" />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList ID="ddlApprove" runat="server" Visible="false" />
                    <asp:TextBox ID="approveUser" runat="server" CssClass="textboxnoline" ReadOnly="true" Visible="false" />
                    <asp:TextBox ID="approveText" runat="server" Visible="false" />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="approveDate" runat="server" CssClass="textboxnoline no-print" TextMode="Date" />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="approveRemark" runat="server" CssClass="textboxnoline" />
                </asp:TableCell>
            </asp:TableRow>

        </asp:Table>

        <asp:CustomValidator ID="CustomValidator1" ClientValidationFunction="validateTextboxes" runat="server" ValidationGroup="AcknowCheck" Display="Dynamic"></asp:CustomValidator>
        <asp:CustomValidator ID="CustomValidator2" ClientValidationFunction="validateTextboxes" runat="server" ValidationGroup="AuthorCheck" Display="Dynamic"></asp:CustomValidator>
        <asp:CustomValidator ID="CustomValidator3" ClientValidationFunction="validateTextboxes" runat="server" ValidationGroup="ApproveCheck" Display="Dynamic"></asp:CustomValidator>


        <asp:TextBox ID="adminUser" runat="server" Visible="false" />

        <div class="btnClass">
            <asp:Button ID="btnRequest" runat="server" Text="Request" CausesValidation="true" ValidationGroup="RequestCheck" CssClass="no-print" Onclick="btnRequest_Click" />
            <asp:Button ID="btnDraft" runat="server" Text="Save as Draft" CssClass="no-print" Onclick="btnDraft_Click" />
            <asp:Button ID="btnAcknow" runat="server" Text="Acknowledge" CausesValidation="true" ValidationGroup="AcknowCheck" CssClass="no-print" Onclick="btnAcknow_Click" Visible="false" />
            <asp:Button ID="btnAuthor" runat="server" Text="Authorised" CausesValidation="true" ValidationGroup="AuthorCheck" CssClass="no-print" Onclick="btnAuthor_Click" Visible="false" />
            <asp:Button ID="btnPrint" runat="server" Text="Print" OnClientClick="printPage(); return false;" CssClass="no-print" Visible="false" />
            <asp:Button ID="btnApprove" runat="server" Text="Approve" CausesValidation="true" ValidationGroup="ApproveCheck" CssClass="no-print" Onclick="btnApprove_Click" Visible="false" />
            <asp:Button ID="btnReject" runat="server" Text="Reject" CssClass="no-print" Onclick="btnReject_Click" Visible="false" />
            <asp:Button ID="btnSendMail" runat="server" Text="Send Mail" Onclick="btnSendMail_Click" Visible="false" />
        </div>

        <div class="btnClass">
            <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="no-print" Onclick="btnClose_Click" />
        </div>

    </form>

    

    <script>

        console.log("HMMMMMMMM");

        var statusElement = document.getElementById('status');
        var STATUS = statusElement ? statusElement.innerText.trim() : '';

        console.log("This is: " + STATUS);

        

        function validateTextboxes(oSource, args) {
            console.log("NEHHHHHHHHHHHH");

            var statusElement = document.getElementById('status');
            var STATUS = statusElement ? statusElement.innerText.trim() : '';

            var checkUser = document.getElementById("checkUser");
            var qcUser = document.getElementById("qcUser");
            var productUser = document.getElementById("productUser");
            var purchaseUser = document.getElementById("purchaseUser");
            var wareUser = document.getElementById("wareUser");
            var anpUser = document.getElementById("anpUser");
            var itUser = document.getElementById("itUser");
            var approveUser = document.getElementById("approveUser");

            var valid = true;
            var errorMessage = "";
            args.IsValid = true;

            console.log("WAHAHAHAHAHAHAH");

            if (STATUS.includes("Checker")) {
                console.log("HIHIHIHIHIHIHIHI");
                if (checkUser === "" || document.getElementById('checkSign').value.trim() === "" || document.getElementById('checkDate').value.trim() === "" || document.getElementById('checkRemark').value.trim() === "") {
                    valid = false;
                    errorMessage += "Fill in all fields for Checker before APPROVE!!.";
                }
            }
            if (STATUS.includes("QC")) {
                if (qcUser === "" || document.getElementById('qcSign').value.trim() === "" || document.getElementById('qcDate').value.trim() === "" || document.getElementById('qcRemark').value.trim() === "") {
                    valid = false;
                    errorMessage += "Fill in all fields for QC before AUTHORISE!!.";
                }
            }
            if (STATUS.includes("Product")) {
                if (productUser === "" || document.getElementById('productSign').value.trim() === "" || document.getElementById('productDate').value.trim() === "" || document.getElementById('productRemark').value.trim() === "") {
                    valid = false;
                    errorMessage += "Fill in all fields for Product before AUTHORISE!!.";
                }
            }
            if (STATUS.includes("Purchase")) {
                if (purchaseUser === "" || document.getElementById('purchaseSign').value.trim() === "" || document.getElementById('purchaseDate').value.trim() === "" || document.getElementById('purchaseRemark').value.trim() === "") {
                    valid = false;
                    errorMessage += "Fill in all fields for Purchase before AUTHORISE!!.";
                }
            }
            if (STATUS.includes("Warehouse")) {
                if (wareUser === "" || document.getElementById('wareSign').value.trim() === "" || document.getElementById('wareDate').value.trim() === "" || document.getElementById('wareRemark').value.trim() === "") {
                    valid = false;
                    errorMessage += "Fill in all fields for Ware before ACKNOWLEDGE!!.";
                }
            }
            if (STATUS.includes("Marketing")) {
                if (anpUser === "" || document.getElementById('anpSign').value.trim() === "" || document.getElementById('anpDate').value.trim() === "" || document.getElementById('anpRemark').value.trim() === "") {
                    valid = false;
                    errorMessage += "Fill in all fields for ANP before ACKNOWLEDGE!!.";
                }
            }
            if (STATUS.includes("IT")) {
                if (itUser === "" || document.getElementById('itSign').value.trim() === "" || document.getElementById('itDate').value.trim() === "" || document.getElementById('itRemark').value.trim() === "") {
                    valid = false;
                    errorMessage += "Fill in all fields for IT before ACKNOWLEDGE!!.";
                }
            }
            if (STATUS.includes("Approve")) {
                if (approveUser === "" || document.getElementById('approveSign').value.trim() === "" || document.getElementById('approveDate').value.trim() === "" || document.getElementById('approveRemark').value.trim() === "") {
                    valid = false;
                    errorMessage += "Fill in all fields for Approve before APPROVE!!.";
                }
            }

            if (!valid) {
                alert(errorMessage);
            }

            args.IsValid = valid;
            return valid;
        }


        // use for int exp: qty, litre, size
        function checkNum(oSrouce, args) {
            var CrdLmtNum = args.Value;
            var message = "";

            if (/[\+/-\/a-zA-Z]/.test(CrdLmtNum)) {
                message = "Size/QTY - Please enter a valid number"
                args.IsValid = false;

            } else {
                var number = Number(CrdLmtNum);
                if (isNaN(number) || number <= 0) {
                    message = "Size/QTY - Please insert a positive number!";
                    args.IsValid = false;
                } else {
                    args.IsValid = true;
                }
            }

            if (!args.IsValid) {
                alert(message)
            }
        }

    </script>

</body>
</html>
