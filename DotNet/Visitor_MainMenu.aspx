<%@ Page Title="Main Menu" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Visitor_MainMenu.aspx.cs" Inherits="DotNet.Visitor_MainMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <form runat="server">

        <div class="col-12 col-s-12" style="text-align: center">
            <!--==============================================================================-->
            <a href="Visitor_CreateNewAppointment.aspx" id="NewAppointmentTag" runat="server" style="text-decoration: none; border: 0;" visible="false">
                <div class="image_properties">
                    <img src="RESOURCES/CreateNew.png" class="menuimage_nohighlight" alt="Customer" />
                    <img src="RESOURCES/HOVERFRAME.png" class="image_highlight" alt="Customer" />
                </div>
            </a>

            <a href="Visitor_History.aspx" id="AppointmentHistoryTag" runat="server" style="text-decoration: none; border: 0;" visible="false">
                <div class="image_properties">
                    <img src="RESOURCES/Appointment.png" class="menuimage_nohighlight" alt="NewCustomer" />
                    <img src="RESOURCES/HOVERFRAME.png" class="image_highlight" alt="NewCustomer" />
                </div>
            </a>

            <%if (Session["UserRole"].ToString() != "3")
                { %>
            <div class="image_properties sfa" id="SFATag" runat="server" style="cursor: pointer">
                <img src="RESOURCES/BackToSFA.png" class="menuimage_nohighlight" alt="Setting" style="border: 1px solid black; border-radius: 5px;" />
                <img src="RESOURCES/HOVERFRAME.png" class="image_highlight" alt="Setting" />
            </div>
            <%} %>
        </div>

        <!--==============================================================================-->
        <asp:ScriptManager ID="ScriptManager1" runat="server" />

        <asp:UpdateProgress runat="server" ID="UpdateProgress2" class="gettext">
            <ProgressTemplate>
                <div class="loading">
                    <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </form>

    <script>
        $(document).ready(function () {
            $('.sfa').on('click', function () {
                var userName = '<%= Session["UserName"] %>';
                var productionWebsite = '<%= GLOBAL_VAR.GLOBAL.ProductionWebsite %>';

                $.ajax({
                    type: "POST",
                    url: "/MainMenu.aspx/RedirectToMainMenu",
                    data: JSON.stringify({ UserName: userName }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        window.location.href = productionWebsite + "MainMenu.aspx";
                    }
                });
            })

            $("#SFATag").on('click', function () {
                var userName = '<%= Session["UserName"] %>';
                var productionWebsite = '<%= GLOBAL_VAR.GLOBAL.ProductionWebsite %>';

                $.ajax({
                    type: "POST",
                    url: "/MainMenu.aspx/RedirectToMainMenu",
                    data: JSON.stringify({ UserName: userName }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        window.location.href = productionWebsite + "MainMenu.aspx";
                    }
                });
            })
        })
    </script>
</asp:Content>
