using System;
using System.IO;

namespace Ellumination.Antlr.Regressions.Case
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;
    using Xunit;
    using Xunit.Abstractions;
    using static String;

    public abstract class TestFixtureBase : IDisposable
    {
        protected ITestOutputHelper OutputHelper { get; }

        protected TestFixtureBase(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        protected delegate string ParseStringCallback();

        /// <summary>
        /// Gets or Sets whether IsExceptionExpected. Defaults to True.
        /// </summary>
        protected bool IsExceptionExpected { get; set; } = true;

        /// <summary>
        /// Gets or Sets the ExpectedGroup.
        /// </summary>
        protected GroupDescriptor ExpectedGroup { get; set; }

        /// <summary>
        /// Tries to Parse the <see cref="string"/> obtained by <paramref name="source"/> into
        /// the <paramref name="parsedGroup"/>.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="parsedGroup"></param>
        /// <returns></returns>
        private bool TryVerifyParse(ParseStringCallback source, out GroupDescriptor parsedGroup)
        {
            parsedGroup = null;

            var s = (source() ?? Empty).Trim();

            OutputHelper.WriteLine($"Given: {s}");

            /* I've taken the time to isolate a more helpful scaffold than the brute force
             * creation up of assets, but this will work for verification purposes for now... */

            using (var reader = new StringReader(s))
            {
                var charStream = new AntlrInputStream(reader);

                var lexer = new CaseRegressionLexer(charStream);

                var tokenStream = new CommonTokenStream(lexer);

                var parser = new CaseRegressionParser(tokenStream);

                // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
                parser.AddErrorListener(new DefaultErrorListener { });

                // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
                var listener = new CaseRegressionListener { };

                new ParseTreeWalker().Walk(listener, parser.start());

                parsedGroup = listener.RootDescriptor;
            }

            return parsedGroup != null;
        }

        protected abstract void VerifyGroupDescriptor(GroupDescriptor expectedGroup, GroupDescriptor actualGroup);

        protected virtual void Dispose(bool disposing)
        {
            if (IsExceptionExpected)
            {
                Assert.Throws<InvalidOperationException>(
                    () => TryVerifyParse(() => ExpectedGroup?.RenderString(), out _)
                ).VerifyException(exception =>
                {
                    const int zed = 0;
                    Assert.True(zed < (int) exception.Data["line"]);
                    Assert.True(zed < (int) exception.Data["charPositionInLine"]);
                    OutputHelper.WriteLine($"Message: {exception.Data["msg"]}");
                });
            }
            else
            {
                Assert.True(TryVerifyParse(() => ExpectedGroup?.RenderString(), out var actualGroup));
                VerifyGroupDescriptor(ExpectedGroup, actualGroup);
            }
        }

        public void Dispose() => Dispose(true);
    }
}
