using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;

namespace BinLibrary.RevitExtension
{
	public static class ParameterSetExtension
	{
		public static Parameter[] ToArray(this ParameterSet paraset)
		{
			return paraset.ToList().ToArray();
		}

		public static List<Parameter> ToList(this ParameterSet paraset)
		{
			List<Parameter> list = new List<Parameter>();
			foreach (Parameter item in paraset)
			{
				list.Add(item);
			}
			return list;
		}
	}
}
