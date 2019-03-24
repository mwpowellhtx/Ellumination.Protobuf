// https://github.com/tunnelvisionlabs/antlr4cs
// https://github.com/antlr/antlr4/blob/master/doc/csharp-target.md#how-do-i-create-and-run-a-custom-listener
// https://developers.google.com/protocol-buffers/docs/reference/proto2-spec
// https://github.com/antlr/antlr4/blob/master/doc/lexer-rules.md
// https://github.com/antlr/antlr4/blob/master/doc/parser-rules.md
// https://community.esri.com/thread/182866-mismatch-between-the-processor-architecture-of-the-project-being-built-msil-and-the-processor-architecture-of-the-reference-oracledataaccess
// https://stackoverflow.com/questions/47752764/there-was-a-mismatch-between-the-processor-architecture-of-the-project-being-bui
// https://stackoverflow.com/questions/10113532/how-do-i-fix-the-visual-studio-compile-error-mismatch-between-processor-archit

// This grammar supports a subset of the Protocol Buffers Version 2. We support up to but
// not including the Service declaration, RPC, or Streams. This is the extent of the support
// we require for what we are trying to accomplish with this project. If additional elements,
// code generation, etc, are required, we will gladly consider Pull Requests for said
// functionalityaligned with our licensing model.

grammar Proto;

// TODO: TBD: for now, re-routing White Space and Comments to the HIDDEN channel. 
// TODO: TBD: eventually, I could see potentially re-routing Comments to another Channel.
// Support for Single Line and Multi Line Comments.
SINGLELINE: '//' ~[\r\n]* NEWLINE? -> channel(HIDDEN);

// We do not care about NEWLINE for MULTILINE, in fact, we would not expect that.
MULTILINE: '/*' .+? '*/' -> channel(HIDDEN);

WS: [ \r\n\t]+ -> channel(HIDDEN);

// NEWLINE : '\r'? '\n' { {System.out.println("Newlines so far: " + (++counter)); } };
NEWLINE: '\r'? '\n';

// capitalLetter =  "A" … "Z"
fragment CAP_LET: [A-Z];
// decimalDigit = "0" … "9"
fragment DIG: [0-9];
fragment DIG_19: [1-9];
fragment E: [Ee];
// It is worth establishing lexical comprehension for EXPONENT.
fragment EXP: [Ee] [+-]? [0-9]+;
fragment PERIOD: '.';
// hexDigit     = "0" … "9" | "A" … "F" | "a" … "f"
fragment HEX_DIG: [0-9a-zA-Z];
// letter = "A" … "Z" | "a" … "z"
fragment LET: [a-zA-Z];
// octalDigit   = "0" … "7"
fragment OCT_DIG: [0-7];
fragment SIGNAGE: [+-];
fragment UNDERSCORE: '_';
fragment X: [Xx];
fragment ZED: [0];

// Protocol Buffer Tokens
PROTO2: 'proto2';

CLOSE_ANGLE_BRACKET: '>';
CLOSE_CURLY_BRACE: '}';
CLOSE_PAREN: ')';
CLOSE_SQUARE_BRACKET: ']';
COMMA: ',';
DEFAULT: 'default';
DOT: PERIOD;
ENUM: 'enum';
EOS: ';';
EQU: '=';
EXTEND: 'extend';
EXTENSIONS: 'extensions';
FIELD: 'field';
GROUP: 'group';
IMPORT: 'import';
MAP: 'map';
MAX: 'max';
MESSAGE: 'message';
ONEOF: 'oneof';
OPEN_ANGLE_BRACKET: '<';
OPEN_CURLY_BRACE: '{';
OPEN_PAREN: '(';
OPEN_SQUARE_BRACKET: '[';
OPTION: 'option';
OPTIONAL: 'optional';
PACKAGE: 'package';
PUBLIC: 'public';
REPEATED: 'repeated';
REQUIRED: 'required';
RESERVED: 'reserved';
//RETURNS: 'returns';
//RPC: 'rpc';
//SERVICE: 'service';
SIGN: SIGNAGE;
//STREAM: 'stream';
SYNTAX: 'syntax';
TO: 'to';
WEAK: 'weak';

