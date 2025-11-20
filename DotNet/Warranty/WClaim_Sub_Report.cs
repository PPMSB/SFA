using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DotNet
{
    public partial class WClaim : Page
    {
        public void JobDaysTakenReport(int PAGE_INDEX)
        {
            gvJobsTaken.DataSource = null;
            Axapta DynAx = Function_Method.GlobalAxapta();

            try
            {
                int LF_WarrantyTable = 30661; //live
                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", LF_WarrantyTable);

                string fromDate = Request.Form[txtFromDate.UniqueID];
                string toDate = Request.Form[txtToDate.UniqueID];

                var qbr1 = (AxaptaObject)axQueryDataSource.Call("addRange", 30003);//ClaimStatus
                string claimtype = "";
                if (ddlWarranty.SelectedIndex.ToString() == "1")
                {
                    claimtype = "Batch";
                    qbr1.Call("value", "*" + claimtype + "*");
                }
                else if (ddlWarranty.SelectedIndex.ToString() == "2")
                {
                    claimtype = "Warranty";
                    qbr1.Call("value", "*" + claimtype + "*");
                }

                axQueryDataSource.Call("addSortField", 30001, 0);//asc
                AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

                //===========================================
                DataTable dt = new DataTable();
                int data_count = 23;
                string[] N = new string[data_count];
                N[0] = "No."; N[1] = "Form No."; N[2] = "Salesman Code"; N[3] = "Customer"; N[4] = "Type";
                N[5] = "Claim Type"; N[6] = "WH"; N[7] = "Battery Request"; N[8] = "Serial No."; N[9] = "Transport Arrange"; N[10] = "Receive";
                N[11] = "Invoice Check"; N[12] = "Inspect/Recharge"; N[13] = "Verify"; N[14] = "Approval"; N[15] = "RMA";
                N[16] = "CNCC"; N[17] = "Total"; N[18] = "Status"; N[19] = "Item"; N[20] = "Reject Remark"; N[21] = "Approve Remark";
                N[22] = "Submitted Date";

                for (int i = 0; i < data_count; i++)
                {
                    dt.Columns.Add(new DataColumn(N[i], typeof(string)));
                }
                //===========================================
                DataRow row;
                int countA = 0;
                //===========================================
                // Loop through the set of retrieved records.

                while ((bool)axQueryRun.Call("next"))
                {
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", LF_WarrantyTable);

                    int TotalReceivedDt = 0;
                    //row["No."] = countA;
                    string submittedDt = Convert.ToDateTime(DynRec.get_Field("createdDateTime")).ToString("dd/MM/yyyy");
                    if (Convert.ToDateTime(submittedDt) >= Convert.ToDateTime(fromDate) &&
                        Convert.ToDateTime(submittedDt) <= Convert.ToDateTime(toDate))
                    {

                        var item = WClaim_GET_Report.getItemList(DynAx, DynRec.get_Field("ClaimNumber").ToString());
                        for (int i = 0; i < item.Count; i++)
                        {
                            row = dt.NewRow();

                            countA = countA + 1;
                            //if (countA >= startA && countA <= endA)
                            //{
                            string ClaimNum = DynRec.get_Field("ClaimNumber").ToString();
                            row["Form No."] = ClaimNum;

                            var getSalesmanId = SFA_GET_Enquiries_BatteryOutstanding.getEmplId(DynAx, DynRec.get_Field("CustAccount").ToString());
                            row["Salesman Code"] = getSalesmanId.Item1;

                            string CustomerName = SFA_GET_Enquiries_BatteryOutstanding.getCust(DynAx, DynRec.get_Field("CustAccount").ToString());
                            row["Customer"] = CustomerName;

                            row["Type"] = DynRec.get_Field("ClaimType").ToString();
                            string subclaimtype = "";
                            if (DynRec.get_Field("SubClaimType").ToString() == "1")
                            {
                                subclaimtype = "Battery";
                            }
                            else if (DynRec.get_Field("SubClaimType").ToString() == "2")
                            {
                                subclaimtype = "Lubricant";
                            }
                            else
                            {
                                subclaimtype = "Others";
                            }
                            row["Claim Type"] = subclaimtype;
                            row["WH"] = DynRec.get_Field("InventLocationId").ToString();
                            row["Battery Request"] = "-";
                            var getSerialNumber = WClaim_GET_Report.getItem(DynAx, ClaimNum);
                            row["Serial No."] = getSerialNumber.Item4;
                            DateTime createdDt = Convert.ToDateTime(DynRec.get_Field("createdDateTime"));
                            string invoiceChkDt = DynRec.get_Field("InvoiceCheckedDate").ToString();

                            string dtArrange = DynRec.get_Field("TransArrangeDate").ToString();
                            string goodReceiveDt = DynRec.get_Field("GoodsReceivedDate").ToString();
                            DateTime dtInspectDt = new DateTime();
                            string inspectDt = DynRec.get_Field("InspectionDate").ToString();
                            if (!string.IsNullOrEmpty(inspectDt))
                            {
                                dtInspectDt = Convert.ToDateTime(inspectDt);
                            }
                            int TotalInspectDt = 0;

                            string verifyDt = DynRec.get_Field("VerifiedDate").ToString();
                            string approvedDt = DynRec.get_Field("ApprovedDate").ToString();
                            string rejectReason = DynRec.get_Field("RejectReason").ToString();
                            string approveReason = DynRec.get_Field("ApprovalRemark").ToString();

                            if (!string.IsNullOrEmpty(invoiceChkDt))
                            {
                                DateTime dtInvoiceChecked = Convert.ToDateTime(invoiceChkDt);
                                int TotalInvoiceCheckedDt = GetBusinessDays(createdDt, dtInvoiceChecked);
                                row["Invoice Check"] = TotalInvoiceCheckedDt;
                                if (!string.IsNullOrEmpty(goodReceiveDt))
                                {
                                    DateTime dtReceive = Convert.ToDateTime(goodReceiveDt);
                                    if (!string.IsNullOrEmpty(dtArrange))
                                    {
                                        int transArrangeDays = GetBusinessDays(createdDt, Convert.ToDateTime(dtArrange));
                                        row["Transport Arrange"] = transArrangeDays;

                                        TotalReceivedDt = GetBusinessDays(Convert.ToDateTime(dtArrange), dtReceive);
                                        row["Receive"] = TotalReceivedDt;
                                    }
                                    else
                                    {
                                        TotalReceivedDt = GetBusinessDays(dtInvoiceChecked, dtReceive);
                                        row["Receive"] = TotalReceivedDt;
                                    }

                                    if (!string.IsNullOrEmpty(inspectDt))
                                    {
                                        TotalInspectDt = GetBusinessDays(dtReceive, dtInspectDt);
                                        row["Inspect/Recharge"] = TotalInspectDt;
                                    }
                                }

                                if (!string.IsNullOrEmpty(verifyDt))
                                {
                                    DateTime dtVerifyDt = Convert.ToDateTime(verifyDt);
                                    int TotalVerifyDt = GetBusinessDays(dtInspectDt, dtVerifyDt);
                                    row["Verify"] = TotalVerifyDt;

                                    if (!string.IsNullOrEmpty(approvedDt))
                                    {
                                        DateTime dtApprovalDt = Convert.ToDateTime(approvedDt);
                                        int TotalApprovedDt = GetBusinessDays(dtVerifyDt, dtApprovalDt);
                                        row["Approval"] = TotalApprovedDt;

                                        DateTime? rmaDate = DynRec.get_Field("SOCreatedDate") as DateTime?;
                                        if (rmaDate.HasValue)
                                        {
                                            int totdalRMAdate = GetBusinessDays(dtApprovalDt, rmaDate.Value);
                                            row["RMA"] = totdalRMAdate;
                                        }
                                        else
                                        {
                                            row["RMA"] = "-";
                                        }
                                        var getCNCreatedDt = WClaim_GET_Report.get_SalesId(DynAx, DynRec.get_Field("RMAID").ToString());
                                        if (!string.IsNullOrEmpty(getCNCreatedDt.Item1))
                                        {
                                            if (!string.IsNullOrEmpty(rmaDate.ToString()))
                                            {
                                                int totalCnDays = GetBusinessDays(Convert.ToDateTime(rmaDate.ToString()), Convert.ToDateTime(getCNCreatedDt.Item2.ToString()));
                                                row["CNCC"] = totalCnDays;
                                            }
                                        }
                                        else
                                        {
                                            row["CNCC"] = "-";
                                        }

                                        double totalDays = TotalReceivedDt + TotalInvoiceCheckedDt + TotalInspectDt + TotalVerifyDt + TotalApprovedDt;
                                        if (totalDays == 0)//at least one day to proccess all the flow 
                                        {
                                            totalDays = 1;
                                        }
                                        row["Total"] = totalDays;
                                    }
                                }
                            }

                            if (DynRec.get_Field("TransportRequired").ToString() == "1")
                            {
                                row["Transport Arrange"] = "Yes";
                            }
                            else
                            {
                                row["Transport Arrange"] = "No";
                            }

                            row["Status"] = DynRec.get_Field("ClaimStatus").ToString();

                            row["Item"] = item[i];
                            row["Reject Remark"] = rejectReason;
                            row["Approve Remark"] = approveReason;
                            //Neil Changes for Submitted Date Time Format
                            string createdDT = DynRec.get_Field("createdDateTime").ToString();
                            DateTime parsedDateTime = DateTime.Parse(createdDT).AddHours(8);
                            if (parsedDateTime.Hour >= 12)
                            {
                                parsedDateTime = parsedDateTime.AddHours(-12);
                                // Set "PM"
                                string amOrPm = "PM";
                                // If the resultTime is exactly 12:00 PM, it should stay "PM"
                                if (parsedDateTime.Hour == 0)
                                {
                                    amOrPm = "AM";
                                }
                                string formattedResultTime = parsedDateTime.ToString("dd/M/yyyy h:mm:ss tt").Replace("AM", amOrPm).Replace("PM", amOrPm);
                                row["Submitted Date"] = formattedResultTime;
                            }
                            else
                            {
                                row["Submitted Date"] = parsedDateTime;
                            }
                            dt.Rows.Add(row);
                        }
                    }

                    // Advance to the next row.
                    DynRec.Dispose();
                }
                //Data-Binding with our GRID
                gvJobsTaken.DataSource = dt;
                gvJobsTaken.DataBind();
            }
            catch (Exception ER_WC_05)
            {
                Function_Method.MsgBox("ER_WC_05: " + ER_WC_05.Message, this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        /// <summary>
        //Backup in case need to revert back
        /// </summary>
        //public void JobDaysTakenReport(int PAGE_INDEX)
        //{
        //    gvJobsTaken.DataSource = null;
        //    Axapta DynAx = Function_Method.GlobalAxapta();

        //    try
        //    {
        //        int LF_WarrantyTable = 30661; //live
        //        AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
        //        AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", LF_WarrantyTable);

        //        string fromDate = Request.Form[txtFromDate.UniqueID];
        //        string toDate = Request.Form[txtToDate.UniqueID];

        //        var qbr1 = (AxaptaObject)axQueryDataSource.Call("addRange", 30003);//ClaimStatus
        //        string claimtype = "";
        //        if (ddlWarranty.SelectedIndex.ToString() == "1")
        //        {
        //            claimtype = "Batch";
        //            qbr1.Call("value", "*" + claimtype + "*");
        //        }
        //        else if (ddlWarranty.SelectedIndex.ToString() == "2")
        //        {
        //            claimtype = "Warranty";
        //            qbr1.Call("value", "*" + claimtype + "*");
        //        }

        //        axQueryDataSource.Call("addSortField", 30001, 0);//asc
        //        AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

        //        //===========================================
        //        DataTable dt = new DataTable();
        //        int data_count = 22;
        //        string[] N = new string[data_count];
        //        N[0] = "No."; N[1] = "Form No."; N[2] = "Salesman Code"; N[3] = "Customer"; N[4] = "Type";
        //        N[5] = "Claim Type"; N[6] = "WH"; N[7] = "Battery Request"; N[8] = "Serial No."; N[9] = "Transport Arrange"; N[10] = "Receive";
        //        N[11] = "Invoice Check"; N[12] = "Inspect/Recharge"; N[13] = "Verify"; N[14] = "Approval"; N[15] = "RMA";
        //        N[16] = "CNCC"; N[17] = "Total"; N[18] = "Status"; N[19] = "Item"; N[20] = "Reject Remark"; N[21] = "Approve Remark";

        //        for (int i = 0; i < data_count; i++)
        //        {
        //            dt.Columns.Add(new DataColumn(N[i], typeof(string)));
        //        }
        //        //===========================================
        //        DataRow row;
        //        int countA = 0;
        //        //===========================================
        //        // Loop through the set of retrieved records.

        //        while ((bool)axQueryRun.Call("next"))
        //        {
        //            AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", LF_WarrantyTable);

        //            countA = countA + 1;
        //            int TotalReceivedDt = 0;
        //            if (countA > 0)
        //            {
        //                row = dt.NewRow();
        //                //row["No."] = countA;
        //                string submittedDt = Convert.ToDateTime(DynRec.get_Field("createdDateTime")).ToString("dd/MM/yyyy");
        //                if (Convert.ToDateTime(submittedDt) >= Convert.ToDateTime(fromDate) &&
        //                    Convert.ToDateTime(submittedDt) <= Convert.ToDateTime(toDate))
        //                {
        //                    string ClaimNum = DynRec.get_Field("ClaimNumber").ToString();
        //                    row["Form No."] = ClaimNum;

        //                    var getSalesmanId = SFA_GET_Enquiries_BatteryOutstanding.getEmplId(DynAx, DynRec.get_Field("CustAccount").ToString());
        //                    row["Salesman Code"] = getSalesmanId.Item1;

        //                    string CustomerName = SFA_GET_Enquiries_BatteryOutstanding.getCust(DynAx, DynRec.get_Field("CustAccount").ToString());
        //                    row["Customer"] = CustomerName;

        //                    row["Type"] = DynRec.get_Field("ClaimType").ToString();
        //                    string subclaimtype = "";
        //                    if (DynRec.get_Field("SubClaimType").ToString() == "1")
        //                    {
        //                        subclaimtype = "Battery";
        //                    }
        //                    else if (DynRec.get_Field("SubClaimType").ToString() == "2")
        //                    {
        //                        subclaimtype = "Lubricant";
        //                    }
        //                    else
        //                    {
        //                        subclaimtype = "Others";
        //                    }
        //                    row["Claim Type"] = subclaimtype;
        //                    row["WH"] = DynRec.get_Field("InventLocationId").ToString();
        //                    row["Battery Request"] = "-";
        //                    var getSerialNumber = WClaim_GET_Report.getItem(DynAx, ClaimNum);
        //                    row["Serial No."] = getSerialNumber.Item4;
        //                    DateTime createdDt = Convert.ToDateTime(DynRec.get_Field("createdDateTime"));
        //                    string invoiceChkDt = DynRec.get_Field("InvoiceCheckedDate").ToString();

        //                    string dtArrange = DynRec.get_Field("TransArrangeDate").ToString();
        //                    string goodReceiveDt = DynRec.get_Field("GoodsReceivedDate").ToString();
        //                    DateTime dtInspectDt = new DateTime();
        //                    string inspectDt = DynRec.get_Field("InspectionDate").ToString();
        //                    if (!string.IsNullOrEmpty(inspectDt))
        //                    {
        //                        dtInspectDt = Convert.ToDateTime(inspectDt);
        //                    }
        //                    int TotalInspectDt = 0;

        //                    string verifyDt = DynRec.get_Field("VerifiedDate").ToString();
        //                    string approvedDt = DynRec.get_Field("ApprovedDate").ToString();
        //                    string rejectReason = DynRec.get_Field("RejectReason").ToString();
        //                    string approveReason = DynRec.get_Field("ApprovalRemark").ToString();

        //                    if (!string.IsNullOrEmpty(invoiceChkDt))
        //                    {
        //                        DateTime dtInvoiceChecked = Convert.ToDateTime(invoiceChkDt);
        //                        int TotalInvoiceCheckedDt = GetBusinessDays(createdDt, dtInvoiceChecked);
        //                        row["Invoice Check"] = TotalInvoiceCheckedDt;
        //                        if (!string.IsNullOrEmpty(goodReceiveDt))
        //                        {
        //                            DateTime dtReceive = Convert.ToDateTime(goodReceiveDt);
        //                            if (!string.IsNullOrEmpty(dtArrange))
        //                            {
        //                                int transArrangeDays = GetBusinessDays(createdDt, Convert.ToDateTime(dtArrange));
        //                                row["Transport Arrange"] = transArrangeDays;

        //                                TotalReceivedDt = GetBusinessDays(Convert.ToDateTime(dtArrange), dtReceive);
        //                                row["Receive"] = TotalReceivedDt;
        //                            }
        //                            else
        //                            {
        //                                TotalReceivedDt = GetBusinessDays(dtInvoiceChecked, dtReceive);
        //                                row["Receive"] = TotalReceivedDt;
        //                            }

        //                            if (!string.IsNullOrEmpty(inspectDt))
        //                            {
        //                                TotalInspectDt = GetBusinessDays(dtReceive, dtInspectDt);
        //                                row["Inspect/Recharge"] = TotalInspectDt;
        //                            }
        //                        }

        //                        if (!string.IsNullOrEmpty(verifyDt))
        //                        {
        //                            DateTime dtVerifyDt = Convert.ToDateTime(verifyDt);
        //                            int TotalVerifyDt = GetBusinessDays(dtInspectDt, dtVerifyDt);
        //                            row["Verify"] = TotalVerifyDt;

        //                            if (!string.IsNullOrEmpty(approvedDt))
        //                            {
        //                                DateTime dtApprovalDt = Convert.ToDateTime(approvedDt);
        //                                int TotalApprovedDt = GetBusinessDays(dtVerifyDt, dtApprovalDt);
        //                                row["Approval"] = TotalApprovedDt;

        //                                DateTime? rmaDate = DynRec.get_Field("SOCreatedDate") as DateTime?;
        //                                if (rmaDate.HasValue)
        //                                {
        //                                    int totdalRMAdate = GetBusinessDays(dtApprovalDt, rmaDate.Value);
        //                                    row["RMA"] = totdalRMAdate;
        //                                }
        //                                else
        //                                {
        //                                    row["RMA"] = "-";
        //                                }
        //                                var getCNCreatedDt = WClaim_GET_Report.get_SalesId(DynAx, DynRec.get_Field("RMAID").ToString());
        //                                if (!string.IsNullOrEmpty(getCNCreatedDt.Item1))
        //                                {
        //                                    if (!string.IsNullOrEmpty(rmaDate.ToString()))
        //                                    {
        //                                        int totalCnDays = GetBusinessDays(Convert.ToDateTime(rmaDate.ToString()), Convert.ToDateTime(getCNCreatedDt.Item2.ToString()));
        //                                        row["CNCC"] = totalCnDays;
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    row["CNCC"] = "-";
        //                                }

        //                                double totalDays = TotalReceivedDt + TotalInvoiceCheckedDt + TotalInspectDt + TotalVerifyDt + TotalApprovedDt;
        //                                if (totalDays == 0)//at least one day to proccess all the flow 
        //                                {
        //                                    totalDays = 1;
        //                                }
        //                                row["Total"] = totalDays;
        //                            }
        //                        }
        //                    }

        //                    if (DynRec.get_Field("TransportRequired").ToString() == "1")
        //                    {
        //                        row["Transport Arrange"] = "Yes";
        //                    }
        //                    else
        //                    {
        //                        row["Transport Arrange"] = "No";
        //                    }

        //                    row["Status"] = DynRec.get_Field("ClaimStatus").ToString();

        //                    var item = WClaim_GET_Report.getItem(DynAx, DynRec.get_Field("ClaimNumber").ToString());
        //                    row["Item"] = item.Item1;
        //                    row["Reject Remark"] = rejectReason;
        //                    row["Approve Remark"] = approveReason;
        //                    dt.Rows.Add(row);
        //                }

        //                // Advance to the next row.
        //                DynRec.Dispose();
        //            }
        //        }
        //        //Data-Binding with our GRID
        //        gvJobsTaken.DataSource = dt;
        //        gvJobsTaken.DataBind();
        //    }
        //    catch (Exception ER_WC_05)
        //    {
        //        Function_Method.MsgBox("ER_WC_05: " + ER_WC_05.Message, this.Page, this);
        //    }
        //    finally
        //    {
        //        DynAx.Logoff();
        //    }
        //}


        public void BatteryQueryReport()
        {
            gvBatteryReport.DataSource = null;
            gvBatteryReport.DataBind();
            Axapta DynAx = Function_Method.GlobalAxapta();
            try
            {
                //int LF_WarrantyTable = 0;
                //if (GLOBAL.debug)
                //{
                //    LF_WarrantyTable = 50773;
                //}
                //else
                //{
                int LF_WarrantyTable = 30661; //live
                //}
                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", LF_WarrantyTable);

                string fromDate = Request.Form[txtFromDate.UniqueID];
                string toDate = Request.Form[txtToDate.UniqueID];

                //string[] split = fromDate.Substring(3).Split('/');
                //string startDate = split[1] + split[0];

                //string[] split2 = toDate.Substring(3).Split('/');
                //int add1 = Convert.ToInt16(split2[0]) + 1;
                //string endDate = split[1] + add1;

                var qbr6 = (AxaptaObject)axQueryDataSource.Call("addRange", 30003);//claimtype
                qbr6.Call("value", "Battery");

                var qbr5 = (AxaptaObject)axQueryDataSource.Call("addRange", 30006);//claimstatus
                qbr5.Call("value", "Approved, Rejected");

                var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", 30013);//approvedclaimdate
                qbr.Call("value", fromDate + ".." + toDate + "*");

                axQueryDataSource.Call("addSortField", 30001, 0);//asc
                AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

                //===========================================
                DataTable dt = new DataTable();
                int data_count = 19;
                string[] N = new string[data_count];
                N[0] = "No."; N[1] = "Form No."; N[2] = "Salesman"; N[3] = "Customer"; N[4] = "SN"; N[5] = "Battery Request";
                N[6] = "Transport Arrange"; N[7] = "Receive"; N[8] = "Invoice Check"; N[9] = "Inspect/Recharge"; N[10] = "Verify";
                N[11] = "Approval"; N[12] = "RMA"; N[13] = "CNSA"; N[14] = "CNCC"; N[15] = "Total"; N[16] = "AOI"; N[17] = "Inspection Remark";
                N[18] = "Submitted Date";
                for (int i = 0; i < data_count; i++)
                {
                    dt.Columns.Add(new DataColumn(N[i], typeof(string)));
                }
                //===========================================
                DataRow row;
                int countA = 0;
                //===========================================
                // Loop through the set of retrieved records.

                while ((bool)axQueryRun.Call("next"))
                {
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", LF_WarrantyTable);

                    countA = countA + 1;
                    if (countA > 0)
                    {
                        row = dt.NewRow();
                        //row["No."] = countA;
                        row["Form No."] = DynRec.get_Field("ClaimNumber").ToString();

                        var getSalesmanId = SFA_GET_Enquiries_BatteryOutstanding.getEmplId(DynAx, DynRec.get_Field("CustAccount").ToString());
                        row["Salesman"] = getSalesmanId.Item2;

                        string CustomerName = SFA_GET_Enquiries_BatteryOutstanding.getCust(DynAx, DynRec.get_Field("CustAccount").ToString());
                        row["Customer"] = CustomerName;

                        row["SN"] = SalesReport_Get_Budget.getBatterySerialNumber(DynAx, DynRec.get_Field("ClaimNumber").ToString());
                        row["Battery Request"] = "-";
                        row["Transport Arrange"] = DynRec.get_Field("TransportRequired").ToString();

                        string dtReceive = Convert.ToDateTime(DynRec.get_Field("GoodsReceivedDate")).ToString("dd/MM/yyyy");
                        string dtArrange = Convert.ToDateTime(DynRec.get_Field("TransArrangeDate")).ToString("dd/MM/yyyy");
                        int TotalReceivedDt = GetBusinessDays(Convert.ToDateTime(dtReceive), Convert.ToDateTime(dtArrange));
                        row["Receive"] = TotalReceivedDt;

                        string dtInvoiceChecked = Convert.ToDateTime(DynRec.get_Field("InvoiceCheckedDate")).ToString("dd/MM/yyyy");
                        int TotalInvoiceCheckedDt = GetBusinessDays(Convert.ToDateTime(dtInvoiceChecked), Convert.ToDateTime(dtReceive));
                        row["Invoice Check"] = TotalInvoiceCheckedDt;

                        string dtInspectDt = Convert.ToDateTime(DynRec.get_Field("InspectionDate")).ToString("dd/MM/yyyy");
                        int TotalInspectDt = GetBusinessDays(Convert.ToDateTime(dtInspectDt), Convert.ToDateTime(dtInvoiceChecked));
                        row["Inspect/Recharge"] = TotalInspectDt;

                        string dtVerifyDt = Convert.ToDateTime(DynRec.get_Field("VerifiedDate")).ToString("dd/MM/yyyy");
                        int TotalVerifyDt = GetBusinessDays(Convert.ToDateTime(dtVerifyDt), Convert.ToDateTime(dtInspectDt));
                        row["Verify"] = TotalVerifyDt;

                        string dtApprovalDt = Convert.ToDateTime(DynRec.get_Field("ApprovedDate")).ToString("dd/MM/yyyy");
                        int TotalApprovedDt = GetBusinessDays(Convert.ToDateTime(dtApprovalDt), Convert.ToDateTime(dtVerifyDt));
                        row["Approval"] = TotalApprovedDt;

                        string rmaDate = DynRec.get_Field("SOCreatedDate").ToString();
                        if (!string.IsNullOrEmpty(rmaDate))
                        {
                            int totdalRMAdate = GetBusinessDays(Convert.ToDateTime(rmaDate), Convert.ToDateTime(dtApprovalDt));
                            row["RMA"] = totdalRMAdate;
                        }
                        else
                        {
                            row["RMA"] = "-";
                        }

                        row["CNSA"] = "-";
                        row["CNCC"] = "-";

                        double totalDays = TotalReceivedDt + TotalInvoiceCheckedDt + TotalInspectDt + TotalVerifyDt + TotalApprovedDt;
                        row["Total"] = totalDays;

                        row["AOI"] = "-";
                        //Neil Changes for Inspection Remark
                        string createdDT = DynRec.get_Field("createdDateTime").ToString();
                        DateTime parsedDateTime = DateTime.Parse(createdDT).AddHours(8);
                        if (parsedDateTime.Hour >= 12)
                        {
                            parsedDateTime = parsedDateTime.AddHours(-12);
                            // Set "PM"
                            string amOrPm = "PM";
                            // If the resultTime is exactly 12:00 PM, it should stay "PM"
                            if (parsedDateTime.Hour == 0)
                            {
                                amOrPm = "AM";
                            }
                            string formattedResultTime = parsedDateTime.ToString("dd/M/yyyy h:mm:ss tt").Replace("AM", amOrPm).Replace("PM", amOrPm);
                            row["Submitted Date"] = formattedResultTime;
                        }
                        else
                        {
                            row["Submitted Date"] = parsedDateTime;
                        }
                        dt.Rows.Add(row);
                        // Advance to the next row.
                        DynRec.Dispose();
                    }
                }
                //Data-Binding with our GRID
                gvBatteryReport.DataSource = dt;
                gvBatteryReport.DataBind();
            }
            catch (Exception ER_WC_18)
            {
                Function_Method.MsgBox("ER_WC_18: " + ER_WC_18.Message, this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        public void BatteryStatisticReport(int PAGE_INDEX)
        {
            gvBatteryStatisticReport.DataSource = null;
            gvBatteryStatisticReport.DataBind();
            Axapta DynAx = Function_Method.GlobalAxapta();
            try
            {
                int InventTable = 175;
                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", InventTable);

                string fromDate = Request.Form[txtFromDate.UniqueID];
                string toDate = Request.Form[txtToDate.UniqueID];

                string[] split = toDate.Split('/');
                int add1 = Convert.ToInt16(split[1]) + 1;
                string endDate = split[0] + "/" + add1 + "/" + split[2];

                var qbr6 = (AxaptaObject)axQueryDataSource.Call("addRange", 30002);//ItemGrp2
                qbr6.Call("value", "B0010");

                //axQueryDataSource.Call("addSortField", 50002, 0);//asc
                AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

                //===========================================
                DataTable dt = new DataTable();
                int data_count = 6;
                string[] N = new string[data_count];
                N[0] = "No."; N[1] = "Item Name"; N[2] = "Total Sold"; N[3] = "Total Claim"; N[4] = "Percentage";
                N[5] = "Submitted Date";
                for (int i = 0; i < data_count; i++)
                {
                    dt.Columns.Add(new DataColumn(N[i], typeof(string)));
                }
                //===========================================
                DataRow row;

                // Loop through the set of retrieved records.

                while ((bool)axQueryRun.Call("next"))
                {
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", InventTable);

                    int totalSold = WClaim_GET_Report.getTotalSold(DynAx, DynRec.get_Field("ItemId").ToString(), fromDate, endDate);

                    var totalClaim = WClaim_GET_Report.getTotalClaim(DynAx, DynRec.get_Field("ItemId").ToString());
                    if (totalSold != 0)
                    {
                        row = dt.NewRow();
                        row["Item Name"] = DynRec.get_Field("ItemName").ToString();
                        row["Total Sold"] = Math.Abs(totalSold);

                        if (totalClaim.Item2 == 0)
                        {
                            row["Total Claim"] = "-";
                        }
                        else
                        {
                            row["Total Claim"] = totalClaim.Item2;
                        }

                        double percentage = (double)totalClaim.Item2 / totalSold * 100;
                        if (percentage == 0)
                        {
                            row["Percentage"] = "-";
                        }
                        else
                        {
                            row["Percentage"] = percentage.ToString("N2");
                        }
                        //Neil Changes for Submitted Date Time Format
                        string createdDT = DynRec.get_Field("createdDateTime").ToString();
                        DateTime parsedDateTime = DateTime.Parse(createdDT).AddHours(8);
                        if (parsedDateTime.Hour >= 12)
                        {
                            parsedDateTime = parsedDateTime.AddHours(-12);
                            // Set "PM"
                            string amOrPm = "PM";
                            // If the resultTime is exactly 12:00 PM, it should stay "PM"
                            if (parsedDateTime.Hour == 0)
                            {
                                amOrPm = "AM";
                            }
                            string formattedResultTime = parsedDateTime.ToString("dd/M/yyyy h:mm:ss tt").Replace("AM", amOrPm).Replace("PM", amOrPm);
                            row["Submitted Date"] = formattedResultTime;
                        }
                        else
                        {
                            row["Submitted Date"] = parsedDateTime;
                        }

                        dt.Rows.Add(row);
                        // Advance to the next row.
                        DynRec.Dispose();
                    }
                }

                //Data-Binding with our GRID
                gvBatteryStatisticReport.DataSource = dt;
                gvBatteryStatisticReport.DataBind();
            }
            catch (Exception ER_WC_08)
            {
                Function_Method.MsgBox("ER_WC_08: " + ER_WC_08.Message, this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        protected void ExportGridToExcel()
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";

            string FileName = "";
            GridView gv = new GridView();
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";

            if (btnTitle.Text == "Job Days Taken")
            {
                FileName = "Report Job Days Taken" + DateTime.Now + ".xls";
                gv = gvJobsTaken;
            }
            else if (btnTitle.Text == "Battery Query Report")
            {
                FileName = "Report Battery " + DateTime.Now + ".xls";
                gv = gvBatteryReport;
            }
            else if (btnTitle.Text == "Battery Statistic Report")
            {
                FileName = "Report Battery Statistic " + DateTime.Now + ".xls";
                gv = gvBatteryStatisticReport;
            }
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            gv.GridLines = GridLines.Both;
            gv.HeaderStyle.Font.Bold = true;
            gv.RenderControl(htmltextwrtter);
            Response.Write(strwritter.ToString());
            Response.Flush();
            Response.End();
        }

        public void queryReport_Overview()
        {
            gvQueryReport.DataSource = null;
            gvQueryReport.DataBind();
            // Log on to Microsoft Dynamics AX.
            Axapta DynAx = Function_Method.GlobalAxapta();
            try
            {
                int LF_WarrantyTable = 30661; //live

                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", LF_WarrantyTable);

                var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", 30003);//claim type
                qbr.Call("value", "Batch");

                var qbr1 = (AxaptaObject)axQueryDataSource.Call("addRange", 30006);//claim status
                qbr1.Call("value", "Approved, Rejected");

                axQueryDataSource.Call("addSortField", 30001, 1); // warrantynum desc
                axQueryDataSource.Call("addSortField", 30011, 1);// invoicecheckeddate desc
                AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
                //===========================================
                DataTable dt = new DataTable();
                int data_count = 11;
                string[] N = new string[data_count];
                N[0] = "No."; N[1] = "Claim Number"; N[2] = "Customer Account"; N[3] = "Customer Name";
                N[4] = "Claim Type"; N[5] = "Claim Status";
                N[6] = "Verified Date"; N[7] = "Transport Required"; N[8] = "Reject Remark"; N[9] = "Approve Remark";
                N[10] = "Submitted Date";
                for (int i = 0; i < data_count; i++)
                {
                    dt.Columns.Add(new DataColumn(N[i], typeof(string)));
                }
                //===========================================
                DataRow row;
                int countA = 0;

                //===========================================
                // Loop through the set of retrieved records.

                while ((bool)axQueryRun.Call("next"))
                {
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", LF_WarrantyTable);
                    string temp_DraftCN = DynRec.get_Field("ClaimStatus").ToString();
                    countA = countA + 1;

                    row = dt.NewRow();

                    row["Claim Number"] = DynRec.get_Field("ClaimNumber").ToString();
                    string temp_CustAcc = DynRec.get_Field("CUSTACCOUNT").ToString();
                    row["Customer Account"] = temp_CustAcc;

                    var tuple_getCustInfo = EOR_GET_NewApplicant.getCustInfo(DynAx, temp_CustAcc);
                    string CustName = tuple_getCustInfo.Item1;
                    row["Customer Name"] = CustName;
                    row["Claim Type"] = DynRec.get_Field("ClaimType").ToString();
                    row["Claim Status"] = DynRec.get_Field("ClaimStatus").ToString();
                    row["Verified Date"] = DynRec.get_Field("VerifiedDate").ToString();
                    row["Reject Remark"] = DynRec.get_Field("RejectReason").ToString();
                    row["Approve Remark"] = DynRec.get_Field("ApprovalRemark").ToString();

                    string temp_Trans_Required = DynRec.get_Field("TransportRequired").ToString();
                    if (temp_Trans_Required == "1")//need transport
                    {
                        row["Transport Required"] = "Yes";
                    }
                    else
                    {
                        row["Transport Required"] = "No";
                    }
                    //Neil Changes for Submitted Date Time Format
                    string createdDT = DynRec.get_Field("createdDateTime").ToString();
                    DateTime parsedDateTime = DateTime.Parse(createdDT).AddHours(8);
                    if (parsedDateTime.Hour >= 12)
                    {
                        parsedDateTime = parsedDateTime.AddHours(-12);
                        // Set "PM"
                        string amOrPm = "PM";
                        // If the resultTime is exactly 12:00 PM, it should stay "PM"
                        if (parsedDateTime.Hour == 0)
                        {
                            amOrPm = "AM";
                        }
                        string formattedResultTime = parsedDateTime.ToString("dd/M/yyyy h:mm:ss tt").Replace("AM", amOrPm).Replace("PM", amOrPm);
                        row["Submitted Date"] = formattedResultTime;
                    }
                    else
                    {
                        row["Submitted Date"] = parsedDateTime;
                    }
                    dt.Rows.Add(row);
                    // Advance to the next row.
                    DynRec.Dispose();
                }

                // Log off from Microsoft Dynamics AX.
                //FINISH:
                gvQueryReport.VirtualItemCount = countA;

                gvQueryReport.DataSource = dt;
                gvQueryReport.DataBind();
            }
            catch (Exception ER_WC_14)
            {
                Function_Method.MsgBox("ER_WC_14: " + ER_WC_14.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        public int GetBusinessDays(DateTime startDate, DateTime endDate)
        {
            int days = 0;

            if (startDate.Date == endDate.Date)
            {
                // Same date, set days to 0
                days = 0;
            }
            else if (startDate.Date.AddDays(1) == endDate.Date)
            {
                // Consecutive dates, count as 1 day if both are weekdays
                if (startDate.DayOfWeek != DayOfWeek.Saturday && startDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    days = 1;
                }
            }
            else
            {
                for (DateTime date = startDate; date < endDate; date = date.AddDays(1))
                {
                    if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                    {
                        days++;
                    }
                }
            }

            return days;
        }
    }
}