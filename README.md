DataTableProxy
==============

A quick utility class for converting IEnumerables into easily bindable DataTables

Usage:
```csharp
var dtp = new DataTableProxy<YourObject>();
dtp.ColumnDefs.Add("ID", yo => yo.Id);
dtp.ColumnDefs.Add("Name", yo => yo.Name);
dtp.ColumnDefs.Add("Last Date", yo => yo.Dates.Max());

dtp.DataSource = YourObjectRepository.GetAll();

dtp.FillTable();

dataGridView1.DataSource = dtp.Table;
```

Notes:
DataTableProxy will intelligently discover the datatype for each column added based on the return type of the first non-null evaluation.
DataTableProxy will call "ToString" on reference-type return values. 