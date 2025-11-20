using GLOBAL_FUNCTION;
using System.Web.UI.WebControls;
namespace DotNet
{
    public partial class SFA : System.Web.UI.Page
    {
        private bool promo_layer3_checking()
        {

            string temp_err_msg = "";
            //=========================================================================================================================
            if (Section1.Visible == true)//got section 1
            {
                if (GridView_PromoSection1_sum.Visible == true)
                {
                    hidden_ItemId_Sum_Section1.Text = "";
                    hidden_Qty_Sum_Section1.Text = "";
                    GridView selected_Grid = GridView_PromoSection1_sum;

                    int counter_row = selected_Grid.Rows.Count;
                    if (counter_row > 1)
                    {
                        if (Label_PromoSection1_Remaining.Text == "0")//correct amount
                        {
                            //ok to take value 
                            TextBox temp_TextBox_PromoQTYSec;
                            bool flag_init = true;
                            for (int i = 0; i < counter_row; i++)
                            {
                                temp_TextBox_PromoQTYSec = (selected_Grid.Rows[i].Cells[2].FindControl("TextBox_PromoQTYSec1_sum") as TextBox);
                                string temp = temp_TextBox_PromoQTYSec.Text;
                                if ((temp == "") || temp == "0")
                                {
                                    //do nothing
                                }
                                else
                                {
                                    if (flag_init == true)//first time
                                    {
                                        hidden_ItemId_Sum_Section1.Text = selected_Grid.Rows[i].Cells[1].Text;
                                        hidden_Qty_Sum_Section1.Text = temp_TextBox_PromoQTYSec.Text;
                                    }
                                    else// not first time
                                    {
                                        hidden_ItemId_Sum_Section1.Text = hidden_ItemId_Sum_Section1.Text + "|" + selected_Grid.Rows[i].Cells[8].Text;
                                        hidden_Qty_Sum_Section1.Text = hidden_Qty_Sum_Section1.Text + "|" + temp_TextBox_PromoQTYSec.Text;
                                    }
                                    flag_init = false;
                                }

                            }
                        }
                        else
                        {
                            temp_err_msg = temp_err_msg + "Remaining item in Section 1. ";
                        }
                    }
                    else
                    {//ok to for next
                        TextBox temp_TextBox_PromoQTYSec;
                        temp_TextBox_PromoQTYSec = (selected_Grid.Rows[0].Cells[2].FindControl("TextBox_PromoQTYSec1_sum") as TextBox);
                        hidden_ItemId_Sum_Section1.Text = selected_Grid.Rows[0].Cells[1].Text;
                        hidden_Qty_Sum_Section1.Text = temp_TextBox_PromoQTYSec.Text;
                    }
                }
                if (GridView_PromoSection1_fixed.Visible == true)
                {
                    hidden_ItemId_fixed_Section1.Text = "";
                    hidden_Qty_fixed_Section1.Text = "";

                    GridView selected_Grid = GridView_PromoSection1_fixed;
                    int counter_row = GridView_PromoSection1_fixed.Rows.Count;

                    bool temp_check = false;
                    for (int i = 0; i < counter_row; i++)
                    {
                        CheckBox CheckBox_c;
                        CheckBox_c = (selected_Grid.Rows[i].Cells[0].FindControl("chkRow_PromoSection1_fixed") as CheckBox);
                        if (CheckBox_c.Checked)//have checkbox
                        {
                            temp_check = true;
                            //ok to next
                            TextBox temp_TextBox_PromoQTYSec = (selected_Grid.Rows[i].Cells[2].FindControl("TextBox_PromoQTYSec1_fixed") as TextBox);
                            hidden_ItemId_fixed_Section1.Text = selected_Grid.Rows[i].Cells[1].Text;
                            hidden_Qty_fixed_Section1.Text = temp_TextBox_PromoQTYSec.Text;

                        }
                    }
                    if (temp_check != true)
                    {
                        temp_err_msg = temp_err_msg + "Untick checkbox in Section 1. ";
                    }
                }
            }
            //=========================================================================================================================
            if (Section2.Visible == true)//got section 2
            {
                if (GridView_PromoSection2_sum.Visible == true)
                {
                    hidden_ItemId_Sum_Section2.Text = "";
                    hidden_Qty_Sum_Section2.Text = "";

                    GridView selected_Grid = GridView_PromoSection2_sum;

                    int counter_row = selected_Grid.Rows.Count;
                    if (counter_row > 1)
                    {
                        if (Label_PromoSection2_Remaining.Text == "0")
                        {
                            //ok to take value 
                            TextBox temp_TextBox_PromoQTYSec;
                            bool flag_init = true;
                            for (int i = 0; i < counter_row; i++)
                            {
                                temp_TextBox_PromoQTYSec = (selected_Grid.Rows[i].Cells[2].FindControl("TextBox_PromoQTYSec2_sum") as TextBox);
                                string temp = temp_TextBox_PromoQTYSec.Text;
                                if ((temp == "") || temp == "0")
                                {
                                    //do nothing
                                }
                                else
                                {
                                    if (flag_init == true)//first time
                                    {
                                        hidden_ItemId_Sum_Section2.Text = selected_Grid.Rows[i].Cells[8].Text;
                                        hidden_Qty_Sum_Section2.Text = temp_TextBox_PromoQTYSec.Text;
                                    }
                                    else// not first time
                                    {
                                        hidden_ItemId_Sum_Section2.Text = hidden_ItemId_Sum_Section2.Text + "|" + selected_Grid.Rows[i].Cells[8].Text;
                                        hidden_Qty_Sum_Section2.Text = hidden_Qty_Sum_Section2.Text + "|" + temp_TextBox_PromoQTYSec.Text;
                                    }
                                    flag_init = false;
                                }
                            }
                        }
                        else
                        {
                            if (hidden_layer2_camp_type.Text != "7")//campaign 7 dont check
                            {
                                temp_err_msg = temp_err_msg + "Remaining item in Section 2. ";
                            }
                        }
                    }
                    else
                    {
                        //ok to for next
                        TextBox temp_TextBox_PromoQTYSec;
                        temp_TextBox_PromoQTYSec = (selected_Grid.Rows[0].Cells[2].FindControl("TextBox_PromoQTYSec2_sum") as TextBox);
                        hidden_ItemId_Sum_Section2.Text = selected_Grid.Rows[0].Cells[1].Text;
                        hidden_Qty_Sum_Section2.Text = temp_TextBox_PromoQTYSec.Text;
                    }
                }
                if (GridView_PromoSection2_fixed.Visible == true)
                {
                    hidden_ItemId_fixed_Section2.Text = "";
                    hidden_Qty_fixed_Section2.Text = "";

                    GridView selected_Grid = GridView_PromoSection2_fixed;
                    int counter_row = GridView_PromoSection2_fixed.Rows.Count;

                    bool temp_check = false;
                    for (int i = 0; i < counter_row; i++)
                    {
                        CheckBox CheckBox_c;
                        CheckBox_c = (selected_Grid.Rows[i].Cells[0].FindControl("chkRow_PromoSection2_fixed") as CheckBox);
                        if (CheckBox_c.Checked)//have checkbox
                        {
                            temp_check = true;
                            //ok to next
                            TextBox temp_TextBox_PromoQTYSec = (selected_Grid.Rows[i].Cells[2].FindControl("TextBox_PromoQTYSec2_fixed") as TextBox);
                            hidden_ItemId_fixed_Section2.Text = selected_Grid.Rows[i].Cells[8].Text;
                            hidden_Qty_fixed_Section2.Text = temp_TextBox_PromoQTYSec.Text;
                        }
                    }
                    if (temp_check != true)
                    {
                        if (hidden_layer2_camp_type.Text != "7")//campaign 7 dont check
                        {
                            temp_err_msg = temp_err_msg + "Untick checkbox in Section 2. ";
                        }
                    }
                }
            }
            //=========================================================================================================================
            if (Section3.Visible == true)//got section 3
            {
                if (GridView_PromoSection3_sum.Visible == true)
                {
                    hidden_ItemId_Sum_Section3.Text = "";
                    hidden_Qty_Sum_Section3.Text = "";

                    GridView selected_Grid = GridView_PromoSection3_sum;

                    int counter_row = selected_Grid.Rows.Count;
                    if (counter_row > 1)
                    {
                        if (Label_PromoSection3_Remaining.Text == "0")
                        {
                            //ok to take value 
                            TextBox temp_TextBox_PromoQTYSec;
                            bool flag_init = true;
                            for (int i = 0; i < counter_row; i++)
                            {
                                temp_TextBox_PromoQTYSec = (selected_Grid.Rows[i].Cells[2].FindControl("TextBox_PromoQTYSec3_sum") as TextBox);
                                string temp = temp_TextBox_PromoQTYSec.Text;
                                if ((temp == "") || temp == "0")
                                {
                                    //do nothing
                                }
                                else
                                {
                                    if (flag_init == true)//first time
                                    {
                                        hidden_ItemId_Sum_Section3.Text = selected_Grid.Rows[i].Cells[8].Text;
                                        hidden_Qty_Sum_Section3.Text = temp_TextBox_PromoQTYSec.Text;
                                    }
                                    else// not first time
                                    {
                                        hidden_ItemId_Sum_Section3.Text = hidden_ItemId_Sum_Section3.Text + "|" + selected_Grid.Rows[i].Cells[8].Text;
                                        hidden_Qty_Sum_Section3.Text = hidden_Qty_Sum_Section3.Text + "|" + temp_TextBox_PromoQTYSec.Text;
                                    }
                                    flag_init = false;
                                }
                            }
                        }
                        else
                        {
                            if (hidden_layer2_camp_type.Text != "7")//campaign 7 dont check
                            {
                                temp_err_msg = temp_err_msg + "Remaining item in Section 3. ";
                            }
                        }
                    }
                    else
                    {
                        //ok to for next
                        TextBox temp_TextBox_PromoQTYSec;
                        temp_TextBox_PromoQTYSec = (selected_Grid.Rows[0].Cells[2].FindControl("TextBox_PromoQTYSec3_sum") as TextBox);
                        hidden_ItemId_Sum_Section3.Text = selected_Grid.Rows[0].Cells[8].Text;
                        hidden_Qty_Sum_Section3.Text = temp_TextBox_PromoQTYSec.Text;
                    }
                }
                if (GridView_PromoSection3_fixed.Visible == true)
                {
                    GridView selected_Grid = GridView_PromoSection3_fixed;
                    int counter_row = GridView_PromoSection3_fixed.Rows.Count;

                    bool temp_check = false;
                    for (int i = 0; i < counter_row; i++)
                    {
                        CheckBox CheckBox_c;
                        CheckBox_c = (selected_Grid.Rows[i].Cells[0].FindControl("chkRow_PromoSection3_fixed") as CheckBox);
                        if (CheckBox_c.Checked)//have checkbox
                        {
                            temp_check = true;
                            //ok for next
                            TextBox temp_TextBox_PromoQTYSec = (selected_Grid.Rows[i].Cells[2].FindControl("TextBox_PromoQTYSec3_fixed") as TextBox);
                            hidden_ItemId_fixed_Section3.Text = selected_Grid.Rows[i].Cells[1].Text;
                            hidden_Qty_fixed_Section3.Text = temp_TextBox_PromoQTYSec.Text;
                        }
                    }
                    if (hidden_layer2_camp_type.Text != "7")//campaign 7 dont check
                    {
                        if (temp_check != true)
                        {
                            temp_err_msg = temp_err_msg + "Untick checkbox in Section 3. ";
                        }
                    }

                }
            }
            if (temp_err_msg == "")
            {
                hidden_Qty_fixed_Section1.Visible = true;
                hidden_Qty_fixed_Section2.Visible = true;
                hidden_Qty_fixed_Section3.Visible = true;
                hidden_ItemId_fixed_Section1.Visible = true;
                hidden_ItemId_fixed_Section2.Visible = true;
                hidden_ItemId_fixed_Section3.Visible = true;

                hidden_Qty_Sum_Section1.Visible = true;
                hidden_Qty_Sum_Section2.Visible = true;
                hidden_Qty_Sum_Section3.Visible = true;
                hidden_ItemId_Sum_Section1.Visible = true;
                hidden_ItemId_Sum_Section2.Visible = true;
                hidden_ItemId_Sum_Section3.Visible = true;
                return true;
            }
            else
            {
                Function_Method.MsgBox(temp_err_msg, this.Page, this);
                return false;
            }

        }
    }
}