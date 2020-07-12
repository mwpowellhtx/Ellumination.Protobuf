namespace Ellumination.Antlr.Regressions.Case
{
    using Xunit;
    using Xunit.Abstractions;

    public class GroupDescriptorTests : GroupDescriptorTestFixtureBase
    {
        public GroupDescriptorTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        protected void VerifyGroupDescriptor(string expectedName, bool exceptionExpected = false)
        {
            ExpectedGroup = new GroupDescriptor {Name = expectedName};
            IsExceptionExpected = exceptionExpected;
        }

        [Fact]
        public void ValidNameWorks() => VerifyGroupDescriptor("Valid123_Name456");

        [Fact]
        public void InvalidNameDoesNotWork() => VerifyGroupDescriptor("invalidName", true);
    }
}
