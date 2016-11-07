using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Falcon.Data.Domain;
using Falcon.Mvc.Controllers;
using System.Reflection;
using System.Web.Mvc;

namespace Falcon.Security
{
    public class PermissionExplorer
    {
        /// <summary>
        /// Sử dụng reflection lấy tất cả các class kế thừa từ AdminBaseController
        /// </summary>
        /// <returns></returns>
        public List<PermissionInfo> GetAllControllers()
        {
            List<PermissionInfo> result = new List<PermissionInfo>();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.FullName.StartsWith("Falcon."))
                {
                    IEnumerable<Type> controllers = assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(AdminBaseController))).ToList();

                    foreach (Type type in controllers)
                    {
                        PermissionInfo permission = new PermissionInfo()
                        {
                            Module = GetModuleName(type.Namespace),
                            Controller = GetControllerName(type.Name),
                            Actions = GetActions(type)
                        };
                        result.Add(permission);
                    }
                }                
            }
            
            return result;
        }

        private List<string> GetActions(Type type)
        {            
            List<string> result = new List<string>();
            IEnumerable<MethodInfo> methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                    .Where(action => action.ReturnType.IsAssignableFrom(typeof(ActionResult)) || action.ReturnType == typeof(string));
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
    }
}
