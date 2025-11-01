using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Mega.WhatsAppApi.Api.Middlewares
{
    /// <summary>
    /// For now this class is here just to enable Request buffering, so the SessionFilter class can read the body and log it.
    /// </summary>
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="next"></param>
        /// <param name="loggerFactory"></param>
        public LoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
        }

        /// <summary>
        /// Invokes.
        /// </summary>
        /// <param name="context">The http context.</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            context.Request.EnableBuffering();
            await _next(context);
        }

        private async Task LogRequest(HttpContext context)
        {
            await Task.Run(() => { });
            // context.Request.EnableBuffering();

            // await using var requestStream = _recyclableMemoryStreamManager.GetStream();
            // await context.Request.Body.CopyToAsync(requestStream);
            
            // _logger.LogInformation($"Http Request Information:{Environment.NewLine}" +
            //                     $"Schema:{context.Request.Scheme} " +
            //                     $"Host: {context.Request.Host} " +
            //                     $"Path: {context.Request.Path} " +
            //                     $"QueryString: {context.Request.QueryString} " +
            //                     $"Request Body: {ReadStreamInChunks(requestStream)}");
            // context.Request.Body.Position = 0;
        }

        private async Task LogResponse(HttpContext context)
        {
            await Task.Run(() => { });
            // var originalBodyStream = context.Response.Body;
            // await using var responseBody = _recyclableMemoryStreamManager.GetStream();
            // context.Response.Body = responseBody;
            // await _next(context);
            // context.Response.Body.Seek(0, SeekOrigin.Begin);
            // var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
            // context.Response.Body.Seek(0, SeekOrigin.Begin);
            // _logger.LogInformation($"Http Response Information:{Environment.NewLine}" +
            //                     $"Schema:{context.Request.Scheme} " +
            //                     $"Host: {context.Request.Host} " +
            //                     $"Path: {context.Request.Path} " +
            //                     $"QueryString: {context.Request.QueryString} " +
            //                     $"Response Body: {text}");
            // await responseBody.CopyToAsync(originalBodyStream);
        }

        private static string ReadStreamInChunks(Stream stream)
        {
            const int readChunkBufferLength = 4096;
            stream.Seek(0, SeekOrigin.Begin);

            using var textWriter = new StringWriter();
            using var reader = new StreamReader(stream);

            var readChunk = new char[readChunkBufferLength];
            int readChunkLength;
            do
            {
                readChunkLength = reader.ReadBlock(readChunk, 0, readChunkBufferLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            } while (readChunkLength > 0);

            return textWriter.ToString();
        }
    }
}