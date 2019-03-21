using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Xunit.Abstractions;
    using static Collections;
    using static Domain;
    using static Identification;

    public class GroupFieldStatementParserTests : MessageBodyParserTestFixtureBase<GroupFieldStatement>
    {
        public GroupFieldStatementParserTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        private GroupFieldStatement ExpectedGroupField
        {
            get
            {
                void InstallGroupField()
                {
                    if (ExpectedMessage.Items.Any(x => x is GroupFieldStatement))
                    {
                        return;
                    }

                    const LabelKind label = LabelKind.Optional;

                    var groupName = GetGroupName(5);
                    var fieldNumber = FieldNumber;

                    ExpectedMessage.Items.Add(
                        new GroupFieldStatement {Label = label, Name = groupName, Number = fieldNumber}
                    );
                }

                InstallGroupField();

                return ExpectedMessage.Items.Single(x => x is GroupFieldStatement) as GroupFieldStatement;
            }
        }

        /// <summary>
        /// In this case, the <see cref="IMessageBodyItem"/> <see cref="IHasBody{T}"/> is
        /// the <see cref="ITopLevelDefinition"/> <see cref="MessageStatement"/>.
        /// </summary>
        protected override IList<IMessageBodyItem> ExpectedBody
        {
            get => ExpectedGroupField.Items;
            set => ExpectedGroupField.Items = value ?? GetRange<IMessageBodyItem>().ToList();
        }
    }
}
