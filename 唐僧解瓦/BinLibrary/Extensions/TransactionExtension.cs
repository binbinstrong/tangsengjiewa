using Autodesk.Revit.DB;

namespace 唐僧解瓦.BinLibrary.Extensions
{
    public static class TransactionExtension
    {
        public static void IgnoreFailure(this Transaction trans)
        {
            var options = trans.GetFailureHandlingOptions();
            options.SetFailuresPreprocessor(new failure_ignore());
        }

        //public static void Invoke(this Document doc, Action<Transaction> action, string transactionName = "aaa")
        //{
        //    Transaction ts = new Transaction(doc, transactionName);
        //    LogHelper.LogException(delegate
        //    {
        //        ts.Start();
        //        action(ts);
        //        ts.Commit();
        //    }, @"c:\transactionException.txt");
        //}
    }

    public class failure_ignore : IFailuresPreprocessor
    {
        public FailureProcessingResult PreprocessFailures(FailuresAccessor failuresAccessor)
        {
            failuresAccessor.DeleteAllWarnings();
            //failuresAccessor.DeleteElements(failuresAccessor.el);
            return FailureProcessingResult.Continue;
        }
    }
}
