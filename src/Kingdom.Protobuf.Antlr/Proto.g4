// http://github.com/tunnelvisionlabs/antlr4cs
// http://github.com/antlr/antlr4/blob/master/doc/csharp-target.md#how-do-i-create-and-run-a-custom-listener
// http://developers.google.com/protocol-buffers/docs/reference/proto2-spec
// http://github.com/antlr/antlr4/blob/master/doc/lexer-rules.md
// http://github.com/antlr/antlr4/blob/master/doc/parser-rules.md
// http://community.esri.com/thread/182866-mismatch-between-the-processor-architecture-of-the-project-being-built-msil-and-the-processor-architecture-of-the-reference-oracledataaccess
// http://stackoverflow.com/questions/47752764/there-was-a-mismatch-between-the-processor-architecture-of-the-project-being-bui
// http://stackoverflow.com/questions/10113532/how-do-i-fix-the-visual-studio-compile-error-mismatch-between-processor-archit
// http://stackoverflow.com/questions/39958165/antlr4-how-can-i-match-end-of-lines-inside-multiline-comments
// http://stackoverflow.com/questions/12898052/antlr-how-to-skip-multiline-comments

// This grammar supports a subset of the Protocol Buffers Version 2. We support up to but not
// including the Service declaration, RPC, or Streams. This is the extent of the support we
// require for what we are trying to accomplish with this project. If additional elements, code
// generation, etc, are required, we will gladly consider Pull Requests for said functionality
// aligned with our licensing model.

/*
http://developers.google.com/protocol-buffers/docs/reference/proto2-spec
http://developers.google.com/protocol-buffers/docs/proto
|   alternation
()  grouping
[]  option (zero or one time)
{}  repetition (any number of times)
*/

grammar Proto;

// LEXER RULES //////////////////////////////////////////////////////////////////////////
ENUM: 'enum';
EXTEND: 'extend';
EXTENSIONS: 'extensions';
GROUP: 'group';
IMPORT: 'import';
OPTION: 'option';
MAP: 'map';
MESSAGE: 'message';
ONEOF: 'oneof';
PACKAGE: 'package';
RESERVED: 'reserved';
SYNTAX: 'syntax';

//RPC: 'rpc';
//SERVICE: 'service';
//RETURNS: 'returns';
//STREAM: 'stream';

MAX: 'max';
OPTIONAL: 'optional';
PUBLIC: 'public';
REPEATED: 'repeated';
REQUIRED: 'required';
TO: 'to';
WEAK: 'weak';

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

FALSE_LIT: 'false';
INF: 'inf';
NAN: 'nan';
//PROTO2: 'proto2';
TRUE_LIT: 'true';

MULTI_LINE_COMMENT: LML .*? RML -> channel(HIDDEN);
SINGLE_LINE_COMMENT: SL ~[\r\n]* -> channel(HIDDEN);

// Whitespace.
WS: ( SP | HT | VT )+ -> channel(HIDDEN);

// NEWLINE : '\r'? '\n' { {System.out.println("Newlines so far: " + (++counter)); } };
NL: CR? LF -> channel(HIDDEN);

// Define some punctuation lexer rules.
QUOT: QUOT_MARK;
TICK: TICK_MARK;

// Open, or Left, such and such.
LANGLE: '<';
LCURLY: '{';
LPAREN: '(';
LSQUARE: '[';

// Close, or Right, such and such.
RANGLE: '>';
RCURLY: '}';
RPAREN: ')';
RSQUARE: ']';

COMMA: ',';
EOS: ';';
EQU: '=';

DOT: PERIOD;
SIGN: SIGNAGE;

/*
decimalLit = ( "1" ... "9" ) { decimalDigit }
octalLit = "0" { octalDigit }
hexLit = "0" ( "x" | "X" ) hexDigit { hexDigit } 
*/
DEC_LIT: ZED | DEC_DIG_19 DEC_DIG*;
HEX_LIT: ZED X HEX_DIG+;
OCT_LIT: ZED OCT_DIG*;

/*
// Lexer is the way to go with this rule.
// It is better for us to capture the Infinity and Not-a-Number cases separately
// as "first-class" Parser Rules, not that Lexer Rules are not ("first-class").
floatLit = "inf" | "nan" | (
    decimals "." [ decimals ] [ exponent ]
    | decimals exponent
    | "." decimals [ exponent ] )
// We will just use the ANTLR4 operators here, not worth a definition for any multiples.
decimals = decimalDigit { decimalDigit }
*/
FLOAT_LIT:
    DEC_DIG+ PERIOD DEC_DIG* EXPONENT?
    | DEC_DIG+ EXPONENT
    | PERIOD DEC_DIG+ EXPONENT?
