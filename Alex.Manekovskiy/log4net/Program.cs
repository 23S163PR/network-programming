using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace log4net
{
    class Program
    {
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            try
            {
                if (args.Length < 2)
                {
                    throw new ArgumentException("The number of input parameters should be at least 2.");
                }
                logger.InfoFormat("Received {0} input arguments", args.Length);

                int firstArgument;
                if (int.TryParse(args[0], out firstArgument))
                {
                    logger.DebugFormat("Parsed first argument: {0}", firstArgument);
                }
                else
                {
                    throw new ArgumentException("Argument should be a valid integer number.", "firstArgument");
                }

                int secondArgument;
                if (int.TryParse(args[1], out secondArgument))
                {
                    logger.DebugFormat("Parsed second argument: {0}", secondArgument);
                }
                else
                {
                    throw new ArgumentException("Argument should be a valid integer number.", "secondArgument");
                }

                Console.WriteLine("{0} + {1} = {2}", firstArgument, secondArgument, firstArgument + secondArgument);
            }
            catch (Exception e)
            {
                logger.ErrorFormat("Error occured: {0}", e);
            }

            Console.ReadKey();
        }
    }
}
