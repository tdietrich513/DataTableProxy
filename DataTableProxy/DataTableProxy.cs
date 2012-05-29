using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.ComponentModel;

namespace DataTableProxy
{
	public class DataTableProxy<T>
	{
		public static Dictionary<string, Func<T, object>> EmptyColumnsList()
		{
			return new Dictionary<string, Func<T, object>>();
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

		public DataTableProxy()
		{
			Table = new DataTable("results");
			ColumnDefs = EmptyColumnsList();
		}

		public DataTableProxy(Dictionary<string, Func<T, object>> Columns)
		{
			ColumnDefs = Columns;
		}

		public DataTableProxy(Dictionary<string, Func<T, object>> Columns, IEnumerable<T> Data)
		{
			ColumnDefs = Columns;
			DataSource = Data;
		}

		public void FillTable()
		{
			FillTable(false);
		}

		public void FillTable(bool RemoveEmptyColumns)
		{

			if (DataSource == null || ColumnDefs == null) return;

			if (Table != null) Table.Dispose();
			Table = new DataTable("results");

			var ColumnNames = ColumnDefs.Keys.ToArray();

			foreach (string Name in ColumnNames)
			{
				Table.Columns.Add(Name);
			}

			int i;
			object[] values;
			var ColumnValues = ColumnDefs.Values.ToArray();
			var ColumnCount = ColumnDefs.Count;
			List<int> UsedColumns = new List<int>();

			var Data = new List<object[]>();

			foreach (T Item in this.DataSource)
			{
				i = 0;
				values = new object[ColumnCount];

				foreach (Func<T, object> Value in ColumnValues)
				{
					object CurrentValue = Value(Item);
					if (CurrentValue != null && !CurrentValue.GetType().IsPrimitive && !CurrentValue.GetType().IsValueType) CurrentValue = CurrentValue.ToString();
					values[i] = CurrentValue;
					if (values[i] != null && !string.IsNullOrEmpty(values[i].ToString()))
						if (!UsedColumns.Contains(i))
						{
							UsedColumns.Add(i);
							Table.Columns[i].DataType = values[i].GetType();
						}
					i++;
				}
				Data.Add(values);
			}

			foreach (object[] row in Data)
				Table.Rows.Add(row);

			if (RemoveEmptyColumns)
			{
				var ColumnsToRemove = new List<DataColumn>();

				for (i = 0; i < ColumnCount; i++)
					if (!UsedColumns.Contains(i)) ColumnsToRemove.Add(Table.Columns[i]);

				foreach (DataColumn dc in ColumnsToRemove)
					Table.Columns.Remove(dc);
			}
		}
	}
}

