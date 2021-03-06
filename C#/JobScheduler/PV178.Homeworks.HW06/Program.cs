﻿using System;
using PV178.Homeworks.HW06.Utils;

namespace PV178.Homeworks.HW06
{
    public class Program
    {
        static void Main(string[] args)
        {
            LogHelper.OpenLogWriter();

            Console.WriteLine("Job scheduler ready.");
            ConsoleHelper.PrintTypeInCommand();
            for (;;)
            {
                var input = Console.ReadLine();
                CommandProcessor.AnalyzeInput(input);
            }
        }
    }
}
