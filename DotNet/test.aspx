<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="DotNet.test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="shortcut icon" href="RESOURCES/LFIB_icon.ico"/>

    <script src="scripts/GoToTab.js"></script>
    <link href="STYLES/xxtra_global_Content_Adjuster.css" rel="stylesheet" />
    <link href="STYLES/xtra_ButtonUp.css" rel="stylesheet" />
    <link href="STYLES/xtra_Grid_View.css" rel="stylesheet" />
    <link href="STYLES/xtra_Glowing.css" rel="stylesheet" />
    
    <title>Test</title>
     <meta http-equiv="X-UA-Compatible" content="IE=Edge"/>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>    
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0"/>  
    <meta name="apple-mobile-web-app-capable" content="yes"/>
    <meta name="mobile-web-app-capable" content="yes"/>
    
    <script src="scripts/BrowserReload_ThroughHistory.js"></script>
    <script src="scripts/PrintDiv.js"></script>

            <style type="text/css">
          html, body, #map-canvas { height: 100%; margin: 0; padding: 0;}
        </style>
<%--        <script type="text/javascript"
          src="https://maps.googleapis.com/maps/api/js?key=AIzaSyCnzUohfRwS_Z4C9vq1s9MJ6nJfyCHo5c0">
        </script>--%>
        <script type="text/javascript">
            function initialize() {
                var mapOptions = {
                    center: { lat: 3.140853, lng: 101.693207 },
                    zoom: 8
                };
                var map = new google.maps.Map(document.getElementById('map-canvas'),
                    mapOptions);
            }
            google.maps.event.addDomListener(window, 'load', initialize);
        </script>
</head>

<body>    
    <iframe src="http://maps.google.com/maps?q=3.140853,101.693207&z=8&output=embed" height="450" width="600"></iframe>

    <div id="map-canvas"></div>

   <div class="wrapper"><img src="RESOURCES/user.png" class="avatar"/>
      <div class="title">Check-In</div>
      <form runat="server">
        <div class="field">
          <input type="text" required="required"/>
          <label>Name</label>
        </div>
        <div class="field">
          <input type="password"  required="required"/>
          <label>Phone Number</label>
        </div>
        
        <div class="field">
            <asp:Button ID="Button_Confirm" runat="server"  OnClick="Button_Confirm_click" Text="Confirm"/>        
        </div>
       
      </form>
    </div>
    <div class="footer">
    	    <p>&copy; <script>document.write(new Date().getFullYear())</script>
 			    LION POSIM BERHAD 198201002310 (82056-X) All Rights Reserved.</p>
    </div>
    
</body>
</html>
