
// Construct a list of all columns and measures
var objects = Model.Tables.Cast<ITabularNamedObject>().Concat(Model.AllMeasures).Concat(Model.AllColumns);
// Get their properties in TSV format (tabulator-separated):
var tsv = ExportProperties(objects,"Name,ObjectType,Parent,Description,FormatString,DataType,Expression,IsHidden,DisplayFolder");
// Save TSV file
SaveFile("TEST-DataSet-Export.tsv", tsv);

#r "System.IO"
#r "Microsoft.Office.Interop.Excel"

using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

string filePath = @"C:\\Users\\dominic\\Syntera\\GL ORG - Documents\\Sales\\Power BI Meetup\\"; // Update this to be the location of the Dokumentation Excel, add \\ to the end
string excelFileName = "3-Full-DataSet-Dokumentation.xlsx"; // Update this to the full excel file name of the Documentation
string excelTabName = "technical Documentation"; // Update excel sheetname where 
string excelFilePath = filePath + excelFileName;


// Open Excel
var excelApp = new Excel.Application();
excelApp.Visible = false;
excelApp.DisplayAlerts = false;

// Open Workbook, Worksheet
var wb = excelApp.Workbooks.Open(excelFilePath); 
var ws = wb.Worksheets[excelTabName] as Excel.Worksheet;

// Count rows and columns
Excel.Range xlRange = ws.UsedRange;

int rowCount = xlRange.Rows.Count;

for (int r = 2; r <= rowCount; r++)
{   
    string tableName = (string)(ws.Cells[r,1] as Excel.Range).Text.ToString();
    string objType = (string)(ws.Cells[r,2] as Excel.Range).Text.ToString();
    string objName = (string)(ws.Cells[r,3] as Excel.Range).Text.ToString();
    string desc = (string)(ws.Cells[r,4] as Excel.Range).Text.ToString();
    desc = desc.Replace(System.Environment.NewLine, "\r\n");
    string isHidden = (string)(ws.Cells[r,6] as Excel.Range).Text.ToString().ToLower();
    string formatString = (string)(ws.Cells[r,8] as Excel.Range).Text.ToString();
    string displayFolder = (string)(ws.Cells[r,9] as Excel.Range).Text.ToString();
    
    //Helper to output variables
    //tableName.Output();
    //objType.Output();
    //objName.Output();
    //desc.Output();
    //isHidden.Output();
    //formatString.Output();
    //displayFolder.Output();
    
    if (objType == "Table")
    {
        try
        {
            Model.Tables[tableName].Description = desc; 
            Model.Tables[tableName].IsHidden = Convert.ToBoolean(isHidden);
        }
        catch
        {
        }
    }
    else if (objType == "Column")
    {
        try
        {
            Model.Tables[tableName].Columns[objName].Description = desc;
            Model.Tables[tableName].Columns[objName].IsHidden = Convert.ToBoolean(isHidden);
            Model.Tables[tableName].Columns[objName].FormatString = formatString;
            Model.Tables[tableName].Columns[objName].DisplayFolder = displayFolder;
        }
        catch
        {            
        }
    }
    else if (objType == "Measure")
    {
        try
        {
            Model.Tables[tableName].Measures[objName].Description = desc;
            Model.Tables[tableName].Measures[objName].IsHidden = Convert.ToBoolean(isHidden);
            Model.Tables[tableName].Measures[objName].FormatString = formatString;
            Model.Tables[tableName].Measures[objName].DisplayFolder = displayFolder;
        }
        catch
        {
        }
    }
    /*else if (objType == "Hierarchy")
    {
        try
        {
            Model.Tables[tableName].Hierarchies[objName].Description = desc;
        }
        catch
        {
        }
    }
    else if (objType == "Calculation Group")
    {
        try
        {
            Model.Tables[tableName].Description = desc;
        }
        catch
        {
        }
    }
    else if (objType == "Calculation Item")
    {
        try
        {
            (Model.Tables[tableName] as CalculationGroupTable).CalculationItems[objName].Description = desc;
        }
        catch
        {
        }
    }*/
}

// Close workbook and quit Excel program
wb.Close();
excelApp.Quit();
System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);