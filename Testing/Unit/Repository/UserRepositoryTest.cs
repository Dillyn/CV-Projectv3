using Domain.Models;
using Infrastructure.Data;
using Integration.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Testing.Repositories
{
    [TestFixture]
    public class UserRepositoryTests
    {
        private ApplicationDBcontext _dbContext;
        private UserRepository _userRepository;

        // One Time Set up an in-memory database before each test
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDBcontext>()
                .UseInMemoryDatabase("TestDatabase")  // Use an in-memory database for testing
                .Options;

            _dbContext = new ApplicationDBcontext(options);
            _userRepository = new UserRepository(_dbContext);

        }
        //seeds data
        [SetUp]
        public void SetUp()
        {
            SeedData();     // Seed data for tests
        }
        // Seed the data manually for testing purposes
        private void SeedData()
        {
            _dbContext.User.AddRange(
                new UserEntity { Id = 11, Name = "John", Surname = "Doe" },
                new UserEntity { Id = 21, Name = "Jane", Surname = "Doe" },
                new UserEntity { Id = 31, Name = "John", Surname = "Smith" }
            );

            _dbContext.SaveChanges();  // Save changes to the in-memory database
        }

        // Test 1: AddAsync should add a user and return the user
        [Test]
        public async Task AddAsync_Should_Add_User()
        {
            var newUser = new UserEntity { Name = "Alice", Surname = "Wonder" };

            var result = await _userRepository.AddAsync(newUser);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Alice"));
            Assert.That(result.Surname, Is.EqualTo("Wonder"));

            var addedUser = await _dbContext.User.FindAsync(result.Id);

            Assert.That(addedUser, Is.Not.Null);
            Assert.That(addedUser.Name, Is.EqualTo("Alice"));
        }

        // Test 2: DeleteAsync should delete a user by ID
        [Test]
        public async Task DeleteAsync_Should_Delete_User()
        {
            var userIdToDelete = 11;
            await _userRepository.DeleteAsync(userIdToDelete);

            var deletedUser = await _dbContext.User.FindAsync(userIdToDelete);
            Assert.That(deletedUser, Is.Null);
        }

        // Test 3: GetAllAsync should return all users
        [Test]
        public async Task GetAllAsync_Should_Return_All_Users()
        {
            var users = await _userRepository.GetAllAsync();

            Assert.That(users, Has.Count.EqualTo(3));
            Assert.That(users, Has.Exactly(1).Matches<UserEntity>(u => u.Name == "John" && u.Surname == "Doe"));
            Assert.That(users, Has.Exactly(1).Matches<UserEntity>(u => u.Name == "Jane" && u.Surname == "Doe"));

        }

        // Test 4: GetByIdAsync should return the user with the given ID
        [Test]
        public async Task GetByIdAsync_Should_Return_User_By_Id()
        {
            var userId = 21;

            var user = await _userRepository.GetByIdAsync(userId);

            Assert.That(user, Is.Not.Null);
            Assert.That(user.Id, Is.EqualTo(userId));
            Assert.That(user.Name, Is.EqualTo("Jane"));

        }

        // Test 5: UpdateAsync should update the user's details
        [Test]
        public async Task UpdateAsync_Should_Update_User()
        {
            var userId = 11;
            var userToUpdate = new UserEntity { Name = "John Updated", Surname = "Doe Updated" };

            var updatedUser = await _userRepository.UpdateAsync(userId, userToUpdate);

            Assert.That(updatedUser, Is.Not.Null);
            Assert.That(updatedUser.Name, Is.EqualTo("John Updated"));
            Assert.That(updatedUser.Surname, Is.EqualTo("Doe Updated"));


            var userFromDb = await _dbContext.User.FindAsync(userId);
            Assert.That(userFromDb.Name, Is.EqualTo("John Updated"));
            Assert.That(updatedUser.Surname, Is.EqualTo("Doe Updated"));
        }

        // Test 6: GetByNameAsync should return users with the given name
        [Test]
        public async Task GetByNameAsync_Should_Return_Users_By_Name()
        {
            var name = "John";

            var users = await _userRepository.GetByNameAsync(name);

            Assert.That(users, Is.Not.Null);
            Assert.That(users, Has.Count.EqualTo(2));
            Assert.That(users, Has.Some.Matches<UserEntity>(u => u.Name == "John"));
        }

        // Clean the database to ensure a fresh state for each test
        private void CleanDatabase()
        {
            _dbContext.User.RemoveRange(_dbContext.User);
            _dbContext.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            CleanDatabase(); // Clean the database to ensure a fresh state
        }

        // Clean up after each test (optional but good practice)
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _dbContext.Database.EnsureDeleted();  // Delete the in-memory database after tests
            _dbContext.Dispose();  // Dispose of the DbContext to release resources
        }
    }
}
