<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompanyReport.aspx.cs" Inherits="DotNet.CompanyReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="shortcut icon" href="RESOURCES/LFIB_icon.ico" />

    <script src="scripts/GoToTab.js"></script>
    <link href="STYLES/xxtra_global_Content_Adjuster.css" rel="stylesheet" />
    <link href="STYLES/xtra_Grid_View.css" rel="stylesheet" />
    <link href="STYLES/xtra_Glowing.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/5.3.0/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://cdn.datatables.net/1.13.6/css/dataTables.bootstrap5.min.css" />
    <link href="STYLES/thirdForm.css" rel="stylesheet" />

    <title>New Customer</title>
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="mobile-web-app-capable" content="yes" />

    <script src="scripts/BrowserReload_ThroughHistory.js"></script>
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/1.11.6/css/jquery.dataTables.min.css">
<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
<script src="https://cdn.datatables.net/1.11.6/js/jquery.dataTables.min.js"></script>
    <link rel="stylesheet" href="https://cdn.datatables.net/1.13.6/css/jquery.dataTables.css" />
<script src="https://cdn.datatables.net/1.13.6/js/jquery.dataTables.js"></script>

    <script>
        function goBack() {
            window.location.href = 'NewCustomer.aspx'; 
        }
    </script>

    <div class="header1">
            <header>
                <div class="row">
            <div class="mobile_hidden">
                <div class="col-3 col-s-3 image_icon">
                    <img src="RESOURCES/POSIM PETROLEUM MARKETING.png" class="image_resize" />
                </div>

                <div class="col-9 col-s-9 image_title">
                    <h1>New Customer</h1>
                </div>
            </div>

                    
            <!--==============================================================================-->
            <link href="STYLES/xtra_top_navigation.css" rel="stylesheet" />
            <div class="topnav" id="myTopnav">
                <a href="MainMenu.aspx">Home</a>  
                <a href="New_Customer.aspx" id="NewCustomer2" runat="server" visible="true">New Customer</a>
                <a href="CustomerMaster.aspx" class="active" id="CustomerMasterTag2" runat="server" visible="false">Customer</a>
                <a href="SFA.aspx" id="SFATag2" runat="server" visible="false">Sales Automation</a>
                <a href="SalesQuotation.aspx" id="SalesQuotation2" runat="server" visible="false">Quotation</a>
                <a href="Payment.aspx" id="PaymentTag2" runat="server" visible="false">Payment</a>
                <a href="Redemption.aspx" id="RedemptionTag2" runat="server" visible="false">Redemption</a>
                <%--<a href="EOR.aspx" id="EORTag2" runat="server" visible="false">EOR</a>--%>
                <a href="SignboardEquipment.aspx" id="EORTag2" runat="server" visible="true">Sign & Equip</a>
                <a href="InventoryMaster.aspx" id="InventoryMasterTag2" runat="server" visible="false">Inventory</a>
                <a href="WClaim.aspx" id="WClaimTag2" runat="server" visible="false">Warranty</a>
                <%--<a href="SignboardEquipment.aspx" id="SignboardTag2" runat="server">Sign & Equip</a>--%>
                <a href="Setting.aspx" id="SettingTag2" runat="server">Setting</a>
                <a href="LoginPage.aspx" class="Log_Out top_nav_logout_properties">
                    <asp:Label runat="server" CssClass="fa fa-sign-out" Style="font-size: 20px;"></asp:Label>
                    <asp:Label runat="server" Text="Logout" Font-Bold="true"></asp:Label>
                    <%--                        <img src="RESOURCES/LogOut.png" />
                        <img src="RESOURCES/LogOut_h.png" class="top_nav_logout_highlight" />--%>
                </a>

                <a href="javascript:void(0);" class="icon" onclick="topnavigation()">
<%--                    <img src="RESOURCES/menu.png" />--%>
                <div class="container" onclick=" myFunction(this);">
                    <div class="bar1"></div>
                    <div class="bar2"></div>
                    <div class="bar3"></div>
                </div>
                </a>
                <script src="scripts/top_navigation.js"></script>
            </div>
    </header>
        </div>
</head>
   
<body>

            <!--==============================================================================-->
                    <div class="col-12 col-s-12">
                       <div class="function">

                <div class="function">
                </div>
        </div>
                </div>
     <form id="CustomerReport"  runat="server" onsubmit="return submitForm()">
    
    <asp:TextBox ID="HiddenNewCustID" runat="server" style="display:none;"></asp:TextBox>


         <div class="CustomerReport">
    <table>
                <tr>
                   <th> Salesman Name:</th>
                <td>
                    <asp:TextBox runat="server" ID="txtSalesmanName" ReadOnly="true"></asp:TextBox>
                </td>


                     <th> Status:</th>
                    <td>
                        <asp:TextBox runat="server" ID="txtStatus" ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th> Salesman Code:</th>
                    <td>
                        <asp:TextBox runat="server" ID="txtSalesmanCode" ReadOnly="true"></asp:TextBox>
                    </td>

                      <th> Form No:</th>
                    <td>
                        <asp:TextBox runat="server" ID="txtFormNo" ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th> Salesman's Remark:</th>
                    <td>
                        <asp:TextBox runat="server" ID="txtSalesmanRemark" ></asp:TextBox>
                    </td>
                </tr>
            </table>
