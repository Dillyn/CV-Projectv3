using Domain.Models;
using Infrastructure.Data;
using Integration.Repositories.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Integration.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBcontext _dbContext;
        public UserRepository(ApplicationDBcontext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> AddAsync(User user)
        {
            try
            {
                await _dbContext.User.AddAsync(user);
                await _dbContext.SaveChangesAsync();
                return user;  // Save changes to the database and return the created user
            }
            catch (Exception ex)
            {

                throw new DatabaseException("An error occurred while adding the user.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var user = await _dbContext.User.FindAsync(id);  // Find the user by ID

                if (user == null)
                {
                    // Handle the case when the user is not found (e.g., log, throw exception, etc.)
                    throw new EntityNotFoundException("User not found");
                }

                _dbContext.User.Remove(user);  // Remove the user from DbSet
                await _dbContext.SaveChangesAsync();  // Save changes to the database
            }
            catch (Exception ex)
            {

                // Log the error
                throw new DatabaseException($"An error occurred while deleting {typeof(User).Name}.", ex);
            }
        }

        public async Task<List<User>> GetAllAsync()
        {
            try
            {
                var users = await _dbContext.User.ToListAsync(); // Retrieve all users from the DbSet asynchronously
                return users;
            }
            catch (Exception ex)
            {

                // Log the error
                throw new DatabaseException("An error occurred while retrieving users.", ex);
            }
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _dbContext.User.FindAsync(id);  // Find and return the user by ID
        }

        public async Task<User> UpdateAsync(int id, User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }

            try
            {
                var existingUser = await _dbContext.User.FindAsync(id);  // Find the existing user by ID

                if (existingUser == null)
                {
                    throw new EntityNotFoundException($"User with ID {id} not found.");
                }

                // Update the properties of the existing user with the values from the passed user object
                existingUser.Name = user.Name;
                existingUser.Surname = user.Surname;
                // Add other properties as needed

                await _dbContext.SaveChangesAsync();  // Save changes to the database

                return existingUser;  // Return the updated user
            }
            catch (Exception ex)
            {
                throw new DatabaseException("An error occurred while updating the user.", ex);
            }
        }


        async Task<User?> IUserRepository.GetByNameAsync(string name)
        {
            return await _dbContext.User.FirstOrDefaultAsync(u => u.Name == name);
        }
    }
}
