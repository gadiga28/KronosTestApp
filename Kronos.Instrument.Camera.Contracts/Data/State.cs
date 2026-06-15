// ------------------------------------------------------------------
// © Copyright 2018 Thermo Fisher Scientific Inc. All rights reserved.
// ------------------------------------------------------------------
using Thermo.Kronos.Instrument.Camera.Contracts.Enums;

namespace Thermo.Kronos.Instrument.Camera.Contracts.Data
{
    /// <summary/>
    public class State
    {
        /// <summary>
        /// Gets or sets the state level.
        /// </summary>
        public StateLevel StateLevel { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }
    }
}