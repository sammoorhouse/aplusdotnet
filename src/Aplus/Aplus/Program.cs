using System;
using AplusCore.Types;
using System.Globalization;
using System.Threading;
using Microsoft.Scripting.Hosting;
using AplusCore.Runtime;
using System.Reflection;
using AplusCore.Compiler;
using Microsoft.Scripting.Runtime;
using System.Text;

namespace Aplus
{

    class Program
    {
        static void Main(string[] args)
        {
            /*
             * hosting: commandline,
               runtime: context,
             
             */

            AplusCore.Hosting.AplusConsoleHost console = new AplusCore.Hosting.AplusConsoleHost();

            Console.WriteLine("RES: {0}", console.Run(args));

            Console.WriteLine("exiting... (press enter)");
            Console.ReadLine();
        }
    }
}
