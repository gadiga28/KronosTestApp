// ------------------------------------------------------------------
// © Copyright 2017 Thermo Fisher Scientific Inc. All rights reserved.
// ------------------------------------------------------------------
using System;

namespace Thermo.Kronos.Instrument.Camera.Contracts.Data
{
    /// <summary>
    /// </summary>
    [Serializable]
    public class CameraRegion
    {
        /// <summary>
        /// Gets or sets the start point x in pixel.
        /// </summary>
        public short PositionX
        {
            get { return m_positionX; }
            set { m_positionX = value; }
        }

        /// <summary>
        /// Gets or sets the start point y in pixel.
        /// </summary>
        public short PositionY
        {
            get { return m_positionY; }
            set { m_positionY = value; }
        }

        /// <summary>
        /// Gets or sets the width of the region in pixel.
        /// </summary>
        public short Width
        {
            get { return m_width; }
            set { m_width = value; }
        }

        /// <summary>
        /// Gets or sets the height of the region in pixel.
        /// </summary>
        public short Height
        {
            get { return m_height; }
            set { m_height = value; }
        }

        #region private fields and constants
        private short m_width;
        private short m_height;
        private short m_positionX;
        private short m_positionY;
        #endregion
    }
}