// ------------------------------------------------------------------
// © Copyright 2017 Thermo Fisher Scientific Inc. All rights reserved.
// ------------------------------------------------------------------
namespace Thermo.Kronos.Instrument.Camera.Contracts.Enums
{
    /// <summary></summary>
    public enum CameraConnectionStatus
    {
        /// <summary>
        /// The stopped state. (Initial and default state)
        /// </summary>
        Stopped,

        /// <summary>
        /// The initializing state, indicating that the client was just started.
        /// </summary>
        Initializing,

        /// <summary>
        /// The disconnected state, which always occurs after the <code>Initializing</code> state.
        /// </summary>
        Disconnected,

        /// <summary>
        /// The connected state, which indicates that the client is actively connected to a server.
        /// </summary>
        Connected,

        /// <summary>
        /// The connected state, which indicates that a client has either disconnected, or is being disconnected.
        /// </summary>
        Disconnecting
    }
}