<table>
    <h2>Customer Information</h2>
    <tr>
        <th>
            <asp:Label ID="lblCustomerName" runat="server">Customer Name:</asp:Label>
        </th>
        <td>
            <asp:TextBox ID="txtCustomerName" runat="server" Required="true" ReadOnly="true"></asp:TextBox>
        </td>

        <th>
            <asp:Label ID="lblCustomerCategory" runat="server">Customer Category:</asp:Label>
        </th>
        <td>
            <asp:TextBox ID="txtCustomerCategory" runat="server" ReadOnly="true"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <th>
            <asp:Label ID="lblCustomerAddress" runat="server">Customer Full Address:</asp:Label>
        </th>
        <td>
            <asp:TextBox ID="txtCustomerAddress" runat="server" TextMode="MultiLine" Rows="4" Required="true"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <th>
            <asp:Label ID="lblCity" runat="server">City:</asp:Label>
        </th>
        <td>
            <asp:TextBox ID="txtCity" runat="server" Required="true"></asp:TextBox>
        </td>

        <th>
            <asp:Label ID="lblPhone" runat="server">Phone</asp:Label>
        </th>
        <td>
            <asp:TextBox ID="txtPhone" runat="server" Minlength="10" MaxLength="12"></asp:TextBox>
        </td>
    </tr>
    
        <tr>
        <th>
            <asp:Label ID="lblPostalCode" runat="server">Postal Code:</asp:Label>
        </th>
        <td>
            <asp:TextBox ID="txtPostalCode" runat="server" Required="true" ReadOnly="true"></asp:TextBox>
        </td>

        <th>
            <asp:Label ID="lblCustomerClass" runat="server">Customer Class:</asp:Label>
        </th>
        <td>
            <asp:TextBox ID="txtCustomerClass" runat="server" ReadOnly="true"></asp:TextBox>
        </td>
    </tr>

    <tr>
        <th>
            <asp:Label ID="lblState" runat="server" >State:</asp:Label>
        </th>
        <td>
            <asp:DropDownList ID="ddlState" runat="server" required></asp:DropDownList>
        </td>

        <th>
            <asp:Label ID="lblCoordinate" runat="server">Coordinate:</asp:Label>
        </th>
        <td>
            <asp:TextBox ID="txtCoordinate" runat="server" ReadOnly="true"></asp:TextBox>
        </td>
    </tr>

    <tr>
        <th>
            <asp:Label ID="lblRocRob" runat="server">ROC/ROB:</asp:Label>
        </th>
        <td>
            <asp:TextBox ID="txtRocRob" runat="server" ReadOnly="true"></asp:TextBox>
        </td>

        <th>
            <asp:Label ID="lblCreditTerm" runat="server">Credit Term:</asp:Label>
        </th>
        <td>
            <asp:TextBox ID="txtCreditTerm" runat="server" ReadOnly="true"></asp:TextBox>
        </td>
    </tr>

   <tr>
          <th>
            <asp:Label ID="lblTIN" runat="server">TIN (Tax Identification No):</asp:Label>
        </th>
        <td>
            <asp:TextBox ID="txtTIN" runat="server" ></asp:TextBox>
        </td>

         <th>
            <asp:Label ID="lblCreditLimit" runat="server">Credit Limit:</asp:Label>
        </th>
        <td>
            <asp:TextBox ID="txtCreditlimit" runat="server" ReadOnly="true"></asp:TextBox>
        </td>
    </tr>

    <tr>
          <th>
            <asp:Label ID="lblCustType" runat="server">Customer Type:</asp:Label>
        </th>
        <td>
            <asp:TextBox ID="txtCustType" runat="server" ReadOnly="true"></asp:TextBox>
        </td>
    </tr>

<tr>
    <th>
        <asp:Label ID="lblCustomerType" runat="server">Existing Customer Type:</asp:Label>
    </th>
    <td colspan="2">
        <asp:RadioButton ID="rbCustomerTypeNA" runat="server" Text="NA" GroupName="customerType" Checked="true" />
    </td>
</tr>

<tr>
    <td></td>
    <td colspan="2">
        <asp:RadioButton ID="rbCustomerTypeExisting" runat="server" Text="Existing Document" GroupName="customerType" />
    </td>
</tr>
<tr>
    <td></td>
    <td colspan="2">
        <asp:RadioButton ID="rbCustomerTypeNew" runat="server" Text="New Document" GroupName="customerType" />
    </td>


</tr>

    <tr>
    <th>
        <asp:Label ID="lblMainCustomerAccount" runat="server">Main Customer Account:</asp:Label>
    </th>
    <td colspan="2">
        <asp:TextBox ID="txtMainCustomerAccount" runat="server" ></asp:TextBox>
    </td>
    </tr>
    </table>

<table >
    <h2>Customer Information - Guarantor</h2>
    <h5 class="white-background" >Guarantor - 1</h5>
    <tr>
        <th>Title:</th>
        <td>
            <asp:DropDownList ID="ddlTitle" runat="server">
                <asp:ListItem Text="-- SELECT --" Value="Null" />
                <asp:ListItem Text="Encik" Value="Encik" />
                <asp:ListItem Text="Cik" Value="Cik" />
                <asp:ListItem Text="Datin" Value="Datin" />
                <asp:ListItem Text="Datuk" Value="Datuk" />
            </asp:DropDownList>
        </td>
       
        <th>Type:</th>
        <td>
            <asp:DropDownList ID="ddlType" runat="server">
                <asp:ListItem Text="Blank" Value="Blank" />
                <asp:ListItem Text="Corporate" Value="Corporate" />
                <asp:ListItem Text="Personal" Value="Personal" />
                <asp:ListItem Text="BG" Value="BG" />
                <asp:ListItem Text="Sole Proprietor" Value="Sole Proprietor" />
                <asp:ListItem Text="Partner" Value="Partner" />
                <asp:ListItem Text="3rd Party" Value="3rd Party" />
                <asp:ListItem Text="Director" Value="Director" />
                <asp:ListItem Text="Undertaking Letter" Value="Undertaking Letter" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <th>Name:</th>
        <td>
            <asp:TextBox ID="txtName" runat="server" />
        </td>
        <th>Birth Date:</th>
        <td>
            <asp:TextBox ID="txtBirthDate" runat="server" TextMode="Date" />
        </td>
    </tr>
    <tr>
        <th>IC No:</th>
        <td>
            <asp:TextBox ID="txtICNo" runat="server" />
        </td>

        <th>Mobile Phone:</th>
        <td>
            <asp:TextBox ID="txtMobilePhone1" runat="server" Minlength="10" MaxLength="12" />
        </td>
    </tr>
    <tr>
        <th>Old IC:</th>
        <td>
            <asp:TextBox ID="txtOldIC" runat="server" />
        </td>
        <th>Phone:</th>
        <td>
            <asp:TextBox ID="TextBoxPhone1" runat="server" Minlength="10" MaxLength="12" />
        </td>
    </tr>
    <tr>
         <th>Address</th>
        <td > 
            <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" Rows="1" />
        </td>
    </tr>
