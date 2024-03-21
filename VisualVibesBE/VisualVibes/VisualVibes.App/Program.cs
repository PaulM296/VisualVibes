using VisualVibes.Domain.Models;

var user1 = new User
{
    UserId = Guid.NewGuid(),
    Username = "Paulinho",
    Password = "123456789",
    UserFeed = new Feed()
};

var feed1 = new Feed
{
    UserId = user1.UserId,
    Posts = new List<Post>()
};
user1.UserFeed = feed1;

var userProfile1 = new UserProfile
{
    UserProfileId = Guid.NewGuid(),
    FirstName = "Paul",
    LastName = "Micluta",
    Email = "paulmicluta@gmail.com",
    ProfilePicture = "URLProfilePicture1",
    DateOfBirth = new DateTime(2000, 06, 29)
};

var user2 = new User
{
    UserId = Guid.NewGuid(),
    Username = "Waganaha",
    Password = "123456",
    UserFeed = new Feed
    {
        UserId = Guid.NewGuid(),
        Posts = new List<Post>()
    }
};
var feed2 = new Feed
{
    UserId = user2.UserId,
    Posts = new List<Post>()
};
user2.UserFeed = feed2;

var userProfile2 = new UserProfile
{
    UserProfileId = Guid.NewGuid(),
    FirstName = "Casian",
    LastName = "Anton",
    Email = "casian.anton@gmail.com",
    ProfilePicture = "URLProfilePicture2",
    DateOfBirth = new DateTime(2000, 08, 09)
};

var user3 = new User
{
    UserId = Guid.NewGuid(),
    Username = "LukeX19",
    Password = "987654321",
    UserFeed = new Feed
    {
        UserId = Guid.NewGuid(),
        Posts = new List<Post>()
    }
};

var feed3 = new Feed
{
    UserId = user3.UserId,
    Posts = new List<Post>()
};
user3.UserFeed = feed3;

var userProfile3 = new UserProfile
{
    UserProfileId = Guid.NewGuid(),
    FirstName = "Catalin",
    LastName = "Mircea",
    Email = "catalin.mircea@gmail.com",
    ProfilePicture = "URLProfilePicture3",
    DateOfBirth = new DateTime(2000, 12, 29)
};

var user4 = new User
{
    UserId = Guid.NewGuid(),
    Username = "Sheina",
    Password = "987654",
    UserFeed = new Feed
    {
        UserId = Guid.NewGuid(),
        Posts = new List<Post>()
    }
};
var feed4 = new Feed
{
    UserId = user4.UserId,
    Posts = new List<Post>()
};
user4.UserFeed = feed4;

var userProfile4 = new UserProfile
{
    UserProfileId = Guid.NewGuid(),
    FirstName = "Diana",
    LastName = "Andreea",
    Email = "diana_andreea@gmail.com",
    ProfilePicture = "URLProfilePicture4",
    DateOfBirth = new DateTime(2000, 03, 08)
};

var user5 = new User
{
    UserId = Guid.NewGuid(),
    Username = "Alexa",
    Password = "456123789",
    UserFeed = new Feed
    {
        UserId = Guid.NewGuid(),
        Posts = new List<Post>()
    }
};

var feed5 = new Feed
{
    UserId = user5.UserId,
    Posts = new List<Post>()
};
user5.UserFeed = feed5;

var userProfile5 = new UserProfile
{
    UserProfileId = Guid.NewGuid(),
    FirstName = "Alexandra",
    LastName = "Miruna",
    Email = "alexandra_miruna@gmail.com",
    ProfilePicture = "URLProfilePicture5",
    DateOfBirth = new DateTime(2000, 08, 29)
};

Console.WriteLine($"User:\n\t-Username: {user1.Username} \n\t-Password: {user1.Password}" +
    $"\nProfile: \n\t-Last name: {userProfile1.LastName} \n\t-First name: {userProfile1.FirstName}" +
    $"\n\t-Email: {userProfile1.Email} \n\t-Birth date: {userProfile1.DateOfBirth} \n\t-Profile picture: {userProfile1.ProfilePicture}");

