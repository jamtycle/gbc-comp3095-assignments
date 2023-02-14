using assignment1.Data;
using assignment1.Models;

namespace assignment1.Middleware
{
    public class SessionMiddleware
    {
        private readonly RequestDelegate request_delegate;

        public SessionMiddleware(RequestDelegate _request_delegate)
        {
            this.request_delegate = _request_delegate;
        }

        public async Task InvokeAsync(HttpContext _context)
        {
            // TODO: The idea here was to check if the cookie of a session exists before it hits a Controller,
            //          and once identified that return a view based on a certains rules (mostly return the 
            //          home if trying to access register/login processes after a session is created).
            // Maybe middleware is not a good solution 🤔
            // if (_context.Request.Cookies.ContainsKey("user_session"))
            // {
            //     string? session = _context.Request.Cookies["user_session"];
            //     if (!string.IsNullOrEmpty(session) && !string.IsNullOrWhiteSpace(session))
            //     {
            //         var info = new DBConnector().RecoverSession(session);
            //         if (info is string @message)
            //         {
            //             _context.Request.Headers.Add("Location", "Index");
            //         }
            //         else if (info is LoginModel @login)
            //         {
                        
            //         }
            //         else throw new Exception("Something got REALLY WRONG!");
            //     }
            // }

            await request_delegate(_context);
        }

        // private async Task MiddlewareResponse(HttpContext _context, int _status_code, string _response)
        // {
        //     _context.Response.StatusCode = _status_code;
        //     await _context.Response.WriteAsync(_response);
        // }
    }
}