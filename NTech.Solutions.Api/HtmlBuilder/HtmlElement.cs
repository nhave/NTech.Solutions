namespace NTech.Solutions.Api.HtmlBuilder
{
    /// <summary>
    /// Represents an HTML element with a tag name, attributes, inner text, and child elements.
    /// </summary>
    /// <remarks>Use this class to construct and render HTML elements programmatically. The element can
    /// contain attributes, inner text, and nested child elements, allowing for the creation of complex HTML structures.
    /// The class implements IHtmlElement and IHtmlRenderable to support consistent manipulation and rendering of HTML
    /// content.</remarks>
    public class HtmlElement : IHtmlElement, IHtmlRenderable
    {
        /// <summary>
        /// Gets or sets the name of the tag associated with the element.
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// Gets the collection of custom attributes associated with this instance.
        /// </summary>
        /// <remarks>The attributes are represented as key-value pairs, where each key is the attribute
        /// name and each value is the corresponding attribute value. The collection is read-only from outside the
        /// class; to modify the attributes, use the provided methods or accessors within the class.</remarks>
        public Dictionary<string, string> Attributes { get; private set; } = new Dictionary<string, string>();

        /// <summary>
        /// Gets or sets the inner text content of the element.
        /// </summary>
        public string? InnerText { get; set; }

        /// <summary>
        /// Gets or sets the collection of child HTML elements contained within this element.
        /// </summary>
        public List<IHtmlElement> Children { get; set; } = new List<IHtmlElement>();

        /// <summary>
        /// Initializes a new instance of the HtmlElement class with the specified tag name.
        /// </summary>
        /// <param name="tagName">The name of the HTML tag to associate with this element. Cannot be null or empty.</param>
        public HtmlElement(string tagName)
        {
            this.TagName = tagName;
        }

        /// <summary>
        /// Generates an HTML string representation of the current element, including its attributes, inner text, and
        /// child elements.
        /// </summary>
        /// <remarks>Child elements that implement the IHtmlRenderable interface are recursively rendered
        /// and included in the output. Attributes are rendered as key-value pairs within the opening tag. If InnerText
        /// is not null or empty, it is included between the opening and closing tags.</remarks>
        /// <returns>A string containing the HTML markup for the element and its children.</returns>
        public string Render()
        {
            var html = $"<{TagName}";
            foreach (var attr in Attributes)
            {
                html += $" {attr.Key}=\"{attr.Value}\"";
            }
            html += ">";
            if (!string.IsNullOrEmpty(InnerText))
            {
                html += InnerText;
            }
            foreach (var child in Children)
            {
                if (child is IHtmlRenderable renderable)
                {
                    html += renderable.Render();
                }
            }
            html += $"</{TagName}>";
            return html;
        }
    }
}
