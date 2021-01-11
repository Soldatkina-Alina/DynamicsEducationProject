using Common.Interfaces;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class PluginProvider : IPluginProvider
    {
        IServiceProvider serviceProvider;
        bool isServiseFromUserName;
        Entity target;
        IOrganizationService service;

        Entity image;
        Entity IPluginProvider.Target { 
            get {
                return target;
            }
        }
        ITracingService IPluginProvider.TraceService  {
            get { return (ITracingService)serviceProvider.GetService(typeof(ITracingService)); }
        }

        IOrganizationService IPluginProvider.Service {
            get {
                return service;
            }
        }

        Entity Image { get { return image; } }

        public PluginProvider(IServiceProvider serviceProvider, bool isServiseFromUserName = true)
        {
            this.serviceProvider = serviceProvider;
            this.isServiseFromUserName = isServiseFromUserName;

            var pluginContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            target = (Entity)pluginContext.InputParameters["Target"];

            var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            if (isServiseFromUserName)
                service = serviceFactory.CreateOrganizationService(Guid.Empty);
            else service = serviceFactory.CreateOrganizationService(null);

            image = pluginContext.PreEntityImages["Image"];

        }

    }
}
