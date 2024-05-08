using ContactProvider.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactProvider.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<ContactEntity> Contacts { get; set; }
}