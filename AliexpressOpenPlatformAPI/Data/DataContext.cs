using AliexpressOpenPlatformAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace AliexpressOpenPlatformAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        { }
        public DbSet<AliExpressDropshipUser> AliExpressDropshipUsers { get; set; }
       
    }
}