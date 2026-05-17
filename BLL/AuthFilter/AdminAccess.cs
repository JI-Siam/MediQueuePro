using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class AdminAccess : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var userName = context.HttpContext.Session.GetString("Uname");
        var userType = context.HttpContext.Session.GetInt32("UType");

        if (userName == null || userType != 1)
        {
            context.Result = new RedirectToActionResult("Index", "Home", null);
        }
    }
}
