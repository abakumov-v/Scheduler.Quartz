using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Models.Options
{
    /// <summary>
    /// Command line arguments.
    /// The name of the properties must fully match with the names of the arguments (case is not important).
    /// </summary>
    public class ConsoleSettings
    {
        public bool Service { get; set; }
        public bool Start { get; set; }
    }
}