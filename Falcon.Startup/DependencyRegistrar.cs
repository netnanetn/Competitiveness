using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Falcon.Caching;
using Falcon.Configuration;
using Falcon.Environment;
using Falcon.Infrastructure;
using Falcon.Infrastructure.DependencyManagement;
//using Falcon.Plugins;
//using Falcon.EmbeddedViews;
using Falcon.Mvc.Routes;
using Falcon.Themes;
using Falcon.Services;
using Falcon.Data;
using Falcon.Mvc;
using Falcon.Data.Repository;
using Falcon.Services.Users;
using Falcon.Services.Security;
using Falcon.Security.Providers;
using Falcon.Services.Themes;
using Falcon.Security;
using Falcon.Services.Thumbnails;
using Falcon.Services.SystemSettings;
//using Falcon.Framework.Fakes;
using System.Configuration;
using Falcon.Services.Pages;
using Falcon.Services.Supports;
using Falcon.Data.Repository.Interface;

namespace Falcon.Startup
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            string cacheProvider = ConfigurationManager.AppSettings["CacheProvider"];

            if (string.IsNullOrEmpty(cacheProvider) || !Enum.IsDefined(typeof(CacheTypeEnum), cacheProvider))
            {
                cacheProvider = CacheTypeEnum.Memory.ToString();
            }

            builder.RegisterControllers(typeFinder.GetAssemblies().ToArray());

            //hosting environment register
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
            builder.RegisterType<DefaultHostEnvironment>().As<IHostEnvironment>().SingleInstance();

            //register logging autofac module
            //builder.RegisterModule(new LoggingModule());

            //Register Data Provider
            //var dataSettingsManager = new DataSettingsManager();
            //var dataProviderSettings = dataSettingsManager.LoadSettings();
            //builder.Register(c => dataSettingsManager.LoadSettings()).As<DataSettings>();

            //switch (dataProviderSettings.DataProvider.ToLower())
            //{
            //    case "linqtosql.sqlserver":
            //        //Register all Linq relation classes
            //        LinqToSqlRegistra.RegisterAll(builder, dataProviderSettings.DataConnectionString);
            //        break;
            //    case "entityframework.sqlserver":

            //        break;
            //    default:
            //        //use linq for default
            //        LinqToSqlRegistra.RegisterAll(builder, dataProviderSettings.DataConnectionString);
            //        break;
            //}

            builder.Register(x => new DatabaseFactory()).As<IDatabaseFactory>().InstancePerHttpRequest();

            #region Register Security Repositories
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerHttpRequest();
            builder.RegisterType<UserRoleRepository>().As<IUserRoleRepository>().InstancePerHttpRequest();
            builder.RegisterType<RoleRepository>().As<IRoleRepository>().InstancePerHttpRequest();
            builder.RegisterType<ResourceRepository>().As<IResourceRepository>().InstancePerHttpRequest();
            builder.RegisterType<PermissionRepository>().As<IPermissionRepository>().InstancePerHttpRequest();
            #endregion

            #region Register Theme Repositories
            builder.RegisterType<ThemeRepository>().As<IThemeRepository>().InstancePerHttpRequest();
            //builder.RegisterType<StoreThemeRepository>().As<IStoreThemeRepository>().InstancePerHttpRequest();
            #endregion

            #region Other Repositories
            builder.RegisterType<ThumbnailSettingRepository>().As<IThumbnailSettingRepository>().InstancePerHttpRequest();
            builder.RegisterType<SystemSettingRepository>().As<ISystemSettingRepository>().InstancePerHttpRequest();
            builder.RegisterType<PageRepository>().As<IPageRepository>().InstancePerHttpRequest();
            builder.RegisterType<ArticleRepository>().As<IArticleRepository>().InstancePerHttpRequest();
            builder.RegisterType<CategoryRepository>().As<ICategoryRepository>().InstancePerHttpRequest();
            builder.RegisterType<CategoryTypeRepository>().As<ICategoryTypeRepository>().InstancePerHttpRequest();
            builder.RegisterType<SieuwebArticleRepository>().As<ISieuwebArticleRepository>().InstancePerHttpRequest();
            builder.RegisterType<SieuWebCategoryRepository>().As<ISieuWebCategoryRepository>().InstancePerHttpRequest();

            #endregion

            //register services
            #region Register Services
            builder.RegisterType<UserService>().As<IUserService>().InstancePerHttpRequest();
            builder.RegisterType<AuthorizationService>().As<IAuthorizationService>().InstancePerHttpRequest();
            builder.RegisterType<FormsAuthenticationService>().As<IAuthenticationService>().InstancePerHttpRequest();
            builder.RegisterType<DefaultEncryptionService>().As<IEncryptionService>().SingleInstance();
            builder.RegisterType<ThumbnailSettingService>().As<IThumbnailSettingService>().InstancePerHttpRequest();
            builder.RegisterType<SystemSettingService>().As<ISystemSettingService>().InstancePerHttpRequest();
            builder.RegisterType<PageService>().As<IPageService>().InstancePerHttpRequest();
            builder.RegisterType<ArticleService>().As<IArticleService>().InstancePerHttpRequest();
            builder.RegisterType<CategoryService>().As<ICategoryService>().InstancePerHttpRequest();
            builder.RegisterType<CategoryTypeService>().As<ICategoryTypeService>().InstancePerHttpRequest();
            #endregion


            builder.RegisterType<ThemeService>().As<IThemeService>().InstancePerHttpRequest();

            //register FakeHttpContext when HttpContext is not available
            //builder.Register(c => HttpContext.Current != null ?
            //    (new HttpContextWrapper(HttpContext.Current) as HttpContextBase) :
            //    (new FakeHttpContext("~/") as HttpContextBase))
            //    .As<HttpContextBase>()
            //    .InstancePerHttpRequest();
            //builder.Register(c => c.Resolve<HttpContextBase>().Request)
            //    .As<HttpRequestBase>()
            //    .InstancePerHttpRequest();
            //builder.Register(c => c.Resolve<HttpContextBase>().Response)
            //    .As<HttpResponseBase>()
            //    .InstancePerHttpRequest();
            //builder.Register(c => c.Resolve<HttpContextBase>().Server)
            //    .As<HttpServerUtilityBase>()
            //    .InstancePerHttpRequest();
            //builder.Register(c => c.Resolve<HttpContextBase>().Session)
            //    .As<HttpSessionStateBase>()
            //    .InstancePerHttpRequest();


            //plugins
            //builder.RegisterType<PluginFinder>().As<IPluginFinder>().InstancePerHttpRequest();

            ////cache manager
            builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().Named<ICacheManager>(CacheTypeEnum.Memory.ToString()).InstancePerHttpRequest();
            builder.RegisterType<PerRequestCacheManager>().As<ICacheManager>().Named<ICacheManager>(CacheTypeEnum.PerRequest.ToString()).InstancePerHttpRequest();
            builder.RegisterType<RedisCacheManager>().As<ICacheManager>().Named<ICacheManager>(CacheTypeEnum.Redis.ToString()).InstancePerHttpRequest();

            //work context
            builder.RegisterType<WorkContext>().As<IWorkContext>().InstancePerHttpRequest();

            //builder.RegisterSource(new SettingsSource());

            builder.RegisterType<ThemeProvider>().As<IThemeProvider>().SingleInstance();
            builder.RegisterType<ThemeContext>().As<IThemeContext>().InstancePerHttpRequest();


            //builder.RegisterType<EmbeddedViewResolver>().As<IEmbeddedViewResolver>().SingleInstance();
            builder.RegisterType<RoutePublisher>().As<IRoutePublisher>().SingleInstance();
        }

        public int Order
        {
            get { return Int32.MinValue; }
        }
    }


    //public class SettingsSource : IRegistrationSource
    //{
    //    static readonly MethodInfo BuildMethod = typeof(SettingsSource).GetMethod(
    //        "BuildRegistration",
    //        BindingFlags.Static | BindingFlags.NonPublic);

    //    public IEnumerable<IComponentRegistration> RegistrationsFor(
    //            Service service,
    //            Func<Service, IEnumerable<IComponentRegistration>> registrations)
    //    {
    //        var ts = service as TypedService;
    //        if (ts != null && typeof(ISettings).IsAssignableFrom(ts.ServiceType))
    //        {
    //            var buildMethod = BuildMethod.MakeGenericMethod(ts.ServiceType);
    //            yield return (IComponentRegistration)buildMethod.Invoke(null, null);
    //        }
    //    }

    //    static IComponentRegistration BuildRegistration<TSettings>() where TSettings : ISettings, new()
    //    {
    //        return RegistrationBuilder
    //            .ForDelegate((c, p) => c.Resolve<IConfigurationProvider<TSettings>>().Settings)
    //            .InstancePerHttpRequest()
    //            .CreateRegistration();
    //    }

    //    public bool IsAdapterForIndividualComponents { get { return false; } }
    //}

}
