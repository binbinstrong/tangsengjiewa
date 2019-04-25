using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.DB.Events;

namespace BinLibrary.RevitHelper
{
   public static class TransactionFailureHelper
    {
        public static void FailureDefault(this Transaction tran,FailuresPreprocessor_throwError preProcessor)
        {
            FailureHandlingOptions options = tran.GetFailureHandlingOptions();
            options.SetForcedModalHandling(false);
            options.SetClearAfterRollback(false);
            options.SetDelayedMiniWarnings(false);
            options.SetFailuresPreprocessor(preProcessor);
            tran.SetFailureHandlingOptions(options);
        }

        public static void IgnoreFailure(this Transaction tran ,FailuresPreprocessor_throwError preProFessor = null)
        {
            if (preProFessor==null)
            {
                preProFessor = new FailuresPreprocessor_throwError();
            }
            var options = tran.GetFailureHandlingOptions();
            options.SetFailuresPreprocessor(preProFessor);
            tran.SetFailureHandlingOptions(options);
        }
    }
 

    /// <summary>
    /// 错误提示静默处理不弹出提示框
    /// </summary>
    public class FailuresPreprocessor : IFailuresPreprocessor
    {
        private string _failureMessage;
        private bool _hasError;
        public string FailureMessage
        {
            get { return _failureMessage; }
            set { _failureMessage = value; }
        }
        public bool HasError
        {
            get { return _hasError; }
            set { _hasError = value; }
        }
        public FailureProcessingResult PreprocessFailures(FailuresAccessor failuresAccessor)
        {
            IList<FailureMessageAccessor> listFma = failuresAccessor.GetFailureMessages();
            if (listFma.Count == 0)
                return FailureProcessingResult.Continue;
            foreach (FailureMessageAccessor fma in listFma)
            {
                if (fma.GetSeverity() == FailureSeverity.Error)
                {
                    if (fma.HasResolutions())
                        failuresAccessor.ResolveFailure(fma);
                }
                if (fma.GetSeverity() == FailureSeverity.Warning)
                {
                    failuresAccessor.DeleteWarning(fma);
                }

            }
            return FailureProcessingResult.ProceedWithCommit;
        }
    }

    /// <summary>
    /// 错误处理 警告忽略 错误抛出
    /// </summary>
    public class FailuresPreprocessor_throwError : IFailuresPreprocessor
    {
        private string _failureMessage;
        private bool _hasError;
        public string FailureMessage
        {
            get { return _failureMessage; }
            set { _failureMessage = value; }
        }
        public bool HasError
        {
            get { return _hasError; }
            set { _hasError = value; }
        }
        public FailureProcessingResult PreprocessFailures(FailuresAccessor failuresAccessor)
        {
            IList<FailureMessageAccessor> listFma = failuresAccessor.GetFailureMessages();
            if (listFma.Count == 0)
                return FailureProcessingResult.Continue;
            foreach (FailureMessageAccessor fma in listFma)
            {
                if (fma.GetSeverity() == FailureSeverity.Error)
                {
                    _failureMessage = fma.GetDescriptionText();
                    _hasError = true;
                    return FailureProcessingResult.ProceedWithRollBack;

                    //if (fma.HasResolutions())
                    //    failuresAccessor.ResolveFailure(fma);
                }
                if (fma.GetSeverity() == FailureSeverity.Warning)
                {
                    failuresAccessor.DeleteWarning(fma);
                }
            }
            return FailureProcessingResult.ProceedWithCommit;
        }
    }
}
