<%@ Page Title="Submit Form" Language="C#" MasterPageFile="~/Campaign_Site.Master" AutoEventWireup="true" CodeBehind="Campaign_SubmitForm.aspx.cs" Inherits="DotNet.Campaign_SubmitForm" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <link rel="shortcut icon" href="RESOURCES/LFIB_icon.ico" />
    <script src="scripts/GoToTab.js"></script>
    <link href="STYLES/xxtra_global_Content_Adjuster.css" rel="stylesheet" />
    <link href="STYLES/xtra_ButtonUp.css" rel="stylesheet" />
    <link href="STYLES/xtra_Grid_View.css" rel="stylesheet" />
    <link href="STYLES/xtra_Glowing.css" rel="stylesheet" />
    <link href="Content/form_custom.css" rel="stylesheet" />
    <link href="Content/custom.css" rel="stylesheet" />

    <style>
        .frame_style {
            margin-bottom: 5px;
        }

        .glow_green {
            min-width: 150px;
        }

        .button-margin {
            margin-right: 10px;
        }

        .CurrentPage {
            background-color: #b2b2b2 !important;
        }



        .serial-number-style {
            text-decoration: underline;
            font-weight: 900;
            font-size: 30px;
        }

        .bordered-container {
            border: 2px solid #007bff; /* Sets a solid border around the container */
            padding: 20px; /* Adds padding inside the border */
            margin: 20px 0; /* Adds margin to separate it from other elements */
            border-radius: 8px; /* Optional: rounds the corners of the border */
            background-color: #f9f9f9; /* Optional: adds a light background color */
        }

        .btn {
            border: 1px solid black;
            background-color: #eeeeee;
            border-radius: 20px;
            font-weight: 800;
        }

        .fit-content {
            max-width: fit-content;
            min-width: fit-content;
        }

        .aware-color {
            margin-bottom: 15px;
            display: flex;
            align-items: flex-end;
            justify-content: flex-start;
            margin-bottom: 15px
        }

        #DocumentRecordsSection td {
            font-size: 15px;
            padding-left: 5px;
            padding-right: 5px;
        }

        #DocumentRecordsSection table, td, th {
            border: 1px solid;
        }

        .btn-approve {
            font-size: 13px;
            margin-bottom: 5px;
            background-color: #4CAF50;
            color: white;
            transition: 0.5s;
        }

            .btn-approve:hover {
                transform: scale(1.1);
                background-color: #4CAF50;
                color: white;
            }

        .btn-reject {
            font-size: 13px;
            background-color: #F44336;
            color: white;
            transition: 0.5s;
        }

            .btn-reject:hover {
                transform: scale(1.1);
                background-color: #F44336;
                color: white;
            }

        .btn-cancel {
            font-size: 13px;
            background-color: #ff9227;
            color: white;
            transition: 0.5s;
        }

            .btn-cancel:hover {
                transform: scale(1.1);
                background-color: #ff9227;
                color: white;
            }

                        /* Custom CSS for the Download LinkButton */ 
            .btn-download {
        color: white !important;            /* White text */
        border: 2px solid #28a745 !important; /* Green border (Bootstrap's success color) */
        background-color: #28a745 !important; /* Transparent background */
        padding: 6px 12px;                 /* Adjust padding */
        text-decoration: none !important;  /* Remove underline */
        border-radius: 4px;                /* Rounded corners */
        transition: all 0.3s;              /* Smooth hover effect */
            }
            .btn-download:hover {
        background-color: #28a745 !important; /* Green background on hover */
        color: white !important;
            }


        .wide-column {
            min-width: 80px;
        }

        input[type="file"] {
            display: none;
        }

        .custom-file-upload {
            border: 1px solid black;
            display: inline-block;
            padding: 6px 12px;
            cursor: pointer;
            background-color: #ffbd5e;
            border-radius: 20px
        }

        .modal {
            display: none;
            position: fixed;
            z-index: 1;
            left: 0;
            top: 0;
            width: 100%;
            height: 100%;
            overflow: auto;
            background-color: rgba(0, 0, 0, 0.4);
        }
    </style>


    <form runat="server" enctype="multipart/form-data">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

                <asp:ScriptManager ID="ScriptManager1" runat="server" />
                <asp:UpdateProgress runat="server" ID="UpdateProgress" class="gettext" AssociatedUpdatePanelID="UpdatePanel1">
                    <ProgressTemplate>
                        <div class="loading">
                            <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <div>
                    <div class="col-10 col-s-12" id="filter_section" runat="server">

                        <%if (GLOBAL_VAR.GLOBAL.CampaignReport == true)
                            { %>
                        <div class="col-3 col-s-12">
                            <div class="col-4 col-s-2">
                                <label class="labeltext">Campaign.      </label>
                            </div>
                            <div class="col-5 col-s-3">
                                <asp:DropDownList ID="ddlCampaignList" runat="server" class="dropdownlist" Style="font-size: 12px"></asp:DropDownList>
                            </div>
                        </div>
                        <%} %>

                        <div class="col-3 col-s-12">
                            <div class="col-4 col-s-2">
                                <label class="labeltext">Status.      </label>
                            </div>
                            <div class="col-5 col-s-3">
                                <asp:DropDownList ID="ddlStatus" runat="server" class="dropdownlist" Style="font-size: 12px"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-6 col-s-12">
                            <div class="col-4 col-s-3">
                                <label class="labeltext required">Customer Account:      </label>
                            </div>

                            <div class="col-5 col-s-3">
                                <asp:TextBox ID="txtCustomerAccount" class="inputtext" runat="server" Text=""></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-10 col-s-12" id="dateRange_section" runat="server">
                            <div class="col-2 col-s-2">
                                <asp:Label ID="lbldate" runat="server" CssClass="labeltext" Text="Submitted Date Range: "></asp:Label>
                            </div>
                            <div class="col-3 col-s-6">

                                <div class="">
                                    <div class="form-group">
                                        <div class="input-group date" id="txtFromDateTimePicker">
                                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control newformdatepicker" placeholder="Select Date and Time" Style="font-size: 11px"></asp:TextBox>
                                            <span class="input-group-addon" style="display: flex; justify-content: center">
                                                <span class="glyphicon glyphicon-calendar glyph-new"></span>

                                            </span>
                                            <p style="display: flex; align-items: center; font-size: 15px">&nbsp;- </p>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-3 col-s-6">

                                <div class="">
                                    <div class="form-group">
                                        <div class="input-group date" id="txtToDateTimePicker">
                                            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control newformdatepicker" placeholder="Select Date and Time" Style="font-size: 11px"></asp:TextBox>
                                            <span class="input-group-addon" style="display: flex; justify-content: center">
                                                <span class="glyphicon glyphicon-calendar glyph-new"></span>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-10 col-s-12" id="MonthDateWrapper" runat="server" visible="false">
                        <div class="col-2 col-s-2">
                            <asp:Label ID="Label1" runat="server" CssClass="labeltext" Text="Campaign Start Date: "></asp:Label>
                        </div>
                        <div class="col-3 col-s-6">

                            <div class="">
                                <div class="form-group">
                                    <div class="input-group date" id="txtMonthDatePicker">
                                        <asp:TextBox ID="txtMonthDate" runat="server" CssClass="form-control submitdatemonthpicker" placeholder="Select Date and Time" Style="font-size: 11px"></asp:TextBox>
                                        <span class="input-group-addon" style="display: flex; justify-content: center">
                                            <span class="glyphicon glyphicon-calendar glyph-submit"></span>

                                        </span>
                                        <p style="display: flex; align-items: center; font-size: 15px">&nbsp;</p>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-6 col-s-12">
                            <div class="col-4 col-s-4">
                                <asp:Label ID="Label12" runat="server" Text="Status:" CssClass="labeltext"></asp:Label>
                            </div>

                            <div class="col-4 col-s-4">
                                <asp:DropDownList ID="ddlDownloadStatus" runat="server" CssClass="dropdownlist">
                                    <asp:ListItem Text="All" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Not Returned Form Yet" Value="1"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>



                    <div class="col-12 mt-2 mb-2">
                        <asp:Button ID="BtnFilter" runat="server" OnClick="FilterRecords" Text="Generate" class="glow_green button-margin" Enabled="true" />
                    </div>

                    <div class="col-12 col-s-12" id="export" runat="server" visible="true">
                        <div class="image_properties">
                            <asp:ImageButton ID="imgbtnExport" class="image_nohighlight" runat="server" ImageUrl="~/RESOURCES/icons8-xls-export-50.png" Visible="false" />
                            <asp:ImageButton ID="ImageButton2" class="image_highlight" runat="server" ImageUrl="~/RESOURCES/hover-xls-export-50.png" OnClick="btnExport_Click" CausesValidation="false" />
                        </div>
                    </div>

                    <asp:Panel runat="server" ID="InformationWrapper">

                        <div id="DocumentRecordsSection" runat="server" style="max-width: 100%; overflow: auto; max-height: 110%; min-width: 100%">
                            <asp:GridView ID="gvDocumentRecords" runat="server"
                                PageSize="20" HorizontalAlign="Left" CssClass="mydatagrid"
                                HeaderStyle-CssClass="header" RowStyle-CssClass="rows" DataKeyNames="No."
                                OnPageIndexChanging="datagrid_PageIndexChanging_DocumentRecords" AllowCustomPaging="false"
                                HtmlEncode="False" Style="overflow: hidden;" AutoGenerateColumns="False" AllowPaging="false" OnRowDataBound="gvDocumentRecords_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="No.">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <div class="col-12 col-s-12">
                                                <div class="col" runat="server" id="lblUpload">
                                                    <label for="file<%# Eval("ID") %>" class="custom-file-upload">
                                                        Upload Document
                                                    </label>
                                                    <input accept="image/*,.pdf" type="file" data-docid="<%# Eval("ID") %>" data-fileno="<%# Eval("FileID") %>" id="file<%# Eval("ID") %>" name="file<%# Eval("ID") %>" data-campaignid="<%# Eval("CampaignID")%>" data-custacc="<%# Eval("CustomerAccount") %>" />
                                                </div>
                                                <div class="col" runat="server" id="ActionWrapper">
                                                    <asp:Button ID="Button_Approve" runat="server" Text="Approve" CssClass="button_grid btn btn-approve mb-1" CommandArgument='<%# Eval("ID") %>' OnClick="ActionButton" />
                                                    <asp:Button ID="Button_Reject" runat="server" Text="Reject" CssClass="button_grid btn btn-reject mb-1" CommandArgument='<%# Eval("ID") %>' OnClick="ActionButton" />
                                                    <asp:Button ID="Button_Cancel" runat="server" Text="Cancel" CssClass="button_grid btn btn-cancel mb-1" CommandArgument='<%# Eval("ID") %>' OnClick="ActionButton" />
                                                </div>

                                                <div class="col" runat="server" id="DownloadWrapper">
                                                    <asp:Button ID="Button_Delete" runat="server" Text="Delete" CssClass="button_grid btn btn-reject mb-1" CommandArgument='<%# Eval("CampaignID")+ "|" + Eval("CustomerAccount")%>' OnClick="ActionButton" />
                                                    <asp:LinkButton ID="Button_Download" runat="server" Text="Download" CssClass="button_grid btn btn-download mb-1" CommandArgument='<%# Eval("CampaignID")+ "|" + Eval("CustomerAccount")%>' OnClick="ActionButton" />
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Submitted File">
                                        <ItemTemplate>
                                            <div class="col-12 col-s-12">
                                                <div class="col">
                                                    <%--<asp:FileUpload ID="FileUpload1" runat="server" />--%>
                                                    <%--<a href="<%# Eval("FileURL") %>"><%# Eval("FileName")%></a>--%>
                                                    <asp:HyperLink ID="HyperLinkView" runat="server" Style="color: blue; text-decoration: underline"
                                                        NavigateUrl='<%# /*"Handler/ImageHandler.ashx?id=" + Eval("FileID")*/ Eval("FileURL")%>'
                                                        Text='<%#Eval("FileName") %>' Target="_blank" />
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Ori Receive">
                                        <ItemTemplate>
                                            <div class="col-12 col-s-12">
                                                <div class="col" runat="server" id="lblOriRec">
                                                    <input type="checkbox" data-docid="<%# Eval("ID") %>" <%# Eval("IsOriReceive").ToString() == "True" ? "checked=\"checked\"" : ""%>>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%--<asp:BoundField DataField="ID" HeaderText="ID" />--%>
                                    <asp:BoundField DataField="CampaignID" HeaderText="Campaign ID" />
                                    <%--<asp:BoundField DataField="SequenceNo" HeaderText="Sequence No" />--%>
                                    <asp:BoundField DataField="CustomerAccount" HeaderText="Customer Account" />
                                    <asp:BoundField DataField="WorkshopName" HeaderText="Workshop Name" />
                                    <asp:BoundField DataField="Status" HeaderText="Status" />
                                    <asp:BoundField DataField="Salesman" HeaderText="Salesman" />
                                    <asp:BoundField DataField="Target" HeaderText="Target" />
                                    <asp:BoundField DataField="RejectRemarks" HeaderText="Rejected Remarks" />

                                    <asp:BoundField DataField="CampaignStartDate" HeaderText="Start Date" ItemStyle-CssClass="wide-column" HeaderStyle-CssClass="wide-column" />
                                    <asp:BoundField DataField="CampaignEndDate" HeaderText="End Date" ItemStyle-CssClass="wide-column" HeaderStyle-CssClass="wide-column" />
                                    <asp:BoundField DataField="PastyearSales" HeaderText="Past Year Sales" />
                                    <asp:BoundField DataField="DocumentRecDate" HeaderText="Document Receive Date" />
                                    <asp:BoundField DataField="CreatedDateTime" HeaderText="Created Date Time" ItemStyle-CssClass="wide-column" HeaderStyle-CssClass="wide-column" />
                                    <%--<asp:BoundField DataField="CreatedBy" HeaderText="Created By" />--%>
                                    <asp:BoundField DataField="UpdatedDateTime" HeaderText="Updated Date Time" />
                                    <asp:BoundField DataField="UpdatedBy" HeaderText="Updated By" />
                                    <%--<asp:BoundField DataField="FileID" HeaderText="File ID" />--%>
                                    <%--<asp:BoundField DataField="FileName" HeaderText="File Name" />--%>
                                </Columns>
                            </asp:GridView>
                            <asp:Label ID="AppointmentHistoryLabel" runat="server" CssClass="labeltext" Visible="false" Style="color: red">No records found!</asp:Label>
                        </div>

                    </asp:Panel>
                </div>

                <div id="myModal" class="modal except" tabindex="-1" role="dialog" style="align-content: center">
                    <div class="modal-dialog modal-xl" role="document">
                        <div class="modal-content">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <div class="modal-body" style="align-content: center; padding: 10px 30px 0px 30px; font-size: 12px; overflow: auto">
                                        <span></span>
                                        <div id="Div3" runat="server">
                                            <!--==============================================================================-->

                                            <h2>Reject VPPP Campaign</h2>
                                            <p>Please provide a reason for reject:</p>
                                            <asp:HiddenField runat="server" ID="RecordID" />
                                            <asp:TextBox ID="txtRejectReason" runat="server" TextMode="MultiLine" Text=""></asp:TextBox>
                                            <br />
                                            <br />
                                        </div>
                                    </div>
                                </ContentTemplate>

                            </asp:UpdatePanel>
                            <div class="modal-footer" style="display: flex; justify-content: center">
                                <div class="row" style="display: flex; justify-content: center;">
                                    <div>

                                        <button id="BtnCloseModal" type="button" class="glow_green">Close</button>

                                        <asp:Button ID="btnReject" runat="server" Text="OK" OnClick="ActionButton" CssClass="glow_green" />
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>


            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="ImageButton2" />
                <%--<asp:PostBackTrigger ControlID="Button_report_section" />--%>
            </Triggers>
        </asp:UpdatePanel>
    </form>
   
     <script type="text/javascript">
         $(document).ready(function () {
             $(document).on('change', '[type="file"]', function () {
                 var fileUpload = $(this).get(0);

                 // Step 1: Log files array
                 console.log('Files array:', fileUpload.files);
                 if (!fileUpload.files || fileUpload.files.length === 0) {
                     alert('No file detected! Make sure you are selecting a file and the file input is visible/clickable.');
                     return;
                 }

                 var files = fileUpload.files[0];
                 console.log('File selected:', files.name, 'Size:', files.size, 'Type:', files.type);
                 var productionWebsite = '<%= GLOBAL_VAR.GLOBAL.ProductionWebsite %>';
             var user = <%= "'" + GLOBAL_VAR.GLOBAL.user_id + "'"%>;
             var fullName = <%= "'" + GLOBAL_VAR.GLOBAL.logined_user_name + "'"%>;
             console.log("asdfghjk");
             debugger;
             var fileUpload = $(this).get(0);
             if (fileUpload.files.length > 0) {
                 var maxSizeInMB = 20;  // Set your max size in MB
                 var maxSizeInBytes = maxSizeInMB * 1024 * 1024;
                 var files = fileUpload.files[0]

                 if (files.size < maxSizeInBytes) {

                     //if ($(this).data("campaigndate") == "") {
                     //    var CampaignStartDate = prompt("Campaign Start Date (Format: 24/10/2024):");

                     //    var CampaignEndDate = prompt("Campaign End Date (Format: 24/10/2024):");
                     //}
                     var formData = new FormData();

                     formData.append('File', files);
                     formData.append('FileNo', $(this).data('fileno'));
                     formData.append('DocID', $(this).data('docid'));
                     formData.append('UserName', user);
                     formData.append('CampaignID', $(this).data('campaignid'));
                     formData.append('CustAcc', $(this).data('custacc'));
                     formData.append('FullName', fullName);

                     debugger;
                     console.log("Before AX");
                     // Step 2: Log FormData contents
                     for (var pair of formData.entries()) {
                         console.log(pair[0] + ':', pair[1]);
                     }
                     $.ajax({
                         type: "POST",
                         url: productionWebsite + "api/API/UploadFile",
                         data: formData,
                         processData: false,
                         contentType: false,
                         success: function (data) {
                             if (data == "") {
                                 if (!alert("Submitted.")) {
                                     window.location.reload();
                                 }
                             } else {
                                 alert(data)
                             }
                         },
                         error: function (xhr, textStatus, errorThrown) {
                             console.log('Error in Operation' + "," + xhr + "," + textStatus + "," + errorThrown);
                             console.log(xhr)
                         }
                     });

                 }
                 else {
                     alert("File size exceeds " + maxSizeInMB + " MB limit.")
                 }
             }
         });

         $(document).on('change', '[type="checkbox"]', function () {
             var productionWebsite = '<%= GLOBAL_VAR.GLOBAL.ProductionWebsite %>';

             var formData = new FormData();
             var isOriRec = $(this).is(':checked');
             formData.append('DocID', $(this).data('docid'));
             formData.append('IsOriReceive', isOriRec);

             $.ajax({
                 type: "POST",
                 url: productionWebsite + "api/API/UpdateOriReceive",
                 data: formData,
                 processData: false,
                 contentType: false,
                 success: function (data) {
                     if (data == "") {
                         if (!alert("Updated.")) {
                         }
                     } else {
                         alert(data)
                     }
                 },
                 error: function (xhr, textStatus, errorThrown) {
                     console.log('Error in Operation' + "," + xhr + "," + textStatus + "," + errorThrown);
                     console.log(xhr)
                 }
             });

         });


         $('#BtnCloseModal').on('click', function () {
             $('#myModal').hide();
         })

     });

     </script>

</asp:Content>
