// ------------------------------------------------------------------
// © Copyright 2018 Thermo Fisher Scientific Inc. All rights reserved.
// ------------------------------------------------------------------
using System;
using System.Runtime.Serialization;

namespace Thermo.Kronos.Instrument.Camera.Contracts
{
    /// <summary>
    /// </summary>
    [Serializable]
    public class CameraException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CameraException"/> class.
        /// </summary>
        public CameraException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public CameraException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public CameraException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the System.Exception class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        public CameraException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}