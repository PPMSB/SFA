<%@ page language="C#" autoeventwireup="true" codebehind="AddApplication.aspx.cs" inherits="DotNet.AddApplication" %>

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
    <link href="STYLES/AddAccStyle.css" rel="stylesheet" />


    <title>Customer Account Application</title>
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="mobile-web-app-capable" content="yes" />

    <script src="scripts/BrowserReload_ThroughHistory.js"></script>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link rel="stylesheet" href="https://cdn.datatables.net/1.13.6/css/jquery.dataTables.css" />
    <script src="https://cdn.datatables.net/1.13.6/js/jquery.dataTables.js"></script>

<script>
    function goBack() {
        window.location.href = 'NewCustomer.aspx';
    }

    function closeForm() {
        window.close(); // Close the current window
    }

</script>

    <script>
        function handleCheckboxClick(checkbox) {
            var checkboxes = document.querySelectorAll('.checkbox-group input[type="checkbox"]');
            checkboxes.forEach(function (cb) {
                if (cb !== checkbox) {
                    cb.checked = false;
                }
            });
        }
    </script>

<script type="text/javascript">
    // Assuming you are using jQuery
    $(document).ready(function () {
        $('.salesCheckbox').change(function () {
            $('.salesCheckbox').prop('checked', false);
            $(this).prop('checked', true);
        });
    });
</script>


    <div class="header1">
        <header>
            <div class="row">
                <div class="mobile_hidden">
                    <div class="col-3 col-s-3 image_icon">
                        <img src="RESOURCES/POSIM PETROLEUM MARKETING.png" class="image_resize" />
                    </div>

                    <div class="col-9 col-s-9 image_title">
                      <h1>Customer Account Application</h1>
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

        <div class="col-12 col-s-12">
            <div class="function">

                <div class="function">
                </div>
            </div>
        </div>

<form id="CustomerAccountForm"  runat="server" onsubmit="return submitForm()">
    <asp:TextBox ID="HiddenNewCustID" runat="server" style="display:none;"></asp:TextBox>

        <div class="AddAcc">
                <table>
                    <tr>
            <div class="form-group">
                <th>
                    <label for="salesmanName">Salesman Name:</label>
                </th>
                <td>
                    <asp:TextBox ID="salesmanNameTextBox" runat="server" required ReadOnly="true"></asp:TextBox>
                </td>
            </div>

            <div class="form-group">
                <th>
                    <label for="status">Status:</label>
                </th>
                <td>
                    <asp:TextBox ID="txtStatus" runat="server" required ReadOnly="true"></asp:TextBox>
                </td>
            </div>
        </tr>

        <tr>
            <div class="form-group">

                <th>
                    <label for="salesmanCode">Salesman Code:</label>
                </th>

                <td>
                    <asp:TextBox ID="salesmanCodeTextBox" runat="server"  ReadOnly="true"></asp:TextBox>
                </td>
            </div>

            <div class="form-group">
                <th>
                    <label for="formNo">Form No:</label>
                </th>
                <td>
                    <asp:TextBox ID="txtFormNo" runat="server"  ReadOnly="true"></asp:TextBox>
                </td>
            </div>
        </tr>

            <tr>
                <div class="form-group">
                    <th><label for="salesmansMark">Salesman's Mark:</label></th>
                    <td><asp:TextBox ID="txtSalesmansMark" runat="server" TextMode="MultiLine" Rows="4" ></asp:TextBox></td>
                </div>
            </tr>

              </table>     
                <br /> 
        <div class="AddAcc1" id="customerInformationSection" >
                     
            
            <h2>Customer Information</h2>
           <table>       
 <tr>
                <th>
                    <label for="customerName">Customer Name:</label>
                </th>
                <td>
                    <div class="form-group">
                     <asp:TextBox ID="txtCustName" runat="server" required ReadOnly="true"></asp:TextBox>
                    </div>
                </td>

          
     <th colspan="5"><label>Customer Category:</label></th>
            </tr>

            <tr>
                <td colspan="2"></td>
                <td colspan="2">
                    <asp:RadioButtonList ID="CustCategoryDropDownList" runat="server" CssClass="form-group" RepeatDirection="Vertical">
                        <asp:ListItem Text="DistributionLubricant" />
                        <asp:ListItem Text="WorkShop" />
                        <asp:ListItem Text="CounterSales" />
                        <asp:ListItem Text="VanSales" />
                        <asp:ListItem Text="Distributors" />
                    </asp:RadioButtonList>
                </td>
                <td colspan="3"></td>
            </tr>



        <tr>
            <div class="form-group">
                <th><label for="customerAddress">Customer Address:</label></th>
                <td><asp:TextBox ID="customerAddress" runat="server" TextMode="MultiLine" Rows="4" ></asp:TextBox></td>
            </div>
        </tr>
              
        <tr>
            <div class="form-group">
                <th><label for="province">Province:</label></th>
                <td><asp:TextBox ID="province" runat="server" ></asp:TextBox></td>
            </div>
        </tr>
            
