using System.Collections.Generic;
using System.Data;

namespace DSHelper.Data
{
    public interface IRepository<TDataRow>
        where TDataRow : DataRow
    {
        TDataRow Get(int id);
        IEnumerable<TDataRow> List();
        void Save(TDataRow entity);
        void Delete(TDataRow entity);
        void Update();
    }
}
