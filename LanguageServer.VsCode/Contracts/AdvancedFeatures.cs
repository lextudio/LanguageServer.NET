using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LanguageServer.VsCode.Contracts
{
    public interface ITextDocumentIdentifierParams
    {
        TextDocumentIdentifier TextDocument { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class TextDocumentPositionParams : ITextDocumentIdentifierParams
    {
        [JsonProperty]
        public TextDocumentIdentifier TextDocument { get; set; }

        [JsonProperty]
        public Position Position { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class DefinitionParams : TextDocumentPositionParams
    {
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class HoverParams : TextDocumentPositionParams
    {
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class CompletionParams : TextDocumentPositionParams
    {
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class DocumentHighlightParams : TextDocumentPositionParams
    {
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class ReferenceParams : TextDocumentPositionParams
    {
        [JsonProperty]
        public ReferenceContext Context { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class DocumentSymbolParams : ITextDocumentIdentifierParams
    {
        [JsonProperty]
        public TextDocumentIdentifier TextDocument { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class WorkspaceSymbolParams
    {
        [JsonProperty]
        public string Query { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class CodeActionParams : ITextDocumentIdentifierParams
    {
        [JsonProperty]
        public TextDocumentIdentifier TextDocument { get; set; }

        [JsonProperty]
        public Range Range { get; set; }

        [JsonProperty]
        public CodeActionContext Context { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class CodeAction
    {
        [JsonProperty]
        public string Title { get; set; }

        [JsonProperty]
        public string Kind { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsPreferred { get; set; }

        [JsonProperty]
        public ICollection<Diagnostic> Diagnostics { get; set; }

        [JsonProperty]
        public WorkspaceEdit Edit { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class SemanticTokensParams : ITextDocumentIdentifierParams
    {
        [JsonProperty]
        public TextDocumentIdentifier TextDocument { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class SemanticTokensRangeParams : ITextDocumentIdentifierParams
    {
        [JsonProperty]
        public TextDocumentIdentifier TextDocument { get; set; }

        [JsonProperty]
        public Range Range { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class SemanticTokensDeltaParams : ITextDocumentIdentifierParams
    {
        [JsonProperty]
        public TextDocumentIdentifier TextDocument { get; set; }

        [JsonProperty]
        public string PreviousResultId { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class SemanticTokens
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ResultId { get; set; }

        [JsonProperty]
        public int[] Data { get; set; } = Array.Empty<int>();
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class SemanticTokensEdit
    {
        [JsonProperty]
        public int Start { get; set; }

        [JsonProperty]
        public int DeleteCount { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int[] Data { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class SemanticTokensDelta
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ResultId { get; set; }

        [JsonProperty]
        public ICollection<SemanticTokensEdit> Edits { get; set; } = Array.Empty<SemanticTokensEdit>();
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class SemanticTokensPartialResult
    {
        [JsonProperty]
        public int[] Data { get; set; } = Array.Empty<int>();
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class SemanticTokensDeltaPartialResult
    {
        [JsonProperty]
        public ICollection<SemanticTokensEdit> Edits { get; set; } = Array.Empty<SemanticTokensEdit>();
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class SemanticTokensFullOptions
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool Delta { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class SemanticTokensLegend
    {
        [JsonProperty]
        public IList<string> TokenTypes { get; } =
        new List<string>
        {
            "namespace",
            "type",
            "class",
            "enum",
            "interface",
            "struct",
            "typeParameter",
            "parameter",
            "variable",
            "property",
            "enumMember",
            "event",
            "function",
            "method",
            "macro",
            "keyword",
            "modifier",
            "comment",
            "string",
            "number",
            "regexp",
            "operator",
        };

        [JsonProperty]
        public IList<string> TokenModifiers { get; } =
        new List<string>
        {
            "declaration",
            "definition",
            "readonly",
            "static",
            "deprecated",
            "abstract",
            "async",
            "modification",
            "documentation",
            "defaultLibrary",
        };
    }

    public enum SemanticTokenType
    {
        Namespace,
        Type,
        Class,
        Enum,
        Interface,
        Struct,
        TypeParameter,
        Parameter,
        Variable,
        Property,
        EnumMember,
        Event,
        Function,
        Method,
        Macro,
        Keyword,
        Modifier,
        Comment,
        String,
        Number,
        Regexp,
        Operator,
    }

    public enum SemanticTokenModifier
    {
        Declaration,
        Definition,
        Readonly,
        Static,
        Deprecated,
        Abstract,
        Async,
        Modification,
        Documentation,
        DefaultLibrary,
    }

    public class SemanticTokensBuilder
    {
        private readonly List<(int line, int startChar, int length, int tokenType, int tokenModifiers)> _tokens = new List<(int line, int startChar, int length, int tokenType, int tokenModifiers)>();

        public void Push(int line, int startChar, int length, SemanticTokenType tokenType, SemanticTokenModifier[] tokenModifiers)
        {
            var modifiersMask = 0;
            foreach (var modifier in tokenModifiers)
            {
                modifiersMask |= 1 << (int)modifier;
            }

            _tokens.Add((line, startChar, length, (int)tokenType, modifiersMask));
        }

        public SemanticTokens Build()
        {
            _tokens.Sort((left, right) =>
            {
                var lineCmp = left.line.CompareTo(right.line);
                return lineCmp != 0 ? lineCmp : left.startChar.CompareTo(right.startChar);
            });

            var data = new List<int>(_tokens.Count * 5);
            var previousLine = 0;
            var previousStart = 0;
            var first = true;

            foreach (var token in _tokens)
            {
                var deltaLine = first ? token.line : token.line - previousLine;
                var deltaStart = first || deltaLine != 0 ? token.startChar : token.startChar - previousStart;
                data.Add(deltaLine);
                data.Add(deltaStart);
                data.Add(token.length);
                data.Add(token.tokenType);
                data.Add(token.tokenModifiers);
                previousLine = token.line;
                previousStart = token.startChar;
                first = false;
            }

            return new SemanticTokens
            {
                Data = data.ToArray(),
            };
        }
    }

    [JsonObject]
    public class SemanticTokensOptions : IWorkDoneProgressOptions
    {
        [JsonProperty]
        public SemanticTokensLegend Legend { get; set; }

        [JsonProperty]
        public object Full { get; set; }

        [JsonProperty]
        public bool Range { get; set; }

        [JsonProperty]
        public bool WorkDoneProgress { get; set; }
    }
}