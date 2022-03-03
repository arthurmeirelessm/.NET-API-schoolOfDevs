using SchoolOfDevs.Services;

namespace SchoolOfDevs.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public  async Task Invoke(HttpContext context, IUserService userService, IJwtService jwtService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = jwtService.ValidateToken(token);
            if (userId != null)
            {
                context.Items["User"] = await userService.GetById(userId.Value);
            }
            await _next(context);
        }
    }
}
