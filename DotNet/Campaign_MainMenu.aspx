<%@ Page Title="Main Menu" Language="C#" MasterPageFile="~/Campaign_Site.Master" AutoEventWireup="true" CodeBehind="Campaign_MainMenu.aspx.cs" Inherits="DotNet.Campaign_MainMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <form runat="server">
        <asp:UpdateProgress runat="server" ID="UpdateProgress" class="gettext">
            <ProgressTemplate>
                <div class="loading">
                    <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <div class="col-12 col-s-12" style="text-align: center">
            <!--==============================================================================-->

            <%if (GLOBAL_VAR.GLOBAL.CampaignReport != false)
                {
            %>
            <a href="Campaign_SubmitForm.aspx" id="A1" runat="server" style="text-decoration: none; border: 0;">
                <div class="image_properties">
                    <img src="RESOURCES/CampaignReport.png" class="menuimage_nohighlight" alt="CampaignReport" />
                    <img src="RESOURCES/HOVERFRAME.png" class="image_highlight" alt="CampaignReport" />
                </div>
            </a>
            <%}
                else
                { %>
            <a href="Campaign_CreateNewOffer.aspx" id="NewAppointmentTag" runat="server" style="text-decoration: none; border: 0;">
                <div class="image_properties">
                    <img src="RESOURCES/NewForm.png" class="menuimage_nohighlight" alt="Offer" />
                    <img src="RESOURCES/HOVERFRAME.png" class="image_highlight" alt="Offer" />
                </div>
            </a>


            <a href="Campaign_SubmitForm.aspx?download=true" id="A3" runat="server" style="text-decoration: none; border: 0;">
                <div class="image_properties">
                    <img src="RESOURCES/Download.png" class="menuimage_nohighlight" alt="DownloadForm" />
                    <img src="RESOURCES/HOVERFRAME.png" class="image_highlight" alt="DownloadForm" />
                </div>
            </a>
            <a href="Campaign_SubmitForm.aspx" id="A4" runat="server" style="text-decoration: none; border: 0;">
                <div class="image_properties">
                    <img src="RESOURCES/SubmitForm.png" class="menuimage_nohighlight" alt="DownloadForm" />
                    <img src="RESOURCES/HOVERFRAME.png" class="image_highlight" alt="DownloadForm" />
                </div>
            </a>



            <%} %>
            <asp:Button runat="server" ID="btnHidden" Style="display: none" OnClick="DownloadInformation" />

            <div class="image_properties" id="DownloadInformationURL" style="cursor: pointer">
                <img src="RESOURCES/DownloadVPPPInformation.png" class="menuimage_nohighlight" alt="Information" style="border: 1px solid black; border-radius: 5px; width: 128px;" />
                <img src="RESOURCES/HOVERFRAME.png" class="image_highlight" alt="Information" />
            </div>

            <a href="MainMenu.aspx" id="A2" runat="server" style="text-decoration: none; border: 0;">

                <div class="image_properties">
                    <img src="RESOURCES/BackToSFA.png" class="menuimage_nohighlight" alt="Setting" style="border: 1px solid black; border-radius: 5px;" />
                    <img src="RESOURCES/HOVERFRAME.png" class="image_highlight" alt="Setting" />
                </div>
            </a>

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
        $('#DownloadInformationURL').on('click', function () {
            var btnHidden = $('#<%= btnHidden.ClientID %>');
            if (btnHidden != null) {
                btnHidden.click();
            }
        })

    </script>
</asp:Content>
