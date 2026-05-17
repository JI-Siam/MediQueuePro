using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class Logged : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var userName = context.HttpContext.Session.GetString("Uname");
        var userType = context.HttpContext.Session.GetString("UType");

        if (userName == null)
        {
            switch (userType)
            {
                case "2":
                    context.Result = new RedirectToActionResult("Login", "Patient", null);
                    break;
                case "3":
                    context.Result = new RedirectToActionResult("Login", "Doctor", null);
                    break;
                default:
                    context.Result = new RedirectToActionResult("Login", "Patient", null);
                    break;
            }
        }



    }
}
