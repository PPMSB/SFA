    <%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DraftCustomerGPS.aspx.cs" Inherits="DotNet.DraftCustomerGPS" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <script>
        function goBack() {
            window.location.href = 'NewCustomer.aspx';
        }
    </script>

 
    <script>
        function getLocationCoordinate() {
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(
                    function (position) {
                        // Display coordinates in text box
                        var longitude = position.coords.longitude.toFixed(6);
                        var latitude = position.coords.latitude.toFixed(6);
                        var formattedCoordinates = latitude + ', ' + longitude;

                        document.getElementById('<%=Coordinate.ClientID %>').value = formattedCoordinates;
                    },
                    function (error) {
                        alert('Error getting location: ' + error.message);
                    }
                );
            } else {
                alert('Geolocation is not supported by this browser.');
            }
        }
    </script>

<script type="text/javascript">
    function showSuccessMessage() {
        // Display a confirmation dialog with "OK" and "Cancel" options
        var confirmation = confirm("Form submitted successfully! Click OK to proceed to AddApplication.aspx.");

        // If the user clicks "OK", execute the redirection
        if (confirmation) {
            redirectToAddApplication();
        } else {
            // If the user clicks "Cancel" or closes the dialog, redirect to NewCustomer.aspx
            goBack();
        }
    }

    function redirectToAddApplication() {
        // Redirect to AddApplication.aspx
        window.location.href = 'AddApplication.aspx';
    }

    function goBack() {
        // Redirect to NewCustomer.aspx
        window.location.href = 'NewCustomer.aspx';
    }

    function showErrorMessage() {
        // Display an error message
        alert("Form submission failed. Please try again.");
    }
</script>




<head runat="server">
 <link rel="shortcut icon" href="RESOURCES/LFIB_icon.ico" />

    <script src="scripts/GoToTab.js"></script>
    <link href="STYLES/xxtra_global_Content_Adjuster.css" rel="stylesheet" />
    <link href="STYLES/xtra_Grid_View.css" rel="stylesheet" />
    <link href="STYLES/xtra_Glowing.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link href="STYLES/DraftCust.css" rel="stylesheet" />


    <title>Draft Customer GPS</title>
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="mobile-web-app-capable" content="yes" />
    <script src="https://api.mapbox.com/mapbox-gl-js/v2.9.1/mapbox-gl.js"></script>
    <link href="https://api.mapbox.com/mapbox-gl-js/v2.9.1/mapbox-gl.css" rel="stylesheet" />
    <script defer src="scripts/gps-map.js"></script>

    <script src="scripts/BrowserReload_ThroughHistory.js"></script>

    

     
    <!--==============================================================================-->
   <header>

     New Customer Account Applicant - Draft Customer GPS
            </header>


</head>
<body> 
    <form id="DraftForm" runat="server" onsubmit="return submitForm()">
        <div class="DraftForm">
            <table class="form-table">
                <tr class="form-group">
                    <th>
                        <label for="SalesmanNameDropdown">Select Salesman Name:</label>
                    </th>
                    <td>
                        <asp:DropDownList ID="DropDownListSalesmanName" runat="server" Required="true" CssClass="control"></asp:DropDownList>
                    </td>
                      <td>
            <label for="Status">Status:</label>
        </td>
        <td>
            <asp:TextBox ID="status" runat="server" ReadOnly="true" Text="DRAFT" CssClass="control"></asp:TextBox>
        </td>
       
    </tr>

                <tr class="form-group">
                    <th>
                        <label for="SalesmanCodeDropdown">Select Salesman Code:</label>
                    </th>
                    <td>
                        <asp:DropDownList ID="DropDownListSalesmanCode" runat="server" Required="true" CssClass="control"></asp:DropDownList>
                    </td>
                </tr>

            <tr class="form-group">
                <th>
                    <label for="Company">Company:</label>
                </th>
                <td colspan="5">
                    <asp:TextBox ID="Company" runat="server" TextMode="MultiLine" ReadOnly="true" Text="Posim Petroleum Marketing Sdn Bhd"></asp:TextBox>
                </td>
            </tr>



                <tr class="form-group">
                    <th>
                        <label for="CustName">Customer Name:</label>
                    </th>
                    <td>
                        <asp:TextBox ID="TextBoxCustName" runat="server" Required="true" CssClass="control"></asp:TextBox>
                    </td>
                </tr>

               <tr class="form-group">
    <th>
        <label for="Coordinate">Coordinates:</label>
    </th>
    <td>
        <asp:TextBox ID="Coordinate" runat="server" Required="true" CssClass="control coordinate-textbox"></asp:TextBox>
<button type="button" onclick="getLocationCoordinate()">Get Coordinate</button>

    </td>
</tr>


                <tr class="form-group">
                    <td>
                        <asp:RadioButton ID="ExistDoc" runat="server" Text="Existing Document (Same as Parent Document)" Value="0" GroupName="DocumentType" CssClass="control" />
                    </td>
                    <td>
                        <asp:RadioButton ID="NewDoc" runat="server" Text="New Document" Value="1" GroupName="DocumentType" CssClass="control" />
                    </td>
                </tr>

                <tr>
                 <div class="form-group">
                     <td><asp:Button ID="submitFormButton" runat="server" Text="Save" OnClick="submitFormButton_Click" /></td>
                   <td> <button type="button" onclick="goBack()">Close</button></td>
                </div>
                </tr>
            </table>

            <div id="output" runat="server"></div>
        </div>
    </form>

</body>     
</html>
