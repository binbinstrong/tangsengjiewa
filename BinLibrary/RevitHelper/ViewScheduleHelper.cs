using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Autodesk.Revit.DB;

namespace BinLibrary.RevitHelper
{
    public static class ViewScheduleHelper
    {
        public static DataTable ToDataTable(this ViewSchedule viewSchedule)
        {
            DataTable result = new DataTable();

            var tableData = viewSchedule.GetTableData();
            var sectionNums = tableData.NumberOfSections;

            var sectionData_body = tableData.GetSectionData(SectionType.Body);

            var rowNums = sectionData_body.NumberOfRows;
            var columnNums = sectionData_body.NumberOfColumns;

            for (int i = 0; i < columnNums; i++)
            {
                result.Columns.Add();
            }

            for (int x = 0; x < sectionNums; x++)
            {
                columnNums = tableData.GetSectionData(x).NumberOfColumns;
                rowNums = tableData.GetSectionData(x).NumberOfRows;
            
                SectionType sectype = x == 0 ? SectionType.Header : SectionType.Body;

                for (int i = 0; i < rowNums; i++)
                {
                    List<object> rowList = new List<object>();
                    for (int j = 0; j < columnNums; j++)
                    {
                        rowList.Add(viewSchedule.GetCellText(sectype, i, j));
                        if (sectype==SectionType.Header)
                        {
                            break;
                        }
                    }
                    result.Rows.Add(rowList.ToArray());
                }
            }
            
            return result;
        }
    }
}
