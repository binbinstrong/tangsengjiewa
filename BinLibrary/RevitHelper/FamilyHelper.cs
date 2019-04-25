using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using Autodesk.Revit.DB;
using BinLibrary.Extensions;
using BinLibrary.RevitExtension;

namespace BinLibrary.RevitHelper
{
    public static class FamilyHelper
    {
        public static FamilySymbol FamilyLoad(this Document doc, string path, string familyname)
        {
            FamilySymbol result = null;
            Family family = null;
            if (!File.Exists(path))
            {
                return result;
            }
            try
            {
                var familysymbols = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol));
                var iterator = familysymbols.GetEnumerator();
                while (iterator.MoveNext())
                {
                    var familysymbol = iterator.Current as FamilySymbol;
                    if (familysymbol.Family.Name == familyname)
                    {
                        result = familysymbol;
                        //MessageBox.Show("find it");
                        break;
                    }
                }
                if (result == null)
                {
                    doc.LoadFamily(path, out family);
                    var symbols = family.GetFamilySymbolIds();
                    var _iterator = symbols.GetEnumerator();

                    while (_iterator.MoveNext())
                    {
                        var symbol = _iterator.Current.GetElement(doc) as FamilySymbol;
                        result = symbol;
                        break;
                    }
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return null;
            }
            return result;
        }
    }
}
