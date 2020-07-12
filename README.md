# Ellumination.Protobuf

This repository furnishes some helpful features focused on [Google Protocol Buffers](https://github.com/protocolbuffers/protobuf).

## Objectives

The primary motivation for this workspace is simply to extrapolate descriptor level details concerning a Protocol Buffer specification.

Support for wire level interpretation of Protocol Buffer messages is beyond ths scope of this workspace.

Additionally, third party extensions to the grammar specification are also beyond the scope of this workspace and are expressly unsupported.

## Limitations

Note, at this time my grammar only supports a subset of the [Protocol Buffer version 2 syntax](https://developers.google.com/protocol-buffers/docs/reference/proto2-spec) for purposes of what I am accomplishing. Support is provided up to but not including the [Service Definition](https://developers.google.com/protocol-buffers/docs/reference/proto2-spec#service_definition).

## Contributions

[Pull Requests](https://github.com/mwpowellhtx/Ellumination.Protobuf/pulls) are welcome in the event you want to add additional support, i.e. extending through [Service level support](https://developers.google.com/protocol-buffers/docs/reference/proto2-spec#service_definition), support for [version 3](https://developers.google.com/protocol-buffers/docs/reference/proto3-spec), or so on.

## Future Directions

### String Rendering

The key to any string rendering approach is to be able to [specify mission critical options](https://github.com/mwpowellhtx/Ellumination.Protobuf/blob/master/src/Ellumination.Protobuf.Descriptors/Interfaces/IStringRenderingOptions.cs), such as how to render numeric values, such as [`System.Int64`](https://docs.microsoft.com/en-us/dotnet/api/system.int64) or [`long`](https://docs.microsoft.com/en-us/dotnet/api/system.int64) values, or [`System.Double`](https://docs.microsoft.com/en-us/dotnet/api/system.double) or [`double`](https://docs.microsoft.com/en-us/dotnet/api/system.double), or what level of multi-line or single-line comments or whitespace to support.

For the moment, I have implemented an interface, [`ICanRenderString`](https://github.com/mwpowellhtx/Ellumination.Protobuf/blob/master/src/Ellumination.Protobuf.Descriptors/Interfaces/ICanRenderString.cs), for purposes of rendering descriptor elements. In the future, I may consider pursuing a more formalized string serialization approach, using a [System.IO.TextWriter](https://docs.microsoft.com/en-us/dotnet/api/system.io.textwriter) based approach, but for now, the implemented interface does just fine for me.

I use the string rendering extensively in order to [verify](https://github.com/mwpowellhtx/Ellumination.Protobuf/blob/master/src/Ellumination.Protobuf.Antlr.Tests/ProtoParserTestFixtureBase.cs) that descriptor [*Abstract Syntax Tree*](https://en.wikipedia.org/wiki/Abstract_syntax_tree) ([*AST*](https://en.wikipedia.org/wiki/Abstract_syntax_tree)) [synthesis](https://github.com/mwpowellhtx/Ellumination.Protobuf/blob/master/src/Ellumination.Protobuf.Antlr/ProtoDescriptorListener.cs) occurs properly during [ANTLR4](https://github.com/antlr/antlr4) parsing.
