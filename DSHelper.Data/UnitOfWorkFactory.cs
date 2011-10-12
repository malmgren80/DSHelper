using System;

namespace DSHelper.Data
{
    public static class UnitOfWorkFactory
    {
        [ThreadStatic]
        public static IUnitOfWork Current; 

        public static IUnitOfWork Start()
        {
            Current = new UnitOfWork();
            return Current;
        }
    }
}
