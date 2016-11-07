using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Falcon.Infrastructure;
using Common.Logging;

namespace Falcon.Mvc
{
    public class FalconDependencyResolver : IDependencyResolver
    {
        private ILog log = LogManager.GetCurrentClassLogger();

        public FalconDependencyResolver()
        {            
        }
        
        public object GetService(Type serviceType)
        {
            try
            {
                return EngineContext.Current.ResolveOptional(serviceType);
            }
            catch (Exception e)
            {                
                log.Error(e);
                return null;
            }            
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            var type = typeof(IEnumerable<>).MakeGenericType(serviceType);
            return (IEnumerable<object>)EngineContext.Current.ResolveOptional(type);

            //try
            //{
            //    var type = typeof(IEnumerable<>).MakeGenericType(serviceType);
            //    return (IEnumerable<object>)EngineContext.Current.Resolve(type);
            //}
            //catch (Exception e)
            //{
            //    log.Error(e);
            //    return null;
            //}

        }
    }
}
