using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Core.Models.Infrastructure
{
    /// <summary>
    /// Model of all possible command line arguments
    /// </summary>
    public class ConsoleArgumentsModel
    {
        public static class Arguments
        {
            public const string IsService = "--service";
            public const string StartMigrationImmediately = "--start";
        }

        public bool IsShowHelp { get; }
        public bool IsNeedStartImmediately { get; }
        public bool IsService { get; }
        public string[] Args { get; }

        public ConsoleArgumentsModel(string[] args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));
            Args = args;

            IsNeedStartImmediately = GetIsNeedStartImmediately();
            IsService = GetIsService();
            IsShowHelp = GetIsShowHelp();
        }

        private bool GetIsShowHelp()
        {
            return !GetIsService() && !GetIsNeedStartImmediately();
        }


        private bool GetIsNeedStartImmediately()
        {
            var isNeedStartImmediatelyArgument = GetArgumentWithValue(Arguments.StartMigrationImmediately);
            if (isNeedStartImmediatelyArgument == null)
                return false;

            var consoleArgument = new ConsoleArgument(isNeedStartImmediatelyArgument);
            return Convert.ToBoolean(consoleArgument.Value);
        }
        private string GetArgumentWithValue(string argument)
        {
            return Args.FirstOrDefault(e => e.Contains(argument));
        }

        private bool GetIsService()
        {
            var isServiceArgument = GetArgumentWithValue(Arguments.IsService);
            if (isServiceArgument == null)
                return false;

            var consoleArgument = new ConsoleArgument(isServiceArgument);
            return Convert.ToBoolean(consoleArgument.Value);
        }
    }
}