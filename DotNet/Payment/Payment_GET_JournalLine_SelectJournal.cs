using Microsoft.Dynamics.BusinessConnectorNet;
using System;
namespace DotNet
{
    public class Payment_GET_JournalLine_SelectJournal
    {
        public static Tuple<string, string, double, string> getMarkStatus(Axapta DynAx, string CustTransRefRecId, string CustAccNo, string JournalNum)
        {
            double currbal = 0;
            string MarkRecord = "";
            string MarkRecordA = "";
            string chequeDt = "";

            int CustTransOpen = 865;

            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", CustTransOpen);

            var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", 5);//REFRECID
            qbr1.Call("value", CustTransRefRecId);

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);
            if ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", CustTransOpen);
                string SpecRefRecId = DynRec1.get_Field("RECID").ToString();

                //=================================================================================================
                int SpecTrans = 417;
                AxaptaObject axQuery2 = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource2 = (AxaptaObject)axQuery2.Call("addDataSource", SpecTrans);

                var qbr2 = (AxaptaObject)axQueryDataSource2.Call("addRange", 7);//RefRecId
                qbr2.Call("value", SpecRefRecId);

                var qbr = (AxaptaObject)axQueryDataSource2.Call("addRange", 1);//spectableid
                qbr.Call("value", "212");

                AxaptaObject axQueryRun2 = DynAx.CreateAxaptaObject("QueryRun", axQuery2);

                if ((bool)axQueryRun2.Call("next"))
                {
                    AxaptaRecord DynRec2 = (AxaptaRecord)axQueryRun2.Call("Get", SpecTrans);
                    MarkRecord = "In Use";
                    string SpecJouRecID = DynRec2.get_Field("SpecRecId").ToString();
                    currbal = Convert.ToDouble(DynRec2.get_Field("Balance01"));

                    //=================================================================================================
                    int LedgerJournalTrans = 212;
                    AxaptaObject axQuery3 = DynAx.CreateAxaptaObject("Query");
                    AxaptaObject axQueryDataSource3 = (AxaptaObject)axQuery3.Call("addDataSource", LedgerJournalTrans);

                    var qbr3 = (AxaptaObject)axQueryDataSource3.Call("addRange", 4);//CustAccNo
                    qbr3.Call("value", CustAccNo);
                    AxaptaObject axQueryRun3 = DynAx.CreateAxaptaObject("QueryRun", axQuery3);

                    while ((bool)axQueryRun3.Call("next"))
                    {
                        AxaptaRecord DynRec3 = (AxaptaRecord)axQueryRun3.Call("Get", LedgerJournalTrans);
                        string LedgerJournalRecID = DynRec3.get_Field("RECID").ToString();

                        if (SpecJouRecID == LedgerJournalRecID)
                        {
                            chequeDt = Convert.ToDateTime(DynRec3.get_Field("TransDate")).ToString("dd/MM/yyyy");
                            string JourLedger = DynRec3.get_Field("JournalNum").ToString();
                            if (JournalNum == JourLedger)
                            {
                                MarkRecordA = "1";
                            }
                            else
                            {
                                MarkRecordA = "";
                            }
                        }
                        DynRec3.Dispose();
                    }

                    //=================================================================================================
                    DynRec2.Dispose();
                }
                else
                {
                    MarkRecord = "-";
                }
                //=================================================================================================
                DynRec1.Dispose();
            }
            else
            {
                MarkRecord = "0";
            }
            return new Tuple<string, string, double, string>(MarkRecord, MarkRecordA, currbal, chequeDt);
        }

        public static string get_RecIds(Axapta DynAx, string RecordId)
        {
            string CustTransRecId = "";

            int CustTransOpen = 865;

            AxaptaObject axQuery4 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource4 = (AxaptaObject)axQuery4.Call("addDataSource", CustTransOpen);

            var qbr4 = (AxaptaObject)axQueryDataSource4.Call("addRange", 5);//RecId
            qbr4.Call("value", RecordId);

            AxaptaObject axQueryRun4 = DynAx.CreateAxaptaObject("QueryRun", axQuery4);
            if ((bool)axQueryRun4.Call("next"))//use if only one record
            {
                AxaptaRecord DynRec4 = (AxaptaRecord)axQueryRun4.Call("Get", CustTransOpen);
                CustTransRecId = DynRec4.get_Field("RECID").ToString();
                DynRec4.Dispose();
            }


            return CustTransRecId;//SpecRefRecId
        }

        public static Tuple<string, string, string[], string[], int, string, string, Tuple<double[], double[], double[]>> get_MultipleCustInvoiceLine(Axapta DynAx, string ParentRecId)
        {
            int count = 0;
            string Particulars = "";
            string HeaderParticulars = "";
            string reason = "";
            string[] LF_InvoiceId = new string[50];
            string[] LF_TotalDiscount = new string[50];
            string LF_CustInvoiceTransRecId = "";
            double[] lf_Qty = new double[50];
            double[] AmountCur = new double[50];
            double[] totalAmount = new double[50];

            int CustInvoiceLine = 63;

            AxaptaObject axQuery1_11 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1_11 = (AxaptaObject)axQuery1_11.Call("addDataSource", CustInvoiceLine);

            axQueryDataSource1_11.Call("addSortField", 2, 0);//LineNum, ascending
            //to avoid getting empty header and particular 3/5/23

            var qbr1_11 = (AxaptaObject)axQueryDataSource1_11.Call("addRange", 1);
            qbr1_11.Call("value", ParentRecId);//ParentRecId

            AxaptaObject axQueryRun1_11 = DynAx.CreateAxaptaObject("QueryRun", axQuery1_11);
            while ((bool)axQueryRun1_11.Call("next"))
            {
                AxaptaRecord DynRec1_11 = (AxaptaRecord)axQueryRun1_11.Call("Get", CustInvoiceLine);
                Particulars = DynRec1_11.get_Field("Particulars").ToString();
                HeaderParticulars = DynRec1_11.get_Field("HeaderParticulars").ToString();
                LF_InvoiceId[count] = DynRec1_11.get_Field("LF_InvoiceID").ToString();
                LF_TotalDiscount[count] = Convert.ToDouble(DynRec1_11.get_Field("LF_TotalDisc")).ToString("f1");
                LF_CustInvoiceTransRecId = DynRec1_11.get_Field("LF_CustInvoiceTransRecId").ToString();
                reason = DynRec1_11.get_Field("CN_Reason").ToString();
                lf_Qty[count] = Convert.ToDouble(DynRec1_11.get_Field("LF_Qty"));
                AmountCur[count] = Convert.ToDouble(DynRec1_11.get_Field("AmountCur"));

                totalAmount[count] = lf_Qty[count] * AmountCur[count];
                if (totalAmount[count] == 0)
                {
                    totalAmount[count] = AmountCur[count];
                }
                DynRec1_11.Dispose();
                count = count + 1;
            }
            return new Tuple<string, string, string[], string[], int, string, string, Tuple<double[], double[], double[]>>
                (Particulars, HeaderParticulars, LF_InvoiceId, LF_TotalDiscount, count, LF_CustInvoiceTransRecId, reason, Tuple.Create(lf_Qty, AmountCur, totalAmount));
        }

    }
}