<tr>
  <th>Postal Code:</th>
  <td>
    <div class="form-group">
      <asp:TextBox ID="postalCode" runat="server" ></asp:TextBox>
    </div>
  </td>

    <th>City: </th>
    <td><asp:TextBox ID="city" runat="server"  /></asp:TextBox></td>
</tr>
<tr>
  <th>ROC / ROB:</th>
  <td>
    <div class="form-group">
      <asp:TextBox id="rocRob" runat="server" ></asp:TextBox>
    </div>
  </td>

<th>Customer Class:</th>
<td>
    <div class="form-group">
       <asp:DropDownList ID="DropDownListCustomerDescription" runat="server"></asp:DropDownList>
    </div>
</td>
</tr>
     

<tr>
    <div class="form-group">
        <th><label for="coordinate">Coordinate:</label></th>
        <td>
            <asp:TextBox ID="txtCoordinate" runat="server" required ReadOnly="true"></asp:TextBox>
        </td>
    </div>
<th>Credit Term:</th>
<td>
    <div class="form-group">
       <asp:DropDownList ID="DropDownListCreditTerm" runat="server"></asp:DropDownList>


        <asp:Label ID="lblDiscount" runat="server" AssociatedControlID="chkDiscount" Text="Discount"></asp:Label>
        <asp:CheckBox ID="chkDiscount" runat="server" />
    </div>
</td>

</tr>
<tr>
    <th>Territory:</th>
    <td>
        <div class="form-group">
            <asp:TextBox ID="territory" runat="server"  ></asp:TextBox>
        </div>
    </td>

<tr>
    <th>Customer VPPP:</th>
    <td>
        <div class="form-group">
            <asp:RadioButton ID="vpppYes" runat="server" Text="Yes" GroupName="customerVPPP" Value="1" />
            <asp:RadioButton ID="vpppNo" runat="server" Text="No" GroupName="customerVPPP" Value="0" />
        </div>
    </td>
</tr>


<tr>
    <th>Credit Limit:</th>
    <td>
        <div class="form-group">
            <asp:TextBox ID="creditLimit" runat="server"  ></asp:TextBox>
        </div>
    </td>

<th>Customer Type:</th>
<td>
   <asp:RadioButtonList ID="customerTypeRadioButtonList" runat="server" CssClass="form-group" RepeatDirection="Vertical">
    <asp:ListItem Text="Private Limited Company (Sdn Bhd)" />
    <asp:ListItem Text="Public Limited Company (BerHad)" />
    <asp:ListItem Text="Sole Proprietorship" />
    <asp:ListItem Text="Partnership" />
</asp:RadioButtonList>

</td>
</tr>
    </table>

   <h2>Salesman Declaration</h2>
            <table class="form-table">
                <tr><h5>(A) MANAGEMENT'S EXPERIENCE</h5></tr>
                <tr>
                        <th><span>Name of Customer:</span></th>
                        <td><asp:TextBox ID="txtCustomerName" runat="server" ></asp:TextBox></td>
                </tr>
              <tr>
                    <th><label>Do you know the customer personally?</label></th>
                    <td>
                        <asp:RadioButton ID="radYes" runat="server" Text="Yes" GroupName="knowCustomer" Value="1" />
                        <asp:RadioButton ID="radNo" runat="server" Text="No" GroupName="knowCustomer" Value="0" />
                    </td>
                </tr>

                <tr>
                        <th><label for="ddlYearsKnown">How many years have you known the customer?</label></th>
                    <td>
                        <asp:TextBox ID="CustomerYearsKnown" runat="server" ></asp:TextBox>                    
                    </td>
                </tr>
     
           <tr>