</table>
            


            <table >
    <h5 class="white-background" >Guarantor - 2 </h5>
    <tr>
        <th>Title:</th>
        <td>
            <asp:DropDownList ID="DropDownList1" runat="server">
                <asp:ListItem Text="-- SELECT --" Value="Null" />
                <asp:ListItem Text="Encik" Value="Encik" />
                <asp:ListItem Text="Cik" Value="Cik" />
                <asp:ListItem Text="Datin" Value="Datin" />
                <asp:ListItem Text="Datuk" Value="Datuk" />
            </asp:DropDownList>
        </td>
       
        <th>Type:</th>
        <td>
            <asp:DropDownList ID="DropDownList2" runat="server">
                <asp:ListItem Text="Blank" Value="Blank" />
                <asp:ListItem Text="Corporate" Value="Corporate" />
                <asp:ListItem Text="Personal" Value="Personal" />
                <asp:ListItem Text="BG" Value="BG" />
                <asp:ListItem Text="Sole Proprietor" Value="Sole Proprietor" />
                <asp:ListItem Text="Partner" Value="Partner" />
                <asp:ListItem Text="3rd Party" Value="3rd Party" />
                <asp:ListItem Text="Director" Value="Director" />
                <asp:ListItem Text="Undertaking Letter" Value="Undertaking Letter" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <th>Name:</th>
        <td>
            <asp:TextBox ID="TextBox1" runat="server" />
        </td>
        <th>Birth Date:</th>
        <td>
            <asp:TextBox ID="TextBox2" runat="server" TextMode="Date" />
        </td>
    </tr>
    <tr>
        <th>IC No:</th>
        <td>
            <asp:TextBox ID="TextBox3" runat="server" />
        </td>
        <th>Mobile Phone:</th>
        <td>
            <asp:TextBox ID="TextBox4" runat="server" Minlength="10" MaxLength="12" />
        </td>
    </tr>
    <tr>
        <th>Old IC:</th>
        <td>
            <asp:TextBox ID="TextBox5" runat="server" />
        </td>
        <th>Phone:</th>
        <td>
            <asp:TextBox ID="TextBox6" runat="server" Minlength="10" MaxLength="12"/>
        </td>
    </tr>
    <tr>
         <th>Address</th>
        <td >
            <asp:TextBox ID="TextBox7" runat="server" TextMode="MultiLine" Rows="1" />
        </td>
    </tr>
</table>

            <table >
   <h5 class="white-background">Guarantor - 3</h5>
    <tr>
        <th>Title:</th>
        <td>
            <asp:DropDownList ID="DropDownList3" runat="server">
                <asp:ListItem Text="-- SELECT --" Value="Null" />
                <asp:ListItem Text="Encik" Value="Encik" />
                <asp:ListItem Text="Cik" Value="Cik" />
                <asp:ListItem Text="Datin" Value="Datin" />
                <asp:ListItem Text="Datuk" Value="Datuk" />
            </asp:DropDownList>
        </td>
       
        <th>Type:</th>
        <td>
            <asp:DropDownList ID="DropDownList4" runat="server">
                <asp:ListItem Text="Blank" Value="Blank" />
                <asp:ListItem Text="Corporate" Value="Corporate" />
                <asp:ListItem Text="Personal" Value="Personal" />
                <asp:ListItem Text="BG" Value="BG" />
                <asp:ListItem Text="Sole Proprietor" Value="Sole Proprietor" />
                <asp:ListItem Text="Partner" Value="Partner" />
                <asp:ListItem Text="3rd Party" Value="3rd Party" />
                <asp:ListItem Text="Director" Value="Director" />
                <asp:ListItem Text="Undertaking Letter" Value="Undertaking Letter" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <th>Name:</th>
        <td>
            <asp:TextBox ID="TextBox8" runat="server" />
        </td>
        <th>Birth Date:</th>
        <td>
            <asp:TextBox ID="TextBox9" runat="server" TextMode="Date" />
        </td>
    </tr>
    <tr>
        <th>IC No:</th>
        <td>
            <asp:TextBox ID="TextBox10" runat="server" />
        </td>
        <th>Mobile Phone:</th>
        <td>
            <asp:TextBox ID="TextBox11" runat="server" Minlength="10" MaxLength="12" />
        </td>
    </tr>
    <tr>
        <th>Old IC:</th>
        <td>
            <asp:TextBox ID="TextBox12" runat="server" />
        </td>
        <th>Phone:</th>
        <td>
            <asp:TextBox ID="TextBox13" runat="server" Minlength="10" MaxLength="12"/>
        </td>
    </tr>
    <tr>
         <th>Address</th>
        <td >
            <asp:TextBox ID="TextBox14" runat="server" TextMode="MultiLine" Rows="1" />
        </td>
    </tr>
