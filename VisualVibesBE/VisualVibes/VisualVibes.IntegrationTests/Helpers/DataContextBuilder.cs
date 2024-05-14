using Microsoft.EntityFrameworkCore;
using VisualVibes.Domain.Models.BaseEntity;
using VisualVibes.Infrastructure;

namespace VisualVibes.IntegrationTests.Helpers
{
    public class DataContextBuilder : IDisposable
    {
        private readonly VisualVibesDbContext _dataContext;

        public DataContextBuilder(string dbName = "TestDatabase")
        {
            var options = new DbContextOptionsBuilder<VisualVibesDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var context = new VisualVibesDbContext(options);

            _dataContext = context;
        }

        public VisualVibesDbContext GetDbContext()
        {
            _dataContext.Database.EnsureCreated();
            return _dataContext;
        }

        public List<User> SeedUsers(int number = 1)
        {
            var users = new List<User>();

            for (int i = 0; i < number; i++)
            {
                var id = Guid.NewGuid();

                users.Add(new User
                {
                    Id = id,
                    Username = $"user{i + 1}",
                    Password = "password123",
                });
            }

            _dataContext.AddRange(users);
            _dataContext.SaveChanges();
            return users;
        }

        public User SeedUser()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "user1",
                Password = "password123"
            };

            _dataContext.Users.Add(user);

            _dataContext.SaveChanges();

            return user;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dataContext.Dispose();
            }
        }
    }
}