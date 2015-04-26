using System;

namespace OpenBackup.ConsoleMode
{
    internal class ColoredConsole : IDisposable
    {
        private ConsoleColor _oldForegroundColor;

        public ColoredConsole(ConsoleColor foregroudColor)
        {
            _oldForegroundColor = Console.ForegroundColor;

            Console.ForegroundColor = foregroudColor;
        }

        public void Dispose()
        {
            Console.ForegroundColor = _oldForegroundColor;
        }
    }
}
