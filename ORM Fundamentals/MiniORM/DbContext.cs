using System;

namespace MiniORM
{
    class DbContext
    {
        public static Type[] AllowedSqlTypes = new[]
        {
            typeof(int),
            typeof(string)
        };
    }
}