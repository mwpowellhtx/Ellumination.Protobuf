using System;
using Kingdom.Collections.Variants;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Xunit;
    using Xunit.Abstractions;

    public class VariantDomainTests : VariantDependencyTestFixtureBase
    {
        protected override Type FixtureType { get; } = typeof(Variant);

        public VariantDomainTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        protected override void VerifyConfigurationCorrect(IVariantConfigurationCollection configuration)
        {
            Assert.Collection(InternalConfiguration
                , x =>
                {
                    // TODO: TBD: evaluate the callbacks more substantively?
                    Assert.NotNull(x.EquatableCallback);
                    Assert.NotNull(x.ComparableCallback);
                    Assert.Equal(typeof(ProtoType), x.VariantType);
                }
                , x =>
                {
                    Assert.NotNull(x.EquatableCallback);
                    Assert.NotNull(x.ComparableCallback);
                    Assert.Equal(typeof(ElementTypeIdentifierPath), x.VariantType);
                }
            );
        }
    }
}
