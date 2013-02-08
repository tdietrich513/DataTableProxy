namespace Tests
{
	using System.Collections.Generic;
	using System.Data;

	using DataTableProxy;

	using FluentAssertions;

	using NUnit.Framework;

	[TestFixture]
	public class WhenMappingViaReflection
	{
		public class TestClass
		{
			public int Id { get; set; }
			public string Name { get; set; }
			public decimal OtherValue { get; set; }
		}

		private readonly List<TestClass> _testData;
		private readonly DataTable _result;

		public WhenMappingViaReflection()
		{
			_testData = new List<TestClass>();
			_testData.Add(new TestClass { Id = 1, Name = "First", OtherValue = 0.01M });
			_testData.Add(new TestClass { Id = 2, Name = "Second" });
			_testData.Add(new TestClass { Id = 3, Name = "Third", OtherValue = 99.0M });
			_testData.Add(new TestClass { Id = 4, Name = "Fourth", OtherValue = 212M });
			_testData.Add(new TestClass { Id = 5, Name = "Fifth", OtherValue = 18.29M });

			_result = _testData.ToTable(new ClassMapping<TestClass>().AddAllPropertiesAsColumns());
		}

		[Test]
		public void AllPropertiesShouldGetColumns()
		{
			var columnNames = new List<string>();
			for (var colIndex = 0; colIndex < _result.Columns.Count; colIndex++)
			{
				columnNames.Add(_result.Columns[colIndex].ColumnName);
			}

			columnNames.Should().Contain(new[]{ "Id", "Name", "OtherValue" });
		}		
	}
}