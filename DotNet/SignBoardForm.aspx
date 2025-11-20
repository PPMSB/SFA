<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignBoardForm.aspx.cs" Inherits="DotNet.SignBoardForm" %>

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
            /*border: 1px solid;*/
        }

        table {
            width: 100%;
            border-collapse: collapse;
        }

        td{
            height:27px;
        }
    </style>
    <script type="text/javascript">
    function printPanel() {
        var printContents = document.getElementById('<%= pnlContent.ClientID %>').innerHTML;
        var originalContents = document.body.innerHTML;
        document.body.innerHTML = printContents;
        window.print();
        document.body.innerHTML = originalContents;
    }
    </script>
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
                        <div style="width: 100%; font-size:10px">
                       
                               
                                <%--Title--%>
                                <div class="mt-3 p-2" style="display: flex; justify-content: space-between; align-items: center; border-bottom: 1px solid grey; width: 100%">

                                    <div style="line-height: 0.3;">

                                        <p style="font-weight: bold; font-size: 14px">POSIM PETROLEUM MARKETING SDN BHD</p>  <%--198501007134 (139576-M)--%>
                                        <p style="font-size: 20px">SIGNBOARD FORM</p>
                                        <%--<p style="font-size: 12px">Wisma Posim, Lot 72, Persiaran Jubli Perak, 40000 Shah Alam, Selangor D.E</p>
                                        <p style="font-size: 12px">P.O.Box 7695, 40724 Shah Alam, Selangor Darul Ehsan, Malaysia</p>
                                        <p style="font-size: 12px">Tel: +603 5102 2999 (Hunting Line)</p>--%>
                                    </div>
                                    <div>
                                        <%--<div id="divLabelDraft" runat="server" visible="false;">State As Draft</div>--%>
                                        <div class="label-row">
    Application No :  <asp:Label ID="Label_AppNo" runat="server"></asp:Label><br />
</div>
                                        <div class="label-row">
                                            Submission Date :  <asp:Label ID="Label_SubDate" runat="server"></asp:Label><br />
                                        </div>
                                    </div>

                                    <%--<img class="Company_Icon" alt="POSIM PETROLEUM MARKETING" src="<%= GLOBAL_VAR.GLOBAL.BaseDirectory + "RESOURCES/PosimNewLogo.jpeg"%>" style="width: 25%;" />--%>
                                </div>

                            <%--Customer Info--%>
                                <div><table>
  <tbody>
    <tr>
      <td style="width: 14%;">CUST. NAME  :</td>
      <td style="width: 36%; border-bottom: 1px solid grey;"><asp:Label ID="lblCustName" runat="server"></asp:Label></td>

      <td style="width: 14%;">HQ/ BR  :</td>
      <td style="width: 36%; border-bottom: 1px solid grey;"><asp:Label ID="lblHQBR" runat="server"></asp:Label></td>
    </tr>
    <tr>
      <td>A/C NO.  :</td>
      <td style="border-bottom: 1px solid grey;"><asp:Label ID="lblCustAcc" runat="server"></asp:Label></td>

      <td>CLASS  :</td>
      <td style="border-bottom: 1px solid grey;"><asp:Label ID="lblClass" runat="server"></asp:Label></td>
    </tr>
    <tr>
      <td>TEL / HP NO.  :</td>
      <td style="border-bottom: 1px solid grey;"><asp:Label ID="lblCustTelNo" runat="server"></asp:Label></td>

      <td>SALESMAN  :</td>
      <td style="border-bottom: 1px solid grey;"><asp:Label ID="lblSalesman" runat="server"></asp:Label></td>
    </tr>
    <tr>
      <td>CONTACT PERSON  :</td>
      <td style="border-bottom: 1px solid grey;"><asp:Label ID="lblCustContactPerson" runat="server"></asp:Label></td>

      <td>DEPOSIT AMOUNT/ %  :</td>
      <td style="border-bottom: 1px solid grey;"><asp:Label ID="lblDepositAmt" runat="server"></asp:Label></td>
    </tr>
    <tr>
      <td>REQUEST TYPE  :</td>
      <td style="border-bottom: 1px solid grey;"><asp:Label ID="lblRequestType" runat="server"></asp:Label></td>
      
      <td>DEPOSIT FOR EOR/SC  :</td>
      <td style="border-bottom: 1px solid grey;"><asp:Label ID="lblDepositEORSC" runat="server"></asp:Label></td>
    </tr>
      <tr></tr>
  </tbody>
