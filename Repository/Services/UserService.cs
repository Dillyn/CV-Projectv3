using Domain.Models;
using Integration.Repositories;
using Integration.Repositories.Exceptions;
using Services.Services.Exceptions;

namespace Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task DeleteAsync(int id)
        {

            // Check if the user exists
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new EntityNotFoundException("User not found");
            }


            await _userRepository.DeleteAsync(id);
        }

        public async Task<List<UserEntity>> GetAllAsync()
        {
            try
            {
                return await _userRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new ServiceException("An error occured while retrieving all users", ex);
            }
        }

        public async Task<UserEntity> GetByFirstLetterOfUserName(string letter)
        {
            try
            {
                return await _userRepository.GetByNameAsync(letter);
            }
            catch (Exception ex)
            {
                throw new ServiceException("An error occured while retrieving the user", ex);
            }
        }

        public Task<UserEntity> GetByFirstLetterOfUserName(char letter)
        {
            throw new NotImplementedException();
        }

        public async Task<UserEntity?> GetByIdAsync(int id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                {
                    throw new UserNotFoundException($"User with ID {id} not found.");
                }
                return user;
            }
            catch (UserNotFoundException ex)
            {
                throw new ServiceException("An error occured while retrieving the user", ex);
            }

        }

        public Task UpdateAsync(UserEntity entity)
        {
            throw new NotImplementedException();
        }

        Task IServices<UserEntity>.AddAsync(UserEntity entity)
        {
            try
            {
                return _userRepository.AddAsync(entity);
            }
            catch (Exception ex)
            {
                throw new ServiceException("An error occured while retrieving all users", ex);
            }


        }

        Task<UserEntity> IServices<UserEntity>.UpdateAsync(UserEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
