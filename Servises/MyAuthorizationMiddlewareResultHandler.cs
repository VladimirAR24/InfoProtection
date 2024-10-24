using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Authorization;

namespace InfoProtection.Servises
{
    public class MyAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler DefaultHandler = new AuthorizationMiddlewareResultHandler();

        public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
        {
            if (authorizeResult.Challenged)
            {
                // Перенаправляем на страницу регистрации
                context.Response.Redirect("/Login");
                return;
            }

            // Обрабатываем как обычно
            await DefaultHandler.HandleAsync(next, context, policy, authorizeResult);
        }
    }

}
