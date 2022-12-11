// Construct a list of all tables, columns and measures
var objects = Model.Tables.Cast<ITabularNamedObject>().Concat(Model.AllMeasures).Concat(Model.AllColumns);
// Export properties for the currently selected objects:
var tsv = ExportProperties(objects);
SaveFile("1-basic-export.tsv", tsv);