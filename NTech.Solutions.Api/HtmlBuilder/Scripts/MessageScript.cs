namespace NTech.Solutions.Api.HtmlBuilder.Scripts
{
    /// <summary>
    /// Represents an HTML script element that posts a message to the opener window and closes the current window. Used
    /// to facilitate communication from a popup window back to its parent using a specified message type and payload.
    /// </summary>
    /// <remarks>Use MessageScript when you need to send structured data from a popup window to its opener and
    /// ensure the popup is closed after the message is sent. The message content is serialized as JSON and posted using
    /// the postMessage API. If the window does not have an opener, only the close operation is performed. This class
    /// implements IHtmlElement, IHtmlRenderable, and IScriptElement to support integration with HTML rendering
    /// frameworks.</remarks>
    public class MessageScript : IHtmlElement, IHtmlRenderable, IScriptElement
    {
        private readonly object _message;
        private readonly string _messageType;

        /// <summary>
        /// Initializes a new instance of the MessageScript class with the specified message type and message content.
        /// </summary>
        /// <param name="type">The type of the message to associate with this script. Cannot be null or empty.</param>
        /// <param name="message">The content or payload of the message. Can be any object representing the message data.</param>
        public MessageScript(string type, object message)
        {
            _messageType = type;
            _message = message;
        }

        /// <summary>
        /// Generates an HTML script that posts a serialized message to the opener window and closes the current window.
        /// </summary>
        /// <remarks>Use this method when you need to send a message from a popup window back to its
        /// opener and close the popup. The message is serialized as JSON and sent with a specified message type. If the
        /// window does not have an opener, the script will simply close the window without posting a message.</remarks>
        /// <returns>A string containing an HTML script element. The script posts the message to the opener window using the
        /// postMessage API and then closes the window.</returns>
        public string Render()
        {
            var payload = System.Text.Json.JsonSerializer.Serialize(_message);
            return $"<script>if (window.opener) {{ window.opener.postMessage({{type:'{_messageType}', payload: '{payload}'}}, '*'); }} window.close();</script>";
        }
    }
}
