using Autodesk.Revit.DB;
using System;

namespace BinLibrary.RevitHelper
{
	public static class FamilyParameterHelper
	{
		public static void SetParaValue(this FamilyParameter fmp, FamilyManager fm, object value)
		{
			switch (fmp.StorageType)
			{
			case (StorageType)1:
				fm.Set(fmp, (int)value);
				break;
			case (StorageType)2:
				fm.Set(fmp, (double)value);
				break;
			case (StorageType)3:
				fm.Set(fmp, (string)value);
				break;
			case (StorageType)4:
				fm.Set(fmp, (ElementId)value);
				break;
			}
		}
	}
}
