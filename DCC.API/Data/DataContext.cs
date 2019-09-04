using DCC.API.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DCC.API.Data
{
    public class DataContext : IdentityDbContext<User, Role, string,
    IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>,
     IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<TreatmentBulletin> TreatmentBulletins { get; set; }
        public DbSet<Drug> Drugs { get; set; }
        public DbSet<Symptom> Symptoms { get; set; }

        public DbSet<BodyAreas> BodyAreas { get; set; }
        public DbSet<Request> Requests { get; set; }
 
        public DbSet<DrugSymptom> DrugSymptom { get; set; }

        public DbSet<DrugType> DrugTyps { get; set; }





        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

                userRole.HasOne(ur => ur.User)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            });

            builder.Entity<Like>()
            .HasKey(k => new { k.LikerId, k.LikeeId });
            builder.Entity<Like>()
            .HasOne(u => u.Likee)
            .WithMany(u => u.Likers)
            .HasForeignKey(u => u.LikeeId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Like>()
          .HasOne(u => u.Liker)
          .WithMany(u => u.Likees)
          .HasForeignKey(u => u.LikerId)
          .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
            .HasOne(u => u.Sender)
            .WithMany(u => u.MessagesSent)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
       .HasOne(u => u.Recipient)
       .WithMany(u => u.MessagesReceived)
       .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<DrugSymptom>().HasKey(dc => new { dc.DrugId, dc.SymptomId });

            builder.Entity<DrugSymptom>()
                .HasOne<Drug>(dc => dc.Drug)
                .WithMany(s => s.DrugSymptom)
                .HasForeignKey(dc => dc.DrugId);


            builder.Entity<DrugSymptom>()
                .HasOne<Symptom>(sc => sc.Symptom)
                .WithMany(s => s.DrugSymptom)
                .HasForeignKey(sc => sc.SymptomId);



            builder.Entity<Drug>()
                   .HasOne(a => a.TreatmentBulletin)
                   .WithOne(b => b.Drug)
                   .HasForeignKey<TreatmentBulletin>(b => b.DrugId);




            builder.Entity<Drug>(entity =>
            {
                entity.HasKey(t => t.DrugId);

                entity.HasOne(t => t.DrugType).WithMany(t => t.Drug).HasForeignKey(t => t.DrugTypeId)
                            .OnDelete(DeleteBehavior.Restrict);

            });






        }

    }
}
