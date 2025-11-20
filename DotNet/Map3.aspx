<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Map3.aspx.cs" Inherits="DotNet.Map3" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="shortcut icon" href="RESOURCES/LFIB_icon.ico" />
    <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
    <script src="scripts/GoToTab.js"></script>
    <link href="STYLES/xxtra_global_Content_Adjuster.css" rel="stylesheet" />
    <link href="STYLES/xtra_Grid_View.css" rel="stylesheet" />
    <link href="STYLES/xtra_Glowing.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
    <title>Setting</title>
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="mobile-web-app-capable" content="yes" />

    <script src="scripts/BrowserReload_ThroughHistory.js"></script>

    <script async defer src="https://maps.googleapis.com/maps/api/js?key=AIzaSyDW7gX8oJkzygXu92IBAQ6SJ_CvIiZEs3c&libraries=places&callback=initMap"></script>

    <style>
        #map {
            height: 500px;
            width: 100%;
            border: 1px solid #ccc;
            border-radius: 8px;
            margin-bottom: 20px;
        }

        #overlay {
            top: 82%;
            width: 100%;
            /* Set the width of the overlay */
            height: 24%;
            /* Adjust the background color and opacity */
            z-index: 2; /* Set a higher z-index to appear on top of the map */
        }

        #locationDropdown {
            z-index: 3; /* Set a higher z-index than the overlay to appear on top of it */
        }

        #stateInfo {
            font-size: 18px;
            font-weight: bold;
        }
    </style>
    <script>
        var map;

        function initMap() {
            // Coordinates for multiple locations
            var coordinateList = [
                { lat: 3.8491032319773484, lng: 103.02780973055721 }
            ];

            // Create a new map centered at the first coordinate
            var map = new google.maps.Map(document.getElementById('map'), {
                center: coordinateList[0],
                zoom: 8
            });

            // Iterate over the coordinateList and add markers for each coordinate
            //for (var i = 0; i < coordinateList.length; i++) {
            //    var marker = new google.maps.Marker({
            //        position: coordinateList[i],
            //        map: map,
            //        title: 'Marker ' + (i + 1)
            //    });
            //}
        }

        function updateMap(data) {
            var coordinatesList = data.coordinates;
            var namesList = data.names;

            console.log('Received coordinates:', coordinatesList);
            var icon = {
                url: 'RESOURCES/PinPoint.png',
                scaledSize: new google.maps.Size(66, 66), // Size of the icon
                origin: new google.maps.Point(0, 0), // Position of the icon within the image sprite
                anchor: new google.maps.Point(20, 40) // Position at which to anchor the marker
            };

            var map = new google.maps.Map(document.getElementById('map'), {
                center: coordinatesList[0],
                zoom: 5
            });

            var bounds = new google.maps.LatLngBounds();
            var infoWindow = new google.maps.InfoWindow(); //Single instance of InfoWindow

            // Iterate over the coordinatesList and add markers for each valid coordinate
            for (var i = 0; i < coordinatesList.length; i++) {
                var parts = coordinatesList[i].split(',');
                var name = namesList[i];
                console.log('Title:', name);

                // Ensure that the coordinate has both latitude and longitude parts
                if (parts.length === 2) {
                    var lat = parseFloat(parts[0]);
                    var lng = parseFloat(parts[1]);

                    // Check if lat and lng are valid numbers
                    if (!isNaN(lat) && !isNaN(lng)) {
                        var position = { lat: lat, lng: lng };
                        console.log('Marker position:', position);

                        if (map) {
                            var marker = new google.maps.Marker({
                                position: position,
                                map: map,
                                icon: icon,
                                title: name // Optional: used for debugging or accessibility
                            });

                            // Add a click event listener to show the name on click
                            (function (marker, name) {
                                //var infoWindow = new google.maps.InfoWindow({
                                //    content: `<div style="font-size:14px;font-weight:bold;">${name}</div>`
                                //});

                                marker.addListener('click', function () {
                                    infoWindow.setContent(`<div style="font-size:14px;font-weight:bold;">${name}</div>`);
                                    // Close any other open info windows and then open this one
                                    infoWindow.open(map, marker);
                                });
                            })(marker, name);
                        } else {
                            console.error('Google map is not initialized.');
                        }

                        bounds.extend(position);
                    } else {
                        console.error('Invalid coordinate:', coordinatesList[i]);
                    }
                } else {
                    console.error('Invalid coordinate format:', coordinatesList[i]);
                }
            }

            map.fitBounds(bounds);
        }


        // JavaScript function to handle dropdown list selection change
        function ddlState_SelectedIndexChanged() {
            var selectedState = document.getElementById('ddlState').value;

            console.log(selectedState);
            // Make an AJAX call to the GetCoordinates WebMethod
            $.ajax({
                type: "POST",
                url: "Map3.aspx/GetCoordinates",
                //data: JSON.stringify({ selectedState: selectedState }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ selectedState: selectedState }),
                success: function (response) {

                    // Call updateMap function with received data
                    var data = JSON.parse(response.d);
                    console.log(data);
                    updateMap(data);
                },
                error: function (xhr, status, error) {
                    console.error("Error:", error);
                }
            });
        }

        // JavaScript function to handle dropdown list selection change
        function ddlCustomerMainGroup_SelectedIndex() {
            //var selectedState = document.getElementById('ddlState').value;
            var selectedGroup = document.getElementById('ddlCustomerMainGroup').value;
            console.log(selectedGroup);
            // Make an AJAX call to the GetCoordinates WebMethod
            $.ajax({
                type: "POST",
                url: "Map3.aspx/GetGroupCoordinates",

                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ customerGroup: selectedGroup }),
                success: function (response) {

                    // Call updateMap function with received data
                    var data = JSON.parse(response.d);
                    //console.log(stateCoordinate);
                    updateMap(data);
                },
                error: function (xhr, status, error) {
                    console.error("Error:", error);
                }
            });
        }


    </script>

