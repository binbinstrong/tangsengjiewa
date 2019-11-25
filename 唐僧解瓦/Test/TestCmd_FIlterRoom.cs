using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;
using System.Linq;
using System.Windows;
using 唐僧解瓦.BinLibrary.Helpers;

namespace 唐僧解瓦.Test
{
    /// <summary>
    /// 房间过滤测试
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    [Journaling(JournalingMode.UsingCommandData)]
    [Regeneration(RegenerationOption.Manual)]
    class TestCmd_FIlterRoom : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiapp = commandData.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var doc = uidoc.Document;
            var acview = doc.ActiveView;
            var sel = uidoc.Selection;

            var roomfilter = new RoomFilter();
            var collector = new FilteredElementCollector(doc);

            var rooms = collector.WherePasses(roomfilter).WhereElementIsNotElementType();

            foreach (var element in rooms)
            {
                var location = element.Location;
            }
            MessageBox.Show(rooms.Count().ToString());


            //ParameterFilterElement filterelement = ParameterFilterElement.Create(doc,"喷淋");

            var wall = doc.TCollector<Wall>().First();

            var schemas = Schema.ListSchemas();

            foreach (var schema in schemas)
            {
                var guids = wall.GetEntitySchemaGuids();
                var schemas1 = guids.Select(m => Schema.Lookup(m));
                foreach (var schema1 in schemas1)
                {
                    wall.DeleteEntity(schema1);
                }
            }

            return Result.Succeeded;
        }
    }
}
