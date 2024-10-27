using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Net;
using System.Security.Claims;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class JwtRoleAuthorizationAttribute : Attribute, IActionFilter
{
    private readonly string _requiredRole;

    // استقبال الدور المطلوب عند تعيين الفلتر على الكونترولر أو الأكشن
    public JwtRoleAuthorizationAttribute(string requiredRole)
    {
        _requiredRole = requiredRole;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var user = context.HttpContext.User;

        // التأكد من أن المستخدم مصادق عليه
        if (user.Identity == null || !user.Identity.IsAuthenticated)
        {
            context.Result = new JsonResult(new { message = "غير مسموح لك، يجب أن تكون مسجلاً." })
            {
                StatusCode = (int)HttpStatusCode.Unauthorized
            };
            return;
        }

        // التحقق إذا كان المستخدم يمتلك الدور المطلوب من Claims
        var hasRequiredRole = user.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == _requiredRole);

        if (!hasRequiredRole)
        {
            context.Result = new JsonResult(new { message = $"غير مسموح لك، يجب أن تكون لديك صلاحية {_requiredRole} للوصول إلى هذا الكمبيوتر." })
            {
                StatusCode = (int)HttpStatusCode.Forbidden
            };
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // لا حاجة إلى تنفيذ أي شيء بعد انتهاء الـ Action
    }
}
