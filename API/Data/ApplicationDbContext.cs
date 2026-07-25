using API.Models.Authorization;
using API.Models.Entities;
using API.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Designation> Designations => Set<Designation>();
    public DbSet<Asset> Assets => Set<Asset>();
    public DbSet<Document> Documents => Set<Document>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<UserPermission> UserPermissions => Set<UserPermission>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Employee -> Department (Many to One)
        builder.Entity<Employee>()
            .HasOne(e => e.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(e => e.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Employee -> Designation (Many to One)
        builder.Entity<Employee>()
            .HasOne(e => e.Designation)
            .WithMany(d => d.Employees)
            .HasForeignKey(e => e.DesignationId)
            .OnDelete(DeleteBehavior.Restrict);

        // Employee -> Manager (Self Reference) An employee can be a Manager who can have multiple members in a team
        builder.Entity<Employee>()
            .HasOne(e => e.Manager)
            .WithMany(e => e.TeamMembers)
            .HasForeignKey(e => e.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Employee -> Documents (One to Many)
        builder.Entity<Document>()
            .HasOne(d => d.Employee)
            .WithMany(e => e.Documents)
            .HasForeignKey(d => d.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Employee -> Assets (One to Many)
        builder.Entity<Asset>()
            .HasOne(a => a.Employee)
            .WithMany(e => e.Assets)
            .HasForeignKey(a => a.EmployeeId)
            .OnDelete(DeleteBehavior.SetNull);

        // Employee -> ApplicationUser (One to One)
        builder.Entity<ApplicationUser>()
            .HasOne(u => u.Employee)
            .WithOne(e => e.User)
            .HasForeignKey<ApplicationUser>(u => u.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Permission name should never duplicate
        builder.Entity<Permission>()
            .HasIndex(p => p.Name)
            .IsUnique();


        builder.Entity<RolePermission>()
            .HasKey(rp => new
            {
                rp.RoleId,
                rp.PermissionId
            });

        builder.Entity<RolePermission>()
            .HasOne(rp => rp.Role)
            .WithMany()
            .HasForeignKey(rp => rp.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<RolePermission>()
            .HasOne(rp => rp.Permission)
            .WithMany(p => p.RolePermissions)
            .HasForeignKey(rp => rp.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<UserPermission>()
            .HasKey(up => new
            {
                up.UserId,
                up.PermissionId
            });

        builder.Entity<UserPermission>()
            .HasOne(up => up.User)
            .WithMany()
            .HasForeignKey(up => up.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<UserPermission>()
            .HasOne(up => up.Permission)
            .WithMany(p => p.UserPermissions)
            .HasForeignKey(up => up.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}