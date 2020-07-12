namespace Ellumination.Antlr.Regressions.Case
{
    using Xunit;
    using Xunit.Abstractions;

    public abstract class GroupDescriptorTestFixtureBase : TestFixtureBase
    {
        protected GroupDescriptorTestFixtureBase(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        /// <summary>
        /// In my production code there is so much more going on, but the rule that has broken
        /// is the Group Name rule.
        /// </summary>
        /// <param name="expectedGroup"></param>
        /// <param name="actualGroup"></param>
        protected override void VerifyGroupDescriptor(GroupDescriptor expectedGroup, GroupDescriptor actualGroup)
        {
            Assert.NotNull(actualGroup);
            Assert.NotSame(expectedGroup, actualGroup);
            Assert.Equal(expectedGroup.Name, actualGroup.Name);
        }
    }
}