// Type Keywords
// https://developers.google.com/protocol-buffers/docs/proto#scalar
// https://developers.google.com/protocol-buffers/docs/proto3#scalar
BOOL: 'bool';
BYTES: 'bytes';
DOUBLE: 'double';
FIXED32: 'fixed32';
FIXED64: 'fixed64';
FLOAT: 'float';
INT32: 'int32';
INT64: 'int64';
SFIXED32: 'sfixed32';
SFIXED64: 'sfixed64';
SINT32: 'sint32';
SINT64: 'sint64';
STRING: 'string';
UINT32: 'uint32';
UINT64: 'uint64';

BOOLEAN_FALSE: 'false';
BOOLEAN_TRUE: 'true';

HEX_LIT: ZED X HEX_DIG+;
OCT_LIT: ZED OCT_DIG*;
DEC_LIT: DIG_19 DIG*;

INFINITY: 'inf';
NOT_A_NUMBER: 'nan';

FLOAT_DIG_DOT_DIG_OPT_EXP: DIG+ PERIOD DIG* EXP?;
FLOAT_DIG_EXP: DIG+ EXP;
FLOAT_DOT_DIG_OPT_EXP: PERIOD DIG+ EXP?;

/*
// Supports both cases:
ident = letter { letter | decimalDigit | "_" }
groupName = capitalLetter { letter | decimalDigit | "_" }
// Group Name requires validation during the Listener AST synthesis phase.
*/
IDENT: LET ( LET | DIG | UNDERSCORE )*;

//// TODO: TBD: would a lexer rule work?
//STR_LIT: ( '\\"' | ~( '\n' | '\r' ) )*;

// Required in order to not silently drop unexpected characters.
ERROR_CHAR: .;

// See Lexer Rule comments. Listeners must perform the required Capital Letter validation.
groupName: IDENT;

// http://developers.google.com/protocol-buffers/docs/reference/proto2-spec
// http://developers.google.com/protocol-buffers/docs/proto
/*
|   alternation
()  grouping
[]  option (zero or one time)
{}  repetition (any number of times)
*/

/*
strLit = ( "'" { charValue } "'" ) | ( '"' { charValue } '"' )
charValue = hexEscape | octEscape | charEscape | /[^\0\n\\]/
hexEscape = '\' ( "x" | "X" ) hexDigit hexDigit
octEscape = '\' octalDigit octalDigit octalDigit
charEscape = '\' ( "a" | "b" | "f" | "n" | "r" | "t" | "v" | '\' | "'" | '"' )
quote = "'" | '"'
*/

// Parser Rules

// https://stackoverflow.com/questions/39958165/antlr4-how-can-i-match-end-of-lines-inside-multiline-comments
// https://stackoverflow.com/questions/12898052/antlr-how-to-skip-multiline-comments

/*
fullIdent = ident { "." ident }
messageName = ident
enumName = ident
fieldName = ident
oneofName = ident
mapName = ident
serviceName = ident
rpcName = ident
streamName = ident
messageType = [ "." ] { ident "." } messageName
enumType = [ "." ] { ident "." } enumName
groupName = capitalLetter { letter | decimalDigit | "_" }
*/

// See Lexer Rule comments.
ident: IDENT;
// FullIdent needs a first class AST in order to tell the difference between that and a string literal.
fullIdent: ident ( DOT ident )*;

// optionName = ( ident | "(" fullIdent ")" ) { "." ident }
groupedOptionNamePrefix: OPEN_PAREN fullIdent CLOSE_PAREN;
singleOptionNamePrefix: ident;
optionNamePrefix: groupedOptionNamePrefix | singleOptionNamePrefix;
optionNameSuffix: ( DOT ident )*;
optionName: optionNamePrefix optionNameSuffix;

