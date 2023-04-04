using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models;

public partial class AppointmentdbContext : DbContext
{
    public AppointmentdbContext()
    {
    }

    public AppointmentdbContext(DbContextOptions<AppointmentdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySQL("server=localhost;port=3306;user=root;password=;database=appointmentsdb");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