</head>

<body>
    <form id="form1" runat="server">
        <div class="row">
            <div class="mobile_hidden">
                <div class="col-3 col-s-3 image_icon">
                    <img src="RESOURCES/LOCATION.png" class="image_resize" />
                </div>

                <div class="col-9 col-s-9 image_title">
                    <h1>Map</h1>
                </div>
            </div>
            <!--==============================================================================-->
            <link href="STYLES/xtra_top_navigation.css" rel="stylesheet" />
            <div class="topnav" id="myTopnav">
                <a href="MainMenu.aspx">Home</a>
                <a href="CustomerMaster.aspx" id="CustomerMasterTag2" runat="server" visible="false">Customer</a>
                <a href="SFA.aspx" id="SFATag2" runat="server" visible="false">Sales</a>
                <a href="SalesQuotation.aspx" id="SalesQuotation2" runat="server" visible="false">Quotation</a>
                <a href="Payment.aspx" id="PaymentTag2" runat="server" visible="false">Payment</a>
                <a href="Redemption.aspx" id="RedemptionTag2" runat="server" visible="false">Redemption</a>
                <%--<a href="EOR.aspx" id="EORTag2" runat="server" visible="false">EOR</a>--%>
                <a href="SignboardEquipment.aspx" id="EORTag2" runat="server" visible="true">Sign & Equip</a>
                <a href="InventoryMaster.aspx" id="InventoryMasterTag2" runat="server" visible="false">Inventory</a>
                <a href="WClaim.aspx" id="WClaimTag2" runat="server" visible="false">Warranty</a>
                <%--<a href="SignboardEquipment.aspx" id="SignboardTag2" runat="server">Sign & Equip</a>--%>
                <a href="EventBudget.aspx" id="EventBudgetTag2" runat="server" visible="false">Event Budget</a>
                <a href="Setting.aspx" id="SettingTag2" class="active" runat="server">Setting</a>
                <a href="LoginPage.aspx" class="Log_Out top_nav_logout_properties">
                    <asp:Label runat="server" CssClass="fa fa-sign-out" Style="font-size: 20px;"></asp:Label>
                    <asp:Label runat="server" Text="Logout" Font-Bold="true"></asp:Label>

                    <%--                        <img src="RESOURCES/LogOut.png" />
                        <img src="RESOURCES/LogOut_h.png" class="top_nav_logout_highlight" />--%>
                </a>

                <a href="javascript:void(0);" class="icon" onclick="topnavigation()">
                    <div class="container" onclick=" myFunction(this);">
                        <div class="bar1"></div>
                        <div class="bar2"></div>
                        <div class="bar3"></div>
                    </div>
                </a>
                <script src="scripts/top_navigation.js"></script>
            </div>
        </div>

        <div id="map" runat="server" class="col-12"></div>
        <div id="overlay" runat="server" class="row">

            <table border="1" margin-bottom="50px">

                <tr>
                    <th>State</th>
                    <td>
                        <asp:DropDownList ID="ddlState" runat="server" onchange="ddlState_SelectedIndexChanged()"></asp:DropDownList>
                    </td>
                </tr>

                <tr>
                    <th>Customer Group</th>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlCustomerMainGroup" onchange="ddlCustomerMainGroup_SelectedIndex()"></asp:DropDownList>
                    </td>
                    <%--                    <td>
                        <select>
                            <option value="0">--SELECT--</option>
                            <option value="ALL">ALL</option>
                            <option value="2W">2W</option>
                            <option value="4W">4W</option>
                            <option value="IND">IND</option>
                        </select></td>--%>
                </tr>
                <tr>
                    <th>Area</th>
                    <td>
                        <select>
                            <option>--All--</option>
                        </select></td>

                </tr>

                <tr>
                    <td>
                        <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Text="Customer List" CssClass="col-12"></asp:Button>

                    </td>
                </tr>
                <%--                        nSearch_Click" />
       <button type="button" onclick="reloadMap()">Reload</button>                 </td>
                --%>
            </table>

        </div>
        <div id="stateInfo"></div>

    </form>
</body>

</html>


<script>

    function geocodeCountry(country, latitude, longitude, callback) {
        // Make a request to the Geocoding API
        var geocoder = new google.maps.Geocoder();
        geocoder.geocode({ 'address': country }, function (results, status) {
            if (status === 'OK' && results[0]) {
                var location = results[0].geometry.location;
                var coordinates = { lat: latitude, lng: longitude };
                callback(coordinates);
            } else {
                console.error('Geocoding failed. Status:', status);
            }
        });
    }

</script>
