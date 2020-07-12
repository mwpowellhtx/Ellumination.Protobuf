#if DEBUG

using System;
using System.Collections.Generic;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using Collections.Variants;
    using Xunit;
    using Xunit.Abstractions;
    using IBytesEnumerable = IEnumerable<byte>;

    /// <summary>
    /// Provides <see cref="Constant"/> specific unit test verification.
    /// </summary>
    /// <inheritdoc />
    public class ConstantDomainTests : VariantDependencyTestFixtureBase
    {
        /// <inheritdoc />
        /// <remarks>We have to do it this way because <see cref="Constant"/> is static,
        /// so it does not pass via Generic parameter list.</remarks>
        protected override Type FixtureType { get; } = typeof(Constant);

        public ConstantDomainTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        protected override void VerifyConfigurationCorrect(IVariantConfigurationCollection configuration)
            => Assert.Collection(InternalConfiguration
                , VerifyConfiguration<bool>
                , VerifyConfiguration<long>
                , VerifyConfiguration<ulong>
                , VerifyConfiguration<float>
                , VerifyConfiguration<double>
                , VerifyConfiguration<string>
                , VerifyConfiguration<IBytesEnumerable>
                , VerifyConfiguration<IdentifierPath>
            );

#pragma warning disable xUnit1008
        [InlineData(typeof(Constant))]
        public override void Verify_Type_Is_Expected(Type expectedType) => base.Verify_Type_Is_Expected(expectedType);
#pragma warning restore xUnit1008
    }
}

#endif // DEBUG
