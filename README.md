# Kingdom.Protobuf

This repository furnishes some helpful features focused on [Google Protocol Buffers](/protocolbuffers/protobuf).

## Objectives

The primary motivation for this workspace is simply to extrapolate descriptor level details concerning a Protocol Buffer specification.

Support for wire level interpretation of Protocol Buffer messages is beyond ths scope of this workspace.

Additionally, third party extensions to the grammar specification are also beyond the scope of this workspace and are expressly unsupported.

## Limitations

Note, at this time my grammar only supports a subset of the [Protocol Buffer version 2 syntax](http://developers.google.com/protocol-buffers/docs/reference/proto2-spec) for purposes of what I am accomplishing. Support is provided up to but not including the [Service Definition](http://developers.google.com/protocol-buffers/docs/reference/proto2-spec#service_definition).

## Contributions

[Pull Requests](/mwpowellhtx/Kingdom.Protobuf/pulls) are welcome in the event you want to add additional support, i.e. extending through [Service level support](http://developers.google.com/protocol-buffers/docs/reference/proto2-spec#service_definition), support for [version 3](http://developers.google.com/protocol-buffers/docs/reference/proto3-spec), or so on.

## Future Directions

### String Rendering

The key to any string rendering approach is to be able to [specify mission critical options](http://github.com/mwpowellhtx/Kingdom.Protobuf/blob/master/src/Kingdom.Protobuf.Descriptors/Interfaces/IStringRenderingOptions.cs), such as how to render numeric values, such as [``System.Int64``](http://docs.microsoft.com/en-us/dotnet/api/system.int64) or [``long``](http://docs.microsoft.com/en-us/dotnet/api/system.int64) values, or [``System.Double``](http://docs.microsoft.com/en-us/dotnet/api/system.double) or [``double``](http://docs.microsoft.com/en-us/dotnet/api/system.double), or what level of multi-line or single-line comments or whitespace to support.

For the moment, I have implemented an interface, [``ICanRenderString``](http://github.com/mwpowellhtx/Kingdom.Protobuf/blob/master/src/Kingdom.Protobuf.Descriptors/Interfaces/ICanRenderString.cs), for purposes of rendering descriptor elements. In the future, I may consider pursing a more formalized string serialization approach, using a [System.IO.TextWriter](http://docs.microsoft.com/en-us/dotnet/api/system.io.textwriter) based approach, but for now, the implemented interface does just fine for me.

I use the string rendering extensively in order to verify that descriptor [*Abstract Syntax Tree*](http://en.wikipedia.org/wiki/Abstract_syntax_tree) ([*AST*](http://en.wikipedia.org/wiki/Abstract_syntax_tree)) synthesis occurs properly during [ANTLR4](http://github.com/antlr/antlr4) parsing.
