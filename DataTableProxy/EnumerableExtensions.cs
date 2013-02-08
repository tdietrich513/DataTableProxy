using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataTableProxy
{
	using System.Data;

	public static class EnumerableExtensions
    {        
		/// <summary>
		/// Returns a datatable sourced from the IEnumerable and with the provided columns.
		/// </summary>		
		/// <param name="source">Data source for the datatable.</param>
		/// <param name="columns">The columns to use. ClassMapping can be used to fluently configure this.></typeparam></param>
		/// <returns>A datatable with the appropriate results.</returns>
		public static DataTable ToTable<T>(this IEnumerable<T> source, IEnumerable<ColumnMapping<T>> columns)
		{
			var dtp = new DataTableProxy<T> { DataSource = source };

			foreach (var cm in columns)
				dtp.ColumnDefs.Add(cm.ColumnName, cm.ColumnData);

			dtp.FillTable();
			return dtp.Table;
		}

		public static DataTable ToTable<T>(this IEnumerable<T> source, ClassMapping<T> columns)
		{
			return ToTable(source, columns.AsEnumerable());
		}
    }
}
