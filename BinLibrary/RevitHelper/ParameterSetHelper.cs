using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;

namespace BinLibrary.RevitHelper
{
	public static class ParameterSetHelper
	{
		public static Dictionary<BuiltInParameter, Parameter> GetParameterBuitInDic(this ParameterSet paraSet)
		{
			Dictionary<BuiltInParameter, Parameter> dictionary = new Dictionary<BuiltInParameter, Parameter>();
			foreach (Parameter parameter in paraSet)
			{
				InternalDefinition internalDefinition = parameter.Definition as InternalDefinition;
				bool flag = dictionary.ContainsKey(internalDefinition.BuiltInParameter) || internalDefinition.BuiltInParameter == (BuiltInParameter)(-1);
				if (!flag)
				{
					dictionary.Add(internalDefinition.BuiltInParameter, parameter);
				}
			}
			return dictionary;
		}

		public static Dictionary<string, Parameter> GetParameterNameDic(this ParameterSet paraSet)
		{
			Dictionary<string, Parameter> dictionary = new Dictionary<string, Parameter>();
			foreach (Parameter parameter in paraSet)
			{
				bool flag = !dictionary.ContainsKey(parameter.Definition.Name);
				if (flag)
				{
					dictionary.Add(parameter.Definition.Name, parameter);
				}
			}
			return dictionary;
		}

		public static Dictionary<Guid, Parameter> GetParaeterGuidDic(this ParameterSet paraSet)
		{
			Dictionary<Guid, Parameter> dictionary = new Dictionary<Guid, Parameter>();
			foreach (Parameter parameter in paraSet)
			{
				bool isShared = parameter.IsShared;
				if (isShared)
				{
					dictionary.Add(parameter.GUID, parameter);
				}
			}
			return dictionary;
		}
	}
}
