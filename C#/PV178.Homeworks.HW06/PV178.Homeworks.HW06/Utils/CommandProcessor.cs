using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using PV178.Homeworks.HW06.Infrastructure;
using PV178.Homeworks.HW06.Model;
using PV178.Homeworks.HW06.Model.Enums;
using PV178.Homeworks.HW06.Model.JobParameters;
using PV178.Homeworks.HW06.Content;

namespace PV178.Homeworks.HW06.Utils
{
    public static class CommandProcessor
    {
        #region Constants
        /// <summary>
        /// Dictionary where:
        /// key is (string) command for corresponding job type
        /// value is the jobtype represented by command
        /// </summary>
        private static readonly Dictionary<string, JobType> AvailableJobsToSchedule = new Dictionary<string, JobType>
        {
            {"contrast", JobType.ImageProcessingContrast},
            {"brightness", JobType.ImageProcessingBrightness},
            {"sort", JobType.Sorting}
        };

        /// <summary>
        /// Schedules job with given name. Note that both priority and argument
        /// are optional but must be given in the right order. (Priority.Normal 
        /// and corresponding constant from JobSettings are used as a default values).
        /// 
        /// Usage: "schedule {jobName} {argument} {jobPriority}"
        /// 
        /// Actual examples:
        /// "schedule contrast 10 AboveAverage" -> schedules contrast increase by 10 points with AboveAverage priority
        /// "schedule contrast -10" -> schedules contrast decrease by 10 points with default (Normal) priority
        /// </summary>
        private const string ScheduleJobCommand = "schedule";

        /// <summary>
        /// Schedules all jobs within given file name with given name.
        /// 
        /// Usage: "batch-schedule {filename}"
        /// 
        /// Actual examples:
        /// "batch-schedule jobs1" -> schedules all jobs within file jobs1.txt
        /// </summary>
        private const string BatchScheduleJobCommand = "batch-schedule";

        /// <summary>
        /// Cancels currently running job.
        /// Usage: "cancel"
        /// </summary>
        private const string CancelJobCommand = "cancel";

        /// <summary>
        /// Lists all available jobs, or more precisely jobName 
        /// and a short description for every available job.
        /// Usage: "list"
        /// </summary>
        private const string ListAllJobsCommand = "list";

        /// <summary>
        /// Lists all available commands.
        /// Usage: "help"
        /// </summary>
        private const string HelpCommand = "help";

        /// <summary>
        /// Terminates the program.
        /// Usage: "exit"
        /// </summary>
        private const string ExitCommand = "exit";

        #endregion

        /// <summary>
        /// Analyzes user input
        /// </summary>
        /// <param name="input">user input</param>
        public static void AnalyzeInput(string input)
        {
            ConsoleHelper.EraseTypeInText();

            if (String.IsNullOrEmpty(input))
            {
                return;
            }
            var lowerCaseInput = input.ToLower();

            if (lowerCaseInput.Contains(ListAllJobsCommand))
            {
                ProcessListAllJobsCommand();
                ConsoleHelper.PrintTypeInCommand();
                return;
            }
            if (lowerCaseInput.Contains(BatchScheduleJobCommand))
            {
                ProcessBatchScheduleCommand(lowerCaseInput);
                ConsoleHelper.PrintTypeInCommand();
                return;
            }
            if (lowerCaseInput.Contains(ScheduleJobCommand))
            {
                ProcessScheduleCommand(lowerCaseInput);
                ConsoleHelper.PrintTypeInCommand();
                return;
            }

            if (lowerCaseInput.Contains(CancelJobCommand))
            {
                ProcessCancelCommand();
                ConsoleHelper.PrintTypeInCommand();
                return;
            }
            if (lowerCaseInput.Contains(HelpCommand))
            {
                ProcessHelpCommand();
                ConsoleHelper.PrintTypeInCommand();
                return;
            }
            if (lowerCaseInput.Contains(ExitCommand))
            {
                Environment.Exit(0);
            }
            Console.WriteLine($"Command '{input}' was not recognized, type 'help' to see valid commands." + Environment.NewLine);
        }

        /// <summary>
        /// Writes all available jobs
        /// </summary>
        private static void ProcessListAllJobsCommand()
        {
            Console.WriteLine("Available jobs:");
            foreach (var keyValuePair in AvailableJobsToSchedule)
            {
                Console.WriteLine($"Name: {keyValuePair.Key}, Description: {keyValuePair.Value}");
            }
            ConsoleHelper.PrintTypeInCommand();
        }