;

/*
// Supports both cases:
ident = letter { letter | decimalDigit | "_" }
groupName = capitalLetter { letter | decimalDigit | "_" }
// Group Name requires validation during the Listener AST synthesis phase.
*/
IDENT: LET ( LET | DEC_DIG | UNDERSCORE )*;

STR_LIT: QUOT_STR_LIT | TICK_STR_LIT;

// Required in order to not silently drop unexpected characters.
ERROR_CHAR: . -> channel(HIDDEN);

// Open and close, or Left and Right, Multi-Line comment.
fragment LML: '/*';
fragment RML: '*/';
// Single-Line comment.
fragment SL: '//';

// Carriage Return, Line Feed.
fragment CR: '\r';
fragment LF: '\n';
// Space.
fragment SP: ' ';
// Tab, or Horizontal Tab.
fragment HT: '\t';
// Form Feed, or Vertical Tab.
fragment VT: '\f';

// decimalDigit = "0" ... "9"
// octalDigit   = "0" ... "7"
// hexDigit     = "0" ... "9" | "A" ... "F" | "a" ... "f"
fragment DEC_DIG: [0-9];
fragment DEC_DIG_19: [1-9];
fragment OCT_DIG: [0-7];
fragment OCT_DIG_03: [0-3];
fragment HEX_DIG: [0-9a-zA-Z];
fragment SIGNAGE: [+-];
fragment EXP: [Ee];
// exponent = ( "e" | "E" ) [ "+" | "-" ] decimals 
fragment EXPONENT: EXP SIGNAGE? DEC_DIG+;
fragment PERIOD: '.';
// letter = "A" ... "Z" | "a" ... "z"
fragment LET: [a-zA-Z];
fragment UNDERSCORE: '_';
// capitalLetter =  "A" ... "Z"
fragment CAP_LET: [A-Z];
fragment ZED: [0];
fragment X: [Xx];
fragment ESC_START: '\\';

fragment ESC_SEQ:
    ESC_START ( 'a' | 'v' | 'b' | 't' | 'n' | 'f' | 'r' | '?' | '"' | '\'' | '\\' )
    | HEX_ESC
    //| UNICODE_ESC
    | OCT_ESC
;

//// TODO: TBD: is this really Unicode?
//fragment UNICODE_ESC: '\\' 'u' HEX_DIG HEX_DIG HEX_DIG HEX_DIG;
fragment HEX_ESC:
    ESC_START X HEX_DIG HEX_DIG
    | ESC_START X HEX_DIG
;

fragment OCT_ESC:
    ESC_START OCT_DIG_03 OCT_DIG OCT_DIG
    | ESC_START OCT_DIG OCT_DIG
    | ESC_START OCT_DIG
;

fragment QUOT_MARK: '"';
fragment TICK_MARK: '\'';

