#r "System.IO"
#r "Microsoft.Office.Interop.Excel"

using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

string path = "C:\\Git\\pbi-exportimport-te2\\"; // Add path to Export Folder, add \\ to end of Path
string fileNameTSV = "3-doku-export.tsv";
string fileNameExcel = "3-Full-DataSet-Dokumentation.xlsx";
string excelTabName = "technical Documentation";

string pathFilenameTSV = path + fileNameTSV;
string pathFilenameExcel = path + fileNameExcel;

// Construct a list of all columns and measures
var objects = Model.Tables.Cast<ITabularNamedObject>().Concat(Model.AllMeasures).Concat(Model.AllColumns);

// Construct a list of all hidden columns and measures:
// var objects = Model.AllMeasures.Where(m => !m.IsHidden && !m.Table.IsHidden).Cast<ITabularNamedObject>()
//      .Concat(Model.AllColumns.Where(c => !c.IsHidden && !c.Table.IsHidden));

// Get their properties in TSV format (tabulator-separated):
var tsv = ExportProperties(objects,"Name,ObjectType,Parent,Description,FormatString,DataType,Expression,IsHidden,DisplayFolder");

// (Optional) Output to screen (can then be copy-pasted into Excel):
//tsv.Output();

// Delete existing .tsv files
try
{
    File.Delete(pathFilenameTSV);
}
catch
{
}

// Save TSV file
SaveFile(pathFilenameTSV, tsv);

// open excel file
var excelApp = new Excel.Application();
excelApp.Visible = false;
excelApp.DisplayAlerts = false;
excelApp.Workbooks.Open(Filename:pathFilenameExcel, ReadOnly:false, UpdateLinks:0, IgnoreReadOnlyRecommended:true);

// refresh data source
var wb = excelApp.ActiveWorkbook;
var ws = (Excel.Worksheet)wb.Sheets["param"];
((Excel.Range)ws.Cells[2, 1]).Value = pathFilenameTSV;
wb.RefreshAll();

Console.WriteLine(pathFilenameExcel);
try{
    // wb.SaveAs(Filename:pathFilenameExcel, AccessMode:Excel.XlSaveAsAccessMode.xlNoChange);
    wb.SaveAs(Filename:pathFilenameExcel, ConflictResolution:1, FileFormat:Excel.XlFileFormat.xlWorkbookDefault, ReadOnlyRecommended:true, AccessMode:Excel.XlSaveAsAccessMode.xlNoChange);
}
catch (Exception e) {
    Console.WriteLine("Could not safe Excel file. Error Message:" + e.Message);
}
finally{
    // Close workbook and quit Excel program
    wb.Close();
    excelApp.Quit();
    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
}

// Delete tsv export file as it is no longer needed
/*
try
{
    File.Delete(pathFilenameTSV);
}
catch
{
}
*/

