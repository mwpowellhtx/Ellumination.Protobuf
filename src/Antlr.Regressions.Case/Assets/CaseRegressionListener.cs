using System;
using System.Linq;

namespace Ellumination.Antlr.Regressions.Case
{
    using static DescriptorStackContext;
    using static String;

    public class CaseRegressionListener : CaseRegressionListenerBase
    {
        public override void EnterGroupName(CaseRegressionParser.GroupNameContext context)
        {
            OnEnterSynthesizeAttribute(context, ctx => Empty);
        }

        public override void ExitGroupName(CaseRegressionParser.GroupNameContext context)
        {
            var s = context.GetText();

            using (CreateContext(context, Descriptors
                    , () => TryOnExitResolveSynthesizedAttribute((ref GroupDescriptor x, string _) => x.Name = s)
                )
            )
            {
            }
        }

        public override void EnterGroupDecl(CaseRegressionParser.GroupDeclContext context)
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            GroupDescriptor GetDefault() => new GroupDescriptor { };
            OnEnterSynthesizeAttribute(context, _ => GetDefault());
        }

        public override void ExitGroupDecl(CaseRegressionParser.GroupDeclContext context)
        {
            using (CreateContext(context, Descriptors,
                    // ReSharper disable once RedundantAssignment
                    () => TryOnExitResolveSynthesizedAttribute((ref GroupDescriptor x, GroupDescriptor y) => x = y)
                )
            )
            {
            }
        }

        public override void EnterStart(CaseRegressionParser.StartContext context)
        {
            GroupDescriptor GetDefault() => null;
            OnEnterSynthesizeAttribute(context, _ => GetDefault());
        }

        public override void ExitStart(CaseRegressionParser.StartContext context)
        {
            if (Descriptors.Count > 1)
            {
                throw new InvalidOperationException($"Expected one item in the {nameof(Descriptors)} stack.");
            }

            var (type, value) = Descriptors.First();

            RootDescriptor = value as GroupDescriptor
                             ?? throw new InvalidOperationException(
                                 "Expected root item to be a type of"
                                 + $" '{typeof(GroupDescriptor).FullName}' instead of '{type.FullName}'.");

            Descriptors.Clear();
        }
    }
}
