using System;

namespace Core.Models.Infrastructure
{
    /// <summary>
    /// Command line argument model
    /// </summary>
    public class ConsoleArgument
    {
        public string Argument { get; }
        public char ValueSeparator { get; }
        public int ValueSeparatorPosition { get; }
        public string Name { get; }
        public string Value { get; }

        public ConsoleArgument(string argument, char valueSeparator = '=')
        {
            if (string.IsNullOrWhiteSpace(argument))
                throw new ArgumentNullException(nameof(argument));
            Argument = argument;
            ValueSeparator = valueSeparator;

            ValueSeparatorPosition = GetValueSeparatorPosition();
            Name = GetArgumentName();
            Value = GetValue();
        }

        private int GetValueSeparatorPosition()
        {
            return Argument.IndexOf(ValueSeparator);

        }
        private string GetArgumentName()
        {
            if (ValueSeparatorPosition < 0)
                return Argument;

            return Argument.Substring(0, ValueSeparatorPosition);
        }

        private string GetValue()
        {
            var separatorCharLength = 1;
            if (ValueSeparatorPosition < 0)
                return "true";

            var startPositionForSubstringArgumentValue = ValueSeparatorPosition + separatorCharLength;
            
            var argumentValue = Argument.Substring(startPositionForSubstringArgumentValue);
            if(string.IsNullOrWhiteSpace(argumentValue))
                throw new ArgumentException($"The argument \"{Name}\" has no value");
            return argumentValue;
        }
    }
}