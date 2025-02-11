using CBT.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;

namespace CBT.API.Data
{

    public class CBTDbContext : DbContext
    {
    public CBTDbContext(DbContextOptions<CBTDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<VRoute> Routes { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Payment> Payments { get; set; }
    }

}