</table>
</div>

                            <div></br>
                                <table style="border: 1px solid grey; border-collapse: collapse; text-align:center;">
  <tr>
    <th style="border: 1px solid grey;">NO.</th>
    <th style="border: 1px solid grey;">EQUIPMENTS/ SIGNBOARD ITEM</th>
    <th style="border: 1px solid grey;">QTY</th>
    <th style="border: 1px solid grey;">SIZE</th>
  </tr>
  <tr>
    <td style="width:9%; border: 1px solid grey;">1</td>
    <td style="width:45%; border: 1px solid grey;"><asp:Label ID="lblItem1" runat="server" /></td>
    <td style="width:18%; border: 1px solid grey;"><asp:Label ID="lblQty1" runat="server" /></td>
    <td style="width:18%; border: 1px solid grey;"><asp:Label ID="lblSize1" runat="server" /></td>
  </tr>
  <tr>
    <td style="border: 1px solid grey;">2</td>
    <td style="border: 1px solid grey;"><asp:Label ID="lblItem2" runat="server" /></td>
    <td style="border: 1px solid grey;"><asp:Label ID="lblQty2" runat="server" /></td>
    <td style="border: 1px solid grey;"><asp:Label ID="lblSize2" runat="server" /></td>
  </tr>
  <tr>
    <td style="border: 1px solid grey;">3</td>
    <td style="border: 1px solid grey;"><asp:Label ID="lblItem3" runat="server" /></td>
    <td style="border: 1px solid grey;"><asp:Label ID="lblQty3" runat="server" /></td>
    <td style="border: 1px solid grey;"><asp:Label ID="lblSize3" runat="server" /></td>
  </tr>
  <tr>
    <td style="border: 1px solid grey;"></td>
    <td style="border: 1px solid grey;"></td>
    <td style="border: 1px solid grey;">Total Cost :</td>
    <td style="border: 1px solid grey;"><asp:Label ID="lblTotalCost" runat="server" /></td>
  </tr>
</table>
</div>

                                <div></br>
                                    <table>
                                    <tbody>
                                        <tr>
                                            <td style="width: 14%;">Item Deliver To :</td><td style="width: 36%; border-bottom: 1px solid grey;"><asp:Label ID="lblItemDeliver" runat="server"></asp:Label></td>
                                            <td style="width: 14%;">Supplier :</td><td style="width: 36%; border-bottom: 1px solid grey;"><asp:Label ID="lblSupplier" runat="server"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td>Deliver Address :</td><td style="border-bottom: 1px solid grey;" ><asp:Label ID="lblAddress" runat="server"></asp:Label></td>
                                            <td>Beneficiary Bank A/C. :</td><td style="border-bottom: 1px solid grey;"><asp:Label ID="lblBeneficiaryBank" runat="server"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td>Remarks :</td><td style="border-bottom: 1px solid grey;"><asp:Label ID="lblRemarks" runat="server"></asp:Label></td>
                                            <td>Workshop Name:</td><td style="border-bottom: 1px solid grey;"><asp:Label ID="lblSBWorkshopName" runat="server"></asp:Label></td>

                                        </tr>
                                        </tbody>
                                     </table>
                                </div>

                                 <%--SIGNAGE REQUESTED  ////////////////////////////////////////////////////////////////////////////////////--%> 
                              
                                <%--<form id="form2" runat="server">--%>
                                        <div style="height:40px;"></br>
                                       <p style="height:3px;"> <b>SIGNAGE REQUESTED</b> (* PPM will not be responsible for offence committed due to licensing.)</p>
                            <asp:RadioButtonList ID="rblRequestSign" runat="server" RepeatDirection="Horizontal" Enabled="false"  style="height:5px;">
            <asp:ListItem Text="To pre-apply license" Value="1"></asp:ListItem>
            <asp:ListItem Text="To install pending license" Value="2"></asp:ListItem>
            <asp:ListItem Text="Use existing license" Value="3"></asp:ListItem>
        </asp:RadioButtonList>
    </div>
                                        <%--</form>--%>
                                
                                <%--Past 6 Months Purchase Record  ////////////////////////////////////////////////////////////////////////////////////--%> 
                               <div></br>
                                   <p style="height:3px;"><b>Past 6 Months Purchase Record</b></p>
                                    <div id="Div8" runat="server" class="mb-3">
                                       <%-- <div class="col-12">--%>
                                            <asp:Label ID="lblPastRecord" runat="server"></asp:Label>
                                        <%--</div>--%>
                                    </div>
                                   <div style="float:right;" ><b><asp:Label ID="lblAvgSalesMonth" runat="server"></asp:Label></b>
                                     
                                   </div>
                               </div>
                                 <%--Location Map  ////////////////////////////////////////////////////////////////////////////////////--%>
                               
                                <div>
                                    <p style="height:3px;"><b>Map Location</b></p>
