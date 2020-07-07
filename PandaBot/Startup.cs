using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PandaBot
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(string[] args)
        {
            var builder = new ConfigurationBuilder();
            Configuration = builder.Build();
        }
        
        public static async Task RunAsync(string[] args)
        {
            var startup = new Startup(args);
            await startup.RunAsync();
        }

        public async Task RunAsync()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            var provider = services.BuildServiceProvider();
            provider.GetRequiredService<Services.CommandHandler>();

            await provider.GetRequiredService<Services.StartupService>().StartAsync();
            await Task.Delay(-1);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig        // adding discord to collection
            {
                LogLevel = LogSeverity.Verbose,
                MessageCacheSize = 1000
            }))
                .AddSingleton(new CommandService(new CommandServiceConfig        // adding command service to collection
            {
                LogLevel = LogSeverity.Verbose,
                DefaultRunMode = RunMode.Async        // forces commands to run async
            }))
                .AddSingleton<Services.CommandHandler>()
                .AddSingleton<Services.StartupService>()
                .AddSingleton<Random>()
                .AddSingleton(Configuration);
        }
    }
}