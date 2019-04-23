using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using static CollectionHelpers;

    internal class ProtocolBufferIntegrationTestCases : TestCasesBase
    {
        private static IEnumerable<object[]> _privateCases;

        private static IEnumerable<object[]> PrivateCases
        {
            get
            {
                /* Copied bop_parameters.proto aside and "modified" the C/C++-style to be v2 specification
                 * compliant. Otherwise, the files are all verbatim copies of the originals. */
                IEnumerable<object[]> GetAll()
                {
                    foreach (var x in GetRange("boolean_problem", "#bop_parameters"
                        , "flow_problem", "linear_solver", "parameters", "sat_parameters"))
                    {
                        yield return GetRange<object>($"Resources.{x}.proto").ToArray();
                    }
                }

                return _privateCases ?? (_privateCases = GetAll());
            }
        }

        protected override IEnumerable<object[]> Cases => PrivateCases;
    }
}
