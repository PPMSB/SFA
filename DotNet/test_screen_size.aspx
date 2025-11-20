<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test_screen_size.aspx.cs" Inherits="DotNet.test_screen_size" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="shortcut icon" href="RESOURCES/LFIB_icon.ico"/>

    <script src="scripts/GoToTab.js"></script>
  
    <title>Test</title>
     <meta http-equiv="X-UA-Compatible" content="IE=Edge"/>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>    
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0"/>  
    <meta name="apple-mobile-web-app-capable" content="yes"/>
    <meta name="mobile-web-app-capable" content="yes"/>
    <script src="scripts/BrowserReload_ThroughHistory.js"></script>

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script>
    $(document).ready(function(){
      $("button").click(function(){
        var txt = "";
        txt += "Width: " + $("#div1").width() + "</br>";
        txt += "Height: " + $("#div1").height();
        $("#div1").html(txt);
      });
    });
    </script>
    <style>
    #div1 {top:0;position:fixed;
      height: 100%;
      width: 100%;
      padding: 0px;
      margin:0px;
      border: 1px solid blue;
      background-color: lightblue;
    }
    </style>   


</head>


<body>
  <div id="div1"></div>
   <br /><br /><br />
    <button style="position:absolute">Display screen's dimension</button>
</body>
</html>
