using Autodesk.Revit.DB;
using System;

namespace BinLibrary.RevitExtension
{
	public static class ReferenceExtension
	{
		public static Element GetElement(this Reference thisref, Document doc)
		{
			return doc.GetElement(thisref);
		}
	}
}