</table>

   <table>
            <h2>Customer Information - Contact Details</h2>
           <h5 class="white-background" >Contact Details - 1</h5>
            <tr>
                <td>
                    <asp:Label ID="Label6" runat="server" Text="Name:" />
                </td>
                <td>
                    <asp:TextBox ID="CustomerNametxt" runat="server" />
                </td>
                <td>
                    <asp:Label ID="Label7" runat="server" Text="Mobile Phone:" />
                </td>
                <td>
                    <asp:TextBox ID="MobilePhonetxt" runat="server" Minlength="10" MaxLength="12"/>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblICIDNo" runat="server" Text="IC/ID No:" />
                </td>
                <td>
                    <asp:TextBox ID="txtICIDNo" runat="server" />
                </td>
                <td>
                    <asp:Label ID="Label8" runat="server" Text="Phone:" />
                </td>
                <td>
                    <asp:TextBox ID="Phonetxt" runat="server" Minlength="10" MaxLength="12" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblEmail" runat="server" Text="Email:" />
                </td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server" />
                </td>
                <td>
                    <asp:Label ID="Label9" runat="server" Text="Address:" />
                </td>
                <td>
                    <asp:TextBox ID="Addresstxt" runat="server" TextMode="MultiLine" Rows="2" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label10" runat="server" Text="Postal Code:" />
                </td>
                <td>
                    <asp:TextBox ID="Postaltxt" runat="server" />
                </td>
            </tr>
  </table>

                   <table>
           <h5 class="white-background" >Contact Details - 2</h5>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Name:" />
                </td>
                <td>
                    <asp:TextBox ID="CustomerNametxt1" runat="server" />
                </td>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Mobile Phone:" />
                </td>
                <td>
                    <asp:TextBox ID="MobilePhonetxt1" runat="server" Minlength="10" MaxLength="12"/>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label3" runat="server" Text="IC/ID No:" />
                </td>
                <td>
                    <asp:TextBox ID="txtICIDNo1" runat="server" />
                </td>
                <td>
                    <asp:Label ID="Label4" runat="server" Text="Phone:" />
                </td>
                <td>
                    <asp:TextBox ID="Phonetxt1" runat="server" Minlength="10" MaxLength="12" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label5" runat="server" Text="Email:" />
                </td>
                <td>
                    <asp:TextBox ID="txtEmail1" runat="server" />
                </td>
                <td>
                    <asp:Label ID="Label11" runat="server" Text="Address:" />
                </td>
                <td>
                    <asp:TextBox ID="Addresstxt1" runat="server" TextMode="MultiLine" Rows="2" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label12" runat="server" Text="Postal Code:" />
                </td>
                <td>
                    <asp:TextBox ID="Postaltxt1" runat="server" />
                </td>
            </tr>
  </table>

                   <table>
           <h5 class="white-background" >Contact Details - 3</h5>
            <tr>
                <td>
                    <asp:Label ID="Label13" runat="server" Text="Name:" />
                </td>
                <td>
                    <asp:TextBox ID="CustomerNametxt2" runat="server" />
                </td>
                <td>
                    <asp:Label ID="Label14" runat="server" Text="Mobile Phone:" />
                </td>
                <td>
                    <asp:TextBox ID="MobilePhonetxt2" runat="server" Minlength="10" MaxLength="12"/>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label15" runat="server" Text="IC/ID No:" />
                </td>
                <td>
                    <asp:TextBox ID="txtICIDNo2" runat="server" />
                </td>
                <td>
                    <asp:Label ID="Label16" runat="server" Text="Phone:" />
                </td>
                <td>
                    <asp:TextBox ID="Phonetxt2" runat="server" Minlength="10" MaxLength="12" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label17" runat="server" Text="Email:" />
                </td>
                <td>
                    <asp:TextBox ID="txtEmail2" runat="server" />
                </td>
                <td>
                    <asp:Label ID="Label18" runat="server" Text="Address:" />
                </td>
                <td>
                    <asp:TextBox ID="Addresstxt2" runat="server" TextMode="MultiLine" Rows="2" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label19" runat="server" Text="Postal Code:" />
                </td>
                <td>
                    <asp:TextBox ID="Postaltxt2" runat="server" />
                </td>
            </tr>
  </table>


       <table>
              <h2>Salesman Declaration</h2>
    
                <tr><h5>(A) MANAGEMENT'S EXPERIENCE</h5></tr>
                <tr>
                        <th><span>Name of Customer:</span></th>
                        <td><asp:TextBox ID="TextBoxCustName" runat="server" ReadOnly="true"></asp:TextBox></td>
                </tr>
              <tr>
                    <th><label>Do you know the customer personally?</label></th>
                    <td>
                        <asp:TextBox ID="personally" runat="server" ReadOnly="true"></asp:TextBox>                    
                    </td>
                </tr>

                <tr>
                        <th><label for="ddlYearsKnown">How many years have you known the customer?</label></th>
                    <td>
                        <asp:TextBox ID="CustomerYearsKnown" runat="server" ReadOnly="true"></asp:TextBox>                    
                    </td>
                </tr>
     
           <tr>

          <tr>
                <th><label for="txtPersonName">Mr:</label></th>
                <th><asp:TextBox ID="txtPersonName" runat="server" ReadOnly="true"></asp:TextBox></th>
            
       
                <th><label for="txt">is My</label></th>
                <th><asp:TextBox ID="Relationship" runat="server" ReadOnly="true"></asp:TextBox></th>
               
            </tr>

                <tr>
                        <th><span>I do not know the customer personally, but he was introduced to me by Mr:</span></th>
                        <td><asp:TextBox ID="txtIntroducedBy" runat="server"  ReadOnly="true"></asp:TextBox></td>
                        <th><span>of</span></th>
                        <td><asp:TextBox ID="txtCompanyName" runat="server" ReadOnly="true" ></asp:TextBox></td>
                    <th><span>(company name)</span></th>
                </tr>

                 <tr>
                    <th class="form-group">
                        <label>The customer was working as:</label>
                    </th>
                        <td><asp:TextBox ID="txtWorkTitle" runat="server" ReadOnly="true" ></asp:TextBox></td>
                        <th><span>for</span></th>
                        <td><asp:TextBox ID="txtWorkYears" runat="server" ReadOnly="true" ></asp:TextBox></td>
                        <th><span>years in</span></th>
                        <td><asp:TextBox ID="txtWorkCompany" runat="server"  ReadOnly="true"></asp:TextBox></td>
                        <th><span>(company name)</span></th>
                </tr>
                <tr>
                    <th class="form-group">
                        <label>Reason for leaving last company customer was working in:</label>
                    </th>
                    <td class="form-group">
                        <asp:TextBox ID="txtReasonForLeaving" runat="server" ReadOnly="true" ></asp:TextBox>
                    </td>
                </tr>

                   <tr>
                    <th class="form-group">
                        <label>Has the customer worked in another trade or occupation previously?</label>
                    </th>
                    <td class="form-group radio-group">
                        <asp:TextBox ID="WorkedInOtherTrade" runat="server"  ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>

                  <tr>
                  <th><h5>(B) MANAGEMENT CHARACTER</h5></th>
              </tr>

                <tr>

                    <th class="form-group">
                        <label>How many days a week is the shop open:</label>
                    </th>
                    <td class="form-group">
                        <asp:TextBox ID="txtDaysOpenPerWeek" runat="server" ReadOnly="true" ></asp:TextBox>
                    </td>
                </tr>
              <tr>
                    <th class="form-group">
                        <label>What are the usual working hours:</label>
                    </th>
                    <td class="form-group">
                        <asp:TextBox ID="txtStartHour" runat="server"   Width="50px" ReadOnly="true"></asp:TextBox>
                        <asp:Label runat="server" Text="a.m to" CssClass="form-label"></asp:Label>
                        <asp:TextBox ID="txtEndHour" runat="server"   Width="50px" ReadOnly="true"></asp:TextBox>
                        <asp:Label runat="server" Text="p.m" CssClass="form-label"></asp:Label>
                    </td>
                </tr>

                <tr>
                    <th class="form-group">
                        <label>Does the customer play heavily in the stock market?</label>
                    </th>
                    <td class="form-group ">
                        <asp:TextBox ID="stockmarket" runat="server"  ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th class="form-group">
                        <label>Does the customer involve in borrowing/gambling (any type)?</label>
                    </th>
                    <td class="form-group ">
                        <asp:TextBox ID="borrowing" runat="server" ReadOnly="true" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th class="form-group">
                        <label>Do the partners get along well?</label>
                    </th>
                    <td class="form-group ">
                         <asp:TextBox ID="alongwell" runat="server" ReadOnly="true" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th class="form-group">
                        <label>Has the customer had marital problems due to outside interests?</label>
                    </th>
                    <td class="form-group ">
                         <asp:TextBox ID="maritalproblems" runat="server" ReadOnly="true" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th class="form-group">
                        <label>Does the customer keep his promises? (Is the customer reliable)?</label>
                    </th>
                    <td class="form-group">
                        <asp:TextBox ID="promises" runat="server" ReadOnly="true" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th class="form-group">
                        <label>Has any of his cheques been dishonored?</label>
                    </th>
                    <td class="form-group">
                        <asp:TextBox ID="cheques" runat="server" ReadOnly="true" ></asp:TextBox>
                    </td>
                </tr>

         <tr>
                <th class="form-group">
                    <label>How do you find the customer?</label>
                </th>
                <td class="form-group" colspan="3">
                    <table>
                        <tr>
                       <td class="form-group">
                        <asp:TextBox ID="findCust" runat="server" TextMode="MultiLine" Rows="3" ReadOnly="true" ></asp:TextBox>
                    </td>
                        </tr>
                    </table>
                </td>
            </tr>



                  <tr>
                    <th class="form-group">
                        <label>Currently, as far as you know, what are the customer's hobbies or interests other than work?</label>
                    </th>
                    <td class="form-group">
                        <asp:TextBox ID="txtHobbiesInterests" runat="server" ReadOnly="true" ></asp:TextBox>
                    </td>
                </tr>

                <th class="form-group">
                        <label>Is the shop located along the main road? Busy junction?</label>
                    </th>
                    <td class="form-group radio-group">
                        <asp:TextBox ID="MainRoad" runat="server" ReadOnly="true" ></asp:TextBox>                     
                    </td>
                </tr>
                <tr>
                    <th class="form-group">
                        <label>Is the shop location difficult to find?</label>
                    </th>
                    <td class="form-group radio-group">
                        <asp:TextBox ID="DifficultToFind" runat="server" ReadOnly="true" ></asp:TextBox>
                    </td>
                </tr>
              
                <tr>
                    <th class="form-group">
                        <label>Is the shop busy with customers?</label>
                    </th>
                    <td class="form-group">
                        <div class="checkbox-group">
                            <div class="checkbox-item">
                        </div>
                        <asp:TextBox ID="shopbusy" runat="server" ReadOnly="true" ></asp:TextBox>
                            </div>
                        </div>
                    </td>
                </tr>

                <tr>
                    <th class="form-group">
                        <label>From your observation, are the goods in the shop moving fast?</label>
                    </th>
                    <td class="form-group radio-group">
                        <asp:TextBox ID="GoodsMovingFast" runat="server"  ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>

                  <th><h5>(C) FINACIAL STABLILITY</h5></th>

                   <tr>
                    <th class="form-group">
                        <label>Does the workshop/shop belong to the customer?</label>
                    </th>
                    <td class="form-group radio-group">
                        <asp:TextBox ID="BelongsToCustomer" runat="server" ReadOnly="true" ></asp:TextBox>
                    </td>
                </tr>

    <tr>
        <th class="form-group">
            <label>If rented/purchased, what is the rental/installment per month?</label>
        </th>
        <td>
            <table>
                <tr>
                    <td>
                        <span>RM</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtRentalInstallment" runat="server"   style="margin-left: 5px;" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td>
                        <span>per month</span>
                    </td>
                </tr>
            </table>
        </td>
    </tr>




                <tr>
                    <th class="form-group">
                        <label>Does the customer own a house/apartment & cars? (Please state No, type & location)</label>
                    </th>
                    <td class="form-group">
                        <span>No:</span>
                        <asp:TextBox ID="txtOwnsHouseNo" runat="server" ReadOnly="true" ></asp:TextBox>
                        <br /><br />
                        <span>Type:</span>
                        <asp:TextBox ID="txtOwnsHouseType" runat="server"  ReadOnly="true"></asp:TextBox>
                        <br /><br />
                        <span>Location:</span>
                        <asp:TextBox ID="txtOwnsHouseLocation" runat="server" ReadOnly="true" ></asp:TextBox>
                    </td>
                </tr>


                   <tr>
                    <th class="form-group">
                        <label>If the customer has financial assistance from someone else, please advise assisted by:</label>
                    </th>
                    <td class="form-group">
                        <asp:TextBox ID="txtFinacialBy" runat="server" ReadOnly="true" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th class="form-group">
                        <label>The customer's other assets are:</label>
                    </th>
                    <td class="form-group">
                        <asp:TextBox ID="txtOtherAssets" runat="server"  ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>

               <tr>
                  <th> <h5>OTHER COMMMENTS</h5></th>
              </tr>

        <tr>
            <th class="form-group">
                <label>I personally consider this customer to be:</label>
            </th>
            <td class="form-group">
                <div class="radio-group">
                        <asp:TextBox ID="CustConsider" runat="server" ReadOnly="true" ></asp:TextBox>
                </div>
            </td>
        </tr>
    </table>

    <table>
         <h2>Document's Check List</h2>

          <tr>
                <th colspan="2" style="vertical-align: middle; text-align: center;"><h3>By Salesman</h3></th>

                <td>
                
                </td>
                <th>
                    <label>Received By Credit Admin</label>
                </th>
                <th>
                     <label>Request New Document</label>
                </th>
            </tr>

        <tr>
                <th colspan="2">1. Credit Application Form x 2 sets (complete filled up)</th>
                <td>
                    <asp:Image runat="server" ImageUrl="RESOURCES/ok.png" AlternateText="OK Image" ></asp:Image>
                </td>
                <td class="center-checkbox">
                    <asp:CheckBox runat="server" ID="creditApplicationFormReceivedByCreditAdmin"  Value="Received By Credit Admin"/>
                </td>
               <td class="center-checkbox">
                    <asp:CheckBox runat="server" ID="creditApplicationFormRequestNewDocument" Value="New Document Request" />
                </td>
            </tr>

            <tr>
                <th colspan="2">2. Personal Guarantee x 2 sets (no. of guarantors signed)</th>
                <td>
                    <asp:Image runat="server" ImageUrl="RESOURCES/ok.png" AlternateText="OK Image" />
                </td>
               <td class="center-checkbox">
                    <asp:CheckBox runat="server" ID="personalGuaranteeReceivedByCreditAdmin" Value="Received By Credit Admin"/>
                </td>
               <td class="center-checkbox">
                    <asp:CheckBox runat="server" ID="personalGuaranteeRequestNewDocument" Value="New Document Request" />
                </td>
            </tr>

                    <tr>
                <th colspan="2">3. Salesman Recommendation Form</th>
                <td>
                    <asp:Image runat="server" ImageUrl="RESOURCES/ok.png" AlternateText="OK Image" />
                </td>
               <td class="center-checkbox">
                    <asp:CheckBox runat="server" ID="SalesmanRecommmendationFormByCreditAdmin"  Value="Received By Credit Admin"/>
                </td>
               <td class="center-checkbox">
                    <asp:CheckBox runat="server" ID="SalesmanRecommmendationFormByNewDocument" Value="New Document Request" />
                </td>
            </tr>

                    <tr>
                <th colspan="2">4. Photocopy of IC-front&back complete with signature(owner/partner/directors & guarantors) - compulsory forEOL</th>
                <td>
                    <asp:Image runat="server" ImageUrl="RESOURCES/ok.png" AlternateText="OK Image" Value="Received By Credit Admin"/>
                </td>
               <td class="center-checkbox">
                    <asp:CheckBox runat="server" ID="PhotocopyByCreditAdmin" Value="Received By Credit Admin" />
                </td>
               <td class="center-checkbox">
                    <asp:CheckBox runat="server" ID="PhotocopyByNewDocument" Value="New Document Request" />
                </td>
            </tr>

            <tr>
                <th colspan="2">5. Declaration Form on IC particulars validation</th>
                <td>
                    <asp:Image runat="server" ImageUrl="RESOURCES/ok.png" AlternateText="OK Image" />
                </td>
               <td class="center-checkbox">
                    <asp:CheckBox runat="server" ID="declarationFormReceivedByCreditAdmin"  Value="Received By Credit Admin"/>
                </td>
               <td class="center-checkbox">
                    <asp:CheckBox runat="server" ID="declarationFormRequestNewDocument" Value="New Document Request" />
                </td>
            </tr>
     
             <tr>
                <th colspan="2">6. Photos of the workshop/factory with Company name signboard</th>
             </tr>
               
                    <tr>
                <th colspan="2">* Full view of the whole workshop building (Front & nearer) e.g., Double/Single/Linked/Attached</th>
                <td>
                    <asp:Image runat="server" ImageUrl="RESOURCES/ok.png" AlternateText="OK Image" />
                </td>
               <td class="center-checkbox">
                    <asp:CheckBox runat="server" ID="workshopPhotos1ReceivedByCreditAdmin" Value="Received By Credit Admin" />
                </td>
               <td class="center-checkbox">
                    <asp:CheckBox runat="server" ID="workshopPhotos1RequestNewDocument" Value="New Document Request" />
                </td>
            </tr>

                    <tr>
                <th colspan="2">* Full view - left side of the workshop(whole street) *Note: Please take outside of the shop</th>
                <td>
                    <asp:Image runat="server" ImageUrl="RESOURCES/ok.png" AlternateText="OK Image" />
                </td>
               <td class="center-checkbox">
                    <asp:CheckBox runat="server" ID="workshopPhotos2ReceivedByCreditAdmin" Value="Received By Credit Admin" />
                </td>
               <td class="center-checkbox">
                    <asp:CheckBox runat="server" ID="workshopPhotos2RequestNewDocument"  Value="New Document Request"/>
                </td>
            </tr>

                    <tr>
                <th colspan="2">* Full view - right  side of the workshop(whole street) *Note: Please take outside of the shop</th>
                <td>
                    <asp:Image runat="server" ImageUrl="RESOURCES/ok.png" AlternateText="OK Image" />
                </td>
               <td class="center-checkbox">
                    <asp:CheckBox runat="server" ID="workshopPhotos3ReceivedByCreditAdmin"  Value="Received By Credit Admin"/>
                </td>
               <td class="center-checkbox">
                    <asp:CheckBox runat="server" ID="workshopPhotos3RequestNewDocument" Value="New Document Request" />
                </td>
            </tr>

                    <tr>
                <th colspan="2">* Inside of the workshop (shown the products/equipments, etc)
