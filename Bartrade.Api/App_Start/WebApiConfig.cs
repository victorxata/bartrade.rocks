using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using Microsoft.Practices.Unity;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Repositories.Interfaces;
using Repositories.MongoDb;
using Services;
using Services.Interfaces;
using Unity.WebApi;

namespace Api
{
    /// <summary>
    /// 
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            ConfigureUnity(config);
            EnableCamelCaseJson(config);
        }

        /// <summary>
        /// Enables indented camelCase for ease of reading.
        /// </summary>
        /// <param name="config">HttpConfiguration for this api</param>
        private static void EnableCamelCaseJson(HttpConfiguration config)
        {
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            var stringEnumConverter = new StringEnumConverter { CamelCaseText = true };
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(stringEnumConverter);
            config.Formatters.JsonFormatter.Indent = true;
        }

        /// <summary>
        /// Enables dependency injection
        /// </summary>
        /// <param name="config">HttpConfiguration for this api</param>
        private static void ConfigureUnity(HttpConfiguration config)
        {
            var container = new UnityContainer();
            config.DependencyResolver = new UnityDependencyResolver(container);
            // Miscelanea
            container.RegisterType<IGeoLocationService, GeoLocationService>(new TransientLifetimeManager());
            container.RegisterType<IEmailService, EmailService>(new TransientLifetimeManager());
            container.RegisterType<IEmailTemplatesRepository, EmailTemplatesRepository>(new TransientLifetimeManager());

            // Languages
            container.RegisterType<ILanguagesService, LanguagesService>(new TransientLifetimeManager());
            container.RegisterType<ICountriesRepository, CountriesRepository>(new TransientLifetimeManager());
            container.RegisterType<ICharacterSetsRepository, CharacterSetsRepository>(new TransientLifetimeManager());
            container.RegisterType<ILanguagesRepository, LanguagesRepository>(new TransientLifetimeManager());

            // Users
            container.RegisterType<IUsersService, UsersService>(new TransientLifetimeManager());
            container.RegisterType<IRecoverPasswordRepository, RecoverPasswordRepository>(new TransientLifetimeManager());
        }
    }
}
