<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CheckIn.aspx.cs" Inherits="DotNet.CheckIn" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="shortcut icon" href="RESOURCES/LFIB_icon.ico"/>
    <link href="STYLES/xxtra_global_Content_Adjuster.css" rel="stylesheet" />
    <link href="STYLES/xtra_Flip_Image.css" rel="stylesheet" />
    <link href="STYLES/xtra_Glowing.css" rel="stylesheet" />

    <title>Check-In</title>

    <meta http-equiv="X-UA-Compatible" content="IE=Edge"/>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>    
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0"/>  
    <meta name="apple-mobile-web-app-capable" content="yes"/>
    <meta name="mobile-web-app-capable" content="yes"/>

    <script src="scripts/BrowserReload_ThroughHistory.js"></script>
</head>
<body>
    <form runat="server">
        <div class="row">
           
             <div class="col-12 col-s-12 image_icon" style="border-bottom: 3px solid #45A29E;">
                     <img src="RESOURCES/CheckInLion.png" class="image_resize"/> 
            </div>
 
              <div class="col-12 col-s-12">
              <!--==============================================================================-->
                <asp:ScriptManager ID="ScriptManager1" runat="server"/>
                <asp:UpdateProgress runat="server" id="UpdateProgress2" class="gettext" AssociatedUpdatePanelID="UpdatePanel1">
                    <ProgressTemplate>
                                <div class="loading">
                            
                                        <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                            </div> 

                    </ProgressTemplate>
                </asp:UpdateProgress>
                
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                <div class="col-12 col-s-12" runat="server"  style="text-align: center;">
                
                        <asp:TextBox ID="TextBox_Name" class="inputtext_2" autocomplete="on" placeholder="Your Name" runat="server"></asp:TextBox>
               
                <br /><br />
                        <asp:TextBox ID="TextBox_Phone" class="inputtext_2" autocomplete="on" placeholder="Your Phone Number" runat="server" Minlength="10" MaxLength="12"></asp:TextBox>
                <br /><br />
                        <asp:Label ID="Label_Location" class="gettext" runat="server"></asp:Label>
                <br /><br />          
                <asp:Button ID="Button_Confirm" runat="server"  OnClick="Button_Confirm_click" Text="Confirm" class="glow_green"/>
                </div>
                </ContentTemplate>
                </asp:UpdatePanel>
              <!--==============================================================================--> 
              </div>
        </div>

        <div class="footer">
    	    <p>&copy; <script>document.write(new Date().getFullYear())</script>
 			    LION POSIM BERHAD 198201002310 (82056-X) All Rights Reserved.</p>
        </div>

    </form>
   
</body>
</html>
