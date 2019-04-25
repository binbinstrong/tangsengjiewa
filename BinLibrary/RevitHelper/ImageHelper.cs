using Autodesk.Revit.DB;
using System;

namespace BinLibrary.RevitHelper
{
    public static class ImageHelper
    {
        public static bool ExportImage1(this Document doc, string filepath)
        {
            View activeView = doc.ActiveView;
            ImageExportOptions imageExportOptions = new ImageExportOptions();
            imageExportOptions.ExportRange = (ExportRange)(2);
            imageExportOptions.FilePath = (filepath);
            imageExportOptions.ZoomType = (ZoomFitType)(0);
            imageExportOptions.PixelSize = (1000);
            imageExportOptions.ImageResolution = (ImageResolution)(2);
            bool result;
            try
            {
                doc.ExportImage(imageExportOptions);
            }
            catch (Exception var_2_46)
            {
                result = false;
                return result;
            }
            result = true;
            return result;
        }

        public static void SetPreviewIcon(this Document doc, ElementId viewid)
        {
            bool flag = viewid.Equals(ElementId.InvalidElementId);
            if (!flag)
            {
                SaveOptions saveOptions = new SaveOptions();
                saveOptions.PreviewViewId = (viewid);
                doc.Save(saveOptions);
                doc.GetDocumentPreviewSettings();
                StartingViewSettings startingViewSettings = StartingViewSettings.GetStartingViewSettings(doc);
            }
        }
    }
}
