namespace NTech.Solutions.Api.HtmlBuilder
{
    /// <summary>
    /// Represents an HTML <script> element with configurable source and type attributes.
    /// </summary>
    /// <remarks>Use this class to generate script tags for including external JavaScript files in HTML
    /// output. The default script type is "text/javascript", but it can be changed by setting the Type
    /// property.</remarks>
    public class ScriptElement : IHtmlElement, IHtmlRenderable
    {
        /// <summary>
        /// Gets or sets the source URL or path for the content.
        /// </summary>
        public string Src { get; set; }

        /// <summary>
        /// Gets or sets the MIME type of the script content.
        /// </summary>
        public string Type { get; set; } = "text/javascript";

        /// <summary>
        /// Initializes a new instance of the ScriptElement class with the specified script source URL.
        /// </summary>
        /// <param name="src">The URL of the external script to associate with this element. Cannot be null or empty.</param>
        public ScriptElement(string src)
        {
            this.Src = src;
        }

        /// <summary>
        /// Generates the HTML markup for a script element using the current type and source values.
        /// </summary>
        /// <remarks>The returned string can be embedded directly into an HTML document to include the
        /// referenced script. Ensure that the Type and Src properties are set to valid values before calling this
        /// method.</remarks>
        /// <returns>A string containing the HTML <script> element with the specified type and source attributes.</returns>
        public string Render()
        {
            return $"<script type=\"{Type}\" src=\"{Src}\"></script>";
        }
    }
}
