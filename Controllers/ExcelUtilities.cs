using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Data;
using System.Text.RegularExpressions;
using System.IO;
using DocumentFormat.OpenXml;
using System.Text;
using System.Runtime.InteropServices;

namespace PMODashboard.Controllers
{
    public class ExcelUtilities
    {

        //public static DataTable ExcelToDataTable1(FileInfo fileinfo)
        //{
        //    DataTable dt = new DataTable();

        //    using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(fileinfo.FullName, false))
        //    {

        //        WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart;
        //        IEnumerable<Sheet> sheets = spreadSheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
        //        string relationshipId = sheets.First().Id.Value;
        //        WorksheetPart worksheetPart = (WorksheetPart)spreadSheetDocument.WorkbookPart.GetPartById(relationshipId);
        //        Worksheet workSheet = worksheetPart.Worksheet;
        //        SheetData sheetData = workSheet.GetFirstChild<SheetData>();
        //        IEnumerable<Row> rows = sheetData.Descendants<Row>();

        //        foreach (Cell cell in rows.ElementAt(0))
        //        {
        //            dt.Columns.Add(GetCellValue(spreadSheetDocument, cell));
        //        }

        //        foreach (Row row in rows) //this will also include your header row...
        //        {
        //            DataRow tempRow = dt.NewRow();

        //            for (int i = 0; i < row.Descendants<Cell>().Count(); i++)
        //            {
        //                tempRow[i] = GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(i - 1));
        //            }

        //            dt.Rows.Add(tempRow);
        //        }

        //    }
        //    dt.Rows.RemoveAt(0);
        //    return dt;
        //}
        public static string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
            
