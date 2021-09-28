using System;
using System.Linq;
using System.Web;
using System.Security.Claims;
using System.Threading;
using System.Web.Mvc;

namespace Euphoria.Web.Models
{
    public class CustomAuthorize : AuthorizeAttribute
    {
        readonly ApplicationDbContext context = new ApplicationDbContext(); // my entity  
        private readonly string[] allowedroles;
        public CustomAuthorize(params string[] roles)
        {
            this.allowedroles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            foreach (var role in allowedroles)
            {
                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                // Get the claims values
                try
                {
                    string userName = identity.Claims.Where(c => c.Type == ClaimTypes.Name)
                                                      .Select(c => c.Value).SingleOrDefault();
                    var loginUser = context.UserMasters.Where(x => x.UserName == userName).SingleOrDefault();
                    int roleId = context.Roless.SingleOrDefault(x => x.Name == role).Id;
                    var user = context.UserMasters.Where(m => m.roleId == roleId && m.UserName == userName).ToList();
                    if (user.Count() > 0)
                    {
                        authorize = true; /* return true if Entity has current user(active) with specific role */
                    }

                }
                catch (Exception e)
                {
                    return false;
                }

            }
            return authorize;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/Shared/Error.cshtml"
            };
        }


    }
}