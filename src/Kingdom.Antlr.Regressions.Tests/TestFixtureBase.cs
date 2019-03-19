using System;
using System.IO;

namespace Kingdom.Antlr.Regressions.Case.Tests
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;
    using Xunit;
    using static String;

    public abstract class TestFixtureBase : IDisposable
    {
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
        private static bool TryVerifyParse(ParseStringCallback source, out GroupDescriptor parsedGroup)
        {
            parsedGroup = null;

            var s = (source() ?? Empty).Trim();

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
                );
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
