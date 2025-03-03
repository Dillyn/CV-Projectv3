using Domain.Models;
using Domain.Models.Hobby;
using FluentAssertions;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;


namespace Testing.Intergration
{
    [TestFixture]
    public class ApplicationDBcontextTests
    {
        private ApplicationDBcontext _dbContext;

        // One-time setup to initialize the in-memory database and seed data
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDBcontext>()
                .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")  // Ensure each test run uses a unique name
                .Options;

            _dbContext = new ApplicationDBcontext(options);
            _dbContext.Database.EnsureCreated(); // Ensure the schema is created

            // Seed data
            SeedData();
        }

        // Setup method to ensure the database is clean and re-seeded before each test
        [SetUp]
        public void SetUp()
        {
            CleanDatabase(); // Clean the database to ensure a fresh state
            SeedData(); // Seed data for each test
        }

        // Seed the data manually for testing purposes
        private void SeedData()
        {
            var dillynUser = new UserEntity
            {
                Id = 1001,
                Name = "Dillyn",
                Surname = "Lakey"
            };

            var gamingHobby = new HobbyEntity
            {
                Hobby_Id = 1001,
                Title = "Gaming",
                Image = "/online-gaming.jpg"
            };

            var hikingHobby = new HobbyEntity
            {
                Hobby_Id = 2001,
                Title = "Hiking",
                Image = "/hike.jpg"
            };

            var userHobby1 = new UserHobbyEntity
            {
                Id = 1001,
                UserId = 1001,
                HobbyId = 1001,
                Description = "Online gaming has become a major part of my life, providing a fun and exciting escape. Whether I'm teaming up with friends or challenging myself in competitive matches, gaming gives me a sense of achievement and connection."
            };

            var userHobby2 = new UserHobbyEntity
            {
                Id = 2001,
                UserId = 1001,
                HobbyId = 2001,
                Description = "Hiking has become my way of escaping the everyday and reconnecting with nature. Each trail offers something new—whether it’s a challenge, a breathtaking view, or a moment of peace."
            };

            _dbContext.User.Add(dillynUser);
            _dbContext.Hobby.AddRange(gamingHobby, hikingHobby);
            _dbContext.UserHobbies.AddRange(userHobby1, userHobby2);

            _dbContext.SaveChanges(); // Save the seeded data to the in-memory database
        }

        // Clean the database to ensure a fresh state for each test
        private void CleanDatabase()
        {
            _dbContext.User.RemoveRange(_dbContext.User);
            _dbContext.Hobby.RemoveRange(_dbContext.Hobby);
            _dbContext.UserHobbies.RemoveRange(_dbContext.UserHobbies);

            _dbContext.SaveChanges();
        }

        // One-time teardown to clean up resources after all tests have run
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _dbContext.Database.EnsureDeleted(); // Delete the in-memory database after tests
            _dbContext.Dispose(); // Dispose of the context
        }

        // Test 1: Check if user is seeded correctly
        [Test]
        public void Should_Seed_User_Correctly()
        {
            var user = _dbContext.User.FirstOrDefault(u => u.Id == 1001);

            Assert.That(user, Is.Not.Null);
            Assert.That(user.Name, Is.EqualTo("Dillyn"));
            Assert.That(user.Surname, Is.EqualTo("Lakey"));

        }

        // Test 2: Check if hobbies are seeded correctly
        [Test]
        public void Should_Seed_Hobbies_Correctly()
        {
            var gamingHobby = _dbContext.Hobby.FirstOrDefault(h => h.Hobby_Id == 1001);
            var hikingHobby = _dbContext.Hobby.FirstOrDefault(h => h.Hobby_Id == 2001);

            Assert.That(gamingHobby, Is.Not.Null);
            Assert.That(hikingHobby, Is.Not.Null);

            Assert.That(gamingHobby.Title, Is.EqualTo("Gaming"));
            Assert.That(hikingHobby.Title, Is.EqualTo("Hiking"));

        }

        // Test 3: Check if UserHobby relationship is seeded correctly
        [Test]
        public void Should_Seed_UserHobby_Correctly()
        {
            var userHobbies = _dbContext.UserHobbies
                .Where(uh => uh.UserId == 1001)
                .ToList();

            Assert.That(userHobbies, Has.Count.EqualTo(2));


            var gamingHobbyDescription = userHobbies.FirstOrDefault(uh => uh.HobbyId == 1001)?.Description;
            var hikingHobbyDescription = userHobbies.FirstOrDefault(uh => uh.HobbyId == 2001)?.Description;

            Assert.That(gamingHobbyDescription, Is.EqualTo("Online gaming has become a major part of my life, providing a fun and exciting escape. Whether I'm teaming up with friends or challenging myself in competitive matches, gaming gives me a sense of achievement and connection."));
            Assert.That(hikingHobbyDescription, Is.EqualTo("Hiking has become my way of escaping the everyday and reconnecting with nature. Each trail offers something new—whether it’s a challenge, a breathtaking view, or a moment of peace."));

        }

        // Clean up after each test
        [TearDown]
        public void TearDown()
        {
            CleanDatabase(); // Clean the database after each test
        }
    }
}