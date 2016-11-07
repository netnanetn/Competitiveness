using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Falcon.Data.Domain;
using Falcon.Mvc.Controllers;
using System.Reflection;
using System.Web.Mvc;
using System.IO;
using System.Xml;
using Falcon.Infrastructure;

namespace Falcon.Security
{
    public class ModuleProvider
    {
        private string moduleBasePath;
        private string coreModuleBasePath;
        private List<ModuleConfiguration> moduleConfigurations = new List<ModuleConfiguration>();

        private readonly IWebHelper webHelper;

        public ModuleProvider(LoadModuleType loadType = LoadModuleType.FromAssembly)
        {
            if (loadType == LoadModuleType.FromConfigurationFile)
            {
                this.webHelper = EngineContext.Current.Resolve<IWebHelper>();
                this.moduleBasePath = this.webHelper.MapPath("~/Modules/");
                this.coreModuleBasePath = this.webHelper.MapPath("~/CoreModules/");
                this.LoadConfigurations();
            }
            else
            {
                this.LoadConfigurationFromAssemblies();
            }            
        }

        public List<ModuleConfiguration> ModuleConfigurations
        {
            get
            {
                return moduleConfigurations;
            }
        }
        #region Load from Configuration File
        private void LoadConfigurations()
        {
            foreach (string moduleName in Directory.GetDirectories(moduleBasePath))
            {
                var configuration = CreateModuleConfiguration(moduleName, "~/Modules/");
                if (configuration != null)
                {
                    moduleConfigurations.Add(configuration);
                }
            }

            foreach (string moduleName in Directory.GetDirectories(coreModuleBasePath))
            {
                var configuration = CreateModuleConfiguration(moduleName, "~/CoreModules/");
                if (configuration != null)
                {
                    moduleConfigurations.Add(configuration);
                }
            }
        }

        private ModuleConfiguration CreateModuleConfiguration(string modulePath, string basePath)
        {
            var moduleDirectory = new DirectoryInfo(modulePath);
            var themeConfigFile = new FileInfo(Path.Combine(moduleDirectory.FullName, "Module.config"));

            if (themeConfigFile.Exists)
            {
                var doc = new XmlDocument();
                doc.Load(themeConfigFile.FullName);
                return new ModuleConfiguration(moduleDirectory.Name, moduleDirectory.FullName, basePath + moduleDirectory.Name, doc);
            }

            return null;
        }
        #endregion

        #region Load From Assembly
        public void LoadConfigurationFromAssemblies()
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.FullName.StartsWith("Falcon."))
                {
                    ModuleConfiguration module = new ModuleConfiguration();
                    
                    IEnumerable<Type> controllers = assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(AdminBaseController))).ToList();

                    if (controllers.Count() > 0)
                    {
                        module.Name = GetModuleName(assembly.GetName().Name);
                        foreach (Type type in controllers)
                        {
                            ControllerInfo controller = new ControllerInfo();
                            controller.Name = GetControllerName(type.Name);
                            controller.Actions = GetActions(type);
                            module.Controllers.Add(controller);
                        }
                        ModuleConfigurations.Add(module);                        
                    }
                    
                }
            }
        }

        private List<string> GetActions(Type type)
        {
            List<string> result = new List<string>();
            IEnumerable<MethodInfo> methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                    .Where(action => action.ReturnType == typeof(ActionResult));
            foreach (MethodInfo method in methods)
            {
                if (result.IndexOf(method.Name) < 0)
                {
                    result.Add(method.Name);
                }
            }
            return result;
        }

        private string GetModuleName(string strNamespace)
        {
            //Falcon.Modules.Abc.Controllers -> Abc
            return strNamespace.Split('.')[2];
        }

        private string GetControllerName(string controller)
        {
            //AdminController => Admin
            return controller.Substring(0, controller.LastIndexOf("Controller"));
        } 
        #endregion        
    }

    public enum LoadModuleType
    {
        FromConfigurationFile,
        FromAssembly
    }
}
