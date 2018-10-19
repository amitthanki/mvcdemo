using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ENRLReconSystem.BL;
using ENRLReconSystem.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ERSBackgroundProcess
{
    public class OOAEGHPExclusion
    {
        long _lCurrentMasterUserId = StartBackgroundProcess.CurrentMasterUserId;
        BLCommon _BLCommon = new BLCommon();
        /// <summary>
        /// Refresh the Employer group from Excel file
        /// </summary>
        /// <returns></returns>
        public ExceptionTypes StartEGHPExcelProcess()
        {
            ExceptionTypes result = ExceptionTypes.Success;
            DataTable dataTable = new DataTable();
            string fileName = CacheUtility.GetConfigrationValueByConfigName(ConstantTexts.EGHPExclusionFileName);
            string errorMessage = string.Empty;
            try
            {
                ReadAsDataTable(fileName, out dataTable, out errorMessage);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    result = _BLCommon.EGHPExcelProcess(dataTable, out errorMessage);
                }
                else
                {
                    BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRSubmission, (long)ExceptionTypes.Uncategorized, "Error in EGHP File", errorMessage);
                }
                if (result != ExceptionTypes.Success)
                {
                    BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRSubmission, (long)ExceptionTypes.Uncategorized, errorMessage, errorMessage);
                }
            }
            catch (Exception ex)
            {

                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRSubmission, (long)ExceptionTypes.Uncategorized, "Exception EGHP Exclusion Excel", ex.StackTrace.ToString());
            }

            return result;

        }

        private void ReadAsDataTable(string fileName, out DataTable dataTable, out string errorMessage)
        {
            try
            {
                errorMessage = string.Empty;
                dataTable = new DataTable();
                using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(fileName, false))
                {
                    WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart;
                    IEnumerable<Sheet> sheets = spreadSheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                    string relationshipId = sheets.First().Id.Value;
                    WorksheetPart worksheetPart = (WorksheetPart)spreadSheetDocument.WorkbookPart.GetPartById(relationshipId);
                    Worksheet workSheet = worksheetPart.Worksheet;
                    SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                    IEnumerable<Row> rows = sheetData.Descendants<Row>();

                    foreach (Cell cell in rows.ElementAt(0))
                    {
                        dataTable.Columns.Add(GetCellValue(spreadSheetDocument, cell));
                    }

                    foreach (Row row in rows)
                    {
                        DataRow dataRow = dataTable.NewRow();
                        for (int i = 0; i < row.Descendants<Cell>().Count(); i++)
                        {
                            dataRow[i] = GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(i));
                        }

                        dataTable.Rows.Add(dataRow);
                    }

                }
                dataTable.Rows.RemoveAt(0);

            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString();
                throw ex;
            }


        }
        private string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            try
            {
                SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
                string value = cell.CellValue.InnerXml;

                if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
                {
                    return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
                }
                else
                {
                    return value;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
