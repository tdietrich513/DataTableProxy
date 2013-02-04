namespace DataTableProxy
{
    public class FluentTableProxy<T>
    {
        internal DataTableProxy<T> Dtp;

        public FluentTableProxy(DataTableProxy<T> dtp)
        {
            Dtp = dtp;
        }
    }
}