<tr>
    <th><label for="txtPersonName">Mr:</label></th>
    <th><asp:TextBox ID="txtPersonName" runat="server"></asp:TextBox></th>

    <th><label for="txt">is My</label></th>
    <td colspan="4">
        <div class="form-group">
            <asp:CheckBox ID="chkClassmate" runat="server" Text="Classmate"  onclick="limitCheckboxes(this)" />
        </div>
        <div class="form-group">
            <asp:CheckBox ID="chkNeighbor" runat="server" Text="Neighbor"  onclick="limitCheckboxes(this)" />
        </div>
        <div class="form-group">
            <asp:CheckBox ID="chkPersonalFriend" runat="server" Text="Personal Friend"  onclick="limitCheckboxes(this)" />
        </div>
        <div class="form-group">
            <asp:CheckBox ID="chkOthers" runat="server" Text="Others"  onclick="limitCheckboxes(this)" />
        </div>
    </td>
</tr>

<script type="text/javascript">
    function limitCheckboxes(checkbox) {
        var checkboxes = document.querySelectorAll('[id*="chk"]');

        for (var i = 0; i < checkboxes.length; i++) {
            if (checkboxes[i] !== checkbox) {
                checkboxes[i].checked = false;
            }
        }
    }
</script>
        
               <tr>
                        <th><span>I do not know the customer personally, but he was introduced to me by Mr:</span></th>
                        <td><asp:TextBox ID="txtIntroducedBy" runat="server"  ></asp:TextBox></td>
                        <th><span>of</span></th>
                        <td><asp:TextBox ID="txtCompanyName" runat="server"  ></asp:TextBox></td>
                    <th><span>(company name)</span></th>
                </tr>

                 <tr>
                    <th class="form-group">
                        <label>The customer was working as:</label>
                    </th>
                        <td><asp:TextBox ID="txtWorkTitle" runat="server"  ></asp:TextBox></td>
                        <th><span>for</span></th>
                        <td><asp:TextBox ID="txtWorkYears" runat="server"  ></asp:TextBox></td>
                        <th><span>years in</span></th>
                        <td><asp:TextBox ID="txtWorkCompany" runat="server"  ></asp:TextBox></td>
                        <th><span>(company name)</span></th>
                </tr>
                <tr>
                    <th class="form-group">
                        <label>Reason for leaving last company customer was working in:</label>
                    </th>
                    <td class="form-group">
                        <asp:TextBox ID="txtReasonForLeaving" runat="server"  ></asp:TextBox>
                    </td>
                </tr>

                   <tr>
                    <th class="form-group">
                        <label>Has the customer worked in another trade or occupation previously?</label>
                    </th>
                    <td class="form-group radio-group">
                        <asp:RadioButton ID="radWorkedInOtherTradeYes" runat="server" Text="Yes" GroupName="workedInOtherTrade" />
                        <asp:RadioButton ID="radWorkedInOtherTradeNo" runat="server" Text="No" GroupName="workedInOtherTrade" />
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
                        <asp:TextBox ID="txtDaysOpenPerWeek" runat="server"  ></asp:TextBox>
                    </td>
                </tr>
               <tr>
                    <th class="form-group">
                        <label>What are the usual working hours:</label>
                    </th>
                    <td class="form-group">
                        <input type="time" id="txtStartHourAM" runat="server" style="width: 100px;" />
                        <asp:Label runat="server" Text=" to " CssClass="form-label"></asp:Label>
                        <input type="time" id="txtEndHourPM" runat="server" style="width: 100px;" />
                    </td>
                </tr>




                <tr>
                    <th class="form-group">
                        <label>Does the customer play heavily in the stock market?</label>
                    </th>
                    <td class="form-group radio-group">
                        <asp:RadioButton ID="radStockMarketYes" runat="server" Text="Yes" GroupName="stockMarket" />
                        <asp:RadioButton ID="radStockMarketNo" runat="server" Text="No" GroupName="stockMarket" />
                    </td>
                </tr>
                <tr>
                    <th class="form-group">
                        <label>Does the customer involve in borrowing/gambling (any type)?</label>
                    </th>
                    <td class="form-group radio-group">
                        <asp:RadioButton ID="radBorrowingGamblingYes" runat="server" Text="Yes" GroupName="borrowingGambling" />
                        <asp:RadioButton ID="radBorrowingGamblingNo" runat="server" Text="No" GroupName="borrowingGambling" />
                    </td>
                </tr>
                <tr>
                    <th class="form-group">
                        <label>Do the partners get along well?</label>
                    </th>
                    <td class="form-group radio-group">
                        <asp:RadioButton ID="radPartnersGetAlongWellYes" runat="server" Text="Yes" GroupName="partnersGetAlongWell" />
                        <asp:RadioButton ID="radPartnersGetAlongWellNo" runat="server" Text="No" GroupName="partnersGetAlongWell" />
                    </td>
                </tr>
                <tr>
                    <th class="form-group">
                        <label>Has the customer had marital problems due to outside interests?</label>
                    </th>
                    <td class="form-group radio-group">
                        <asp:RadioButton ID="radMaritalProblemsYes" runat="server" Text="Yes" GroupName="maritalProblems" />
                        <asp:RadioButton ID="radMaritalProblemsNo" runat="server" Text="No" GroupName="maritalProblems" />
                    </td>
                </tr>
                <tr>
                    <th class="form-group">
                        <label>Does the customer keep his promises? (Is the customer reliable)?</label>
                    </th>
                    <td class="form-group radio-group">
                        <asp:RadioButton ID="radKeepPromisesYes" runat="server" Text="Yes" GroupName="keepPromises" />
                        <asp:RadioButton ID="radKeepPromisesNo" runat="server" Text="No" GroupName="keepPromises" />
                    </td>
                </tr>
                <tr>
                    <th class="form-group">
                        <label>Has any of his cheques been dishonored?</label>
                    </th>
                    <td class="form-group radio-group">
                        <asp:RadioButton ID="radChequesDishonoredYes" runat="server" Text="Yes" GroupName="chequesDishonored" />
                        <asp:RadioButton ID="radChequesDishonoredNo" runat="server" Text="No" GroupName="chequesDishonored" />
                    </td>
                </tr>

         <tr>
                <th class="form-group">
                    <label>How do you find the customer?</label>
                </th>
                <td class="form-group" colspan="3">
                    <table>
                        <tr>
                            <td class="checkbox-item">
                                <asp:CheckBox ID="chkAggressive" runat="server" Text="Aggressive" />
                            </td>
                            <td class="checkbox-item">
                                <asp:CheckBox ID="chkOrganized" runat="server" Text="Organized" />
                            </td>
                            <td class="checkbox-item">
                                <asp:CheckBox ID="chkUnableToExpress" runat="server" Text="Unable to express himself" />
                            </td>
                        </tr>
                        <tr>
                            <td class="checkbox-item">
                                <asp:CheckBox ID="chkCapable" runat="server" Text="Capable" />
                            </td>
                            <td class="checkbox-item">
                                <asp:CheckBox ID="chkQuiet" runat="server" Text="Quiet" />
                            </td>
                            <td class="checkbox-item">
                                <asp:CheckBox ID="chkUnorganized" runat="server" Text="Unorganized" />
                            </td>
                        </tr>
                        <tr>
                            <td class="checkbox-item">
                                <asp:CheckBox ID="chkEloquent" runat="server" Text="Eloquent" />
                            </td>
                            <td class="checkbox-item">
                                <asp:CheckBox ID="chkSkillful" runat="server" Text="Skillful" />
                            </td>
                            <td class="checkbox-item">
                                <asp:CheckBox ID="chkUnskillful" runat="server" Text="Unskillful" />
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
                        <asp:TextBox ID="txtHobbiesInterests" runat="server"  ></asp:TextBox>
                    </td>
                </tr>

                <th class="form-group">
                        <label>Is the shop located along the main road? Busy junction?</label>
                    </th>
                    <td class="form-group radio-group">
                        <asp:RadioButton ID="radMainRoadYes" runat="server" Text="Yes" GroupName="mainRoad" />
                        <asp:RadioButton ID="radMainRoadNo" runat="server" Text="No" GroupName="mainRoad" />
                    </td>
                </tr>
                <tr>
                    <th class="form-group">
                        <label>Is the shop location difficult to find?</label>
                    </th>
                    <td class="form-group radio-group">
                        <asp:RadioButton ID="radDifficultToFindYes" runat="server" Text="Yes" GroupName="difficultToFind" />
                        <asp:RadioButton ID="radDifficultToFindNo" runat="server" Text="No" GroupName="difficultToFind" />
                    </td>
                </tr>
              
                <tr>
                    <th class="form-group">
                        <label>Is the shop busy with customers?</label>
                    </th>
                    <td class="form-group">
                        <div class="checkbox-group">
                            <div class="checkbox-item">
                                <asp:CheckBox ID="chkAlwaysBusy" runat="server" Text="Always" onclick="handleCheckboxClick(this)" />
                            </div>
                            <div class="checkbox-item">
                                <asp:CheckBox ID="chkUsuallyBusy" runat="server" Text="Usually" onclick="handleCheckboxClick(this)" />
                            </div>
                            <div class="checkbox-item">
                                <asp:CheckBox ID="chkSometimesBusy" runat="server" Text="Sometimes" onclick="handleCheckboxClick(this)" />
                            </div>
                            <div class="checkbox-item">
                                <asp:CheckBox ID="chkNeverBusy" runat="server" Text="Never" onclick="handleCheckboxClick(this)" />
                            </div>
                        </div>
                    </td>
                </tr>

                <tr>
                    <th class="form-group">
                        <label>From your observation, are the goods in the shop moving fast?</label>
                    </th>
                    <td class="form-group radio-group">
                        <asp:RadioButton ID="radGoodsMovingFastYes" runat="server" Text="Yes" GroupName="goodsMovingFast" />
                        <asp:RadioButton ID="radGoodsMovingFastNo" runat="server" Text="No" GroupName="goodsMovingFast" />
                    </td>
                </tr>

                  <th><h5>(C) FINACIAL STABLILITY</h5></th>

                   <tr>
                    <th class="form-group">
                        <label>Does the workshop/shop belong to the customer?</label>
                    </th>
                    <td class="form-group radio-group">
                        <asp:RadioButton ID="radBelongsToCustomerYes" runat="server" Text="Yes" GroupName="belongsToCustomer" />
                        <asp:RadioButton ID="radBelongsToCustomerNo" runat="server" Text="No" GroupName="belongsToCustomer" />
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
                        <asp:TextBox ID="txtRentalInstallment" runat="server"   style="margin-left: 5px;"></asp:TextBox>
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
                        <asp:TextBox ID="txtOwnsHouseNo" runat="server"  ></asp:TextBox>
                        <br /><br />
                        <span>Type:</span>
                        <asp:TextBox ID="txtOwnsHouseType" runat="server"  ></asp:TextBox>
                        <br /><br />
                        <span>Location:</span>
                        <asp:TextBox ID="txtOwnsHouseLocation" runat="server"  ></asp:TextBox>
                    </td>
                </tr>


                   <tr>
                    <th class="form-group">
                        <label>If the customer has financial assistance from someone else, please advise assisted by:</label>
                    </th>
                    <td class="form-group">
                        <asp:TextBox ID="txtFinacialBy" runat="server"  ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th class="form-group">
                        <label>The customer's other assets are:</label>
                    </th>
                    <td class="form-group">
                        <asp:TextBox ID="txtOtherAssets" runat="server"  ></asp:TextBox>
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
                    <asp:RadioButton ID="rdoVeryGood" runat="server" Text="Very Good" GroupName="consideration" />
                    <asp:RadioButton ID="rdoGood" runat="server" Text="Good" GroupName="consideration" />
                    <asp:RadioButton ID="rdoAverage" runat="server" Text="Average" GroupName="consideration" />
                </div>
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

    <td>
            <div class="form-group">
               <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
               <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />

        </div>
        </td>

                <td>
                    <button type="button" onclick="goBack()">Close</button>
                </td>
            </tr>
 </table>


    </div>
</form>
   
</body>
</html>