/*
strLit = ( "'" { charValue } "'" ) | ( '"' { charValue } '"' )
charValue = hexEscape | octEscape | charEscape | /[^\0\n\\]/
hexEscape = '\' ( "x" | "X" ) hexDigit hexDigit
octEscape = '\' octalDigit octalDigit octalDigit
charEscape = '\' ( "a" | "b" | "f" | "n" | "r" | "t" | "v" | '\' | "'" | '"' )
quote = "'" | '"'
*/
fragment QUOT_STR_LIT: QUOT_MARK ( ESC_SEQ | ~[\\"\r\n] )* QUOT_MARK;
fragment TICK_STR_LIT: TICK_MARK ( ESC_SEQ | ~[\\\'\r\n] )* TICK_MARK;

// PARSER RULES /////////////////////////////////////////////////////////////////////////
// See Lexer Rule comments. Listeners must perform the required Capital Letter validation.

protoDecl: syntaxDecl (
    importDecl
    | packageDecl
    | optionDecl
    | topLevelDef
    | emptyDecl )* EOF
;

//  Not supporting Service for this purpose.
topLevelDef:
    messageDecl
    | enumDecl
    | extendDecl
    /*| serviceDecl*/
;

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
// FullIdent needs a first class AST in order to tell the difference between that and a string literal.
ident: IDENT;
fullIdent: ident ( DOT ident )*;

// optionName = ( ident | "(" fullIdent ")" ) { "." ident }
groupedOptionNamePrefix: LPAREN fullIdent RPAREN;
singleOptionNamePrefix: ident;
optionNamePrefix: groupedOptionNamePrefix | singleOptionNamePrefix;
optionNameSuffix: ( DOT ident )*;
optionName: optionNamePrefix optionNameSuffix;

// boolLit = "true" | "false" 
booleanFalse: FALSE_LIT;
booleanTrue: TRUE_LIT;
booleanLit: booleanFalse | booleanTrue;

// Helps to isolate the issue for the general-purpose <fullident/>.
fullIdentLit: fullIdent;

// Technically the spec does not differentiate + from - Infinity, but we can here.
inf: INF;
nan: NAN;
numericFloatLit: FLOAT_LIT;
floatLit: inf | nan | numericFloatLit;
// Signage does not apply until the Constant Phrase.
signedFloatLit: sign? floatLit;

// intLit = decimalLit | octalLit | hexLit
hexLit: HEX_LIT;
octLit: OCT_LIT;
decLit: DEC_LIT;
//      Order still matters!
intLit: hexLit | octLit | decLit;

// Signage does not apply until the Constant Phrase.
sign: SIGN;
signedIntLit: sign? intLit;

// This is probably sufficient. We may need to comprehend whether the enclosing bits were QUOT ('"') or TICK ("'").
strLit: STR_LIT;

// constant = boolLit    | fullIdent    | ( [ "-" | "+" ] intLit ) | ( [ "-" | "+" ] floatLit ) | strLit
constant:     booleanLit | fullIdentLit | signedFloatLit           | signedIntLit               | strLit;

/*
syntax = "syntax" "=" quote "proto2" quote ";"
// Instead of defining a lexer keyword, let it fall through the String Literal definition.
// Then we handle the issue as a matter of Listener interpretation. The only down side of
// doing it this way is that casual users of the grammar apart from the Listener itself
// need to do some leg work in order validate.
*/
syntaxDecl: SYNTAX EQU strLit EOS;

// import = "import" [ "weak" | "public" ] strLit ";" 
importDecl: IMPORT importModifier? strLit EOS;
importModifier: WEAK | PUBLIC;

// emptyStatement = ";"
emptyDecl: EOS;

// package = "package" fullIdent ";"
packageDecl: PACKAGE fullIdent EOS;

/*
option = "option" optionName  "=" constant ";"
optionName = ( ident | "(" fullIdent ")" ) { "." ident }
*/
optionDecl: OPTION optionName EQU constant EOS;

/*
message = "message" messageName messageBody
messageBody = "{" { field | enum | message | extend | extensions | group |
option | oneof | mapField | reserved | emptyStatement } "}"
*/
messageDecl: MESSAGE ident messageBody;

messageBody
  : LCURLY (
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
    )* RCURLY
;

/*
label = "required" | "optional" | "repeated"
type = "double" | "float" | "int32" | "int64" | "uint32" | "uint64" | "sint32" | "sint64"
    | "fixed32" | "fixed64" | "sfixed32" | "sfixed64" | "bool" | "string" | "bytes"
    | messageType | enumType
*/
label:
    REQUIRED
    | OPTIONAL
    | REPEATED
;

/*
// Should resolve to ProtoType, literally. Type Keywords:
// https://developers.google.com/protocol-buffers/docs/proto#scalar
// https://developers.google.com/protocol-buffers/docs/proto3#scalar
*/
protoType:
    DOUBLE
    | FLOAT
    | INT32
    | INT64
    | UINT32
    | UINT64
    | SINT32
    | SINT64
    | FIXED32
    | FIXED64
    | SFIXED32
    | SFIXED64
    | BOOL
    | STRING
    | BYTES
;

/*
Cannot determine on the first pass whether this is MESSAGE_TYPE or ENUM_TYPE.
This suffers the same context-agnostic issue as with the Boost.Spirir.Qi baesd approach.
There is really no way around this on the first pass.
*/
elementTypeGlobalScope: DOT;
/*
// Technically, the specification is ambiguous, with the "intent" here:
messageType = [ "." ] { ident "." } messageName
enumType = [ "." ] { ident "." } enumName
// However, the ambiguity is that the Parser itself cannot know whether "ident" should be MessageDecl or EnumDecl.
*/
elementType: elementTypeGlobalScope? ( ident DOT )* ident;

// Should resolve to a Variant type.
type: protoType | elementType;

// fieldNumber = intLit
fieldNumber: intLit;

/*
field = label type fieldName "=" fieldNumber [ "[" fieldOptions "]" ] ";"
fieldNumber = intLit
fieldOptions = fieldOption { ","  fieldOption }
fieldOption = optionName "=" constant
*/
fieldDecl: label type ident EQU fieldNumber fieldOptions? EOS;
fieldOptions: LSQUARE fieldOption ( COMMA fieldOption )* RSQUARE;
fieldOption: optionName EQU constant;

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
enumBody: LCURLY ( optionDecl | enumFieldDecl | emptyDecl )* RCURLY;
//                    Or FieldNumber, but IntLit works just fine here.
enumFieldDecl: ident EQU enumFieldOrdinal ( LSQUARE enumValueOptions RSQUARE )? EOS;
enumValueOptions: enumValueOption ( COMMA enumValueOption )*;
enumValueOption: optionName EQU constant;

// extend = "extend" messageType "{" {field | group | emptyStatement} "}"
extendDecl: EXTEND elementType LCURLY ( fieldDecl | groupDecl | emptyDecl )* RCURLY;

// group = label "group" groupName "=" fieldNumber messageBody
groupName: IDENT;
groupDecl: label GROUP groupName EQU fieldNumber messageBody;

/*
extensions = "extensions" ranges ";"
ranges = range { "," range }
*/
extensionsDecl: EXTENSIONS rangesDecl EOS;
rangesDecl: rangeDecl ( COMMA rangeDecl )*;
// Overall: range =  intLit [ "to" ( intLit | "max" ) ]
rangeDecl: rangeMinimumLit rangeMaximum?;
rangeMinimumLit: intLit;
// RangeDescriptor.Maximum Phrase: "to" ( intLit | "max" )
rangeMaximum: TO ( rangeMaximumMax | rangeMaximumLit );
rangeMaximumMax: MAX;
rangeMaximumLit: intLit;

/*
oneof = "oneof" oneofName "{" { oneofField | emptyStatement } "}"
oneofField = type fieldName "=" fieldNumber [ "[" fieldOptions "]" ] ";"
*/
oneOfDecl: ONEOF ident LCURLY ( oneOfField | emptyDecl )* RCURLY;
oneOfField: type ident EQU fieldNumber fieldOptions? EOS;

/*
mapField = "map" "<" keyType "," type ">" mapName "=" fieldNumber [ "[" fieldOptions "]" ] ";"
keyType = "int32" | "int64" | "uint32" | "uint64" | "sint32" | "sint64" | "fixed32" | "fixed64"
    | "sfixed32" | "sfixed64" | "bool" | "string"
*/
mapFieldDecl: MAP LANGLE keyType COMMA type RANGLE ident EQU fieldNumber fieldOptions? EOS;

keyType:
    INT32
    | INT64
    | UINT32
    | UINT64
    | SINT32
    | SINT64
    | FIXED32
    | FIXED64
    | SFIXED32
    | SFIXED64
    | BOOL
    | STRING
;

/*
reserved = "reserved" ( ranges | fieldNames ) ";"
fieldNames = fieldName { "," fieldName }
*/
// We have to separate these two because we must know the kind of Reserved that it is.
reservedDecl: ( fieldNamesReservedDecl | rangesReservedDecl );
rangesReservedDecl: RESERVED rangesDecl EOS;
fieldNamesReservedDecl: RESERVED fieldNames EOS;
// Turns out this needs to be a First Class Parser Rule after all.
fieldNames: fieldName ( COMMA fieldName )*;
fieldName: ident;


//// TODO: TBD: will not support this for now, it really is not necessary given the Proto I am interested in.
//// TODO: TBD: if I were to more fully support Service, etc, I would refactor some rules in order to better identify the given phrase.
//// TODO: TBD: for instance, in the RPC rule, which messageType (or elementType) do you reduce to an AST, first or second?
//// TODO: TBD: same for STREAM.

//// service = "service" serviceName "{" { option | rpc | stream | emptyStatement } "}"
//serviceDecl:
//  SERVICE ident LCURLY ( optionDecl | rpcDecl | streamDecl | emptyDecl )* RCURLY
//;

//// TODO: TBD: to be honest, I'm not sure why they don't simply support the empty body, yet have an end of statement alternative.
//// rpc = "rpc" rpcName "(" [ "stream" ] messageType ")" "returns" "(" [ "stream" ] messageType ")" ( ( "{" { option | emptyStatement } "}" ) | ";" )
//rpcDecl:
//  RPC ident LPAREN STREAM? elementType RPAREN
//    RETURNS LPAREN STREAM? elementType RPAREN
//      ( ( LCURLY ( optionDecl | emptyDecl )* RCURLY ) | EOS )
//;

//// TODO: TBD: similar question here as above...
//// stream = "stream" streamName "(" messageType "," messageType ")" ( ( "{" { option | emptyStatement } "}") | ";" )
//streamDecl:
//  STREAM ident LPAREN elementType COMMA elementType RPAREN
//    ( ( LCURLY ( optionDecl | emptyDecl )* RCURLY ) | EOS )
//;
