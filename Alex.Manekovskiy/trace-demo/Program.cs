using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trace_demo
{
    public class TraceCategories
    {
        public const string Network = "NETWORK ERROR";
        public const string Configuration = "CONFIGURATION ERROR";
    }

    public class ColoredConsoleTraceListener : ConsoleTraceListener
    {
        public override void WriteLine(string message, string category)
        {
            var previousColor = Console.ForegroundColor;

            ConsoleColor consoleColor;
            switch (category)
            {
                case TraceCategories.Network:
                    consoleColor = ConsoleColor.Yellow;
                    break;
                case TraceCategories.Configuration:
                    consoleColor = ConsoleColor.White;
                    break;
                default:
                    consoleColor = previousColor;
                    break;
            }

            Console.ForegroundColor = consoleColor;

            base.WriteLine(message, category);

            Console.ForegroundColor = previousColor;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Trace.Listeners.Add(new ColoredConsoleTraceListener());

            Trace.WriteLine("We have failed. Exception details:", TraceCategories.Network);
            Trace.WriteLine("We have failed. Exception details:", TraceCategories.Configuration);

            Console.ReadKey();
        }
    }
}
