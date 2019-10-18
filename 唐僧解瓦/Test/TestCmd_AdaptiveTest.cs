using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace 唐僧解瓦.Test
{
    class TestCmd_AdaptiveTest : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            var revitDoc = commandData.Application.ActiveUIDocument.Document;

            //bool modelCurveZhu = revitDoc.LoadFamilySymbol(TempFamily.FamilyPath_first, System.IO.Path.GetFileNameWithoutExtension(TempFamily.FamilyPath_first), out FamilySymbol familySymbol);   //这里的族一定要有类型才行

            //familySymbol.Activate();   //激活族类型
            //famIns1 = AdaptiveComponentInstanceUtils.CreateAdaptiveComponentInstance(revitDoc, familySymbol);   //自适应族实例化
            //IList<ElementId> placePointIds = AdaptiveComponentInstanceUtils.GetInstancePlacementPointElementRefIds(famIns1);
            //point = revitDoc.GetElement(placePointIds[0]) as ReferencePoint;                //自适应构件族的自适应点

            //point.Position = modelCurve.GeometryCurve.GetEndPoint(0);
            //PointLocationOnCurve pointLocationOnCurve00 = new PointLocationOnCurve(PointOnCurveMeasurementType.NormalizedCurveParameter, 0, PointOnCurveMeasureFrom.Beginning);
            //PointOnEdge poe00 = revitApp.Create.NewPointOnEdge(modelCurve.GeometryCurve.Reference, pointLocationOnCurve00);  // 将自适应点和模型线关联起来                
            //point.SetPointElementReference(poe00 as PointElementReference);
             


            return Result.Succeeded;
        }
    }
}