</th>
                <td>
                    <asp:Image runat="server" ImageUrl="RESOURCES/ok.png" AlternateText="OK Image" />
                </td>
               <td class="center-checkbox">
                    <asp:CheckBox runat="server" ID="workshopPhotos4ReceivedByCreditAdmin"  Value="Received By Credit Admin"/>
                </td>
               <td class="center-checkbox">
                    <asp:CheckBox runat="server" ID="workshopPhotos4RequestNewDocument" Value="New Document Request" />
                </td>
            </tr>

                    <tr>
                <th colspan="2">* Workshop/factory with Company name signboard</th>
                <td>
                    <asp:Image runat="server" ImageUrl="RESOURCES/ok.png" AlternateText="OK Image" />
                </td>
               <td class="center-checkbox">
                    <asp:CheckBox runat="server" ID="workshopPhotos5ReceivedByCreditAdmin" Value="Received By Credit Admin" />
                </td>
               <td class="center-checkbox">
                    <asp:CheckBox runat="server" ID="workshopPhotos5RequestNewDocument" Value="New Document Request" />
                </td>
            </tr>

              <tr>
                <th colspan="2"> 7. Hand Phone Numbers (Owner/All partners/All Directors/Person in charge of the business)</th>
                <td>
                    <asp:Image runat="server" ImageUrl="RESOURCES/ok.png" AlternateText="OK Image" />
                </td>
               <td class="center-checkbox">
                    <asp:CheckBox runat="server" ID="PhoneReceivedByCreditAdmin" Value="Received By Credit Admin" />
                </td>
               <td class="center-checkbox">
                    <asp:CheckBox runat="server" ID="PhoneRequestNewDocument" Value="New Document Request" />
                </td>
            </tr>

               <tr>
                <th colspan="2"> 8. Email address (Owner/All partners/All Directors/Person in charge of the business)</th>
                <td>
                    <asp:Image runat="server" ImageUrl="RESOURCES/ok.png" AlternateText="OK Image" />
                </td>
               <td class="center-checkbox">
                    <asp:CheckBox runat="server" ID="EmailReceivedByCreditAdmin" Value="Received By Credit Admin" />
                </td>
               <td class="center-checkbox">
                    <asp:CheckBox runat="server" ID="EmailRequestNewDocument"  Value="New Document Request"/>
                </td>
            </tr>

              <tr>
                <th colspan="2"> 9. Business Registration Certificate (Form A or Form B)</th>
                <td>
                    <asp:Image runat="server" ImageUrl="RESOURCES/not_ok.png" AlternateText="NOT OK Image" />
                </td>
               <td class="center-checkbox">
                    <asp:CheckBox runat="server" ID="registrationCertificateReceivedByCreditAdmin"  Value="Received By Credit Admin"/>
                </td>
               <td class="center-checkbox">
                    <asp:CheckBox runat="server" ID="registrationCertificateRequestNewDocument"  Value="New Document Request"/>
                </td>
            </tr>

                      <tr>
                <th colspan="2">10. Form D - Business Annual License</th>
                <td>
                    <asp:Image runat="server" ImageUrl="RESOURCES/not_ok.png" AlternateText="NOT OK Image" />
                </td>
               <td class="center-checkbox">
                    <asp:CheckBox runat="server" ID="businessLicenseReceivedByCreditAdmin" Value="Received By Credit Admin" />
                </td>
               <td class="center-checkbox">
                    <asp:CheckBox runat="server" ID="businessLicenseRequestNewDocument"  Value="New Document Request"/>
                </td>
            </tr>

                      <tr>
                <th colspan="2"> 11. Latest 2 years audited Financial Report</th>
                <td>
                    <asp:Image runat="server" ImageUrl="RESOURCES/not_ok.png" AlternateText="NOT OK Image" />
                </td>
               <td class="center-checkbox">
                    <asp:CheckBox runat="server" ID="financialReportReceivedByCreditAdmin" Value="Received By Credit Admin" />
                </td>
               <td class="center-checkbox">
                    <asp:CheckBox runat="server" ID="financialReportRequestNewDocument"  Value="New Document Request"/>
                </td>
            </tr>

                      <tr>
                <th colspan="2"> 12. Latest 3 months bank statements</th>
                <td>
                    <asp:Image runat="server" ImageUrl="RESOURCES/not_ok.png" AlternateText="NOT OK Image" />
                </td>
               <td class="center-checkbox">
                    <asp:CheckBox runat="server" ID="bankStatementsReceivedByCreditAdmin" Value="Received By Credit Admin" />
                </td>
               <td class="center-checkbox">
                    <asp:CheckBox runat="server" ID="bankStatementsRequestNewDocument"  Value="New Document Request"/>
                </td>
            </tr>
        </table>
            
         

