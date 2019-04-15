using System;
using System.IO;
using Antlr4.Runtime;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Xunit;
    using Xunit.Abstractions;
    using static String;

    public class ProtocolBufferIntegrationTests : TestFixtureBase
    {
        public ProtocolBufferIntegrationTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        // ReSharper disable once StringLiteralTypo
        /// <summary>
        /// &quot;#sat_parameter.proto&quot;
        /// </summary>
        private static string ResourcePath => Join(".", "Resources", "#sat_parameters", "proto");

        private Stream ResourceStream
        {
            get
            {
                var type = GetType();
                return type.Assembly.GetManifestResourceStream(type, ResourcePath);
            }
        }

        private string ProtocolBufferSource
        {
            get
            {
                using (var stream = ResourceStream)
                {
                    Assert.NotNull(stream);

                    using (var reader = new StreamReader(stream))
                    {
                        return reader.ReadToEndAsync().Result;
                    }
                }
            }
        }

        [Fact]
        public void Protocol_Buffer_Integration_Source_Parses()
        {
            ProtoParser.ProtoContext EvaluateCallback(ProtoParser parser) => parser.proto();

            ProtoDescriptor EvaluateProtoDescriptor()
            {
                var source = ProtocolBufferSource;

                var listener = source.Trim().WalkEvaluatedContext<ProtoLexer, CommonTokenStream, ProtoParser
                    // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
                    , ProtoParser.ProtoContext, ProtoDescriptorListener>(EvaluateCallback, new DefaultErrorListener { });

                return listener.ActualProto;
            }

            var descriptor = EvaluateProtoDescriptor();

            // TODO: TBD: could jam this through a visitor?
            Assert.NotNull(descriptor);
        }
    }
}
