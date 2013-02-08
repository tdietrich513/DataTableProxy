using System;
using System.Data;

namespace DataTableProxy
{
	public class ColumnMapping<T>
	{
		public string ColumnName { get; set; }
		public Func<T, object> ColumnData { get; set; }		
	}	
}