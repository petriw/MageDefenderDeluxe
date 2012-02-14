using System;

namespace MageDefenderDeluxe
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (MageDefender game = new MageDefender())
            {
                game.Run();
            }
        }
    }
#endif
}

