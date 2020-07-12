#if DEBUG

using System;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using Collections.Variants;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Provides <see cref="Variant"/> specific unit test verification.
    /// </summary>
    /// <inheritdoc />
    public class VariantDomainTests : VariantDependencyTestFixtureBase
    {
        /// <inheritdoc />
        /// <remarks>We have to do it this way because <see cref="Variant"/> is static,
        /// so it does not pass via Generic parameter list.</remarks>
        protected override Type FixtureType { get; } = typeof(Variant);

        public VariantDomainTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        protected override void VerifyConfigurationCorrect(IVariantConfigurationCollection configuration)
            => Assert.Collection(InternalConfiguration
                , VerifyConfiguration<ProtoType>
                , VerifyConfiguration<ElementTypeIdentifierPath>
            );

#pragma warning disable xUnit1008
        [InlineData(typeof(Variant))]
        public override void Verify_Type_Is_Expected(Type expectedType) => base.Verify_Type_Is_Expected(expectedType);
#pragma warning restore xUnit1008

    }
}

#endif // DEBUG
