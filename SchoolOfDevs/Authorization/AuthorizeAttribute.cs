using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SchoolOfDevs.Dto.User;
using SchoolOfDevs.Enuns;

namespace SchoolOfDevs.Authorization
{
    [AttributeUsage(AttributeTargets.Method |  AttributeTargets.Class)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly IList<TypeUser> _roles;

        public AuthorizeAttribute(params TypeUser[] roles)
        {
            _roles = roles ?? Array.Empty<TypeUser>();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
            {
                return;
            }
            var user = (UserResponse)context.HttpContext.Items["User"];
            if (user == null || (_roles.Any() && !_roles.Contains(user.TypeUser)))
            {
                context.Result = new JsonResult(new { message = "Unauthorized" })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }
        }
    }
}
