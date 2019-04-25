using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using BinLibrary.RevitHelper;
using BinLibrary.WinHelper;

namespace 红瓦功能揭秘.BinLibrary.Helpers
{
    public static class TransactionHelper
    {
        public static void Invoke(this Document doc, Action<Transaction> action, string name = "Invoke")
        {
#if DEBUG
            LogHelper.LogException(delegate
            {
#endif
                using (Transaction transaction = new Transaction(doc, name))
                {
                    transaction.Start();
                    action(transaction);
                    bool flag = transaction.GetStatus() == (TransactionStatus)1;
                    if (flag)
                    {
                        transaction.Commit();
                    }
                }
#if DEBUG
            }, "c:\\revitExceptionlog.txt");
#endif

        }
        public static void Invoke(this Document doc, Action<Transaction> action, string name = "Invoke", bool ignorefailure = true)
        {
            LogHelper.LogException(delegate
            {
                using (Transaction transaction = new Transaction(doc, name))
                {
                    transaction.Start();

                    if (ignorefailure)
                        transaction.IgnoreFailure();

                    action(transaction);
                    bool flag = transaction.GetStatus() == TransactionStatus.Started;
                    if (flag)
                    {
                        transaction.Commit();
                    }
                }
            }, "c:\\revitExceptionlog.txt");
        }

        public static void SubtranInvoke(this Document doc, Action<SubTransaction> action)
        {
            using (SubTransaction subTransaction = new SubTransaction(doc))
            {
                subTransaction.Start();
                action(subTransaction);
                bool flag = subTransaction.GetStatus() == (TransactionStatus)1;
                if (flag)
                {
                    subTransaction.Commit();
                }


            }
        }
    }
}