<asp:Table ID="tblHODRecommendation" runat="server" >
<asp:TableHeaderRow>
    <asp:TableHeaderCell ColumnSpan="6">
        <h2>HOD Recommendation</h2>
    </asp:TableHeaderCell>
</asp:TableHeaderRow>



    <asp:TableRow>
        <asp:TableHeaderCell>
            <asp:Label ID="lblHOD1" runat="server" Text="Recommended by Chee Yun Keen" />
        </asp:TableHeaderCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="lblCreditTermHOD1" runat="server" Text="Credit Term:" />
        </asp:TableCell>
        <asp:TableCell>
            <asp:TextBox ID="txtCreditTermHOD1" runat="server" ReadOnly="true"/>
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="lblCreditLimitHOD1" runat="server" Text="Credit Limit:" />
        </asp:TableCell>
        <asp:TableCell>
            <asp:TextBox ID="txtCreditLimitHOD1" runat="server" ReadOnly="true"/>
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="lblRemarksHOD1" runat="server" Text="Remarks:" />
        </asp:TableCell>
        <asp:TableCell>
            <asp:TextBox ID="txtRemarksHOD1" runat="server" />
        </asp:TableCell>
    </asp:TableRow>

    <asp:TableRow>
        <asp:TableHeaderCell>
            <asp:Label ID="lblHOD2" runat="server" Text="Recommended by Kenny Chuah Yew Siang" />
        </asp:TableHeaderCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="lblCreditTermHOD2" runat="server" Text="Credit Term:" />
        </asp:TableCell>
        <asp:TableCell>
            <asp:TextBox ID="txtCreditTermHOD2" runat="server" ReadOnly="true"/>
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="lblCreditLimitHOD2" runat="server" Text="Credit Limit:" />
        </asp:TableCell>
        <asp:TableCell>
            <asp:TextBox ID="txtCreditLimitHOD2" runat="server" ReadOnly="true"/>
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="lblRemarksHOD2" runat="server" Text="Remarks:" />
        </asp:TableCell>
        <asp:TableCell>
            <asp:TextBox ID="txtRemarksHOD2" runat="server" />
        </asp:TableCell>
    </asp:TableRow>
