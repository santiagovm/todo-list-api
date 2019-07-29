using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Serilog;

namespace TodoListAPI.Logging
{
    internal class OopsExceptionHandler : ExceptionHandler
    {
        #region Overrides of ExceptionHandler

        public override void Handle(ExceptionHandlerContext context)
        {
            Guid errorId = Guid.NewGuid();

            Log.Error(context.Exception, "unhandled exception. ErrorId: [{ErrorId}]", errorId);

            string content = $"Oops! Sorry! Something went wrong. Please contact support@foo.com so we can try to fix it. ErrorId: [{errorId}]";

            context.Result = new TextPlainErrorResult(content, context.ExceptionContext.Request);
        }

        #endregion
        
        private class TextPlainErrorResult : IHttpActionResult
        {
            public TextPlainErrorResult(string content, HttpRequestMessage request)
            {
                _content = content ?? throw new ArgumentNullException(nameof(content));
                _request = request ?? throw new ArgumentNullException(nameof(request));
            }

            #region Implementation of IHttpActionResult

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                               {
                                   Content = new StringContent(_content),
                                   RequestMessage = _request
                               };

                return Task.FromResult(response);
            }

            #endregion

            private readonly string _content;
            private readonly HttpRequestMessage _request;
        }
    }
}
