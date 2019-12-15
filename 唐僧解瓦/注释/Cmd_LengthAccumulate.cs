using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using 唐僧解瓦.BinLibrary.Extensions;
using 唐僧解瓦.BinLibrary.RevitHelper;
using 唐僧解瓦.注释.UIs;

namespace 唐僧解瓦.注释
{
    /// <summary>
    /// 统计长度
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class Cmd_LengthAccumulate : IExternalCommand
    {
        public static List<ModelLine> modelLines = new List<ModelLine>();
        public static List<ElementId> addedIds = new List<ElementId>();
        public static Document _doc = default(Document);

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiapp = commandData.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var doc = uidoc.Document;
            var acview = doc.ActiveView;

            var sel = uidoc.Selection;

            _doc = doc;

            doc.Application.DocumentChanged += OnDocumentChanged;

            ResultShow resultshowwin = ResultShow.Instance;
            resultshowwin.helper().Owner = RevitWindowHelper.GetRevitHandle();
            ResultShow.Instance.Show();
             
            uiapp.PostCommand(RevitCommandId.LookupPostableCommandId(PostableCommand.ModelLine));
             

            return Result.Succeeded;
        }

        private void OnDocumentChanged(object sender, DocumentChangedEventArgs e)
        {
            var app = sender as Application;
            var ids = e.GetAddedElementIds();

            foreach (var elementId in ids)
            {
                if (!addedIds.Contains(elementId))
                {
                    addedIds.Add(elementId);
                }
            }

        }

    }
}
