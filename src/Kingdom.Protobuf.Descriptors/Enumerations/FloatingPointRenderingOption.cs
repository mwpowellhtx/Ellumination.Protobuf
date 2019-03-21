using System;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum FloatingPointRenderingOption : uint
    {
        /// <summary>
        /// 
        /// </summary>
        NoFloatingPointRenderingOption = 0,

        /// <summary>
        /// Usually something like <see cref="FloatingPointFixed"/>, but I think it may
        /// also extend into <see cref="FloatingPointScientific"/> for larger numbers.
        /// </summary>
        FloatingPointGeneral = 1,

        /// <summary>
        /// Allowing for greater precision than <see cref="FloatingPointGeneral"/> allows.
        /// </summary>
        FloatingPointRoundTrip = 1 << 1,

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Avoid this setting. We will keep it in for now for sake of completeness, however,
        /// he formatting fails to preserve the expected number of digits heading into the Parser.</remarks>
        [Obsolete("Does not preserve the expected number of digits rendering for the Parser.")]
        FloatingPointFixed = 1 << 2,

        /// <summary>
        /// 
        /// </summary>
        FloatingPointScientific = 1 << 3,

        /// <summary>
        /// 
        /// </summary>
        FloatingPointUpperCase = 1 << 4,

        /// <summary>
        /// 
        /// </summary>
        FloatingPointLowerCase = 1 << 5,

        /// <summary>
        /// 
        /// </summary>
        FloatingPointForceTrailingDot = 1 << 6,

        /// <summary>
        /// 
        /// </summary>
        FloatingPointForceLeadingDot = 1 << 7,

        /// <summary>
        /// Negatively signed values render as expected. However, Positively signed values do not
        /// include the sign. The purpose of this option is to force that the sign be included in
        /// order to exercise that aspect of the Parser.
        /// </summary>
        FloatingPointForceSignage = 1 << 8,

        /// <summary>
        /// The Mask of Format Options.
        /// </summary>
        /// <see cref="FloatingPointGeneral"/>
        /// <see cref="FloatingPointRoundTrip"/>
        /// <see cref="FloatingPointScientific"/>
        FloatingPointFormatOptions = FloatingPointGeneral | FloatingPointRoundTrip | FloatingPointScientific,

        /// <summary>
        /// The Mask of Case Options.
        /// </summary>
        /// <see cref="FloatingPointLowerCase"/>
        /// <see cref="FloatingPointUpperCase"/>
        FloatingPointCaseOptions = FloatingPointLowerCase | FloatingPointUpperCase,

        // ReSharper disable once CommentTypo, IdentifierTypo
        /// <summary>
        /// The Mask of Dottage Options.
        /// </summary>
        /// <see cref="FloatingPointForceLeadingDot"/>
        /// <see cref="FloatingPointForceTrailingDot"/>
        FloatingPointDottageOptions = FloatingPointForceLeadingDot | FloatingPointForceTrailingDot
    }
}
