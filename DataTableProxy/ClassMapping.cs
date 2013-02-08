namespace DataTableProxy
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	public class ClassMapping<T> : IEnumerable<ColumnMapping<T>>
	{
		private readonly List<ColumnMapping<T>> _columnMappings;

		public ClassMapping()
		{
			_columnMappings = new List<ColumnMapping<T>>();
		}

		public bool ColumnExists(string name)
		{
			return _columnMappings.Any(cm => cm.ColumnName == name);
		}
		
		public ClassMapping<T> AddColumn(string name, Func<T, object> data)
		{
			if (ColumnExists(name)) RemoveColumn(name);

			_columnMappings.Add(new ColumnMapping<T> { ColumnName = name, ColumnData = data });
			return this;
		}

		public ClassMapping<T> RemoveColumn(string name)
		{
			if (!ColumnExists(name)) return this;

			_columnMappings.RemoveAll(cm => cm.ColumnName == name);
			
			return this;
		}

		public ClassMapping<T> AddAllPropertiesAsColumns()
		{
			var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

			foreach (var prop in props)
			{
				var pi = prop;

				if (ColumnExists(pi.Name)) RemoveColumn(pi.Name);

				_columnMappings.Add(new ColumnMapping<T> { ColumnName = pi.Name, ColumnData = t => pi.GetValue(t, null) });				
			}

			return this;
		}

		public IEnumerator<ColumnMapping<T>> GetEnumerator()
		{
			return _columnMappings.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}