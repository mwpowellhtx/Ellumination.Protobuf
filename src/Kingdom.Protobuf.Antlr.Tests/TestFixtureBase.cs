// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Xunit.Abstractions;
    using static System.Diagnostics.Debugger;

    public abstract class TestFixtureBase
    {
        protected ITestOutputHelper OutputHelper { get; }

        protected TestFixtureBase(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        // TODO: TBD: may refactor to a static class, along same lines as Identification, Domain, etc
        protected delegate bool DebuggerBreakCallback();

        /// <summary>
        /// Provides a workaround to the lack or removal of Debugger Breakpoint Expression
        /// evaluation. This did once work, so now we need to take other measures, apparently.
        /// </summary>
        /// <param name="callback"></param>
        protected static void BreakDebuggerOn(DebuggerBreakCallback callback)
        {
            if (!callback())
            {
                return;
            }

            Break();
        }
    }
}
