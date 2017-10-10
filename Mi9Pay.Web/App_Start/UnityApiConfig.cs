using Mi9Pay.Service;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Unity.WebApi;

namespace Mi9Pay.Web
{
    public class UnityApiConfig
    {
        public static void RegisterComponents()
        {
            var container = BuildUnityContainer();
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            //container.RegisterType<IProductService, ProductService>()
            //         .RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());

            RegisterTypes(container);

            return container;
        }

        private static void RegisterTypes(IUnityContainer container)
        {
            container
                .RegisterType<IGatewayMgrService, GatewayMgrService>();
        }
    }
}