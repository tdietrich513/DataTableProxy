using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataTableProxy
{
    public static class EnumerableExtensions
    {
        public static FluentTableProxy<T> ToTable<T>(this IEnumerable<T> source)
        {
            var dtp = new DataTableProxy<T> {DataSource = source};
            return new FluentTableProxy<T>(dtp);
        }        
    }
}
