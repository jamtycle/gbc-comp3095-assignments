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
            // if (_context.Request.Cookies.ContainsKey("session"))
            // {
            //     string? session = _context.Request.Cookies["session"];
            //     if (!string.IsNullOrEmpty(session) && !string.IsNullOrWhiteSpace(session))
            //     {
            //         // TODO: Check the session with the databases
                    
            //     }
            // }

            await request_delegate(_context);
        }

        private async Task MiddlewareResponse(HttpContext _context, int _status_code, string _response)
        {
            _context.Response.StatusCode = _status_code;
            await _context.Response.WriteAsync(_response);
        }
    }
}