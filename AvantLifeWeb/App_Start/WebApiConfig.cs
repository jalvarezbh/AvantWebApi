using System.Web.Http;
using System.Web.Http.Cors;

namespace AvantLifeWeb
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            EnableCorsAttribute cors = new EnableCorsAttribute("*", "*", "*");
            // Web API configuration and services
            config.EnableCors(cors);
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new {action = "Index", id = RouteParameter.Optional }
            );
        }
    }
}
