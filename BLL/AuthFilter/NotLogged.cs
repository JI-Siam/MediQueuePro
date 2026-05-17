using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class NotLogged : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var userName = context.HttpContext.Session.GetString("Uname");

        if (!string.IsNullOrWhiteSpace(userName))
        {
            var userType = context.HttpContext.Session.GetInt32("UType");

            context.Result = userType switch
            {
                2 => new RedirectToActionResult("Dashboard", "Patient", null),
                3 => new RedirectToActionResult("Index", "Doctor", null),
                1 => new RedirectToActionResult("Index", "Admin", null),
                _ => new RedirectToActionResult("Index", "Home", null)
            };
        }
    }
}