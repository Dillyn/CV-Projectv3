using Domain.Models;
using Domain.Models.Hobby;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDBcontext : IdentityDbContext<UserI>//Inherit from IdentityDbContext
    {
        public ApplicationDBcontext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }//Pass up to Db context
        public DbSet<UserEntity> User { get; set; }// set table
        public DbSet<HobbyEntity> Hobby { get; set; }// set table

        public DbSet<UserHobbyEntity> UserHobbies { get; set; } // Join table for many-to-many relationship between User and Hobby

        // Overriding OnModelCreating to add Fluent API configuration
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Call base class' OnModelCreating for Identity configurations

            // Seed the user - Dillyn Lakey
            var dillynUserId = 1;  // For example, assuming this is the first user added.
            modelBuilder.Entity<UserEntity>().HasData(
                new UserEntity
                {
                    Id = dillynUserId,
                    Name = "Dillyn",
                    Surname = "Lakey"
                }
            );

            // Seed hobbies
            var gamingHobbyId = 1;
            var hikingHobbyId = 2;

            modelBuilder.Entity<HobbyEntity>().HasData(
                new HobbyEntity
                {
                    Hobby_Id = gamingHobbyId,
                    Title = "Gaming",
                    Image = "/online-gaming.jpg"
                },
                new HobbyEntity
                {
                    Hobby_Id = hikingHobbyId,
                    Title = "Hiking",
                    Image = "/hike.jpg"
                }
            );

            // Seed the UserHobbies relationship (assigning hobbies to Dillyn)
            modelBuilder.Entity<UserHobbyEntity>().HasData(
                new UserHobbyEntity
                {
                    Id = 1,
                    UserId = dillynUserId,
                    HobbyId = gamingHobbyId,
                    Description = "Online gaming has become a major part of my life, providing a fun and exciting escape. Whether I'm teaming up with friends or challenging myself in competitive matches, gaming gives me a sense of achievement and connection."
                },
                new UserHobbyEntity
                {
                    Id = 2,
                    UserId = dillynUserId,
                    HobbyId = hikingHobbyId,
                    Description = "Hiking has become my way of escaping the everyday and reconnecting with nature. Each trail offers something new—whether it’s a challenge, a breathtaking view, or a moment of peace. I’ve learned to appreciate the simple things: the fresh air, the sounds of the forest, and the satisfaction of reaching the summit."
                }
            );

            // Configure the UserHobby entity
            modelBuilder.Entity<UserHobbyEntity>()
                .HasKey(uh => uh.Id);  // Explicitly define the primary key for UserHobby

            // Configure the relationship between UserHobby and User
            modelBuilder.Entity<UserHobbyEntity>()
                .HasOne(uh => uh.User)  // A UserHobby belongs to one User
                .WithMany(u => u.UserHobbies)  // A User can have many UserHobbies
                .HasForeignKey(uh => uh.UserId);  // UserId is the foreign key in UserHobby

            // Configure the relationship between UserHobby and Hobby
            modelBuilder.Entity<UserHobbyEntity>()
                .HasOne(uh => uh.Hobby)  // A UserHobby belongs to one Hobby
                .WithMany(h => h.UserHobbies)  // A Hobby can have many UserHobbies
                .HasForeignKey(uh => uh.HobbyId);  // HobbyId is the foreign key in UserHobby
        }
    }
}