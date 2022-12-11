// set file name
string filenameTSV = "2-doku-export.tsv";
// Construct a list of all tables, columns and measures
var objects = Model.Tables.Cast<ITabularNamedObject>().Concat(Model.AllMeasures).Concat(Model.AllColumns);

// Construct a list of all hidden columns and measures:
// var objects = Model.AllMeasures.Where(m => !m.IsHidden && !m.Table.IsHidden).Cast<ITabularNamedObject>()
//      .Concat(Model.AllColumns.Where(c => !c.IsHidden && !c.Table.IsHidden));

// Get their properties in TSV format (tabulator-separated):
var tsv = ExportProperties(objects,"Name,Description,SourceColumn,Expression,FormatString,DataType");

// Save TSV file
SaveFile(filenameTSV, tsv);