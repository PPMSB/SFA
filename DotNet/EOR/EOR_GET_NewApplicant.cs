using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace DotNet
{
    public class EOR_GET_NewApplicant
    {
        public static Tuple<string, string, string, string, string, string, string> getCustInfo(Axapta DynAx, string CustAccount)
        {
            //int CustTable = 77;
            int tableId = DynAx.GetTableId("CustTable");

            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", tableId);
            var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", 1);//CustAccount
            qbr1.Call("value", CustAccount);
            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);

            string CustName = "";
            string Address = "";
            string temp_EmplId = "";
            string temp_EmpName = "";
            string BranchID = "";
            string CustomerClass = "";
            string getOpeningAccDate = "";

            if ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", tableId);
                CustName = DynRec1.get_Field("Name").ToString();
                Address = DynRec1.get_Field("Address").ToString();
                temp_EmplId = DynRec1.get_Field("EmplId").ToString();
                temp_EmpName = getEmpName(DynAx, temp_EmplId);

                BranchID = DynRec1.get_Field("Dimension").ToString();
                CustomerClass = DynRec1.get_Field("CustomerClass").ToString();

                getOpeningAccDate = DynRec1.get_Field("OpeningAccDate").ToString();
                if (getOpeningAccDate != "")
                {
                    string[] arr_getOpeningAccDate = getOpeningAccDate.Trim().Split(' ');//date + " " + time;
                    getOpeningAccDate = arr_getOpeningAccDate[0];
                }
                else
                {
                    getOpeningAccDate = "-";
                }

                DynRec1.Dispose();
            }
            return new Tuple<string, string, string, string, string, string, string>
                (CustName, Address, temp_EmplId, temp_EmpName, BranchID, CustomerClass, getOpeningAccDate);
        }

        public static Tuple<string, string, string, string, string, string, string> getCustInfo_2(Axapta DynAx, string CustAccount)
        {
            int CustTable = 77;
            AxaptaObject axQuery1_2 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1_2 = (AxaptaObject)axQuery1_2.Call("addDataSource", CustTable);
            var qbr1_2 = (AxaptaObject)axQueryDataSource1_2.Call("addRange", 1);//CustAccount
            qbr1_2.Call("value", CustAccount);
            AxaptaObject axQueryRun1_2 = DynAx.CreateAxaptaObject("QueryRun", axQuery1_2);

            string CustomerContactId = "";
            string CustTelNo = "";
            string CustStreet = ""; string CustZipCode = ""; string CustCity = ""; string CustState = ""; string CustCountry = "";
            if ((bool)axQueryRun1_2.Call("next"))
            {
                AxaptaRecord DynRec1_2 = (AxaptaRecord)axQueryRun1_2.Call("Get", CustTable);
                CustomerContactId = DynRec1_2.get_Field("CONTACTPERSONID").ToString();

                CustStreet = DynRec1_2.get_Field("Street").ToString();
                CustZipCode = DynRec1_2.get_Field("Zipcode").ToString();
                CustCity = DynRec1_2.get_Field("City").ToString();
                CustState = DynRec1_2.get_Field("State").ToString();
                CustCountry = DynRec1_2.get_Field("CountryRegionId").ToString();
                CustTelNo = DynRec1_2.get_Field("Phone").ToString();
                DynRec1_2.Dispose();
            }
            return new Tuple<string, string, string, string, string, string, string>
                (CustomerContactId, CustTelNo, CustStreet, CustZipCode, CustCity, CustState, CustCountry);
        }

        public static string getClassDetails(Axapta DynAx, string CustomerClass)
        {
            if (CustomerClass == "")
            {
                return "";
            }
            string ClassDesc = "";
            int MSBCustomerClass = 30003;
            AxaptaObject axQuery3 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource3 = (AxaptaObject)axQuery3.Call("addDataSource", MSBCustomerClass);

            var qbr3 = (AxaptaObject)axQueryDataSource3.Call("addRange", 30002);//CustomerClass
            qbr3.Call("value", CustomerClass);

            AxaptaObject axQueryRun3 = DynAx.CreateAxaptaObject("QueryRun", axQuery3);
            if ((bool)axQueryRun3.Call("next"))
            {
                AxaptaRecord DynRec3 = (AxaptaRecord)axQueryRun3.Call("Get", MSBCustomerClass);
                ClassDesc = DynRec3.get_Field("ClassDesc").ToString();
                DynRec3.Dispose();
            }
            return ClassDesc;
        }

        // Jerry 2024-12-16 Remove hardcoded table and field id, get contact person where Inactive == 0
        //public static string getContactPersonName(Axapta DynAx, string ContactPersonId)
        public static string getContactPersonName(Axapta DynAx, string CustAccount)
        {
            string getContactPersonName = "";
            //int CONTACTPERSON = 520;
            int ContactPerson_table_id = DynAx.GetTableIdWithLock("ContactPerson");

            AxaptaObject axQuery8 = DynAx.CreateAxaptaObject("Query");
            //AxaptaObject axQueryDataSource8 = (AxaptaObject)axQuery8.Call("addDataSource", CONTACTPERSON);
            AxaptaObject axQueryDataSource8 = (AxaptaObject)axQuery8.Call("addDataSource", ContactPerson_table_id);

            int CustAccount_field_id = DynAx.GetFieldId(ContactPerson_table_id, "CustAccount");
            var qbr8 = (AxaptaObject)axQueryDataSource8.Call("addRange", CustAccount_field_id);//custAccount
            qbr8.Call("value", CustAccount);

            int LeftCompany_field_id = DynAx.GetFieldId(ContactPerson_table_id, "LeftCompany");
            var LeftCompany_range = (AxaptaObject)axQueryDataSource8.Call("addRange", LeftCompany_field_id);//leftcompany
            LeftCompany_range.Call("value", "0");

            AxaptaObject axQueryRun8 = DynAx.CreateAxaptaObject("QueryRun", axQuery8);
            if ((bool)axQueryRun8.Call("next"))
            {
                //AxaptaRecord DynRec8 = (AxaptaRecord)axQueryRun8.Call("Get", CONTACTPERSON);
                AxaptaRecord DynRec8 = (AxaptaRecord)axQueryRun8.Call("Get", ContactPerson_table_id);
                getContactPersonName = DynRec8.get_Field("Name").ToString().Trim();
                DynRec8.Dispose();
            }

            return getContactPersonName;
        }
        // Jerry 2024-12-16 Remove hardcoded table and field id, get contact person where Inactive == 0 - END

        public static Tuple<string, string, string> getEORClassSetup(Axapta DynAx, string CustomerClass)
        {
            string EOR_CartonValue = "";
            string EOR_PointValue = "";
            string EOR_ApprovedBy = "";
            int LF_EOR_CLASSSETUP = 50176;

            AxaptaObject axQuery9 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource9 = (AxaptaObject)axQuery9.Call("addDataSource", LF_EOR_CLASSSETUP);

            var qbr9 = (AxaptaObject)axQueryDataSource9.Call("addRange", 50003);//LF_EOR_CUSTOMERCLASS
            qbr9.Call("value", CustomerClass);

            AxaptaObject axQueryRun9 = DynAx.CreateAxaptaObject("QueryRun", axQuery9);

            if ((bool)axQueryRun9.Call("next"))
            {
                AxaptaRecord DynRec9 = (AxaptaRecord)axQueryRun9.Call("Get", LF_EOR_CLASSSETUP);

                EOR_CartonValue = DynRec9.get_Field("LF_EOR_CartonValue").ToString().Trim();
                EOR_PointValue = DynRec9.get_Field("LF_EOR_PointValue").ToString().Trim();

                EOR_ApprovedBy = DynRec9.get_Field("APPROVEDBY").ToString().Trim();

                DynRec9.Dispose();
            }

            return new Tuple<string, string, string>(EOR_CartonValue, EOR_PointValue, EOR_ApprovedBy);
        }

        public static Tuple<double[], string[], int, int[]> getAmountMST_TransDate_Invoice(Axapta DynAx, string CustomerAcc, int system_checking)
        {
            double[] double_arr_AmountMST = { 0, 0, 0, 0, 0, 0 };//6 month data
            string[] arr_TransDate = { "", "", "", "", "", "" };
            int[] int_transaction_number = { 0, 0, 0, 0, 0, 0 };//6 month data

            int CustTrans = 78;

            AxaptaObject axQuery10 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource10 = (AxaptaObject)axQuery10.Call("addDataSource", CustTrans);

            var qbr10 = (AxaptaObject)axQueryDataSource10.Call("addRange", 1);//ACCOUNTNUM
            qbr10.Call("value", CustomerAcc);

            axQueryDataSource10.Call("addSortField", 65534, 1);//RecId , dsc

            AxaptaObject axQueryRun10 = DynAx.CreateAxaptaObject("QueryRun", axQuery10);
            //-----------//-----------//-----------//-----------//-----------
            string TodayDate = DateTime.Now.ToString("dd/MM/yyyy");//default
            string[] arr_raw_TodayDate = TodayDate.Split('/');
            string Month_TodayDate = arr_raw_TodayDate[1];
            int int_Month_TodayDate = Convert.ToInt32(Month_TodayDate);
            string Year_TodayDate = arr_raw_TodayDate[2];

            int int_Year_TodayDate = Convert.ToInt32(Year_TodayDate);
            int[] target_month = new int[6]; int[] target_year = new int[6];

            int temp = int_Month_TodayDate;// 6 month records
            for (int i = 0; i < 6; i++)
            {
                temp = temp - 1;
                if (temp > 0)
                {
                    target_month[i] = temp;
                    target_year[i] = int_Year_TodayDate;
                }
                else
                {
                    target_month[i] = temp + 12;
                    target_year[i] = int_Year_TodayDate - 1;
                }
            }
            //-----------//-----------//-----------//-----------//-----------//-----------

            int count = 0;
            while ((bool)axQueryRun10.Call("next"))
            {
                AxaptaRecord DynRec10 = (AxaptaRecord)axQueryRun10.Call("Get", CustTrans);

                string SettleAmountMST = DynRec10.get_Field("SettleAmountMST").ToString();

                int length_SettleAmountMST = SettleAmountMST.Length;
                string first_char_SettleAmountMST = SettleAmountMST.Substring(0, 1);
                string AmountMST = DynRec10.get_Field("AmountMST").ToString();
                if (first_char_SettleAmountMST == "-")
                {
                    //do nothing, it is credit acc
                }
                else if (AmountMST == "0")
                {
                    //do nothing, it is 0
                }
                else
                {

                    string temp_TransDate = DynRec10.get_Field("TRANSDATE").ToString();
                    string temp_invoice_list = DynRec10.get_Field("Invoice").ToString();
                    string[] arr_temp_TransDate = temp_TransDate.Split(' ');//date + " " + time;
                    string Raw_temp_TransDate = arr_temp_TransDate[0];
                    string TransDate = Function_Method.get_correct_date(GLOBAL.system_checking, Raw_temp_TransDate, true);
                    string[] arr_part_TransDate = TransDate.Split('/');//date
                    int Month_TransDate = Convert.ToInt32(arr_part_TransDate[1]);
                    int Year_TransDate = Convert.ToInt32(arr_part_TransDate[2]);

                    double double_AmountMST = Convert.ToDouble(AmountMST);
                    for (int j = 0; j < 6; j++)
                    {
                        if ((Month_TransDate - target_month[j] == 0) && (Year_TransDate - target_year[j] == 0))
                        {
                            double_arr_AmountMST[j] = double_arr_AmountMST[j] + double_AmountMST;
                            arr_TransDate[j] = Month_TransDate.ToString() + "/" + Year_TransDate.ToString();
                            int_transaction_number[j] = int_transaction_number[j] + 1;
                            count = count + 1;
                            goto NEXT;
                        }
                        else
                        {
                            if (target_year[j] - Year_TransDate > 2)
                            {
                                goto SKIP;
                            }

                            goto NEXT;
                        }
                    NEXT:;
                    }

                    //if(count==6)
                    //{
                    DynRec10.Dispose();
                    //  goto SKIP;
                    //}
                }
                DynRec10.Dispose();
            }
        SKIP:

            return new Tuple<double[], string[], int, int[]>(double_arr_AmountMST, arr_TransDate, count, int_transaction_number);
        }

        public static List<ListItem> get_SearchEquipment(Axapta DynAx, string Description_toSearch, string Filter)
        {
            List<ListItem> List_SearchEquipment = new List<ListItem>();

            int InventTable = 175;

            AxaptaObject axQuery11 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource11 = (AxaptaObject)axQuery11.Call("addDataSource", InventTable);
            string fieldValue = "*" + Description_toSearch + "*";

            var qbr11 = (AxaptaObject)axQueryDataSource11.Call("addRange", 3);//ItemName
            qbr11.Call("value", fieldValue);

            if (Filter != "")
            {
                var qbr11_2 = (AxaptaObject)axQueryDataSource11.Call("addRange", 30002);//ITEMGRP2
                qbr11_2.Call("value", "L0010");
            }
            AxaptaObject axQueryRun11 = DynAx.CreateAxaptaObject("QueryRun", axQuery11);
            List_SearchEquipment.Add(new ListItem("-- SELECT --", ""));
            while ((bool)axQueryRun11.Call("next"))
            {
                AxaptaRecord DynRec11 = (AxaptaRecord)axQueryRun11.Call("Get", InventTable);
                string temp_ItemName = DynRec11.get_Field("ItemName").ToString();

                if (temp_ItemName.Length > 5)
                {
                    if (temp_ItemName.Substring(0, 6) == "PLEASE") //remove please dont use item
                    {
                        goto SKIP_2;
                    }
                }
                string ItemName = DynRec11.get_Field("ItemName").ToString();
                string ItemId = DynRec11.get_Field("ItemId").ToString();
                string RecId = DynRec11.get_Field("RecId").ToString();
                string QpuItem = DynRec11.get_Field("QpuItem").ToString();
                List_SearchEquipment.Add(new ListItem(ItemName, ItemId + "|" + RecId + "|" + QpuItem));

            SKIP_2: DynRec11.Dispose();
            }
            return List_SearchEquipment;
        }

        //*************************************************************************************************************
        public static string getEmpName(Axapta DynAx, string temp_EmplId)
        {
            if (temp_EmplId == "")
            {
                return "";
            }

            string temp_EmpName = "";
            //int EmplTable = 103;
            int tableId = DynAx.GetTableId("EmplTable");
            int fieldId = DynAx.GetFieldId(tableId, "EmplId");

            AxaptaObject axQuery2 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource2 = (AxaptaObject)axQuery2.Call("addDataSource", tableId);

            var qbr2 = (AxaptaObject)axQueryDataSource2.Call("addRange", fieldId);//temp_EmplId
            qbr2.Call("value", temp_EmplId);
            AxaptaObject axQueryRun2 = DynAx.CreateAxaptaObject("QueryRun", axQuery2);
            if ((bool)axQueryRun2.Call("next"))
            {
                AxaptaRecord DynRec2 = (AxaptaRecord)axQueryRun2.Call("Get", tableId);
                temp_EmpName = DynRec2.get_Field("Del_Name").ToString();
                DynRec2.Dispose();
            }
            return temp_EmpName;
        }

        public static string get_User_Id(Axapta DynAx, string EmplId)
        {
            string UserId = "";

            //need to use "select" becoz of salesman ony can access with this method
            AxaptaRecord DynRec;
            string TableName = "EmplTable";
            string fieldName = ("EmplId");
            //string fieldValue = ("your_search_criteria_here");

            // Define the record
            DynRec = DynAx.CreateAxaptaRecord(TableName);
            DynRec.ExecuteStmt(string.Format("select * from %1 where %1.{0} == '{1}'", fieldName, EmplId));
            // Check if the query returned any data.
            if (DynRec.Found)
            {
                string temp_Email = (string)DynRec.get_Field("LF_EmpEMailID");
                string[] arr_temp_Email = temp_Email.Split('@');
                UserId = arr_temp_Email[0];
            }
            DynRec.Dispose();
            return UserId;
        }

        public static double getPPMTP_AdjPts(Axapta DynAx, string CustAcc)
        {
            double getPPMTP_AdjPts = 0;

            int PPMTP_tmpAdjPts = DynAx.GetTableId("PPMTP_tmpAdjPts");
            AxaptaObject axQuery5 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource5 = (AxaptaObject)axQuery5.Call("addDataSource", PPMTP_tmpAdjPts);

            var qbr5 = (AxaptaObject)axQueryDataSource5.Call("addRange", 40001);//AccountNum
            qbr5.Call("value", CustAcc);

            double CalA = 0;
            double CalATotal = 0;

            AxaptaObject axQueryRun5 = DynAx.CreateAxaptaObject("QueryRun", axQuery5);
            while ((bool)axQueryRun5.Call("next"))
            {
                AxaptaRecord DynRec5 = (AxaptaRecord)axQueryRun5.Call("Get", PPMTP_tmpAdjPts);
                CalA = Convert.ToDouble(DynRec5.get_Field("PPMTP_AdjPts"));
                CalATotal = CalATotal + CalA;
                DynRec5.Dispose();
            }
            getPPMTP_AdjPts = CalATotal;
            return getPPMTP_AdjPts;
        }

        public static double getPPMTP_UsedPt(Axapta DynAx, string CustAcc)
        {
            double getPPMTP_UsedPt = 0;

            int PPMTP_tmpUsedPts = DynAx.GetTableId("PPMTP_tmpUsedPts");
            AxaptaObject axQuery6 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource6 = (AxaptaObject)axQuery6.Call("addDataSource", PPMTP_tmpUsedPts);

            var qbr6 = (AxaptaObject)axQueryDataSource6.Call("addRange", 40004);//AccountNum
            qbr6.Call("value", CustAcc);
            AxaptaObject axQueryRun6 = DynAx.CreateAxaptaObject("QueryRun", axQuery6);
            double CalB = 0;
            double CalBTotal = 0;
            while ((bool)axQueryRun6.Call("next"))
            {
                AxaptaRecord DynRec6 = (AxaptaRecord)axQueryRun6.Call("Get", PPMTP_tmpUsedPts);
                CalB = Convert.ToDouble(DynRec6.get_Field("PPMTP_UsedPt"));
                CalBTotal = CalBTotal + CalB;
                DynRec6.Dispose();
            }
            getPPMTP_UsedPt = CalBTotal;
            return getPPMTP_UsedPt;
        }

        public static Tuple<double, string> getPointBalance(Axapta DynAx, string CustAcc)
        {
            double getPointBalance = 0; string recID = "";
            int PointBalance = DynAx.GetTableId("PointBalance");
            int blocked = DynAx.GetFieldId(PointBalance, "Blocked");
            AxaptaObject axQuery7 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource7 = (AxaptaObject)axQuery7.Call("addDataSource", PointBalance);

            var qbr7 = (AxaptaObject)axQueryDataSource7.Call("addRange", 40001);//CustomerClass
            qbr7.Call("value", CustAcc);           
            
            var qbr8 = (AxaptaObject)axQueryDataSource7.Call("addRange", blocked);//blocked
            qbr8.Call("value", "0");
            AxaptaObject axQueryRun7 = DynAx.CreateAxaptaObject("QueryRun", axQuery7);
            if ((bool)axQueryRun7.Call("next"))
            {
                AxaptaRecord DynRec7 = (AxaptaRecord)axQueryRun7.Call("Get", PointBalance);
                getPointBalance = Convert.ToDouble(DynRec7.get_Field("TPBalance"));
                recID = DynRec7.get_Field("recid").ToString();
                DynRec7.Dispose();
            }

            return new Tuple <double, string> (getPointBalance, recID);
        }

        public static Tuple<string, string> getItemGroupId_RecId(Axapta DynAx, string ItemId)
        {
            string getItemGroupId = "";
            string Item_RecId = "";

            int InventTable = 175;
            AxaptaObject axQuery12 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource12 = (AxaptaObject)axQuery12.Call("addDataSource", InventTable);

            var qbr12 = (AxaptaObject)axQueryDataSource12.Call("addRange", 2);//ItemId
            qbr12.Call("value", ItemId);
            AxaptaObject axQueryRun12 = DynAx.CreateAxaptaObject("QueryRun", axQuery12);
            if ((bool)axQueryRun12.Call("next"))
            {
                AxaptaRecord DynRec12 = (AxaptaRecord)axQueryRun12.Call("Get", InventTable);
                getItemGroupId = DynRec12.get_Field("ITEMGROUPID").ToString();
                Item_RecId = DynRec12.get_Field("RecId").ToString();
                DynRec12.Dispose();
            }

            return new Tuple<string, string>(getItemGroupId, Item_RecId);
        }

        public static Tuple<string, string, string> Check_EOR_Carton_List_Auto(Axapta DynAx, string RecId_Equipment, string SalesmanNo, string EquipmentItemId, string ContractDuration)
        {
            string Suggest_CartonDep = ""; string Suggest_CartonNoDep = ""; string Suggest_Deposit = "";

            int LF_InventTable_EOR = 30559;
            AxaptaObject axQuery13 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource13 = (AxaptaObject)axQuery13.Call("addDataSource", LF_InventTable_EOR);

            var qbr13 = (AxaptaObject)axQueryDataSource13.Call("addRange", 30001);//RecId
            qbr13.Call("value", RecId_Equipment);

            var qbr13_2 = (AxaptaObject)axQueryDataSource13.Call("addRange", 30003);//ItemId
            qbr13_2.Call("value", EquipmentItemId);

            //var var_ContractDuration = Convert.ToDouble(ContractDuration);
            var qbr13_4 = (AxaptaObject)axQueryDataSource13.Call("addRange", 30010);//Period
            qbr13_4.Call("value", ContractDuration);

            axQueryDataSource13.Call("addSortField", DynAx.GetFieldId(LF_InventTable_EOR, "RecId"), 1);//RecId , asc

            AxaptaObject axQueryRun13 = DynAx.CreateAxaptaObject("QueryRun", axQuery13);
            while ((bool)axQueryRun13.Call("next"))
            {
                AxaptaRecord DynRec13 = (AxaptaRecord)axQueryRun13.Call("Get", LF_InventTable_EOR);

                string SmanTo = DynRec13.get_Field("SmanTo").ToString();
                string SmanFrom = DynRec13.get_Field("SmanFrom").ToString();
                //check salesman eligiblity first
                bool CheckSalesmanEligibility = check_Salesman_Carton_List_Auto(DynAx, SmanFrom, SmanTo, SalesmanNo);
                //end of check salesman
                if (CheckSalesmanEligibility == true)
                {
                    string TodayDate = DateTime.Now.ToString("dd/MM/yyyy");//default
                    TodayDate = Function_Method.get_correct_date(GLOBAL.system_checking, TodayDate, false);

                    var var_StartQDate = DateTime.ParseExact(TodayDate, "dd/MM/yyyy", null);// in lotus use cdat
                    string EffectiveDate = DynRec13.get_Field("EffectiveDate").ToString();
                    string[] arr_EffectiveDate = EffectiveDate.Split(' ');//date + " " + time;
                    string Raw_EffectiveDate = arr_EffectiveDate[0];
                    string temp_EffectiveDate = Function_Method.get_correct_date(GLOBAL.system_checking, Raw_EffectiveDate, true);

                    //endDate = Function_Method.get_correct_date(GLOBAL.system_checking, endDate, false);
                    var var_EffectiveDate = DateTime.ParseExact(temp_EffectiveDate, "dd/MM/yyyy", null);// in lotus use cdat
                    int res = DateTime.Compare(var_StartQDate, var_EffectiveDate);

                    if (res < 0)//Today date < Effective date
                    {
                        //do nothing
                    }
                    else
                    {
                        Suggest_CartonDep = DynRec13.get_Field("CTN_Dep").ToString();
                        Suggest_CartonNoDep = DynRec13.get_Field("CTN_NoDep").ToString();
                        Suggest_Deposit = DynRec13.get_Field("Deposit").ToString();
                        return new Tuple<string, string, string>(Suggest_CartonDep, Suggest_CartonNoDep, Suggest_Deposit);
                    }
                }
                else
                {
                    //do nothing
                }
                DynRec13.Dispose();
            }

            return new Tuple<string, string, string>(Suggest_CartonDep, Suggest_CartonNoDep, Suggest_Deposit);
        }

        public static bool check_Salesman_Carton_List_Auto(Axapta DynAx, string SalesmanFrom, string SalesmanTo, string SalesmanNo)
        {
            AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("EmplTable");

            DynRec.ExecuteStmt(string.Format("select * from %1 where %1.{0}>='{1}' && %1.{0} <='{2}'", "EmplId", SalesmanFrom, SalesmanTo));
            // Check if the query returned any data.
            if (DynRec.Found)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Tuple<string> getItemInfo_from_RecId(Axapta DynAx, string RecId)
        {
            string ItemId = "";
            int InventTable = 175;
            AxaptaObject axQuery14 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource14 = (AxaptaObject)axQuery14.Call("addDataSource", InventTable);

            var qbr14 = (AxaptaObject)axQueryDataSource14.Call("addRange", 65534);//RecId
            qbr14.Call("value", RecId);
            AxaptaObject axQueryRun14 = DynAx.CreateAxaptaObject("QueryRun", axQuery14);
            if ((bool)axQueryRun14.Call("next"))
            {
                AxaptaRecord DynRec14 = (AxaptaRecord)axQueryRun14.Call("Get", InventTable);
                ItemId = DynRec14.get_Field("ITEMID").ToString();
                DynRec14.Dispose();
            }

            return new Tuple<string>(ItemId);
        }

        public static Tuple<string, string, string, string, string, string, string> Check_ApprovalListFromLF_WebEquipment(Axapta DynAx, string EQUIP_ID)
        {
            int LF_WebEquipment = 30346;
            string NA_HOD = ""; string NA_Admin = ""; string NA_Manager = ""; string NA_GM = "";

            string NextApproval = ""; string NextApprovalAlt = ""; string HandOnStatus = "";

            AxaptaObject axQuery18 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource18 = (AxaptaObject)axQuery18.Call("addDataSource", LF_WebEquipment);

            var qbr18 = (AxaptaObject)axQueryDataSource18.Call("addRange", 30001);//EQUIP_ID
            qbr18.Call("value", EQUIP_ID);
            AxaptaObject axQueryRun18 = DynAx.CreateAxaptaObject("QueryRun", axQuery18);
            if ((bool)axQueryRun18.Call("next"))
            {
                AxaptaRecord DynRec18 = (AxaptaRecord)axQueryRun18.Call("Get", LF_WebEquipment);
                NA_HOD = DynRec18.get_Field("NA_HOD").ToString();
                NA_Admin = DynRec18.get_Field("NA_Admin").ToString();
                NA_Manager = DynRec18.get_Field("NA_Manager").ToString();
                NA_GM = DynRec18.get_Field("NA_GM").ToString();
                NextApproval = DynRec18.get_Field("NextApprover").ToString();
                NextApprovalAlt = DynRec18.get_Field("NextApproverAlt").ToString();
                HandOnStatus = DynRec18.get_Field("DocStatus").ToString();

                DynRec18.Dispose();
            }
            return new Tuple<string, string, string, string, string, string, string>(NA_HOD, NA_Admin, NA_Manager, NA_GM, NextApproval, NextApprovalAlt, HandOnStatus);
        }

        public static string getWebEquipmentProcessStatus(Axapta DynAx, string Equip_Id)
        {
            int LF_WebEquipment = 30346; string WebEquipmentProcessStatus = "";

            AxaptaObject axQuery19 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource19 = (AxaptaObject)axQuery19.Call("addDataSource", LF_WebEquipment);

            var qbr19 = (AxaptaObject)axQueryDataSource19.Call("addRange", 30001);//EQUIP_ID
            qbr19.Call("value", Equip_Id);
            AxaptaObject axQueryRun19 = DynAx.CreateAxaptaObject("QueryRun", axQuery19);

            if ((bool)axQueryRun19.Call("next"))
            {
                AxaptaRecord DynRec18 = (AxaptaRecord)axQueryRun19.Call("Get", LF_WebEquipment);
                WebEquipmentProcessStatus = DynRec18.get_Field("ProcessStatus").ToString();
                DynRec18.Dispose();
            }
            return WebEquipmentProcessStatus;
        }

        public static Tuple<string, string, string> Check_LF_EOR_Approval(Axapta DynAx, string Company)
        {
            int LF_EOR_Approval = 50677;
            string NA_SalesAdmin = ""; string NA_SalesManager = "";
            string NA_GM = "";

            AxaptaObject axQuery17 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource17 = (AxaptaObject)axQuery17.Call("addDataSource", LF_EOR_Approval);

            var qbr17 = (AxaptaObject)axQueryDataSource17.Call("addRange", 50001);//Company
            qbr17.Call("value", Company);
            AxaptaObject axQueryRun17 = DynAx.CreateAxaptaObject("QueryRun", axQuery17);
            if ((bool)axQueryRun17.Call("next"))
            {
                AxaptaRecord DynRec17 = (AxaptaRecord)axQueryRun17.Call("Get", LF_EOR_Approval);
                string temp_SalesAdmin = DynRec17.get_Field("SalesAdmin").ToString();
                string temp_AltSalesAdmin = DynRec17.get_Field("AltSalesAdmin").ToString();
                string temp_SalesManager = DynRec17.get_Field("SalesAdminManager").ToString();
                string temp_AltSalesManager = DynRec17.get_Field("AltSalesAdminManager").ToString();
                string temp_GM = DynRec17.get_Field("GM").ToString();
                /*
                SalesAdmin = WClaim_GET_NewApplicant.CheckUserName(DynAx, temp_SalesAdmin);
                AltSalesAdmin = WClaim_GET_NewApplicant.CheckUserName(DynAx, temp_AltSalesAdmin);
                SalesManager = WClaim_GET_NewApplicant.CheckUserName(DynAx, temp_SalesManager);
                AltSalesManager = WClaim_GET_NewApplicant.CheckUserName(DynAx, temp_AltSalesManager);
                GM = WClaim_GET_NewApplicant.CheckUserName(DynAx, temp_GM);
                */
                if (temp_AltSalesAdmin != "") NA_SalesAdmin = temp_SalesAdmin + "_" + temp_AltSalesAdmin;
                else NA_SalesAdmin = temp_SalesAdmin;

                if (temp_SalesManager != "") NA_SalesManager = temp_SalesManager + "_" + temp_AltSalesManager;
                else NA_SalesAdmin = temp_SalesManager;

                NA_GM = temp_GM;
                DynRec17.Dispose();
            }
            return new Tuple<string, string, string>(NA_SalesAdmin, NA_SalesManager, NA_GM);
        }

        public static string get_NA_HODbyLevel(Axapta DynAx, string SalesmanNo)
        {
            string NA_HODbyLevel = "";
            var tuple_GroupId_ReportingTo = EOR_GET_NewApplicant.Check_User_GroupId_ReportingTo(DynAx, SalesmanNo);
            string GroupId = tuple_GroupId_ReportingTo.Item1;       // to find GroupId
            //string ReportingTo = tuple_GroupId_ReportingTo.Item2;   // Sales HOD
            if (GroupId != "")
            {
                var tuple_Check_EmplIdLvl = EOR_GET_NewApplicant.Check_EmplIdLvl(DynAx, GroupId);

                string EmplIdLvl1 = EOR_GET_NewApplicant.get_User_Id(DynAx, tuple_Check_EmplIdLvl.Item1);//lowests
                string EmplIdLvl2 = EOR_GET_NewApplicant.get_User_Id(DynAx, tuple_Check_EmplIdLvl.Item2);//lowests
                string EmplIdLvl3 = EOR_GET_NewApplicant.get_User_Id(DynAx, tuple_Check_EmplIdLvl.Item3);//lowests
                string EmplIdLvl4 = EOR_GET_NewApplicant.get_User_Id(DynAx, tuple_Check_EmplIdLvl.Item4);//lowests

                if (EmplIdLvl1 != "") NA_HODbyLevel = EmplIdLvl1;
                if (EmplIdLvl2 != "") NA_HODbyLevel = NA_HODbyLevel + "_" + EmplIdLvl2;
                if (EmplIdLvl3 != "") NA_HODbyLevel = NA_HODbyLevel + "_" + EmplIdLvl3;
                if (EmplIdLvl4 != "") NA_HODbyLevel = NA_HODbyLevel + "_" + EmplIdLvl4;
            }
            return NA_HODbyLevel;
        }

        public static string get_WebEquipmentId(Axapta DynAx, string RecId)
        {
            string EquipId = "";
            int LF_WebEquipment = 30346;

            AxaptaObject axQuery20 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource20 = (AxaptaObject)axQuery20.Call("addDataSource", LF_WebEquipment);

            var qbr20 = (AxaptaObject)axQueryDataSource20.Call("addRange", DynAx.GetFieldId(LF_WebEquipment, "RecId"));
            qbr20.Call("value", RecId);

            AxaptaObject axQueryRun20 = DynAx.CreateAxaptaObject("QueryRun", axQuery20);
            if ((bool)axQueryRun20.Call("next"))
            {
                AxaptaRecord DynRec15 = (AxaptaRecord)axQueryRun20.Call("Get", LF_WebEquipment);
                EquipId = DynRec15.get_Field("Equip_ID").ToString();

                DynRec15.Dispose();
            }
            return EquipId;
        }

        public static Tuple<string, string, string> Check_User_GroupId_ReportingTo(Axapta DynAx, string UserId)
        {
            int EmplTable = 103;
            string GroupId = ""; string ReportingTo = ""; string name = "";

            AxaptaObject axQuery15 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource15 = (AxaptaObject)axQuery15.Call("addDataSource", EmplTable);

            var qbr15 = (AxaptaObject)axQueryDataSource15.Call("addRange", 1);//UserId
            qbr15.Call("value", UserId);
            AxaptaObject axQueryRun15 = DynAx.CreateAxaptaObject("QueryRun", axQuery15);
            if ((bool)axQueryRun15.Call("next"))
            {
                AxaptaRecord DynRec15 = (AxaptaRecord)axQueryRun15.Call("Get", EmplTable);
                GroupId = DynRec15.get_Field("LF_SalesApprovalGroup").ToString();
                ReportingTo = DynRec15.get_Field("ReportTo").ToString();
                name = DynRec15.get_Field("LF_EmplName").ToString();
                name = (string.IsNullOrEmpty(name) ? DynRec15.get_Field("Del_Name").ToString() : name);
                DynRec15.Dispose();
            }
            return new Tuple<string, string, string>(GroupId, ReportingTo, name);
        }

        public static Tuple<string, string, string, string> Check_EmplIdLvl(Axapta DynAx, string GroupId)
        {
            int LF_SalesApprovalLevel = 30370;
            string EmplIdLvl1 = ""; string EmplIdLvl2 = ""; string EmplIdLvl3 = ""; string EmplIdLvl4 = "";

            AxaptaObject axQuery16 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource16 = (AxaptaObject)axQuery16.Call("addDataSource", LF_SalesApprovalLevel);

            var qbr16 = (AxaptaObject)axQueryDataSource16.Call("addRange", 30001);//GroupId
            qbr16.Call("value", GroupId);
            AxaptaObject axQueryRun16 = DynAx.CreateAxaptaObject("QueryRun", axQuery16);
            if ((bool)axQueryRun16.Call("next"))
            {
                AxaptaRecord DynRec16 = (AxaptaRecord)axQueryRun16.Call("Get", LF_SalesApprovalLevel);
                EmplIdLvl1 = DynRec16.get_Field("EmplIdLvl1").ToString();
                EmplIdLvl2 = DynRec16.get_Field("EmplIdLvl2").ToString();
                EmplIdLvl3 = DynRec16.get_Field("EmplIdLvl3").ToString();
                EmplIdLvl4 = DynRec16.get_Field("EmplIdLvl4").ToString();
                DynRec16.Dispose();
            }
            return new Tuple<string, string, string, string>(EmplIdLvl1, EmplIdLvl2, EmplIdLvl3, EmplIdLvl4);
        }
    }
}