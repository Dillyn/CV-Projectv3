﻿
using Domain.Models;
using Integration.Repositories;
using Integration.Repositories.Exceptions;
using Services.Services.Exceptions;

namespace Services.Services
{
    public class GUserService(GIRepository _repository)
    {


        public UserEntity GetUserById(int id)
        {
            try
            {
                var user = _repository.GetById<UserEntity>(id);
                if (user == null)
                {
                    throw new UserNotFoundException($"User with ID {id} not found.");
                }
                return user;
            }
            catch (UserNotFoundException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("An error occurred while retrieving the user.", ex);
            }
        }

        public List<UserEntity> GetAllUsers()
        {
            try
            {
                return _repository.GetAll<UserEntity>();
            }
            catch (Exception ex)
            {
                throw new ServiceException("An error occurred while retrieving all users.", ex);
            }
        }

        public void CreateUser(UserEntity user)
        {
            try
            {
                _repository.Create(user);
            }
            catch (Exception ex)
            {
                throw new ServiceException("An error occurred while creating the user.", ex);
            }
        }

        public async Task UpdateUser(UserEntity user)
        {
            try
            {
                // Attempt to update the user in the repository
                await _repository.Update(user);
            }
            catch (EntityNotFoundException ex)
            {
                // Specific handling for a user not being found
                throw new ServiceException($"User with ID {user.Id} not found.", ex);
            }
            catch (ArgumentNullException ex)
            {
                // Specific handling for null argument
                throw new ServiceException("Provided user data is invalid or incomplete.", ex);
            }
            catch (DatabaseException ex)
            {
                // Specific handling for database-related issues
                throw new ServiceException("Database error while updating user.", ex);
            }
            catch (Exception ex)
            {
                // Catch any unexpected exceptions
                throw new ServiceException("An unexpected error occurred while updating the user.", ex);
            }
        }

        public void DeleteUser(int id)
        {
            try
            {
                _repository.Delete<UserEntity>(id);
            }
            catch (Exception ex)
            {
                throw new ServiceException("An error occurred while deleting the user.", ex);
            }
        }

        public List<UserEntity> FindUsersByFirstLetter(char letter)
        {
            if (!char.IsLetter(letter))
            {
                throw new ArgumentException("Invalid character. Please provide a letter.");
            }

            string firstLetter = letter.ToString().ToLower();



            // Filter users by the first name starting with the given letter and retrieves them from the Database
            var filteredUsers = _repository
                .Query<UserEntity>()                                    // returns IQueryable<User>
                .Where(u => !string.IsNullOrEmpty(u.Name) &&      // chacks if string is not null or emty  
                     u.Name.ToLower().StartsWith(firstLetter))              // and checks if name start with firstletter
                        .ToList();


            return filteredUsers;
        }
    }
}
