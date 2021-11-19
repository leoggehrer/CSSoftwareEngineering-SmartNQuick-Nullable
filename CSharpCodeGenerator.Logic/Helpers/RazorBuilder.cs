//@BaseCode
//MdStart
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpCodeGenerator.Logic.Helpers
{
    public class RazorBuilder
    {
        private enum SourceType
        {
            CodeTag,
            OpenBlock,
            CloseBlock,
            OpenTag,
            SingleTag,
            CloseTag,
        }

        private record SourceItem
        {
            public SourceType Type { get; init; }
            public string TagName { get; init; }
            public string CssClass { get; init; }
            public string CssAttributes { get; init; }
            public int Indent { get; init; }

            public override string ToString() =>
                Type switch
                {
                    SourceType.CodeTag => TagName.SetIndent(Indent),
                    SourceType.OpenBlock => TagName.SetIndent(Indent),
                    SourceType.CloseBlock => TagName.SetIndent(Indent),
                    SourceType.OpenTag => $"<{CreateTag()}>".SetIndent(Indent),
                    SourceType.SingleTag => $"<{CreateTag()} />".SetIndent(Indent),
                    SourceType.CloseTag => $"</{TagName}>".SetIndent(Indent),
                    _ => throw new NotImplementedException(),
                };
            private string CreateTag()
            {
                var result = $"{TagName}";

                if (CssClass.HasContent())
                {
                    result += $" class=\"{CssClass}\"";
                }
                if (CssAttributes.HasContent())
                {
                    result += $" {CssAttributes}";
                }
                return result;
            }
        }
        private int codeBlocks = 0;
        private readonly List<SourceItem> sourceItems = new();
        private readonly Stack<SourceItem> openElements = new();

        public bool HasOpenCodeBlock => codeBlocks > 0;

        public RazorBuilder()
        {
        }
        public RazorBuilder(RazorBuilder razorBuilder)
        {
            Add(razorBuilder);
        }
        public void Add(RazorBuilder razorBuilder)
        {
            razorBuilder.CheckArgument(nameof(razorBuilder));

            if (razorBuilder.openElements.Count > 0)
                throw new Exception("Added razor builder is not closed.");

            foreach (var item in razorBuilder.sourceItems)
            {
                if (item.Type == SourceType.CodeTag)
                    AddCode(item.TagName);
                else if (item.Type == SourceType.OpenBlock)
                    OpenCodeBlock();
                else if (item.Type == SourceType.CloseBlock)
                    CloseCodeBlock();
                else if (item.Type == SourceType.OpenTag)
                    OpenTag(item.TagName, item.CssClass, item.CssAttributes);
                else if (item.Type == SourceType.SingleTag)
                    AddTag(item.TagName, item.CssClass, item.CssAttributes);
                else if (item.Type == SourceType.CloseTag)
                    CloseTag();
            }
        }
        public RazorBuilder AddCode(string line)
        {
            line.CheckArgument(nameof(line));

            sourceItems.Add(new SourceItem
            {
                Type = SourceType.CodeTag,
                TagName = line,
                Indent = openElements.Count + codeBlocks,
            });
            return this;
        }

        public RazorBuilder OpenCodeBlock()
        {
            return OpenCodeBlock("{");
        }
        public RazorBuilder OpenCodeBlock(string openTag)
        {
            openTag.CheckNotNullOrEmpty(nameof(openTag));

            sourceItems.Add(new SourceItem
            {
                Type = SourceType.OpenBlock,
                TagName = openTag,
                Indent = openElements.Count + codeBlocks,
            });
            codeBlocks++;
            return this;
        }
        public RazorBuilder CloseCodeBlock(string closeBlock)
        {
            closeBlock.CheckNotNullOrEmpty(nameof(closeBlock));

            if (codeBlocks > 0)
            {
                codeBlocks--;
                sourceItems.Add(new SourceItem
                {
                    Type = SourceType.CloseBlock,
                    TagName = closeBlock,
                    Indent = openElements.Count + codeBlocks,
                });
            }
            return this;
        }
        public RazorBuilder CloseCodeBlock()
        {
            return CloseCodeBlock("}");
        }

        public RazorBuilder OpenTag(string name)
        {
            return OpenTag(name, string.Empty, string.Empty);
        }
        public RazorBuilder OpenTag(string name, string cssAttributes)
        {
            return OpenTag(name, string.Empty, cssAttributes);
        }
        public RazorBuilder OpenTag(string name, string cssClass, string cssAttributes)
        {
            name.CheckNotNullOrEmpty(nameof(name));

            var item = new SourceItem
            {
                Type = SourceType.OpenTag,
                TagName = name,
                CssClass = cssClass,
                CssAttributes = cssAttributes,
                Indent = openElements.Count + codeBlocks,
            };

            sourceItems.Add(item);
            openElements.Push(item with { Type = SourceType.CloseTag });
            return this;
        }
        public RazorBuilder CloseTag()
        {
            sourceItems.Add(openElements.Pop());
            return this;
        }

        public RazorBuilder AddTag(string name)
        {
            return AddTag(name, string.Empty, string.Empty);
        }
        public RazorBuilder AddTag(string name, string cssAttributes)
        {
            return AddTag(name, string.Empty, cssAttributes);
        }
        public RazorBuilder AddTag(string name, string cssClass, string cssAttributes)
        {
            sourceItems.Add(new SourceItem
            {
                Type = SourceType.SingleTag,
                TagName = name,
                CssClass = cssClass,
                CssAttributes = cssAttributes,
                Indent = openElements.Count + codeBlocks,
            });
            return this;
        }
        public RazorBuilder Execute(Action<RazorBuilder> action)
        {
            action?.Invoke(this);
            return this;
        }
        public override string ToString()
        {
            return sourceItems.Select(e => e.ToString()).ToText();
        }
    }
}
//MdEnd