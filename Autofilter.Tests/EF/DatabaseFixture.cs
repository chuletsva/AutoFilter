using Autofilter.Tests.EF.Common;
using AutoFixture;
using Microsoft.EntityFrameworkCore;

namespace Autofilter.Tests.EF;

public class DatabaseFixture : IAsyncLifetime
{
    public AppDbContext DbContext { get; private set; }

    public async Task InitializeAsync()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        DbContext = new AppDbContextFactory().CreateDbContext();

        await DbContext.Database.MigrateAsync();

        if (!await DbContext.Products.AnyAsync())
        {
            var rnd = new Random();
            var names = new[] { "Snickers", "Mars" };

            var fixture = new Fixture();

            var bools = new[] { true, false };

            var products = fixture.Build<Product>().OmitAutoProperties()
                .With(x => x.Id)
                .With(x => x.Name, () => names[rnd.Next(0, names.Length)] + Guid.NewGuid())
                .With(x => x.Price)
                .With(x => x.IsInStock, () => bools[rnd.Next(0, bools.Length)])
                .With(x => x.IsForSale, () => bools[rnd.Next(0, bools.Length)])
                .With(x => x.ExpireDate, () => DateTime.UtcNow.Add(TimeSpan.FromDays(rnd.Next(1, 20)) + fixture.Create<TimeSpan>()))
                .CreateMany(10_000);

            await DbContext.Products.AddRangeAsync(products);
            await DbContext.SaveChangesAsync();
        }
    }

    public async Task DisposeAsync()
    {
        await DbContext.DisposeAsync();
    }
}