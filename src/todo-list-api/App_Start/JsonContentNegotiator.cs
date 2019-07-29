using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace TodoListAPI
{
    internal class JsonContentNegotiator : IContentNegotiator
    {
        public JsonContentNegotiator(JsonMediaTypeFormatter jsonFormatter)
        {
            _jsonFormatter = jsonFormatter ?? throw new ArgumentNullException(nameof(jsonFormatter));
        }

        #region Implementation of IContentNegotiator

        public ContentNegotiationResult Negotiate(Type type, HttpRequestMessage request, IEnumerable<MediaTypeFormatter> formatters)
        {
            // checking if accept header is json or any media type is also ok to send back (e.g. */*)
            bool acceptHeaderIsJson =
                request.Headers.Accept.Any(mediaTypeWithQuality => mediaTypeWithQuality.MediaType == "application/json" 
                                                                   || mediaTypeWithQuality.MediaType == "*/*"
                                                                   || mediaTypeWithQuality.MediaType == "*/json"
                                                                   || mediaTypeWithQuality.MediaType == "application/*"
                                          );

            return acceptHeaderIsJson 
                       ? new ContentNegotiationResult(_jsonFormatter, new MediaTypeHeaderValue("application/json")) 
                       : null; // null generates http 406 Not Acceptable
        }

        #endregion

        private readonly JsonMediaTypeFormatter _jsonFormatter;
    }
}