label: REQUIRED | OPTIONAL | REPEATED;

/*
Cannot determine on the first pass whether this is MESSAGE_TYPE or ENUM_TYPE.
This suffers the same context-agnostic issue as with the Boost.Spirir.Qi baesd approach.
There is really no way around this on the first pass.
*/
elementTypeGlobalScope: DOT;
elementType: elementTypeGlobalScope? ( ident DOT )* ident;

// Should resolve to ProtoType, literally.
protoType: DOUBLE | FLOAT | INT32 | INT64 | UINT32 | UINT64 | SINT32 | SINT64 | FIXED32 | FIXED64 | SFIXED32 | SFIXED64 | BOOL | STRING | BYTES;

// Should resolve to a Variant type.
type: elementType | protoType;

keyType : INT32 | INT64 | UINT32 | UINT64 | SINT32 | SINT64 | FIXED32 | FIXED64 | SFIXED32 | SFIXED64 | BOOL | STRING;

// boolLit = "true" | "false" 
booleanFalse: BOOLEAN_FALSE;
booleanTrue: BOOLEAN_TRUE;
booleanLit: booleanFalse | booleanTrue;

/*
strLit = ( "'" { charValue } "'" ) | ( '"' { charValue } '"' )
charValue = hexEscape | octEscape | charEscape | /[^\0\n\\]/
hexEscape = '\' ( "x" | "X" ) hexDigit hexDigit
octEscape = '\' octalDigit octalDigit octalDigit
charEscape = '\' ( "a" | "b" | "f" | "n" | "r" | "t" | "v" | '\' | "'" | '"' )
quote = "'" | '"'
*/
strLit: ( '\\"' | ~( '\n' | '\r' ) )*;
//// TODO: TBD: this was working, I think, prior to last week's .NET updates...
//strLit: ( ~[\n\r] | '\\"' );

// TODO: TBD: need to elaborate this one significantly...
quotedStrLit: '"' strLit '"';

sign: SIGN;

/*
intLit     = decimalLit | octalLit | hexLit
decimalLit = ( "1" … "9" ) { decimalDigit }
octalLit   = "0" { octalDigit }
hexLit     = "0" ( "x" | "X" ) hexDigit { hexDigit } 
// TODO: TBD: how to represent OCT or HEX literals?
*/
hexLit: HEX_LIT;
octLit: OCT_LIT;
decLit: DEC_LIT;
//      Order still matters!
intLit: hexLit | octLit | decLit;
// Signage does not apply until the Constant Phrase.
constIntLit: sign? intLit;

// fieldNumber = intLit
fieldNumber: intLit;

/*
floatLit = (
        decimals "." [ decimals ] [ exponent ]
        | decimals exponent
        | "." decimals [ exponent ]
    )
    | "inf"
    | "nan"
decimals  = decimalDigit { decimalDigit }
exponent  = ( "e" | "E" ) [ "+" | "-" ] decimals 
// Also, technically the spec does not differentiate + from - Infinity, but we can here.
 */
infinity: INFINITY;
nan: NOT_A_NUMBER;
/*
At minimum, Floating Point values must involved a Dot.
Otherwise the Grammar confuses the phrase with the Integer rules.
*/
floatingPointValue: FLOAT_DIG_DOT_DIG_OPT_EXP | FLOAT_DIG_EXP | FLOAT_DOT_DIG_OPT_EXP;
floatLit: infinity | nan | floatingPointValue;
// Signage does not apply until the Constant Phrase.
constFloatLit: sign? floatLit;

// Helps to isolate the issue for the general-purpose <fullident/>.
fullIdentLit: fullIdent;

/*
constant = fullIdent | ( [ "-" | "+" ] intLit ) | ( [ "-" | "+" ] floatLit ) |
                strLit | boolLit 
*/
constant : booleanLit | quotedStrLit | constFloatLit | constIntLit | fullIdentLit;

