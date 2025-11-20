using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace DotNet
{
    public partial class SFA : System.Web.UI.Page
    {
        //for layer 2
        private bool promo_layer2(Axapta DynAx, string SO_number, string camp_id, string camp_type)
        {
            try
            {
                Promotion_Layer1.Visible = false; Promotion_Layer2.Visible = true;
                var tuple_getPromotionId_getstmres = SFA_GET_PROMO_LAYER2.getPromotionId_getstmres(DynAx, SO_number, camp_id, camp_type);
                string getPromotionId = tuple_getPromotionId_getstmres.Item1;
                string getstmres = tuple_getPromotionId_getstmres.Item2;
                if (getPromotionId == "0")
                {
                    return false;//no promotion
                }

                var tuple_getPromotionId = SFA_GET_PROMO_LAYER2.GeneralSplit_TotalItem_id_level_qty_vtype_unitid_section(getPromotionId);

                int TotalItem = tuple_getPromotionId.Item1;
                string[] id = tuple_getPromotionId.Item2;       //ItemId 
                string[] level = tuple_getPromotionId.Item3;    //Item level
                string[] qty = tuple_getPromotionId.Item4;      //Quantity Entitle
                string[] vtype = tuple_getPromotionId.Item5;    //variable type, S:Sum F:Fixed
                string[] unitid = tuple_getPromotionId.Item6;
                string[] section = tuple_getPromotionId.Item7;
                //initialize

                Section1.Visible = false; Section2.Visible = false; Section3.Visible = false;

                GridView_PromoSection1_sum.DataSource = null; GridView_PromoSection1_sum.DataBind(); div_GridView_PromoSection1_sum.Visible = false;
                GridView_PromoSection1_fixed.DataSource = null; GridView_PromoSection1_fixed.DataBind(); div_GridView_PromoSection1_fixed.Visible = false;

                GridView_PromoSection2_sum.DataSource = null; GridView_PromoSection2_sum.DataBind(); div_GridView_PromoSection2_sum.Visible = false;
                GridView_PromoSection2_fixed.DataSource = null; GridView_PromoSection2_fixed.DataBind(); div_GridView_PromoSection2_fixed.Visible = false;

                GridView_PromoSection3_sum.DataSource = null; GridView_PromoSection3_sum.DataBind(); div_GridView_PromoSection3_sum.Visible = false;
                GridView_PromoSection3_fixed.DataSource = null; GridView_PromoSection3_fixed.DataBind(); div_GridView_PromoSection3_fixed.Visible = false;
                for (int k = 3; k < 10; k++)//show other column in the grid
                {
                    GridView_PromoSection1_sum.Columns[k].Visible = true; GridView_PromoSection1_fixed.Columns[k].Visible = true;
                    GridView_PromoSection2_sum.Columns[k].Visible = true; GridView_PromoSection2_fixed.Columns[k].Visible = true;
                    GridView_PromoSection3_sum.Columns[k].Visible = true; GridView_PromoSection3_fixed.Columns[k].Visible = true;
                }

                Label_PromoSection1_Total_Note.Text = ""; Label_PromoSection1_Remaining.Text = ""; Label_PromoSection1_Total.Text = "";
                Label_PromoSection2_Total_Note.Text = ""; Label_PromoSection2_Remaining.Text = ""; Label_PromoSection2_Total.Text = "";
                Label_PromoSection3_Total_Note.Text = ""; Label_PromoSection3_Remaining.Text = ""; Label_PromoSection3_Total.Text = "";
                //
                DataTable dt_Section1_sum = new DataTable();
                dt_Section1_sum.Columns.AddRange(new DataColumn[8] { new DataColumn("ItemId"), new DataColumn("Item"), new DataColumn("Quantity"), new DataColumn("unitid"), new DataColumn("FOCFQty"), new DataColumn("FOCPrice"), new DataColumn("FOCVol"), new DataColumn("FOCFQtyVol") });

                DataTable dt_Section1_fixed = new DataTable();
                dt_Section1_fixed.Columns.AddRange(new DataColumn[8] { new DataColumn("ItemId"), new DataColumn("Item"), new DataColumn("Quantity"), new DataColumn("unitid"), new DataColumn("FOCFQty"), new DataColumn("FOCPrice"), new DataColumn("FOCVol"), new DataColumn("FOCFQtyVol") });
                //
                //
                DataTable dt_Section2_sum = new DataTable();
                dt_Section2_sum.Columns.AddRange(new DataColumn[8] { new DataColumn("ItemId"), new DataColumn("Item"), new DataColumn("Quantity"), new DataColumn("unitid"), new DataColumn("FOCFQty"), new DataColumn("FOCPrice"), new DataColumn("FOCVol"), new DataColumn("FOCFQtyVol") });

                DataTable dt_Section2_fixed = new DataTable();
                dt_Section2_fixed.Columns.AddRange(new DataColumn[8] { new DataColumn("ItemId"), new DataColumn("Item"), new DataColumn("Quantity"), new DataColumn("unitid"), new DataColumn("FOCFQty"), new DataColumn("FOCPrice"), new DataColumn("FOCVol"), new DataColumn("FOCFQtyVol") });

                DataTable dt_Section3_sum = new DataTable();
                dt_Section3_sum.Columns.AddRange(new DataColumn[8] { new DataColumn("ItemId"), new DataColumn("Item"), new DataColumn("Quantity"), new DataColumn("unitid"), new DataColumn("FOCFQty"), new DataColumn("FOCPrice"), new DataColumn("FOCVol"), new DataColumn("FOCFQtyVol") });

                DataTable dt_Section3_fixed = new DataTable();
                dt_Section3_fixed.Columns.AddRange(new DataColumn[8] { new DataColumn("ItemId"), new DataColumn("Item"), new DataColumn("Quantity"), new DataColumn("unitid"), new DataColumn("FOCFQty"), new DataColumn("FOCPrice"), new DataColumn("FOCVol"), new DataColumn("FOCFQtyVol") });
                //  
                //
                string[] FOCItemName = new string[TotalItem]; string[] FOCFQty = new string[TotalItem];
                string[] FOCPrice = new string[TotalItem]; string[] FOCVol = new string[TotalItem];
                string[] FOCFQtyVol = new string[TotalItem];

                bool Sec1_sum = false; bool Sec1_fixed = false;
                bool Sec2_sum = false; bool Sec2_fixed = false;
                bool Sec3_sum = false; bool Sec3_fixed = false;

                int TotalFOCSec1 = 0; int TotalFOCSec2 = 0; int TotalFOCSec3 = 0;
                //
                //end initialize

                for (int i = 0; i < TotalItem; i++)
                {
                    /*start obtain and transfer to grid*/
                    FOCPrice[i] = SFA_GET_PROMO_LAYER2.GetPromoPrice(DynAx, SO_number, camp_id, id[i]);
                    FOCVol[i] = SFA_GET_PROMO_LAYER2.getVolQty(DynAx, camp_id, id[i]);
                    FOCFQtyVol[i] = SFA_GET_PROMO_LAYER2.getitemvol(DynAx, camp_id, id[i]);

                    if (section[i] == "1")
                    {
                        FOCItemName[i] = SFA_GET_PROMO_LAYER2.getItemNameFOC(DynAx, camp_id, id[i]);
                        FOCFQty[i] = "0";

                        Section1.Visible = true;

                        if (vtype[i] == "S")
                        {
                            Sec1_sum = true;
                            TotalFOCSec1 = TotalFOCSec1 + Convert.ToInt32(qty[i]);

                            dt_Section1_sum.Rows.Add(id[i], FOCItemName[i], /*level[i],*/ qty[i], /*vtype[i],*/ unitid[i], /*section[i],*/ FOCFQty[i], FOCPrice[i], FOCVol[i], FOCFQtyVol[i]);
                            GridView_PromoSection1_sum.DataSource = dt_Section1_sum;
                            GridView_PromoSection1_sum.DataBind();
                        }
                        else
                        {
                            Sec1_fixed = true;
                            dt_Section1_fixed.Rows.Add(id[i], FOCItemName[i], /*level[i],*/ qty[i], /*vtype[i],*/ unitid[i], /*section[i],*/ FOCFQty[i], FOCPrice[i], FOCVol[i], FOCFQtyVol[i]);
                            GridView_PromoSection1_fixed.DataSource = dt_Section1_fixed;
                            GridView_PromoSection1_fixed.DataBind();
                        }
                    }
                    else
                    {
                        FOCItemName[i] = SFA_GET_PROMO_LAYER2.getItemNameOption(DynAx, camp_id, id[i]);
                        FOCFQty[i] = qty[i];

                        if (section[i] == "2")
                        {
                            Section2.Visible = true;
                            if (vtype[i] == "S")
                            {
                                Sec2_sum = true; ;
                                TotalFOCSec2 = TotalFOCSec2 + Convert.ToInt32(qty[i]);
                                dt_Section2_sum.Rows.Add(id[i], FOCItemName[i], /*level[i],*/ qty[i], /*vtype[i],*/ unitid[i], /*section[i],*/ FOCFQty[i], FOCPrice[i], FOCVol[i], FOCFQtyVol[i]);
                                GridView_PromoSection2_sum.DataSource = dt_Section2_sum;
                                GridView_PromoSection2_sum.DataBind();
                            }
                            else
                            {
                                Sec2_fixed = true;
                                dt_Section2_fixed.Rows.Add(id[i], FOCItemName[i], /*level[i],*/ qty[i], /*vtype[i],*/ unitid[i], /*section[i],*/ FOCFQty[i], FOCPrice[i], FOCVol[i], FOCFQtyVol[i]);
                                GridView_PromoSection2_fixed.DataSource = dt_Section2_fixed;
                                GridView_PromoSection2_fixed.DataBind();
                            }
                        }
                        if (section[i] == "3")
                        {
                            Section3.Visible = true;
                            if (vtype[i] == "S")
                            {
                                Sec3_sum = true; ;
                                TotalFOCSec3 = TotalFOCSec3 + Convert.ToInt32(qty[i]);
                                dt_Section3_sum.Rows.Add(id[i], FOCItemName[i], /*level[i],*/ qty[i], /*vtype[i],*/ unitid[i], /*section[i],*/ FOCFQty[i], FOCPrice[i], FOCVol[i], FOCFQtyVol[i]);
                                GridView_PromoSection3_sum.DataSource = dt_Section3_sum;
                                GridView_PromoSection3_sum.DataBind();
                            }
                            else
                            {
                                Sec3_fixed = true; ;
                                dt_Section3_fixed.Rows.Add(id[i], FOCItemName[i], /*level[i],*/ qty[i], /*vtype[i],*/ unitid[i], /*section[i],*/ FOCFQty[i], FOCPrice[i], FOCVol[i], FOCFQtyVol[i]);
                                GridView_PromoSection3_fixed.DataSource = dt_Section3_fixed;
                                GridView_PromoSection3_fixed.DataBind();
                            }
                        }
                    }
                }

                Label_PromoSection1_Remaining.Text = TotalFOCSec1.ToString(); Label_PromoSection1_Total.Text = TotalFOCSec1.ToString();
                Label_PromoSection2_Remaining.Text = TotalFOCSec2.ToString(); Label_PromoSection2_Total.Text = TotalFOCSec2.ToString();
                Label_PromoSection3_Remaining.Text = TotalFOCSec3.ToString(); Label_PromoSection3_Total.Text = TotalFOCSec3.ToString();
                //
                if (Section1.Visible == true)//for section 1
                {
                    int selected_section = 1;

                    if (Sec1_sum == true)//Summation type for section 1
                    {
                        bool selection_sum = true;//true: sum 
                        GridView selected_Grid = GridView_PromoSection1_sum;
                        string selected_TextBox = "TextBox_PromoQTYSec1_sum";

                        if (Layer2_Control_section(selected_section, selected_Grid, selection_sum, selected_TextBox) == true)//more than one row
                        {
                            Label_PromoSection1_Remaining_Note.Visible = true; Label_PromoSection1_Remaining.Visible = true;
                        }
                        else
                        {
                            CheckBox CheckBox_c = (selected_Grid.Rows[0].Cells[0].FindControl("chkRow_PromoSection1_sum") as CheckBox);
                            CheckBox_c.Checked = true; CheckBox_c.Enabled = false;
                            Label_PromoSection1_Remaining_Note.Visible = false; Label_PromoSection1_Remaining.Visible = false;
                        }
                    }
                    if (Sec1_fixed == true)//Fixed type for section 1
                    {
                        bool selection_sum = false;//false:fixed
                        GridView selected_Grid = GridView_PromoSection1_fixed;
                        string selected_TextBox = "TextBox_PromoQTYSec1_fixed";
                        if (Layer2_Control_section(selected_section, selected_Grid, selection_sum, selected_TextBox) == true)//more than one row
                        {
                            Label_vtype_Sec1_fixed.Text = "*Note: Tick on the checkbox. Only one checkbox allowed."; Label_vtype_Sec1_fixed.Visible = true;
                        }
                        else
                        {
                            Label_vtype_Sec1_fixed.Text = ""; Label_vtype_Sec1_fixed.Visible = false;

                            CheckBox CheckBox_c = (selected_Grid.Rows[0].Cells[0].FindControl("chkRow_PromoSection1_fixed") as CheckBox);
                            CheckBox_c.Checked = true; CheckBox_c.Enabled = false;
                        }
                    }
                }
                if (Section2.Visible == true)//for section 2
                {
                    int selected_section = 2;

                    if (Sec2_sum == true)//Summation type for section 2
                    {
                        bool selection_sum = true;//true: sum 
                        GridView selected_Grid = GridView_PromoSection2_sum;
                        string selected_TextBox = "TextBox_PromoQTYSec2_sum";

                        if (Layer2_Control_section(selected_section, selected_Grid, selection_sum, selected_TextBox) == true)//more than one row
                        {
                            Label_PromoSection2_Remaining_Note.Visible = true; Label_PromoSection2_Remaining.Visible = true;
                        }
                        else
                        {
                            CheckBox CheckBox_c = (selected_Grid.Rows[0].Cells[0].FindControl("chkRow_PromoSection2_sum") as CheckBox);
                            if (hidden_layer2_camp_type.Text == "7")// only promo7
                            {
                                CheckBox_c.Checked = false; CheckBox_c.Enabled = true;
                                Label_PromoSection2_Remaining_Note.Visible = true; Label_PromoSection2_Remaining.Visible = true;
                            }
                            else
                            {
                                CheckBox_c.Checked = true; CheckBox_c.Enabled = false;
                                Label_PromoSection2_Remaining_Note.Visible = false; Label_PromoSection2_Remaining.Visible = false;
                            }
                        }
                    }
                    if (Sec2_fixed == true)//Fixed type for section 2
                    {
                        bool selection_sum = false;//false:fixed
                        GridView selected_Grid = GridView_PromoSection2_fixed;
                        string selected_TextBox = "TextBox_PromoQTYSec2_fixed";

                        if (Layer2_Control_section(selected_section, selected_Grid, selection_sum, selected_TextBox) == true)//more than one row
                        {
                            Label_vtype_Sec2_fixed.Text = "*Note: Tick on the checkbox. Only one checkbox allowed."; Label_vtype_Sec2_fixed.Visible = true;
                        }
                        else
                        {
                            if (hidden_layer2_camp_type.Text == "7")// only type 7 promo
                            {
                                Label_vtype_Sec2_fixed.Text = ""; Label_vtype_Sec2_fixed.Visible = false;
                                CheckBox CheckBox_c = (selected_Grid.Rows[0].Cells[0].FindControl("chkRow_PromoSection2_fixed") as CheckBox);
                                CheckBox_c.Checked = false; CheckBox_c.Enabled = true;
                            }
                            else
                            {
                                Label_vtype_Sec2_fixed.Text = ""; Label_vtype_Sec2_fixed.Visible = false;
                                CheckBox CheckBox_c = (selected_Grid.Rows[0].Cells[0].FindControl("chkRow_PromoSection2_fixed") as CheckBox);
                                CheckBox_c.Checked = true; CheckBox_c.Enabled = false;

                            }
                        }
                    }
                }
                if (Section3.Visible == true)//for section 3
                {
                    int selected_section = 3;

                    if (Sec3_sum == true)//Summation type for section 3
                    {
                        bool selection_sum = true;//true: sum 
                        GridView selected_Grid = GridView_PromoSection3_sum;
                        string selected_TextBox = "TextBox_PromoQTYSec3_sum";

                        if (Layer2_Control_section(selected_section, selected_Grid, selection_sum, selected_TextBox) == true)//more than one row
                        {
                            Label_PromoSection3_Remaining_Note.Visible = true; Label_PromoSection3_Remaining.Visible = true;
                        }
                        else
                        {
                            CheckBox CheckBox_c = (selected_Grid.Rows[0].Cells[0].FindControl("chkRow_PromoSection3_sum") as CheckBox);
                            CheckBox_c.Checked = true; CheckBox_c.Enabled = false;
                            Label_PromoSection3_Remaining_Note.Visible = false; Label_PromoSection3_Remaining.Visible = false;
                        }
                    }
                    if (Sec3_fixed == true)//Fixed type for section3
                    {
                        bool selection_sum = false;//false:fixed
                        GridView selected_Grid = GridView_PromoSection3_fixed;
                        string selected_TextBox = "TextBox_PromoQTYSec3_fixed";

                        if (Layer2_Control_section(selected_section, selected_Grid, selection_sum, selected_TextBox) == true)//more than one row
                        {
                            Label_vtype_Sec3_fixed.Text = "*Note: Tick on the checkbox. Only one checkbox allowed."; Label_vtype_Sec3_fixed.Visible = true;
                        }
                        else
                        {
                            Label_vtype_Sec3_fixed.Text = ""; Label_vtype_Sec3_fixed.Visible = false;

                            CheckBox CheckBox_c = (selected_Grid.Rows[0].Cells[0].FindControl("chkRow_PromoSection3_fixed") as CheckBox);
                            CheckBox_c.Checked = true; CheckBox_c.Enabled = false;
                        }
                    }
                }

                /*===================================================================================================================================*/

                if ((camp_type == "6") || (camp_type == "7"))
                {
                    Label_PromoSection1_Total_Note.Text = "Total Entitlement Item: ";
                    Label_PromoSection2_Total_Note.Text = "Total Entitlement Item: ";
                    Label_PromoSection3_Total_Note.Text = "Total Entitlement Item: ";
                }
                else
                {
                    Label_PromoSection1_Total_Note.Text = "Total Qty Voucher: ";
                    Label_PromoSection2_Total_Note.Text = "Total Qty Voucher: ";
                    Label_PromoSection3_Total_Note.Text = "Total Qty Voucher: ";
                }
                //
                /*===================================================================================================================================*/
                //visibility control
                /*===================================================================================================================================*/
                //section 1
                if (Section1.Visible == true)
                {
                    if (Sec1_sum == true)//Summation type for section 1
                    {
                        div_GridView_PromoSection1_sum.Visible = true;
                    }
                    else
                    {
                        div_GridView_PromoSection1_sum.Visible = false;
                    }
                    if (Sec1_fixed == true)//fixed type for section 1
                    {
                        div_GridView_PromoSection1_fixed.Visible = true;
                    }
                    else
                    {
                        div_GridView_PromoSection1_fixed.Visible = false;
                    }
                }

                //section 2
                if (Section2.Visible == true)
                {
                    if ((camp_type == "6") && (Sec2_sum == true) && (Sec2_fixed == true))//Type 6, and Grid_sum and Grid_fixed have value
                    {
                        Type6_Option.Visible = true;

                        //default, option A selected
                        div_GridView_PromoSection2_sum.Visible = true;
                        div_GridView_PromoSection2_fixed.Visible = false;
                        ImageButtonOptionB.Attributes.Add("style", "opacity:0.3"); ImageButtonOptionB_h.Attributes.Add("style", "opacity:0.3");
                        ImageButtonOptionA.Attributes.Add("style", "opacity:1"); ImageButtonOptionA_h.Attributes.Add("style", "opacity:1");
                    }
                    else
                    {
                        Type6_Option.Visible = false;
                    }
                    if (Type6_Option.Visible == false)
                    {
                        if (Sec2_sum == true)//Summation type for section 2
                        {
                            div_GridView_PromoSection2_sum.Visible = true;
                        }
                        else
                        {
                            div_GridView_PromoSection2_sum.Visible = false;
                        }
                        if (Sec2_fixed == true)//fixed type for section 2
                        {
                            div_GridView_PromoSection2_fixed.Visible = true;
                        }
                        else
                        {
                            div_GridView_PromoSection2_fixed.Visible = false;
                        }
                    }
                }

                //section 3
                if (Section3.Visible == true)
                {
                    if (Sec3_sum == true)//Summation type for section 3
                    {
                        div_GridView_PromoSection3_sum.Visible = true;
                    }
                    else
                    {
                        div_GridView_PromoSection3_sum.Visible = false;
                    }
                    if (Sec3_fixed == true)//fixed type for section 3
                    {
                        div_GridView_PromoSection3_fixed.Visible = true;
                    }
                    else
                    {
                        div_GridView_PromoSection3_fixed.Visible = false;
                    }
                }
                /*===================================================================================================================================*/

                for (int k = 3; k < 10; k++)//hide other column in the grid
                {
                    if (GLOBAL.debug == true)
                    {
                        GridView_PromoSection1_sum.Columns[k].Visible = true; GridView_PromoSection1_fixed.Columns[k].Visible = true;
                        GridView_PromoSection2_sum.Columns[k].Visible = true; GridView_PromoSection2_fixed.Columns[k].Visible = true;
                        GridView_PromoSection3_sum.Columns[k].Visible = true; GridView_PromoSection3_fixed.Columns[k].Visible = true;
                    }
                    else
                    {
                        GridView_PromoSection1_sum.Columns[k].Visible = false; GridView_PromoSection1_fixed.Columns[k].Visible = false;
                        GridView_PromoSection2_sum.Columns[k].Visible = false; GridView_PromoSection2_fixed.Columns[k].Visible = false;
                        GridView_PromoSection3_sum.Columns[k].Visible = false; GridView_PromoSection3_fixed.Columns[k].Visible = false;
                    }
                }
                GridView_PromoSection3_fixed.Columns[4].Visible = true;// requested by Keegan not to show
                return true;
                /*===================================================================================================================================*/
            }
            catch (Exception ER_SF_21)
            {
                Function_Method.MsgBox("ER_SF_21: " + ER_SF_21.Message, this.Page, this);
                return false;
            }
        }

        protected void ImageButtonOptionA_Click(object sender, ImageClickEventArgs e)
        {
            div_GridView_PromoSection2_sum.Visible = true;
            div_GridView_PromoSection2_fixed.Visible = false;
            ImageButtonOptionB.Attributes.Add("style", "opacity:0.3"); ImageButtonOptionB_h.Attributes.Add("style", "opacity:0.3");
            ImageButtonOptionA.Attributes.Add("style", "opacity:1"); ImageButtonOptionA_h.Attributes.Add("style", "opacity:1");
        }

        protected void ImageButtonOptionB_Click(object sender, ImageClickEventArgs e)
        {
            div_GridView_PromoSection2_sum.Visible = false;
            div_GridView_PromoSection2_fixed.Visible = true;
            ImageButtonOptionA.Attributes.Add("style", "opacity:0.3"); ImageButtonOptionA_h.Attributes.Add("style", "opacity:0.3");
            ImageButtonOptionB.Attributes.Add("style", "opacity:1"); ImageButtonOptionB_h.Attributes.Add("style", "opacity:1");
        }

        private bool Layer2_Control_section(int section, GridView selected_Grid, bool selection_sum, string selected_TextBox)
        {
            if (selection_sum == true)//Summation type
            {
                if (selected_Grid.Rows.Count > 1)
                {
                    TextBox temp_TextBox_PromoQTYSec = (selected_Grid.Rows[0].Cells[2].FindControl(selected_TextBox) as TextBox);
                    temp_TextBox_PromoQTYSec.Enabled = true;
                    temp_TextBox_PromoQTYSec.Text = "";
                    return true;//more than row=true

                }
                else//only one item in Summation type for section 2
                {
                    TextBox temp_TextBox_PromoQTYSec = (selected_Grid.Rows[0].Cells[2].FindControl(selected_TextBox) as TextBox);
                    if (hidden_layer2_camp_type.Text == "7" && section != 1)// only promo7
                    {
                        temp_TextBox_PromoQTYSec.Enabled = true;
                        temp_TextBox_PromoQTYSec.Text = "";
                        return false;
                    }
                    else
                    {
                        temp_TextBox_PromoQTYSec.Enabled = false;
                        temp_TextBox_PromoQTYSec.Text = selected_Grid.Rows[0].Cells[4].Text;
                        selected_Grid.Rows[0].BackColor = Color.FromName("#ff8000");
                        return false;
                    }
                }
            }
            else//Fixed type
            {
                if (selected_Grid.Rows.Count > 1)
                {
                    return true;//more than one row=true
                }
                else//only one item in fixed type for section 
                {
                    TextBox temp_TextBox_PromoQTYSec = (selected_Grid.Rows[0].Cells[2].FindControl(selected_TextBox) as TextBox);
                    if (hidden_layer2_camp_type.Text == "7" && section != 1) // only promo7
                    {
                        temp_TextBox_PromoQTYSec.Enabled = true;
                        temp_TextBox_PromoQTYSec.Text = "";

                        return false;
                    }
                    else
                    {
                        temp_TextBox_PromoQTYSec.Enabled = false;
                        temp_TextBox_PromoQTYSec.Text = selected_Grid.Rows[0].Cells[4].Text;
                        //selected_Grid.Rows[0].BackColor = Color.FromName("#ff8000");
                        return false;
                    }
                }
            }
        }
        //========================================================================================
        private void Global_TextBox_Promo_change(object sender, GridView selected_Grid, int Section)
        {
            //get the the id that call the function
            TextBox TextBox_PromoQTY = sender as TextBox;
            string ClientID = TextBox_PromoQTY.ClientID;
            string[] arr_ClientID = ClientID.Split('_');
            int arr_count = arr_ClientID.Count();
            int ClientRow = Convert.ToInt32(arr_count - 1);
            //

            int Qty_keyin = 0; int Total_Qty_keyin = 0; int Total_Qty_keyin_Previous = 0; int selected_Total = 0; Label selected_Remaining_Label;
            CheckBox CheckBox_c; TextBox temp_TextBox_PromoQTYSec;
            int row_count = selected_Grid.Rows.Count;

            //arrange so that clientRow is last row
            int[] arranged_row = new int[row_count];

            arranged_row[row_count - 1] = ClientRow;
            int temp_count = 0;
            for (int i = 0; i < row_count; i++)
            {
                if (i != ClientRow)
                {
                    arranged_row[temp_count] = i;
                    temp_count = temp_count + 1;
                }
            }

            for (int j = 0; j < row_count; j++)
            {
                int i = arranged_row[j];

                if (selected_Grid.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    if (Section == 1)
                    {
                        temp_TextBox_PromoQTYSec = (selected_Grid.Rows[i].Cells[2].FindControl("TextBox_PromoQTYSec1_sum") as TextBox);
                        CheckBox_c = (selected_Grid.Rows[i].Cells[0].FindControl("chkRow_PromoSection1_sum") as CheckBox);
                        if (temp_TextBox_PromoQTYSec.Text == "" || temp_TextBox_PromoQTYSec.Text == "0")
                        {
                            temp_TextBox_PromoQTYSec.Text = "";
                            CheckBox_c.Checked = false;
                            Qty_keyin = 0;
                        }
                        else
                        {
                            Qty_keyin = Convert.ToInt32(temp_TextBox_PromoQTYSec.Text);
                        }

                        selected_Remaining_Label = Label_PromoSection1_Remaining;
                        selected_Total = Convert.ToInt32(Label_PromoSection1_Total.Text);
                    }
                    else if (Section == 2)
                    {
                        temp_TextBox_PromoQTYSec = (selected_Grid.Rows[i].Cells[2].FindControl("TextBox_PromoQTYSec2_sum") as TextBox);
                        CheckBox_c = (selected_Grid.Rows[i].Cells[0].FindControl("chkRow_PromoSection2_sum") as CheckBox);
                        if (temp_TextBox_PromoQTYSec.Text == "" || temp_TextBox_PromoQTYSec.Text == "0")
                        {
                            temp_TextBox_PromoQTYSec.Text = "";
                            CheckBox_c.Checked = false;
                            Qty_keyin = 0;
                        }
                        else
                        {
                            Qty_keyin = Convert.ToInt32(temp_TextBox_PromoQTYSec.Text);
                        }

                        selected_Remaining_Label = Label_PromoSection2_Remaining;
                        selected_Total = Convert.ToInt32(Label_PromoSection2_Total.Text);
                    }
                    else// (Section == 3)
                    {
                        temp_TextBox_PromoQTYSec = (selected_Grid.Rows[i].Cells[2].FindControl("TextBox_PromoQTYSec3_sum") as TextBox);
                        CheckBox_c = (selected_Grid.Rows[i].Cells[0].FindControl("chkRow_PromoSection3_sum") as CheckBox);
                        if (temp_TextBox_PromoQTYSec.Text == "" || temp_TextBox_PromoQTYSec.Text == "0")
                        {
                            temp_TextBox_PromoQTYSec.Text = "";
                            CheckBox_c.Checked = false;
                            Qty_keyin = 0;
                        }
                        else
                        {
                            Qty_keyin = Convert.ToInt32(temp_TextBox_PromoQTYSec.Text);
                        }
                        selected_Remaining_Label = Label_PromoSection3_Remaining;
                        selected_Total = Convert.ToInt32(Label_PromoSection3_Total.Text);
                    }

                    if (i != ClientRow)//not last row, clientrow
                    {
                        Total_Qty_keyin = Total_Qty_keyin + Qty_keyin;
                    }
                    else//last row, ClientRow
                    {
                        Total_Qty_keyin_Previous = Total_Qty_keyin;
                        Total_Qty_keyin = Total_Qty_keyin + Qty_keyin;

                        if (Total_Qty_keyin > selected_Total)//exceeded
                        {
                            Function_Method.MsgBox("The quantity had exceeded. Quantity corrected to correct ratio", this.Page, this);
                            int temp_remaining = selected_Total - Total_Qty_keyin_Previous;
                            temp_TextBox_PromoQTYSec.Text = temp_remaining.ToString();
                            CheckBox_c.Checked = true;
                            selected_Remaining_Label.Text = "0";
                        }
                        else//not exceed
                        {
                            int temp_remaining = selected_Total - Total_Qty_keyin;
                            selected_Remaining_Label.Text = temp_remaining.ToString();
                            if (temp_TextBox_PromoQTYSec.Text != "")
                            {
                                CheckBox_c.Checked = true;
                            }
                            else
                            {
                                CheckBox_c.Checked = false;
                            }
                        }
                    }
                }
            }
        }
        //========================================================================================
        protected void TextBox_PromoQTYSec1_sum_changed(object sender, EventArgs e)
        {
            int Section = 1;
            Global_TextBox_Promo_change(sender, GridView_PromoSection1_sum, Section);
        }
        protected void TextBox_PromoQTYSec2_sum_changed(object sender, EventArgs e)
        {
            int Section = 2;
            Global_TextBox_Promo_change(sender, GridView_PromoSection2_sum, Section);
        }
        protected void TextBox_PromoQTYSec3_sum_changed(object sender, EventArgs e)
        {
            int Section = 3;
            Global_TextBox_Promo_change(sender, GridView_PromoSection3_sum, Section);
        }
        //========================================================================================
        protected void CheckBox_PromoSection1_fixed(object sender, EventArgs e)//vytpe="F", fixed
        {
            int Section = 1;
            Global_CheckBox_Promo_fixed(sender, GridView_PromoSection1_fixed, Section);
        }

        protected void CheckBox_PromoSection2_fixed(object sender, EventArgs e)
        {
            int Section = 2;
            Global_CheckBox_Promo_fixed(sender, GridView_PromoSection2_fixed, Section);
        }

        protected void CheckBox_PromoSection3_fixed(object sender, EventArgs e)
        {
            int Section = 3;
            Global_CheckBox_Promo_fixed(sender, GridView_PromoSection3_fixed, Section);
        }

        protected void CheckBox_PromoSection1_sum(object sender, EventArgs e)
        {
            int Section = 1;
            Global_CheckBox_Promo_sum(sender, GridView_PromoSection1_sum, Section);
        }

        protected void CheckBox_PromoSection2_sum(object sender, EventArgs e)
        {
            int Section = 2;
            Global_CheckBox_Promo_sum(sender, GridView_PromoSection2_sum, Section);
        }

        protected void CheckBox_PromoSection3_sum(object sender, EventArgs e)
        {
            int Section = 3;
            Global_CheckBox_Promo_sum(sender, GridView_PromoSection3_sum, Section);
        }

        private void Global_CheckBox_Promo_fixed(object sender, GridView selected_Grid, int Section)
        {
            try
            {
                //get the the id that call the function
                CheckBox CheckBox_selection = sender as CheckBox;
                string ClientID = CheckBox_selection.ClientID;
                string[] arr_ClientID = ClientID.Split('_');
                GridViewRow row = (GridViewRow)CheckBox_selection.NamingContainer;
                int index = row.RowIndex;
                //int arr_count = arr_ClientID.Count();
                //int ClientRow = Convert.ToInt32(arr_ClientID[arr_count - 1]);

                int row_count = selected_Grid.Rows.Count;
                for (int i = 0; i < row_count; i++)
                {
                    CheckBox CheckBox_c;
                    TextBox temp_TextBox_PromoQTYSec;

                    if (Section == 1)
                    {
                        CheckBox_c = (selected_Grid.Rows[i].Cells[0].FindControl("chkRow_PromoSection1_fixed") as CheckBox);
                        temp_TextBox_PromoQTYSec = (selected_Grid.Rows[i].Cells[2].FindControl("TextBox_PromoQTYSec1_fixed") as TextBox);
                    }
                    else if (Section == 2)
                    {
                        CheckBox_c = (selected_Grid.Rows[i].Cells[0].FindControl("chkRow_PromoSection2_fixed") as CheckBox);
                        temp_TextBox_PromoQTYSec = (selected_Grid.Rows[i].Cells[2].FindControl("TextBox_PromoQTYSec2_fixed") as TextBox);
                    }
                    else// (Section == 3)
                    {
                        CheckBox_c = (selected_Grid.Rows[i].Cells[0].FindControl("chkRow_PromoSection3_fixed") as CheckBox);
                        temp_TextBox_PromoQTYSec = (selected_Grid.Rows[i].Cells[2].FindControl("TextBox_PromoQTYSec3_fixed") as TextBox);
                    }

                    if (selected_Grid.Rows[i].RowType == DataControlRowType.DataRow)
                    {
                        if (i != index)//allow only one selection
                        {
                            CheckBox_c.Checked = false;
                            temp_TextBox_PromoQTYSec.Text = "";
                        }

                        if (CheckBox_c.Checked)//highlight
                        {
                            temp_TextBox_PromoQTYSec.Text = selected_Grid.Rows[i].Cells[4].Text;
                            selected_Grid.Rows[i].BackColor = Color.FromName("#ff8000");
                        }
                        else
                        {
                            temp_TextBox_PromoQTYSec.Text = "";
                        }
                    }
                }
            }
            catch (Exception ER_SF_15)
            {
                Function_Method.MsgBox("ER_SF_15: " + ER_SF_15.ToString(), this.Page, this);
            }
        }

        private void Global_CheckBox_Promo_sum(object sender, GridView selected_Grid, int Section)
        {
            CheckBox CheckBox_select = sender as CheckBox;

            GridViewRow row = (GridViewRow)CheckBox_select.NamingContainer;
            try
            {
                int count_check = 0; bool remainder = false; bool flag_counted = false; int checkbox_quantity_each = 0; int temp_PromoQTY_Update = 0; int temp_last_CheckBox_Checked = 0;
                int row_count = selected_Grid.Rows.Count;

                int selected_Remaining = 0;
                int selected_Total = 0;

            ENTER_VALUE: for (int i = 0; i < row_count; i++)
                {
                    CheckBox CheckBox_c;
                    TextBox temp_TextBox_PromoQTYSec;

                    if (Section == 1)
                    {
                        CheckBox_c = (selected_Grid.Rows[i].Cells[0].FindControl("chkRow_PromoSection1_sum") as CheckBox);
                        //if (GridView_PromoSection1_sum.Rows[i].RowType == DataControlRowType.DataRow)
                        //{
                        //    if (i != index)
                        //    {
                        //        CheckBox chkbox = ((CheckBox)(GridView_PromoSection1_sum.Rows[i].FindControl("chkRow_PromoSection1_sum")));
                        //        CheckBox_select.Checked = false;
                        //    }
                        //}
                        temp_TextBox_PromoQTYSec = (selected_Grid.Rows[i].Cells[2].FindControl("TextBox_PromoQTYSec1_sum") as TextBox);
                        selected_Total = Convert.ToInt32(Label_PromoSection1_Total.Text);
                        selected_Remaining = Convert.ToInt32(Label_PromoSection1_Remaining.Text);
                    }
                    else if (Section == 2)
                    {
                        CheckBox_c = (selected_Grid.Rows[i].Cells[0].FindControl("chkRow_PromoSection2_sum") as CheckBox);
                        temp_TextBox_PromoQTYSec = (selected_Grid.Rows[i].Cells[2].FindControl("TextBox_PromoQTYSec2_sum") as TextBox);
                        selected_Total = Convert.ToInt32(Label_PromoSection2_Total.Text);
                        selected_Remaining = Convert.ToInt32(Label_PromoSection2_Remaining.Text);
                    }
                    else// (Section == 3)
                    {
                        CheckBox_c = (selected_Grid.Rows[i].Cells[0].FindControl("chkRow_PromoSection3_sum") as CheckBox);
                        temp_TextBox_PromoQTYSec = (selected_Grid.Rows[i].Cells[2].FindControl("TextBox_PromoQTYSec3_sum") as TextBox);
                        selected_Total = Convert.ToInt32(Label_PromoSection3_Total.Text);
                        selected_Remaining = Convert.ToInt32(Label_PromoSection3_Remaining.Text);
                    }

                    if (selected_Grid.Rows[i].RowType == DataControlRowType.DataRow)
                    {
                        if (flag_counted == true)//already counted
                        {
                            if (CheckBox_c.Checked)//highlight
                            {
                                if (remainder == false)
                                {
                                    temp_TextBox_PromoQTYSec.Text = checkbox_quantity_each.ToString();
                                }
                                else//(remainder==true)
                                {
                                    temp_TextBox_PromoQTYSec.Text = checkbox_quantity_each.ToString();
                                    temp_last_CheckBox_Checked = i;
                                    if (temp_TextBox_PromoQTYSec.Text == "0")
                                    {
                                        CheckBox_c.Checked = false;
                                    }
                                }
                                temp_PromoQTY_Update = temp_PromoQTY_Update + checkbox_quantity_each;
                            }
                        }
                        else
                        {
                            //clear all the textbox
                            temp_TextBox_PromoQTYSec.Text = "";
                            if (CheckBox_c.Checked)//highlight
                            {
                                count_check = count_check + 1;
                            }
                        }
                    }
                }

                if (flag_counted == true)
                {
                    int ideal_Remaining = 0;
                    int actual_Remaining = selected_Total - temp_PromoQTY_Update;
                    TextBox temp_TextBox_PromoQTYSec;

                    if (Section == 1)
                    {
                        temp_TextBox_PromoQTYSec = (selected_Grid.Rows[temp_last_CheckBox_Checked].Cells[2].FindControl("TextBox_PromoQTYSec1_sum") as TextBox);
                        Label_PromoSection1_Remaining.Text = "0";
                    }
                    else if (Section == 2)
                    {
                        temp_TextBox_PromoQTYSec = (selected_Grid.Rows[temp_last_CheckBox_Checked].Cells[2].FindControl("TextBox_PromoQTYSec2_sum") as TextBox);
                        Label_PromoSection2_Remaining.Text = "0";
                    }
                    else// (Section == 3)
                    {
                        temp_TextBox_PromoQTYSec = (selected_Grid.Rows[temp_last_CheckBox_Checked].Cells[2].FindControl("TextBox_PromoQTYSec3_sum") as TextBox);
                        Label_PromoSection3_Remaining.Text = "0";
                    }

                    if (ideal_Remaining != actual_Remaining)//not match
                    {
                        string temp_last_CheckBox_Checked_value = temp_TextBox_PromoQTYSec.Text;
                        temp_last_CheckBox_Checked_value = temp_last_CheckBox_Checked_value + actual_Remaining;
                        temp_TextBox_PromoQTYSec.Text = temp_last_CheckBox_Checked_value.ToString();
                    }
                }

                else// (flag_counted==false)
                {
                    if (count_check != 0)
                    {
                        if (selected_Total % count_check == 0)//no remainder
                        {
                            remainder = false;
                        }
                        else
                        {
                            remainder = true;
                        }
                        checkbox_quantity_each = selected_Total / count_check;
                        flag_counted = true;
                        goto ENTER_VALUE;
                    }
                    else//no checkbox
                    {
                        if (Section == 1)
                        {
                            Label_PromoSection1_Remaining.Text = Label_PromoSection1_Total.Text;
                        }
                        else if (Section == 2)
                        {
                            Label_PromoSection2_Remaining.Text = Label_PromoSection2_Total.Text;
                        }
                        else// (Section == 3)
                        {
                            Label_PromoSection3_Remaining.Text = Label_PromoSection3_Total.Text;
                        }
                    }
                }
            }
            catch (Exception ER_SF_20)
            {
                Function_Method.MsgBox("ER_SF_20: " + ER_SF_20.ToString(), this.Page, this);
            }
        }
    }
}