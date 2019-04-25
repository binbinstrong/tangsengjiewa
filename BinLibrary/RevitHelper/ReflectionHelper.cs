using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace BinLibrary.RevitHelper
{
    public static class ReflectionHelper
    {
        public static bool InvokeModule(string mName, object[] parameters, bool apiModule = true)
        {
            try
            {
                MethodInfo m = null;
                if (apiModule) m = NativeModule.GetApiMoudlerMethod(mName);
                else
                {
                    m = NativeModule.GetUIMoudlerMethod(mName);
                }
                m.Invoke(null, parameters);
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }


    }

    public class NativeModule
    {
        public static MethodInfo GetApiMoudlerMethod(string name)
        {
            var module = NativeModule.getApiModule();
            var rs = module.GetMethods(BindingFlags.NonPublic | BindingFlags.Static).Where(m => m.Name == name);

            return rs.Count() == 0 ? null:rs.First();
        }

        public static MethodInfo GetUIMoudlerMethod(string name)
        {
            var module = NativeModule.getUIModule();
            var rs = module.GetMethods(BindingFlags.NonPublic | BindingFlags.Static).Where(m => m.Name == name);

            return rs.Count() == 0 ? null : rs.First();

        }

        internal static Module getApiModule()
        {
            return typeof(Document).Assembly.Modules.First();
        }

        internal static Module getUIModule()
        {
            return typeof(UIDocument).Assembly.Modules.First();
        }

    }

    public static class parameterExtension
    {
        public static IntPtr ToParamDef(this Parameter parameter)
        {
            try
            {
                var m = typeof(Parameter).GetMethod("getParamDef", BindingFlags.NonPublic | BindingFlags.Instance);
                return ((Pointer) m.Invoke(parameter, null)).ToIntPtr();
            }
            catch (Exception e)
            {
                 return IntPtr.Zero;
            }
        }
        public static bool Setvisibility(this Parameter parameter ,bool visible)
        {
            var parameterInPtr = parameter.ToParamDef();
            if (parameterInPtr==IntPtr.Zero)
            {
                return false;
            }

            var result =
                ReflectionHelper.InvokeModule("ParamDef.setUservisible", new object[] {parameterInPtr, visible});
            return result;
        }
    }

    public static class PointerExtension
    {
        public unsafe static IntPtr ToIntPtr(this Pointer p)
        {
            return (IntPtr)p
                .GetType()
                .GetMethod("GetPointerValue", BindingFlags.NonPublic | BindingFlags.Instance)
                .Invoke(p, null);
        }
    }
}
