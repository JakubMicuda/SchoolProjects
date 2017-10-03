using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PV178.Homeworks.HW06.Infrastructure;
using PV178.Homeworks.HW06.Jobs;
using PV178.Homeworks.HW06.Utils;

namespace PV178.Homeworks.HW06.Tests
{
    [TestClass]
    public class JobSchedulerTests
    {
        #region ExpectedLogs

        private readonly string jobs1ExpectedLog = "Scheduling ImageProcessingContrast job (ID: 1) with AboveAverage priority." + Environment.NewLine +
        "Scheduling ImageProcessingBrightness job (ID: 2) with BelowAverage priority." + Environment.NewLine +
        "Scheduling ImageProcessingContrast job (ID: 3) with AboveAverage priority." + Environment.NewLine +
        "Scheduling ImageProcessingContrast job (ID: 4) with AboveAverage priority." + Environment.NewLine +
        "Scheduling ImageProcessingContrast job (ID: 5) with Normal priority." + Environment.NewLine +
        "Scheduling ImageProcessingBrightness job (ID: 6) with Normal priority." + Environment.NewLine +
        "Scheduling ImageProcessingBrightness job (ID: 7) with Normal priority." + Environment.NewLine +
        "Scheduling ImageProcessingContrast job (ID: 8) with AboveAverage priority." + Environment.NewLine +
        "Scheduling ImageProcessingBrightness job (ID: 9) with Normal priority." + Environment.NewLine +
        "Scheduling ImageProcessingContrast job (ID: 10) with AboveAverage priority." + Environment.NewLine +
        "Scheduling ImageProcessingBrightness job (ID: 11) with Normal priority." + Environment.NewLine +
        "Scheduling ImageProcessingBrightness job (ID: 12) with Normal priority." + Environment.NewLine +
        "Scheduling ImageProcessingContrast job (ID: 13) with BelowAverage priority." + Environment.NewLine +
        "Scheduling ImageProcessingBrightness job (ID: 14) with AboveAverage priority." + Environment.NewLine +
        "Finished job: Changed contrast by 15 point(s), ID: 1" + Environment.NewLine +
        "Finished job: Changed brightness by 80 point(s), ID: 2" + Environment.NewLine +
        "Finished job: Changed contrast by -50 point(s), ID: 3" + Environment.NewLine +
        "Finished job: Changed contrast by 10 point(s), ID: 4" + Environment.NewLine +
        "Finished job: Changed contrast by 10 point(s), ID: 5" + Environment.NewLine +
        "Finished job: Changed brightness by 100 point(s), ID: 6" + Environment.NewLine +
        "Finished job: Changed contrast by 5 point(s), ID: 8" + Environment.NewLine +
        "Finished job: Changed contrast by 50 point(s), ID: 10" + Environment.NewLine +
        "Finished job: Changed brightness by 40 point(s), ID: 14" + Environment.NewLine +
        "Finished job: Changed brightness by -70 point(s), ID: 7" + Environment.NewLine +
        "Finished job: Changed brightness by 65 point(s), ID: 9" + Environment.NewLine +
        "Finished job: Changed brightness by -10 point(s), ID: 11" + Environment.NewLine +
        "Finished job: Changed brightness by -100 point(s), ID: 12" + Environment.NewLine +
        "Finished job: Changed contrast by 20 point(s), ID: 13" + Environment.NewLine;

        private readonly string brightnessExpectedLog = "Finished job: Changed brightness by -25 point(s), ID:";

        private readonly string brightnessMaxChangeExpectedLog = "Finished job: Changed brightness by 100 point(s), ID:";

        private readonly string contrastExpectedLog = "Finished job: Changed contrast by 30 point(s), ID:";

        private readonly string contrastWithoutArgumentExpectedLog = $"Finished job: Changed contrast by {JobSettings.DefaultContrastAdjustment} point(s), ID:";

        private readonly string sortExpectedLog = "Finished job: Sorting of file etmb73.txt was completed";

        #endregion

        [TestInitialize]
        public void Foo()
        {
            JobIdAssigner.Reset();
        }