        /// <summary>
        /// Writes all available commands
        /// </summary>
        private static void ProcessHelpCommand()
        {
            Console.WriteLine("Supported commands: (command - description)" + Environment.NewLine);

            Console.WriteLine("'schedule {jobName} {argument} {jobPriority}' - Schedules job, both argument and priority are optional."
                + Environment.NewLine + "Example: 'schedule contrast 25 AboveAverage'" + Environment.NewLine);
            Console.WriteLine("'batch-schedule {filename}' - Schedules all jobs within given file name with given name."
                + Environment.NewLine + "Example: 'batch-schedule jobs1'" + Environment.NewLine);
            Console.WriteLine("'list' - Lists jobName and a short description for every available job.");
            Console.WriteLine("'cancel' - Cancels currently running job.");
            Console.WriteLine("'exit' - Terminates the program.");
        }

        /// <summary>
        /// Schedules job extracted from user input 
        /// (in case of incorrect format, error message should be displyed in console)
        /// </summary>
        /// <param name="lowerCaseInput">user input</param>
        public static void ProcessScheduleCommand(string lowerCaseInput)
        {
            try
            {
                string[] commandUserInput;
                ScheduleCommandData data = ParseScheduleInput(lowerCaseInput,out commandUserInput);
                ScheduleParams param = new ScheduleParams(data.Priority, data.Argument, commandUserInput);

                Job processedJob = JobResolver.Resolve(param, data.JobName);
                JobScheduler.ScheduleJob(processedJob);
            }
            catch (ParseCommandException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Schedules multiple jobs extracted from file given via user input 
        /// (in case of incorrect format, error message should be displyed in console)
        /// </summary>
        public static void ProcessBatchScheduleCommand(string lowerCaseInput)
        {
            string fileName = ParseBatchScheduleInput(lowerCaseInput);

            using (var fileStream = File.Open(Paths.BatchProcessJob(fileName), FileMode.Open))
            using (var reader = new StreamReader(fileStream))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    ProcessScheduleCommand(line);
                }
            }
        }

        /// <summary>
        /// Cancels currently running job
        /// </summary>
        private static void ProcessCancelCommand()
        {
            JobScheduler.CancelCurrentJob();
        }

        private static ScheduleCommandData ParseScheduleInput(string input,out string[] commandUserInput)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(ScheduleJobCommand) || AvailableJobsToSchedule == null)
            {
                throw new ParseCommandException("missing arguments");
            }

            ScheduleCommandData output = new ScheduleCommandData();
            commandUserInput = input.Split(' ');

            if (commandUserInput.Length > 4 || commandUserInput.Length < 2)
            {
                throw new ParseCommandException($"unsupported number of arguments: {commandUserInput.Length - 1}");
            }
            if (commandUserInput[0] != ScheduleJobCommand)
            {
                throw new ParseCommandException($"unknown command: {commandUserInput[0]}");
            }
            if (!AvailableJobsToSchedule.ContainsKey(commandUserInput[1]))
            {
                throw new ParseCommandException($"unsupported job type: {commandUserInput[1]}");
            }

            output.JobName = AvailableJobsToSchedule[commandUserInput[1]];
            output.Argument = string.Empty;
            output.Priority = JobPriority.Normal;

            if (commandUserInput.Length == 3)
            {
                JobPriority? priority = GetPriority(commandUserInput[2]);
                if (priority.HasValue)
                {
                    output.Priority = priority.Value;
                    return output;
                }
            }

            if (commandUserInput.Length > 2)
            {
                output.Argument = commandUserInput[2];
            }

            if (commandUserInput.Length > 3)
            {
                JobPriority? priority = GetPriority(commandUserInput[3]);
                if (priority.HasValue)
                {
                    output.Priority = priority.Value;
                }else throw new ParseCommandException($"unknown priority: {commandUserInput[3]}");
            }

            return output;
        }

        private static JobPriority? GetPriority(string input)
        {
            JobPriority? output = null;

            switch (input.ToLower())
            {
                case "normal":
                    output = JobPriority.Normal;
                    break;

                case "aboveaverage":
                    output = JobPriority.AboveAverage;
                    break;

                case "belowaverage":
                    output = JobPriority.BelowAverage;
                    break;
            }
            return output;
        }

        private static string ParseBatchScheduleInput(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ParseCommandException("missing arguments");
            }

            string[] splittedData = input.Split(' ');

            if (splittedData[0] != BatchScheduleJobCommand)
            {
                throw new ParseCommandException($"unknown command: {splittedData[0]}");
            }

            if (splittedData.Length != 2)
            {
                throw new ParseCommandException($"unsupported number of arguments: {splittedData.Length - 1}");
            }

            return splittedData[1];
        }
    }

    public struct ScheduleCommandData
    {
        public JobType JobName { get; set; }
        public string Argument { get; set; }
        public JobPriority Priority { get; set; }
    }

    public class ParseCommandException : Exception
    {
        public ParseCommandException(string message) : base(message)
        {

        }
    }
}
