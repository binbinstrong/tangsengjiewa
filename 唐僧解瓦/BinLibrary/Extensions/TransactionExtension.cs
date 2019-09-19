using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 唐僧解瓦.BinLibrary.Extensions
{
    public static class TransactionExtension
    {
        public static void IgnoreFailure(this Transaction trans)
        {
            var options = trans.GetFailureHandlingOptions();
            options.SetFailuresPreprocessor(new failure_ignore());
        }
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
