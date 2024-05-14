using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VisualVibes.App.DTOs.UserDtos;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Users.Commands;
using VisualVibes.App.Users.CommandsHandler;

namespace VisualVibes.IntegrationTests.Helpers
{
    public static class TestHelpers
    {
        public static IMediator CreateMediator(IUnitOfWork unitOfWork)
        {
            var services = new ServiceCollection();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IUserRepository).Assembly));
            services.AddSingleton(unitOfWork);
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddLogging(cfg => cfg.AddConsole())
                .AddTransient(typeof(ILogger<>), typeof(Logger<>));
            
            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetRequiredService<IMediator>();
        }
    }
}
