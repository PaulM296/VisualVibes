using Microsoft.Extensions.DependencyInjection;
using VisualVibes.App;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;
using VisualVibes.Infrastructure.Repositories;


var diContainer = new ServiceCollection()
                 .AddScoped<IUserRepository, UserRepository>()
                 .AddTransient<FileSystemLogger>(provider => new FileSystemLogger("logs"))
                 .BuildServiceProvider();

var logger = diContainer.GetRequiredService<FileSystemLogger>();

var user1 = new User
{
    Id = Guid.NewGuid(),
    Username = "Paulinho",
    Password = "123456789",
    UserFeed = new Feed()
};

var feed1 = new Feed
{
    Id = user1.Id,
    Posts = new List<Post>()
};
user1.UserFeed = feed1;

var userProfile1 = new UserProfile
{
    Id = Guid.NewGuid(),
    FirstName = "Paul",
    LastName = "Micluta",
    Email = "paulmicluta@gmail.com",
    ProfilePicture = "URLProfilePicture1",
    DateOfBirth = new DateTime(2000, 06, 29)
};

var user2 = new User
{
    Id = Guid.NewGuid(),
    Username = "Waganaha",
    Password = "123456",
    UserFeed = new Feed
    {
        Id = Guid.NewGuid(),
        Posts = new List<Post>()
    }
};
var feed2 = new Feed
{
    Id = user2.Id,
    Posts = new List<Post>()
};
user2.UserFeed = feed2;

var userProfile2 = new UserProfile
{
    Id = Guid.NewGuid(),
    FirstName = "Casian",
    LastName = "Anton",
    Email = "casian.anton@gmail.com",
    ProfilePicture = "URLProfilePicture2",
    DateOfBirth = new DateTime(2000, 08, 09)
};

var user3 = new User
{
    Id = Guid.NewGuid(),
    Username = "LukeX19",
    Password = "987654321",
    UserFeed = new Feed
    {
        Id = Guid.NewGuid(),
        Posts = new List<Post>()
    }
};

var feed3 = new Feed
{
    Id = user3.Id,
    Posts = new List<Post>()
};
user3.UserFeed = feed3;

var userProfile3 = new UserProfile
{
    Id = Guid.NewGuid(),
    FirstName = "Catalin",
    LastName = "Mircea",
    Email = "catalin.mircea@gmail.com",
    ProfilePicture = "URLProfilePicture3",
    DateOfBirth = new DateTime(2000, 12, 29)
};

var user4 = new User
{
    Id = Guid.NewGuid(),
    Username = "Sheina",
    Password = "987654",
    UserFeed = new Feed
    {
        Id = Guid.NewGuid(),
        Posts = new List<Post>()
    }
};
var feed4 = new Feed
{
    Id = user4.Id,
    Posts = new List<Post>()
};
user4.UserFeed = feed4;

var userProfile4 = new UserProfile
{
    Id = Guid.NewGuid(),
    FirstName = "Diana",
    LastName = "Andreea",
    Email = "diana_andreea@gmail.com",
    ProfilePicture = "URLProfilePicture4",
    DateOfBirth = new DateTime(2000, 03, 08)
};

var user5 = new User
{
    Id = Guid.NewGuid(),
    Username = "Alexa",
    Password = "456123789",
    UserFeed = new Feed
    {
        Id = Guid.NewGuid(),
        Posts = new List<Post>()
    }
};

var feed5 = new Feed
{
    Id = user5.Id,
    Posts = new List<Post>()
};
user5.UserFeed = feed5;

var userProfile5 = new UserProfile
{
    Id = Guid.NewGuid(),
    FirstName = "Alexandra",
    LastName = "Miruna",
    Email = "alexandra_miruna@gmail.com",
    ProfilePicture = "URLProfilePicture5",
    DateOfBirth = new DateTime(2000, 08, 29)
};

Console.WriteLine($"User:\n\t-Username: {user1.Username} \n\t-Password: {user1.Password}" +
    $"\nProfile: \n\t-Last name: {userProfile1.LastName} \n\t-First name: {userProfile1.FirstName}" +
    $"\n\t-Email: {userProfile1.Email} \n\t-Birth date: {userProfile1.DateOfBirth} \n\t-Profile picture: {userProfile1.ProfilePicture}");


var userRepository = diContainer.GetRequiredService<IUserRepository>();
var allUsersTest = userRepository.GetAllAsync();

userRepository.AddAsync(user1);
userRepository.AddAsync(user2);
userRepository.AddAsync(user3);
userRepository.AddAsync(user4);
userRepository.AddAsync(user5);
userRepository.AddAsync(user5);
userRepository.AddAsync(user1);

var allUsers = await userRepository.GetAllAsync();

foreach(var user in allUsers)
{
    Console.WriteLine($"User:\n\tUserId: {user.Id}\n\t-Username: {user.Username}\n\t-Password: {user.Password}\n\t");
}

userRepository.GetAllAsync();
userRepository.GetByIdAsync(user1.Id);
userRepository.GetByIdAsync(user2.Id);
userRepository.GetByIdAsync(user3.Id);
userRepository.GetByIdAsync(Guid.NewGuid());
userRepository.GetByIdAsync(user4.Id);
userRepository.GetByIdAsync(user5.Id);
userRepository.GetByIdAsync(Guid.NewGuid());

userRepository.RemoveAsync(user1);
userRepository.RemoveAsync(user5);
userRepository.RemoveAsync(user5);

foreach (var user in allUsers)
{
    Console.WriteLine($"User:\n\tUserId: {user.Id}\n\t-Username: {user.Username}\n\t-Password: {user.Password}\n\t");
}
user2.Username = "ChangedUsername";
userRepository.UpdateAsync(user2);

foreach (var user in allUsers)
{
    Console.WriteLine($"User:\n\tUserId: {user.Id}\n\t-Username: {user.Username}\n\t-Password: {user.Password}\n\t");
}


string loggingFilePath = "C:\\Users\\paul.micluta\\Desktop\\VisualVibes\\VisualVibes\\VisualVibesBE\\VisualVibes\\VisualVibes.App\\bin\\Debug\\net8.0\\logs\\Logs_26-03-2024.txt";
string loggingContent = await logger.ReadLogFileAsync(loggingFilePath);

Console.WriteLine(loggingContent);