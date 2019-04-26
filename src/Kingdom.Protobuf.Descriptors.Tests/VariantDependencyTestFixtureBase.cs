#if DEBUG

using System;
using System.Linq;
using System.Reflection;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Collections.Variants;
    using Xunit;
    using Xunit.Abstractions;
    using static Type;
    using static BindingFlags;

    public abstract class VariantDependencyTestFixtureBase : TestFixtureBase
    {
        /// <summary>
        /// Override in order to obtain the desired Type.
        /// </summary>
        protected abstract Type FixtureType { get; }

        /// <summary>
        /// Gets the Configuration given the <see cref="FixtureType"/>.
        /// By contract we will agree that the internal names shall be consistent.
        /// </summary>
        protected IVariantConfigurationCollection InternalConfiguration
        {
            get
            {
                var fixtureType = FixtureType.AssertNotNull();

                const BindingFlags flags = Static | NonPublic | GetProperty | DeclaredOnly;

                var propertyInfo = fixtureType.GetProperty(nameof(InternalConfiguration)
                    , flags, DefaultBinder, typeof(IVariantConfigurationCollection)
                    , GetRange<Type>().ToArray(), GetRange<ParameterModifier>().ToArray());

                var obj = propertyInfo.AssertNotNull().GetValue(null);

                return Assert.IsAssignableFrom<VariantConfigurationCollection>(obj.AssertNotNull());
            }
        }

        protected VariantDependencyTestFixtureBase(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Fact]
        public void Type_Is_Static() => Assert.True(FixtureType.AssertNotNull().IsStatic());

        [Fact]
        public void Configuration_Is_Generated() => Assert.NotNull(InternalConfiguration);

        [Fact]
        public void Yields_Different_Configuration_Instances() => GetRange(
            InternalConfiguration, InternalConfiguration
            , InternalConfiguration, InternalConfiguration).AssertAllDifferent();

        protected abstract void VerifyConfigurationCorrect(IVariantConfigurationCollection configuration);

        [Fact]
        public void Configuration_Is_Correct() => VerifyConfigurationCorrect(InternalConfiguration);
    }
}

#endif // DEBUG
