using Assignment_DataStorage.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_DataStorage.Contexts;

internal class DataContext : DbContext
{
    #region Configure
    // Not Secure!
    private readonly string _connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Simon\Dropbox\Plugg\Webb_.NET\WIN22\Course-5-Databases\Assignment-DataStorage\Assignment-DataStorage\Data\sql-db-local.mdf;Integrated Security=True;Connect Timeout=30";

    // Remove if Dependency Injection
    public DataContext()
    {
    }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    #endregion


    public DbSet<CustomerEntity> Customers { get; set; } = null!;
    public DbSet<BranchEntity> Branches { get; set; } = null!;
    public DbSet<StatusEntity> Status { get; set; } = null!;
    public DbSet<TicketEntity> Tickets { get; set; } = null!;
    public DbSet<CommentEntity> Comments { get; set; } = null!;
}
