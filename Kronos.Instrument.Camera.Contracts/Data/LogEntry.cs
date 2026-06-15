// ------------------------------------------------------------------
// © Copyright 2017 Thermo Fisher Scientific Inc. All rights reserved.
// ------------------------------------------------------------------
using System;
using Thermo.Kronos.Instrument.Camera.Contracts.Enums;

namespace Thermo.Kronos.Instrument.Camera.Contracts.Data
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class LogEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogEntry" /> class.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        /// <param name="text">The text.</param>
        public LogEntry(LogLevel logLevel, string text)
        {
            Timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss.fff");
            LogLevel = logLevel;
            Text = text;
        }

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        public string Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the log level.
        /// </summary>
        public LogLevel LogLevel { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text { get; set; }
    }
}