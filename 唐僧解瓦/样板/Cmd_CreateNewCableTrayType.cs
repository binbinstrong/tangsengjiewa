using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;
using 唐僧解瓦.BinLibrary.Extensions;
using 唐僧解瓦.样板.UIs;

namespace 唐僧解瓦.样板
{
    /// <summary>
    /// 创建桥架类型
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    class Cmd_CreateNewCableTrayType : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uidoc = commandData.Application.ActiveUIDocument;
            var doc = uidoc.Document;


            var collector = new FilteredElementCollector(doc);
            var cabletrytypes = collector.OfClass(typeof(CableTrayType)).Cast<CableTrayType>().ToList();

            TypeSelector selector = new TypeSelector();

            selector.typecombo.ItemsSource = cabletrytypes;
            selector.typecombo.DisplayMemberPath = "Name";
            selector.typecombo.SelectedIndex = 0;

            selector.ShowDialog();

            var targettypeName = selector.typeName.Text;
            var typeNoteText = selector.noteText.Text;
            if (string.IsNullOrWhiteSpace(targettypeName) || targettypeName == "新类型名称")
            {
                MessageBox.Show("名称错误");
                return Result.Cancelled;
            }

            var targettype = selector.typecombo.SelectedItem as CableTrayType;

            var elbowpara = targettype.LookupParameter("水平弯头");
            var teepara = targettype.LookupParameter("T 形三通");
            var verticalElbowParaIN = targettype.LookupParameter("垂直内弯头");
            var verticalElbowParaOUT = targettype.LookupParameter("垂直外弯头");
            var transitionPara = targettype.LookupParameter("过渡件");
            var unionPara = targettype.LookupParameter("活接头");

            var elbow = elbowpara.AsElementId().GetElement(doc) as FamilySymbol;
            var tee = teepara.AsElementId().GetElement(doc) as FamilySymbol;

            var verticalElbowIn = verticalElbowParaIN.AsElementId().GetElement(doc) as FamilySymbol;
            var verticalElbowOUT = verticalElbowParaOUT.AsElementId().GetElement(doc) as FamilySymbol;

            var transition = transitionPara.AsElementId().GetElement(doc) as FamilySymbol;
            var union = unionPara.AsElementId().GetElement(doc) as FamilySymbol;

            Transaction ts = new Transaction(doc, "创建新桥架类型");
            ts.Start();
            //创建新的连接件类型

            var newelbow = elbow.Duplicate(targettypeName);
            var newtee = tee.Duplicate(targettypeName);
            var newverticalelbowIn = verticalElbowIn.Duplicate(targettypeName);
            var newverticalelbowOut = verticalElbowOUT.Duplicate(targettypeName);
            var newtransition = transition.Duplicate(targettypeName);
            var newunion = union.Duplicate(targettypeName);

            doc.Regenerate();

            var newtype = targettype.Duplicate(targettypeName);

            var list = new List<Element>(){newelbow,newtee,newverticalelbowOut,newverticalelbowIn,newtransition,newunion,newtype};

            foreach (var element in list)
            {
                var typeNotePara = element.LookupParameter("类型注释");
                typeNotePara.Set(typeNoteText);
            }
            
            var newelbowpara = newtype.LookupParameter("水平弯头");
            var newteepara = newtype.LookupParameter("T 形三通");
            var newverticalElbowParaIN = newtype.LookupParameter("垂直内弯头");
            var newverticalElbowParaOUT = newtype.LookupParameter("垂直外弯头");
            var newtransitionPara = newtype.LookupParameter("过渡件");
            var newunionPara = newtype.LookupParameter("活接头");

            newelbowpara.Set(newelbow.Id);
            newteepara.Set(newtee.Id);
            newverticalElbowParaIN.Set(newverticalelbowIn.Id);
            newverticalElbowParaOUT.Set(newverticalelbowOut.Id);
            newtransitionPara.Set(newtransition.Id);
            newunionPara.Set(newunion.Id);
             
            ts.Commit();

            selector.Close();
            return Result.Succeeded;
        }
    }
}
