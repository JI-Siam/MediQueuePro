using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class DoctorAccess : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var userName = context.HttpContext.Session.GetString("Uname");
        var userType = context.HttpContext.Session.GetInt32("UType");

        if (userName == null || userType != 3)
        {
            context.Result = new RedirectToActionResult("Login", "Doctor", null);
        }
    }
}