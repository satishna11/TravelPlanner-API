// using System;
// using System.Text.Json;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Http;
//
// namespace ProjectTypeMiddlewares.Middleware
// {
//     public class ProjectTypeMiddleware
//     {
//         private readonly RequestDelegate _next;
//         private const string HeaderName = "X-Powered-By";
//         private const string HeaderValue = "Garry";
//
//         public ProjectTypeMiddleware(RequestDelegate next)
//         {
//             _next = next;
//         }
//
//         public async Task Invoke(HttpContext context)
//         {
//             if (!context.Request.Headers.TryGetValue(HeaderName, out var value) ||
//                 value != HeaderValue)
//             {
//                 context.Response.StatusCode = StatusCodes.Status403Forbidden;
//                 context.Response.ContentType = "application/json";
//
//                 await context.Response.WriteAsync(JsonSerializer.Serialize(new
//                 {
//                     success = false,
//                     message = "Invalid request header."
//                 }));
//
//                 return;
//             }
//
//             await _next(context);
//         }
//     }
// }