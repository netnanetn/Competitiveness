﻿using System;
using System.Collections.Generic;
using System.Linq;
using Falcon.Configuration;
using Elmah;
using Common.Logging;
using System.Diagnostics;

namespace Falcon.Infrastructure.DependencyManagement
{
    /// <summary>
    /// Configures the inversion of control container with services used by Nop.
    /// </summary>
    public class ContainerConfigurer
    {
        private ILog log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Known configuration keys used to configure services.
        /// </summary>
        public static class ConfigurationKeys
        {
            //TODO do we need it?
            /// <summary>Key used to configure services intended for medium trust.</summary>
            public const string MediumTrust = "MediumTrust";
            /// <summary>Key used to configure services intended for full trust.</summary>
            public const string FullTrust = "FullTrust";
        }

        public virtual void Configure(FalconEngine engine, ContainerManager containerManager, EventBroker broker, FalconConfig configuration)
        {
            //register dependencies provided by other asemblies
            containerManager.AddComponent<IWebHelper, WebHelper>("falcon.webHelper");
            containerManager.AddComponent<ITypeFinder, WebAppTypeFinder>("falcon.typeFinder");
            var typeFinder = containerManager.Resolve<ITypeFinder>();
            containerManager.UpdateContainer(x =>
            {
                var drTypes = typeFinder.FindClassesOfType<IDependencyRegistrar>();
                var drInstances = new List<IDependencyRegistrar>();
                foreach (var drType in drTypes)
                {
                    try
                    {
                        drInstances.Add((IDependencyRegistrar)Activator.CreateInstance(drType));
                    }
                    catch (Exception e)
                    {
                        Exception ex = new Exception("Error loading type " + drType.Name + " :" + e.ToString());
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        log.Error(ex);
                    }                    
                }
                    
                //sort
                drInstances = drInstances.AsQueryable().OrderBy(t => t.Order).ToList();
                foreach (var dependencyRegistrar in drInstances)
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();

                    dependencyRegistrar.Register(x, typeFinder);

                    stopwatch.Stop();

                    string clazz = dependencyRegistrar.GetType().FullName;
                    log.Info(string.Format("Registra {0} cost {1}", clazz, stopwatch.Elapsed));
                }
                    
            });

            //other dependencies
            containerManager.AddComponentInstance<FalconConfig>(configuration, "falcon.configuration");
            containerManager.AddComponentInstance<FalconEngine>(engine, "falcon.engine");
            containerManager.AddComponentInstance<ContainerConfigurer>(this, "falcon.containerConfigurer");

            //if (configuration.Components != null)
            //    RegisterConfiguredComponents(containerManager, configuration.Components);

            //event broker
            containerManager.AddComponentInstance(broker);

            //service registration
            containerManager.AddComponent<DependencyAttributeRegistrator>("falcon.serviceRegistrator");
            var registrator = containerManager.Resolve<DependencyAttributeRegistrator>();
            var services = registrator.FindServices();
            var configurations = GetComponentConfigurations(configuration);
            services = registrator.FilterServices(services, configurations);
            registrator.RegisterServices(services);
        }

        protected virtual string[] GetComponentConfigurations(FalconConfig configuration)
        {
            List<string> configurations = new List<string>();
            string trustConfiguration = (CommonHelper.GetTrustLevel() > System.Web.AspNetHostingPermissionLevel.Medium)
                ? ConfigurationKeys.FullTrust
                : ConfigurationKeys.MediumTrust;
            configurations.Add(trustConfiguration);
            return configurations.ToArray();
        }

        private void AddComponentInstance(FalconEngine engine, object instance)
        {
            engine.ContainerManager.AddComponentInstance(instance.GetType(), instance, instance.GetType().FullName);
        }

        //protected virtual void RegisterConfiguredComponents(ContainerManager container, NopConfig config)
        //{
        //    foreach (ComponentElement component in config.Components)
        //    {
        //        Type implementation = Type.GetType(component.Implementation);
        //        Type service = Type.GetType(component.Service);

        //        if (implementation == null)
        //            throw new ComponentRegistrationException(component.Implementation);

        //        if (service == null && !String.IsNullOrEmpty(component.Service))
        //            throw new ComponentRegistrationException(component.Service);

        //        if (service == null)
        //            service = implementation;

        //        string name = component.Key;
        //        if (string.IsNullOrEmpty(name))
        //            name = implementation.FullName;

        //        if (component.Parameters.Count == 0)
        //        {
        //            container.AddComponent(service, implementation, name);
        //        }
        //        else
        //        {
        //            container.AddComponentWithParameters(service, implementation,
        //                                                 component.Parameters.ToDictionary(), name);
        //        }
        //    }
        //}
    }
}
