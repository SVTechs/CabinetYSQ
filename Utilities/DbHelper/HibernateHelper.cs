namespace Utilities.DbHelper
{
    public class HibernateHelper
    {
        public class WhereEntity
        {
            public string GroupOp;
            public string ColumnName;
            public string ColumnOp;
            public string ColumnValue;
        }

        public class OrderEntity
        {
            public string ColumnName;
            public string Order;
        }
    }
}
