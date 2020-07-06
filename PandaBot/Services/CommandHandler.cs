using System;
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
        
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly IServiceProvider _provider;

        #endregion

        public CommandHandler(DiscordSocketClient discord, CommandService commands, IServiceProvider provider)
        {
            _commands = commands;
            _discord = discord;
            _provider = provider;

            _discord.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            // Don't process command if it's system message.
            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            // Number to track where the prefix ends and the command begins.
            int argPos = 0;
            var context = new SocketCommandContext(_discord, message);
            
            // Determines if message is command and user is not a bot.
            if (!(message.HasCharPrefix('!', ref argPos) ||
                  message.HasMentionPrefix(_discord.CurrentUser, ref argPos)) || message.Author.IsBot) return;

            // Executes command in command context, also provides service provider for precondition checks.
            var result = await _commands.ExecuteAsync(context, argPos, _provider);

            // replies with error if not successful
            if (!result.IsSuccess)
                await context.Channel.SendMessageAsync(result.ToString());
        }

    }
}