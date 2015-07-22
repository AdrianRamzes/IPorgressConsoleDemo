using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IPorgressConsoleDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            var progressIndicator = new Progress<int>(ReportProgress);

            Console.WriteLine("Start");
            Console.WriteLine("Press any key to cancel.");
            Task.Run(async () =>
            {
                await DoSomeWorkAsync(progressIndicator, token);
            });
            Console.ReadKey();
            cts.Cancel();
            Console.WriteLine("Cancel Request");
            //Console.WriteLine("Press any key to close application...");
            Console.ReadKey();
        }

        private static Task DoSomeWorkAsync(IProgress<int> progress, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                for (int i = 0; i < int.MaxValue; i++)
                {
                    progress.Report(i);
                    Thread.Sleep(500);

                    if (cancellationToken.IsCancellationRequested)
                    {
                        Console.WriteLine("Canceled!");
                        break;
                    }
                }
            });
        }

        private static void ReportProgress(int value)
        {
            Console.WriteLine("Current progress: {0}", value);
        }
    }
}