        /// <summary>
        /// Important: start this test first (otherwise job IDs would not match within diff)
        /// </summary>
        [TestMethod]
        public void JobScheduler_BatchScheduleJobs1_CreatesCorrectLog()
        {
            // arrange
            Action action = () => CommandProcessor.ProcessBatchScheduleCommand("batch-schedule jobs1");

            var jobs1ActualLog = PerformLoggedJobSchedule(action);
            // assert            
            Assert.AreEqual(jobs1ExpectedLog, jobs1ActualLog, "Actual log differs from the expected one, please see the log output.");
        }


        [TestMethod]
        public void JobScheduler_ScheduleContrastWithAllParams_CreatesCorrectLog()
        {
            // arrange
            Action action = () => CommandProcessor.ProcessScheduleCommand("schedule contrast 30 aboveAverage");

            var contrastLog = PerformLoggedJobSchedule(action);
            Assert.AreEqual(true, contrastLog.Contains(contrastExpectedLog), "Actual log does not contain expected output.");
        }

        [TestMethod]
        public void JobScheduler_ScheduleContrastWithSingleParam_CreatesCorrectLog()
        {
            // arrange
            Action action = () => CommandProcessor.ProcessScheduleCommand("schedule contrast 30");

            var contrastLog = PerformLoggedJobSchedule(action);
            Assert.AreEqual(true, contrastLog.Contains(contrastExpectedLog), "Actual log does not contain expected output.");
        }

        [TestMethod]
        public void JobScheduler_ScheduleContrastWithPriority_CreatesCorrectLog()
        {
            // arrange
            Action action = () => CommandProcessor.ProcessScheduleCommand("schedule contrast aboveAverage");

            var contrastLog = PerformLoggedJobSchedule(action);
            Assert.AreEqual(true, contrastLog.Contains(contrastWithoutArgumentExpectedLog), "Actual log does not contain expected output.");
        }

        [TestMethod]
        public void JobScheduler_ScheduleContrastWithoutParams_CreatesCorrectLog()
        {
            // arrange
            Action action = () => CommandProcessor.ProcessScheduleCommand("schedule contrast");

            var contrastLog = PerformLoggedJobSchedule(action);
            Assert.AreEqual(true, contrastLog.Contains(contrastWithoutArgumentExpectedLog), "Actual log does not contain expected output.");
        }

        [TestMethod]
        public void JobScheduler_ScheduleBrightnessWithAllParams_CreatesCorrectLog()
        {
            // arrange
            Action action = () => CommandProcessor.ProcessScheduleCommand("schedule brightness -25 aboveAverage");

            var contrastLog = PerformLoggedJobSchedule(action);
            Assert.AreEqual(true, contrastLog.Contains(brightnessExpectedLog), "Actual log does not contain expected output.");
        }

        [TestMethod]
        public void JobScheduler_ScheduleBrightnessWithHigherThanMaxBrightness_CreatesCorrectLog()
        {
            // arrange
            Action action = () => CommandProcessor.ProcessScheduleCommand("schedule brightness 120");

            var contrastLog = PerformLoggedJobSchedule(action);
            Assert.AreEqual(true, contrastLog.Contains(brightnessMaxChangeExpectedLog), "Actual log does not contain expected output.");
        }

        [TestMethod]
        public void JobScheduler_ScheduleSort()
        {
            // arrange
            Action action = () => CommandProcessor.ProcessScheduleCommand("schedule sort etmb73");

            var sortLog = PerformLoggedJobSchedule(action);
            Assert.AreEqual(true, sortLog.Contains(sortExpectedLog), "Actual log does not contain expected output.");

        }

        /// <summary>
        /// Performs logged job schedule action
        /// </summary>
        /// <param name="action">job schedule action</param>
        /// <returns>Logged content</returns>
        private static string PerformLoggedJobSchedule(Action action)
        {
            LogHelper.OpenLogWriter();

            // act
            action.Invoke();

            while (!JobScheduler.AllJobsHaveFinished())
            {
                Thread.Sleep(200);
            }
            Thread.Sleep(200);

            LogHelper.CloseLogWriter();
            var jobs1ActualLog = LogHelper.ReadAllLoggedText();
            return jobs1ActualLog;
        }

    }
}
