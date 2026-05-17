using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class Logged : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var userName = context.HttpContext.Session.GetString("Uname");

        if (userName == null)
        {
            context.Result = new RedirectToActionResult("Login", "Patient", null);
        }
    }
}
