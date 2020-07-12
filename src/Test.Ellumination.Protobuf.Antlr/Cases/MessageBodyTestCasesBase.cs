using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    internal abstract class MessageBodyTestCasesBase : TestCasesBase
    {
        /// <summary>
        /// Override in order to provide the default set of Right Hand Side Cases.
        /// </summary>
        /// <see cref="Cases"/>
        protected abstract IEnumerable<object[]> RightHandSideCases { get; }

        private IEnumerable<object[]> _cases;

        /// <summary>
        /// Gets the Cases in terms of <see cref="RightHandSideCases"/> only. Comprehension
        /// of previously Left Hand Side Cases, that is <see cref="MessageStatement"/>
        /// <see cref="DescriptorBase{T}.Name"/> has been refactored into the test fixture itself.
        /// </summary>
        protected override IEnumerable<object[]> Cases => _cases ?? (_cases = RightHandSideCases.ToArray());
    }
}
