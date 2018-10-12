using System;
using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BlogDemo.Extensions.TagHelpers
{
    [HtmlTargetElement("requiredlabel", Attributes = ForAttributeName)]
    public class RequiredLabelTagHelper: TagHelper
    {
        private const string ForAttributeName = "asp-for";

        // Will be used as highlight-class in html
        public string HighlightClass { get; set; }

        /// <summary>
        /// Creates a new <see cref="LabelTagHelper"/>.
        /// </summary>
        /// <param name="generator">The <see cref="IHtmlGenerator"/>.</param>
        public RequiredLabelTagHelper(IHtmlGenerator generator)
        {
            Generator = generator;
        }

        /// <inheritdoc />
        public override int Order => -1000;
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        protected IHtmlGenerator Generator { get; }

        /// <summary>
        /// An expression to be evaluated against the current model.
        /// </summary>
        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        /// <inheritdoc />
        /// <remarks>Does nothing if <see cref="For"/> is <c>null</c>.</remarks>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            var requiredMarkTagBuilder = new TagBuilder("span");
            requiredMarkTagBuilder.AddCssClass(HighlightClass);
            requiredMarkTagBuilder.InnerHtml.Append(" *");

            var tagBuilder = Generator.GenerateLabel(
                ViewContext,
                For.ModelExplorer,
                For.Name,
                labelText: null,
                htmlAttributes: null);

            if (For.ModelExplorer.Metadata.IsRequired)
            {
                using (var writer = new StringWriter())
                {
                    requiredMarkTagBuilder.WriteTo(writer, HtmlEncoder.Default);
                    tagBuilder.InnerHtml.AppendHtml(writer.ToString());
                }
            }

            output.TagName = tagBuilder.TagName;           
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
