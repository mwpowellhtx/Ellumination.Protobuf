using System;
using System.IO;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Antlr4.Runtime;
    using Xunit;
    using Xunit.Abstractions;
    using static String;

    public class ProtocolBufferIntegrationTests : TestFixtureBase
    {
        public ProtocolBufferIntegrationTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourcePath"></param>
        [Theory, ClassData(typeof(ProtocolBufferIntegrationTestCases))]
        public void Protocol_Buffer_Integration_Source_Parses(string resourcePath)
        {
            string GetResourceSource()
            {
                Stream GetResourceStream()
                {
                    var type = GetType();
                    const string streaming = nameof(streaming);
                    OutputHelper.WriteLine(Join(Empty, $"<{streaming}>", $"{type.FullName}.{resourcePath}", $"</{streaming}>"));
                    return type.Assembly.GetManifestResourceStream(type, resourcePath);
                }

                using (var stream = GetResourceStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var s = reader.ReadToEndAsync().Result;
                        const string source = nameof(source);
                        OutputHelper.WriteLine(Join("\r\n", $"<{source}>", s, $"</{source}>"));
                        return s;
                    }
                }
            }

            ProtoParser.ProtoDeclContext EvaluateCallback(ProtoParser parser) => parser.protoDecl();

            ProtoDescriptor EvaluateProtoDescriptor()
            {
                var source = GetResourceSource();

                var listener = source.Trim().WalkEvaluatedContext<ProtoLexer, CommonTokenStream, ProtoParser
                    // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
                    , ProtoParser.ProtoDeclContext, ProtoDescriptorListener>(EvaluateCallback, new DefaultErrorListener { });

                return listener.ActualProto;
            }

            var descriptor = EvaluateProtoDescriptor();

            // TODO: TBD: could jam this through a visitor?
            Assert.NotNull(descriptor);
        }
    }
}
