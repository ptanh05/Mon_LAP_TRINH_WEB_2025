using Thực_hành_model.Models;

namespace Thực_hành_model.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User> CreateUserAsync(User user);
        Task<User?> UpdateUserAsync(int id, User user);
        Task<bool> DeleteUserAsync(int id);
    }
    
    public class UserService : IUserService
    {
        private readonly List<User> _users;
        private int _nextId = 1;

        public UserService()
        {
            _users = new List<User>
            {
                new User { Id = _nextId++, Name = "Nguyễn Văn A", Email = "nguyenvana@email.com", Phone = "0123456789" },
                new User { Id = _nextId++, Name = "Trần Thị B", Email = "tranthib@email.com", Phone = "0987654321" },
                new User { Id = _nextId++, Name = "Lê Văn C", Email = "levanc@email.com", Phone = "0369258147" }
            };
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await Task.FromResult(_users.Where(u => u.IsActive).ToList());
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await Task.FromResult(_users.FirstOrDefault(u => u.Id == id && u.IsActive));
        }

        public async Task<User> CreateUserAsync(User user)
        {
            user.Id = _nextId++;
            user.CreatedAt = DateTime.Now;
            user.IsActive = true;
            _users.Add(user);
            return await Task.FromResult(user);
        }

        public async Task<User?> UpdateUserAsync(int id, User user)
        {
            var existingUser = _users.FirstOrDefault(u => u.Id == id && u.IsActive);
            if (existingUser == null)
                return null;

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.Phone = user.Phone;
            
            return await Task.FromResult(existingUser);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id && u.IsActive);
            if (user == null)
                return false;

            user.IsActive = false;
            return await Task.FromResult(true);
        }
    }
}
