using GLOBAL_FUNCTION;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;

namespace DotNet
{
    public partial class EventBudget : System.Web.UI.Page
    {
        protected void CheckAccInList(object sender, EventArgs e)
        {
            Session["data_passing"] = "_EBCM@";// EventBudget > CustomerMaster
            Response.Redirect("CustomerMaster.aspx");
        }

        protected void CheckAcc(object sender, EventArgs e)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();

            validate(DynAx);

            DynAx.Logoff();
        }
    }
}