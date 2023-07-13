using AliexpressOpenPlatformAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace AliexpressOpenPlatformAPI.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext _context)
        {
                _context.Database.EnsureCreated();

                var testUsers = await _context.AliExpressDropshipUsers.FirstOrDefaultAsync(b => b.StoreURL == "test.com" || b.StoreURL == "google.com" || b.StoreURL == "facebook.com");
                if (testUsers == null)
                {
                    await _context.AliExpressDropshipUsers.AddAsync(new AliExpressDropshipUser { StoreURL = "test.com" });
                    await _context.AliExpressDropshipUsers.AddAsync(new AliExpressDropshipUser { StoreURL = "google.com" });
                    await _context.AliExpressDropshipUsers.AddAsync(new AliExpressDropshipUser { StoreURL = "facebook.com" });
                }

            await _context.SaveChangesAsync();
        }
    }
}
