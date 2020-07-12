namespace Ellumination.Antlr.Regressions.Case
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;

    public abstract class CaseRegressionListenerBase : DescriptorSynthesizingListenerBase, ICaseRegressionListener
    {
        public virtual void VisitTerminal(ITerminalNode node)
        {
        }

        public virtual void VisitErrorNode(IErrorNode node)
        {
        }

        public virtual void EnterEveryRule(ParserRuleContext ctx)
        {
        }

        public virtual void ExitEveryRule(ParserRuleContext ctx)
        {
        }

        public virtual void EnterGroupName(CaseRegressionParser.GroupNameContext context)
        {
        }

        public virtual void ExitGroupName(CaseRegressionParser.GroupNameContext context)
        {
        }

        public virtual void EnterGroupDecl(CaseRegressionParser.GroupDeclContext context)
        {
        }

        public virtual void ExitGroupDecl(CaseRegressionParser.GroupDeclContext context)
        {
        }

        public virtual void EnterStart(CaseRegressionParser.StartContext context)
        {
        }

        public virtual void ExitStart(CaseRegressionParser.StartContext context)
        {
        }
    }
}
