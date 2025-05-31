using apbd_9.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace apbd_9.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    
}