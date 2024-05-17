using VisualVibes.App.Interfaces;
using VisualVibes.Infrastructure.Repositories;
using VisualVibes.Infrastructure;
using VisualVibes.App;
using Microsoft.EntityFrameworkCore;
using VisualVibes.Api.Services;
using VisualVibes.Infrastructure.Services;

namespace VisualVibes.Api.Extensions
{
    public static class ServiceConfigurator
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IReactionRepository, ReactionRepository>();
            services.AddScoped<IUserProfileRepository, UserProfileRepository>();
            services.AddScoped<IFeedRepository, FeedRepository>();
            services.AddScoped<IFeedPostRepository, FeedPostRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IConversationRepository, ConversationRepository>();
            services.AddScoped<IUserFollowerRepository, UserFollowerRepository>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void AddMediatR(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IUserRepository).Assembly));
        }

        public static void AddDbContext(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddDbContext<VisualVibesDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
        }

        public static void AddFileSystemLogger(this IServiceCollection services)
        {
            services.AddTransient(provider => new FileSystemLogger("logs"));
        }

        public static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
