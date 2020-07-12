using System;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum IntegerRenderingOption : uint
    {
        /// <summary>
        /// 
        /// </summary>
        NoIntegerRenderingOption = 0,

        /// <summary>
        /// hexLit     = "0" ( "x" | "X" ) hexDigit { hexDigit } 
        /// </summary>
        HexadecimalInteger = 1,

        /// <summary>
        /// octalLit   = "0" { octalDigit }
        /// </summary>
        OctalInteger = 1 << 1,

        /// <summary>
        /// decimalLit = ( "1" … "9" ) { decimalDigit }
        /// </summary>
        DecimalInteger = 1 << 2,

        /// <summary>
        /// Does not really enter into the mix apart from Hex formatted integer literals.
        /// </summary>
        UpperCaseInteger = 1 << 3,

        /// <summary>
        /// Does not really enter into the mix apart from Hex formatted integer literals.
        /// </summary>
        LowerCaseInteger = 1 << 4,

        /// <summary>
        /// Negatively signed values render as expected. However, Positively signed values do not
        /// include the sign. The purpose of this option is to force that the sign be included in
        /// order to exercise that aspect of the Parser.
        /// </summary>
        /// <remarks>As far as I know this only applies with working with
        /// <see cref="DecimalInteger"/> formatted values. Or rather, does not apply when
        /// working with <see cref="OctalInteger"/> or <see cref="HexadecimalInteger"/>
        /// formatted values.</remarks>
        IntegerForcedSignage = 1 << 5,

        /// <summary>
        /// The Mask of Format Options.
        /// </summary>
        /// <see cref="HexadecimalInteger"/>
        /// <see cref="OctalInteger"/>
        /// <see cref="DecimalInteger"/>
        IntegerFormatOptions = HexadecimalInteger | OctalInteger | DecimalInteger,

        /// <summary>
        /// The Mask of Case Options.
        /// </summary>
        /// <see cref="LowerCaseInteger"/>
        /// <see cref="UpperCaseInteger"/>
        IntegerCaseOptions = LowerCaseInteger | UpperCaseInteger,

        /// <summary>
        /// The Mask of Signage Options.
        /// </summary>
        /// <see cref="IntegerForcedSignage"/>
        IntegerSignageOptions = IntegerForcedSignage
    }
}
