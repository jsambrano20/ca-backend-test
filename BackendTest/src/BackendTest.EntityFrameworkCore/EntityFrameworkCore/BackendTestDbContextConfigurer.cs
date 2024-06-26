using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace BackendTest.EntityFrameworkCore
{
    public static class BackendTestDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<BackendTestDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<BackendTestDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
