using NCrawler.Interfaces;
using NCrawler.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BiggWhaleDataCollector
{
    static class Program
    {
        public static IFilter[] ExtensionsToSkip = new[]
            {
                (RegexFilter)new Regex(@"(\.jpg|\.css|\.js|\.gif|\.jpeg|\.png|\.ico|\.pdf)",
                    RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)
            };

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
