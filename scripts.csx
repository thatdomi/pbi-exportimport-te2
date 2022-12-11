// Export properties for the currently selected objects:
var tsv = ExportProperties(Selected);
SaveFile("test.tsv", tsv);

// Imports and applies the properties in the specified file:
var tsv = ReadFile("testcopy.tsv");
ImportProperties(tsv);


string filenameTSV = "TEST-DataSet-Export.tsv";
// Construct a list of all columns and measures
var objects = Model.Tables.Cast<ITabularNamedObject>().Concat(Model.AllMeasures).Concat(Model.AllColumns);

// Construct a list of all hidden columns and measures:
// var objects = Model.AllMeasures.Where(m => !m.IsHidden && !m.Table.IsHidden).Cast<ITabularNamedObject>()
//      .Concat(Model.AllColumns.Where(c => !c.IsHidden && !c.Table.IsHidden));

// Get their properties in TSV format (tabulator-separated):
var tsv = ExportProperties(objects,"Name,Description,SourceColumn,Expression,FormatString,DataType");

// Save TSV file
SaveFile(filenameTSV, tsv);