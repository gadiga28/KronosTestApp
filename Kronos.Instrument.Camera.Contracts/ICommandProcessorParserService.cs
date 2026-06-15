// ------------------------------------------------------------------
// © Copyright 2017 Thermo Fisher Scientific Inc. All rights reserved.
// ------------------------------------------------------------------
using System.Collections.Generic;
using Thermo.Kronos.Instrument.Camera.Contracts.Data;

namespace Thermo.Kronos.Instrument.Camera.Contracts
{
    /// <summary>
    /// Parses the CidApp script files into our ExposureSettings structure
    /// </summary>
    public interface ICommandProcessorParserService
    {
        /// <summary>
        /// Parses the specified script file to a list of CameraExposureSettings.
        /// </summary>
        /// <param name="pathToScriptFile">The path to script file.</param>
        /// <returns></returns>
        IList<CameraExposureSettings> Parse(string pathToScriptFile);
    }
}