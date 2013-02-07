DataTableProxy
==============

A quick utility class for converting IEnumerables into easily bindable DataTables. Available as a [NuGet Package][1]. 

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

Or use the new Fluent Syntax:
```csharp
dataGridView1.DataSource = YourObjectRepository.GetAll().ToTable()
   .WithColumn("ID", yo => yo.Id)
   .WithColumn("Name", yo => yo.Name)
   .WithColumn("Last Date", yo => yo.Dates.Max()
   .GetResult();;
```

Notes:
DataTableProxy will intelligently discover the datatype for each column added based on the return type of the first non-null evaluation.
DataTableProxy will call "ToString" on reference-type return values. 

Licence Information
===================
Copyright (c) 2012 Tom Dietrich.
All rights reserved.

Redistribution and use in source and binary forms are permitted.

THIS SOFTWARE IS PROVIDED ''AS IS'' AND WITHOUT ANY EXPRESS OR
IMPLIED WARRANTIES, INCLUDING, WITHOUT LIMITATION, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE.

   [1]: http://nuget.org/packages/DataTableProxy/
