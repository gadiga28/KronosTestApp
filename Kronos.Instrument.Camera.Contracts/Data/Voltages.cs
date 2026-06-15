// ------------------------------------------------------------------
// © Copyright 2017 Thermo Fisher Scientific Inc. All rights reserved.
// ------------------------------------------------------------------
namespace Thermo.Kronos.Instrument.Camera.Contracts.Data
{
    /// <summary>
    /// </summary>
    public class Voltages
    {
        /// <summary>
        /// The Select storage voltage B setting.
        /// </summary>
        public float selstoreVb { get; set; }

        /// <summary>
        /// The Select storage voltage C setting.
        /// </summary>
        public float selstoreVc { get; set; }

        /// <summary>
        /// The Select storage voltage A setting.
        /// </summary>
        public float selstoreVa { get; set; }

        /// <summary>
        /// The Select sense voltage B setting.
        /// </summary>
        public float selsenseVb { get; set; }

        /// <summary>
        /// The Select sense voltage C setting.
        /// </summary>
        public float selsenseVc { get; set; }

        /// <summary>
        /// The Select sense voltage A setting.
        /// </summary>
        public float selsenseVa { get; set; }

        /// <summary>
        /// The Select inject voltage B setting.
        /// </summary>
        public float selinjectVb { get; set; }

        /// <summary>
        /// The Select inject voltage C setting.
        /// </summary>
        public float selinjectVc { get; set; }

        /// <summary>
        /// The Select inject voltage D setting.
        /// </summary>
        public float selinjectVd { get; set; }

        /// <summary>
        /// The Ad9826Offset setting.
        /// </summary>
        public float ad9826Offset { get; set; }

        /// <summary>
        /// The Select inject voltage C setting.
        /// </summary>
        public float ad9826Gain { get; set; }

        /// <summary>
        /// The Imrefcds voltage A setting.
        /// </summary>
        public float imrefcdsVa { get; set; }

        /// <summary>
        /// The Imrefcds voltage A setting.
        /// </summary>
        public float impxlbiasVa { get; set; }

        /// <summary>
        /// The Imrefcds voltage A setting.
        /// </summary>
        public float unselinjectVc { get; set; }

        /// <summary>
        /// The Imrefcds voltage A setting.
        /// </summary>
        public float unselsenseVc { get; set; }

        /// <summary>
        /// The Imrefcds voltage A setting.
        /// </summary>
        public float unselstoreVc { get; set; }

        /// <summary>
        /// The Imrefcds voltage A setting.
        /// </summary>
        public float selresetVb { get; set; }

        /// <summary>
        /// The selresetVc voltage C setting.
        /// </summary>
        public float selresetVc { get; set; }

        /// <summary>
        /// The outbiasVd voltage D setting.
        /// </summary>
        public float outbiasVd { get; set; }

        /// <summary>
        /// The outbiasVd voltage D setting.
        /// </summary>
        public float unselresetVc { get; set; }

        /// <summary>
        /// The outbiasVd voltage D setting.
        /// </summary>
        public float seltgVc { get; set; }

        /// <summary>
        /// The outbiasVd voltage D setting.
        /// </summary>
        public float unseltgVc { get; set; }

        /// <summary>
        /// The outbiasVd voltage D setting.
        /// </summary>
        public float pixelvddVa { get; set; }

        /// <summary>
        /// The outbiasVd voltage D setting.
        /// </summary>
        public float videobiasVb { get; set; }

        /// <summary>
        /// The outbiasVd voltage D setting.
        /// </summary>
        public float incdsVc { get; set; }

        /// <summary>
        /// The outbiasVd voltage D setting.
        /// </summary>
        public float imcdsbiasVd { get; set; }

    }
}