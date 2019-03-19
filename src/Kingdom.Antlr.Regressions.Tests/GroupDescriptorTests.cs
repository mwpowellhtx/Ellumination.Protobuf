namespace Kingdom.Antlr.Regressions.Case.Tests
{
    using Xunit;

    public class GroupDescriptorTests : GroupDescriptorTestFixtureBase
    {
        protected void VerifyGroupDescriptor(string expectedName, bool exceptionExpected = false)
        {
            ExpectedGroup = new GroupDescriptor {Name = expectedName};
            IsExceptionExpected = exceptionExpected;
        }

        [Fact]
        public void ValidNameWorks() => VerifyGroupDescriptor("ValidName123");

        [Fact]
        public void InvalidNameDoesNotWork() => VerifyGroupDescriptor("invalidName123", true);
    }
}
