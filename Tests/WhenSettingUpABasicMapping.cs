using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using DataTableProxy;

namespace Tests
{
	using System.Data;

	using FluentAssertions;

	[TestFixture]
	public class WhenSettingUpABasicMapping
	{
		private readonly IEnumerable<int> _testData;
		private readonly DataTable _result;

		public WhenSettingUpABasicMapping()
		{
			_testData = Enumerable.Range(1, 100);
			_result = _testData.ToTable(
				new ClassMapping<int>()
					.AddColumn("Value", i => i)
					.AddColumn("Squared", i => i * i)
				);
		}

		[Test]
		public void ThereShouldBeTheCorrectNumberOfColumns()
		{
			_result.Columns.Count.Should().Be(2);			
		}		

		[Test]
		public void TheColumnsShouldBeCorrectlyNamed()
		{
			_result.Columns[0].ColumnName.Should().Be("Value");
			_result.Columns[1].ColumnName.Should().Be("Squared");			
		}

		[Test]
		public void TheNumberOfResultRowsShouldMatchTheSource()
		{
			_result.Rows.Count.Should().Be(_testData.Count());			
		}

		[Test]
		public void FormulasShouldEvaluate()
		{
			for (var rowIndex = 0; rowIndex < _result.Rows.Count; rowIndex ++)
			{
				var value = (int)_result.Rows[rowIndex]["Value"];
				var squared = (int)_result.Rows[rowIndex]["Squared"];

				squared.Should().Be(value * value);
			}
		}

		[Test]
		public void TheDataTypeOfEachColumnShouldBeSet()
		{
			_result.Columns["Value"].DataType.Should().Be(typeof(int));
			_result.Columns["Squared"].DataType.Should().Be(typeof(int));
		}
	}
}
