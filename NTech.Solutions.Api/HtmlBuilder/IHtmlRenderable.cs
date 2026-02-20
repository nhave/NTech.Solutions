namespace NTech.Solutions.Api.HtmlBuilder
{
    /// <summary>
    /// Defines a contract for objects that can generate HTML or similar markup as a string representation.
    /// </summary>
    /// <remarks>Implementations of this interface should provide a meaningful rendered output suitable for
    /// display or further processing. The returned string may contain HTML, XML, or other markup formats, depending on
    /// the implementation. This interface is commonly used in scenarios where content needs to be dynamically generated
    /// and rendered in web applications or templating systems.</remarks>
    public interface IHtmlRenderable
    {
        /// <summary>
        /// Generates and returns a string representation of the rendered content.
        /// </summary>
        /// <returns>A string containing the rendered output. The returned value may be empty if there is no content to render.</returns>
        string Render();
    }
}
