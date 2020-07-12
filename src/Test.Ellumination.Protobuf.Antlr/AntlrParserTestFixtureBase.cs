using System;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;
    using Xunit;
    using Xunit.Abstractions;
    using static Objects;

    // TODO: TBD: this one would be more appropriately named AntlrParserTestFixtureBase ...
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource">It can be a <see cref="Lexer"/>, but it does not have to be.</typeparam>
    /// <typeparam name="TStream"></typeparam>
    /// <typeparam name="TParser"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    /// <typeparam name="TListener"></typeparam>
    /// <typeparam name="TTarget"></typeparam>
    public abstract class AntlrParserTestFixtureBase<TSource, TStream, TParser, TContext, TListener, TTarget> : TestFixtureBase
        where TSource : class, ITokenSource
        where TStream : class, ITokenStream
        where TParser : Parser
        where TContext : class, IParseTree
        where TListener : class, IParseTreeListener
        where TTarget : class
    {
        protected abstract TTarget ExpectedTarget { get; set; }

        protected AntlrParserTestFixtureBase(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        /// <summary>
        /// Verifies the <paramref name="given"/> Rendered String for simple verification.
        /// </summary>
        /// <param name="given"></param>
        private static string VerifyGiven(string given)
        {
            Assert.NotNull(given);
            Assert.NotEmpty(given);
            return given;
        }

        /// <summary>
        /// Override in order to provide the desired <typeparamref name="TContext"/> Callback.
        /// </summary>
        protected abstract AntlrEvaluateParserContextDelegate<TParser, TContext> EvaluateCallback { get; }

        protected delegate string RenderTargetCallback(TTarget target);

        protected string RenderedTarget { get; private set; }

        protected void VerifyParse(RenderTargetCallback render)
        {
            // TODO: TBD: verify Expected Target Not Null?
            // TODO: TBD: or what? return>
            Assert.NotNull(ExpectedTarget);

            //if (ExpectedTarget == null)
            //{
            //    return;
            //}

            RenderedTarget = render(ExpectedTarget);

            OutputHelper.WriteLine($"{nameof(RenderedTarget)}: {RenderedTarget}");

            /* Besides all the Verification going on here, we also provide an Error Listener
             * in case of failure to Render properly. That gives us a halfway decent summary
             * as to where to look for the issue. */

            VerifyListener(
                VerifyGiven(RenderedTarget)
                    .WalkEvaluatedContext<TSource, TStream, TParser, TContext, TListener>(
                        EvaluateCallback, Construct<DefaultErrorListener>().VerifyNotNull()
                    )
            );
        }

        protected delegate void ParseExceptionThrownCallback<in TException>(TException exception)
            where TException : Exception;

        protected void VerifyParse<TThrownException>(RenderTargetCallback render
            , ParseExceptionThrownCallback<TThrownException> callback = null)
            where TThrownException : Exception
        {
            try
            {
                callback?.Invoke(Assert.Throws<TThrownException>(() => VerifyParse(render)));
            }
            finally
            {
                // We must clear the Target Instance afterwards.
                ExpectedTarget = null;
            }
        }

        protected virtual void VerifyListener(TListener listener)
        {
            Assert.NotNull(listener);
        }
    }
}
