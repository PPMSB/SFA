using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GLOBAL_VAR;
namespace DotNet
{
    public partial class Campaign_Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Campaign_CheckSession.SystemCheckSession(this.Page, this);

            MainMenu.Attributes.Remove("class");
            CreateNewOffer.Attributes.Remove("class");
            SubmitForm.Attributes.Remove("class");
            CampaignReport.Attributes.Remove("class");

            switch (this.Page.ToString())
            {
                case "ASP.campaign_mainmenu_aspx":
                    MainMenu.Attributes.Add("class", "CurrentPage");

                    break;

                case "ASP.campaign_createnewoffer_aspx":
                    CreateNewOffer.Attributes.Add("class", "CurrentPage");

                    break;

                case "ASP.campaign_submitform_aspx":
                    if (GLOBAL.CampaignReport == false)
                    {
                        if (Request.QueryString["download"] == "true")
                        {
                            DownloadForm.Attributes.Add("class", "CurrentPage");
                        }
                        else
                        {
                            SubmitForm.Attributes.Add("class", "CurrentPage");
                        }
                    }
                    else
                    {
                        CampaignReport.Attributes.Add("class", "CurrentPage");
                    }
                    break;
            }
        }
    }
}