using System;
using System.Collections.Generic;
using System.Data;

namespace DataTableProxy
{
    public static class FluentTableProxyExtensions
    {
        public static FluentTableProxy<T> WithColumn<T>(this FluentTableProxy<T> ftp, string columnName, Func<T, object> columnData)
        {
            ftp.Dtp.ColumnDefs.Add(columnName, columnData);
            return ftp;
        }

        public static DataTable GetResult<T>(this FluentTableProxy<T> ftp)
        {
            ftp.Dtp.FillTable();
            return ftp.Dtp.Table;
        }
    }
}