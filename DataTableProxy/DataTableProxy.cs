using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DataTableProxy
{
    public class DataTableProxy<T>
    {
        public DataTableProxy()
        {
            Table = new DataTable("results");
            ColumnDefs = EmptyColumnsList();
        }

        public DataTableProxy(Dictionary<string, Func<T, object>> columns)
        {
            ColumnDefs = columns;
        }

        public DataTableProxy(Dictionary<string, Func<T, object>> columns, IEnumerable<T> data)
        {
            ColumnDefs = columns;
            DataSource = data;
        }

        /// <summary>
        /// Each item in this dictionary is the source for a column. The key is the column name, the predicate in the value will be executed on each member of the data source to provide a value for that column.
        /// </summary>
        public Dictionary<string, Func<T, object>> ColumnDefs { get; set; }

        /// <summary>
        /// The table that is built by FillTable- will be null until FillTable is called.
        /// </summary>
        public DataTable Table { get; private set; }

        /// <summary>
        /// Each member of the datasource will become a row for the table. 
        /// </summary>
        public IEnumerable<T> DataSource { get; set; }

        public static Dictionary<string, Func<T, object>> EmptyColumnsList()
        {
            return new Dictionary<string, Func<T, object>>();
        }

        public void FillTable()
        {
            FillTable(false);
        }

        public void FillTable(bool removeEmptyColumns)
        {
            if (DataSource == null || ColumnDefs == null) return;

            if (Table != null) Table.Dispose();
            Table = new DataTable("results");

            var columnNames = ColumnDefs.Keys.ToArray();

            foreach (var name in columnNames)
            {
                Table.Columns.Add(name);
            }

            int i;
            var columnValues = ColumnDefs.Values.ToArray();
            var columnCount = ColumnDefs.Count;
            var usedColumns = new List<int>();

            var data = new List<object[]>();

            foreach (var item in DataSource)
            {
                i = 0;
                var values = new object[columnCount];

                foreach (var value in columnValues)
                {
                    var currentValue = value(item);
                    if (currentValue != null && !currentValue.GetType().IsPrimitive &&
                        !currentValue.GetType().IsValueType) currentValue = currentValue.ToString();
                    values[i] = currentValue;
                    if (values[i] != null && !string.IsNullOrEmpty(values[i].ToString()))
                        if (!usedColumns.Contains(i))
                        {
                            usedColumns.Add(i);
                            Table.Columns[i].DataType = values[i].GetType();
                        }
                    i++;
                }
                data.Add(values);
            }

            foreach (var row in data)
                Table.Rows.Add(row);

            if (removeEmptyColumns)
            {
                var columnsToRemove = new List<DataColumn>();

                for (i = 0; i < columnCount; i++)
                    if (!usedColumns.Contains(i)) columnsToRemove.Add(Table.Columns[i]);

                foreach (var dc in columnsToRemove)
                    Table.Columns.Remove(dc);
            }
        }
    }
}