<%@ Page Title="Appointment" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Visitor_CheckInOut.aspx.cs" Inherits="DotNet.Visitor_CheckInOut" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <asp:UpdateProgress runat="server" ID="UpdateProgress" class="gettext" AssociatedUpdatePanelID="upBatch">
            <ProgressTemplate>
                <div class="loading">
                    <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="upBatch" runat="server">
            <ContentTemplate>
                <asp:Panel ID="CheckAppointmentWrapper" runat="server">
                    <div style="display: flex; justify-content: center; margin: 10px">
                        <div class="row mb-1 mt-3 col-12">
                            <div class="col-3 col-s-4">
                                <label class="labeltext">Appointment ID:        </label>
                            </div>
                            <div class="col-7 col-s-6" style="display: flex; justify-content: center;">
                                <asp:TextBox ID="txtAppointmentID" class="" runat="server" Text=" " PlaceHolder="Please Key In Your Appointment ID" Style="width: 100%; height: 30px;"></asp:TextBox>
                            </div>
                        </div>


                    </div>
<%--                    <div style="display: flex; justify-content: center; margin: 10px">
                        <div class="row mb-1 mt-3 col-12">
                            <div class="col-3 col-s-4">
                                <label class="labeltext">Full Name:        </label>
                            </div>
                            <div class="col-7 col-s-6" style="display: flex; justify-content: center;">
                                <asp:TextBox ID="txtFullName" class="" runat="server" Text=" " PlaceHolder="Please Key In Your Registered Full Name" Style="width: 100%; height: 30px;"></asp:TextBox>
                            </div>
                        </div>
                    </div>--%>
                    <div style="display: flex; justify-content: center; margin: 10px">

                        <asp:Button ID="BtnSubmit" runat="server" OnClick="CheckAppointmentID" Text="Proceed" class="glow_green button-margin" Enabled="true" />
                    </div>
                </asp:Panel>

                <asp:Panel ID="PerformActionWrapper" runat="server" Visible="false">

                    <div style="display: flex; justify-content: center; margin: 10px; justify-content: space-evenly">
                        <%--<button id="BtnOpenModal" type="button" class="glow_green" onclick="OpenCloseModal(true)">Cancel Appointment</button>--%>
                        <asp:Button ID="BtnCheckInOut" runat="server" OnClick="ActionButton_Click" Text="Check In" class="glow_green button-margin" value="1" Enabled="true" />


                    </div>


                </asp:Panel>

                <asp:Panel ID="CompletedWrapper" runat="server" Visible="false">
                    <div style="text-align:center" class="p-4">
                        <h3 style="font-weight: 900">You have completed your checkout.</h3>
                    </div>
                </asp:Panel>


                <div id="CancelModal" class="modal" tabindex="-1" role="dialog" style="align-content: center; background-color: rgb(10 10 10 / 75%);">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content p-2" style="text-align: center">
                            <h3>Are you certain you would like to cancel your appointment?</h3>
                            <div class="modal-footer" style="display: flex; justify-content: center">
                                <div class="row" style="display: flex; justify-content: center;">
                                    <button id="BtnBack" type="button" class="glow_green" onclick="OpenCloseModal(false)" style="margin-right: 5px;">No</button>
                                    <asp:Button ID="BtnCancelAppointment" runat="server" OnClick="ActionButton_Click" Text="Yes" class="glow_green button-margin" value="0" Enabled="true" Style="margin-left: 5px" />

                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <script>
                    function OpenCloseModal(isOpen) {
                        if (isOpen) {
                            $('#CancelModal').show();
                        } else {
                            $('#CancelModal').hide();
                        }
                    }
                </script>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>

</asp:Content>