</asp:Table>

      <asp:Table ID="tblCreditControlDocumentation" runat="server">
    <asp:TableHeaderRow>
        <asp:TableHeaderCell ColumnSpan="4">
            <h2>Credit Control Department - Documentation Status</h2>
        </asp:TableHeaderCell>
    </asp:TableHeaderRow>

    <asp:TableRow>
        <asp:TableHeaderCell ColumnSpan="2">CTOS Status</asp:TableHeaderCell>
        <asp:TableCell ColumnSpan="2">
            <asp:TextBox runat="server" ID="CTOStxt"></asp:TextBox>
        </asp:TableCell>
    </asp:TableRow>

    <asp:TableRow>
        <asp:TableHeaderCell ColumnSpan="2">Document Remarks</asp:TableHeaderCell>
        <asp:TableCell ColumnSpan="2">
            <asp:TextBox runat="server" ID="documentRemarksTextBox"></asp:TextBox>
        </asp:TableCell>
        <asp:TableCell ColumnSpan="2"></asp:TableCell>
        <asp:TableCell ColumnSpan="2">
            <asp:CheckBox runat="server" ID="ctosCheckbox" Text="CTOS" />
        </asp:TableCell>
    </asp:TableRow>

    <asp:TableRow>
        <asp:TableCell ColumnSpan="4"></asp:TableCell>
        <asp:TableCell ColumnSpan="4">
            <asp:CheckBox runat="server" ID="cbmCheckbox" Text="CBM" />
        </asp:TableCell>
    </asp:TableRow>

    <asp:TableRow>
        <asp:TableCell ColumnSpan="4"></asp:TableCell>
        <asp:TableCell ColumnSpan="4">
            <asp:CheckBox runat="server" ID="photoCheckbox" Text="PHOTO" />
        </asp:TableCell>
    </asp:TableRow>

    <asp:TableRow>
        <asp:TableCell ColumnSpan="4"></asp:TableCell>
        <asp:TableCell ColumnSpan="4">
            <asp:CheckBox runat="server" ID="icCheckbox" Text="IC" />
        </asp:TableCell>
    </asp:TableRow>

    <asp:TableRow>
        <asp:TableHeaderCell ColumnSpan="2">Customer Type</asp:TableHeaderCell>
        <asp:TableCell ColumnSpan="2">
            <asp:TextBox runat="server" ID="customerTypeTextBox"></asp:TextBox>
        </asp:TableCell>
    </asp:TableRow>

    <asp:TableRow>
        <asp:TableHeaderCell ColumnSpan="2">Paid up Capital</asp:TableHeaderCell>
        <asp:TableCell ColumnSpan="2">
            <asp:TextBox runat="server" ID="paidUpCapitalTextBox"></asp:TextBox>
        </asp:TableCell>
    </asp:TableRow>
