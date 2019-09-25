using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;

namespace 唐僧解瓦.Test
{
    /// <summary>
    /// 房间过滤测试
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    [Journaling(JournalingMode.UsingCommandData)]
    [Regeneration(RegenerationOption.Manual)]
    class TestCmd_FIlterRoom:IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiapp = commandData.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var doc = uidoc.Document;
            var sel = uidoc.Selection;
            
            var roomfilter = new RoomFilter();
            var collector = new FilteredElementCollector(doc);

            var rooms = collector.WherePasses(roomfilter).WhereElementIsNotElementType();

            foreach (var element in rooms)
            {
                var location = element.Location;
            }
            MessageBox.Show(rooms.Count().ToString());
            
            return Result.Succeeded;
        }
    }
}
