using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Interop;

namespace 唐僧解瓦.机电.About
{
    [Transaction(TransactionMode.Manual)]
    class AboutCommand:IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            About about = new About();
            var acwinHwd = Process.GetCurrentProcess().MainWindowHandle;
            var acwin = NativeWindow.FromHandle(acwinHwd);

            WindowInteropHelper winhelper = new WindowInteropHelper(about);
            winhelper.Owner = acwinHwd;
            about.Show();

            return Result.Succeeded;
        }
    }
}
