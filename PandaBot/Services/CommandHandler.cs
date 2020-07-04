using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace PandaBot.Services
{
    public class CommandHandler
    {
        #region variable declarations
        
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        
        #endregion

        public CommandHandler(DiscordSocketClient client, CommandService commands)
        {
            _commands = commands;
            _client = client;
        }

        public async Task InstallCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;

            // Discovers all command modules in entry assembly and loads
            await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(),
                services: null);
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            // Don't process command if it's system message.
            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            // Number to track where the prefix ends and the command begins.
            int argPos = 0;

            // Determines if message is command and user is not a bot.
            if (!(message.HasCharPrefix('!', ref argPos) ||
                  message.HasMentionPrefix(_client.CurrentUser, ref argPos)) || message.Author.IsBot) return;

            var context = new SocketCommandContext(_client, message);

            // Executes command in comand context, also provides service provider for precondition checks.
            await _commands.ExecuteAsync(context: context, argPos: argPos, services: null);
        }

    }
}