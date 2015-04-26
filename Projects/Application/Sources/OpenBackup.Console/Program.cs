using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using OpenBackup.Framework;
using OpenBackup.Runtime;

namespace OpenBackup.ConsoleMode
{
    public static class Program
    {
        public static void Main()
        {
            InitializeTrace();
            StartDiag();
            Run();
            ShutdownTrace();
            StopDiag();
        }

        private static void InitializeTrace()
        {
            DefaultTraceSource.Instance = new TraceSource("Open Backup")
            {
                Switch = new SourceSwitch("All")
                {
                    Level = SourceLevels.All
                }
            };
        }

        private static void ShutdownTrace()
        {
            DefaultTraceSource.Instance.Flush();
        }

        private static void StartDiag()
        {
            if (!CommandLine.IsSwitchSpecified("diag"))
                return;

            Diag.Start();
        }

        private static void StopDiag()
        {
            if (!CommandLine.IsSwitchSpecified("diag"))
                return;

            Diag.Stop();
        }

        private static void Run()
        {
            var binPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            DefaultTraceSource.TraceInformation("Bin Path: {0}", binPath);

            var loader = new SolutionLoader();
            loader.RegisterExtensionProvider(new DirectoryExtensionProvider(binPath));

            var solution = loader.LoadSolution(CommandLine.GetArgument(1));

            var preview = CommandLine.IsSwitchSpecified("preview");
            DefaultTraceSource.TraceInformation("Preview Mode: {0}", preview);

            var job = CommandLine.GetSwitchValue("job");

            if (job != null)
            {
                var jobsToRun = solution.Jobs.Where(j => string.Equals(j.Name, job, StringComparison.OrdinalIgnoreCase)).ToArray();

                if (jobsToRun.Length != 0)
                    RunJobs(jobsToRun, preview);
                else
                    using (new ColoredConsole(ConsoleColor.DarkRed))
                        Console.WriteLine("Cannot find job '{0}' to run.", job);
            }
            else
            {
                RunJobs(solution.Jobs, preview);
            }
        }

        private static void RunJobs(IEnumerable<IJob> jobs, bool preview)
        {
            foreach (var job in jobs)
                RunJob(job, preview);
        }

        private static void RunJob(IJob job, bool preview)
        {
            var engine = new Engine();

            Console.WriteLine();

            using (var coloredConsole = new ColoredConsole(ConsoleColor.Green))
                Console.WriteLine("Running Job: {0}", job.Name);

            var startTime = DateTime.Now;

            foreach (var operation in engine.RunJob(job))
            {
                using (new ColoredConsole(ConsoleColor.DarkGray))
                    Console.Write(DateTime.Now.ToString("HH:mm:ss "));

                using (new ColoredConsole(!preview ? ConsoleColor.White : Console.ForegroundColor))
                    Console.WriteLine(operation.ToString());

                if (!preview)
                    operation.Execute();
            }

            var endTime = DateTime.Now;
            var errors = engine.Errors.ToList();

            if (errors.Count > 0)
            {
                using (new ColoredConsole(ConsoleColor.DarkRed))
                    foreach (var error in errors)
                        if (error is OperationException)
                            Console.WriteLine("Error: {0} when executing operation: {1}", error.Message, ((OperationException)error).Operation);
                        else
                            Console.WriteLine("Error: {0}", error.Message);
            }

            Console.WriteLine("Time elapsed: {0}", endTime - startTime);
        }
    }
}
