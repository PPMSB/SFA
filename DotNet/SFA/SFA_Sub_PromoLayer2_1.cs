using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Web.UI.WebControls;
namespace DotNet
{
    public partial class SFA : System.Web.UI.Page
    {
        private Tuple<bool, string, string, string> promo_layer2_checking()
        {
            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            try
            {
                foreach (GridViewRow row in GridView_Promotion_Campaign.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox CheckBox_selection = (row.Cells[0].FindControl("chkRow_Promotion_Campaign") as CheckBox);
                        if (CheckBox_selection.Checked)
                        {
                            string SO_number = hidden_Label_SO_No.Text;
                            string camp_id = row.Cells[1].Text;
                            string camp_type = row.Cells[3].Text;

                            hidden_layer2_camp_type.Text = camp_type; bool promo_exist = promo_layer2(DynAx, SO_number, camp_id, camp_type);

                            if (promo_exist == false)
                            {
                                Promotion_Layer1.Visible = true; Promotion_Layer2.Visible = false;
                                return new Tuple<bool, string, string, string>(false, SO_number, camp_id, camp_type);
                            }
                            return new Tuple<bool, string, string, string>(true, SO_number, camp_id, camp_type);
                        }
                    }
                }
                return new Tuple<bool, string, string, string>(false, "", "", "");
            }
            catch (Exception ER_SF_13)
            {
                Function_Method.MsgBox("ER_SF_13: " + ER_SF_13.ToString(), this.Page, this);
                return new Tuple<bool, string, string, string>(false, "", "", "");
            }
            finally
            {
                DynAx.Logoff();
            }

        }
    }
}