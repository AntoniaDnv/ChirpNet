using ChirpNet.Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChirpNet.Tests.Helpers
{
    public static class TestDbContextFactory
    {
        public static ApplicationDbContext CreateDbContext()
        {
            DbContextOptions<ApplicationDbContext> options =
                new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

            return new ApplicationDbContext(options);
        }
    }
}
