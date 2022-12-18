// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Dotnet.AuditApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            //var item = new Item { Name = "Ice Coffee", Description = "Coffee", Type = "Coffee" };

            //var broker = new StorageBroker();
            //broker.Add(item);
            //broker.SaveChanges();

            var broker = new LocalStorage();
            var itemForUpdate = new Item { Id = 1, Name = "Juice" };
            broker.Update(itemForUpdate);
            broker.SaveChanges();

            IQueryable<Item> itemsHistory = broker.Items.TemporalAll();

            foreach (var item in itemsHistory)
            {
                Console.WriteLine(item.Name);
            }

        }
    }
}

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    public string TypeDescription { get; set; }

}


public class LocalStorage : DbContext
{
    public LocalStorage() => 
        this.Database.Migrate();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=DotnetAuditDB");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>()
            .ToTable(
                name: "Items", 
                itemsTable => 
                    itemsTable.IsTemporal(t =>
                    {
                        t.UseHistoryTable("Items_AT");
                    }
                ));
    }

    public DbSet<Item> Items { get; set; }
}