</asp:Table>


              <asp:Table  ID="tblCreditControlAccStatus" runat="server">
    <asp:TableHeaderRow>
        <asp:TableHeaderCell ColumnSpan="4">
            <h2>Credit Control Department - Customer Account Status</h2>
        </asp:TableHeaderCell>
    </asp:TableHeaderRow>

    <asp:TableRow>
        <asp:TableHeaderCell ColumnSpan="2">Status</asp:TableHeaderCell>
        <asp:TableCell ColumnSpan="2">
            <asp:DropDownList runat="server" ID="statusDropDownList">
                <asp:ListItem Text="Draft" Value="Draft" />
                <asp:ListItem Text="New" Value="New" />
                <asp:ListItem Text="Approved" Value="Approved" />
            </asp:DropDownList>
        </asp:TableCell>

        <asp:TableHeaderCell ColumnSpan="2">Opening Acc. Date</asp:TableHeaderCell>
        <asp:TableCell ColumnSpan="2">
            <asp:TextBox runat="server" ID="openingDateTextBox"></asp:TextBox>
        </asp:TableCell>
    </asp:TableRow>

    <asp:TableRow>
        <asp:TableHeaderCell ColumnSpan="2">Customer Type</asp:TableHeaderCell>
        <asp:TableCell ColumnSpan="2">
            <asp:DropDownList runat="server" ID="customerTypeDropDown">
                <asp:ListItem Text="New" Value="New" />
                <asp:ListItem Text="Existing" Value="Existing" />
            </asp:DropDownList>
        </asp:TableCell>

        <asp:TableHeaderCell ColumnSpan="2">Customer Class</asp:TableHeaderCell>
        <asp:TableCell ColumnSpan="2">
            <asp:DropDownList runat="server" ID="customerClassDropDown"></asp:DropDownList>
        </asp:TableCell>
    </asp:TableRow>

    <asp:TableRow>
        <asp:TableHeaderCell ColumnSpan="2">Warehouse</asp:TableHeaderCell>
        <asp:TableCell ColumnSpan="2">
            <asp:DropDownList runat="server" ID="warehouseDropDown"></asp:DropDownList>
        </asp:TableCell>

        <asp:TableHeaderCell ColumnSpan="2">Customer Main Group</asp:TableHeaderCell>
        <asp:TableCell ColumnSpan="2">
            <asp:DropDownList runat="server" ID="customerMainGroupDropDown"></asp:DropDownList>
        </asp:TableCell>
    </asp:TableRow>

    <asp:TableRow>
        <asp:TableHeaderCell ColumnSpan="2">Credit Limit (RM)</asp:TableHeaderCell>
        <asp:TableCell ColumnSpan="2">
            <asp:TextBox runat="server" ID="creditLimitTextBox"></asp:TextBox>
        </asp:TableCell>

        <asp:TableHeaderCell ColumnSpan="2">Credit Term</asp:TableHeaderCell>
        <asp:TableCell ColumnSpan="2">
            <asp:DropDownList runat="server" ID="creditTermDropDown"></asp:DropDownList>
        </asp:TableCell>
    </asp:TableRow>

    <asp:TableRow>
        <asp:TableHeaderCell ColumnSpan="2">Customer Account No</asp:TableHeaderCell>
        <asp:TableCell ColumnSpan="2">
            <asp:TextBox runat="server" ID="customerAccountNoTextBox"></asp:TextBox>
        </asp:TableCell>

        <asp:TableHeaderCell ColumnSpan="2">Incorporation Date</asp:TableHeaderCell>
        <asp:TableCell ColumnSpan="2">
            <asp:TextBox runat="server" ID="incorporationDateTextBox" TextMode="Date"></asp:TextBox>
        </asp:TableCell>
    </asp:TableRow>

    <asp:TableRow>
        <asp:TableHeaderCell ColumnSpan="2">ROC NEW</asp:TableHeaderCell>
        <asp:TableCell ColumnSpan="2">
            <asp:TextBox runat="server" ID="rocNewTextBox"></asp:TextBox>
        </asp:TableCell>
    </asp:TableRow>

    <asp:TableRow>
        <asp:TableHeaderCell ColumnSpan="2">ROC/ROB</asp:TableHeaderCell>
        <asp:TableCell ColumnSpan="2">
            <asp:TextBox runat="server" ID="rocRobTextBox"></asp:TextBox>
        </asp:TableCell>

        <asp:TableHeaderCell ColumnSpan="2">Territory</asp:TableHeaderCell>
        <asp:TableCell ColumnSpan="2">
            <asp:DropDownList runat="server" ID="DropDownListTerritory"></asp:DropDownList>
        </asp:TableCell>
    </asp:TableRow>

    <asp:TableRow>
        <asp:TableHeaderCell ColumnSpan="2">Customer Name</asp:TableHeaderCell>
        <asp:TableCell ColumnSpan="2">
            <asp:TextBox runat="server" ID="customerNameTextBox"></asp:TextBox>
        </asp:TableCell>
    </asp:TableRow>

    <asp:TableRow>
        <asp:TableHeaderCell ColumnSpan="2">Cust Acc Remarks</asp:TableHeaderCell>
        <asp:TableCell ColumnSpan="2">
            <asp:TextBox runat="server" ID="custAccTextBox"></asp:TextBox>
        </asp:TableCell>

        <asp:TableHeaderCell ColumnSpan="2">Customer Type</asp:TableHeaderCell>
        <asp:TableCell ColumnSpan="2">
            <asp:DropDownList runat="server" ID="customerTypeDropDownList"></asp:DropDownList>
        </asp:TableCell>
    </asp:TableRow>
</asp:Table>


<asp:Button runat="server" ID="submitButton" Text="Submit" OnClick="btnSubmit"/>
<asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
<asp:Panel runat="server" CssClass="form-group">
    <asp:Button runat="server" ID="closeButton" Text="Close" OnClientClick="goBack(); return false;" />
</asp:Panel>

   </form>



</body>
</html>

