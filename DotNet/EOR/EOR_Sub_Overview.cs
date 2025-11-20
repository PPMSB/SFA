using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DotNet
{
    public partial class EOR : System.Web.UI.Page
    {
        protected void Button_ListAll_Click(object sender, EventArgs e)
        {
            f_Button_ListAll();
        }
        private void f_Button_ListAll()
        {
            Session["flag_temp"] = 0;//List all
            GridViewOverviewList.PageIndex = 0;
            GridViewOverviewList.Columns[1].Visible = true;//Equipment Id button
            GridViewOverviewList.Columns[2].Visible = false;//Equipment Id label
            //GridViewOverviewList.Columns[2].Visible = false;//Equipment Id label
            EOR_Overview(0, "");
            TextBox_Search_Overview.Text = "";
            //CheckBox_div_Searchable_ID_Overview.Visible = true;
            Button_ListOutStanding.Attributes.Add("style", "background-color:#f58345");
            overview_section_general.Visible = true; Button_Overview_accordion.Text = "List All";
        }

        //protected void CheckBox_div_Searchable_Overview(object sender, EventArgs e)
        //{
        //    if (CheckBox_div_Searchable_ID_Overview.Checked)
        //    {
        //        div_Searchable_Overview.Visible = true;
        //        CheckBox_div_Searchable_ID_Overview.Text = "Hide Search Bar";
        //    }
        //    else
        //    {
        //        div_Searchable_Overview.Visible = false;
        //        CheckBox_div_Searchable_ID_Overview.Text = "Show Search Bar";
        //    }
        //}

        protected void Button_Search_Overview_Click(object sender, ImageClickEventArgs e)//renamed as Add Sales Line
        {
            string fieldName = "";
            switch (DropDownList_Search_Overview.SelectedItem.Text)
            {
                case "Equipment Id":
                    fieldName = "EQUIP_ID";//EQUIP_ID
                    break;
                case "Customer Account No.":
                    fieldName = "CUSTACCOUNT";//CUSTACCOUNT
                    break;

                default:
                    fieldName = "";
                    break;
            }
            EOR_Overview(0, fieldName);
        }

        private void EOR_Overview(int PAGE_INDEX, string fieldName)
        {
            GridViewOverviewList.DataSource = null;
            GridViewOverviewList.DataBind();

            Axapta DynAx = new Axapta();
            try
            {
                // Log on to Microsoft Dynamics AX.
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                    GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                int LF_WEBEQUIPMENT = 30346;

                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", LF_WEBEQUIPMENT);

                //var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", 30043);//DEPOSITTYPE
                //qbr.Call("value", "EOR");
                /*
                switch (Convert.ToInt32(Session["flag_temp"]))
                {
                    case 0:// List Outstanding
                        //var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", 33);//SalesStatus
                        //qbr.Call("value", "1");//1=Open Order; 2=Delivered; 3=Invoiced; 4:Cancelled
                        break;
                    default:
                        //nothing
                        break;
                }
                */
                string temp_SearchValue = "*" + TextBox_Search_Overview.Text.Trim() + "*";
                if (fieldName != "" && temp_SearchValue != "")
                {
                    if (fieldName == "EQUIP_ID")
                    {
                        var qbr3_1 = (AxaptaObject)axQueryDataSource.Call("addRange", 30001);
                        qbr3_1.Call("value", temp_SearchValue);
                    }
                    if (fieldName == "CUSTACCOUNT")
                    {
                        var qbr3_2 = (AxaptaObject)axQueryDataSource.Call("addRange", 30002);
                        qbr3_2.Call("value", temp_SearchValue);
                    }
                }
                axQueryDataSource.Call("addSortField", 30001, 1);//EQUIP_ID, descending
                AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
                //===========================================
                DataTable dt = new DataTable();
                int data_count = 11;
                string[] N = new string[data_count];
                N[0] = "No."; N[1] = "Equipment Id"; N[2] = "Customer Account"; N[3] = "Customer Name";
                N[4] = "Customer Phone"; N[5] = "Status"; N[6] = "Next Appr";
                N[7] = "Next Appr Alt"; N[8] = "Salesman"; N[9] = "Applied By";
                N[10] = "Process Status";

                for (int i = 0; i < data_count; i++)
                {
                    dt.Columns.Add(new DataColumn(N[i], typeof(string)));
                }
                //===========================================
                DataRow row;
                int countA = 0;

                int startA = Function_Method.paging_grid(PAGE_INDEX)[0];
                int endA = Function_Method.paging_grid(PAGE_INDEX)[1];
                //===========================================
                // Loop through the set of retrieved records.

                while ((bool)axQueryRun.Call("next"))
                {
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", LF_WEBEQUIPMENT);

                    countA = countA + 1;

                    if (countA >= startA && countA <= endA)
                    {
                        row = dt.NewRow();

                        row["No."] = countA;
                        string temp_EquipmentId = DynRec.get_Field("EQUIP_ID").ToString();
                        row["Equipment Id"] = temp_EquipmentId;
                        row["Customer Name"] = DynRec.get_Field("CustContact").ToString();
                        row["Customer Account"] = DynRec.get_Field("CUSTACCOUNT").ToString();
                        row["Customer Phone"] = DynRec.get_Field("CustPhone").ToString();
                        /*
                        string temp_period = DynRec.get_Field("Period").ToString();
                        if (temp_period=="1")//Duration 1 year
                        {row["Contract Duration"] = "12 months";}
                        else if (temp_period == "2")//Duration 2 years
                        {row["Contract Duration"] = "24 months";}
                        else if (temp_period == "3")//Duration 3 years
                        {row["Contract Duration"] = "36 months";}
                        else if (temp_period == "4")//Duration 4 years
                        {row["Contract Duration"] = "48 months";}
                        else if (temp_period == "5")//Duration 5 years
                        {row["Contract Duration"] = "60 months";}
                        */
                        string temp_DocStatus = DynRec.get_Field("DocStatus").ToString();

                        if (temp_DocStatus == "Approved")
                        {
                            fuSignboard.Visible = false;
                            fuExternal.Visible = false;
                            fuInternal1.Visible = false;
                            fuInternal2.Visible = false;
                            fuEquipment1.Visible = false;
                            fuEquipment2.Visible = false;
                        }// status not approved or rejected still can change

                        string Status = "";

                        switch (temp_DocStatus)
                        {
                            case "0":
                                Status = "Draft";
                                break;
                            case "1":
                                Status = "Awaiting HOD";
                                break;
                            case "2":
                                Status = "Awaiting SalesAdmin";
                                break;
                            case "3":
                                Status = "Awaiting SalesAdmin Manager";
                                break;
                            case "4":
                                Status = "Awaiting GM";
                                break;
                            case "5":
                                Status = "Approved";
                                break;
                            case "6":
                                Status = "Rejected";
                                break;
                            default:
                                Status = "";
                                break;
                        }

                        row["Status"] = Status;

                        row["Next Appr"] = DynRec.get_Field("NextApprover").ToString();
                        row["Next Appr Alt"] = DynRec.get_Field("NextApproverAlt").ToString();
                        row["Salesman"] = DynRec.get_Field("EmplName").ToString();
                        row["Applied By"] = DynRec.get_Field("AppliedBy").ToString();
                        /*
                          string temp_AppliedDate = DynRec.get_Field("APPLIEDDATE").ToString();
                          string[] arr_temp_AppliedDate = temp_AppliedDate.Split(' ');//date + " " + time;
                          string Raw_AppliedDate = arr_temp_AppliedDate[0];
                          row["Applied Date"] = Function_Method.get_correct_date(GLOBAL.system_checking, Raw_AppliedDate, true);
                          */
                        string temp_ProcessStatus = DynRec.get_Field("ProcessStatus").ToString();

                        temp_ProcessStatus = temp_ProcessStatus.Replace(".", "<br>");
                        row["Process Status"] = temp_ProcessStatus;
                        dt.Rows.Add(row);
                        // Advance to the next row.
                        DynRec.Dispose();
                    }
                    if (countA > endA)
                    {
                        goto FINISH;//speed up process
                    }
                }

            // Log off from Microsoft Dynamics AX.
            FINISH:
                GridViewOverviewList.VirtualItemCount = countA;
                DynAx.Logoff();
                //Data-Binding with our GRID

                GridViewOverviewList.DataSource = dt;
                GridViewOverviewList.DataBind();
            }
            catch (Exception ER_EO_13)
            {
                Function_Method.MsgBox("ER_EO_13: " + ER_EO_13.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        protected void datagrid_PageIndexChanging_Overview(object sender, GridViewPageEventArgs e)
        {
            try
            {
                if (TextBox_Search_Overview.Text == "")
                {
                    EOR_Overview(e.NewPageIndex, "");
                }
                else
                {
                    string fieldName = "";
                    switch (DropDownList_Search_Overview.SelectedItem.Text)
                    {
                        case "Equipment Id":
                            fieldName = "EQUIP_ID";//EQUIP_ID
                            break;
                        case "Customer Account No.":
                            fieldName = "CUSTACCOUNT";//CUSTACCOUNT
                            break;

                        default:
                            fieldName = "";
                            break;
                    }

                    EOR_Overview(e.NewPageIndex, fieldName);
                }

                GridViewOverviewList.PageIndex = e.NewPageIndex;
                GridViewOverviewList.DataBind();
            }
            catch (Exception ER_SO_14)
            {
                Function_Method.MsgBox("ER_SO_14: " + ER_SO_14.ToString(), this.Page, this);
            }
        }
        //end of Overview section

        protected void Button_EquipmentId_Click(object sender, EventArgs e)
        {
            string selected_Equipment_Id = "";
            Button Button_EquipmentId = sender as Button;
            if (Button_EquipmentId != null)
            {
                selected_Equipment_Id = Button_EquipmentId.Text;

                string ClientID = Button_EquipmentId.ClientID;
                string[] arr_ClientID = ClientID.Split('_');
                //int arr_count = arr_ClientID.Count();
                //int ClientRow = Convert.ToInt32(arr_ClientID[arr_count - 1]);
                //
                GridViewRow row = (GridViewRow)Button_EquipmentId.NamingContainer;
                int index = row.RowIndex;

                string Status = GridViewOverviewList.Rows[index].Cells[6].Text;

                string NextApprover = ""; string NextApproverAlt = "";
                string temp_NextApprover = GridViewOverviewList.Rows[index].Cells[7].Text;
                string temp_NextApproverAlt = GridViewOverviewList.Rows[index].Cells[8].Text;

                if (temp_NextApprover != "" && temp_NextApprover != "&nbsp;")
                {
                    NextApprover = temp_NextApprover;
                }
                if (temp_NextApproverAlt != "" && temp_NextApproverAlt != "&nbsp;")
                {
                    NextApproverAlt = temp_NextApproverAlt;
                }

                Axapta DynAx = new Axapta();
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                    GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                try
                {
                    if (Status == "Rejected")
                    {
                        Function_Method.MsgBox("This EOR have been rejected. Please contact IT for more info. ", this.Page, this);
                        return;
                    }

                    if (Status == "Approved" && GLOBAL.user_id != GLOBAL.AdminID)
                    {
                        Function_Method.MsgBox("This EOR have been approved. ", this.Page, this);
                        return;
                    }

                    if (Status != "Draft")
                    {
                        if (GLOBAL.user_id != GLOBAL.AdminID)
                        {
                            if (GLOBAL.logined_user_name != NextApprover)
                            {
                                if (GLOBAL.logined_user_name != NextApproverAlt)
                                {
                                    Function_Method.MsgBox("You are not next approval. Next Approver: " + NextApprover + " .Next ALT Approver: " + NextApproverAlt + ".", this.Page, this);
                                    return;
                                }
                            }
                        }
                    }
                    //if ((GLOBAL.user_authority_lvl != 1) || (GLOBAL.user_authority_lvl != 2))//not superadmin or admin
                    Session["data_passing"] = "@EOEO_" + selected_Equipment_Id + "|" + Status;//EOR>EOR
                    Response.Redirect("EOR.aspx");
                }

                catch (Exception ER_SF_12)
                {
                    Function_Method.MsgBox("ER_SF_12: " + ER_SF_12.ToString(), this.Page, this);
                }
                finally
                {
                    DynAx.Logoff();
                }
                //====================================================================
            }
        }

        private string UploadPicture()
        {
            string salesman = Label_Salesman.Text;
            string customerName = Label_CustName.Text;
            string path = "~/EOR/" + salesman + "/" + customerName;

            string signboard = ""; string external = ""; string internal1 = "";
            string internal2 = ""; string equip1 = ""; string equip2 = "";

            string error = "";
            if (!Directory.Exists(Server.MapPath("~/EOR/" + salesman)))
            {
                Directory.CreateDirectory(Server.MapPath("~/EOR/" + salesman));
                Directory.CreateDirectory(Server.MapPath(path));
            }
            else
            {
                if (!Directory.Exists(Server.MapPath(path)))
                {
                    Directory.CreateDirectory(Server.MapPath(path));
                }
            }

            if (fuSignboard.HasFile)
            {
                signboard = Path.Combine(Server.MapPath(path), fuSignboard.FileName);
                fuSignboard.SaveAs(signboard);
                signboard.Replace("/", "//");
            }

            if (fuExternal.HasFile)
            {
                external = Path.Combine(Server.MapPath(path), fuExternal.FileName);
                fuExternal.SaveAs(external);
                external.Replace("/", "//");
            }

            if (fuInternal1.HasFile)
            {
                internal1 = Path.Combine(Server.MapPath(path), fuInternal1.FileName);
                fuInternal1.SaveAs(internal1);
                internal1.Replace("/", "//");
            }

            if (fuInternal2.HasFile)
            {
                internal2 = Path.Combine(Server.MapPath(path), fuInternal2.FileName);
                fuInternal2.SaveAs(internal2);
                internal2.Replace("/", "//");
            }

            if (fuEquipment1.HasFile)
            {
                equip1 = Path.Combine(Server.MapPath(path), fuEquipment1.FileName);
                fuEquipment1.SaveAs(equip1);
                equip1.Replace("/", "//");
            }

            if (fuEquipment2.HasFile)
            {
                equip2 = Path.Combine(Server.MapPath(path), fuEquipment2.FileName);
                fuEquipment2.SaveAs(equip2);
                equip2.Replace("/", "//");
            }

            MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
            try
            {
                string query = "update eor_pic_tbl set(Salesman, CustomerName, SignboardPicture, ExternalPicture, " +
                    "InternalPicture1, InternalPicture2, EquipmentPicture1, EquipmentPicture2) values (@S1,@C2,@P1,@P2,@P3,@P4,@P5,@P6)";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                MySqlParameter _S1 = new MySqlParameter("@S1", MySqlDbType.VarChar, 0);
                _S1.Value = Label_Salesman.Text;
                cmd.Parameters.Add(_S1);

                MySqlParameter _C2 = new MySqlParameter("@C2", MySqlDbType.VarChar, 0);
                _C2.Value = Label_CustName.Text;
                cmd.Parameters.Add(_C2);

                MySqlParameter _P1 = new MySqlParameter("@P1", MySqlDbType.VarChar, 0);
                if (!string.IsNullOrEmpty(signboard))
                {
                    _P1.Value = path + "/" + fuSignboard.FileName;
                    cmd.Parameters.Add(_P1);
                }

                MySqlParameter _P2 = new MySqlParameter("@P2", MySqlDbType.VarChar, 0);
                if (!string.IsNullOrEmpty(external))
                {
                    _P2.Value = path + "/" + fuExternal.FileName;
                    cmd.Parameters.Add(_P2);
                }

                MySqlParameter _P3 = new MySqlParameter("@P3", MySqlDbType.VarChar, 0);
                if (!string.IsNullOrEmpty(internal1))
                {
                    _P3.Value = path + "/" + fuInternal1.FileName;
                    cmd.Parameters.Add(_P3);
                }

                MySqlParameter _P4 = new MySqlParameter("@P4", MySqlDbType.VarChar, 0);
                if (!string.IsNullOrEmpty(internal2))
                {
                    _P4.Value = path + "/" + fuInternal2.FileName;
                    cmd.Parameters.Add(_P4);
                }

                MySqlParameter _P5 = new MySqlParameter("@P5", MySqlDbType.VarChar, 0);
                if (!string.IsNullOrEmpty(equip1))
                {
                    _P5.Value = path + "/" + fuEquipment1.FileName;
                    cmd.Parameters.Add(_P5);
                }

                MySqlParameter _P6 = new MySqlParameter("@P6", MySqlDbType.VarChar, 0);
                if (!string.IsNullOrEmpty(equip2))
                {
                    _P6.Value = path + "/" + fuEquipment2.FileName;
                    cmd.Parameters.Add(_P6);
                }

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                return error;
            }
            catch (Exception ex)
            {
                error = "ER_EOR_00: " + ex.ToString();
                return error;
            }
        }

        private void getEorPicture(string Salesman, string CustomerName)
        {
            MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);

            string query = "select Salesman, CustomerName, SignboardPicture, ExternalPicture, InternalPicture1, InternalPicture2, " +
                "EquipmentPicture1, EquipmentPicture2 from eor_pic_tbl where Salesman=@S1 and CustomerName=@C2 limit 1";

            MySqlCommand cmd = new MySqlCommand(query, conn);

            MySqlParameter _S1 = new MySqlParameter("@S1", MySqlDbType.VarChar, 0);
            _S1.Value = Salesman;
            cmd.Parameters.Add(_S1);

            MySqlParameter _C2 = new MySqlParameter("@C2", MySqlDbType.VarChar, 0);
            _C2.Value = CustomerName;
            cmd.Parameters.Add(_C2);

            conn.Open();

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (reader.GetValue(1).ToString() == Label_CustName.Text)//check customer name
                    {
                        string Signboard = reader.GetValue(2).ToString();
                        imgSignboard.ImageUrl = Signboard;

                        string External = reader.GetValue(3).ToString();
                        imgExternal.ImageUrl = External;

                        string Internal1 = reader.GetValue(4).ToString();
                        imgInternal1.ImageUrl = Internal1;

                        string Internal2 = reader.GetValue(5).ToString();
                        imgInternal2.ImageUrl = Internal2;

                        string Equip1 = reader.GetValue(6).ToString();
                        imgEquipment1.ImageUrl = Equip1;

                        string Equip2 = reader.GetValue(7).ToString();
                        imgEquipment2.ImageUrl = Equip2;
                    }
                }
            }
        }
    }
}