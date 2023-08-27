using Microsoft.EntityFrameworkCore;
using startup_kit_api.Models;
using System;

namespace startup_kit_api
{
    public static class ModelBuilderExtension
    {
        public static void FeedRoles(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role()
                {
                    Id = 1,
                    Name = "Admin",
                    CreatedAt = DateTime.Now
                },
                new Role()
                {
                    Id = 2,
                    Name = "User",
                    CreatedAt = DateTime.Now
                }
            );
        }
    }
}
