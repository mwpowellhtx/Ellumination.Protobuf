using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using Xunit.Abstractions;
    using static CollectionHelpers;

    public class MessageStatementParserTests : MessageBodyParserTestFixtureBase<MessageStatement>
    {
        public MessageStatementParserTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        /// <summary>
        /// In this case, the <see cref="IMessageBodyItem"/> <see cref="IHasBody{T}"/> is
        /// the <see cref="ITopLevelDefinition"/> <see cref="MessageStatement"/>.
        /// </summary>
        protected override IList<IMessageBodyItem> ExpectedBody
        {
            get => ExpectedMessage.Items;
            set => ExpectedMessage.Items = value ?? GetRange<IMessageBodyItem>().ToList();
        }
    }
}
