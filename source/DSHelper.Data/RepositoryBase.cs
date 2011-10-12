using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DSHelper.Data
{
    public abstract class RepositoryBase
    {
        protected DbConnection Connection { get { return UnitOfWorkFactory.Current.Connection; } }
    }
}
