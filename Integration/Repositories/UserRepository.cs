using Domain.Models;
using Infrastructure.Data;
using Integration.Repositories.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Integration.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBcontext _dbContext;
        private Task<List<UserEntity>> filteredUsers;

        public UserRepository(ApplicationDBcontext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserEntity> AddAsync(UserEntity user)
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
                throw new DatabaseException($"An error occurred while deleting {typeof(UserEntity).Name}.", ex);
            }
        }

        public async Task<List<UserEntity>> GetAllAsync()
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

        public async Task<UserEntity?> GetByIdAsync(int id)
        {
            try { return await _dbContext.User.FindAsync(id); }
            catch (Exception ex)
            {
                throw new EntityNotFoundException($"An error occurred while retrieving the user by ID:{id}.", ex);// Find and return the user by ID
            }
            return await _dbContext.User.FindAsync(id);
        }

        public async Task<UserEntity> UpdateAsync(int id, UserEntity user)
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


        public async Task<List<UserEntity?>> GetByNameAsync(string name)
        {
            try
            {
                var filteredUsers = await _dbContext.User.Where(u => u.Name == name).ToListAsync(); // Find and return the users by Name asynchronously
                return filteredUsers;  // Return the list of users
            }
            catch (Exception ex)
            {
                throw new EntityNotFoundException($"An error occurred while retrieving the user by name: {name}.", ex);
            }
        }


    }
}
