using System.Threading.Tasks;

namespace PandaBot
{
    class Program
    {
        // Program entry point, calls Startup class
        public static Task Main(string[] args)
            => Startup.RunAsync(args);
    }
}