using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Discord;
using Discord.WebSocket;

namespace PandaBot
{
    public class Startup
    {
        #region vairable delcarations
        
        private readonly DiscordSocketClient _client;
        private readonly string _token;
        
        #endregion
        
        public Startup(string[] args)
        {
            _client = new DiscordSocketClient();
            _client.Log += Log;
            
            _token = JsonConvert.DeserializeObject<dynamic>(File.ReadAllText("config.json")).Token;
        }
        
        public static async Task RunAsync(string[] args)
        {
            var startup = new Startup(args);
            await startup.RunAsync();
        }

        public async Task RunAsync()
        {
            await _client.LoginAsync(TokenType.Bot, _token);
            await _client.StartAsync();
            
            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private Task Log(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }
    }
}