<table>
    
  <tr>
    <td rowspan="4" style="text-align:center; vertical-align:middle; width: 260px;">
        <!-- Map Type label -->
      <asp:Label ID="lblMapType" runat="server" />
      <!-- Only one of these divs will be visible -->
      <div id="DivDisplayMapA" runat="server" visible="false">
        <img src="RESOURCES/Location_Map_Type_A.png" alt="Map Type A" style="width:210px; height:120px; border:1px solid grey;" />
      </div>
      <div id="DivDisplayMapB" runat="server" visible="false">
        <img src="RESOURCES/Location_Map_Type_B.png" alt="Map Type B" style="width:210px; height:120px; border:1px solid grey;" />
      </div>
      <div id="DivDisplayMapC" runat="server" visible="false">
        <img src="RESOURCES/Location_Map_Type_C.png" alt="Map Type C" style="width:210px; height:120px; border:1px solid grey;" />
      </div>
    </td>
    <td style="width:14%;">Map Description :</td>
    <td style="width:36%; border-bottom: 1px solid grey;"><asp:Label ID="lblMapDesc" runat="server"></asp:Label></td>
  </tr>
  <tr>
    <td>Map Remark :</td>
    <td style="border-bottom: 1px solid grey;"><asp:Label ID="lblMapRemark" runat="server"></asp:Label></td>
  </tr>
  <tr>
    <td>Traffic Density:</td>
    <td style="border-bottom: 1px solid grey;"><asp:Label ID="lblTrafficDensity" runat="server"></asp:Label></td>
  </tr>
  <tr>
    <td>Signboard Visibility:</td>
    <td style="border-bottom: 1px solid grey;"><asp:Label ID="lblSignVisibility" runat="server"></asp:Label></td>
  </tr>
</table>

</div>

                                 <%--Comment from Sales Person  ////////////////////////////////////////////////////////////////////////////////////--%>
                               
                                 <div>
                                     </br>
                                     <p><b>Comment from Sales Person</b></p>
                                     <table>
<tbody>
<tr>
<td style="width:14%;">Type of Service Center :</td>
<td style="width:36%; border-bottom: 1px solid grey;"><asp:Label ID="lblTypeOfServiceCenter" runat="server"></asp:Label></td>

<td style="width:14%;">Workshop Facilities :</td>
<td style="width:36%; border-bottom: 1px solid grey;"><asp:Label ID="lblWorkshopFacilities" runat="server"></asp:Label></td>

</tr>
<tr>
<td>Owner Experience :</td>
<td style="border-bottom: 1px solid grey;"><asp:Label ID="lblOwnerExperience" runat="server"></asp:Label></td>

<td>Workshop Size/Type :</td>
<td style="border-bottom: 1px solid grey;"><asp:Label ID="lblWorkshopSizeType" runat="server"></asp:Label></td>

</tr>

<tr>
<td>No. of Mechanics :</td>
<td style="border-bottom: 1px solid grey;"><asp:Label ID="lblNumberOfMechanics" runat="server"></asp:Label></td>

<td>Workshop Status :</td>
<td style="border-bottom: 1px solid grey;"><asp:Label ID="lblWorkshopStatus" runat="server"></asp:Label></td>
</tr>
<tr>
<td>Year Of Establishment :</td>
<td style="border-bottom: 1px solid grey;"><asp:Label ID="lblYearOfEstablishment" runat="server"></asp:Label></td>
<td></td><td></td>
</tr>

</tbody>
</table>
</div>   
                                
                                <div>
                                     <table>
<tr>
<td style="width:14%">LOYALTY POINT BAL :</td>
<td style="width:36%;border-bottom: 1px solid grey;"><asp:Label ID="lblLoyalyPoint" runat="server"></asp:Label></td>
<td style="width:14%">CUST. CLASS :</td>
<td style="width:36%;border-bottom: 1px solid grey;"><asp:Label ID="lblCustClass" runat="server"></asp:Label></td>
</tr>
<td>EOL PERFORMANCE :</td>
<td style="border-bottom: 1px solid grey;"><asp:Label ID="lblEOLPerformance" runat="server"></asp:Label></td>
<td>EOR PERFORMANCE :</td>
<td style="border-bottom: 1px solid grey;"><asp:Label ID="lblEORPerformance" runat="server"></asp:Label></td>
<tr>

                                    </table>
                                </div>

                            <div>
                                <table>
                                    <tr>
                                        <td style="width:14%">HOD Remarks :</td>
                                        <td style="width:86%;border-bottom: 1px solid grey;"><asp:Label ID="lblAllRemarks" runat="server" Width="400px"></asp:Label></td>

                                    </tr>
                                    <tr><td>Process Date :</td>
                                        <td style="border-bottom: 1px solid grey;"><asp:Label ID="lblGetProcessDate" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>Process Status :</td>
                                        <td style="border-bottom: 1px solid grey;"><asp:Label ID="lblGetProcessStat" runat="server" Width="400px"></asp:Label></td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>

                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <%--<asp:Button ID="BtnRedirect" runat="server" OnClick="btnGeneratePDF_Click" Text="Print" class="glow_green button-margin" />--%>
        <asp:Button ID="btnPrint" runat="server" Text="Print" class="glow_green button-margin" OnClientClick="printPanel(); return false;" />
    </form>
</body>
</html>
