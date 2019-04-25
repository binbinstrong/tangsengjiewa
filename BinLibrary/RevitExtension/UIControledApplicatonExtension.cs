using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BinLibrary.RevitExtension
{
    public static class UIControledApplicatonExtension
    {
        public static UIApplication UIApplication(this UIControlledApplication uicapp)
        {
            var type = uicapp.GetType();
            var  result =type.InvokeMember("getUIApplication",
               BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod,Type.DefaultBinder, uicapp, null) as UIApplication;
            return result;
        }
    }
}