// emptyStatement = ";"
emptyDecl: EOS;

syntaxValue
  : ( '\'' PROTO2 '\''
        | '"' PROTO2 '"' )
;

syntaxDecl: SYNTAX EQU syntaxValue EOS;

importModifier: WEAK | PUBLIC;

// import = "import" [ "weak" | "public" ] strLit ";" 
importDecl: IMPORT importModifier? quotedStrLit EOS;

// package = "package" fullIdent ";"
packageDecl: PACKAGE fullIdent EOS;

/*
option = "option" optionName  "=" constant ";"
optionName = ( ident | "(" fullIdent ")" ) { "." ident }
*/
optionDecl: OPTION optionName EQU constant EOS;

/*
label = "required" | "optional" | "repeated"
type = "double" | "float" | "int32" | "int64" | "uint32" | "uint64"
      | "sint32" | "sint64" | "fixed32" | "fixed64" | "sfixed32" | "sfixed64"
      | "bool" | "string" | "bytes" | messageType | enumType
*/

/*
field = label type fieldName "=" fieldNumber [ "[" fieldOptions "]" ] ";"
fieldNumber = intLit
fieldOptions = fieldOption { ","  fieldOption }
fieldOption = optionName "=" constant
*/
fieldDecl: label type ident EQU fieldNumber fieldOptions? EOS;
fieldOptions: OPEN_SQUARE_BRACKET fieldOption ( COMMA fieldOption )* CLOSE_SQUARE_BRACKET;
fieldOption: optionName EQU constant;

// group = label "group" groupName "=" fieldNumber messageBody
groupDecl: label GROUP groupName EQU fieldNumber messageBody;

/*
oneof = "oneof" oneofName "{" { oneofField | emptyStatement } "}"
oneofField = type fieldName "=" fieldNumber [ "[" fieldOptions "]" ] ";"
*/
oneOfDecl: ONEOF ident OPEN_CURLY_BRACE ( oneOfField | emptyDecl )* CLOSE_CURLY_BRACE;
oneOfField: type ident EQU fieldNumber fieldOptions? EOS;

/*
mapField = "map" "<" keyType "," type ">" mapName "=" fieldNumber [ "[" fieldOptions "]" ] ";"
keyType = "int32" | "int64" | "uint32" | "uint64" | "sint32" | "sint64" |
          "fixed32" | "fixed64" | "sfixed32" | "sfixed64" | "bool" | "string"
See: MAP, KEY_TYPE, TYPE, ID
*/
mapFieldDecl: MAP OPEN_ANGLE_BRACKET keyType COMMA type CLOSE_ANGLE_BRACKET ident EQU fieldNumber fieldOptions? EOS;

// extensions = "extensions" ranges ";"
extensionsDecl: EXTENSIONS rangesDecl EOS;
// ranges = range { "," range }
rangesDecl: rangeDecl ( COMMA rangeDecl )*;
// Overall: range =  intLit [ "to" ( intLit | "max" ) ]
rangeDecl: rangeMinimumLit rangeMaximum?;
rangeMinimumLit: intLit;
// RangeDescriptor.Maximum Phrase: "to" ( intLit | "max" )
rangeMaximum: TO ( rangeMaximumMax | rangeMaximumLit );
rangeMaximumMax: MAX;
rangeMaximumLit: intLit;

/*
reserved = "reserved" ( ranges | fieldNames ) ";"
fieldNames = fieldName { "," fieldName }
*/
// We have to separate these two because we must know the kind of Reserved that it is.
reservedDecl : ( fieldNamesReservedDecl | rangesReservedDecl );
rangesReservedDecl: RESERVED rangesDecl EOS;
fieldNamesReservedDecl: RESERVED fieldNames EOS;
// Turns out this needs to be a First Class Parser Rule after all.
fieldNames: fieldName ( COMMA fieldName )*;
fieldName: ident;

