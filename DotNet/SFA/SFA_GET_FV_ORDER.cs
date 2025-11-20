using Microsoft.Dynamics.BusinessConnectorNet;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
namespace DotNet
{
    public class SFA_GET_FV_ORDER
    {


        public static List<ListItem> get_NewUnit(Axapta DynAx, string temp_Unit, string temp_ItemId)
        {
            List<ListItem> List_NewUnit = new List<ListItem>();
            List_NewUnit.Add(new ListItem(temp_Unit));
            //
            int InventTableModule = 176;
            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", InventTableModule);

            var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", 1);//ItemId
            qbr1.Call("value", temp_ItemId);

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);
            while ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", InventTableModule);
                string ModuleType = DynRec1.get_Field("ModuleType").ToString();
                string Module = DynRec1.get_Field("UnitId").ToString();
                int counter_list = List_NewUnit.Count();

                for (int i = 0; i < counter_list; i++)
                {

                    //if(ModuleType!="2")
                    //{
                    if (Module != List_NewUnit[i].Text)
                    {
                        List_NewUnit.Add(new ListItem(Module));

                    }
                    else//exit loop
                    {
                        i = counter_list;
                    }
                    //}
                }


                DynRec1.Dispose();
            }
            return List_NewUnit;
        }
    }
}