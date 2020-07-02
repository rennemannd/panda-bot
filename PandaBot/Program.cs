using System.Threading.Tasks;

namespace PandaBot
{
    class Program
    {
        public static Task Main(string[] args)
            => Startup.RunAsync(args);
    }
}