/*
enum = "enum" enumName enumBody
enumBody = "{" { option | enumField | emptyStatement } "}"
// While we associate an "integer" with the enumField... Rather we want an Ordinal.
enumField = ident "=" intLit [ "[" enumValueOption { ","  enumValueOption } "]" ]";"
//                    ^^^^^^
enumValueOption = optionName "=" constant
*/
enumFieldOrdinal: intLit;
enumDecl: ENUM ident enumBody;
enumBody: OPEN_CURLY_BRACE ( optionDecl | enumFieldDecl | emptyDecl )* CLOSE_CURLY_BRACE;
//                    Or FieldNumber, but IntLit works just fine here.
enumFieldDecl: ident EQU enumFieldOrdinal ( OPEN_SQUARE_BRACKET enumValueOptions CLOSE_SQUARE_BRACKET )? EOS;
enumValueOptions: enumValueOption ( COMMA enumValueOption )*;
enumValueOption: optionName EQU constant;

/*
message = "message" messageName messageBody
messageBody = "{" { field | enum | message | extend | extensions | group |
option | oneof | mapField | reserved | emptyStatement } "}"
*/
messageDecl: MESSAGE ident messageBody;

messageBody
  : OPEN_CURLY_BRACE (
    fieldDecl
    | enumDecl
    | messageDecl
    | extendDecl
    | extensionsDecl
    | groupDecl
    | optionDecl
    | oneOfDecl
    | mapFieldDecl
    | reservedDecl
    | emptyDecl
    )* CLOSE_CURLY_BRACE
;

// extend = "extend" messageType "{" {field | group | emptyStatement} "}"
extendDecl: EXTEND elementType OPEN_CURLY_BRACE ( fieldDecl | groupDecl | emptyDecl )* CLOSE_CURLY_BRACE;

proto: syntaxDecl ( importDecl | packageDecl | optionDecl | topLevelDef | emptyDecl )* EOF;

//  Not supporting Service for this purpose.
topLevelDef: messageDecl | enumDecl | extendDecl /*| serviceDecl*/;

//// TODO: TBD: will not support this for now, it really is not necessary given the Proto I am interested in.
//// TODO: TBD: if I were to more fully support Service, etc, I would refactor some rules in order to better identify the given phrase.
//// TODO: TBD: for instance, in the RPC rule, which messageType (or elementType) do you reduce to an AST, first or second?
//// TODO: TBD: same for STREAM.

//// service = "service" serviceName "{" { option | rpc | stream | emptyStatement } "}"
//serviceDecl:
//  SERVICE ident OPEN_CURLY_BRACE ( optionDecl | rpcDecl | streamDecl | emptyDecl )* CLOSE_CURLY_BRACE
//;

//// TODO: TBD: to be honest, I'm not sure why they don't simply support the empty body, yet have an end of statement alternative.
//// rpc = "rpc" rpcName "(" [ "stream" ] messageType ")" "returns" "(" [ "stream" ] messageType ")" ( ( "{" { option | emptyStatement } "}" ) | ";" )
//rpcDecl:
//  RPC ident OPEN_PAREN STREAM? elementType CLOSE_PAREN
//    RETURNS OPEN_PAREN STREAM? elementType CLOSE_PAREN
//      ( ( OPEN_CURLY_BRACE ( optionDecl | emptyDecl )* CLOSE_CURLY_BRACE ) | EOS )
//;

//// TODO: TBD: similar question here as above...
//// stream = "stream" streamName "(" messageType "," messageType ")" ( ( "{" { option | emptyStatement } "}") | ";" )
//streamDecl:
//  STREAM ident OPEN_PAREN elementType COMMA elementType CLOSE_PAREN
//    ( ( OPEN_CURLY_BRACE ( optionDecl | emptyDecl )* CLOSE_CURLY_BRACE ) | EOS )
//;