            string value = cell.CellValue == null ? "" : cell.CellValue.InnerXml;

            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
            }
            else
            {
                return value;
            }
        }

        public static DataTable ExcelToDataTable(FileInfo filepath, [Optional] DataSet columnhead)
        {
            DataTable dt = new DataTable();
            bool ColumnHeader = true;
            bool _Isemptyheader = false;

            using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(filepath.FullName, false))
            {

                WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart;
                IEnumerable<Sheet> sheets = spreadSheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                string relationshipId = sheets.ElementAt(0).Id.Value; //sheets.First().Id.Value;
                WorksheetPart worksheetPart = (WorksheetPart)spreadSheetDocument.WorkbookPart.GetPartById(relationshipId);
                Worksheet workSheet = worksheetPart.Worksheet;
                SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                int rowCount = sheetData.Descendants<Row>().Count();
                if (rowCount == 0)
                {
                    return dt;
                }
                IEnumerable<Row> rows = sheetData.Descendants<Row>();

                var charcolumn = 'A';
                foreach (Cell cell in rows.ElementAt(0))
                {
                    if (GetCellValue(spreadSheetDocument, cell).ToString() != "" || _Isemptyheader)
                    {
                        if (ColumnHeader)
                            dt.Columns.Add(GetCellValue(spreadSheetDocument, cell));
                        else
                        {
                            dt.Columns.Add(charcolumn.ToString());
                            charcolumn++;
                        }
                    }
                }
                foreach (Row row in rows) //this will also include your header row...
                {
                    DataRow tempRow = dt.NewRow();
                    int columnIndex = 0;
                    foreach (Cell cell in row.Descendants<Cell>())
                    {
                        // Gets the column index of the cell with data
                        int cellColumnIndex = (int)GetColumnIndexFromName(GetColumnName(cell.CellReference));
                        cellColumnIndex--; //zero based index
                        if (columnIndex < cellColumnIndex)
                        {
                            do
                            {
                                columnIndex++;
                            }
                            while (columnIndex < cellColumnIndex);
                        }
                        if (columnIndex < dt.Columns.Count)
                        {
                            //var temp = columnhead.Tables[0].Rows[cellColumnIndex][1].ToString();
                            if(columnhead == null)
                                tempRow[columnIndex] = GetCellValue(spreadSheetDocument, cell);
                            else
                            { 
                            if (columnhead.Tables[0].Rows[cellColumnIndex][1].ToString().Trim().Equals("datetime"))
                                tempRow[columnIndex] = TryParseExcelDateTime(GetCellValue(spreadSheetDocument, cell));
                            else
                                tempRow[columnIndex] = GetCellValue(spreadSheetDocument, cell);
                            }
                            columnIndex++;
                        }
                    }

                    dt.Rows.Add(tempRow);
                }
            }
            if (ColumnHeader)
                dt.Rows.RemoveAt(0); //...so i'm taking it out here.
            return dt;
        }
        public static DataTable ExcelToDataTablewithdatatype(FileInfo filepath, [Optional] DataSet columnhead)
        {
            DataTable dt = new DataTable();
            bool ColumnHeader = true;
            bool _Isemptyheader = false;

            using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(filepath.FullName, false))
            {

                WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart;
                IEnumerable<Sheet> sheets = spreadSheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                string relationshipId = sheets.ElementAt(0).Id.Value; //sheets.First().Id.Value;
                WorksheetPart worksheetPart = (WorksheetPart)spreadSheetDocument.WorkbookPart.GetPartById(relationshipId);
                Worksheet workSheet = worksheetPart.Worksheet;
                SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                int rowCount = sheetData.Descendants<Row>().Count();
                if (rowCount == 0)
                {
                    return dt;
                }
                IEnumerable<Row> rows = sheetData.Descendants<Row>();

                var charcolumn = 'A';
                foreach (Cell cell in rows.ElementAt(0))
                {
                    if (GetCellValue(spreadSheetDocument, cell).ToString() != "" || _Isemptyheader)
                    {
                        if (ColumnHeader)
                            dt.Columns.Add(GetCellValue(spreadSheetDocument, cell));
                        else
                        {
                            dt.Columns.Add(charcolumn.ToString());
                            charcolumn++;
                        }
                    }
                }
                foreach (Row row in rows) //this will also include your header row...
                {
                    DataRow tempRow = dt.NewRow();
                    int columnIndex = 0;
                    foreach (Cell cell in row.Descendants<Cell>())
                    {
                        // Gets the column index of the cell with data
                        int cellColumnIndex = (int)GetColumnIndexFromName(GetColumnName(cell.CellReference));
                        cellColumnIndex--; //zero based index
                        if (columnIndex < cellColumnIndex)
                        {
                            do
                            {
                                columnIndex++;
                            }
                            while (columnIndex < cellColumnIndex);
                        }
                        if (columnIndex < dt.Columns.Count)
                        {
                            var temp = columnhead.Tables[0].Rows[cellColumnIndex][1].ToString();
                            if (columnhead.Tables[0].Rows[cellColumnIndex][1].ToString().Trim().Equals("datetime"))
                                tempRow[columnIndex] = TryParseExcelDateTime(GetCellValue(spreadSheetDocument, cell));
                            else
                                tempRow[columnIndex] = GetCellValue(spreadSheetDocument, cell);
                            columnIndex++;
                        }
                    }

                    dt.Rows.Add(tempRow);
                }
            }
            if (ColumnHeader)
                dt.Rows.RemoveAt(0); //...so i'm taking it out here.
            return dt;
        }
        public static string TryParseExcelDateTime(string excelDateTimeAsString)
        {

            double oaDateAsDouble;
            //int oaDateAsint;
            if (!double.TryParse(excelDateTimeAsString, out oaDateAsDouble)) // || int.TryParse(excelDateTimeAsString, out oaDateAsint)this line is Culture dependent!
                return excelDateTimeAsString;

            else
            {
                return DateTime.FromOADate(oaDateAsDouble).ToString();
            }
        }
        private static string GetColumnName(string cellReference)
        {
            // Create a regular expression to match the column name portion of the cell name.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellReference);
            return match.Value;
        }
        /// <summary>
        /// DataTable To Byte Array Along With Dropdown if the Table 2 is available
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public static byte[] DataTableToByteArray(DataSet dt)
        {
            var ds = new DataSet();
            var dtCopy = new DataTable();
            dtCopy = dt.Tables[0].Copy();
            ds.Tables.Add(dtCopy);

            try
            {
                byte[] returnBytes = null;
                var mem = new MemoryStream();
                var workbook = SpreadsheetDocument.Create(mem, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook);
                {
                    var workbookPart = workbook.AddWorkbookPart();
                    workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                    workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();
                    int i = 0;
                    foreach (System.Data.DataTable table in dt.Tables)
                    {
                        i++;
                        var sheetPart = workbook.WorkbookPart.AddNewPart<WorksheetPart>();
                        var sheetData = new DocumentFormat.OpenXml.Spreadsheet.SheetData();
                        sheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData);

                        DocumentFormat.OpenXml.Spreadsheet.Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
                        string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);

                        uint sheetId = 1;
                        if (sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Count() > 0)
                        {
                            sheetId =
                                sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                        }

                        DocumentFormat.OpenXml.Spreadsheet.Sheet sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet()
                        {
                            Id = relationshipId,
                            SheetId = sheetId,
                            Name = table.TableName,
                            State = i == 1 ? SheetStateValues.Visible : SheetStateValues.Hidden
                        };
                        sheets.Append(sheet);

                        DocumentFormat.OpenXml.Spreadsheet.Row headerRow = new DocumentFormat.OpenXml.Spreadsheet.Row();

                        List<String> columns = new List<string>();
                        foreach (System.Data.DataColumn column in table.Columns)
                        {
                            columns.Add(column.ColumnName);

                            DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                            cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                            cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(column.ColumnName);
                            headerRow.AppendChild(cell);
                        }


                        sheetData.AppendChild(headerRow);

                        foreach (System.Data.DataRow dsrow in table.Rows)
                        {
                            DocumentFormat.OpenXml.Spreadsheet.Row newRow = new DocumentFormat.OpenXml.Spreadsheet.Row();
                            foreach (String col in columns)
                            {
                                DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                                cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                                cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(dsrow[col].ToString()); //
                                newRow.AppendChild(cell);
                            }

                            sheetData.AppendChild(newRow);
                        }



                        if (dt.Tables.Count > 1 && dt.Tables[1].Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Tables[1].Rows)
                            {
                                int idValue = row.Field<int>("ID");
                                string columnName = row.Field<string>("ExcelColumn");
                                string[] array = dt.Tables[2].Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                                int startIndex = Array.IndexOf(array, idValue.ToString()) + 2;
                                int lastIndex = Array.LastIndexOf(array, idValue.ToString()) + 2;
                                string formulaString = "'Table2'!$B$" + startIndex + ":$B$" + lastIndex + "";

                                DataValidation dataValidation = new DataValidation
                                {
                                    Type = DataValidationValues.List,
                                    AllowBlank = true,
                                    SequenceOfReferences = new ListValue<StringValue>() { InnerText = columnName },
                                    Formula1 = new Formula1(formulaString)
                                };

                                DataValidations dvs = sheetPart.Worksheet.GetFirstChild<DataValidations>();
                                if (dvs != null)
                                {
                                    dvs.Count = dvs.Count + 1;
                                    dvs.Append(dataValidation);
                                }
                                else
                                {
                                    DataValidations newDVs = new DataValidations();
                                    newDVs.Append(dataValidation);
                                    newDVs.Count = 1;
                                    sheetPart.Worksheet.Append(newDVs);
                                }
                            }
                        }
                    }
                }

                workbook.WorkbookPart.Workbook.Save();
                workbook.Close();
                returnBytes = mem.ToArray();
                return returnBytes;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private static int? GetColumnIndexFromName(string columnNameOrCellReference)
        {
            int columnIndex = 0;
            int factor = 1;
            for (int pos = columnNameOrCellReference.Length - 1; pos >= 0; pos--) // R to L
            {
                if (Char.IsLetter(columnNameOrCellReference[pos])) // for letters (columnName)
                {
                    columnIndex += factor * ((columnNameOrCellReference[pos] - 'A') + 1);
                    factor *= 26;
                }
            }
            return columnIndex;

        }


        ////private static string GetCellValue(SpreadsheetDocument document, DocumentFormat.OpenXml.Spreadsheet.Cell cell)
        ////{
        ////    DateTime ReleaseDate = new DateTime(1899, 12, 30);
        ////    SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
        ////    object value = string.Empty;
        ////    DocumentFormat.OpenXml.Spreadsheet.CellFormats cellFormats = (DocumentFormat.OpenXml.Spreadsheet.CellFormats)document.WorkbookPart.WorkbookStylesPart.Stylesheet.CellFormats;

        ////    string format = string.Empty; uint formatid = 0;

        ////    if (cell.DataType == null)
        ////    {
        ////        DocumentFormat.OpenXml.Spreadsheet.CellFormat cf = new CellFormat();
        ////        if (cell.StyleIndex == null)
        ////        {
        ////            cf = cellFormats.Descendants<DocumentFormat.OpenXml.Spreadsheet.CellFormat>().ElementAt<DocumentFormat.OpenXml.Spreadsheet.CellFormat>(0);
        ////        }
        ////        else
        ////        {
        ////            cf = cellFormats.Descendants<DocumentFormat.OpenXml.Spreadsheet.CellFormat>().ElementAt<DocumentFormat.OpenXml.Spreadsheet.CellFormat>(Convert.ToInt32(cell.StyleIndex.Value));
        ////        }

        ////        formatid = cf.NumberFormatId;

        ////        if (cell != null && cell.InnerText.Length > 0)
        ////        {
        ////            value = cell.CellValue.Text;
        ////            if (formatid > 13 && formatid <= 22)
        ////            {
        ////                DateTime answer = ReleaseDate.AddDays(Convert.ToDouble(cell.CellValue.Text));
        ////                value = answer.ToShortDateString();
        ////            }

        ////        }
        ////        else
        ////        {

        ////            value = cell.InnerText;
        ////        }
        ////    }

        ////    if (cell.DataType != null)
        ////    {
        ////        switch (cell.DataType.Value)
        ////        {
        ////            case CellValues.SharedString:
        ////                return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(cell.CellValue.Text)].InnerText;
        ////            case CellValues.Boolean:
        ////                return cell.CellValue.Text == "1" ? "true" : "false";
        ////            case CellValues.Date:
        ////                {
        ////                    DateTime answer = ReleaseDate.AddDays(Convert.ToDouble(cell.CellValue.Text));
        ////                    return answer.ToShortDateString();
        ////                }
        ////            case CellValues.Number:
        ////                return Convert.ToDecimal(cell.CellValue.Text).ToString();
        ////            default:
        ////                if (cell.CellValue != null)
        ////                    return cell.CellValue.Text;
        ////                return string.Empty;
        ////        }
        ////    }

        ////    return value.ToString();
        ////}
    }
}
