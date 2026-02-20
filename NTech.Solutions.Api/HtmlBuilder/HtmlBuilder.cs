namespace NTech.Solutions.Api.HtmlBuilder
{
    /// <summary>
    /// Provides a builder for constructing and rendering HTML documents with a title and a collection of body elements.
    /// </summary>
    /// <remarks>Use the HtmlBuilder class to assemble an HTML document by specifying a title and adding body
    /// elements that implement the IHtmlElement interface. The rendered output includes the specified title and the
    /// HTML representation of each body element. This class is useful for programmatically generating simple HTML
    /// pages.</remarks>
    public class HtmlBuilder
    {
        /// <summary>
        /// Gets or sets the title associated with the object.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the collection of HTML elements that make up the body content.
        /// </summary>
        /// <remarks>The order of elements in the collection determines their rendering order in the body.
        /// Modifying this collection updates the structure of the body content.</remarks>
        public List<IHtmlElement> Body { get; set; } = new List<IHtmlElement>();

        /// <summary>
        /// Adds the specified HTML element to the body of the document.
        /// </summary>
        /// <param name="element">The HTML element to add to the document body. Cannot be null.</param>
        public void AddBodyElement(IHtmlElement element)
        {
            Body.Add(element);
        }

        /// <summary>
        /// Generates an HTML document as a string representation of the current object, including the title and all
        /// body elements.
        /// </summary>
        /// <remarks>Only elements in the body that implement the IHtmlRenderable interface are rendered
        /// and included in the output. The title is included in the head section if it is not null or empty.</remarks>
        /// <returns>A string containing the complete HTML markup for the document. The string includes the document type
        /// declaration, head, and body sections.</returns>
        public string Render()
        {
            var html = "<!DOCTYPE html><html><head>";
            if (!string.IsNullOrEmpty(Title))
            {
                html += $"<title>{Title}</title>";
            }
            html += "</head><body>";
            foreach (var element in Body)
            {
                if (element is IHtmlRenderable renderable)
                {
                    html += renderable.Render();
                }
            }
            html += "</body></html>";
            return html;
        }
    }
}
